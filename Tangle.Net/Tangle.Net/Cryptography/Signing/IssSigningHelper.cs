namespace Tangle.Net.Cryptography.Signing
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <inheritdoc />
  public class IssSigningHelper : ISigningHelper
  {
    /// <summary>
    /// The hashes per fragment.
    /// </summary>
    public const int HashesPerFragment = PrivateKey.FragmentLength / Constants.TritHashLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="IssSigningHelper"/> class.
    /// </summary>
    public IssSigningHelper()
    {
      this.CurlOne = new Kerl();
      this.CurlTwo = new Kerl();
      this.CurlThree = new Kerl();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IssSigningHelper"/> class.
    /// </summary>
    /// <param name="curlOne">
    /// The curlOne.
    /// </param>
    /// <param name="curlTwo">
    /// The curl Two.
    /// </param>
    /// <param name="curlThree">
    /// The curl Three.
    /// </param>
    public IssSigningHelper(AbstractCurl curlOne, AbstractCurl curlTwo, AbstractCurl curlThree)
    {
      this.CurlOne = curlOne;
      this.CurlTwo = curlTwo;
      this.CurlThree = curlThree;
    }

    /// <summary>
    /// Gets the curlOne.
    /// </summary>
    private AbstractCurl CurlOne { get; }

    /// <summary>
    /// Gets the curl two.
    /// </summary>
    private AbstractCurl CurlTwo { get; }

    /// <summary>
    /// Gets the curl three.
    /// </summary>
    private AbstractCurl CurlThree { get; }

    /// <inheritdoc />
    public int[] DigestFromSubseed(int[] subseed, int securityLevel)
    {
      var length = securityLevel * HashesPerFragment;
      var digest = new int[Constants.TritHashLength];

      this.CurlOne.Reset();
      this.CurlTwo.Reset();
      this.CurlThree.Reset();

      this.CurlOne.Absorb(subseed);

      for (var i = 0; i < length; i++)
      {
        this.CurlOne.Squeeze(digest);

        for (var k = 0; k < Hash.MaxTryteValue - Hash.MinTryteValue + 1; k++)
        {
          this.CurlTwo.Reset();
          this.CurlTwo.Absorb(digest);
          this.CurlTwo.Squeeze(digest);
        }

        this.CurlThree.Absorb(digest);
      }

      this.CurlThree.Squeeze(digest);

      return digest;
    }

    /// <inheritdoc />
    public int[] GetSubseed(Seed seed, int index)
    {
      var subseed = Converter.AddTrits(seed.ToTrits(), Converter.IntToTrits(index, 27));

      this.CurlOne.Reset();
      this.CurlOne.Absorb(subseed);
      this.CurlOne.Squeeze(subseed);

      return subseed;
    }

    /// <inheritdoc />
    public int[] AddressFromDigest(int[] digest)
    {
      var address = new int[Constants.TritHashLength];
      Array.Copy(digest, address, address.Length);

      this.CurlOne.Reset();
      this.CurlOne.Absorb(address);
      this.CurlOne.Squeeze(address);

      return address;
    }

    /// <inheritdoc />
    public int[] PrivateKeyFromSubseed(int[] subseed, int securityLevel)
    {
      var keyLength = securityLevel * PrivateKey.FragmentLength;
      var keyTrits = new int[keyLength];
      var actualKeyTrits = new List<int>();

      this.CurlOne.Reset();
      this.CurlOne.Absorb(subseed);
      this.CurlOne.Squeeze(keyTrits);

      for (var i = 0; i < keyLength / Constants.TritHashLength; i++)
      {
        var offset = i * Constants.TritHashLength;
        this.CurlOne.Reset();
        this.CurlOne.Absorb(keyTrits.Skip(offset).Take(Constants.TritHashLength).ToArray());

        actualKeyTrits.AddRange(this.CurlOne.Rate(Constants.TritHashLength));
      }

      return actualKeyTrits.ToArray();
    }

    /// <inheritdoc />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
    public int[] Signature(int[] hashTrits, int[] key)
    {
      var signature = new List<int>();

      for (var i = 0; i < key.Length / Constants.TritHashLength; i++)
      {
        var buffer = key.Skip(i * Constants.TritHashLength).Take(Constants.TritHashLength).ToArray();
        for (var k = 0; k < Hash.MaxTryteValue - (hashTrits[i * 3] + hashTrits[i * 3 + 1] * 3 + hashTrits[i * 3 + 2] * 9); k++)
        {
          this.CurlOne.Reset();
          this.CurlOne.Absorb(buffer);
          buffer = this.CurlOne.Rate(Constants.TritHashLength);
        }

        signature.AddRange(buffer);
      }

      return signature.ToArray();
    }

    /// <inheritdoc />
    public int ChecksumSecurity(int[] hash)
    {
      if (hash.Take(Constants.TritHashLength / 3).Sum() == 0)
      {
        return 1;
      }

      if (hash.Take(2 * (Constants.TritHashLength / 3)).Sum() == 0)
      {
        return 2;
      }

      return hash.Sum() == 0 ? 3 : 0;
    }

    /// <inheritdoc />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
    public int[] DigestFromSignature(int[] hash, int[] signature)
    {
      var buffer = new List<int>();

      for (var i = 0; i < (signature.Length / Constants.TritHashLength); i++)
      {
        var chunk = signature.Skip(i * Constants.TritHashLength).Take(Constants.TritHashLength).ToArray();

        for (var j = 0; j < (hash[i * 3] + hash[i * 3 + 1] * 3 + hash[i * 3 + 2] * 9) - Hash.MinTryteValue; j++)
        {
          this.CurlOne.Reset();
          this.CurlOne.Absorb(chunk);
          buffer.AddRange(this.CurlOne.Rate(Constants.TritHashLength));
        }
      }

      this.CurlOne.Reset();
      this.CurlOne.Absorb(buffer.ToArray());

      return this.CurlOne.Rate(Constants.TritHashLength);
    }
  }
}