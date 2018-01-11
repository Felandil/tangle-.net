namespace Tangle.Net.Tests.Repository
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Moq;

  using RestSharp;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Repository;

  /// <summary>
  /// The rest iota repository test.
  /// </summary>
  [TestClass]
  public class RestIotaRepositoryTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test get node info should include all parameters.
    /// </summary>
    [TestMethod]
    public void TestGetNodeInfoShouldIncludeAllParameters()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<NodeInfo>(It.IsAny<IRestRequest>())).Returns(
        () =>
          {
            var restResponse = new RestResponse<NodeInfo>
                                 {
                                   Data =
                                     new NodeInfo
                                       {
                                         AppName = "IRI", 
                                         AppVersion = "1.4.1.1", 
                                         Duration = 0, 
                                         JreAvailableProcessors = 4, 
                                         JreFreeMemory = 1433752216, 
                                         JreMaxMemory = 4294967296, 
                                         JreTotalMemory = 4294967296, 
                                         LatestMilestone =
                                           "MWBOADDZVATGJWCZSEQSPJSEEMGROZOGEERYBTIBZIKTJTXGNKMKWDFKIJOAKEPEJBGEMZLE9UVCA9999", 
                                         LatestMilestoneIndex = 325116, 
                                         LatestSolidSubtangleMilestone =
                                           "MWBOADDZVATGJWCZSEQSPJSEEMGROZOGEERYBTIBZIKTJTXGNKMKWDFKIJOAKEPEJBGEMZLE9UVCA9999", 
                                         LatestSolidSubtangleMilestoneIndex = 325116, 
                                         Neighbors = 6, 
                                         PacketsQueueSize = 0, 
                                         Time = 1515707837727, 
                                         Tips = 5946, 
                                         TransactionsToRequest = 550
                                       }
                                 };
            return restResponse;
          });

      var repository = new RestIotaRepository(restClientMock.Object);
      var nodeInfo = repository.GetNodeInfo();

      Assert.AreEqual("IRI", nodeInfo.AppName);
      Assert.AreEqual("1.4.1.1", nodeInfo.AppVersion);
      Assert.AreEqual(0, nodeInfo.Duration);
      Assert.AreEqual(4, nodeInfo.JreAvailableProcessors);
      Assert.AreEqual(1433752216, nodeInfo.JreFreeMemory);
      Assert.AreEqual(4294967296, nodeInfo.JreMaxMemory);
      Assert.AreEqual(4294967296, nodeInfo.JreTotalMemory);
      Assert.AreEqual("MWBOADDZVATGJWCZSEQSPJSEEMGROZOGEERYBTIBZIKTJTXGNKMKWDFKIJOAKEPEJBGEMZLE9UVCA9999", nodeInfo.LatestMilestone);
      Assert.AreEqual(325116, nodeInfo.LatestMilestoneIndex);
      Assert.AreEqual("MWBOADDZVATGJWCZSEQSPJSEEMGROZOGEERYBTIBZIKTJTXGNKMKWDFKIJOAKEPEJBGEMZLE9UVCA9999", nodeInfo.LatestSolidSubtangleMilestone);
      Assert.AreEqual(325116, nodeInfo.LatestSolidSubtangleMilestoneIndex);
      Assert.AreEqual(6, nodeInfo.Neighbors);
      Assert.AreEqual(0, nodeInfo.PacketsQueueSize);
      Assert.AreEqual(1515707837727, nodeInfo.Time);
      Assert.AreEqual(5946, nodeInfo.Tips);
      Assert.AreEqual(550, nodeInfo.TransactionsToRequest);
    }

    #endregion
  }
}