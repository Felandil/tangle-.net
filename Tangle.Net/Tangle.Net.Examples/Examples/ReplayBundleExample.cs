namespace Tangle.Net.Examples.Examples
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository;

  /// <inheritdoc />
  public class ReplayBundleExample : Example<List<TransactionTrytes>>
  {
    /// <inheritdoc />
    public ReplayBundleExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// ReplayBundle finds a already sent bundle, loads it, does the PoW once again and then reattaches it to the tangle. Use ReplayBundleAsync in async context.
    /// 
    /// 1. Get the tail Transaction Hash of the bundle. The tailTransaction has index = 0
    /// 2. Replay the bundle. The example uses a MWM of 14
    public override List<TransactionTrytes> Execute()
    {
      var tailTransactionHash = new Hash("SOMETAILTRANSACTIONHASH");

      return this.Repository.ReplayBundle(tailTransactionHash, 27, 14);
    }
  }
}