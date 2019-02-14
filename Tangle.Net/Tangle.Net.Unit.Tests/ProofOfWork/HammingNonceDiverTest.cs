namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using System.Linq;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.ProofOfWork.HammingNonce;
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
      const string ExpectedNonce = "GCHMOICIPKGYHYL9VMMSSXHGKTU"; // "GCHMOICIPKGYHYL9VMMSSXHGKTUTEQUTIWUQWSVYHZWTAHNIYQICEJWFTCYBGRGRM9DWBCGDELIGEIIIH" complete rust nonce output
      var input = new TryteString("ADHMOICIPKGYHYL9VMLSSXHGKTUTEQUTIWUQWSVYHZWTAHNIYQICEJWFTCYBGRGRM9DWBCGDELIGEIIIH");

      var finder = new HammingNonceDiver(CurlMode.CurlP27);
      var nonce = finder.Search(input.ToTrits(), 2, Constants.TritHashLength / 3, 0);

      Assert.AreEqual(ExpectedNonce, Converter.TritsToTrytes(nonce.Take(81).ToArray()));
    }
  }
}