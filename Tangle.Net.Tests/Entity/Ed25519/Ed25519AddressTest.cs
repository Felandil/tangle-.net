using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Tests.Entity.Ed25519
{
  [TestClass]
  public class Ed25519AddressTest
  {
    [TestMethod]
    public void TestAddressGenerationFromPublicKey()
    {
      var expected = "0bebd6f0febfaf32b4c77eae6755b1d83478dcc9185c33c318d4b0a24ba54d68";

      var seed = Ed25519Seed.FromMnemonic("witch collapse practice feed shame open despair creek road again ice least");
      var address = Ed25519Address.FromPublicKey(seed.KeyPair.PublicKey);

      Assert.AreEqual(expected, address.Address);
    }
  }
}
