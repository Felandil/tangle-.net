using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangle.Net.Crypto;
using Tangle.Net.Crypto.Bip44;
using Tangle.Net.Entity.Bech32;
using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Tests.Entity.Bech32
{
  [TestClass]
  public class Bech32AddressTest
  {
    [TestMethod]
    public void TestAddressCreationFromEd25519AddressAndBip31Path()
    {
      var expectedAddressBech32Address = "iota1qq97h4hsl6l67v45cal2ue64k8vrg7xueyv9cv7rrr2tpgjt54xks8esknu";

      var address = new Ed25519Address
      {
        Address = "0bebd6f0febfaf32b4c77eae6755b1d83478dcc9185c33c318d4b0a24ba54d68", Balance = 100, DustAllowed = true
      };

      var path = Bip44AddressGenerator.GenerateAddress(1, 1, false);
      var actual = Bech32Address.FromEd25519Address(address, path, "iota");

      Assert.AreEqual(expectedAddressBech32Address, actual.Address);
      Assert.AreEqual(path.ToString(), actual.Path.ToString());
      Assert.AreEqual(address.Balance, actual.Balance);
    }
  }
}
