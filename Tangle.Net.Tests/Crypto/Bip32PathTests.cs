using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangle.Net.Crypto;
using Tangle.Net.Crypto.Bip44;

namespace Tangle.Net.Tests.Crypto
{
  [TestClass]
  public class Bip32PathTests
  {
    [TestMethod]
    public void TestPathConstruction()
    {
      var path = new Bip32Path(Bip44AddressGenerator.IotaBip44BasePath);
      Assert.AreEqual(Bip44AddressGenerator.IotaBip44BasePath, path.ToString());
    }

    [TestMethod]
    public void TestPush()
    {
      var path = new Bip32Path(Bip44AddressGenerator.IotaBip44BasePath);
      path.Push(0);

      Assert.AreEqual(Bip44AddressGenerator.IotaBip44BasePath + "/0", path.ToString());
    }

    [TestMethod]
    public void TestPushHardended()
    {
      var path = new Bip32Path(Bip44AddressGenerator.IotaBip44BasePath);
      path.PushHardened(0);

      Assert.AreEqual(Bip44AddressGenerator.IotaBip44BasePath + "/0'", path.ToString());

    }

    [TestMethod]
    public void TestPop()
    {
      var path = new Bip32Path(Bip44AddressGenerator.IotaBip44BasePath);
      path.PushHardened(0);
      path.Pop();

      Assert.AreEqual(Bip44AddressGenerator.IotaBip44BasePath, path.ToString());
    }
  }
}
