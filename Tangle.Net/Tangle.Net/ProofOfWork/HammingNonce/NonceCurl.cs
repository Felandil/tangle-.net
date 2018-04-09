namespace Tangle.Net.ProofOfWork.HammingNonce
{
  using System;

  using Tangle.Net.Cryptography.Curl;

  /// <summary>
  /// The u long trits collection.
  /// </summary>
  public class NonceCurl
  {
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
      this.Rounds = rounds;
      this.Low = new ulong[Curl.StateLength];
      this.High = new ulong[Curl.StateLength];

      Array.Copy(low, this.Low, low.Length);
      Array.Copy(high, this.High, high.Length);
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
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public int Increment(int fromIndex, int toIndex)
    {
      for (var i = fromIndex; i < toIndex; i++)
      {
        var low = this.Low[i];
        var high = this.High[i];

        this.Low[i] = high ^ low;
        this.High[i] = low;

        if ((high & ~low) == 0)
        {
          return toIndex - fromIndex;
        }
      }

      return toIndex - fromIndex + 1;
    }

    /// <summary>
    /// The transform.
    /// </summary>
    public void Transform()
    {
      var scratchpadIndex = 0;
      var scratchpadLow = new ulong[Curl.StateLength];
      var scratchpadHigh = new ulong[Curl.StateLength];

      for (var round = 0; round < this.Rounds; round++)
      {
        Array.Copy(this.Low, scratchpadLow, Curl.StateLength);
        Array.Copy(this.High, scratchpadHigh, Curl.StateLength);

        for (var stateIndex = 0; stateIndex < Curl.StateLength; stateIndex++)
        {
          var alpha = scratchpadLow[scratchpadIndex];
          var beta = scratchpadHigh[scratchpadIndex];
          var gamma = scratchpadHigh[scratchpadIndex += scratchpadIndex < 365 ? 364 : -365];
          var delta = (alpha | (~gamma)) & (scratchpadLow[scratchpadIndex] ^ beta);

          this.Low[stateIndex] = ~delta;
          this.High[stateIndex] = (alpha ^ gamma) | delta;
        }
      }
    }
  }
}