namespace Tangle.Net.Area.Codes.Tests.Entity
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Area.Codes.Entity;

  [TestClass]
  public class IotaAreaCodeDimensionTest
  {
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestPrecisionIsInvalidShouldThrowException()
    {
      var dimension = IotaAreaCodeDimension.GetByPrecision(12);
    }

    [DataTestMethod]
    [DataRow(2, 20D, "20°", 2200000D, "2200km")]
    [DataRow(4, 1D, "1°", 110000D, "110km")]
    [DataRow(6, 0.05D, "0.05°", 5500D, "5.5km")]
    [DataRow(8, 0.0025D, "0.0025°", 275D, "275m")]
    [DataRow(10, 0.000125D, "0.000125°", 14D, "14m")]
    [DataRow(11, null, "", 3.5D, "3.5m")]
    public void TestPrecisionIsValidShouldReturnDimension(int precision, double? sizeDegree, string sizeDegreeFormatted, double sizeMetres, string sizeMetresFormatted)
    {
      var dimension = IotaAreaCodeDimension.GetByPrecision(precision);

      Assert.AreEqual(sizeDegree, dimension.BlocksSizeDegrees);
      Assert.AreEqual(sizeDegreeFormatted, dimension.BlocksSizeDegreesFormatted);
      Assert.AreEqual(sizeMetres, dimension.SizeMetres);
      Assert.AreEqual(sizeMetresFormatted, dimension.SizeMetresFormatted);
    }
  }
}