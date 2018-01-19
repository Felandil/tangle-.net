namespace Tangle.Net.Integration.Tests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using RestSharp;

  using Tangle.Net.Source.Repository;
  using Tangle.Net.Source.Repository.DataTransfer;

  /// <summary>
  /// The node tests.
  /// </summary>
  [TestClass]
  public class NodeTests
  {
    // we only test if operations succeed on a real API. Everything else is covered with unit tests
    #region Fields

    /// <summary>
    /// The repository.
    /// </summary>
    private IIotaRepository repository;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The setup.
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
      this.repository = new RestIotaRepository(new RestClient("http://localhost:14265"));
    }

    /// <summary>
    /// The test get neighbors.
    /// </summary>
    [TestMethod]
    public void TestGetNeighbors()
    {
      var neighborList = this.repository.GetNeighbors();
      Assert.IsTrue(neighborList.Neighbors.Any());
    }

    /// <summary>
    /// The test get node info.
    /// </summary>
    [TestMethod]
    public void TestGetNodeInfo()
    {
      var nodeInfo = this.repository.GetNodeInfo();
      Assert.AreEqual("IRI", nodeInfo.AppName);
    }

    /// <summary>
    /// The test neighbor removing and addition.
    /// </summary>
    [TestMethod]
    public void TestNeighborRemovingAndAddition()
    {
      // obtain neighborList for real neighbors
      var neighborList = this.repository.GetNeighbors();
      var neighbor = neighborList.Neighbors[0];

      // generate corrctly formed URI
      neighbor.Address = new Uri(neighbor.ConnectionType + "://" + neighbor.Address).ToString();

      var neighborsToTestWith = new List<Neighbor>() { neighbor };
      var removeResult = this.repository.RemoveNeighbors(neighborsToTestWith);
      Assert.AreEqual(1, removeResult.RemovedNeighbors);

      var addResult = this.repository.AddNeighbor(neighborsToTestWith);
      Assert.AreEqual(1, addResult.AddedNeighbors);
    }

    #endregion
  }
}