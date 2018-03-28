namespace Tangle.Net.ProofOfWork.Utils
{
  using System;

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
  }
}