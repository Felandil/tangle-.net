namespace Tangle.Net.ProofOfWork
{
  using System;

  using Tangle.Net.Cryptography.Curl;

  /// <summary>
  /// The u long trits collection.
  /// </summary>
  public class NonceCurl
  {
    /// <summary>
    /// The high.
    /// </summary>
    private const ulong Max = 0xFFFFFFFFFFFFFFFF;

    /// <summary>
    /// The low.
    /// </summary>
    private const ulong Min = 0x0000000000000000;

    /// <summary>
    /// Initializes a new instance of the <see cref="NonceCurl"/> class.
    /// </summary>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <param name="rounds">
    /// The rounds.
    /// </param>
    public NonceCurl(int length, int rounds)
    {
      this.Rounds = rounds;
      this.Low = new ulong[length];
      this.High = new ulong[length];
      this.ScratchPadHigh = new ulong[length];
      this.ScratchPadLow = new ulong[length];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NonceCurl"/> class.
    /// </summary>
    /// <param name="low">
    /// The low.
    /// </param>
    /// <param name="high">
    /// The high.
    /// </param>
    /// <param name="rounds">
    /// The rounds.
    /// </param>
    public NonceCurl(ulong[] low, ulong[] high, int rounds)
    {
      this.Low = (ulong[])low.Clone();
      this.High = (ulong[])high.Clone();
      this.Rounds = rounds;

      this.ScratchPadHigh = new ulong[high.Length];
      this.ScratchPadLow = new ulong[low.Length];
    }

    /// <summary>
    /// Gets or sets the high.
    /// </summary>
    public ulong[] High { get; set; }

    /// <summary>
    /// Gets or sets the low.
    /// </summary>
    public ulong[] Low { get; set; }

    /// <summary>
    /// Gets the rounds.
    /// </summary>
    private int Rounds { get; }

    /// <summary>
    /// Gets or sets the high.
    /// </summary>
    private ulong[] ScratchPadHigh { get; set; }

    /// <summary>
    /// Gets or sets the low.
    /// </summary>
    private ulong[] ScratchPadLow { get; set; }

    /// <summary>
    /// The init.
    /// </summary>
    /// <param name="start">
    /// The start.
    /// </param>
    /// <param name="length">
    /// The length.
    /// </param>
    public void Init(int start, int length)
    {
      for (var i = start; i < length; i++)
      {
        this.Low[i] = Max;
        this.High[i] = Max;
      }
    }

    /// <summary>
    /// The from trits.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <param name="offset">
    /// The offset.
    /// </param>
    /// <returns>
    /// The <see cref="NonceCurl"/>.
    /// </returns>
    public int Absorb(int[] trits, int length, int offset)
    {
      for (var i = 0; i < length; i++)
      {
        switch (trits[offset++])
        {
          case 0:
            this.Low[i] = Max;
            this.High[i] = Max;
            break;
          case 1:
            this.Low[i] = Min;
            this.High[i] = Max;
            break;
          default:
            this.Low[i] = Max;
            this.High[i] = Min;
            break;
        }
      }

      return offset;
    }

    /// <summary>
    /// The clone.
    /// </summary>
    /// <returns>
    /// The <see cref="NonceCurl"/>.
    /// </returns>
    public NonceCurl Clone()
    {
      return new NonceCurl(this.Low, this.High, this.Rounds);
    }

    /// <summary>
    /// The increment.
    /// </summary>
    /// <param name="fromIndex">
    /// The from index.
    /// </param>
    /// <param name="toIndex">
    /// The to index.
    /// </param>
    public void Increment(int fromIndex, int toIndex)
    {
      for (var i = fromIndex; i < toIndex; i++)
      {
        if (this.Low[i] == Min)
        {
          this.Low[i] = Max;
          this.High[i] = Min;
        }
        else
        {
          if (this.High[i] == Min)
          {
            this.High[i] = Max;
          }
          else
          {
            this.Low[i] = Min;
          }

          break;
        }
      }
    }

    /// <summary>
    /// The transform.
    /// </summary>
    /// <param name="rounds">
    /// The rounds.
    /// </param>
    public void Transform(int rounds)
    {
      var curlScratchpadIndex = 0;
      for (var round = 0; round < rounds; round++)
      {
        Array.Copy(this.Low, this.ScratchPadLow, Curl.StateLength);
        Array.Copy(this.High, this.ScratchPadHigh, Curl.StateLength);

        for (var curlStateIndex = 0; curlStateIndex < Curl.StateLength; curlStateIndex++)
        {
          var alpha = this.ScratchPadLow[curlScratchpadIndex];
          var beta = this.ScratchPadHigh[curlScratchpadIndex];
          if (curlScratchpadIndex < 365)
          {
            curlScratchpadIndex += 364;
          }
          else
          {
            curlScratchpadIndex += -365;
          }

          var gamma = this.ScratchPadHigh[curlScratchpadIndex];
          var delta = (alpha | ~gamma) & (this.ScratchPadLow[curlScratchpadIndex] ^ beta);

          this.Low[curlStateIndex] = ~delta;
          this.High[curlStateIndex] = (alpha ^ gamma) | delta;
        }
      }
    }
  }
}