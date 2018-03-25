namespace Tangle.Net.ProofOfWork
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Client;
  using Tangle.Net.Repository.Responses;

  /// <summary>
  /// The rest po w service.
  /// </summary>
  public class RestPoWService : IPoWService
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RestPoWService"/> class.
    /// </summary>
    /// <param name="client">
    /// The client.
    /// </param>
    public RestPoWService(IIotaClient client)
    {
      this.Client = client;
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    private IIotaClient Client { get; }

    /// <inheritdoc />
    public List<Transaction> DoPoW(Hash branchTransaction, Hash trunkTransaction, List<Transaction> transactions, int minWeightMagnitude = 18)
    {
      var result = this.Client.ExecuteParameterizedCommand<AttachToTangleResponse>(
        new Dictionary<string, object>
          {
            { "command", CommandType.AttachToTangle },
            { "trunkTransaction", trunkTransaction.ToString() },
            { "branchTransaction", branchTransaction.ToString() },
            { "minWeightMagnitude", minWeightMagnitude },
            { "trytes", transactions.Select(transaction => transaction.ToTrytes().Value).ToList() }
          });

      return result.Trytes.Select(t => Transaction.FromTrytes(new TransactionTrytes(t))).ToList();
    }

    /// <inheritdoc />
    public async Task<List<Transaction>> DoPoWAsync(Hash branchTransaction, Hash trunkTransaction, List<Transaction> transactions, int minWeightMagnitude = 18)
    {
      var result = await this.Client.ExecuteParameterizedCommandAsync<AttachToTangleResponse>(
                     new Dictionary<string, object>
                       {
                         { "command", CommandType.AttachToTangle },
                         { "trunkTransaction", trunkTransaction.ToString() },
                         { "branchTransaction", branchTransaction.ToString() },
                         { "minWeightMagnitude", minWeightMagnitude },
                         { "trytes", transactions.Select(transaction => transaction.ToTrytes().Value).ToList() }
                       });

      return result.Trytes.Select(t => Transaction.FromTrytes(new TransactionTrytes(t))).ToList();
    }
  }
}