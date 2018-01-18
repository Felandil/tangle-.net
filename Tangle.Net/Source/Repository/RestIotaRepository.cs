namespace Tangle.Net.Source.Repository
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net;

  using RestSharp;

  using Tangle.Net.Source.DataTransfer;
  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Repository.Responses;

  /// <summary>
  /// The rest tangle repository.
  /// </summary>
  public class RestIotaRepository : IIotaRepository
  {
    #region Fields

    /// <summary>
    /// The valid find transaction parameters.
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
    public RestIotaRepository(IRestClient client)
    {
      this.Client = client;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    private IRestClient Client { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The attach to tangle.
    /// </summary>
    /// <param name="branchTransaction">
    /// The branch transaction.
    /// </param>
    /// <param name="trunkTransaction">
    /// The trunk transaction.
    /// </param>
    /// <param name="transaction">
    /// The transaction.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public TryteString AttachToTangle(Hash branchTransaction, Hash trunkTransaction, Transaction transaction, int minWeightMagnitude = 18)
    {
      var result =
        this.ExecuteParameterizedCommand<AttachToTangleResponse>(
          new Dictionary<string, object>
            {
              { "command", NodeCommands.AttachToTangle }, 
              { "trunkTransaction", trunkTransaction.ToString() }, 
              { "branchTransaction", branchTransaction.ToString() }, 
              { "minWeightMagnitude", minWeightMagnitude }, 
              { "trytes", transaction.ToTrytes() }
            });

      return new TryteString(result.Trytes);
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

      var command = new Dictionary<string, object> { { "command", NodeCommands.FindTransactions } };

      foreach (var parameter in parameters)
      {
        command.Add(parameter.Key, parameter.Value.Select(value => value.Value).ToString());
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
              { "command", NodeCommands.GetBalances }, 
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
    /// The get neighbors.
    /// </summary>
    /// <returns>
    /// The <see cref="NeighborList"/>.
    /// </returns>
    public NeighborList GetNeighbors()
    {
      return this.ExecuteParameterlessCommand<NeighborList>(NodeCommands.GetNeighbors);
    }

    /// <summary>
    /// The get node info.
    /// </summary>
    /// <returns>
    /// The <see cref="NodeInfo"/>.
    /// </returns>
    public NodeInfo GetNodeInfo()
    {
      return this.ExecuteParameterlessCommand<NodeInfo>(NodeCommands.GetNodeInfo);
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
          new Dictionary<string, object> { { "command", NodeCommands.GetTransactionsToApprove }, { "depth", depth } });

      return new TransactionsToApprove
               {
                 BranchTransaction = new Hash(result.BranchTransaction), 
                 TrunkTransaction = new Hash(result.TrunkTransaction), 
                 Duration = result.Duration
               };
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
          new Dictionary<string, object> { { "command", NodeCommands.GetTrytes }, { "hashes", hashes.Select(h => h.Value).ToList() } });

      return result.Trytes.Select(tryte => new TransactionTrytes(tryte)).ToList();
    }

    /// <summary>
    /// The interrupt attaching to tangle.
    /// </summary>
    public void InterruptAttachingToTangle()
    {
      this.Client.Execute(CreateRequest(new Dictionary<string, object> { { "command", NodeCommands.InterruptAttachingToTangle } }));
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
        throw new IriApiException(string.Format("Command {0} failed!", parameters.First(p => p.Key == "command").Value));
      }

      if (response.ErrorException != null)
      {
        throw new IriApiException(
          string.Format("Command {0} failed! See inner exception for details.", parameters.First(p => p.Key == "command").Value), 
          response.ErrorException);
      }

      throw new IriApiException(string.Format("Command {0} failed!", parameters.First(p => p.Key == "command").Value));
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

    #endregion
  }
}