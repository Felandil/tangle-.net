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

      transactions =
        this.repository.FindTransactionsByBundles(
          new List<Hash> { new Hash("JSHICEYJKLEQLBNR9ZFJ9KIZUNQSGAI9DRZXONQJFZKETCHWCZWD9JMIAFAGDSOVFKIBOSRXY9ZKKFXWD") });

      Assert.IsTrue(transactions.Hashes.Any());

      transactions =
        this.repository.FindTransactionsByApprovees(
          new List<Hash> { new Hash("AYZMIHSFSKIKPUUUBENOIBSEVBGOCBVGAIPRWHNEFHBROZIKKYXXZDPVKJHIUSANFPLDIUBKFUPSA9999") });

      Assert.IsTrue(transactions.Hashes.Any());

      transactions =
        this.repository.FindTransactions(
          new Dictionary<string, IEnumerable<TryteString>>
            {
              {
                "addresses", 
                new List<TryteString>
                  {
                    new TryteString(
                      "GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9")
                  }
              }, 
              {
                "bundles", 
                new List<TryteString>
                  {
                    new TryteString(
                      "JSHICEYJKLEQLBNR9ZFJ9KIZUNQSGAI9DRZXONQJFZKETCHWCZWD9JMIAFAGDSOVFKIBOSRXY9ZKKFXWD")
                  }
              }, 
              {
                "approvees", 
                new List<TryteString>
                  {
                    new TryteString(
                      "AYZMIHSFSKIKPUUUBENOIBSEVBGOCBVGAIPRWHNEFHBROZIKKYXXZDPVKJHIUSANFPLDIUBKFUPSA9999")
                  }
              }
            });

      Assert.IsTrue(transactions.Hashes.Any());
    }

    /// <summary>
    /// The test get inclusion states.
    /// </summary>
    [TestMethod]
    public void TestGetInclusionStates()
    {
      var tips = this.repository.GetTips();
      var inclusionsStates =
        this.repository.GetInclusionStates(
          new List<Hash> { new Hash("HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999") }, 
          tips.Hashes.GetRange(0, 1));

      Assert.IsTrue(
        inclusionsStates.States.First(entry => entry.Key.Value == "HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999")
          .Value);
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

    /// <summary>
    /// The test get trytes.
    /// </summary>
    [TestMethod]
    public void TestGetTrytes()
    {
      var transactionTrytes =
        this.repository.GetTrytes(new List<Hash> { new Hash("HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999") });

      var transaction = Transaction.FromTrytes(transactionTrytes[0]);

      Assert.AreEqual("GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9", transaction.Address.Value);
    }

    #endregion
  }
}