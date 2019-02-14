namespace Tangle.Net.ProofOfWork
{
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.ProofOfWork.Entity;
  using Tangle.Net.ProofOfWork.HammingNonce;
  using Tangle.Net.ProofOfWork.Utils;
  using Tangle.Net.Utils;

  /// <summary>
  /// The abstract pearl diver.
  /// </summary>
  public abstract class AbstractPearlDiver : IPearlDiver
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPearlDiver"/> class.
    /// </summary>
    /// <param name="mode">
    /// The mode.
    /// </param>
    protected AbstractPearlDiver(CurlMode mode)
    {
      this.Rounds = (int)mode;
    }

    /// <summary>
    /// Gets the high 0.
    /// </summary>
    public abstract ulong High0 { get; }

    /// <summary>
    /// Gets the high 1.
    /// </summary>
    public abstract ulong High1 { get; }

    /// <summary>
    /// Gets the high 2.
    /// </summary>
    public abstract ulong High2 { get; }

    /// <summary>
    /// Gets the high 3.
    /// </summary>
    public abstract ulong High3 { get; }

    /// <summary>
    /// Gets the low 0.
    /// </summary>
    public abstract ulong Low0 { get; }

    /// <summary>
    /// Gets the low 1.
    /// </summary>
    public abstract ulong Low1 { get; }

    /// <summary>
    /// Gets the low 2.
    /// </summary>
    public abstract ulong Low2 { get; }

    /// <summary>
    /// Gets the low 3.
    /// </summary>
    public abstract ulong Low3 { get; }

    /// <summary>
    /// Gets the rounds.
    /// </summary>
    protected int Rounds { get; }

    /// <inheritdoc />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
    public int[] Search(int[] trits, int security, int length, int offset)
    {
      var ulongTrits = this.PrepareTrits(trits, offset);
      var curl = new NonceCurl(ulongTrits.Low, ulongTrits.High, this.Rounds);
      var size = Math.Min(length, AbstractCurl.HashLength) - offset;

      var index = 0;

      while (index == 0)
      {
        var incrementResult = curl.Increment(offset + size * 2 / 3, offset + size);
        size = Math.Min(Pascal.RoundThird(offset + size * 2 / 3 + incrementResult), AbstractCurl.HashLength) - offset;

        var curlCopy = curl.Clone();
        curlCopy.Transform();

        index = Check(security, curlCopy.Low.Take(AbstractCurl.HashLength).ToArray(), curlCopy.High.Take(AbstractCurl.HashLength).ToArray());
      }

      var result = new TrinaryDemultiplexer(new UlongTritTouple(curl.Low.Take(size).ToArray(), curl.High.Take(size).ToArray()));
      return result.Get(index);
    }

    /// <summary>
    /// The prepare trits.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <param name="offset">
    /// The offset.
    /// </param>
    /// <returns>
    /// The <see cref="UlongTritTouple"/>.
    /// </returns>
    protected UlongTritTouple PrepareTrits(int[] trits, int offset)
    {
      var ulongTrits = UlongTritConverter.TritsToUlong(trits, Curl.StateLength);

      ulongTrits.Low[offset] = this.Low0;
      ulongTrits.Low[offset + 1] = this.Low1;
      ulongTrits.Low[offset + 2] = this.Low2;
      ulongTrits.Low[offset + 3] = this.Low3;

      ulongTrits.High[offset] = this.High0;
      ulongTrits.High[offset + 1] = this.High1;
      ulongTrits.High[offset + 2] = this.High2;
      ulongTrits.High[offset + 3] = this.High3;

      return ulongTrits;
    }

    /// <summary>
    /// The check.
    /// </summary>
    /// <param name="security">
    /// The security.
    /// </param>
    /// <param name="low">
    /// The low.
    /// </param>
    /// <param name="high">
    /// The high.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int Check(int security, ulong[] low, ulong[] high)
    {
      var demux = new TrinaryDemultiplexer(new UlongTritTouple(low, high));
      for (var i = 0; i < demux.Length; i++)
      {
        var sum = 0;
        for (var j = 0; j < security; j++)
        {
          sum += demux.Get(i).Skip(j * AbstractCurl.HashLength / 3).Take(AbstractCurl.HashLength / 3).Sum();

          if (sum == 0 && j < security - 1)
          {
            sum = 1;
            break;
          }
        }

        if (sum == 0)
        {
          return i;
        }
      }

      return 0;
    }
  }
}