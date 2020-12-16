using System;
using System.Collections.Generic;
using System.Text;

namespace Tangle.Net.Tests.Services
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Utils;

  [TestClass]
  public class ConverterTest
  {
    [TestMethod]
    public void TestConversionToHex()
    {
      var expected = "48656c6c6f20776f726c6421";
      var given = "Hello world!";

      Assert.AreEqual(expected, given.ToHex());
    }

    [TestMethod]
    public void TestConversionFromHex()
    {
      var given = "48656c6c6f20776f726c6421";
      var expected = "Hello world!";

      Assert.AreEqual(expected, given.HexToString());
    }

    [TestMethod]
    public void TestHexAndByteConversion()
    {
      var value = "48656c6c6f20776f726c6421";
      var asBytes = value.Utf8ToBytes();

      var hex = asBytes.ToHex();
      var backToBytes = hex.HexToBytes();

      CollectionAssert.AreEqual(asBytes, backToBytes);
    }
  }
}
