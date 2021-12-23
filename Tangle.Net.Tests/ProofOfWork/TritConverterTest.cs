using System;
using System.Collections.Generic;
using System.Text;

namespace Tangle.Net.Tests.ProofOfWork
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Newtonsoft.Json;

  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Utils;

  [TestClass]
  public class TritConverterTest
  {
    [TestMethod]
    public void TestByteToTritConversion()
    {
      var bytes = new byte[]
                    {
                      139, 74, 107, 29, 67, 49, 132, 238, 147, 153, 254, 191, 215, 194, 73, 54, 153, 220, 121, 116, 56, 115, 84, 181, 67, 168, 204,
                      118, 129, 0, 12, 57
                    };

      var destination = new int[243];
      var tritsLength = TritConverter.Encode(bytes, ref destination);
      var expected = new[]
                       {
                         0, 0, -1, -1, -1, 0, -1, 1, -1, 0, 1, 0, -1, 0, 0, 1, 1, 0, -1, 1, 0, 1, 0, 0, 1, 1, 1, -1, 1, 0, 1, 1, -1, -1, 1, 0, -1, 1,
                         1, 1, 1, -1, 0, 0, 1, -1, 0, 0, -1, 0, 0, -1, -1, 0, -1, -1, 1, -1, -1, 0, 1, -1, 0, 0, 0, 0, 1, -1, -1, 1, -1, 0, 1, 1, 1,
                         1, -1, 0, 1, 0, -1, 1, -1, 0, 1, 0, -1, 0, 1, 0, 0, 0, 0, -1, 1, 0, -1, -1, 1, -1, -1, 0, 0, 0, -1, -1, 0, 0, 1, 1, 1, 1, 1,
                         0, -1, 0, 1, 1, 1, 0, -1, 1, 0, -1, 1, 0, 1, -1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, -1, 1, 0, -1, 0, 1, 1, 1, -1, 1, 0, -1, 1,
                         -1, 0, -1, 0, -1, 1, 0, 1, -1, 0, 1, 0, 1, 1, 1, 0, -1, 0, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, -1, 1,
                         0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                         0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                       };

      Assert.AreEqual(192, tritsLength);
      CollectionAssert.AreEqual(expected, destination);
    }
  }
}


