namespace Tangle.Net.Area.Codes.Tests.Entity
{
  using System;

  using Google.OpenLocationCode;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Area.Codes.Entity;

  [TestClass]
  public class IotaAreaCodesTest
  {
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void TestInvalidConstructorValue()
    {
      var areaCode = new IotaAreaCode("NPHTQORL999XKP");
    }

    [TestMethod]
    public void TestAreaCodeEncodingExtraPrecision()
    {
      var areaCode = IotaAreaCode.Encode(52.529562, 13.413047, OpenLocationCode.CodePrecisionExtra);
      Assert.AreEqual("NPHTQORL9XKP", areaCode.Value);
    }

    [TestMethod]
    public void TestAreaCodeEncodingNormalPrecision()
    {
      var areaCode = IotaAreaCode.Encode(52.529562, 13.413047);
      Assert.AreEqual("NPHTQORL9XK", areaCode.Value);
    }

    [TestMethod]
    public void TestAreCodeDecodingNormalPrecision()
    {
      var areaCode = new IotaAreaCode("NPHTQORL9XK");
      var iacAreaCode = areaCode.Decode();

      Assert.AreEqual(10, iacAreaCode.CodePrecision);
      Assert.AreEqual(52.5295625f, (float)iacAreaCode.Latitude);
      Assert.AreEqual(13.4130625f, (float)iacAreaCode.Longitude);
    }

    [TestMethod]
    public void TestAreCodeDecodingExtraPrecision()
    {
      var areaCode = new IotaAreaCode("NPHTQORL9XKP");
      var iacAreaCode = areaCode.Decode();

      Assert.AreEqual(11, iacAreaCode.CodePrecision);
      Assert.AreEqual(52.52956250000001f, (float)iacAreaCode.Latitude);
      Assert.AreEqual(13.413046874999981f, (float)iacAreaCode.Longitude);
      Assert.AreEqual(52.529575000000015f, (float)iacAreaCode.LatitudeHigh);
      Assert.AreEqual(52.529550000000015f, (float)iacAreaCode.LatitudeLow);
      Assert.AreEqual(13.413062499999983f, (float)iacAreaCode.LongitudeHigh);
      Assert.AreEqual(13.413031249999982f, (float)iacAreaCode.LongitudeLow);

      Assert.AreEqual(11, areaCode.Area.CodePrecision);
      Assert.AreEqual(52.52956250000001f, (float)areaCode.Area.Latitude);
      Assert.AreEqual(13.413046874999981f, (float)areaCode.Area.Longitude);
      Assert.AreEqual(52.529575000000015f, (float)areaCode.Area.LatitudeHigh);
      Assert.AreEqual(52.529550000000015f, (float)areaCode.Area.LatitudeLow);
      Assert.AreEqual(13.413062499999983f, (float)areaCode.Area.LongitudeHigh);
      Assert.AreEqual(13.413031249999982f, (float)areaCode.Area.LongitudeLow);
    }

    [TestMethod]
    public void TestPrecisionCalculation()
    {
      Assert.AreEqual(11, new IotaAreaCode("NPHTQORL9XKP").Area.CodePrecision);
      Assert.AreEqual(10, new IotaAreaCode("NPHTQORL9XK").Area.CodePrecision);
      Assert.AreEqual(8, new IotaAreaCode("NPHTQORL9").Area.CodePrecision);
      Assert.AreEqual(6, new IotaAreaCode("NPHTQOAA9").Area.CodePrecision);
      Assert.AreEqual(4, new IotaAreaCode("NPHTAAAA9").Area.CodePrecision);
      Assert.AreEqual(2, new IotaAreaCode("NPAAAAAA9").Area.CodePrecision);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void TestPrecisionDecreaseIsNotPossibleShouldThrowException()
    {
      var areaCode = new IotaAreaCode("NPAAAAAA9");
      areaCode.DecreasePrecision();
    }

    [TestMethod]
    public void TestSuccessfulPrecisionDecrease()
    {
      var areaCode = new IotaAreaCode("NPHTQORL9XKP");
      Assert.AreEqual(11, areaCode.Area.CodePrecision);

      Assert.AreEqual(10, areaCode.DecreasePrecision().CodePrecision);
      Assert.AreEqual("NPHTQORL9XK", areaCode.Value);

      Assert.AreEqual(8, areaCode.DecreasePrecision().Area.CodePrecision);
      Assert.AreEqual("NPHTQORL9", areaCode.Value);

      Assert.AreEqual(6, areaCode.DecreasePrecision().CodePrecision);
      Assert.AreEqual("NPHTQOAA9", areaCode.Value);

      Assert.AreEqual(4, areaCode.DecreasePrecision().CodePrecision);
      Assert.AreEqual("NPHTAAAA9", areaCode.Value);

      Assert.AreEqual(2, areaCode.DecreasePrecision().CodePrecision);
      Assert.AreEqual("NPAAAAAA9", areaCode.Value);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void TestPrecisionIncreaseIsNotPossibleShouldThrowException()
    {
      var areaCode = new IotaAreaCode("NPHTQORL9XKP");
      areaCode.IncreasePrecision();
    }

    [TestMethod]
    public void TestSuccessfulPrecisionIncrease()
    {
      var areaCode = new IotaAreaCode("NPAAAAAA9");
      Assert.AreEqual(2, areaCode.Area.CodePrecision);

      Assert.AreEqual(4, areaCode.IncreasePrecision().CodePrecision);
      Assert.AreEqual("NPQQAAAA9", areaCode.Value);

      Assert.AreEqual(6, areaCode.IncreasePrecision().Area.CodePrecision);
      Assert.AreEqual("NPQQQQAA9", areaCode.Value);

      Assert.AreEqual(8, areaCode.IncreasePrecision().CodePrecision);
      Assert.AreEqual("NPQQQQQQ9", areaCode.Value);

      Assert.AreEqual(10, areaCode.IncreasePrecision().CodePrecision);
      Assert.AreEqual("NPQQQQQQ9QQ", areaCode.Value);

      Assert.AreEqual(11, areaCode.IncreasePrecision().CodePrecision);
      Assert.AreEqual("NPQQQQQQ9QQQ", areaCode.Value);
    }

    [TestMethod]
    public void TestConversionToOpenLocationCode()
    {
      Assert.AreEqual("9F4MGCH7+R6F", IotaAreaCode.ToOpenLocationCode("NPHTQORL9XKP"));
      Assert.AreEqual("9F4MGCH7+R6F", new IotaAreaCode("NPHTQORL9XKP").ToOpenLocationCode());
    }

    [TestMethod]
    public void TestExtractCodeFromTrytes()
    {
      var areaCode = IotaAreaCode.Extract("NPHTQORL9XKP999999999");
      Assert.AreEqual("NPHTQORL9XKP", areaCode.Value);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestTrytesToExtractAreInvalidShouldThrowException()
    {
      IotaAreaCode.Extract("BNBPHBTQBOBRBLB9XKP999999999");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestSetPrecisionIsOutOfBoundsShouldThrowException()
    {
      new IotaAreaCode("NPHTQORL9").SetPrecision(0);
    }

    [TestMethod]
    public void TestSetPrecision()
    {
      var areaCode = new IotaAreaCode("NPHTQORL9").SetPrecision(4);
      Assert.AreEqual("NPHTAAAA9", areaCode.Value);
    }

    [TestMethod]
    public void TestConversionFromOpenLocationCode()
    {
      var areaCode = IotaAreaCode.FromOpenLocationCode("X4HM+MM");
      Assert.AreEqual("ZHRT9TT", areaCode.Value);
    }

    [TestMethod]
    public void TestIotaAreaCodeIsInvalid()
    {
      Assert.IsFalse(IotaAreaCode.IsValid("JAHAS0"));
    }

    [TestMethod]
    public void TestIotaAreaCodeIsInvalidWithCorrectTrytes()
    {
      Assert.IsFalse(IotaAreaCode.IsValid("NPHTQORL999XKP"));
    }

    [TestMethod]
    public void TestIotaAreaCodeIsValid()
    {
      Assert.IsTrue(IotaAreaCode.IsValid("NPHTQORL9XKP"));
    }

    [TestMethod]
    public void TestPartialAreaCodeIsInvalid()
    {
      Assert.IsFalse(IotaAreaCode.IsValidPartial("JAHAS"));
      Assert.IsFalse(IotaAreaCode.IsValidPartial("JAHASJAHAS"));

      Assert.IsFalse(IotaAreaCode.IsValidPartial("AA9"));
      Assert.IsFalse(IotaAreaCode.IsValidPartial("AAA9"));

      Assert.IsFalse(IotaAreaCode.IsValidPartial("BAAA9"));
    }

    [TestMethod]
    public void TestPartialAreaCodeIsValid()
    {
      Assert.IsTrue(IotaAreaCode.IsValid("NPAAAAAA9"));
    }
  }
}