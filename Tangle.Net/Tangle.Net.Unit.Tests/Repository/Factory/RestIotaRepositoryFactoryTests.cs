namespace Tangle.Net.Unit.Tests.Repository.Factory
{
  using System;
  using System.Collections.Generic;
  using System.Reflection;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.ProofOfWork;
  using Tangle.Net.ProofOfWork.Service;
  using Tangle.Net.Repository.Factory;

  [TestClass]
  public class RestIotaRepositoryFactoryTests
  {
    [TestMethod]
    [ExpectedException(typeof(UriFormatException))]
    public void TestIncorrectUriShouldThrowException()
    {
      IotaRepositoryFactory.Create("asasfa");
    }

    [TestMethod]
    [ExpectedException(typeof(UriFormatException))]
    public void TestIncorrectUrisShouldThrowException()
    {
      IotaRepositoryFactory.CreateWithFallback(new List<string> { "asasfaf", "hasafafa" });
    }

    [TestMethod]
    public void TestLocalPoWTypeSingleNode()
    {
      var repository = IotaRepositoryFactory.Create("http://somenode.uri:1234");
      var powService = repository.GetType().GetProperty("PoWService", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(repository);

      Assert.IsInstanceOfType(powService, typeof(PoWService));
    }

    [TestMethod]
    public void TestLocalPoWTypeMultiNode()
    {
      var repository = IotaRepositoryFactory.CreateWithFallback(new List<string> { "http://somenode.uri:1234" });
      var powService = repository.GetType().GetProperty("PoWService", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(repository);

      Assert.IsInstanceOfType(powService, typeof(PoWService));
    }

    [TestMethod]
    public void TestRemotePoWTypeSingleNode()
    {
      var repository = IotaRepositoryFactory.Create("http://somenode.uri:1234", PoWType.Remote);
      var powService = repository.GetType().GetProperty("PoWService", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(repository);

      Assert.IsInstanceOfType(powService, typeof(RestPoWService));
    }

    [TestMethod]
    public void TestRemotePoWTypeMultiNode()
    {
      var repository = IotaRepositoryFactory.CreateWithFallback(new List<string> { "http://somenode.uri:1234" }, PoWType.Remote);
      var powService = repository.GetType().GetProperty("PoWService", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(repository);

      Assert.IsInstanceOfType(powService, typeof(RestPoWService));
    }

    [TestMethod]
    public void TestPoWSrvPoWTypeSingleNode()
    {
      var repository = IotaRepositoryFactory.Create("http://somenode.uri:1234", PoWType.PoWSrv);
      var powService = repository.GetType().GetProperty("PoWService", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(repository);

      Assert.IsInstanceOfType(powService, typeof(PoWSrvService));
    }

    [TestMethod]
    public void TestPoWSrvPoWTypeMultiNode()
    {
      var repository = IotaRepositoryFactory.CreateWithFallback(new List<string> { "http://somenode.uri:1234" }, PoWType.PoWSrv);
      var powService = repository.GetType().GetProperty("PoWService", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(repository);

      Assert.IsInstanceOfType(powService, typeof(PoWSrvService));
    }
  }
}