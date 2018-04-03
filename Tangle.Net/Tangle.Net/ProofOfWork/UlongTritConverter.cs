namespace Tangle.Net.ProofOfWork.Entity
{
  public class UlongTritConverter
  {
    /// <summary>
    /// The high.
    /// </summary>
    public const ulong Max = 0xFFFFFFFFFFFFFFFF;

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
    /// <returns>
    /// The <see cref="UlongTritTouple"/>.
    /// </returns>
    public static UlongTritTouple TritsToUlong(int[] input, int length)
    {
      var result = new UlongTritTouple(new ulong[length], new ulong[length]);

      for (var i = 0; i < input.Length; i++)
      {
        switch (input[i])
        {
          case 0:
            result.Low[i] = Max;
            result.High[i] = Max;
            break;
          case 1:
            result.Low[i] = Min;
            result.High[i] = Max;
            break;
          default:
            result.Low[i] = Max;
            result.High[i] = Min;
            break;
        }
      }

      if (input.Length < length)
      {
        for (var i = input.Length; i < length; i++)
        {
          result.Low[i] = Max;
          result.High[i] = Max;
        }
      }

      return result;
    }
  }
}
