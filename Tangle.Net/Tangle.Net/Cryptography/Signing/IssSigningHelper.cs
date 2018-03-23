namespace Tangle.Net.Cryptography.Signing
{
  using System;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;

  /// <inheritdoc />
  public class IssSigningHelper : ISigningHelper
  {
    /// <summary>
    /// The hashes per fragment.
    /// </summary>
    public const int HashesPerFragment = PrivateKey.FragmentLength / AbstractCurl.HashLength;

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
      var digest = new int[AbstractCurl.HashLength];

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
      var address = new int[AbstractCurl.HashLength];
      Array.Copy(digest, address, address.Length);

      this.CurlOne.Reset();
      this.CurlOne.Absorb(address);
      this.CurlOne.Squeeze(address);

      return address;
    }
  }
}