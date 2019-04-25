namespace Tangle.Net.Utils
{
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;

  using Org.BouncyCastle.Math;

  using RestSharp;
  using RestSharp.Deserializers;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository.Responses;

  /// <summary>
  /// The string extensions.
  /// </summary>
  public static class Extensions
  {
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
    /// The merge.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <typeparam name="T">
    /// The type.
    /// </typeparam>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public static TryteString Merge<T>(this IEnumerable<T> value)
      where T : TryteString
    {
      return value.Aggregate(new TryteString(), (current, tryteString) => current.Concat(tryteString));
    }

    /// <summary>
    /// The to trits.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] ToTrits(this int value)
    {
      return Converter.IntToTrits(value, 27);
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
      return new TryteString(
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

    public static NodeErrorResponse ToNodeError(this IRestResponse response)
    {
      return new JsonDeserializer().Deserialize<NodeErrorResponse>(response);
    }
  }
}