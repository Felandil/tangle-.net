namespace Tangle.Net.Cryptography.Signing
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The SignatureValidator interface.
  /// </summary>
  public interface ISignatureValidator
  {
    /// <summary>
    /// The validate fragments.
    /// </summary>
    /// <param name="fragments">
    /// The fragments.
    /// </param>
    /// <param name="hash">
    /// The hash.
    /// </param>
    /// <param name="publicKey">
    /// The public key.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    bool ValidateFragments(List<Fragment> fragments, Hash hash, TryteString publicKey);
  }
}