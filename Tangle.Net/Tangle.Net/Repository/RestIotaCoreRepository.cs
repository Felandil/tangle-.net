namespace Tangle.Net.Repository
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository.DataTransfer;
  using Tangle.Net.Repository.Responses;

  /// <summary>
  /// The rest iota repository.
  /// </summary>
  public partial class RestIotaRepository : IIotaRepository
  {

    /// <summary>
    /// The max get trytes hash count.
    /// </summary>
    private const int MaxGetTrytesHashCount = 500;

    /// <summary>
    ///   The valid find transactions parameters.
    /// </summary>
    private readonly string[] validFindTransactionParameters = { "addresses", "tags", "approvees", "bundles" };

    /// <inheritdoc />
    public List<TransactionTrytes> AttachToTangle(
      Hash branchTransaction,
      Hash trunkTransaction,
      IEnumerable<Transaction> transactions,
      int minWeightMagnitude = 14)
    {
      return this.PoWService.DoPoW(branchTransaction, trunkTransaction, transactions.ToList(), minWeightMagnitude).Select(t => t.ToTrytes()).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TransactionTrytes>> AttachToTangleAsync(
      Hash branchTransaction,
      Hash trunkTransaction,
      IEnumerable<Transaction> transactions,
      int minWeightMagnitude = 14)
    {
      return (await this.PoWService.DoPoWAsync(branchTransaction, trunkTransaction, transactions.ToList(), minWeightMagnitude))
        .Select(t => t.ToTrytes()).ToList();
    }

    /// <inheritdoc />
    public void BroadcastTransactions(IEnumerable<TransactionTrytes> transactions)
    {
      this.Client.ExecuteParameterizedCommand(
        new Dictionary<string, object> { { "command", CommandType.BroadcastTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } });
    }

    /// <inheritdoc />
    public async Task BroadcastTransactionsAsync(IEnumerable<TransactionTrytes> transactions)
    {
      await this.Client.ExecuteParameterizedCommandAsync(
        new Dictionary<string, object> { { "command", CommandType.BroadcastTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } });
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
                     "tags", tags.Select(a => new Tag(a.Value)).ToList()
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
              "tags", tags.Select(a => new Tag(a.Value)).ToList()
            }
          });
    }

    /// <inheritdoc />
    public AddressWithBalances GetBalances(List<Address> addresses, int threshold = 100)
    {
      var result = this.Client.ExecuteParameterizedCommand<GetBalanceResponse>(
        new Dictionary<string, object>
          {
            { "command", CommandType.GetBalances },
            { "addresses", addresses.Select(a => a.Value).ToList() },
            { "threshold", threshold }
          });

      for (var i = 0; i < addresses.Count; i++)
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
                         { "command", CommandType.GetBalances },
                         { "addresses", addresses.Select(a => a.Value).ToList() },
                         { "threshold", threshold }
                       });

      for (var i = 0; i < addresses.Count; i++)
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
    public InclusionStates GetInclusionStates(List<Hash> transactionHashes, IEnumerable<Hash> tips)
    {
      var result = this.Client.ExecuteParameterizedCommand<GetInclusionStatesResponse>(
        new Dictionary<string, object>
          {
            { "command", CommandType.GetInclusionStates },
            { "transactions", transactionHashes.Select(t => t.Value).ToList() },
            { "tips", tips.Select(t => t.Value).ToList() }
          });

      var inclusionStates = new Dictionary<Hash, bool>();
      for (var i = 0; i < transactionHashes.Count; i++)
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
                         { "command", CommandType.GetInclusionStates },
                         { "transactions", transactionHashes.Select(t => t.Value).ToList() },
                         { "tips", tips.Select(t => t.Value).ToList() }
                       });

      var inclusionStates = new Dictionary<Hash, bool>();
      for (var i = 0; i < transactionHashes.Count; i++)
      {
        inclusionStates.Add(transactionHashes[i], result.States[i]);
      }

      return new InclusionStates { States = inclusionStates, Duration = result.Duration };
    }

    /// <inheritdoc />
    public TipHashList GetTips()
    {
      var response = this.Client.ExecuteParameterlessCommand<GetTipsResponse>(CommandType.GetTips);

      return new TipHashList
      {
        Duration = response.Duration,
        Hashes = response.Hashes.Select(h => new Hash(h)).ToList()
      };
    }

    /// <inheritdoc />
    public async Task<TipHashList> GetTipsAsync()
    {
      var response = await this.Client.ExecuteParameterlessCommandAsync<GetTipsResponse>(CommandType.GetTips);

      return new TipHashList
      {
        Duration = response.Duration,
        Hashes = response.Hashes.Select(h => new Hash(h)).ToList()
      };
    }

    /// <inheritdoc />
    public TransactionsToApprove GetTransactionsToApprove(int depth = 8)
    {
      var result = this.Client.ExecuteParameterizedCommand<GetTransactionsToApproveResponse>(
        new Dictionary<string, object> { { "command", CommandType.GetTransactionsToApprove }, { "depth", depth } });

      return new TransactionsToApprove
      {
        BranchTransaction = new Hash(result.BranchTransaction),
        TrunkTransaction = new Hash(result.TrunkTransaction),
        Duration = result.Duration
      };
    }

    /// <inheritdoc />
    public async Task<TransactionsToApprove> GetTransactionsToApproveAsync(int depth = 8, Hash reference = null)
    {
      var parameters = new Dictionary<string, object> { { "command", CommandType.GetTransactionsToApprove }, { "depth", depth } };

      if (reference != null)
      {
        parameters.Add("reference", reference.Value);
      }

      var result = await this.Client.ExecuteParameterizedCommandAsync<GetTransactionsToApproveResponse>(parameters);

      return new TransactionsToApprove
      {
        BranchTransaction = new Hash(result.BranchTransaction),
        TrunkTransaction = new Hash(result.TrunkTransaction),
        Duration = result.Duration
      };
    }

    /// <inheritdoc />
    public List<TransactionTrytes> GetTrytes(List<Hash> hashes)
    {
      var result = new GetTrytesResponse { Trytes = new List<string>() };
      if (hashes.Count > MaxGetTrytesHashCount)
      {
        for (var i = 0; i < (hashes.Count / MaxGetTrytesHashCount) + 1; i++)
        {
          var requestHashes = hashes.Skip(i * MaxGetTrytesHashCount).Take(MaxGetTrytesHashCount);
          var interimResult = this.Client.ExecuteParameterizedCommand<GetTrytesResponse>(
            new Dictionary<string, object> { { "command", CommandType.GetTrytes }, { "hashes", requestHashes.Select(h => h.Value).ToList() } });

          result.Trytes.AddRange(interimResult.Trytes);
        }
      }
      else
      {
        result = this.Client.ExecuteParameterizedCommand<GetTrytesResponse>(
          new Dictionary<string, object> { { "command", CommandType.GetTrytes }, { "hashes", hashes.Select(h => h.Value).ToList() } });
      }


      return result.Trytes.Select(tryte => new TransactionTrytes(tryte)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TransactionTrytes>> GetTrytesAsync(List<Hash> hashes)
    {
      var result = new GetTrytesResponse { Trytes = new List<string>() };
      if (hashes.Count > MaxGetTrytesHashCount)
      {
        for (var i = 0; i < (hashes.Count / MaxGetTrytesHashCount) + 1; i++)
        {
          var requestHashes = hashes.Skip(i * MaxGetTrytesHashCount).Take(MaxGetTrytesHashCount);
          var interimResult = await this.Client.ExecuteParameterizedCommandAsync<GetTrytesResponse>(
                                new Dictionary<string, object>
                                  {
                                    { "command", CommandType.GetTrytes },
                                    { "hashes", requestHashes.Select(h => h.Value).ToList() }
                                  });

          result.Trytes.AddRange(interimResult.Trytes);
        }
      }
      else
      {
        result = await this.Client.ExecuteParameterizedCommandAsync<GetTrytesResponse>(
                   new Dictionary<string, object> { { "command", CommandType.GetTrytes }, { "hashes", hashes.Select(h => h.Value).ToList() } });
      }


      return result.Trytes.Select(tryte => new TransactionTrytes(tryte)).ToList();
    }

    /// <inheritdoc />
    public void InterruptAttachingToTangle()
    {
      this.Client.ExecuteParameterizedCommand(new Dictionary<string, object> { { "command", CommandType.InterruptAttachingToTangle } });
    }

    /// <inheritdoc />
    public async Task InterruptAttachingToTangleAsync()
    {
      await this.Client.ExecuteParameterizedCommandAsync(new Dictionary<string, object> { { "command", CommandType.InterruptAttachingToTangle } });
    }


    /// <inheritdoc />
    public void StoreTransactions(IEnumerable<TransactionTrytes> transactions)
    {
      this.Client.ExecuteParameterizedCommand(
        new Dictionary<string, object> { { "command", CommandType.StoreTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } });
    }

    /// <inheritdoc />
    public async Task StoreTransactionsAsync(IEnumerable<TransactionTrytes> transactions)
    {
      await this.Client.ExecuteParameterizedCommandAsync(
        new Dictionary<string, object> { { "command", CommandType.StoreTransactions }, { "trytes", transactions.Select(t => t.Value).ToList() } });
    }

    /// <inheritdoc />
    public List<Address> WereAddressesSpentFrom(List<Address> addresses)
    {
      var response = this.Client.ExecuteParameterizedCommand<WereAddressesSpentFromResponse>(
        new Dictionary<string, object>
          {
            { "command", CommandType.WereAddressesSpentFrom },
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
      var response = await this.Client.ExecuteParameterizedCommandAsync<WereAddressesSpentFromResponse>(
                       new Dictionary<string, object>
                         {
                           { "command", CommandType.WereAddressesSpentFrom },
                           { "addresses", addresses.Select(a => a.Value).ToList() }
                         });

      for (var i = 0; i < addresses.Count; i++)
      {
        addresses[i].SpentFrom = response.States[i];
      }

      return addresses;
    }

    /// <inheritdoc />
    public ConsistencyInfo CheckConsistency(List<Hash> tailHashes)
    {
      var response = this.Client.ExecuteParameterizedCommand<CheckConsistencyResponse>(
        new Dictionary<string, object>
          {
            { "command", CommandType.CheckConsistency },
            { "tails", tailHashes.Select(h => h.Value).ToList() }
          });

      return new ConsistencyInfo
      {
        Duration = response.Duration,
        State = response.State,
        Info = response.Info
      };
    }

    /// <inheritdoc />
    public async Task<ConsistencyInfo> CheckConsistencyAsync(List<Hash> tailHashes)
    {
      var response = await this.Client.ExecuteParameterizedCommandAsync<CheckConsistencyResponse>(
        new Dictionary<string, object>
          {
            { "command", CommandType.CheckConsistency },
            { "tails", tailHashes.Select(h => h.Value).ToList() }
          });

      return new ConsistencyInfo
      {
        Duration = response.Duration,
        State = response.State,
        Info = response.Info
      };
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

      var command = new Dictionary<string, object> { { "command", CommandType.FindTransactions } };

      foreach (var parameter in parameters)
      {
        command.Add(parameter.Key, parameter.Value.Select(value => value.Value).ToList());
      }

      return command;
    }
  }
}