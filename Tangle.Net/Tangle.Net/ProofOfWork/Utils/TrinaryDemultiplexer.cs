namespace Tangle.Net.ProofOfWork.Utils
{
  using System;
  using System.Collections.Generic;

  using Tangle.Net.ProofOfWork.Entity;

  /// <summary>
  /// The trinary demultiplexer.
  /// </summary>
  public class TrinaryDemultiplexer
  {
    /// <summary>
    /// The value width.
    /// </summary>
    public const int Width = 64;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrinaryDemultiplexer"/> class.
    /// </summary>
    /// <param name="ulongTrits">
    /// The ulong trits.
    /// </param>
    public TrinaryDemultiplexer(UlongTritTouple ulongTrits)
    {
      this.UlongTrits = ulongTrits;
    }

    /// <summary>
    /// Gets the number of encoded trinaries.
    /// </summary>
    public int Length
    {
      get
      {
        var low = Convert.ToString((long)this.UlongTrits.Low[0], 2).PadLeft(Width, '0');
        var high = Convert.ToString((long)this.UlongTrits.High[0], 2).PadLeft(Width, '0');

        return Width - Math.Min(low.Split('1')[0].Length, high.Split('1')[0].Length);
      }
    }

    /// <summary>
    /// Gets the ulong trits.
    /// </summary>
    private UlongTritTouple UlongTrits { get; }

    /// <summary>
    /// The get.
    /// </summary>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public int[] Get(int index)
    {
      var result = new List<int>();

      for (var i = 0; i < this.UlongTrits.Low.Length; i++)
      {
        var low = (this.UlongTrits.Low[i] >> index) & 1;
        var high = (this.UlongTrits.High[i] >> index) & 1;

        if (low == 1 && high == 0)
        {
          result.Add(-1);
        }
        else if (low == 0 && high == 1)
        {
          result.Add(1);
        }
        else
        {
          result.Add(0);
        }
      }

      return result.ToArray();
    }
  }
}