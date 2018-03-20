namespace Tangle.Net.Cryptography
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

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
      this.Seed = new Seed(Hash.Empty.Value);
      this.SecurityLevel = Cryptography.SecurityLevel.Medium;
    }

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressGenerator"/> class.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    public AddressGenerator(Seed seed, int securityLevel = Cryptography.SecurityLevel.Medium)
    {
      this.Seed = seed;
      this.SecurityLevel = securityLevel;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the security level.
    /// </summary>
    private int SecurityLevel { get; set; }

    /// <summary>
    /// Gets or sets the seed.
    /// </summary>
    private Seed Seed { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The get address.
    /// </summary>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="Address"/>.
    /// </returns>
    public Address GetAddress(int index)
    {
      var keyGenerator = new KeyGenerator(this.Seed);
      var privateKey = keyGenerator.GetKey(index, this.SecurityLevel);

      return this.GetAddress(privateKey);
    }

    /// <inheritdoc />
    public Address GetAddress(IPrivateKey privateKey)
    {
      var digest = privateKey.Digest;

      var addressTrits = new int[Address.Length * Converter.Radix];
      var kerl = new Kerl();
      kerl.Absorb(digest.ToTrits());
      kerl.Squeeze(addressTrits);

      var address = Address.FromTrits(addressTrits);
      address.KeyIndex = digest.KeyIndex;
      address.SecurityLevel = digest.SecurityLevel;
      address.PrivateKey = privateKey;

      return address;
    }

    /// <summary>
    /// The get addresses.
    /// </summary>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<Address> GetAddresses(int startIndex, int count)
    {
      // since address generation takes very long, we will do it parallel (if there are any concerns regarding this, please communicate them)
      return Enumerable.Range(startIndex, count)
       .AsParallel()
       .Select(this.GetAddress)
       .OrderBy(x => x.KeyIndex)
       .ToList();
    }

    #endregion
  }
}