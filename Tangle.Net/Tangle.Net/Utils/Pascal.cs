namespace Tangle.Net.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography;

  /// <summary>
  /// The pascal.
  /// </summary>
  public static class Pascal
  {
    /// <summary>
    /// The zero.
    /// </summary>
    private static readonly int[] Zero = { 1, 0, 0, -1 };

    /// <summary>
    /// The encode.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] Encode(int value)
    {
      if (value == 0)
      {
        return Zero;
      }

      var length = RoundThird(CalculateMinimumTrits(Math.Abs(value), 1));
      var trits = new int[EncodedLength(value)];
      IntToTrits(value, trits);

      var encoding = 0;
      var index = 0;

      for (var i = 0; i < length - Constants.TritsPerTryte; i += Constants.TritsPerTryte)
      {
        var tritValue = trits.Skip(i).Take(Constants.TritsPerTryte).ToArray();
        var tritsAsInt = Converter.TritsToInt(tritValue);

        if (tritsAsInt >= 0)
        {
          encoding |= 1 << index;
          for (var j = 0; j < tritValue.Length; j++)
          {
            trits[i + j] = -tritValue[j];
          }
        }

        index++;
      }

      if (Converter.TritsToInt(trits.Skip(length - Constants.TritsPerTryte).Take(length).ToArray()) < 0)
      {
        encoding |= 1 << index;
        for (var i = 0; i < trits.Skip(length - Constants.TritsPerTryte).Take(length).ToArray().Length; i++)
        {
          trits[i + length - Constants.TritsPerTryte] = -trits[i + length - Constants.TritsPerTryte];
        }
      }

      var checksumTrits = new int[trits.Length - length];
      IntToTrits(encoding, checksumTrits);

      for (var i = 0; i < checksumTrits.Length; i++)
      {
        trits[length + i] = checksumTrits[i];
      }

      return trits;
    }

    /// <summary>
    /// The decode.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// The <see cref="Tuple"/>.
    /// </returns>
    public static Tuple<int, int> Decode(int[] value)
    {
      if (value.Length < 3)
      {
        throw new ArgumentException("Given trits can not be Pascal decoded. Invalid length.");
      }

      if (value.Take(4).SequenceEqual(Zero))
      {
        return new Tuple<int, int>(0, 4);
      }

      var encoderStart = End(value);
      var inputEnd = encoderStart + (encoderStart / Constants.TritsPerTryte);
      var encoder = Converter.TritsToInt(value.Skip(encoderStart).Take(inputEnd - encoderStart).ToArray());

      var result = 0;
      for (var i = 0; i < encoderStart / Constants.TritsPerTryte; i++)
      {
        var tritsIntValue = ((encoder >> i) & 1) != 0 
                              ? -Converter.TritsToInt(value.Skip(i * Constants.TritsPerTryte).Take(Constants.TritsPerTryte).ToArray()) 
                              : Converter.TritsToInt(value.Skip(i * Constants.TritsPerTryte).Take(Constants.TritsPerTryte).ToArray());

        result = result + ((int)Math.Pow(27, i) * tritsIntValue);
      }

      return new Tuple<int, int>(result, inputEnd);
    }

    /// <summary>
    /// The round third.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public static int RoundThird(int input)
    {
      var rem = input % Converter.Radix;
      if (rem == 0)
      {
        return input;
      }

      return input + Converter.Radix - rem;
    }

    /// <summary>
    /// The encoded length.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public static int EncodedLength(int value)
    {
      if (value == 0)
      {
        return Zero.Length;
      }

      var length = RoundThird(CalculateMinimumTrits(Math.Abs(value), 1));
      return length + (length / Converter.Radix);
    }

    /// <summary>
    /// The end.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int End(int[] input)
    {
      if (Converter.TritsToInt(input.Take(Constants.TritsPerTryte).ToArray()) > 0)
      {
        return Constants.TritsPerTryte;
      }

      return Constants.TritsPerTryte + End(input.Skip(Constants.TritsPerTryte).ToArray());
    }

    /// <summary>
    /// The int to trits.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int IntToTrits(int input, int[] trits)
    {
      var end = WriteTrits(input, trits, 0);

      if (input >= 0)
      {
        return end;
      }

      for (var i = 0; i < trits.Length; i++)
      {
        trits[i] = -trits[i];
      }

      return end;
    }

    /// <summary>
    /// The write trits.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int WriteTrits(int input, IList<int> trits, int index)
    {
      switch (input)
      {
        case 0:
          return 0;
        default:
          var abs = input / Converter.Radix;
          var r = input % Converter.Radix;
          if (r > 1)
          {
            abs += 1;
            r = -1;
          }

          trits[index] = r;
          index++;

          return 1 + WriteTrits(abs, trits, index);
      }
    }

    /// <summary>
    /// The calculate minimum trits.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <param name="basis">
    /// The basis.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int CalculateMinimumTrits(int input, int basis)
    {
      if (input <= basis)
      {
        return 1;
      }

      return 1 + CalculateMinimumTrits(input, 1 + (basis * Constants.TritsPerTryte));
    }
  }
}