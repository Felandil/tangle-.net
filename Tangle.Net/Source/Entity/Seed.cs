namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Linq;
  using System.Security.Cryptography;

  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The seed.
  /// </summary>
  public class Seed : TryteString
  {
    #region Constants

    /// <summary>
    /// The length.
    /// </summary>
    public new const int Length = 81;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Seed"/> class.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if the seed has an incorrect length
    /// </exception>
    public Seed(string seed)
      : base(seed)
    {
      if (seed.Length != Length)
      {
        throw new ArgumentException("Seed must be of length 81");
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The random.
    /// </summary>
    /// <returns>
    /// The <see cref="Seed"/>.
    /// </returns>
    public static Seed Random()
    {
      string seed;

      using (var rnd = new RNGCryptoServiceProvider())
      {
        seed = new string(Enumerable.Repeat(AsciiToTrytes.TryteAlphabet, Length).Select(s => s[GetCryptoInt(rnd, s.Length)]).ToArray());
      }

      return new Seed(seed);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Return cryptographically strong random Int value
    /// </summary>
    /// <param name="rnd">
    /// The rnd.
    /// </param>
    /// <param name="max">
    /// The max.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int GetCryptoInt(RandomNumberGenerator rnd, int max)
    {
      var r = new byte[4];
      int value;
      do
      {
        rnd.GetBytes(r);
        value = BitConverter.ToInt32(r, 0) & int.MaxValue;
      }
      while (value >= max * (int.MaxValue / max));
      return value % max;
    }

    #endregion
  }
}