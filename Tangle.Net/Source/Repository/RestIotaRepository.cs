namespace Tangle.Net.Source.Repository
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using RestSharp;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The rest tangle repository.
  /// </summary>
  public class RestIotaRepository : IIotaRepository
  {
    #region Static Fields

    /// <summary>
    /// The null hash trytes.
    /// </summary>
    private static readonly string NullHashTrytes = string.Concat(Enumerable.Repeat("9", 244));

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
    /// The get balances.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <param name="threshold">
    /// The threshold.
    /// </param>
    /// <returns>
    /// The <see cref="AddressBalances"/>.
    /// </returns>
    public AddressBalances GetBalances(IEnumerable<string> addresses, int threshold)
    {
      return
        this.ExecuteParameterizedCommand<AddressBalances>(
          new Dictionary<string, object> { { "command", "getBalances" }, { "addresses", NormalizeAddresses(addresses) }, { "threshold", threshold } });
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
    /// The get transactions by addresses.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Transactions"/>.
    /// </returns>
    public Transactions GetTransactionsByAddresses(IEnumerable<string> addresses)
    {
      return
        this.ExecuteParameterizedCommand<Transactions>(
          new Dictionary<string, object> { { "command", "findTransactions" }, { "addresses", NormalizeAddresses(addresses) } });
    }

    /// <summary>
    /// The send transfers.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <param name="transfers">
    /// The transfers.
    /// </param>
    /// <param name="options">
    /// The options.
    /// </param>
    public void SendTransfers(string seed, int depth, int minWeightMagnitude, IEnumerable<Transfer> transfers, Dictionary<string, string> options)
    {
    }

    #endregion

    #region Methods

    /// <summary>
    /// The normalize addresses.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <returns>
    /// The <see cref="List{String}"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if an address contains invalid characters.
    /// </exception>
    private static List<string> NormalizeAddresses(IEnumerable<string> addresses)
    {
      var normalizedAddresses = addresses.Select(Checksum.Strip).ToList();
      if (normalizedAddresses.Any(address => !InputValidator.IsTrytes(address, InputValidator.HashLength)))
      {
        throw new ArgumentException("An address does contain non tryte characters.");
      }

      return normalizedAddresses;
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
    private T ExecuteParameterizedCommand<T>(Dictionary<string, object> parameters) where T : new()
    {
      var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
      request.AddHeader("X-IOTA-API-Version", "1");
      request.AddJsonBody(parameters);

      var response = this.Client.Execute<T>(request);

      return response.Data;
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
    /// The prepare transfers.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="transfers">
    /// The transfers.
    /// </param>
    /// <param name="options">
    /// The options.
    /// </param>
    private void PrepareTransfers(string seed, IEnumerable<Transfer> transfers, IReadOnlyDictionary<string, string> options)
    {
      var addHmac = false;
      var addedHmac = false;

      if (!InputValidator.IsTrytes(seed))
      {
        throw new ArgumentException("Seed does contain non tryte characters.");
      }

      if (options.ContainsKey("hmacKey"))
      {
        if (!InputValidator.IsTrytes(options.FirstOrDefault(o => o.Key == "hmacKey").Value))
        {
          throw new ArgumentException("HmacKey does contain non tryte characters.");
        }

        addHmac = true;
      }

      foreach (var transfer in transfers)
      {
        if (addHmac && transfer.Value > 0)
        {
          transfer.Message = NullHashTrytes + transfer.Message;
          addedHmac = true;
        }

        if (transfer.Address.Length == 90)
        {
          if (!Checksum.HasValidChecksum(transfer.Address))
          {
            throw new ArgumentException("Invalid Checksum supplied for address: " + transfer.Address);
          }
        }

        transfer.Address = Checksum.Strip(transfer.Address);
      }
    }

    #endregion
  }
}