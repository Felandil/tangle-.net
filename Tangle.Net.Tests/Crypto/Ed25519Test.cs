using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangle.Net.Crypto;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Utils;

namespace Tangle.Net.Tests.Crypto
{
  [TestClass]
  public class Ed25519Test
  {
    [TestMethod]
    public void TestSigantureGeneration()
    {
      var expected =
        "0acae5d099c82549ead3989cd49b671a72b33118150d00bb699ce31238b548c862d06e675243cbc4195a8239ef593530733142961ac08620c0d8855758581908";

      var seed = Ed25519Seed.FromMnemonic("witch collapse practice feed shame open despair creek road again ice least");
      var actual = Ed25519.Sign("Sign this!".Utf8ToBytes(), seed);

      Assert.AreEqual(expected, actual.Signature);
    }

    [TestMethod]
    public void TestSignatureVerificationShouldSucceed()
    {
      var seed = Ed25519Seed.FromMnemonic("witch collapse practice feed shame open despair creek road again ice least");
      var payload = "Sign this!".Utf8ToBytes();
      var signature = Ed25519.Sign(payload, seed);

      Assert.IsTrue(Ed25519.Verify(payload, signature));
    }

    [TestMethod]
    public void TestSignatureVerificationShouldFail()
    {
      var seed = Ed25519Seed.FromMnemonic("witch collapse practice feed shame open despair creek road again ice least");
      var signature = Ed25519.Sign("Sign this!".Utf8ToBytes(), seed);

      Assert.IsFalse(Ed25519.Verify("Something".Utf8ToBytes(), signature));
    }
  }
}