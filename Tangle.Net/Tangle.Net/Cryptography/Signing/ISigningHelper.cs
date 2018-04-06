namespace Tangle.Net.Cryptography.Signing
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The IssSigningHelper interface.
  /// </summary>
  public interface ISigningHelper
  {
    /// <summary>
    /// The digest from subseed.
    /// </summary>
    /// <param name="subseed">
    /// The subseed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    int[] DigestFromSubseed(int[] subseed, int securityLevel);

    /// <summary>
    /// The get subseed.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    int[] GetSubseed(Seed seed, int index);

    /// <summary>
    /// The address from digest.
    /// </summary>
    /// <param name="digest">
    /// The digest.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    int[] AddressFromDigest(int[] digest);

    /// <summary>
    /// The private key from subseed.
    /// </summary>
    /// <param name="subseed">
    /// The subseed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    int[] PrivateKeyFromSubseed(int[] subseed, int securityLevel);

    /// <summary>
    /// The signature.
    /// </summary>
    /// <param name="hashTrits">
    /// The hash trits.
    /// </param>
    /// <param name="key">
    /// The key.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    int[] Signature(int[] hashTrits, int[] key);

    /// <summary>
    /// The checksum security.
    /// </summary>
    /// <param name="hash">
    /// The hash.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    int ChecksumSecurity(int[] hash);

    /// <summary>
    /// The digest from signature.
    /// </summary>
    /// <param name="hash">
    /// The hash.
    /// </param>
    /// <param name="signature">
    /// The signature.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    int[] DigestFromSignature(int[] hash, int[] signature);
  }
}