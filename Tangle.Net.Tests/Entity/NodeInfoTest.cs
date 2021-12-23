using System;
using System.Collections.Generic;
using System.Text;

namespace Tangle.Net.Tests.Entity
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;

  [TestClass]
  public class NodeInfoTest
  {
    [TestMethod]
    public void TestNetworkIdGeneration()
    {
      var info = new NodeInfo { NetworkId = "alphanet1" };
      Assert.AreEqual("6530425480034647824", info.CalculateMessageNetworkId());
    }
  }
}
