namespace Tangle.Net.ProofOfWork.HammingNonce
{
  using Tangle.Net.ProofOfWork.Entity;

  public class UlongTritConverter
  {
    /// <summary>
    /// The low.
    /// </summary>
    public const ulong Min = 0x0000000000000000;

    /// <summary>
    /// The trits to ulong.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <param name="mode">
    /// The mode.
    /// </param>
    /// <returns>
    /// The <see cref="UlongTritTouple"/>.
    /// </returns>
    public static UlongTritTouple TritsToUlong(int[] input, int length, Mode mode)
    {
      var max = mode == Mode._32bit ? int.MaxValue : ulong.MaxValue;
      var result = new UlongTritTouple(new ulong[length], new ulong[length]);

      for (var i = 0; i < input.Length; i++)
      {
        switch (input[i])
        {
          case 0:
            result.Low[i] = max;
            result.High[i] = max;
            break;
          case 1:
            result.Low[i] = Min;
            result.High[i] = max;
            break;
          default:
            result.Low[i] = max;
            result.High[i] = Min;
            break;
        }
      }

      if (input.Length < length)
      {
        for (var i = input.Length; i < length; i++)
        {
          result.Low[i] = max;
          result.High[i] = max;
        }
      }

      return result;
    }
  }
}
