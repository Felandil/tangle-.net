namespace Tangle.Net.Examples.Examples
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;

  /// <inheritdoc />
  public class GetNewAddressesExample : Example<List<Address>>
  {
    /// <inheritdoc />
    public GetNewAddressesExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// GetNewAddresses generates new addresses for the provided seed. Use GetNewAddressesAsync in async context.
    /// 
    /// 1. Get the seed
    /// 2. Generate x addresses (10 in the example) with a given SecurityLevel
    /// 
    /// NOTE: AFTER A SNAPSHOT THE METHOD WILL RETURN ALREADY USED ADDRESSES IF THEY ARE NOT REATTACHED BEFOREHAND!
    public override List<Address> Execute()
    {
      var seed = new Seed("SOMENICEANDSECURESEEDGOESHERE");
      return this.Repository.GetNewAddresses(seed, 0, 10, SecurityLevel.Medium);
    }
  }
}