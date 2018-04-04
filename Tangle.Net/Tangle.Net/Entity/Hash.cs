namespace Tangle.Net.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Utils;

  /// <summary>
  /// The hash.
  /// </summary>
  public class Hash : TryteString
  {
    /// <summary>
    /// The length.
    /// </summary>
    public const int Length = Constants.TritHashLength / Converter.Radix;

    /// <summary>
    /// The min tryte value.
    /// </summary>
    public const int MinTryteValue = -13;

    /// <summary>
    /// The max tryte value.
    /// </summary>
    public const int MaxTryteValue = 13;

    /// <summary>
    /// Gets the empty.
    /// </summary>
    public static readonly Hash Empty = new Hash(new string('9', Length));

    /// <summary>
    /// Initializes a new instance of the <see cref="Hash"/> class.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    public Hash(string trytes)
      : base(trytes)
    {
      if (this.TrytesLength < Length)
      {
        this.Pad(Length);
      }

      if (this.TrytesLength > Length)
      {
        throw new ArgumentException("Hash must not have length above " + Length);
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hash"/> class.
    /// </summary>
    public Hash()
      : this(string.Empty)
    {
    }

    /// <summary>
    /// The normalize.
    /// </summary>
    /// <param name="hash">
    /// The hash.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] Normalize(Hash hash)
    {
      var sourceHash = hash.Value.Select(hashTryte => Converter.TritsToInt(Converter.TrytesToTrits(string.Empty + hashTryte))).ToList();
      var normalizedHash = new List<int>();
      const int ChunkSize = 27;

      for (var i = 0; i < 3; i++)
      {
        var chunk = sourceHash.GetRange(i * ChunkSize, ChunkSize);
        long sum = chunk.Sum();

        while (sum > 0)
        {
          sum -= 1;
          for (var j = 0; j < ChunkSize; j++)
          {
            if (chunk[j] <= MinTryteValue)
            {
              continue;
            }

            chunk[j]--;
            break;
          }
        }

        while (sum < 0)
        {
          sum += 1;
          for (var j = 0; j < ChunkSize; j++)
          {
            if (chunk[j] >= MaxTryteValue)
            {
              continue;
            }

            chunk[j]++;
            break;
          }
        }

        normalizedHash.AddRange(chunk);
      }

      return normalizedHash.ToArray();
    }

    /// <summary>
    /// The normalize.
    /// </summary>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public int[] Normalize()
    {
      return Normalize(this);
    }
  }
}