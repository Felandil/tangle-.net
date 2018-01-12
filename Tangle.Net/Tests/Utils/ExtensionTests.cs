namespace Tangle.Net.Tests.Utils
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The extension tests.
  /// </summary>
  [TestClass]
  public class ExtensionTests
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test string is not of given length should fill with empty trytes.
    /// </summary>
    [TestMethod]
    public void TestStringIsNotOfGivenLengthShouldFillWithEmptyTrytes()
    {
      const string Value = "JHASJAHSAKS";
      Assert.AreEqual("JHASJAHSAKS99999", Value.FillTrytes(16));
    }

    #endregion
  }
}