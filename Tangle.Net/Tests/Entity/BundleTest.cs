namespace Tangle.Net.Tests.Entity
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The bundle test.
  /// </summary>
  [TestClass]
  public class BundleTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test transfer message fits into one transaction should return transaction count one.
    /// </summary>
    [TestMethod]
    public void TestTransferMessageFitsIntoOneTransactionShouldReturnTransactionCountOne()
    {
      var transfer = new Transfer
                       {
                         Address = "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB999999999", 
                         Value = 42, 
                         Message = AsciiToTrytes.FromString("Hello World!"),
                         Tag = "99999999999999999999999999999"
                       };

      var bundle = new Bundle();
      bundle.AddEntry(1, transfer.Address, transfer.Value, "9999999", 999999999L);
      bundle.Finalize();

      Assert.AreEqual(1, bundle.Transactions.Count);
    }

    #endregion
  }
}