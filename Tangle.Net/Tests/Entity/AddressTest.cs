namespace Tangle.Net.Tests.Entity
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The address test.
  /// </summary>
  [TestClass]
  public class AddressTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test address does not include checksum should have invalid checksum.
    /// </summary>
    [TestMethod]
    public void TestAddressDoesNotIncludeChecksumShouldHaveInvalidChecksum()
    {
      var address = new Address("UYEEERFQYTPFAHIPXDQAQYWYMSMCLMGBTYAXLWFRFFWPYFOICOVLK9A9VYNCKK9TQUNBTARCEQXJHD9VY");

      Assert.IsFalse(address.HasValidChecksum());
    }

    /// <summary>
    /// The test address does not include checksum shouldgenerate correct checksum.
    /// </summary>
    [TestMethod]
    public void TestAddressDoesNotIncludeChecksumShouldGenerateCorrectChecksum()
    {
      var address = new Address("UYEEERFQYTPFAHIPXDQAQYWYMSMCLMGBTYAXLWFRFFWPYFOICOVLK9A9VYNCKK9TQUNBTARCEQXJHD9VY");
      var addressWithChecksum = address.WithChecksum();

      Assert.AreEqual("XOEDEOMRC", addressWithChecksum.Checksum.Value);
      Assert.AreEqual("UYEEERFQYTPFAHIPXDQAQYWYMSMCLMGBTYAXLWFRFFWPYFOICOVLK9A9VYNCKK9TQUNBTARCEQXJHD9VYXOEDEOMRC", addressWithChecksum.ToTrytes());
    }

    /// <summary>
    /// The test address has incorrect length should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestAddressHasIncorrectLengthShouldThrowException()
    {
      var address = new Address("IAMNOTLONGENOUGH");
    }

    /// <summary>
    /// The test address includes checksum should split into address value and checksum correctly.
    /// </summary>
    [TestMethod]
    public void TestAddressIncludesChecksumShouldSplitIntoAddressValueAndChecksumCorrectly()
    {
      var address = new Address("UYEEERFQYTPFAHIPXDQAQYWYMSMCLMGBTYAXLWFRFFWPYFOICOVLK9A9VYNCKK9TQUNBTARCEQXJHD9VYXOEDEOMRC");

      Assert.AreEqual("UYEEERFQYTPFAHIPXDQAQYWYMSMCLMGBTYAXLWFRFFWPYFOICOVLK9A9VYNCKK9TQUNBTARCEQXJHD9VY", address.Value);
      Assert.AreEqual("XOEDEOMRC", address.Checksum.Value);
    }

    /// <summary>
    /// The test address includes checksum should split into address value and checksum correctly.
    /// </summary>
    [TestMethod]
    public void TestAddressIncludesChecksumWhichIsInvalid()
    {
      var address = new Address("UYEEERFQYTPFAHIPXDQAQYWYMSMCLMGBTYAXLWFRFFWPYFOICOVLK9A9VYNCKK9TQUNBTARCEQXJHD9VYXOEDEOMR9");

      Assert.IsFalse(address.HasValidChecksum());
    }

    /// <summary>
    /// The test address includes checksum which is valid.
    /// </summary>
    [TestMethod]
    public void TestAddressIncludesChecksumWhichIsValid()
    {
      var address = new Address("UYEEERFQYTPFAHIPXDQAQYWYMSMCLMGBTYAXLWFRFFWPYFOICOVLK9A9VYNCKK9TQUNBTARCEQXJHD9VYXOEDEOMRC");

      Assert.IsTrue(address.HasValidChecksum());
    }

    #endregion
  }
}