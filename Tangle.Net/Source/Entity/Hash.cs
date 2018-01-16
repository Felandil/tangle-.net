namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Source.Cryptography;

  /// <summary>
  /// The hash.
  /// </summary>
  public class Hash : TryteString
  {
    #region Constants

    /// <summary>
    /// The length.
    /// </summary>
    public new const int Length = Kerl.HashLength / Converter.Radix;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Hash"/> class.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    public Hash(string trytes)
      : base(trytes)
    {
      if (trytes.Length != Length)
      {
        throw new ArgumentException("Hash must be exactly of length " + Length);
      }
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the empty.
    /// </summary>
    public static Hash Empty
    {
      get
      {
        return new Hash("999999999999999999999999999999999999999999999999999999999999999999999999999999999");
      }
    }

    #endregion

    #region Public Methods and Operators

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
            if (chunk[j] <= -13)
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
            if (chunk[j] >= 13)
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

    #endregion
  }
}