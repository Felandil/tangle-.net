namespace Tangle.Net.Mam.Unit.Tests
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Services;
  using Tangle.Net.Utils;

  /// <summary>
  /// The mask tests.
  /// </summary>
  [TestClass]
  public class MaskTests
  {
    #region Fields

    /// <summary>
    /// The auth id.
    /// </summary>
    private TryteString authId = new TryteString("MYMERKLEROOTHASH");

    /// <summary>
    /// The payload.
    /// </summary>
    private TryteString payload =
      new TryteString(
        "AAMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9AMESSAGEFORYOU9MESSAGEFORYOU9");

    #endregion

    /// <summary>
    /// The test mask unmask.
    /// </summary>
    [TestMethod]
    public void TestMaskUnmask()
    {
      var mask = new CurlMask();
      var curl = new Curl(CurlMode.CurlP27);
      curl.Absorb(this.authId.ToTrits());
      var payloadTrits = this.payload.ToTrits();

      mask.Mask(payloadTrits, curl);

      curl.Reset();
      curl.Absorb(this.authId.ToTrits());
      var unmasked = mask.Unmask(payloadTrits, curl);

      Assert.AreEqual(this.payload.Value, Converter.TritsToTrytes(unmasked));
    }

    /// <summary>
    /// The test hashing.
    /// </summary>
    [TestMethod]
    public void TestHashing()
    {
      var mask = new CurlMask();
      var hash = mask.Hash(new TryteString("L9DRGFPYDMGVLH9ZCEWHXNEPC9TQQSA9W9FZVYXLBMJTHJC9HZDONEJMMVJVEMHWCIBLAUYBAUFQOMYSN"));

      Assert.AreEqual("MMUKJ9EXZHFXVLVQSUFPHAHXPFNMPOZRHAIIMANCQNQRHSPRIN9NTPQKLDRRGJGNRAJTTWKJJBPXYWDGB", hash.Value);
    }
  }
}