using System;
using System.Collections.Generic;
using System.Text;

namespace Tangle.Net.Tests.Entity.Message.Payload
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity.Message.Payload;
  using Tangle.Net.Utils;

  [TestClass]
  public class PayloadTest
  {
    [TestMethod]
    public void TestIndexationPayloadSerialization()
    {
      var payload = new IndexationPayload { Index = "Tangle .Net", Data = "Hello world!".ToHex() };

      var expected = new byte[]
                       {
                         33, 0, 0, 0, 2, 0, 0, 0, 11, 0, 84, 97, 110, 103, 108, 101, 32, 46, 78, 101, 116, 12, 0, 0, 0, 72, 101, 108, 108, 111, 32,
                         119, 111, 114, 108, 100, 33
                       };

      var actual = payload.Serialize();

      CollectionAssert.AreEqual(expected, actual);
    }
  }
}
