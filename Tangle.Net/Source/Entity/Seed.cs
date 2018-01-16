namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Linq;

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
      var random = new Random(Guid.NewGuid().GetHashCode());
      var seed = new string(Enumerable.Repeat(AsciiToTrytes.TryteAlphabet, Length).Select(s => s[random.Next(s.Length)]).ToArray());

      return new Seed(seed);
    }

    #endregion
  }
}