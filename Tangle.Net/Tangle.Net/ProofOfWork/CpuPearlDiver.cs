namespace Tangle.Net.ProofOfWork
{
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Utils;

  /// <summary>
  /// The cpu pow diver.
  /// </summary>
  public class CpuPearlDiver : AbstractPearlDiver
  {
    /// <inheritdoc />
    public CpuPearlDiver() : base(CurlMode.CurlP81)
    {
    }

    /// <inheritdoc />
    public CpuPearlDiver(CurlMode mode) : base(mode)
    {
    }

    /// <inheritdoc />
    protected override ulong Check(int minWeightMagnitude, NonceCurl threadCurlClone)
    {
      var mask = NonceCurl.Max;
      for (var i = minWeightMagnitude; i-- > 0;)
      {
        mask &= ~(threadCurlClone.Low[Constants.TritHashLength - 1 - i] ^ threadCurlClone.High[Constants.TritHashLength - 1 - i]);
        if (mask == 0)
        {
          break;
        }
      }

      return mask;
    }
  }
}