using System;
using System.Collections.Generic;
using System.Text;

namespace Tangle.Net.Tests.ProofOfWork
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.ProofOfWork;

  [TestClass]
  public class LocalPowProviderTest
  {
    [TestMethod]
    public void TestPowCalculation()
    {
      var powProvider = new LocalPowProvider();
      var nonce = powProvider.DoPow(
        new byte[]
          {
            139, 74, 107, 29, 67, 49, 132, 238, 147, 153, 254, 191, 215, 194, 73, 54, 153, 220, 121, 116, 56, 115, 84, 181, 67, 168, 204, 118, 129, 0,
            12, 57
          },
        100);

      Assert.AreEqual(3866, nonce);
    }
  }
}
