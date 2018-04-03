namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.ProofOfWork.Entity;

  /// <summary>
  /// The ulong trit converter test.
  /// </summary>
  [TestClass]
  public class UlongTritConverterTest
  {
    /// <summary>
    /// The test trit to u long conversion.
    /// </summary>
    [TestMethod]
    public void TestTritToULongConversion()
    {
      var input = new[] { -1, 0, 1 };
      var result = UlongTritConverter.TritsToUlong(input, Curl.StateLength);

      Assert.AreEqual(UlongTritConverter.Max, result.Low[0]);
      Assert.AreEqual(UlongTritConverter.Min, result.High[0]);

      Assert.AreEqual(UlongTritConverter.Max, result.Low[1]);
      Assert.AreEqual(UlongTritConverter.Max, result.High[1]);

      Assert.AreEqual(UlongTritConverter.Min, result.Low[2]);
      Assert.AreEqual(UlongTritConverter.Max, result.High[2]);
    }
  }
}