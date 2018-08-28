namespace Tangle.Net.ProofOfWork.Service
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using RestSharp;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Responses;

  /// <summary>
  /// Implementation of the PoW Api provided by https://powsrv.io/
  /// </summary>
  public class PoWSrvService : IPoWService
  {
    public PoWSrvService(string apiKey = null)
    {
      this.Client = new RestClient("https://api.powsrv.io:443/");
      this.ApiKey = apiKey;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PoWSrvService"/> class.
    /// </summary>
    /// <param name="client">
    /// The client.
    /// </param>
    /// <param name="apiKey">
    /// The api Key.
    /// </param>
    public PoWSrvService(RestClient client, string apiKey = null)
    {
      this.Client = client;
      this.ApiKey = apiKey;
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    private RestClient Client { get; }

    private string ApiKey { get; }

    /// <inheritdoc />
    public List<Transaction> DoPoW(Hash branchTransaction, Hash trunkTransaction, List<Transaction> transactions, int minWeightMagnitude = 14)
    {
      var response = this.Client.Execute<AttachToTangleResponse>(
        this.CreateRequest(branchTransaction, trunkTransaction, transactions, minWeightMagnitude));

      return response.Data.Trytes.Select(t => Transaction.FromTrytes(new TransactionTrytes(t))).Reverse().ToList();
    }

    /// <inheritdoc />
    public async Task<List<Transaction>> DoPoWAsync(
      Hash branchTransaction,
      Hash trunkTransaction,
      List<Transaction> transactions,
      int minWeightMagnitude = 14)
    {
      var response = await this.Client.ExecuteTaskAsync<AttachToTangleResponse>(
                       this.CreateRequest(branchTransaction, trunkTransaction, transactions, minWeightMagnitude));

      return response.Data.Trytes.Select(t => Transaction.FromTrytes(new TransactionTrytes(t))).Reverse().ToList();
    }


    private RestRequest CreateRequest(TryteString branchTransaction, TryteString trunkTransaction, IEnumerable<Transaction> transactions, int minWeightMagnitude)
    {
      var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
      request.AddHeader("X-IOTA-API-Version", "1");

      if (!string.IsNullOrEmpty(this.ApiKey))
      {
        request.AddHeader("Authorization", $"powsrv-token {this.ApiKey}");
      }

      request.AddJsonBody(
        new Dictionary<string, object>
          {
            { "command", CommandType.AttachToTangle },
            { "trunkTransaction", trunkTransaction.Value },
            { "branchTransaction", branchTransaction.Value },
            { "minWeightMagnitude", minWeightMagnitude },
            { "trytes", transactions.Select(transaction => transaction.ToTrytes().Value).Reverse().ToList() }
          });

      return request;
    }
  }
}