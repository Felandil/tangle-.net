namespace Tangle.Net.Unit.Tests.Cryptography
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The key generator stub.
  /// </summary>
  internal class KeyGeneratorStub : IKeyGenerator
  {
    #region Public Methods and Operators

    #endregion

    /// <inheritdoc />
    public AbstractPrivateKey GetKeyFor(Seed seed, Address address)
    {
      return new PrivateKeyStub();
    }

    /// <inheritdoc />
    public AbstractPrivateKey GetKey(Seed seed, int index, int securityLevel = SecurityLevel.Low)
    {
      return null;
    }
  }
}