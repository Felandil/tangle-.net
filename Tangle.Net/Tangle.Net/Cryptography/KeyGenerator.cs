namespace Tangle.Net.Cryptography
{
  using System;
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The key generator.
  /// </summary>
  public class KeyGenerator : IKeyGenerator
  {
    /// <summary>
    /// The hashes per fragment.
    /// </summary>
    public const int HashesPerFragment = PrivateKey.FragmentLength / AbstractCurl.HashLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyGenerator"/> class.
    /// </summary>
    /// <param name="curl">
    /// The curl.
    /// </param>
    public KeyGenerator(AbstractCurl curl)
    {
      this.Curl = curl;
    }

    /// <summary>
    /// Gets the curl.
    /// </summary>
    private AbstractCurl Curl { get; }

    /// <inheritdoc />
    public AbstractPrivateKey GetKey(Seed seed, int index, int securityLevel = SecurityLevel.Low)
    {
      if (index < 0)
      {
        throw new ArgumentException("Indices must not be negative");
      }

      var subseed = Converter.AddTrits(seed.ToTrits(), Converter.IntToTrits(index, 27));

      this.Curl.Reset();
      this.Curl.Absorb(subseed);
      this.Curl.Squeeze(subseed);
      this.Curl.Reset();
      this.Curl.Absorb(subseed);

      var keyTrits = new List<int>();
      var buffer = new int[subseed.Length];

      for (var fragmentSequence = 0; fragmentSequence < securityLevel; fragmentSequence++)
      {
        for (var hashSequence = 0; hashSequence < HashesPerFragment; hashSequence++)
        {
          this.Curl.Squeeze(buffer);
          keyTrits.AddRange(buffer);
        }
      }

      var trytes = Converter.TritsToTrytes(keyTrits.ToArray());
      return new PrivateKey(trytes, securityLevel, index, new SignatureFragmentGenerator(this.Curl), this.Curl);
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