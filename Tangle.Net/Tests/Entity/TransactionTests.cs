namespace Tangle.Net.Tests.Entity
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The transaction tests.
  /// </summary>
  [TestClass]
  public class TransactionTests
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test transaction to tryte conversion.
    /// </summary>
    [TestMethod]
    public void TestTransactionToTryteConversion()
    {
      var transaction = new Transaction
                          {
                            Address = new Address { Trytes = "9XV9RJGFJJZWITDPKSQXRTHCKJAIZZY9BYLBEQUXUNCLITRQDR9CCD99AANMXYEKD9GLJGVB9HIAGRIBQ" },
                            Tag = "PPDIDNQDJZGUQKOWJ9JZRCKOVGP", 
                            ObsoleteTag = "PPDIDNQDJZGUQKOWJ9JZRCKOVGP", 
                            Timestamp = 1509136296, 
                            CurrentIndex = 0, 
                            LastIndex = 0
                          };
      var transactionTrytes = transaction.ToTrytes();

      Assert.AreEqual(
        "9XV9RJGFJJZWITDPKSQXRTHCKJAIZZY9BYLBEQUXUNCLITRQDR9CCD99AANMXYEKD9GLJGVB9HIAGRIBQ999999999999999999999999999PPDIDNQDJZGUQKOWJ9JZRCKOVGPXVBSEXD99999999999999999999", 
        transactionTrytes);
    }

    #endregion
  }
}