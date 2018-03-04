namespace Tangle.Net.Examples.Examples
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.DataTransfer;

  /// <inheritdoc />
  public class GetLatestInclusionExample : Example<InclusionStates>
  {
    /// <inheritdoc />
    public GetLatestInclusionExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// https://iota.readme.io/v1.3.0/reference#getinclusionstates. Use GetLatestInclusionAsync in async context
    /// 
    /// 1. Get all TransactionHashes to check in a list
    public override InclusionStates Execute()
    {
      var transactionHashes = new List<Hash> { new Hash("SOMETRANSACTIONHASH") };

      return this.Repository.GetLatestInclusion(transactionHashes);
    }
  }
}