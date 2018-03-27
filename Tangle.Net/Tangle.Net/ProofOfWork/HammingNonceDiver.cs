namespace Tangle.Net.ProofOfWork
{
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;

  /// <summary>
  /// The hamming nonce diver.
  /// </summary>
  public class HammingNonceDiver : AbstractPearlDiver
  {
    protected override ulong High0 => 0xB6DB6DB6DB6DB6DB;
    protected override ulong High1 => 0xC7E3F1F8FC7E3F1F;
    protected override ulong High2 => 0xFFFC01FFFF803FFF;
    protected override ulong High3 => 0x000001FFFFFFFFFF;
    protected override ulong Low0 => 0xDB6DB6DB6DB6DB6D;
    protected override ulong Low1 => 0xF8FC7E3F1F8FC7E3;
    protected override ulong Low2 => 0x07FFFE00FFFFC01F;
    protected override ulong Low3 => 0xFFFFFE007FFFFFFF;

    /// <inheritdoc />
    public HammingNonceDiver() : base(CurlMode.CurlP81)
    {
    }

    /// <inheritdoc />
    public HammingNonceDiver(CurlMode mode) : base(mode)
    {
    }

    /// <inheritdoc />
    protected override ulong Check(int minWeightMagnitude, NonceCurl curl)
    {
      var trits = new int[curl.Low.Length];

      for (var i = 0; i < trits.Length; i++)
      {
        var sum = 0;
        for (var j = 0; j < minWeightMagnitude; j++)
        {
          for (var k = 0; k < trits.Length; k++)
          {
            trits[k] = (curl.Low[k] & (ulong)i) == 0 ? 1 : (curl.High[k] & (ulong)i) == 0 ? -1 : 0;
          }

          sum += this.Sum(trits.Skip(j * (AbstractCurl.HashLength / 3)).Take(AbstractCurl.HashLength / 3).ToArray());
          if (sum == 0 && j < minWeightMagnitude - 1)
          {
            sum = 1;
            break;
          }
        }

        if (sum == 0)
        {
          return (ulong)i;
        }
      }

      return 0;
    }

    private int Sum(int[] take)
    {
      var sum = take[0];

      for (var i = 1; i < take.Length; i++)
      {
        sum = Converter.Sum(sum, take[i]);
      }

      return sum;
    }
  }
}