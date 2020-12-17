using System;
using System.Collections.Generic;
using System.Text;

namespace Tangle.Net.ProofOfWork
{
  using System.Linq;
  using System.Numerics;

  public static class TritConverter
  {
    private static readonly int MinTryteValue = -13;

    private static readonly int Radix = 27;

    private static readonly int RadixHalf = 13;

    private static readonly int TritsPerTryte = 3;

    private static readonly List<int[]> TritLookup = new List<int[]>
                                                       {
                                                         new[] { -1, -1, -1 },
                                                         new[] { 0, -1, -1 },
                                                         new[] { 1, -1, -1 },
                                                         new[] { -1, 0, -1 },
                                                         new[] { 0, 0, -1 },
                                                         new[] { 1, 0, -1 },
                                                         new[] { -1, 1, -1 },
                                                         new[] { 0, 1, -1 },
                                                         new[] { 1, 1, -1 },
                                                         new[] { -1, -1, 0 },
                                                         new[] { 0, -1, 0 },
                                                         new[] { 1, -1, 0 },
                                                         new[] { -1, 0, 0 },
                                                         new[] { 0, 0, 0 },
                                                         new[] { 1, 0, 0 },
                                                         new[] { -1, 1, 0 },
                                                         new[] { 0, 1, 0 },
                                                         new[] { 1, 1, 0 },
                                                         new[] { -1, -1, 1 },
                                                         new[] { 0, -1, 1 },
                                                         new[] { 1, -1, 1 },
                                                         new[] { -1, 0, 1 },
                                                         new[] { 0, 0, 1 },
                                                         new[] { 1, 0, 1 },
                                                         new[] { -1, 1, 1 },
                                                         new[] { 0, 1, 1 },
                                                         new[] { 1, 1, 1 }
                                                       };

    public static int Encode(byte[] source, ref int[] destination, int index = 0)
    {
      var j = 0;
      for (var i = 0; i < source.Length; i++)
      {
        var value = source[i];
        var group = EncodeGroup(value);
        StoreTrits(index + j, @group.Item1, ref destination);
        StoreTrits(index + j + TritsPerTryte, @group.Item2, ref destination);
        j += 6;
      }

      return j;
    }

    private static Tuple<int, int> EncodeGroup(int value)
    {
      var v = (value << 24 >> 24) + (RadixHalf * Radix) + RadixHalf;
      var quo = (int)Math.Truncate((double)v / 27);
      var rem = (int)Math.Truncate((double)v % 27);

      return new Tuple<int, int>(rem + MinTryteValue, quo + MinTryteValue);
    }

    private static void StoreTrits(int index, int value, ref int[] destinationTrits)
    {
      var idx = value - MinTryteValue;

      destinationTrits[index] = TritLookup[idx][0];
      destinationTrits[index + 1] = TritLookup[idx][1];
      destinationTrits[index + 2] = TritLookup[idx][2];
    }
  }
}
