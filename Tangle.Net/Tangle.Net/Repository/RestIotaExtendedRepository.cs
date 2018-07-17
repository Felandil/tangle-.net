namespace Tangle.Net.Repository
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using RestSharp;
  using RestSharp.Authenticators;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Repository.Client;
  using Tangle.Net.Repository.DataTransfer;
  using Tangle.Net.Repository.Responses;
  using Tangle.Net.Utils;

  /// <inheritdoc />
  public partial class RestIotaRepository : IIotaRepository
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RestIotaRepository"/> class.
    /// </summary>
    /// <param name="client">
    /// The client.
    /// </param>
    /// <param name="powService">
    /// The pow service.
    /// </param>
    /// <param name="username">
    /// The username.
    /// </param>
    /// <param name="password">
    /// The password.
    /// </param>
    public RestIotaRepository(
      IRestClient client,
      IPoWService powService = null,
      string username = null,
      string password = null)
    {
      if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
      {
        client.Authenticator = new HttpBasicAuthenticator(username, password);
      }

      this.Client = new RestIotaClient(client);
      this.PoWService = powService ?? new RestPoWService(this.Client);
      this.AddressGenerator = new AddressGenerator();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RestIotaRepository"/> class.
    /// </summary>
    /// <param name="client">
    /// The client.
    /// </param>
    /// <param name="powService">
    /// The pow service.
    /// </param>
    /// <param name="addressGenerator">
    /// The address Generator.
    /// </param>
    public RestIotaRepository(IIotaClient client, IPoWService powService, IAddressGenerator addressGenerator = null)
    {
      this.Client = client;
      this.PoWService = powService;
      this.AddressGenerator = addressGenerator ?? new AddressGenerator();
    }

    /// <summary>
    ///   Gets the client.
    /// </summary>
    private IIotaClient Client { get; }

    /// <summary>
    ///   Gets the po w service.
    /// </summary>
    private IPoWService PoWService { get; }

    /// <summary>
    /// Gets the address generator.
    /// </summary>
    private IAddressGenerator AddressGenerator { get; }

    /// <inheritdoc />
    public async Task<bool> IsPromotableAsync(Hash tailTransaction, int depth = 6)
    {
      const int MilestoneInterval = 2 * 60 * 1000;
      const int OneWayDelay = 1 * 60 * 1000;

      var consitencyInfo = await this.CheckConsistencyAsync(new List<Hash> { tailTransaction });
      var transaction = Transaction.FromTrytes((await this.GetTrytesAsync(new List<Hash> { tailTransaction })).First());

      var timestamp = Timestamp.UnixSecondsTimestamp;
      var isAboveMaxDepth = transaction.AttachmentTimestamp < timestamp
                            && timestamp - transaction.AttachmentTimestamp < (depth * MilestoneInterval) - OneWayDelay;

      return consitencyInfo.State && isAboveMaxDepth;
    }

    /// <inheritdoc />
    public async Task PromoteTransactionAsync(Hash tailTransaction, int depth = 8, int minWeightMagnitude = 14, int attempts = 10)
    {
      if (attempts <= 0)
      {
        return;
      }

      var isPromotable = await this.IsPromotableAsync(tailTransaction, depth);

      if (!isPromotable)
      {
        throw new ArgumentException("Transaction not promotable (anymore). Try to reattach!");
      }

      var promotionBundle = new Bundle();
      promotionBundle.AddTransfer(
        new Transfer
          {
            Address = new Address(Hash.Empty.Value),
            Tag = Tag.Empty,
            Message = TryteString.FromUtf8String("Tangle.Net Promotion"),
            ValueToTransfer = 0
          });

      promotionBundle.Finalize();
      promotionBundle.Sign();

      var approveeTransactions = await this.GetTransactionsToApproveAsync(depth, tailTransaction);

      var attachResultTrytes = await this.AttachToTangleAsync(
                                 approveeTransactions.BranchTransaction,
                                 approveeTransactions.TrunkTransaction,
                                 promotionBundle.Transactions,
                                 minWeightMagnitude);

      await this.BroadcastAndStoreTransactionsAsync(attachResultTrytes);
      await this.PromoteTransactionAsync(tailTransaction, depth, minWeightMagnitude, attempts - 1);
    }

    /// <inheritdoc />
    public void BroadcastAndStoreTransactions(List<TransactionTrytes> transactions)
    {
      this.BroadcastTransactions(transactions);
      this.StoreTransactions(transactions);
    }

    /// <inheritdoc />
    public async Task BroadcastAndStoreTransactionsAsync(List<TransactionTrytes> transactions)
    {
      await this.BroadcastTransactionsAsync(transactions);
      await this.StoreTransactionsAsync(transactions);
    }

    /// <inheritdoc />
    public FindUsedAddressesResponse FindUsedAddressesWithTransactions(Seed seed, int securityLevel, int start)
    {
      var usedAddresses = new List<Address>();
      var associatedTransactionHashes = new List<Hash>();

      var currentIndex = start;
      while (true)
      {
        var address = this.AddressGenerator.GetAddress(seed, securityLevel, currentIndex);
        var transactions = this.FindTransactionsByAddresses(new List<Address> { address });

        if (transactions.Hashes.Count > 0)
        {
          usedAddresses.Add(address);
          associatedTransactionHashes.AddRange(transactions.Hashes);
        }
        else
        {
          break;
        }

        currentIndex++;
      }

      return new FindUsedAddressesResponse
               {
                 AssociatedTransactionHashes = associatedTransactionHashes,
                 UsedAddresses = usedAddresses.OrderBy(a => a.KeyIndex).ToList()
               };
    }

    /// <inheritdoc />
    public async Task<FindUsedAddressesResponse> FindUsedAddressesWithTransactionsAsync(Seed seed, int securityLevel, int start)
    {
      var usedAddresses = new List<Address>();
      var associatedTransactionHashes = new List<Hash>();

      var currentIndex = start;
      while (true)
      {
        var address = this.AddressGenerator.GetAddress(seed, securityLevel, currentIndex);
        var transactions = await this.FindTransactionsByAddressesAsync(new List<Address> { address });

        if (transactions.Hashes.Count > 0)
        {
          usedAddresses.Add(address);
          associatedTransactionHashes.AddRange(transactions.Hashes);
        }
        else
        {
          break;
        }

        currentIndex++;
      }

      return new FindUsedAddressesResponse
               {
                 AssociatedTransactionHashes = associatedTransactionHashes,
                 UsedAddresses = usedAddresses.OrderBy(a => a.KeyIndex).ToList()
               };
    }

    /// <inheritdoc />
    public GetAccountDataResponse GetAccountData(
      Seed seed,
      bool includeInclusionStates,
      int securityLevel,
      int addressStartIndex,
      int addressStopIndex = 0)
    {
      var usedAddressesWithTransactions =
        this.FindUsedAddressesWithTransactions(seed, securityLevel, addressStartIndex);
      var usedAddresses = usedAddressesWithTransactions.UsedAddresses;
      var latestUnusedAddress = this.AddressGenerator.GetAddress(seed, securityLevel, usedAddresses.Any() ? usedAddresses.Last().KeyIndex + 1 : 0);
      var addressesWithBalance = new List<Address>();
      var associatedBundles = new List<Bundle>();

      if (usedAddressesWithTransactions.AssociatedTransactionHashes.Count > 0)
      {
        addressesWithBalance = this.GetBalances(usedAddresses).Addresses;
        associatedBundles = this.GetBundles(
          usedAddressesWithTransactions.AssociatedTransactionHashes,
          includeInclusionStates);
      }

      return new GetAccountDataResponse
               {
                 Balance = addressesWithBalance.Sum(a => a.Balance),
                 UsedAddresses = usedAddresses,
                 AssociatedBundles = associatedBundles,
                 LatestUnusedAddress = latestUnusedAddress
               };
    }

    /// <inheritdoc />
    public async Task<GetAccountDataResponse> GetAccountDataAsync(Seed seed, bool includeInclusionStates, int securityLevel, int addressStartIndex, int addressStopIndex = 0)
    {
      var usedAddressesWithTransactions =
        await this.FindUsedAddressesWithTransactionsAsync(seed, securityLevel, addressStartIndex);
      var usedAddresses = usedAddressesWithTransactions.UsedAddresses;
      var latestUnusedAddress = this.AddressGenerator.GetAddress(seed, securityLevel, usedAddresses.Any() ? usedAddresses.Last().KeyIndex + 1 : 0);
      var addressesWithBalance = new List<Address>();
      var associatedBundles = new List<Bundle>();

      if (usedAddressesWithTransactions.AssociatedTransactionHashes.Count > 0)
      {
        addressesWithBalance = (await this.GetBalancesAsync(usedAddresses)).Addresses;
        associatedBundles = await this.GetBundlesAsync(
          usedAddressesWithTransactions.AssociatedTransactionHashes,
          includeInclusionStates);
      }

      return new GetAccountDataResponse
               {
                 Balance = addressesWithBalance.Sum(a => a.Balance),
                 UsedAddresses = usedAddresses,
                 AssociatedBundles = associatedBundles,
                 LatestUnusedAddress = latestUnusedAddress
               };
    }

    /// <inheritdoc />
    public Bundle GetBundle(Hash transactionHash)
    {
      var bundle = new Bundle { Transactions = this.TraverseBundle(transactionHash) };

      var validationResult = bundle.Validate();

      if (!validationResult.IsValid)
      {
        throw new InvalidBundleException(
          "The bundle is not valid. See ValidationErrors for details.",
          validationResult.Errors);
      }

      return bundle;
    }

    /// <inheritdoc />
    public async Task<Bundle> GetBundleAsync(Hash transactionHash)
    {
      var bundle = new Bundle { Transactions = await this.TraverseBundleAsync(transactionHash) };

      var validationResult = bundle.Validate();

      if (!validationResult.IsValid)
      {
        throw new InvalidBundleException(
          "The bundle is not valid. See ValidationErrors for details.",
          validationResult.Errors);
      }

      return bundle;
    }

    /// <inheritdoc />
    public List<Bundle> GetBundles(List<Hash> transactionHashes, bool includeInclusionStates)
    {
      var associatedBundles = new List<Bundle>();
      var tailTransactions = new List<Hash>();
      var nonTailTransactions = new List<Transaction>();

      var transactionTrytes = this.GetTrytes(transactionHashes);

      foreach (var transaction in transactionTrytes.Select(transactionTryte => Transaction.FromTrytes(transactionTryte)))
      {
        if (transaction.IsTail)
        {
          tailTransactions.Add(transaction.Hash);
        }
        else
        {
          nonTailTransactions.Add(transaction);
        }
      }

      if (nonTailTransactions.Any())
      {
        var allBundleTransactions =
          this.FindTransactionsByBundles(nonTailTransactions.Select(t => t.BundleHash).Distinct().ToList()).Hashes;
        var allBundleTransactionTrytes = this.GetTrytes(allBundleTransactions);

        tailTransactions.AddRange(
          from bundleTransactionTryte in allBundleTransactionTrytes
          select Transaction.FromTrytes(bundleTransactionTryte)
          into transaction
          where transaction.IsTail
          select transaction.Hash);
      }

      var inclusionStates = new InclusionStates();
      if (includeInclusionStates)
      {
        inclusionStates = this.GetLatestInclusion(tailTransactions);
      }

      var bundles = this.GetBundles(tailTransactions);

      foreach (var bundle in bundles)
      {
        if (includeInclusionStates)
        {
          bundle.IsConfirmed = inclusionStates.States
            .FirstOrDefault(transactionHash => transactionHash.Key.Value == bundle.Transactions[0].Hash.Value).Value;
        }

        if (associatedBundles.All(b => b.Hash.Value != bundle.Hash.Value))
        {
          associatedBundles.Add(bundle);
        }
      }

      return associatedBundles;
    }

    /// <inheritdoc />
    public async Task<List<Bundle>> GetBundlesAsync(List<Hash> transactionHashes, bool includeInclusionStates)
    {
      var associatedBundles = new List<Bundle>();
      var tailTransactions = new List<Hash>();
      var nonTailTransactions = new List<Transaction>();

      var transactionTrytes = await this.GetTrytesAsync(transactionHashes);

      foreach (var transaction in transactionTrytes.Select(transactionTryte => Transaction.FromTrytes(transactionTryte)))
      {
        if (transaction.IsTail)
        {
          tailTransactions.Add(transaction.Hash);
        }
        else
        {
          nonTailTransactions.Add(transaction);
        }
      }

      if (nonTailTransactions.Any())
      {
        var allBundleTransactions =
          (await this.FindTransactionsByBundlesAsync(nonTailTransactions.Select(t => t.BundleHash).Distinct().ToList())).Hashes;
        var allBundleTransactionTrytes = await this.GetTrytesAsync(allBundleTransactions);

        tailTransactions.AddRange(
          from bundleTransactionTryte in allBundleTransactionTrytes
          select Transaction.FromTrytes(bundleTransactionTryte)
          into transaction
          where transaction.IsTail
          select transaction.Hash);
      }

      var inclusionStates = new InclusionStates();
      if (includeInclusionStates)
      {
        inclusionStates = await this.GetLatestInclusionAsync(tailTransactions);
      }

      var bundles = await this.GetBundlesAsync(tailTransactions);

      foreach (var bundle in bundles)
      {
        if (includeInclusionStates)
        {
          bundle.IsConfirmed = inclusionStates.States
            .FirstOrDefault(transactionHash => transactionHash.Key.Value == bundle.Transactions[0].Hash.Value).Value;
        }

        if (associatedBundles.All(b => b.Hash.Value != bundle.Hash.Value))
        {
          associatedBundles.Add(bundle);
        }
      }

      return associatedBundles;
    }

    /// <inheritdoc />
    public GetInputsResponse GetInputs(Seed seed, long threshold, int securityLevel, int startIndex, int stopIndex = 0)
    {
      if (startIndex > stopIndex)
      {
        throw new ArgumentException("Invalid bounds! StartIndex must not be lower than StopIndex.");
      }

      var resultAddresses = new List<Address>();

      var usedAddresses = stopIndex == 0
                            ? this.FindUsedAddressesWithTransactions(seed, securityLevel, startIndex).UsedAddresses
                            : this.AddressGenerator.GetAddresses(seed, securityLevel, 0, stopIndex - startIndex + 1);

      var usedAddressesWithBalance = this.GetBalances(usedAddresses);

      var currentBalance = 0L;
      foreach (var usedAddressWithBalance in usedAddressesWithBalance.Addresses)
      {
        if (usedAddressWithBalance.Balance > 0)
        {
          resultAddresses.Add(usedAddressWithBalance);
          currentBalance += usedAddressWithBalance.Balance;
        }

        if (currentBalance > threshold)
        {
          break;
        }
      }

      if (currentBalance < threshold)
      {
        throw new Exception("Accumulated balance" + currentBalance + "is lower than given threshold!");
      }

      return new GetInputsResponse { Addresses = resultAddresses, Balance = resultAddresses.Sum(a => a.Balance) };
    }

    /// <inheritdoc />
    public async Task<GetInputsResponse> GetInputsAsync(Seed seed, long threshold, int securityLevel, int startIndex, int stopIndex = 0)
    {
      if (startIndex > stopIndex)
      {
        throw new ArgumentException("Invalid bounds! StartIndex must not be lower than StopIndex.");
      }

      var resultAddresses = new List<Address>();

      var usedAddresses = stopIndex == 0
                            ? (await this.FindUsedAddressesWithTransactionsAsync(seed, securityLevel, startIndex)).UsedAddresses
                            : this.AddressGenerator.GetAddresses(seed, securityLevel, 0, stopIndex - startIndex + 1);

      var usedAddressesWithBalance = await this.GetBalancesAsync(usedAddresses);

      var currentBalance = 0L;
      foreach (var usedAddressWithBalance in usedAddressesWithBalance.Addresses)
      {
        if (usedAddressWithBalance.Balance > 0)
        {
          resultAddresses.Add(usedAddressWithBalance);
          currentBalance += usedAddressWithBalance.Balance;
        }

        if (currentBalance > threshold)
        {
          break;
        }
      }

      if (currentBalance < threshold)
      {
        throw new Exception("Accumulated balance" + currentBalance + "is lower than given threshold!");
      }

      return new GetInputsResponse { Addresses = resultAddresses, Balance = resultAddresses.Sum(a => a.Balance) };
    }


    /// <inheritdoc />
    public InclusionStates GetLatestInclusion(List<Hash> hashes)
    {
      var nodeInfo = this.GetNodeInfo();
      return this.GetInclusionStates(hashes, new List<Hash> { new Hash(nodeInfo.LatestSolidSubtangleMilestone) });
    }

    /// <inheritdoc />
    public async Task<InclusionStates> GetLatestInclusionAsync(List<Hash> hashes)
    {
      var nodeInfo = await this.GetNodeInfoAsync();
      return await this.GetInclusionStatesAsync(hashes, new List<Hash> { new Hash(nodeInfo.LatestSolidSubtangleMilestone) });
    }

    /// <inheritdoc />
    public List<Address> GetNewAddresses(Seed seed, int addressStartIndex, int count, int securityLevel)
    {
      var result = new List<Address>();

      var foundNewAddress = false;
      var foundAddressCount = 0;

      while (!foundNewAddress || foundAddressCount != count)
      {
        var address = this.AddressGenerator.GetAddress(seed, securityLevel, addressStartIndex);
        var transactionsOnAddress = this.FindTransactionsByAddresses(new List<Address> { address });

        addressStartIndex++;

        if (transactionsOnAddress.Hashes.Count != 0)
        {
          continue;
        }

        foundNewAddress = true;
        foundAddressCount++;

        result.Add(address);
      }

      return result;
    }

    /// <inheritdoc />
    public async Task<List<Address>> GetNewAddressesAsync(Seed seed, int addressStartIndex, int count, int securityLevel)
    {
      var result = new List<Address>();

      var foundNewAddress = false;
      var foundAddressCount = 0;

      while (!foundNewAddress || foundAddressCount != count)
      {
        var address = this.AddressGenerator.GetAddress(seed, securityLevel, addressStartIndex);
        var transactionsOnAddress = await this.FindTransactionsByAddressesAsync(new List<Address> { address });

        addressStartIndex++;

        if (transactionsOnAddress.Hashes.Count != 0)
        {
          continue;
        }

        foundNewAddress = true;
        foundAddressCount++;

        result.Add(address);
      }

      return result;
    }

    /// <inheritdoc />
    public List<Bundle> GetTransfers(
      Seed seed,
      int securityLevel,
      bool includeInclusionStates,
      int addressStartIndex,
      int addressStopIndex = 0)
    {
      var transactions = addressStopIndex == 0
                           ? this.FindUsedAddressesWithTransactions(seed, securityLevel, addressStartIndex).AssociatedTransactionHashes
                           : this.FindTransactionsByAddresses(
                             this.AddressGenerator.GetAddresses(seed, securityLevel, 0, addressStartIndex - addressStopIndex + 1)).Hashes;

      return this.GetBundles(transactions, includeInclusionStates);
    }

    /// <inheritdoc />
    public async Task<List<Bundle>> GetTransfersAsync(Seed seed, int securityLevel, bool includeInclusionStates, int addressStartIndex, int addressStopIndex = 0)
    {
      var transactions = addressStopIndex == 0
                           ? (await this.FindUsedAddressesWithTransactionsAsync(seed, securityLevel, addressStartIndex)).AssociatedTransactionHashes
                           : (await this.FindTransactionsByAddressesAsync(
                                this.AddressGenerator.GetAddresses(seed, securityLevel, 0, addressStartIndex - addressStopIndex + 1))).Hashes;

      return await this.GetBundlesAsync(transactions, includeInclusionStates);
    }

    /// <inheritdoc />
    public Bundle PrepareTransfer(
      Seed seed,
      Bundle bundle,
      int securityLevel,
      Address remainderAddress = null,
      List<Address> inputAddresses = null)
    {
      // user wants to spend IOTA, so we need to find input addresses (if not provided) with valid balances
      if (bundle.Balance > 0)
      {
        inputAddresses = inputAddresses == null
                           ? this.GetInputs(seed, bundle.Balance, securityLevel, 0).Addresses
                           : this.GetBalances(inputAddresses).Addresses;

        var availableAmount = inputAddresses.Sum(a => a.Balance);

        if (availableAmount < bundle.Balance)
        {
          throw new IotaApiException($"Insufficient balance! Found {availableAmount}. Need {bundle.Balance}");
        }

        bundle.AddInput(inputAddresses);
      }

      // the bundle balance currently spends less iota tokens than available. that means we have to send remaining funds to a remainder address in order to prevent key reuse
      if (bundle.Balance < 0)
      {
        if (remainderAddress == null)
        {
          remainderAddress = this.GetNewAddresses(seed, 0, 1, securityLevel)[0];
        }

        bundle.AddRemainder(remainderAddress);
      }

      bundle.Finalize();
      bundle.Sign();

      return bundle;
    }

    /// <inheritdoc />
    public async Task<Bundle> PrepareTransferAsync(Seed seed, Bundle bundle, int securityLevel, Address remainderAddress = null, List<Address> inputAddresses = null)
    {
      // user wants to spend IOTA, so we need to find input addresses (if not provided) with valid balances
      if (bundle.Balance > 0)
      {
        inputAddresses = inputAddresses == null
                           ? (await this.GetInputsAsync(seed, bundle.Balance, securityLevel, 0)).Addresses
                           : (await this.GetBalancesAsync(inputAddresses)).Addresses;

        var availableAmount = inputAddresses.Sum(a => a.Balance);

        if (availableAmount < bundle.Balance)
        {
          throw new IotaApiException($"Insufficient balance! Found {availableAmount}. Need {bundle.Balance}");
        }

        bundle.AddInput(inputAddresses);
      }

      // the bundle balance currently spends less iota tokens than available. that means we have to send remaining funds to a remainder address in order to prevent key reuse
      if (bundle.Balance < 0)
      {
        if (remainderAddress == null)
        {
          remainderAddress = (await this.GetNewAddressesAsync(seed, 0, 1, securityLevel))[0];
        }

        bundle.AddRemainder(remainderAddress);
      }

      bundle.Finalize();
      bundle.Sign();

      return bundle;
    }

    /// <inheritdoc />
    public List<TransactionTrytes> ReplayBundle(Hash transactionHash, int depth = 8, int minWeightMagnitude = 14)
    {
      var bundle = this.GetBundle(transactionHash);

      return this.SendTrytes(bundle.Transactions, depth, minWeightMagnitude);
    }

    /// <inheritdoc />
    public async Task<List<TransactionTrytes>> ReplayBundleAsync(Hash transactionHash, int depth = 8, int minWeightMagnitude = 14)
    {
      var bundle = await this.GetBundleAsync(transactionHash);

      return await this.SendTrytesAsync(bundle.Transactions, depth, minWeightMagnitude);
    }

    /// <inheritdoc />
    public Bundle SendTransfer(
      Seed seed,
      Bundle bundle,
      int securityLevel,
      int depth = 8,
      int minWeightMagnitude = 14,
      Address remainderAddress = null,
      List<Address> inputAddresses = null)
    {
      var preparedBundle = this.PrepareTransfer(seed, bundle, securityLevel, remainderAddress, inputAddresses);
      var resultTrytes = this.SendTrytes(preparedBundle.Transactions, depth, minWeightMagnitude);

      return Bundle.FromTransactionTrytes(resultTrytes);
    }

    /// <inheritdoc />
    public async Task<Bundle> SendTransferAsync(
      Seed seed,
      Bundle bundle,
      int securityLevel,
      int depth = 8,
      int minWeightMagnitude = 14,
      Address remainderAddress = null,
      List<Address> inputAddresses = null)
    {
      var preparedBundle = await this.PrepareTransferAsync(seed, bundle, securityLevel, remainderAddress, inputAddresses);
      var resultTrytes = await this.SendTrytesAsync(preparedBundle.Transactions, depth, minWeightMagnitude);

      return Bundle.FromTransactionTrytes(resultTrytes);
    }

    /// <inheritdoc />
    public List<TransactionTrytes> SendTrytes(
      IEnumerable<Transaction> transactions,
      int depth = 8,
      int minWeightMagnitude = 14)
    {
      var transactionsToApprove = this.GetTransactionsToApprove(depth);

      var attachResultTrytes = this.AttachToTangle(
        transactionsToApprove.BranchTransaction,
        transactionsToApprove.TrunkTransaction,
        transactions,
        minWeightMagnitude);

      this.BroadcastAndStoreTransactions(attachResultTrytes);

      return attachResultTrytes;
    }

    /// <inheritdoc />
    public async Task<List<TransactionTrytes>> SendTrytesAsync(IEnumerable<Transaction> transactions, int depth = 8, int minWeightMagnitude = 14)
    {
      var transactionsToApprove = await this.GetTransactionsToApproveAsync(depth);

      var attachResultTrytes = await this.AttachToTangleAsync(
                                 transactionsToApprove.BranchTransaction,
                                 transactionsToApprove.TrunkTransaction,
                                 transactions,
                                 minWeightMagnitude);

      await this.BroadcastAndStoreTransactionsAsync(attachResultTrytes);

      return attachResultTrytes;
    }

    /// <summary>
    /// The get bundles.
    /// </summary>
    /// <param name="tailTransactions">
    /// The tail transactions.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    private List<Bundle> GetBundles(List<Hash> tailTransactions)
    {
      var bundleHashList = new List<Hash>();
      for (var i = 0; i < tailTransactions.Count; i++)
      {
        bundleHashList.Add(Hash.Empty);
      }

      var transactions = this.TraverseBundles(tailTransactions, bundleHashList);

      var result = new List<Bundle>();

      foreach (var transaction in transactions)
      {
        var bundle = result.FirstOrDefault(b => b.Hash.Value == transaction.BundleHash.Value);
        if (bundle != null)
        {
          if (bundle.Transactions.All(t => t.CurrentIndex != transaction.CurrentIndex))
          {
            bundle.Transactions.Add(transaction);
          }
        }
        else
        {
          result.Add(new Bundle { Transactions = new List<Transaction> { transaction } });
        }
      }

      return result;
    }

    /// <summary>
    /// The get bundles async.
    /// </summary>
    /// <param name="tailTransactions">
    /// The tail transactions.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    private async Task<List<Bundle>> GetBundlesAsync(List<Hash> tailTransactions)
    {
      var bundleHashList = new List<Hash>();
      for (var i = 0; i < tailTransactions.Count; i++)
      {
        bundleHashList.Add(Hash.Empty);
      }

      var transactions = await this.TraverseBundlesAsync(tailTransactions, bundleHashList);

      var result = new List<Bundle>();

      foreach (var transaction in transactions)
      {
        var bundle = result.FirstOrDefault(b => b.Hash.Value == transaction.BundleHash.Value);
        if (bundle != null)
        {
          if (bundle.Transactions.All(t => t.CurrentIndex != transaction.CurrentIndex))
          {
            bundle.Transactions.Add(transaction);
          }
        }
        else
        {
          result.Add(new Bundle { Transactions = new List<Transaction> { transaction } });
        }
      }

      return result;
    }

    /// <summary>
    /// The traverse bundles.
    /// </summary>
    /// <param name="transactionHashes">
    /// The transaction hashes.
    /// </param>
    /// <param name="bundleHashes">
    /// The bundle hashes.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    private List<Transaction> TraverseBundles(List<Hash> transactionHashes, List<Hash> bundleHashes)
    {
      var transactionTrytes = this.GetTrytes(transactionHashes);
      var transactions = transactionTrytes.Select(t => Transaction.FromTrytes(t)).ToList();

      var result = new List<Transaction>();
      for (var i = 0; i < transactionHashes.Count; i++)
      {
        var transactionHash = transactionHashes[i];
        var transaction = transactions.FirstOrDefault(t => t.Hash.Value == transactionHash.Value);
        var bundleHash = transaction != null ? transaction.BundleHash : Hash.Empty;

        if (bundleHashes[i].Value != Hash.Empty.Value)
        {
          if (transaction == null || bundleHash.Value != transaction.BundleHash.Value)
          {
            continue;
          }
        }
        else
        {
          bundleHashes[i] = bundleHash;
        }

         result.Add(transaction);
      }

      var nextTransactions = result.Where(t => t.CurrentIndex != t.LastIndex).ToList();
      if (nextTransactions.Count > 0)
      {
        result.AddRange(this.TraverseBundles(nextTransactions.Select(t => t.TrunkTransaction).ToList(), nextTransactions.Select(t => t.BundleHash).ToList()));
      }

      return result;
    }

    /// <summary>
    /// The traverse bundles async.
    /// </summary>
    /// <param name="transactionHashes">
    /// The transaction hashes.
    /// </param>
    /// <param name="bundleHashes">
    /// The bundle hashes.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    private async Task<List<Transaction>> TraverseBundlesAsync(List<Hash> transactionHashes, List<Hash> bundleHashes)
    {
      var transactionTrytes = await this.GetTrytesAsync(transactionHashes);
      var transactions = transactionTrytes.Select(t => Transaction.FromTrytes(t)).ToList();

      var result = new List<Transaction>();
      for (var i = 0; i < transactionHashes.Count; i++)
      {
        var transactionHash = transactionHashes[i];
        var transaction = transactions.FirstOrDefault(t => t.Hash.Value == transactionHash.Value);
        var bundleHash = transaction != null ? transaction.BundleHash : Hash.Empty;

        if (bundleHashes[i].Value != Hash.Empty.Value)
        {
          if (transaction == null || bundleHash.Value != transaction.BundleHash.Value)
          {
            continue;
          }
        }
        else
        {
          bundleHashes[i] = bundleHash;
        }

        result.Add(transaction);
      }

      var nextTransactions = result.Where(t => t.CurrentIndex != t.LastIndex).ToList();
      if (nextTransactions.Count > 0)
      {
        result.AddRange(this.TraverseBundles(nextTransactions.Select(t => t.TrunkTransaction).ToList(), nextTransactions.Select(t => t.BundleHash).ToList()));
      }

      return result;
    }

    /// <summary>
    /// The traverse bundle.
    /// </summary>
    /// <param name="transactionHash">
    /// The transaction hash.
    /// </param>
    /// <param name="bundleHash">
    /// The bundle hash.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    private List<Transaction> TraverseBundle(Hash transactionHash, Hash bundleHash = null)
    {
      var transactionTrytes = this.GetTrytes(new List<Hash> { transactionHash });
      var transaction = Transaction.FromTrytes(transactionTrytes[0]);

      if (bundleHash == null && transaction.CurrentIndex != 0)
      {
        throw new ArgumentException(
          "Traverse bundle started with non tail transaction. Please provide tail transaction Hash.");
      }

      if (bundleHash != null)
      {
        if (bundleHash.Value != transaction.BundleHash.Value)
        {
          return new List<Transaction>();
        }
      }
      else
      {
        bundleHash = transaction.BundleHash;
      }

      if (transaction.CurrentIndex == transaction.LastIndex)
      {
        return new List<Transaction> { transaction };
      }

      var result = new List<Transaction> { transaction };
      result.AddRange(this.TraverseBundle(transaction.TrunkTransaction, bundleHash));

      return result;
    }

    /// <summary>
    /// The traverse bundle async.
    /// </summary>
    /// <param name="transactionHash">
    /// The transaction hash.
    /// </param>
    /// <param name="bundleHash">
    /// The bundle hash.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    private async Task<List<Transaction>> TraverseBundleAsync(Hash transactionHash, Hash bundleHash = null)
    {
      var transactionTrytes = await this.GetTrytesAsync(new List<Hash> { transactionHash });
      var transaction = Transaction.FromTrytes(transactionTrytes[0]);

      if (bundleHash == null && transaction.CurrentIndex != 0)
      {
        throw new ArgumentException(
          "Traverse bundle started with non tail transaction. Please provide tail transaction Hash.");
      }

      if (bundleHash != null)
      {
        if (bundleHash.Value != transaction.BundleHash.Value)
        {
          return new List<Transaction>();
        }
      }
      else
      {
        bundleHash = transaction.BundleHash;
      }

      if (transaction.CurrentIndex == transaction.LastIndex)
      {
        return new List<Transaction> { transaction };
      }

      var result = new List<Transaction> { transaction };
      result.AddRange(await this.TraverseBundleAsync(transaction.TrunkTransaction, bundleHash));

      return result;
    }
  }
}