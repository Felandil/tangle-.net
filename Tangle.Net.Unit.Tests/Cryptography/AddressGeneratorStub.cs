namespace Tangle.Net.Unit.Tests.Cryptography
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The address generator stub.
  /// </summary>
  public class AddressGeneratorStub : IAddressGenerator
  {
    #region Public Methods and Operators

    /// <summary>
    /// The get address.
    /// </summary>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="Address"/>.
    /// </returns>
    public Address GetAddress(int index)
    {
      return new Address(Seed.Random().Value) { PrivateKey = new PrivateKeyStub() };
    }

    /// <summary>
    /// The get addresses.
    /// </summary>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<Address> GetAddresses(int startIndex, int count)
    {
      var addresses = new List<Address>();
      for (var i = startIndex; i < startIndex + count; i++)
      {
        addresses.Add(this.GetAddress(i));
      }

      return addresses;
    }

    #endregion
  }
}