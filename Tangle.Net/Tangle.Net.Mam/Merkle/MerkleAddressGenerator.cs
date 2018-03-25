namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;

  /// <summary>
  /// The merkle address generator.
  /// </summary>
  public class MerkleAddressGenerator : IAddressGenerator
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MerkleAddressGenerator"/> class.
    /// </summary>
    /// <param name="signing">
    /// The signing.
    /// </param>
    public MerkleAddressGenerator(ISigningHelper signing)
    {
      this.Signing = signing;
    }

    /// <summary>
    /// The default.
    /// </summary>
    public static MerkleAddressGenerator Default =>
      new MerkleAddressGenerator(new IssSigningHelper(new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27)));

    /// <summary>
    /// Gets the signing.
    /// </summary>
    private ISigningHelper Signing { get; }


    /// <inheritdoc />
    public Address GetAddress(Seed seed, int securityLevel, int index)
    {
      var subseed = this.Signing.GetSubseed(seed, index);
      var digest = this.Signing.DigestFromSubseed(subseed, securityLevel);
      var addressTrits = this.Signing.AddressFromDigest(digest);
      var privateKeyTrits = this.Signing.PrivateKeyFromSubseed(subseed, securityLevel);

      var privateKey = new MerklePrivateKey(Converter.TritsToTrytes(privateKeyTrits), securityLevel, index, Converter.TritsToTrytes(digest));
      return new Address(Converter.TritsToTrytes(addressTrits)) { PrivateKey = privateKey, KeyIndex = index, SecurityLevel = securityLevel };
    }

    /// <inheritdoc />
    public Address GetAddress(AbstractPrivateKey privateKey)
    {
      return null;
    }

    /// <inheritdoc />
    public List<Address> GetAddresses(Seed seed, int securityLevel, int startIndex, int count)
    {
      var addresses = new List<Address>();

      for (var i = 0; i < count; i++)
      {
        addresses.Add(this.GetAddress(seed, securityLevel, startIndex + i));
      }

      return addresses;
    }
  }
}