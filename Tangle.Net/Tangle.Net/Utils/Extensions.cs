namespace Tangle.Net.Utils
{
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;

  using Org.BouncyCastle.Math;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The string extensions.
  /// </summary>
  public static class Extensions
  {
    #region Public Methods and Operators

    /// <summary>
    /// The get chunks.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <param name="chunkSize">
    /// The chunk size.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public static List<int[]> GetChunks(this int[] value, int chunkSize)
    {
      var chunks = new List<int[]>();

      for (var i = 0; i * chunkSize < value.Count(); i++)
      {
        chunks.Add(value.Skip(i * chunkSize).Take(chunkSize).ToArray());
      }

      return chunks;
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

    /// <summary>
    /// The to trytes.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public static TryteString ToTrytes(this IEnumerable<byte> value)
    {
      return new TryteString(TryteString.BytesToTrytes(value));
    }

    #endregion
  }
}