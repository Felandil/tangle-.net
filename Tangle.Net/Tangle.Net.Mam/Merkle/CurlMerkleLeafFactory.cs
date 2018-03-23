namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The curl merkle leaf factory.
  /// </summary>
  public class CurlMerkleLeafFactory : IMerkleLeafFactory
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMerkleLeafFactory"/> class.
    /// </summary>
    /// <param name="addressGenerator">
    /// The address generator.
    /// </param>
    public CurlMerkleLeafFactory(IAddressGenerator addressGenerator)
    {
      this.AddressGenerator = addressGenerator;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the address generator.
    /// </summary>
    private IAddressGenerator AddressGenerator { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <inheritdoc />
    public List<MerkleNode> Create(Seed seed, int securityLevel, int startIndex, int count)
    {
      return
        this.AddressGenerator.GetAddresses(seed, securityLevel, startIndex, count)
          .Select(address => new MerkleNode { Hash = new Hash(address.Value), Key = address.PrivateKey, Size = 1 })
          .ToList();
    }

    #endregion
  }
}