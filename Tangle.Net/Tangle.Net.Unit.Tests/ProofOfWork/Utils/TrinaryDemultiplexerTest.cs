namespace Tangle.Net.Unit.Tests.ProofOfWork.Utils
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.ProofOfWork.Entity;
  using Tangle.Net.ProofOfWork.Utils;

  /// <summary>
  /// The trinary demultiplexer test.
  /// </summary>
  [TestClass]
  public class TrinaryDemultiplexerTest
  {
    /// <summary>
    /// The test correct length output.
    /// </summary>
    [TestMethod]
    public void TestCorrectLengthOutput()
    {
      var trits = Converter.TrytesToTrits("ADHMOICIPKGYHYL9VMLSSXHGKTUTEQUTIWUQWSVYHZWTAHNIYQICEJWFTCYBGRGRM9DWBCGDELIGEIIIH");
      var ulongTrits = UlongTritConverter.TritsToUlong(trits);

      ulongTrits.Low[0] = AbstractPearlDiver.Low0;
      ulongTrits.Low[1] = AbstractPearlDiver.Low1;
      ulongTrits.Low[2] = AbstractPearlDiver.Low2;
      ulongTrits.Low[3] = AbstractPearlDiver.Low3;

      ulongTrits.High[0] = AbstractPearlDiver.High0;
      ulongTrits.High[1] = AbstractPearlDiver.High1;
      ulongTrits.High[2] = AbstractPearlDiver.High2;
      ulongTrits.High[3] = AbstractPearlDiver.High3;

      var mux = new TrinaryDemultiplexer(ulongTrits);

      Assert.AreEqual(TrinaryDemultiplexer.Width, mux.Length);
    }
  }
}