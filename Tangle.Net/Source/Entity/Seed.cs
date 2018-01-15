namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Linq;

  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The seed.
  /// </summary>
  public class Seed
  {
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
    {
      if (seed.Length != InputValidator.HashLength)
      {
        throw new ArgumentException("Seed must be of length 81");
      }

      this.Value = seed;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; private set; }

    #endregion

    /// <summary>
    /// The random.
    /// </summary>
    /// <returns>
    /// The <see cref="Seed"/>.
    /// </returns>
    public static Seed Random()
    {
      var random = new Random(Guid.NewGuid().GetHashCode());
      var seed = new string(Enumerable.Repeat(AsciiToTrytes.TryteAlphabet, InputValidator.HashLength).Select(s => s[random.Next(s.Length)]).ToArray());

      return new Seed(seed);
    }
  }
}