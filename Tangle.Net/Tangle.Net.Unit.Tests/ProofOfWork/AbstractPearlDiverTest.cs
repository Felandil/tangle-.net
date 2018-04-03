namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
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

      Assert.AreEqual(this.Low0, preparedTrits.Low[0]);
      Assert.AreEqual(this.Low1, preparedTrits.Low[1]);
      Assert.AreEqual(this.Low2, preparedTrits.Low[2]);
      Assert.AreEqual(this.Low3, preparedTrits.Low[3]);
      Assert.AreEqual(this.High0, preparedTrits.High[0]);
      Assert.AreEqual(this.High1, preparedTrits.High[1]);
      Assert.AreEqual(this.High2, preparedTrits.High[2]);
      Assert.AreEqual(this.High3, preparedTrits.High[3]);

      preparedTrits = this.PrepareTrits(trits, 162);

      Assert.AreEqual(this.Low0, preparedTrits.Low[162 + 0]);
      Assert.AreEqual(this.Low1, preparedTrits.Low[162 + 1]);
      Assert.AreEqual(this.Low2, preparedTrits.Low[162 + 2]);
      Assert.AreEqual(this.Low3, preparedTrits.Low[162 + 3]);
      Assert.AreEqual(this.High0, preparedTrits.High[162 + 0]);
      Assert.AreEqual(this.High1, preparedTrits.High[162 + 1]);
      Assert.AreEqual(this.High2, preparedTrits.High[162 + 2]);
      Assert.AreEqual(this.High3, preparedTrits.High[162 + 3]);
    }

    /// <inheritdoc />
    public AbstractPearlDiverTest()
      : base(CurlMode.CurlP27)
    {
    }

    /// <inheritdoc />
    public override ulong High0 => 13176245766935394011;

    /// <inheritdoc />
    public override ulong High1 => 14403622084951293727;

    /// <inheritdoc />
    public override ulong High2 => 18445620372817592319;

    /// <inheritdoc />
    public override ulong High3 => 2199023255551;

    /// <inheritdoc />
    public override ulong Low0 => 15811494920322472813;

    /// <inheritdoc />
    public override ulong Low1 => 17941353825114769379;

    /// <inheritdoc />
    public override ulong Low2 => 576458557575118879;

    /// <inheritdoc />
    public override ulong Low3 => 18446741876833779711;
  }
}