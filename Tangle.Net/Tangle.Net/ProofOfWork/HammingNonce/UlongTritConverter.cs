namespace Tangle.Net.ProofOfWork.HammingNonce
{
  using Tangle.Net.ProofOfWork.Entity;

  public static class UlongTritConverter
  {
    public const ulong Min = 0x0000000000000000;

    public static UlongTritTouple TritsToUlong(int[] input, int length)
    {
      var result = new UlongTritTouple(new ulong[length], new ulong[length]);

      for (var i = 0; i < input.Length; i++)
      {
        switch (input[i])
        {
          case 0:
            result.Low[i] = ulong.MaxValue;
            result.High[i] = ulong.MaxValue;
            break;
          case 1:
            result.Low[i] = Min;
            result.High[i] = ulong.MaxValue;
            break;
          default:
            result.Low[i] = ulong.MaxValue;
            result.High[i] = Min;
            break;
        }
      }

      if (input.Length >= length)
      {
        return result;
      }
     
      for (var i = input.Length; i < length; i++)
      {
        result.Low[i] = ulong.MaxValue;
        result.High[i] = ulong.MaxValue;
      }
      

      return result;
    }
  }
}
