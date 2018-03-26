namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Utils;

  /// <summary>
  /// The cpu po w diver test.
  /// </summary>
  [TestClass]
  public class CpuPearlDiverTest
  {
    /// <summary>
    /// The test pow.
    /// </summary>
    [TestMethod]
    public void TestPow()
    {
      var transaction = new Transaction
                          {
                            Fragment =
                              new Fragment(
                                "DBDQCTCPCFDPCQC9DTCSALAHCCDIDLAFDTCEAFDTCPC9D9DMDEABDCDHDEAVCCDXCBDVCEAHDCDEA9DXCZCTCEAXCHDQALAEACDQCGDTCFDJDTCSCEANBTCTCDDEACCWCCDIDVCWCHDSALACCTC9D9DEAIDGDFALALAKB9D9DEAFDXCVCWCHDQALAEAGDPCXCSCEANBTCTCDDEACCWCCDIDVCWCHDSAEALACCWCTCEAKBBDGDKDTCFDEAHDCDEAHDWCTCEAQBFDTCPCHDEA9CIDTCGDHDXCCDBDSASASALALAHCTCGDIBLALAYBUCEAVBXCUCTCQAEAHDWCTCEADCBDXCJDTCFDGDTCEAPCBDSCEAOBJDTCFDMDHDWCXCBDVCSASASALAEAGDPCXCSCEANBTCTCDDEACCWCCDIDVCWCHDSALAHCTCGDIBIBLALASBGDSASASALALAHCTCGDIBFALALAPBCDFDHDMDRAHDKDCDQALAEAGDPCXCSCEANBTCTCDDEACCWCCDIDVCWCHDQAEAKDXCHDWCEAXCBDUCXCBDXCHDTCEAADPCYCTCGDHDMDEAPCBDSCEARCPC9DADSA"),
                            Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999A9PG9AXCQANAWGJBTFWEAEQCN9WBZB9BJAIIY9UDLIGFOAA"),
                            CurrentIndex = 4,
                            LastIndex = 7,
                            Value = 0,
                            BranchTransaction = new Hash(),
                            BundleHash = new Hash(),
                            Hash = new Hash(),
                            Nonce = new Tag(),
                            TrunkTransaction = new Hash(),
                            Tag = new Tag(),
                            AttachmentTimestamp = 1485020456,
                            AttachmentTimestampLowerBound = 1485020456,
                            AttachmentTimestampUpperBound = 1485020456,
                            Timestamp = 1485020456,
                            ObsoleteTag = new Tag()
                          };

      var powDiver = new CpuPearlDiver();
      var transactionTrits = powDiver.Search(transaction.ToTrytes().ToTrits(), 14, 162, Constants.TritHashLength);
      var transactionFromPow = Transaction.FromTrytes(new TransactionTrytes(Converter.TritsToTrytes(transactionTrits)), transaction.Hash);

      Assert.AreEqual("DGOLNGOKKIHNQTAPICINFYC9VMS", transactionFromPow.Nonce.Value);
      // PEOLNGOKKHHNQTAPICPYBYC9VMS
    }
  }
}