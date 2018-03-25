namespace Tangle.Net.Cryptography
{
  using System;
  using System.Collections.Generic;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;

  /// <summary>
  /// The key generator.
  /// </summary>
  public class KeyGenerator : IKeyGenerator
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="KeyGenerator"/> class.
    /// </summary>
    /// <param name="curl">
    /// The curl.
    /// </param>
    /// <param name="signingHelper">
    /// The signing Helper.
    /// </param>
    public KeyGenerator(AbstractCurl curl, ISigningHelper signingHelper)
    {
      this.Curl = curl;
      this.SigningHelper = signingHelper;
    }

    /// <summary>
    /// Gets the curl.
    /// </summary>
    private AbstractCurl Curl { get; }

    /// <summary>
    /// Gets the signing helper.
    /// </summary>
    private ISigningHelper SigningHelper { get; }

    /// <inheritdoc />
    public AbstractPrivateKey GetKey(Seed seed, int index, int securityLevel = SecurityLevel.Low)
    {
      if (index < 0)
      {
        throw new ArgumentException("Indices must not be negative");
      }

      var subseed = this.SigningHelper.GetSubseed(seed, index);
      var keyTrits = this.SigningHelper.PrivateKeyFromSubseed(subseed, securityLevel);

      return new PrivateKey(Converter.TritsToTrytes(keyTrits), securityLevel, index, new SignatureFragmentGenerator(this.Curl), this.Curl);
    }

    /// <inheritdoc />
    /// <summary>
    /// Note that this method will generate the wrong key if the input
    /// address was generated from a different key!
    /// </summary>
    public AbstractPrivateKey GetKeyFor(Seed seed, Address address)
    {
      return this.GetKey(seed, address.KeyIndex, address.SecurityLevel);
    }
  }
}