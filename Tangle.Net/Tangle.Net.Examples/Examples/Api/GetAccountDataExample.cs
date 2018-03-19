namespace Tangle.Net.Examples.Examples.Api
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Responses;

  /// <inheritdoc />
  public class GetAccountDataExample : Example<GetAccountDataResponse>
  {
    /// <inheritdoc />
    public GetAccountDataExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// GetAccountData can be used to get all information associated with a seed. Use GetAccountDataAsync in async context.
    /// 
    /// 1. Get the seed
    /// 2. Specify whether or not confirmed bundles should be included. Set a start index for address generation.
    /// 
    /// NOTE: Depending on how many transactions are associated with the given seed, this method can be very time consuming!
    public override GetAccountDataResponse Execute()
    {
      var seed = new Seed("SOMENICEANDSECURESEEDHERE");
      return this.Repository.GetAccountData(seed, true, SecurityLevel.Medium, 0);
    }
  }
}