namespace Tangle.Net.Repository
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net;
  using System.Threading.Tasks;

  using RestSharp;
  using RestSharp.Authenticators;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Repository.Client;
  using Tangle.Net.Repository.DataTransfer;
  using Tangle.Net.Repository.Responses;

  /// <summary>
  ///   The rest iota repository.
  /// </summary>
  public class RestIotaRepository : IIotaRepository
  {
    /// <summary>
    ///   The valid find transactions parameters.
    /// </summary>
    private readonly string[] validFindTransactionParameters = { "addresses", "tags", "approvees", "bundles" };

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
    public RestIotaRepository(IIotaClient client, IPoWService powService)
    {
      this.Client = client;
      this.PoWService = powService;
    }

    /// <summary>
    ///   Gets the client.
    /// </summary>
    private IIotaClient Client { get; }

    /// <summary>
    ///   Gets the po w service.
    /// </summary>
    private IPoWService PoWService { get; }

    /// <inheritdoc />
    public AddNeighborsResponse AddNeighbor(IEnumerable<Neighbor> neighbors)
    {
      return this.Client.ExecuteParameterizedCommand<AddNeighborsResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.AddNeighbors },
            { "uris", neighbors.Select(n => n.Address).ToList() }
          });
    }

    /// <inheritdoc />
    public async Task<AddNeighborsResponse> AddNeighborAsync(IEnumerable<Neighbor> neighbors)
    {
      return await this.Client.ExecuteParameterizedCommandAsync<AddNeighborsResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.AddNeighbors },
            { "uris", neighbors.Select(n => n.Address).ToList() }
          });
    }

    /// <inheritdoc />
    public List<TransactionTrytes> AttachToTangle(
      Hash branchTransaction,
      Hash trunkTransaction,
      IEnumerable<Transaction> transactions,
      int minWeightMagnitude = 18)
    {
      return this.PoWService.DoPoW(branchTransaction, trunkTransaction, transactions.ToList(), minWeightMagnitude).Select(t => t.ToTrytes()).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TransactionTrytes>> AttachToTangleAsync(Hash branchTransaction, Hash trunkTransaction, IEnumerable<Transaction> transactions, int minWeightMagnitude = 18)
    {
      return (await this.PoWService.DoPoWAsync(branchTransaction, trunkTransaction, transactions.ToList(), minWeightMagnitude)).Select(t => t.ToTrytes()).ToList();
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
    public void BroadcastTransactions(IEnumerable<TransactionTrytes> transactions)
    {
      this.Client.ExecuteParameterizedCommand(
        new Dictionary<string, object> { { "command", Commands.BroadcastTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } });
    }

    /// <inheritdoc />
    public async Task BroadcastTransactionsAsync(IEnumerable<TransactionTrytes> transactions)
    {
      await this.Client.ExecuteParameterizedCommandAsync(
        new Dictionary<string, object> { { "command", Commands.BroadcastTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } });
    }

    /// <inheritdoc />
    public TransactionHashList FindTransactions(Dictionary<string, IEnumerable<TryteString>> parameters)
    {
      var command = this.CreateFindTransactionsParameters(parameters);
      var result = this.Client.ExecuteParameterizedCommand<GetTransactionsResponse>(command);

      return new TransactionHashList { Hashes = result?.Hashes.ConvertAll(hash => new Hash(hash)) };
    }

    /// <inheritdoc />
    public async Task<TransactionHashList> FindTransactionsAsync(
      Dictionary<string, IEnumerable<TryteString>> parameters)
    {
      var command = this.CreateFindTransactionsParameters(parameters);
      var result = await this.Client.ExecuteParameterizedCommandAsync<GetTransactionsResponse>(command);

      return new TransactionHashList { Hashes = result?.Hashes.ConvertAll(hash => new Hash(hash)) };
    }

    /// <inheritdoc />
    public TransactionHashList FindTransactionsByAddresses(IEnumerable<Address> addresses)
    {
      return this.FindTransactions(
               new Dictionary<string, IEnumerable<TryteString>>
                 {
                   {
                     "addresses",
                     addresses.Select(a => new TryteString(a.Value)).ToList()
                   }
                 });
    }

    /// <inheritdoc />
    public async Task<TransactionHashList> FindTransactionsByAddressesAsync(IEnumerable<Address> addresses)
    {
      return await this.FindTransactionsAsync(
        new Dictionary<string, IEnumerable<TryteString>>
          {
            {
              "addresses",
              addresses.Select(a => new TryteString(a.Value)).ToList()
            }
          });
    }

    /// <inheritdoc />
    public TransactionHashList FindTransactionsByApprovees(IEnumerable<Hash> approveeHashes)
    {
      return this.FindTransactions(
               new Dictionary<string, IEnumerable<TryteString>>
                 {
                   {
                     "approvees",
                     approveeHashes.Select(a => new TryteString(a.Value))
                       .ToList()
                   }
                 });
    }

    /// <inheritdoc />
    public async Task<TransactionHashList> FindTransactionsByApproveesAsync(IEnumerable<Hash> approveeHashes)
    {
      return await this.FindTransactionsAsync(
        new Dictionary<string, IEnumerable<TryteString>>
          {
            {
              "approvees",
              approveeHashes.Select(a => new TryteString(a.Value))
                .ToList()
            }
          });
    }

    /// <inheritdoc />
    public TransactionHashList FindTransactionsByBundles(IEnumerable<Hash> bundleHashes)
    {
      return this.FindTransactions(
               new Dictionary<string, IEnumerable<TryteString>>
                 {
                   {
                     "bundles",
                     bundleHashes.Select(a => new TryteString(a.Value)).ToList()
                   }
                 });
    }

    /// <inheritdoc />
    public async Task<TransactionHashList> FindTransactionsByBundlesAsync(IEnumerable<Hash> bundleHashes)
    {
      return await this.FindTransactionsAsync(
        new Dictionary<string, IEnumerable<TryteString>>
          {
            {
              "bundles",
              bundleHashes.Select(a => new TryteString(a.Value)).ToList()
            }
          });
    }

    /// <inheritdoc />
    public TransactionHashList FindTransactionsByTags(IEnumerable<Tag> tags)
    {
      return this.FindTransactions(
               new Dictionary<string, IEnumerable<TryteString>>
                 {
                   {
                     "tags", tags.Select(a => new TryteString(a.Value)).ToList()
                   }
                 });
    }

    /// <inheritdoc />
    public async Task<TransactionHashList> FindTransactionsByTagsAsync(IEnumerable<Tag> tags)
    {
      return await this.FindTransactionsAsync(
        new Dictionary<string, IEnumerable<TryteString>>
          {
            {
              "tags", tags.Select(a => new TryteString(a.Value)).ToList()
            }
          });
    }

    /// <inheritdoc />
    public FindUsedAddressesResponse FindUsedAddressesWithTransactions(Seed seed, int securityLevel, int start)
    {
      var usedAddresses = new List<Address>();
      var associatedTransactionHashes = new List<Hash>();
      var addressGenerator = new AddressGenerator(seed, securityLevel);

      var currentIndex = start;
      while (true)
      {
        var address = addressGenerator.GetAddress(currentIndex);
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
      var addressGenerator = new AddressGenerator(seed, securityLevel);

      var currentIndex = start;
      while (true)
      {
        var address = addressGenerator.GetAddress(currentIndex);
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
      var latestUnusedAddress =
        new AddressGenerator(seed, securityLevel).GetAddress(
          usedAddresses.Any() ? usedAddresses.Last().KeyIndex + 1 : 0);
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
      var latestUnusedAddress =
        new AddressGenerator(seed, securityLevel).GetAddress(
          usedAddresses.Any() ? usedAddresses.Last().KeyIndex + 1 : 0);
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
    public AddressWithBalances GetBalances(List<Address> addresses, int threshold = 100)
    {
      var result = this.Client.ExecuteParameterizedCommand<GetBalanceResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.GetBalances },
            { "addresses", addresses.Select(a => a.Value).ToList() },
            { "threshold", threshold }
          });

      for (var i = 0; i < addresses.Count(); i++)
      {
        addresses[i].Balance = result.Balances[i];
      }

      return new AddressWithBalances
               {
                 Addresses = addresses,
                 Duration = result.Duration,
                 MilestoneIndex = result.MilestoneIndex,
                 References =
                   result.References.ConvertAll(reference => new TryteString(reference))
               };
    }

    /// <inheritdoc />
    public async Task<AddressWithBalances> GetBalancesAsync(List<Address> addresses, int threshold = 100)
    {
      var result = await this.Client.ExecuteParameterizedCommandAsync<GetBalanceResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.GetBalances },
            { "addresses", addresses.Select(a => a.Value).ToList() },
            { "threshold", threshold }
          });

      for (var i = 0; i < addresses.Count(); i++)
      {
        addresses[i].Balance = result.Balances[i];
      }

      return new AddressWithBalances
               {
                 Addresses = addresses,
                 Duration = result.Duration,
                 MilestoneIndex = result.MilestoneIndex,
                 References =
                   result.References.ConvertAll(reference => new TryteString(reference))
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
    public List<Bundle> GetBundles(IEnumerable<Hash> transactionHashes, bool includeInclusionStates)
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

      foreach (var tailTransaction in tailTransactions)
      {
        var bundle = this.GetBundle(tailTransaction);

        if (includeInclusionStates)
        {
          bundle.IsConfirmed = inclusionStates.States
            .FirstOrDefault(transactionHash => transactionHash.Key.Value == tailTransaction.Value).Value;
        }

        associatedBundles.Add(bundle);
      }

      return associatedBundles;
    }

    /// <inheritdoc />
    public async Task<List<Bundle>> GetBundlesAsync(IEnumerable<Hash> transactionHashes, bool includeInclusionStates)
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

      foreach (var tailTransaction in tailTransactions)
      {
        var bundle = await this.GetBundleAsync(tailTransaction);

        if (includeInclusionStates)
        {
          bundle.IsConfirmed = inclusionStates.States
            .FirstOrDefault(transactionHash => transactionHash.Key.Value == tailTransaction.Value).Value;
        }

        associatedBundles.Add(bundle);
      }

      return associatedBundles;
    }

    /// <inheritdoc />
    public InclusionStates GetInclusionStates(List<Hash> transactionHashes, IEnumerable<Hash> tips)
    {
      var result = this.Client.ExecuteParameterizedCommand<GetInclusionStatesResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.GetInclusionStates },
            { "transactions", transactionHashes.Select(t => t.Value).ToList() },
            { "tips", tips.Select(t => t.Value).ToList() }
          });

      var inclusionStates = new Dictionary<Hash, bool>();
      for (var i = 0; i < transactionHashes.Count(); i++)
      {
        inclusionStates.Add(transactionHashes[i], result.States[i]);
      }

      return new InclusionStates { States = inclusionStates, Duration = result.Duration };
    }

    /// <inheritdoc />
    public async Task<InclusionStates> GetInclusionStatesAsync(List<Hash> transactionHashes, IEnumerable<Hash> tips)
    {
      var result = await this.Client.ExecuteParameterizedCommandAsync<GetInclusionStatesResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.GetInclusionStates },
            { "transactions", transactionHashes.Select(t => t.Value).ToList() },
            { "tips", tips.Select(t => t.Value).ToList() }
          });

      var inclusionStates = new Dictionary<Hash, bool>();
      for (var i = 0; i < transactionHashes.Count(); i++)
      {
        inclusionStates.Add(transactionHashes[i], result.States[i]);
      }

      return new InclusionStates { States = inclusionStates, Duration = result.Duration };
    }

    /// <inheritdoc />
    public GetInputsResponse GetInputs(Seed seed, long threshold, int securityLevel, int startIndex, int stopIndex = 0)
    {
      if (startIndex > stopIndex)
      {
        throw new ArgumentException("Invalid bounds! StartIndex must not be lower than StopIndex.");
      }

      var resultAddresses = new List<Address>();
      var addressGenerator = new AddressGenerator(seed, securityLevel);

      var usedAddresses = stopIndex == 0
                            ? this.FindUsedAddressesWithTransactions(seed, securityLevel, startIndex).UsedAddresses
                            : addressGenerator.GetAddresses(0, stopIndex - startIndex + 1);

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
      var addressGenerator = new AddressGenerator(seed, securityLevel);

      var usedAddresses = stopIndex == 0
                            ? (await this.FindUsedAddressesWithTransactionsAsync(seed, securityLevel, startIndex)).UsedAddresses
                            : addressGenerator.GetAddresses(0, stopIndex - startIndex + 1);

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
    public NeighborList GetNeighbors()
    {
      return this.Client.ExecuteParameterlessCommand<NeighborList>(Commands.GetNeighbors);
    }

    /// <inheritdoc />
    public async Task<NeighborList> GetNeighborsAsync()
    {
      return await this.Client.ExecuteParameterlessCommandAsync<NeighborList>(Commands.GetNeighbors);
    }

    /// <inheritdoc />
    public List<Address> GetNewAddresses(Seed seed, int addressStartIndex, int count, int securityLevel)
    {
      var addressGenerator = new AddressGenerator(seed, securityLevel);
      var result = new List<Address>();

      var foundNewAddress = false;
      var foundAddressCount = 0;

      while (!foundNewAddress || foundAddressCount != count)
      {
        var address = addressGenerator.GetAddress(addressStartIndex);
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
      var addressGenerator = new AddressGenerator(seed, securityLevel);
      var result = new List<Address>();

      var foundNewAddress = false;
      var foundAddressCount = 0;

      while (!foundNewAddress || foundAddressCount != count)
      {
        var address = addressGenerator.GetAddress(addressStartIndex);
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
    public NodeInfo GetNodeInfo()
    {
      return this.Client.ExecuteParameterlessCommand<NodeInfo>(Commands.GetNodeInfo);
    }

    /// <inheritdoc />
    public async Task<NodeInfo> GetNodeInfoAsync()
    {
      return await this.Client.ExecuteParameterlessCommandAsync<NodeInfo>(Commands.GetNodeInfo);
    }

    /// <inheritdoc />
    public TipHashList GetTips()
    {
      var response = this.Client.ExecuteParameterlessCommand<GetTipsResponse>(Commands.GetTips);

      return new TipHashList
               {
                 Duration = response.Duration,
                 Hashes = response.Hashes.Select(h => new Hash(h)).ToList()
               };
    }

    /// <inheritdoc />
    public async Task<TipHashList> GetTipsAsync()
    {
      var response = await this.Client.ExecuteParameterlessCommandAsync<GetTipsResponse>(Commands.GetTips);

      return new TipHashList
               {
                 Duration = response.Duration,
                 Hashes = response.Hashes.Select(h => new Hash(h)).ToList()
               };
    }

    /// <inheritdoc />
    public TransactionsToApprove GetTransactionsToApprove(int depth = 27)
    {
      var result = this.Client.ExecuteParameterizedCommand<GetTransactionsToApproveResponse>(
        new Dictionary<string, object> { { "command", Commands.GetTransactionsToApprove }, { "depth", depth } });

      return new TransactionsToApprove
               {
                 BranchTransaction = new Hash(result.BranchTransaction),
                 TrunkTransaction = new Hash(result.TrunkTransaction),
                 Duration = result.Duration
               };
    }

    /// <inheritdoc />
    public async Task<TransactionsToApprove> GetTransactionsToApproveAsync(int depth = 27)
    {
      var result = await this.Client.ExecuteParameterizedCommandAsync<GetTransactionsToApproveResponse>(
        new Dictionary<string, object> { { "command", Commands.GetTransactionsToApprove }, { "depth", depth } });

      return new TransactionsToApprove
               {
                 BranchTransaction = new Hash(result.BranchTransaction),
                 TrunkTransaction = new Hash(result.TrunkTransaction),
                 Duration = result.Duration
               };
    }

    /// <inheritdoc />
    public List<Bundle> GetTransfers(
      Seed seed,
      int securityLevel,
      bool includeInclusionStates,
      int addressStartIndex,
      int addressStopIndex = 0)
    {
      var addressGenerator = new AddressGenerator(seed, securityLevel);
      var transactions = addressStopIndex == 0
                           ? this.FindUsedAddressesWithTransactions(seed, securityLevel, addressStartIndex)
                             .AssociatedTransactionHashes
                           : this.FindTransactionsByAddresses(
                             addressGenerator.GetAddresses(0, addressStartIndex - addressStopIndex + 1)).Hashes;

      return this.GetBundles(transactions, includeInclusionStates);
    }

    /// <inheritdoc />
    public async Task<List<Bundle>> GetTransfersAsync(Seed seed, int securityLevel, bool includeInclusionStates, int addressStartIndex, int addressStopIndex = 0)
    {
      var addressGenerator = new AddressGenerator(seed, securityLevel);
      var transactions = addressStopIndex == 0
                           ? (await this.FindUsedAddressesWithTransactionsAsync(seed, securityLevel, addressStartIndex)).AssociatedTransactionHashes
                           : (await this.FindTransactionsByAddressesAsync(addressGenerator.GetAddresses(0, addressStartIndex - addressStopIndex + 1)))
                           .Hashes;

      return await this.GetBundlesAsync(transactions, includeInclusionStates);
    }

    /// <inheritdoc />
    public List<TransactionTrytes> GetTrytes(IEnumerable<Hash> hashes)
    {
      var result = this.Client.ExecuteParameterizedCommand<GetTrytesResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.GetTrytes },
            { "hashes", hashes.Select(h => h.Value).ToList() }
          });

      return result.Trytes.Select(tryte => new TransactionTrytes(tryte)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TransactionTrytes>> GetTrytesAsync(IEnumerable<Hash> hashes)
    {
      var result = await this.Client.ExecuteParameterizedCommandAsync<GetTrytesResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.GetTrytes },
            { "hashes", hashes.Select(h => h.Value).ToList() }
          });

      return result.Trytes.Select(tryte => new TransactionTrytes(tryte)).ToList();
    }

    /// <inheritdoc />
    public void InterruptAttachingToTangle()
    {
      this.Client.ExecuteParameterizedCommand(new Dictionary<string, object> { { "command", Commands.InterruptAttachingToTangle } });
    }

    /// <inheritdoc />
    public async Task InterruptAttachingToTangleAsync()
    {
      await this.Client.ExecuteParameterizedCommandAsync(new Dictionary<string, object> { { "command", Commands.InterruptAttachingToTangle } });
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
      bundle.Sign(new KeyGenerator(seed));

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
      bundle.Sign(new KeyGenerator(seed));

      return bundle;
    }

    /// <inheritdoc />
    public RemoveNeighborsResponse RemoveNeighbors(IEnumerable<Neighbor> neighbors)
    {
      return this.Client.ExecuteParameterizedCommand<RemoveNeighborsResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.RemoveNeighbors },
            { "uris", neighbors.Select(n => n.Address).ToList() }
          });
    }

    /// <inheritdoc />
    public async Task<RemoveNeighborsResponse> RemoveNeighborsAsync(IEnumerable<Neighbor> neighbors)
    {
      return await this.Client.ExecuteParameterizedCommandAsync<RemoveNeighborsResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.RemoveNeighbors },
            { "uris", neighbors.Select(n => n.Address).ToList() }
          });
    }

    /// <inheritdoc />
    public List<TransactionTrytes> ReplayBundle(Hash transactionHash, int depth = 27, int minWeightMagnitude = 18)
    {
      var bundle = this.GetBundle(transactionHash);

      return this.SendTrytes(bundle.Transactions, depth, minWeightMagnitude);
    }

    /// <inheritdoc />
    public async Task<List<TransactionTrytes>> ReplayBundleAsync(Hash transactionHash, int depth = 27, int minWeightMagnitude = 18)
    {
      var bundle = await this.GetBundleAsync(transactionHash);

      return await this.SendTrytesAsync(bundle.Transactions, depth, minWeightMagnitude);
    }

    /// <inheritdoc />
    public Bundle SendTransfer(
      Seed seed,
      Bundle bundle,
      int securityLevel,
      int depth = 27,
      int minWeightMagnitude = 18,
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
      int depth = 27,
      int minWeightMagnitude = 18,
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
      int depth = 27,
      int minWeightMagnitude = 18)
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
    public async Task<List<TransactionTrytes>> SendTrytesAsync(IEnumerable<Transaction> transactions, int depth = 27, int minWeightMagnitude = 18)
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

    /// <inheritdoc />
    public void StoreTransactions(IEnumerable<TransactionTrytes> transactions)
    {
      this.Client.ExecuteParameterizedCommand(
        new Dictionary<string, object> { { "command", Commands.StoreTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } });
    }

    /// <inheritdoc />
    public async Task StoreTransactionsAsync(IEnumerable<TransactionTrytes> transactions)
    {
      await this.Client.ExecuteParameterizedCommandAsync(
        new Dictionary<string, object> { { "command", Commands.StoreTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } });
    }

    /// <inheritdoc />
    public List<Address> WereAddressesSpentFrom(List<Address> addresses)
    {
      var response = this.Client.ExecuteParameterizedCommand<WhereAddressesSpentFromResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.WereAddressesSpentFrom },
            { "addresses", addresses.Select(a => a.Value).ToList() }
          });

      for (var i = 0; i < addresses.Count; i++)
      {
        addresses[i].SpentFrom = response.States[i];
      }

      return addresses;
    }

    /// <inheritdoc />
    public async Task<List<Address>> WereAddressesSpentFromAsync(List<Address> addresses)
    {
      var response = await this.Client.ExecuteParameterizedCommandAsync<WhereAddressesSpentFromResponse>(
        new Dictionary<string, object>
          {
            { "command", Commands.WereAddressesSpentFrom },
            { "addresses", addresses.Select(a => a.Value).ToList() }
          });

      for (var i = 0; i < addresses.Count; i++)
      {
        addresses[i].SpentFrom = response.States[i];
      }

      return addresses;
    }

    /// <summary>
    /// The create find transactions parameters.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <returns>
    /// The <see cref="Dictionary"/>.
    /// </returns>
    private Dictionary<string, object> CreateFindTransactionsParameters(
      Dictionary<string, IEnumerable<TryteString>> parameters)
    {
      if (parameters.Count == 0)
      {
        throw new ArgumentException("At least one parameter must be set.");
      }

      if (!parameters.Any(p => this.validFindTransactionParameters.Contains(p.Key)))
      {
        throw new ArgumentException("A parameter seems to be invalid.");
      }

      if (parameters.Any(parameter => !parameter.Value.Any()))
      {
        throw new ArgumentException("A parameter seems to not contain values!");
      }

      var command = new Dictionary<string, object> { { "command", Commands.FindTransactions } };

      foreach (var parameter in parameters)
      {
        command.Add(parameter.Key, parameter.Value.Select(value => value.Value).ToList());
      }

      return command;
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