namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using System.Linq;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Utils;

  /// <summary>
  /// The hamming nonce diver test.
  /// </summary>
  [TestClass]
  public class HammingNonceDiverTest
  {
    /// <summary>
    /// The test nonce generation.
    /// </summary>
    [TestMethod]
    public void TestNonceGeneration()
    {
      var expectedNonce = "GCHMOICIPKGYHYL9VMMSSXHGKTU";
      var input = new TryteString("ADHMOICIPKGYHYL9VMLSSXHGKTUTEQUTIWUQWSVYHZWTAHNIYQICEJWFTCYBGRGRM9DWBCGDELIGEIIIH");
                               // "GCHMOICIPKGYHYL9VMMSSXHGKTUTEQUTIWUQWSVYHZWTAHNIYQICEJWFTCYBGRGRM9DWBCGDELIGEIIIH" complete rust nonce output

      var finder = new HammingNonceDiver(CurlMode.CurlP27);
      var nonce = finder.Search(input.ToTrits(), 2, Constants.TritHashLength / 3, 0);

      Assert.AreEqual(expectedNonce, Converter.TritsToTrytes(nonce.Take(81).ToArray()));
    }
  }
}