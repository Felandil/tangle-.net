namespace Tangle.Net.Cryptography
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The AddressGenerator interface.
  /// </summary>
  public interface IAddressGenerator
  {
    #region Public Methods and Operators

    /// <summary>
    /// The get address.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security Level.
    /// </param>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="Address"/>.
    /// </returns>
    Address GetAddress(Seed seed, int securityLevel, int index);

    /// <summary>
    /// The get address.
    /// </summary>
    /// <param name="privateKey">
    /// The private key.
    /// </param>
    /// <returns>
    /// The <see cref="Address"/>.
    /// </returns>
    Address GetAddress(AbstractPrivateKey privateKey);

    /// <summary>
    /// The get addresses.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security Level.
    /// </param>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<Address> GetAddresses(Seed seed, int securityLevel, int startIndex, int count);

    #endregion
  }
}