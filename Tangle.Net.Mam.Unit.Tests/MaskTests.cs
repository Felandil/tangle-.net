namespace Tangle.Net.Mam.Unit.Tests
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

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
      var payloadTrits = this.payload.ToTrits();
      var authIdTrits = this.authId.ToTrits();

      var cipher = new int[payloadTrits.Length];
      Array.Copy(payloadTrits, cipher, payloadTrits.Length);

      var mask = new CurlMask();

      var maskedCipher = mask.Mask(cipher, authIdTrits);
      var unmaskedCipher = mask.Unmask(maskedCipher, authIdTrits);

      Assert.AreEqual(this.payload.Value, Converter.TritsToTrytes(unmaskedCipher));
    }
  }
}