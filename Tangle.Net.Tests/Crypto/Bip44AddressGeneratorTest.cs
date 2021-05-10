using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangle.Net.Crypto;
using Tangle.Net.Crypto.Bip44;

namespace Tangle.Net.Tests.Crypto
{
  [TestClass]
  public class Bip44AddressGeneratorTest
  {
    [TestMethod]
    public void TestAddressGeneration()
    {
      var path = Bip44AddressGenerator.GenerateAddress(1, 1, false);
      Assert.AreEqual("m/44'/4218'/1'/0'/1'", path.ToString());
    }

    [TestMethod]
    public void TestAddressGenerationFromStateWithFirstAddress()
    {
      var path = Bip44AddressGenerator.GenerateAddress(new DefaultBip44GeneratorState {IsInternal = true}, true);
      Assert.AreEqual("m/44'/4218'/0'/1'/0'", path.ToString());
    }

    [TestMethod]
    public void TestAddressGenerationFromStateWithSecondAddress()
    {
      var path = Bip44AddressGenerator.GenerateAddress(new DefaultBip44GeneratorState {IsInternal = true}, false);
      Assert.AreEqual("m/44'/4218'/0'/0'/1'", path.ToString());
    }
  }
}