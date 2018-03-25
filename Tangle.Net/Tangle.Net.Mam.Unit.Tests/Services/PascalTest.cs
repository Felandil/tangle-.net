namespace Tangle.Net.Mam.Unit.Tests.Services
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Mam.Services;

  /// <summary>
  /// The pascal test.
  /// </summary>
  [TestClass]
  public class PascalTest
  {
    /// <summary>
    /// The test value encrytption.
    /// </summary>
    [TestMethod]
    public void TestValueEncrytption()
    {
      var encoded = Pascal.Encode(0);
      var expected = new[] { 1, 0, 0, -1 };

      for (var i = 0; i < expected.Length; i++)
      {
        Assert.AreEqual(expected[i], encoded[i]);
      }

      encoded = Pascal.Encode(9);
      expected = new[] { 0, 0, 1, 0 };

      for (var i = 0; i < expected.Length; i++)
      {
        Assert.AreEqual(expected[i], encoded[i]);
      }

      encoded = Pascal.Encode(1000);
      expected = new[] { -1, 0, 0, -1, 0, -1, 1, 0, 0, 0, 1, 0 };

      for (var i = 0; i < expected.Length; i++)
      {
        Assert.AreEqual(expected[i], encoded[i]);
      }
    }
  }
}