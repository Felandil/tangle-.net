using System;
using System.Text;

namespace Tangle.Net.Services
{
  using System.Linq;

  public static class Converter
  {
    private static string[] EncodeLookup { get; set; }
    private static int[] DecodeLookup { get; set; }

    static Converter()
    {
      var alphabet = "0123456789abcdef";
      var encodeLookup = new string[256];
      var decodeLookup = new int[256];

      for (var i = 0; i < 256; i++)
      {
        encodeLookup[i] = alphabet[(i >> 4) & 0xF].ToString() + alphabet[i & 0xF];
        if (i >= 16)
        {
          continue;
        }

        if (i < 10)
        {
          decodeLookup[0x30 + i] = (char)i;
        }
        else
        {
          decodeLookup[0x61 - 10 + i] = (char)i;
        }
      }

      EncodeLookup = encodeLookup;
      DecodeLookup = decodeLookup;
    }

    public static string ToHex(this string value)
    {
      return value.Utf8ToBytes().ToHex();
    }

    public static string ToHex(this byte[] bytes, bool reverse = false)
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
