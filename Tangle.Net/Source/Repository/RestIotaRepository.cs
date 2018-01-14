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
    /// <param name="security">
    /// The security.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder address.
    /// </param>
    /// <param name="transfers">
    /// The transfers.
    /// </param>
    /// <param name="inputs">
    /// The inputs.
    /// </param>
    /// <param name="validateInputs">
    /// The validate inputs.
    /// </param>
    /// <param name="validateInputAddresses">
    /// The validate input addresses.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<Tuple<Transaction, bool>> SendTransfers(
      string seed, 
      int depth, 
      int minWeightMagnitude, 
      int security, 
      string remainderAddress, 
      IReadOnlyCollection<Transfer> transfers, 
      List<Input> inputs, 
      bool validateInputs, 
      bool validateInputAddresses)
    {
      var trytes = this.PrepareTransfers(seed, security, remainderAddress, transfers, inputs, validateInputs);

      if (validateInputAddresses)
      {
        // ValidateTransfersAddresses(seed, security, trytes); TODO
      }

      var transactions = this.SendTrytes(trytes.ToArray(), depth, minWeightMagnitude);

      var successful = new bool[transactions.Count];
      var result = new List<Tuple<Transaction, bool>>();

      for (var i = 0; i < transactions.Count; i++)
      {
        // final FindTransactionResponse response = findTransactionsByBundles(transactions[i].Bundle); TODO: get bundle status
        // successful[i] = response.getHashes().length != 0;

        result.Add(new Tuple<Transaction, bool>(transactions[i], true));
      }

      return result;
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
    /// The add remainder.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="security">
    /// The security.
    /// </param>
    /// <param name="item1">
    /// The item 1.
    /// </param>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="tag">
    /// The tag.
    /// </param>
    /// <param name="totalValue">
    /// The total value.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder address.
    /// </param>
    /// <param name="signatureFragments">
    /// The signature fragments.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    private List<string> AddRemainder(
      string seed, 
      int security, 
      List<Input> item1, 
      Bundle bundle, 
      string tag, 
      long totalValue, 
      string remainderAddress, 
      List<string> signatureFragments)
    {
      throw new NotImplementedException();
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
    /// The get inputs and balance.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="security">
    /// The security.
    /// </param>
    /// <param name="i">
    /// The i.
    /// </param>
    /// <param name="i1">
    /// The i 1.
    /// </param>
    /// <param name="totalValue">
    /// The total value.
    /// </param>
    /// <returns>
    /// The <see cref="Tuple"/>.
    /// </returns>
    private Tuple<List<Input>, long> GetInputsAndBalance(string seed, int security, int i, int i1, long totalValue)
    {
      return new Tuple<List<Input>, long>(new List<Input>(), 100);
    }

    /// <summary>
    /// The prepare transfers.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="security">
    /// The security.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder Address.
    /// </param>
    /// <param name="transfers">
    /// The transfers.
    /// </param>
    /// <param name="inputs">
    /// The inputs.
    /// </param>
    /// <param name="validateInputs">
    /// The validate inputs.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    private List<string> PrepareTransfers(
      string seed, 
      int security, 
      string remainderAddress, 
      IReadOnlyCollection<Transfer> transfers, 
      List<Input> inputs, 
      bool validateInputs)
    {
      if (!InputValidator.IsTrytes(seed))
      {
        throw new ArgumentException("Seed does contain non tryte characters.");
      }

      if (!InputValidator.AreValidInputs(inputs))
      {
        throw new ArgumentException("One or more inputs are malformed.");
      }

      foreach (var transfer in transfers)
      {
        if (transfer.Address.Length == 90)
        {
          if (!Checksum.HasValidChecksum(transfer.Address))
          {
            throw new ArgumentException("Invalid Checksum supplied for address: " + transfer.Address);
          }
        }

        transfer.Address = Checksum.Strip(transfer.Address);
      }

      if (!InputValidator.IsTransfersArray(transfers))
      {
        throw new ArgumentException("Malformed transfers");
      }

      var bundle = new Bundle();
      var signatureFragments = new List<string>();
      long totalValue = 0;
      var tag = string.Empty;

      foreach (var transfer in transfers)
      {
        var signatureMessageLength = 1;
        if (transfer.Message.Length > 2187)
        {
          signatureMessageLength += (int)Math.Floor((decimal)transfer.Message.Length / 2187);

          var message = transfer.Message;
          while (message.Length > 0)
          {
            var fragment = message.Substring(0, message.Length >= 2187 ? 2187 : message.Length);
            message = message.Substring(2187, message.Length >= 2187 ? message.Length - 2187 : message.Length);
            fragment = fragment.FillTrytes(2187);

            signatureFragments.Add(fragment);
          }
        }
        else
        {
          var fragment = transfer.Message.FillTrytes(2187);
          signatureFragments.Add(fragment);
        }

        tag = transfer.Tag.FillTrytes(27);
        var timestamp = (long)Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        bundle.AddEntry(signatureMessageLength, transfer.Address, transfer.Value, tag, timestamp);
        totalValue += transfer.Value;
      }

      if (totalValue > 0)
      {
        if (inputs.Count > 0)
        {
          if (!validateInputs)
          {
            return this.AddRemainder(seed, security, inputs, bundle, tag, totalValue, remainderAddress, signatureFragments);
          }

          var inputAddresses = inputs.Select(input => input.Address).ToList();
          var balances = this.GetBalances(inputAddresses, 100);

          var inputsWithBalanceOnAddress = new List<Input>();
          long totalBalance = 0;

          for (var i = 0; i < balances.Balances.Count; i++)
          {
            if (balances.Balances[i] <= 0)
            {
              continue;
            }

            totalBalance += balances.Balances[i];
            inputs[i].Balance = balances.Balances[i];
            inputsWithBalanceOnAddress.Add(inputs[i]);

            if (totalBalance >= totalValue)
            {
              // enough value reached, we can skip further calc
              break;
            }
          }

          if (totalValue > totalBalance)
          {
            throw new InsufficientBalanceException();
          }

          return this.AddRemainder(seed, security, inputsWithBalanceOnAddress, bundle, tag, totalValue, remainderAddress, signatureFragments);
        }

        var inputsAndBalance = this.GetInputsAndBalance(seed, security, 0, 0, totalValue);
        return this.AddRemainder(seed, security, inputsAndBalance.Item1, bundle, tag, totalValue, remainderAddress, signatureFragments);
      }

      bundle.Finalize();
      bundle.AddTrytes(signatureFragments);

      var bundleTrytes = bundle.Transactions.Select(transaction => transaction.ToTrytes()).ToList();
      bundleTrytes.Reverse();

      return bundleTrytes;
    }

    /// <summary>
    /// The send trytes.
    /// </summary>
    /// <param name="toArray">
    /// The to array.
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
    /// <exception cref="NotImplementedException">
    /// </exception>
    private List<Transaction> SendTrytes(string[] toArray, int depth, int minWeightMagnitude)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}