namespace Tangle.Net.Mam.Unit.Tests
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
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

      var maskedCipher = mask.Mask(this.payload, this.authId);
      var unmaskedCipher = mask.Unmask(maskedCipher, this.authId);

      Assert.AreEqual(this.payload.Value, unmaskedCipher.Value);
    }
  }
}