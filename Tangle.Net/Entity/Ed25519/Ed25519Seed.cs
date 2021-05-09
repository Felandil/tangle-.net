using System.Linq;
using Tangle.Net.Crypto;

namespace Tangle.Net.Entity.Ed25519
{
  public class Ed25519Seed
  {
    public Ed25519Seed(byte[] secretKey)
    {
      SecretKey = secretKey;
    }

    private KeyPair keyPair { get; set; }
    public byte[] SecretKey { get; }

    public KeyPair KeyPair
    {
      get
      {
        if (keyPair == null)
        {
          var ed25519 = new Rebex.Security.Cryptography.Ed25519();
          ed25519.FromSeed(this.SecretKey.Take(32).ToArray());

          this.keyPair = new KeyPair
          {
            PrivateKey = ed25519.GetPrivateKey(),
            PublicKey = ed25519.GetPublicKey()
          };
        }

        return keyPair;
      }
    }

    public static Ed25519Seed FromMnemonic(string mnemonic)
    {
      return new Ed25519Seed(Bip39.MnemonicToSeed(mnemonic));
    }
  }
}