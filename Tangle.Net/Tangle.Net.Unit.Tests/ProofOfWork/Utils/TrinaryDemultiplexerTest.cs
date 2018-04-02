namespace Tangle.Net.Unit.Tests.ProofOfWork.Utils
{
  using System.Linq;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.ProofOfWork.Entity;
  using Tangle.Net.ProofOfWork.Utils;
  using Tangle.Net.Utils;

  /// <summary>
  /// The trinary demultiplexer test.
  /// </summary>
  [TestClass]
  public class TrinaryDemultiplexerTest
  {
    /// <summary>
    /// The expected demux. Taken from iota.rs
    /// </summary>
    private static readonly int[] ExpectedDemux =
      {
        0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 2, 3, 4, 4, 3, 2, 2, 2, 3, 3, 4, 4, 4, 4, 5, 6, 5, 4, 3, 4, 5, 6, 5, 6, 7, 6, 6, 5, 5, 6, 7, 6, 6, 6, 7, 8, 8,
        8, 8, 9, 10, 9, 10, 11, 12, 12, 13, 14, 15, 15, 14, 15, 15, 14, 14, 13, 13, 12, 12, 13, 14, 13, 14, 13, 14, 15, 14, 15, 14, 14, 15, 14, 13,
        14, 13, 12, 11, 12, 11, 11, 10, 10, 11, 10, 9, 10, 9, 9, 9, 10, 9, 8, 8, 8, 9, 8, 7, 7, 6, 5, 4, 4, 5, 5, 4, 5, 6, 5, 6, 5, 5, 4, 4, 5, 4, 4,
        4, 3, 2, 2, 1, 2, 1, 2, 2, 2, 1, 1, 2, 1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, -2, -2, -2, -1, -1, 0, 0, -1, -2, -1, 0, 0, 1, 0, -1, -1, -1, -2,
        -1, -2, -1, -2, -2, -1, -1, 0, -1, -1, -2, -1, -1, 0, -1, 0, 0, 0, -1, 0, -1, 0, 0, 0, -1, 0, 1, 2, 2, 2, 2, 3, 4, 4, 3, 2, 2, 1, 2, 2, 2, 3,
        3, 4, 3, 4, 5, 6, 6, 5, 4, 5, 5, 6, 7, 7, 7, 8, 9, 8, 9, 8, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10, 11, 10, 10,
      };

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

    /// <summary>
    /// The test demux maps correctly.
    /// </summary>
    [TestMethod]
    public void TestDemuxMapsCorrectly()
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

      for (var i = 0; i < AbstractCurl.HashLength; i++)
      {
        var tritSum = mux.Get(0).Take(i).ToArray().Sum();
        Assert.AreEqual(ExpectedDemux[i], tritSum);
      }
    }
  }
}