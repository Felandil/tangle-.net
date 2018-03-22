namespace Tangle.Net.Cryptography
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The KeyGenerator interface.
  /// </summary>
  public interface IKeyGenerator
  {
    #region Public Methods and Operators

    /// <summary>
    /// The get key for.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    AbstractPrivateKey GetKeyFor(Seed seed,  Address address);

    /// <summary>
    /// The get key.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="IPrivateKey"/>.
    /// </returns>
    AbstractPrivateKey GetKey(Seed seed, int index, int securityLevel = SecurityLevel.Low);

    #endregion
  }
}