using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Tangle.Net.Utils;

namespace Tangle.Net.Crypto
{
  public class Bip39
  {
    private static string Salt(string password)
    {
      return "mnemonic" + (!string.IsNullOrEmpty(password) ? password : "");
    }

    public static byte[] MnemonicToSeed(string mnemonic, string password = "")
    {
      var saltBytes = Encoding.UTF8.GetBytes(Salt(password.Normalize(NormalizationForm.FormKD)));
      return KeyDerivation.Pbkdf2(mnemonic, saltBytes, KeyDerivationPrf.HMACSHA512, 2048, 64);
    }

    public static string RandomMnemonic(int length = 256)
    {
      if (length % 32 != 0)
      {
        throw new ArgumentException($"Mnemonic length has to be a multiple of 32. ({length} given)");
      }

      var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
      var buffer = new byte[length / 8];

      rngCryptoServiceProvider.GetBytes(buffer);

      var entropyHex = BitConverter.ToString(buffer).Replace("-", "");

      return EntropyToMnemonic(entropyHex);
    }

    public static string EntropyToMnemonic(string entropy)
    {

      //How can I do this more efficiently, the multiple substrings I don't like...
      var entropyBytes = Enumerable.Range(0, entropy.Length / 2)
        .Select(x => Convert.ToByte(entropy.Substring(x * 2, 2), 16))
        .ToArray();


      if (entropyBytes.Length < 16 || entropyBytes.Length > 32 || entropyBytes.Length % 4 != 0)
      {
        throw new FormatException("The length of the entropy is invalid, it should be a multiple of 4, >= 16 and <= 32");
      }


      var entropyBits = entropyBytes.ToBinary();
      var checksumBits = DeriveChecksumBits(entropyBytes);

      var bits = entropyBits + checksumBits;

      var chunks = Regex.Matches(bits, "(.{1,11})")
        .OfType<Match>()
        .Select(m => m.Groups[0].Value)
        .ToArray();

      var words = chunks.Select(binary =>
      {
        var index = Convert.ToInt32(binary, 2);
        return Wordlist.English.MnemonicWords[index];
      });

      return string.Join(" ", words);
    }

    private static string DeriveChecksumBits(byte[] checksum)
    {
      var ent = checksum.Length * 8;
      var cs = ent / 32;

      var sha256Provider = new SHA256CryptoServiceProvider();
      var hash = sha256Provider.ComputeHash(checksum);
      var result = hash.ToBinary();

      return result.Substring(0, cs);
    }
  }
}