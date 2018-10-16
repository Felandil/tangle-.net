namespace Tangle.Net.Unit.Tests.Repository.Client
{
  using System;
  using System.Collections.Generic;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Moq;

  using RestSharp;

  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Client;

  [TestClass]
  public class CircuitedClientTest
  {
    [TestMethod]
    public void TestClientTripsAfterException()
    {
      var restClient = new Mock<IRestClient>();
      restClient.Setup(c => c.Execute<object>(It.IsAny<RestRequest>())).Throws(new Exception());
      var client = new CircuitedClient(new RestClient()) { FailureThresholdPercentage = 100, ResetTimeoutMilliseconds = 1000 };
      try
      {
        client.ExecuteParameterizedCommand(new Dictionary<string, object> { { "command", CommandType.AttachToTangle } });
      }
      catch
      {
        // ignored
      }

      Assert.IsTrue(client.CircuitOpen);
    }
  }
}