namespace Tangle.Net.Repository
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net;

  using RestSharp;
  using RestSharp.Authenticators;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Repository.DataTransfer;
  using Tangle.Net.Repository.Responses;

  /// <summary>
  /// The rest iota repository.
  /// </summary>
  public class RestIotaRepository : IIotaCoreRepository, IIotaNodeRepository, IIotaExtendedRepository
  {
    #region Fields

    /// <summary>
    /// The valid find transactions parameters.
    /// </summary>
    private readonly string[] validFindTransactionParameters = { "addresses", "tags", "approvees", "bundles" };

    #endregion

    #region Constructors and Destructors

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
    public RestIotaRepository(IRestClient client, PoWService powService = null, string username = null, string password = null)
    {
      this.Client = client;
      this.PoWService = powService;

      if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
      {
        this.Client.Authenticator = new HttpBasicAuthenticator(username, password);
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    private IRestClient Client { get; set; }

    /// <summary>
    /// Gets or sets the po w service.
    /// </summary>
    private PoWService PoWService { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The add neighbor.
    /// </summary>
    /// <param name="neighbors">
    /// The neighbors.
    /// </param>
    /// <returns>
    /// The <see cref="AddNeighborsResponse"/>.
    /// </returns>
    public AddNeighborsResponse AddNeighbor(IEnumerable<Neighbor> neighbors)
    {
      return
        this.ExecuteParameterizedCommand<AddNeighborsResponse>(
          new Dictionary<string, object> { { "command", Commands.AddNeighbors }, { "uris", neighbors.Select(n => n.Address).ToList() } });
    }

    /// <summary>
    /// The attach to tangle.
    /// </summary>
    /// <param name="branchTransaction">
    /// The branch transactions.
    /// </param>
    /// <param name="trunkTransaction">
    /// The trunk transactions.
    /// </param>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public List<TransactionTrytes> AttachToTangle(
      Hash branchTransaction, 
      Hash trunkTransaction, 
      IEnumerable<Transaction> transactions, 
      int minWeightMagnitude = 18)
    {
      if (this.PoWService != null)
      {
        return
          this.PoWService.DoPoW(branchTransaction, trunkTransaction, transactions.ToList(), minWeightMagnitude).Select(t => t.ToTrytes()).ToList();
      }

      var result =
        this.ExecuteParameterizedCommand<AttachToTangleResponse>(
          new Dictionary<string, object>
            {
              { "command", Commands.AttachToTangle }, 
              { "trunkTransaction", trunkTransaction.ToString() }, 
              { "branchTransaction", branchTransaction.ToString() }, 
              { "minWeightMagnitude", minWeightMagnitude }, 
              { "trytes", transactions.Select(transaction => transaction.ToTrytes().Value).ToList() }
            });

      return result.Trytes.Select(t => new TransactionTrytes(t)).ToList();
    }

    /// <summary>
    /// The broadcast and store transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    public void BroadcastAndStoreTransactions(List<TransactionTrytes> transactions)
    {
      this.BroadcastTransactions(transactions);
      this.StoreTransactions(transactions);
    }

    /// <summary>
    /// The broadcast transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    public void BroadcastTransactions(IEnumerable<TransactionTrytes> transactions)
    {
      this.Client.Execute(
        CreateRequest(
          new Dictionary<string, object> { { "command", Commands.BroadcastTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } }));
    }

    /// <summary>
    /// The find transactions.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionHashList"/>.
    /// </returns>
    public TransactionHashList FindTransactions(Dictionary<string, IEnumerable<TryteString>> parameters)
    {
      if (parameters.Count == 0)
      {
        throw new ArgumentException("At least one parameter must be set.");
      }

      if (!parameters.Any(p => this.validFindTransactionParameters.Contains(p.Key)))
      {
        throw new ArgumentException("A parameters seems to be invalid.");
      }

      if (parameters.Any(parameter => !parameter.Value.Any()))
      {
        throw new ArgumentException("A parameters seems to not contain values!");
      }

      var command = new Dictionary<string, object> { { "command", Commands.FindTransactions } };

      foreach (var parameter in parameters)
      {
        command.Add(parameter.Key, parameter.Value.Select(value => value.Value).ToList());
      }

      var result = this.ExecuteParameterizedCommand<GetTransactionsResponse>(command);

      return new TransactionHashList { Hashes = result.Hashes.ConvertAll(hash => new Hash(hash)) };
    }

    /// <summary>
    /// The get transactions by addresses.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <returns>
    /// The <see cref="GetTransactionsResponse"/>.
    /// </returns>
    public TransactionHashList FindTransactionsByAddresses(IEnumerable<Address> addresses)
    {
      return
        this.FindTransactions(
          new Dictionary<string, IEnumerable<TryteString>> { { "addresses", addresses.Select(a => new TryteString(a.Value)).ToList() } });
    }

    /// <summary>
    /// The find transactions by approvees.
    /// </summary>
    /// <param name="approveeHashes">
    /// The approvee hashes.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionHashList"/>.
    /// </returns>
    public TransactionHashList FindTransactionsByApprovees(IEnumerable<Hash> approveeHashes)
    {
      return
        this.FindTransactions(
          new Dictionary<string, IEnumerable<TryteString>> { { "approvees", approveeHashes.Select(a => new TryteString(a.Value)).ToList() } });
    }

    /// <summary>
    /// The find transactions by bundles.
    /// </summary>
    /// <param name="bundleHashes">
    /// The bundle hashes.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionHashList"/>.
    /// </returns>
    public TransactionHashList FindTransactionsByBundles(IEnumerable<Hash> bundleHashes)
    {
      return
        this.FindTransactions(
          new Dictionary<string, IEnumerable<TryteString>> { { "bundles", bundleHashes.Select(a => new TryteString(a.Value)).ToList() } });
    }

    /// <summary>
    /// The find transactions by tags.
    /// </summary>
    /// <param name="tags">
    /// The tags.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionHashList"/>.
    /// </returns>
    public TransactionHashList FindTransactionsByTags(IEnumerable<Tag> tags)
    {
      return
        this.FindTransactions(new Dictionary<string, IEnumerable<TryteString>> { { "tags", tags.Select(a => new TryteString(a.Value)).ToList() } });
    }

    /// <summary>
    /// The find used addresses.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="start">
    /// The start.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
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

    /// <summary>
    /// The get account data.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="includeInclusionStates">
    /// The inclusion state.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="addressStartIndex">
    /// The address start index.
    /// </param>
    /// <param name="addressStopIndex">
    /// The address stop index.
    /// </param>
    /// <returns>
    /// The <see cref="GetAccountDataResponse"/>.
    /// </returns>
    public GetAccountDataResponse GetAccountData(
      Seed seed, 
      bool includeInclusionStates, 
      int securityLevel, 
      int addressStartIndex, 
      int addressStopIndex = 0)
    {
      var usedAddressesWithTransactions = this.FindUsedAddressesWithTransactions(seed, securityLevel, addressStartIndex);
      var latestUnusedAddress = new AddressGenerator(seed, securityLevel).GetAddress(usedAddressesWithTransactions.UsedAddresses.Last().KeyIndex + 1);
      var addressesWithBalance = new List<Address>();
      var associatedBundles = new List<Bundle>();

      if (usedAddressesWithTransactions.AssociatedTransactionHashes.Count > 0)
      {
        addressesWithBalance = this.GetBalances(usedAddressesWithTransactions.UsedAddresses).Addresses;
        associatedBundles = this.GetBundles(usedAddressesWithTransactions.AssociatedTransactionHashes, includeInclusionStates);
      }

      return new GetAccountDataResponse
               {
                 Balance = addressesWithBalance.Sum(a => a.Balance), 
                 UsedAddresses = usedAddressesWithTransactions.UsedAddresses, 
                 AssociatedBundles = associatedBundles, 
                 LatestUnusedAddress = latestUnusedAddress
               };
    }

    /// <summary>
    /// The get balances.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <param name="threshold">
    /// The threshold.
    /// </param>
    /// <returns>
    /// The <see cref="AddressWithBalances"/>.
    /// </returns>
    public AddressWithBalances GetBalances(List<Address> addresses, int threshold = 100)
    {
      var result =
        this.ExecuteParameterizedCommand<GetBalanceResponse>(
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
                 References = result.References.ConvertAll(reference => new TryteString(reference))
               };
    }

    /// <summary>
    /// The get bundle.
    /// </summary>
    /// <param name="transactionHash">
    /// The transaction hash.
    /// </param>
    /// <returns>
    /// The <see cref="Bundle"/>.
    /// </returns>
    public Bundle GetBundle(Hash transactionHash)
    {
      var bundle = new Bundle { Transactions = this.TraverseBundle(transactionHash) };

      var validationResult = bundle.Validate();

      if (!validationResult.IsValid)
      {
        throw new InvalidBundleException("The bundle is not valid. See ValidationErrors for details.", validationResult.Errors);
      }

      return bundle;
    }

    /// <summary>
    /// The get bundles.
    /// </summary>
    /// <param name="transactionHashes">
    /// The transaction hashes.
    /// </param>
    /// <param name="includeInclusionStates">
    /// The include inclusion states.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
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

      var allBundleTransactions = this.FindTransactionsByBundles(nonTailTransactions.Select(t => t.BundleHash).Distinct().ToList()).Hashes;
      var allBundleTransactionTrytes = this.GetTrytes(allBundleTransactions);

      tailTransactions.AddRange(
        from bundleTransactionTryte in allBundleTransactionTrytes
        select Transaction.FromTrytes(bundleTransactionTryte)
        into transaction
        where transaction.IsTail
        select transaction.Hash);

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
          bundle.IsConfirmed = inclusionStates.States.FirstOrDefault(transactionHash => transactionHash.Key.Value == tailTransaction.Value).Value;
        }

        associatedBundles.Add(bundle);
      }

      return associatedBundles;
    }

    /// <summary>
    /// The get inclusion states.
    /// </summary>
    /// <param name="transactionHashes">
    /// The transactions hashes.
    /// </param>
    /// <param name="tips">
    /// The tips.
    /// </param>
    /// <returns>
    /// The <see cref="InclusionStates"/>.
    /// </returns>
    public InclusionStates GetInclusionStates(List<Hash> transactionHashes, IEnumerable<Hash> tips)
    {
      var result =
        this.ExecuteParameterizedCommand<GetInclusionStatesResponse>(
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

    /// <summary>
    /// The get inputs.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="threshold">
    /// The threshold.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="startIndex">
    /// The start addressStartIndex.
    /// </param>
    /// <param name="stopIndex">
    /// The stop addressStartIndex.
    /// </param>
    /// <returns>
    /// The <see cref="GetInputsResponse"/>.
    /// </returns>
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

    /// <summary>
    /// The get latest inclusion.
    /// </summary>
    /// <param name="hashes">
    /// The hashes.
    /// </param>
    /// <returns>
    /// The <see cref="InclusionStates"/>.
    /// </returns>
    public InclusionStates GetLatestInclusion(List<Hash> hashes)
    {
      var nodeInfo = this.GetNodeInfo();
      return this.GetInclusionStates(hashes, new List<Hash> { new Hash(nodeInfo.LatestSolidSubtangleMilestone) });
    }

    /// <summary>
    /// The get neighbors.
    /// </summary>
    /// <returns>
    /// The <see cref="NeighborList"/>.
    /// </returns>
    public NeighborList GetNeighbors()
    {
      return this.ExecuteParameterlessCommand<NeighborList>(Commands.GetNeighbors);
    }

    /// <summary>
    /// The get new addresses.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="addressStartIndex">
    /// The addressStartIndex.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
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

    /// <summary>
    /// The get node info.
    /// </summary>
    /// <returns>
    /// The <see cref="NodeInfo"/>.
    /// </returns>
    public NodeInfo GetNodeInfo()
    {
      return this.ExecuteParameterlessCommand<NodeInfo>(Commands.GetNodeInfo);
    }

    /// <summary>
    /// The get tips.
    /// </summary>
    /// <returns>
    /// The <see cref="TipHashList"/>.
    /// </returns>
    public TipHashList GetTips()
    {
      var response = this.ExecuteParameterlessCommand<GetTipsResponse>(Commands.GetTips);

      return new TipHashList { Duration = response.Duration, Hashes = response.Hashes.Select(h => new Hash(h)).ToList() };
    }

    /// <summary>
    /// The get transactions to approve.
    /// </summary>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionsToApprove"/>.
    /// </returns>
    public TransactionsToApprove GetTransactionsToApprove(int depth = 27)
    {
      var result =
        this.ExecuteParameterizedCommand<GetTransactionsToApproveResponse>(
          new Dictionary<string, object> { { "command", Commands.GetTransactionsToApprove }, { "depth", depth } });

      return new TransactionsToApprove
               {
                 BranchTransaction = new Hash(result.BranchTransaction), 
                 TrunkTransaction = new Hash(result.TrunkTransaction), 
                 Duration = result.Duration
               };
    }

    /// <summary>
    /// The get transfers.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="includeInclusionStates">
    /// The include inclusion states.
    /// </param>
    /// <param name="addressStartIndex">
    /// The address start index.
    /// </param>
    /// <param name="addressStopIndex">
    /// The address stop index.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<Bundle> GetTransfers(Seed seed, int securityLevel, bool includeInclusionStates, int addressStartIndex, int addressStopIndex = 0)
    {
      var addressGenerator = new AddressGenerator(seed, securityLevel);
      var transactions = addressStopIndex == 0
                           ? this.FindUsedAddressesWithTransactions(seed, securityLevel, addressStartIndex).AssociatedTransactionHashes
                           : this.FindTransactionsByAddresses(addressGenerator.GetAddresses(0, addressStartIndex - addressStopIndex + 1)).Hashes;

      return this.GetBundles(transactions, includeInclusionStates);
    }

    /// <summary>
    /// The get trytes.
    /// </summary>
    /// <param name="hashes">
    /// The hashes.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public List<TransactionTrytes> GetTrytes(IEnumerable<Hash> hashes)
    {
      var result =
        this.ExecuteParameterizedCommand<GetTrytesResponse>(
          new Dictionary<string, object> { { "command", Commands.GetTrytes }, { "hashes", hashes.Select(h => h.Value).ToList() } });

      return result.Trytes.Select(tryte => new TransactionTrytes(tryte)).ToList();
    }

    /// <summary>
    /// The interrupt attaching to tangle.
    /// </summary>
    public void InterruptAttachingToTangle()
    {
      this.Client.Execute(CreateRequest(new Dictionary<string, object> { { "command", Commands.InterruptAttachingToTangle } }));
    }

    /// <summary>
    /// The prepare transfer.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder address.
    /// </param>
    /// <param name="inputAddresses">
    /// The input addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Bundle"/>.
    /// </returns>
    public Bundle PrepareTransfer(Seed seed, Bundle bundle, int securityLevel, Address remainderAddress = null, List<Address> inputAddresses = null)
    {
      // user wants to spend IOTA, so we need to find input addresses (if not provided) with valid balances
      if (bundle.Balance > 0)
      {
        if (inputAddresses == null)
        {
          inputAddresses = this.GetInputs(seed, bundle.Balance, securityLevel, 0).Addresses;
        }
        else
        {
          inputAddresses = this.GetBalances(inputAddresses).Addresses;
        }

        var availableAmount = inputAddresses.Sum(a => a.Balance);

        if (availableAmount < bundle.Balance)
        {
          throw new IotaApiException(string.Format("Insufficient balance! Found {0}. Need {1}", availableAmount, bundle.Balance));
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

    /// <summary>
    /// The remove neighbors.
    /// </summary>
    /// <param name="neighbors">
    /// The neighbors.
    /// </param>
    /// <returns>
    /// The <see cref="RemoveNeighborsResponse"/>.
    /// </returns>
    public RemoveNeighborsResponse RemoveNeighbors(IEnumerable<Neighbor> neighbors)
    {
      return
        this.ExecuteParameterizedCommand<RemoveNeighborsResponse>(
          new Dictionary<string, object> { { "command", Commands.RemoveNeighbors }, { "uris", neighbors.Select(n => n.Address).ToList() } });
    }

    /// <summary>
    /// The replay bundle.
    /// </summary>
    /// <param name="transactionHash">
    /// The transaction hash.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<TransactionTrytes> ReplayBundle(Hash transactionHash, int depth = 27, int minWeightMagnitude = 18)
    {
      var bundle = this.GetBundle(transactionHash);

      return this.SendTrytes(bundle.Transactions, depth, minWeightMagnitude);
    }

    /// <summary>
    /// The send transfer.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder address.
    /// </param>
    /// <param name="inputAddresses">
    /// The input addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Bundle"/>.
    /// </returns>
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

    /// <summary>
    /// The send trytes.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<TransactionTrytes> SendTrytes(IEnumerable<Transaction> transactions, int depth = 27, int minWeightMagnitude = 18)
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

    /// <summary>
    /// The store transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    public void StoreTransactions(IEnumerable<TransactionTrytes> transactions)
    {
      this.Client.Execute(
        CreateRequest(
          new Dictionary<string, object> { { "command", Commands.BroadcastTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } }));
    }

    #endregion

    #region Methods

    /// <summary>
    /// The create request.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <returns>
    /// The <see cref="RestRequest"/>.
    /// </returns>
    private static RestRequest CreateRequest(IEnumerable<KeyValuePair<string, object>> parameters)
    {
      var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
      request.AddHeader("X-IOTA-API-Version", "1");
      request.AddJsonBody(parameters);

      return request;
    }

    /// <summary>
    /// The execute command.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <typeparam name="T">
    /// The node property to return
    /// </typeparam>
    /// <returns>
    /// The <see cref="T"/>.
    /// </returns>
    private T ExecuteParameterizedCommand<T>(IReadOnlyCollection<KeyValuePair<string, object>> parameters) where T : new()
    {
      var response = this.Client.Execute<T>(CreateRequest(parameters));
      var nullResponse = response == null;

      if (!nullResponse && response.StatusCode == HttpStatusCode.OK)
      {
        return response.Data;
      }

      if (nullResponse)
      {
        throw new IotaApiException(string.Format("Command {0} failed!", parameters.First(p => p.Key == "command").Value));
      }

      if (response.ErrorException != null)
      {
        throw new IotaApiException(
          string.Format("Command {0} failed! See inner exception for details.", parameters.First(p => p.Key == "command").Value), 
          response.ErrorException);
      }

      throw new IotaApiException(string.Format("Command {0} failed!", parameters.First(p => p.Key == "command").Value));
    }

    /// <summary>
    /// The execute command.
    /// </summary>
    /// <param name="commandName">
    /// The command name.
    /// </param>
    /// <typeparam name="T">
    /// The node property to return
    /// </typeparam>
    /// <returns>
    /// The <see cref="T"/>.
    /// </returns>
    private T ExecuteParameterlessCommand<T>(string commandName) where T : new()
    {
      return this.ExecuteParameterizedCommand<T>(new Dictionary<string, object> { { "command", commandName } });
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
        throw new ArgumentException("Traverse bundle started with non tail transaction. Please provide tail transaction Hash.");
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

    #endregion
  }
}