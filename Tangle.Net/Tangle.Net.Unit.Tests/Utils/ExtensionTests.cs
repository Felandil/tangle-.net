namespace Tangle.Net.Unit.Tests.Utils
{
  using System.Linq;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Utils;

  /// <summary>
  /// The extension tests.
  /// </summary>
  [TestClass]
  public class ExtensionTests
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test get chunks on int array happy path.
    /// </summary>
    [TestMethod]
    public void TestGetChunksOnIntArrayHappyPath()
    {
      var value = new[] { 1, 2, 3, 4, 5, 6 };

      var chunks = value.GetChunks(2);

      Assert.AreEqual(3, chunks.Count());
    }

    /// <summary>
    /// The test get chunks on int array with uneven array length.
    /// </summary>
    [TestMethod]
    public void TestGetChunksOnIntArrayWithUnevenArrayLength()
    {
      var value = new[] { 1, 2, 3, 4, 5, 6, 7 };

      var chunks = value.GetChunks(2);

      Assert.AreEqual(4, chunks.Count());
      Assert.AreEqual(1, chunks[3].Count());
    }

    #endregion
  }
}