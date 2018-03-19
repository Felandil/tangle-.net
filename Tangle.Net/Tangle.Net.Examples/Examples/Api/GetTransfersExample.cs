namespace Tangle.Net.Examples.Examples.Api
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;

  /// <summary>
  /// The get transfers example.
  /// </summary>
  public class GetTransfersExample : Example<List<Bundle>>
  {
    /// <inheritdoc />
    public GetTransfersExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// GetTransfers can be used to get any bundle associated with an address on the provided seed. Use GetTransfersAsync in async context.
    /// 
    /// 1. Get the seed
    /// 2. The method takes the seed, the SecurityLevel (Medium is default for the wallet aswell)
    /// You can also specify if the bundles should be confirmed or not (true = only confirmed bundles are returned)
    /// For seeds with lots of generated addresses it sometimes makes sense to set a stop index for address generation to save time
    public override List<Bundle> Execute()
    {
      var seed = new Seed("SOMENICEANDSECURESEEDHERE");
      return this.Repository.GetTransfers(seed, SecurityLevel.Medium, true, 0, 100);
    }
  }
}