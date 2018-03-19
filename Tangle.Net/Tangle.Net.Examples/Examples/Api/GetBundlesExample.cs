namespace Tangle.Net.Examples.Examples.Api
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository;

  /// <inheritdoc />
  public class GetBundlesExample : Example<List<Bundle>>
  {
    /// <inheritdoc />
    public GetBundlesExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// Use GetBundles to get the corresponding bundles for given transactionHashes. Use GetBundlesAsync in async context.
    /// 
    /// 1. Get all transaction Hashes. It can be tail and non tail transactions
    /// 2. Call the method and specify whether the bundles should be confirmed or not
    public override List<Bundle> Execute()
    {
      var transactionHashes = new List<Hash> { new Hash("SOMETRANSACTIONHASHHERE"), new Hash("ONEMOREHASHHERE") };
      return this.Repository.GetBundles(transactionHashes, false);
    }
  }
}