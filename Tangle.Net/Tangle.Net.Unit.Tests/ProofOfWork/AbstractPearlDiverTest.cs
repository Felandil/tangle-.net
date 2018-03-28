namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.ProofOfWork;

  /// <summary>
  /// The abstract pearl diver offset test.
  /// </summary>
  [TestClass]
  public class AbstractPearlDiverTest : AbstractPearlDiver
  {
    /// <summary>
    /// The test prepare sets offsets to correct index.
    /// </summary>
    [TestMethod]
    public void TestPrepareSetsOffsetsToCorrectIndex()
    {
      var trits = Converter.TrytesToTrits("ADHMOICIPKGYHYL9VMLSSXHGKTUTEQUTIWUQWSVYHZWTAHNIYQICEJWFTCYBGRGRM9DWBCGDELIGEIIIH");
      var preparedTrits = this.PrepareTrits(trits, 0);

      Assert.AreEqual(AbstractPearlDiver.Low0, preparedTrits.Low[0]);
      Assert.AreEqual(AbstractPearlDiver.Low1, preparedTrits.Low[1]);
      Assert.AreEqual(AbstractPearlDiver.Low2, preparedTrits.Low[2]);
      Assert.AreEqual(AbstractPearlDiver.Low3, preparedTrits.Low[3]);
      Assert.AreEqual(AbstractPearlDiver.High0, preparedTrits.High[0]);
      Assert.AreEqual(AbstractPearlDiver.High1, preparedTrits.High[1]);
      Assert.AreEqual(AbstractPearlDiver.High2, preparedTrits.High[2]);
      Assert.AreEqual(AbstractPearlDiver.High3, preparedTrits.High[3]);

      preparedTrits = this.PrepareTrits(trits, 162);

      Assert.AreEqual(AbstractPearlDiver.Low0, preparedTrits.Low[162 + 0]);
      Assert.AreEqual(AbstractPearlDiver.Low1, preparedTrits.Low[162 + 1]);
      Assert.AreEqual(AbstractPearlDiver.Low2, preparedTrits.Low[162 + 2]);
      Assert.AreEqual(AbstractPearlDiver.Low3, preparedTrits.Low[162 + 3]);
      Assert.AreEqual(AbstractPearlDiver.High0, preparedTrits.High[162 + 0]);
      Assert.AreEqual(AbstractPearlDiver.High1, preparedTrits.High[162 + 1]);
      Assert.AreEqual(AbstractPearlDiver.High2, preparedTrits.High[162 + 2]);
      Assert.AreEqual(AbstractPearlDiver.High3, preparedTrits.High[162 + 3]);
    }
  }
}