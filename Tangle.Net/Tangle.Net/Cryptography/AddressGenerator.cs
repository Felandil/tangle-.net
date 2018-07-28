namespace Tangle.Net.Cryptography
{
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;

  /// <summary>
  /// The address generator.
  /// </summary>
  public class AddressGenerator : IAddressGenerator
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AddressGenerator"/> class.
    /// </summary>
    public AddressGenerator()
    {
      this.Curl = new Kerl();
      this.KeyGenerator = new KeyGenerator(new Kerl(), new IssSigningHelper());
    }

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressGenerator"/> class.
    /// </summary>
    /// <param name="curl">
    /// The curl.
    /// </param>
    /// <param name="keyGenerator">
    /// The key Generator.
    /// </param>
    public AddressGenerator(AbstractCurl curl, IKeyGenerator keyGenerator)
    {
      this.Curl = curl;
      this.KeyGenerator = keyGenerator;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the curl.
    /// </summary>
    private AbstractCurl Curl { get; }

    /// <summary>
    /// Gets the key generator.
    /// </summary>
    private IKeyGenerator KeyGenerator { get; }

    #endregion

    #region Public Methods and Operators

    /// <inheritdoc />
    public Address GetAddress(Seed seed, int securityLevel, int index)
    {
      var privateKey = this.KeyGenerator.GetKey(seed, index, securityLevel);
      return this.GetAddress(privateKey);
    }

    /// <inheritdoc />
    public Address GetAddress(AbstractPrivateKey privateKey)
    {
      var digest = privateKey.Digest;

      var addressTrits = new int[Address.Length * Converter.Radix];
      this.Curl.Reset();
      this.Curl.Absorb(digest.ToTrits());
      this.Curl.Squeeze(addressTrits);
      this.Curl.Reset();

      var address = Address.FromTrits(addressTrits);
      address.KeyIndex = digest.KeyIndex;
      address.SecurityLevel = digest.SecurityLevel;
      address.PrivateKey = privateKey;

      return address;
    }

    /// <inheritdoc />
    public List<Address> GetAddresses(Seed seed, int securityLevel, int startIndex, int count)
    {
      // since address generation takes very long, we will do it parallel (if there are any concerns regarding this, please communicate them)
      return Enumerable.Range(startIndex, count)
       .Select(i => this.GetAddress(seed, securityLevel, i))
       .AsParallel()
       .OrderBy(x => x.KeyIndex)
       .ToList();
    }

    #endregion
  }
}