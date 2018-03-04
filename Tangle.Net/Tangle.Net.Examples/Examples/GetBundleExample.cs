namespace Tangle.Net.Examples.Examples
{
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;

  /// <summary>
  /// The get bundle example.
  /// </summary>
  public class GetBundleExample : Example<Bundle>
  {
    /// <inheritdoc />
    public GetBundleExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// GetBundle gets a bundle via tail Transaction and validates it. If you don't have the tail Transaction Hash, consider using GetBundles. Use GetBundleAsync in async context
    public override Bundle Execute()
    {
      var tailTransactionHash = new Hash("SOMETAILTRANSACTIONHASH");
      return this.Repository.GetBundle(tailTransactionHash);
    }
  }
}