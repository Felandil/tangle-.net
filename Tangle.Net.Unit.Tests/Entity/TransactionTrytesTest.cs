namespace Tangle.Net.Unit.Tests.Entity
{
  using System;
  using System.Linq;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;

  /// <summary>
  /// The transaction trytes test.
  /// </summary>
  [TestClass]
  public class TransactionTrytesTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test tryte length is bigger than transaction tryte length should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestTryteLengthIsBiggerThanTransactionTryteLengthShouldThrowException()
    {
      var transactionTrytes = new TransactionTrytes(string.Concat(Enumerable.Repeat("9", TransactionTrytes.Length + 1)));
    }

    /// <summary>
    /// The test tryte length is lower than transaction tryte length should pad.
    /// </summary>
    [TestMethod]
    public void TestTryteLengthIsLowerThanTransactionTryteLengthShouldPad()
    {
      var transactionTrytes = new TransactionTrytes("JKAHKJASH");
      Assert.AreEqual(TransactionTrytes.Length, transactionTrytes.TrytesLength);
    }

    #endregion
  }
}