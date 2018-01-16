namespace Tangle.Net.Source.Cryptography
{
  using System;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The address generator.
  /// </summary>
  public class AddressGenerator
  {
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
    public AddressGenerator(Seed seed, int securityLevel = 2)
    {
      this.Seed = seed;
      this.SecurityLevel = securityLevel;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the security level.
    /// </summary>
    private int SecurityLevel { get; set; }

    #endregion

    #region Properties

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
      var digest = privateKey.Digest;

      var addressTrits = new int[Address.Length * Converter.Radix];
      var kerl = new Kerl();
      kerl.Absorb(digest.ToTrits());
      kerl.Squeeze(addressTrits);

      var address = Address.FromTrits(addressTrits);
      address.KeyIndex = digest.KeyIndex;
      address.SecurityLevel = digest.SecurityLevel;

      return address;
    }

    #endregion
  }
}