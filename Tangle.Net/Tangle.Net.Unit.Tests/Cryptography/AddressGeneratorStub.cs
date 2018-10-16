namespace Tangle.Net.Unit.Tests.Cryptography
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The address generator stub.
  /// </summary>
  public class AddressGeneratorStub : IAddressGenerator
  {
    #region Public Methods and Operators

    /// <inheritdoc />
    public Address GetAddress(Seed seed, int securityLevel, int index)
    {
      return new Address(Seed.Random().Value) { PrivateKey = new PrivateKeyStub() };
    }

    /// <inheritdoc />
    public async Task<Address> GetAddressAsync(Seed seed, int securityLevel, int index)
    {
      return this.GetAddress(seed, securityLevel, index);
    }

    /// <inheritdoc />
    public Address GetAddress(AbstractPrivateKey privateKey)
    {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public List<Address> GetAddresses(Seed seed, int securityLevel, int startIndex, int count)
    {
      var addresses = new List<Address>();
      for (var i = startIndex; i < startIndex + count; i++)
      {
        addresses.Add(this.GetAddress(seed, securityLevel, i));
      }

      return addresses;
    }

    /// <inheritdoc />
    public async Task<List<Address>> GetAddressesAsync(Seed seed, int securityLevel, int startIndex, int count)
    {
      return this.GetAddresses(seed, securityLevel, startIndex, count);
    }

    #endregion
  }
}