namespace Tangle.Net.Integration.Tests
{
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using RestSharp;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Repository;

  /// <summary>
  /// The tangle tests.
  /// </summary>
  [TestClass]
  public class TangleTests
  {
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
    /// The test find transactions.
    /// </summary>
    [TestMethod]
    public void TestFindTransactions()
    {
      var transactions =
        this.repository.FindTransactionsByAddresses(
          new List<Address> { new Address("GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9") });

      Assert.IsTrue(transactions.Hashes.Any());
    }

    /// <summary>
    /// The test get tips.
    /// </summary>
    [TestMethod]
    public void TestGetTips()
    {
      var tips = this.repository.GetTips();
      Assert.IsTrue(tips.Hashes.Any());
    }

    #endregion
  }
}