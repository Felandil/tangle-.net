using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangle.Net.Tests.Entity
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;

  [TestClass]
  public class AddressTest
  {
    [TestMethod]
    public void TestAddressHasIncorrectLengthShouldThrowException()
    {
      var address = new Address("IAMNOTLONGENOUGH");
    }
  }
}
