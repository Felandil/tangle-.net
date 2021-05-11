using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Tangle.Net.Utils;

namespace Tangle.Net.Crypto
{
  public static class Bip32
  {
    public static (byte[] Key, byte[] ChainCode) GetMasterKeyFromSeed(byte[] seed)
    {
      return HashKey(Encoding.UTF8.GetBytes("ed25519 seed"), seed);
    }

    private static (byte[] Key, byte[] ChainCode) GetChildKeyDerivation(byte[] key, byte[] chainCode, uint index)
    {
      var buffer = new Bip32Buffer();

      buffer.Write(new byte[] { 0 });
      buffer.Write(key);
      buffer.WriteUInt(index);

      return HashKey(chainCode, buffer.ToArray());
    }

    private static (byte[] Key, byte[] ChainCode) HashKey(byte[] chainCode, byte[] bufferBytes)
    {
      using (var hmacSha512 = new HMACSHA512(chainCode))
      {
        var hash = hmacSha512.ComputeHash(bufferBytes);
        return (Key: hash.Slice(0, 32), ChainCode: hash.Slice(32));
      }
    }

    public static (byte[] Key, byte[] ChainCode) DerivePath(Bip32Path path, byte[] seed)
    {
      if (!IsValidPath(path.ToString())) throw new FormatException("Path is not a valid Bip32 Path");

      var segments = path.ToString()
        .Split('/')
        .Slice(1)
        .Select(a => a.Replace("'", ""))
        .Select(a => Convert.ToUInt32(a, 10));

      var result = GetMasterKeyFromSeed(seed);
      foreach (var segment in segments)
      {
        result = GetChildKeyDerivation(result.Key, result.ChainCode, segment + 0x80000000);
      }

      return result;
    }

    private static bool IsValidPath(string path)
    {
      var regex = new Regex("^m(\\/[0-9]+')+$");

      if (!regex.IsMatch(path))
      {
        return false;
      }

      var valid = path.Split('/')
        .Slice(1)
        .Select(a => a.Replace("'", ""))
        .All(a => int.TryParse(a, out _));

      return valid;
    }
  }
}