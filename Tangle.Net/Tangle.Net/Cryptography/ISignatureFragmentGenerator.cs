namespace Tangle.Net.Cryptography
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The SignatureFragmentGenerator interface.
  /// </summary>
  public interface ISignatureFragmentGenerator
  {
    /// <summary>
    /// The generate.
    /// </summary>
    /// <param name="privateKey">
    /// The private key.
    /// </param>
    /// <param name="hash">
    /// The hash.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<Fragment> Generate(AbstractPrivateKey privateKey, Hash hash);
  }
}