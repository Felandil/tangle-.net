namespace Tangle.Net.Tests.Repository
{
  using System.Collections.Generic;

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

    /// <summary>
    /// The test get neighbors should include all parameters.
    /// </summary>
    [TestMethod]
    public void TestGetNeighborsShouldIncludeAllParameters()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<NeighborList>(It.IsAny<IRestRequest>())).Returns(
        () =>
        {
          var restResponse = new RestResponse<NeighborList>
          {
            Data =
              new NeighborList
                {
                  Duration = 1,
                  Neighbors = new List<Neighbor>
                                {
                                  new Neighbor
                                    {
                                      Address = "/8.8.8.8:14265",
                                      ConnectionType = "Remote",
                                      NumberOfAllTransactions = 45,
                                      NumberOfInvalidTransactions = 0,
                                      NumberOfNewTransactions = 5,
                                      NumberOfRandomTransactionRequests = 3,
                                      NumberOfSentTransactions = 40
                                    }
                                }
                }
          };
          return restResponse;
        });

      var repository = new RestIotaRepository(restClientMock.Object);
      var neighbors = repository.GetNeighbors();

      Assert.AreEqual(1, neighbors.Duration);
      Assert.AreEqual("/8.8.8.8:14265", neighbors.Neighbors[0].Address);
      Assert.AreEqual("Remote", neighbors.Neighbors[0].ConnectionType);
      Assert.AreEqual(45, neighbors.Neighbors[0].NumberOfAllTransactions);
      Assert.AreEqual(0, neighbors.Neighbors[0].NumberOfInvalidTransactions);
      Assert.AreEqual(5, neighbors.Neighbors[0].NumberOfNewTransactions);
      Assert.AreEqual(3, neighbors.Neighbors[0].NumberOfRandomTransactionRequests);
      Assert.AreEqual(40, neighbors.Neighbors[0].NumberOfSentTransactions);
    }

    /// <summary>
    /// The test get balances should include all parameters.
    /// </summary>
    [TestMethod]
    public void TestGetBalancesShouldIncludeAllParameters()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<AddressBalances>(It.IsAny<IRestRequest>())).Returns(
        () =>
        {
          var restResponse = new RestResponse<AddressBalances>
          {
            Data =
              new AddressBalances
                {
                  Balances = new List<long> { 10000 },
                  Duration = 100,
                  MilestoneIndex = 38274234,
                  References = new List<string> { "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB" }
                }
          };
          return restResponse;
        });

      var repository = new RestIotaRepository(restClientMock.Object);
      var balances = repository.GetBalances(new List<string> { "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB" }, 100);

      Assert.AreEqual(10000, balances.Balances[0]);
      Assert.AreEqual(100, balances.Duration);
      Assert.AreEqual(38274234, balances.MilestoneIndex);
      Assert.AreEqual("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB", balances.References[0]);
    }

    [TestMethod]
    public void TestGetTransactionsByAddressesShouldIncludeAllParameters()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<Transactions>(It.IsAny<IRestRequest>())).Returns(
        () =>
        {
          var restResponse = new RestResponse<Transactions>
          {
            Data =
              new Transactions
              {
                Hashes = new List<string>
                  {
                    "EJEAOOZYSAWFPZQESYDHZCGYNSTWXUMVJOVDWUNZJXDGWCLUFGIMZRMGCAZGKNPLBRLGUNYWKLJTYEAQX"
                  }
              }
          };
          return restResponse;
        });

      var repository = new RestIotaRepository(restClientMock.Object);
      var transactions = repository.GetTransactionsByAddresses(new List<string> { "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB" });

      Assert.AreEqual("EJEAOOZYSAWFPZQESYDHZCGYNSTWXUMVJOVDWUNZJXDGWCLUFGIMZRMGCAZGKNPLBRLGUNYWKLJTYEAQX", transactions.Hashes[0]);
    }

    #endregion
  }
}