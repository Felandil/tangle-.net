using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangle.Net.Api.HighLevel.Request;

namespace Tangle.Net.Tests.Api.HighLevel
{
  [TestClass]
  public class SendDataRequestTest
  {
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestIndexationKeyIsEmpty()
    {
      var request = new SendDataRequest(string.Empty, "");
      request.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestIndexationKeyTooLong()
    {
      {
        var request =
          new SendDataRequest(
            "8374598275781252890532895328952893573289529875293523897561289561287561897456128743612879461897461289746125129875129571985712985719257219857125",
            "");
        request.Validate();
      }
    }
  }
}