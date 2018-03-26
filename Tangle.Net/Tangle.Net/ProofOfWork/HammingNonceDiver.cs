namespace Tangle.Net.ProofOfWork
{
  using System;
  using System.Linq;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Utils;

  /// <summary>
  /// The hamming nonce diver.
  /// </summary>
  public class HammingNonceDiver : AbstractPearlDiver
  {
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
      throw new NotImplementedException();
      var mux = this.Demux(curl, 1);
      for (var i = 0; i < mux.Length; i++)
      {
        var sum = 0;
        for (var j = 0; j < minWeightMagnitude; j++)
        {
          sum += mux.Skip(j * Constants.TritHashLength / 3).Take(Constants.TritHashLength / 3).Sum();
          if (sum == 0 && j < minWeightMagnitude)
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
  }
}