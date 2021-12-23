namespace Tangle.Net.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public static class Converter
  {
    private static string[] EncodeLookup { get; set; }

    public static readonly string HexAlphabet = "0123456789abcdef";

    static Converter()
    {
      var encodeLookup = new string[256];

      for (var i = 0; i < 256; i++)
      {
        encodeLookup[i] = HexAlphabet[(i >> 4) & 0xF].ToString() + HexAlphabet[i & 0xF];
      }

      EncodeLookup = encodeLookup;
    }

    public static string ToHex(this string value)
    {
      return value.Utf8ToBytes().ToHex();
    }

    public static string ToHex(this IEnumerable<byte> bytes, bool reverse = false)
    {
      var hex = string.Empty;

      if (reverse)
      {
        hex = bytes.Aggregate(hex, (current, t) => EncodeLookup[t] + current);
      }
      else
      {
        hex = bytes.Aggregate(hex, (current, t) => current + EncodeLookup[t]);
      }

      return hex;
    }

    public static byte[] HexToBytes(this string value, bool reverse = false)
    {
      if (!value.IsHex())
      {
        throw new ArgumentException("Given value is not a hex string!");
      }

      var result = new List<byte>();

      var i = 0;
      while (i < value.Length)
      {
        var hexPart = value.Substring(i, 2);
        result.Add((byte)Array.IndexOf(EncodeLookup, hexPart));

        i += 2;
      }

      if (reverse)
      {
        result.Reverse();
      }

      return result.ToArray();
    }

    public static string HexToString(this string value)
    {
      return value.HexToBytes().BytesToUtf8();
    }

    public static string BytesToUtf8(this byte[] bytes)
    {
      return Encoding.UTF8.GetString(bytes);
    }

    public static string BytesToAscii(this byte[] bytes)
    {
      return Encoding.ASCII.GetString(bytes);
    }

    public static bool IsHex(this string value)
    {
      if (value.Length % 2 == 1)
      {
        return false;
      }

      return value.All(c => HexAlphabet.Contains(c));
    }

    public static byte[] Utf8ToBytes(this string value)
    {
      return Encoding.UTF8.GetBytes(value);
    }

    public static byte[] AsciiToBytes(this string value)
    {
      if (value.Any(c => c > 255))
      {
        throw new ArgumentException("Detected non ASCII string input. You may use Utf8ToBytes");
      }

      return Encoding.ASCII.GetBytes(value);
    }
  }
}
