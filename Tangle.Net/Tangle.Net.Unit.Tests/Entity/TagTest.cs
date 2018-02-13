namespace Tangle.Net.Unit.Tests.Entity
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;

  /// <summary>
  /// The tag test.
  /// </summary>
  [TestClass]
  public class TagTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test length of tag is automatically padded to tag length.
    /// </summary>
    [TestMethod]
    public void TestLengthOfTagIsAutomaticallyPaddedToTagLength()
    {
      var tag = new Tag("HG");
      Assert.AreEqual("HG9999999999999999999999999", tag.Value);

      tag = new Tag();
      Assert.AreEqual("999999999999999999999999999", tag.Value);
    }

    /// <summary>
    /// The test trying to create a tag longer than tag max length throws exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestTryingToCreateATagLongerThanTagMaxLengthThrowsException()
    {
      var tag = new Tag("HG999999999999999999999999999");
    }

    [TestMethod]
    public void TestTagEmptyShouldReturnEmptyTag()
    {
      var tag = Tag.Empty;
      Assert.AreEqual("999999999999999999999999999", tag.Value);
    }

    #endregion
  }
}