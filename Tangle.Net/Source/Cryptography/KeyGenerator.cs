namespace Tangle.Net.Source.Cryptography
{
  using System;
  using System.Collections.Generic;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Tests.Cryptography;

  /// <summary>
  /// The key generator.
  /// </summary>
  public class KeyGenerator : IKeyGenerator
  {
    #region Constants

    /// <summary>
    /// The hashes per fragment.
    /// </summary>
    public const int HashesPerFragment = PrivateKey.FragmentLength / Kerl.HashLength;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyGenerator"/> class.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    public KeyGenerator(Seed seed)
    {
      this.Seed = seed;
      this.SeedTrits = seed.ToTrits();
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the seed trits.
    /// </summary>
    public int[] SeedTrits { get; set; }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the seed.
    /// </summary>
    private Seed Seed { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The get key.
    /// </summary>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="IPrivateKey"/>.
    /// </returns>
    public IPrivateKey GetKey(int index)
    {
      return this.GetKey(index, 1);
    }

    /// <summary>
    /// The get key.
    /// </summary>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="IPrivateKey"/>.
    /// </returns>
    public IPrivateKey GetKey(int index, int securityLevel)
    {
      if (index < 0)
      {
        throw new ArgumentException("Indices must not be negative");
      }

      var subseed = Converter.AddTrits(this.SeedTrits, Converter.IntToTrits(index, 3));

      var kerl = new Kerl();
      kerl.Absorb(subseed);

      kerl.Squeeze(subseed);
      kerl.Reset();
      kerl.Absorb(subseed);

      var keyTrits = new List<int>();
      var buffer = new int[subseed.Length];

      for (var fragmentSequence = 0; fragmentSequence < securityLevel; fragmentSequence++)
      {
        for (var hashSequence = 0; hashSequence < HashesPerFragment; hashSequence++)
        {
          kerl.Squeeze(buffer);
          keyTrits.AddRange(buffer);
        }
      }

      var trytes = Converter.TritsToTrytes(keyTrits.ToArray());
      return new PrivateKey(trytes);
    }

    /// <summary>
    /// Note that this method will generate the wrong key if the input
    /// address was generated from a different key!
    /// </summary>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <returns>
    /// The <see cref="IPrivateKey"/>.
    /// </returns>
    public IPrivateKey GetKeyFor(Address address)
    {
      return this.GetKey(address.KeyIndex, address.SecurityLevel);
    }

    /// <summary>
    /// The get keys.
    /// </summary>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<IPrivateKey> GetKeys(int index, int count, int securityLevel)
    {
      if (count < 1)
      {
        throw new ArgumentException("Count must be > 0");
      }

      var keys = new List<IPrivateKey>();

      for (var i = 0; i < count; i++)
      {
        keys.Add(this.GetKey(index + i, securityLevel));
      }

      return keys;
    }

    /// <summary>
    /// The get keys.
    /// </summary>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<IPrivateKey> GetKeys(int index, int count)
    {
      return this.GetKeys(index, count, 1);
    }

    #endregion
  }
}