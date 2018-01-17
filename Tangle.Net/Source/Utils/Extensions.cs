namespace Tangle.Net.Source.Utils
{
  using System.Globalization;

  using Org.BouncyCastle.Math;

  using Tangle.Net.Source.Cryptography;
  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The string extensions.
  /// </summary>
  public static class Extensions
  {
    #region Public Methods and Operators

    /// <summary>
    /// The to trytes.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <param name="padding">
    /// The padding.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public static TryteString ToTrytes(this int value, int padding)
    {
      return new TryteString(Converter.TritsToTrytes(Converter.IntToTrits(value, padding)));
    }

    /// <summary>
    /// The to trytes.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <param name="padding">
    /// The padding.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public static TryteString ToTrytes(this long value, int padding)
    {
      // TODO: this is ugly. create conversion method for long within converter
      return
        new TryteString(
          Converter.TritsToTrytes(Converter.ConvertBigIntToTrits(new BigInteger(value.ToString(CultureInfo.InvariantCulture)), padding)));
    }

    #endregion
  }
}