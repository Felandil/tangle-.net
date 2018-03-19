namespace Tangle.Net.Examples.Examples.Api
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository;

  /// <summary>
  /// The broadcast and store transactions example.
  /// </summary>
  public class BroadcastAndStoreTransactionsExample : Example<Task>
  {
    /// <inheritdoc />
    public BroadcastAndStoreTransactionsExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// BroadcastAndStoreTransactions can be used to attach transaction Trytes to the tangle. Use BroadcastAndStoreTransactions in sync context.
    /// Input can for example be the list returned from attachToTangle
    public override Task Execute()
    {
      var transactionTrytes = new List<TransactionTrytes>();
      return this.Repository.BroadcastAndStoreTransactionsAsync(transactionTrytes);
    }
  }
}