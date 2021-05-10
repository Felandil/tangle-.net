using System.Linq;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Utils;

namespace Tangle.Net.Crypto
{
  public static class Ed25519
  {
    public static Ed25519Signature Sign(byte[] payload, Ed25519Seed seed)
    {
      var ed25519 = new Rebex.Security.Cryptography.Ed25519();
      ed25519.FromSeed(seed.SecretKey.Take(32).ToArray());

      var signature = ed25519.SignMessage(payload);

      return new Ed25519Signature
      {
        PublicKey = seed.KeyPair.PublicKey.ToHex(),
        Signature = signature.ToHex()
      };
    }

    public static bool Verify(byte[] payload, Ed25519Signature signature)
    {
      var ed25519 = new Rebex.Security.Cryptography.Ed25519();
      ed25519.FromPublicKey(signature.PublicKey.HexToBytes());

      return ed25519.VerifyMessage(payload, signature.Signature.HexToBytes());
    }
  }
}