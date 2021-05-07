using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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
  }
}