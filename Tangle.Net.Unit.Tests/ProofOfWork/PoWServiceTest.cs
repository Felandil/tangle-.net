namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Utils;

  /// <summary>
  /// The po w service test.
  /// </summary>
  [TestClass]
  public class PoWServiceTest
  {
    #region Fields

    /// <summary>
    /// The bundle.
    /// </summary>
    private Bundle bundle;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The setup.
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
      this.bundle = new Bundle();
      this.bundle.Transactions.Add(
        new Transaction
          {
            Fragment = new Fragment(), 
            Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999A9PG9AXCQANAWGJBTFWEAEQCN9WBZB9BJAIIY9UDLIGFOAA"), 
            CurrentIndex = 0, 
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
          });

      this.bundle.Transactions.Add(
        new Transaction
          {
            Fragment = new Fragment("OHCFVELH9GYEMHCF9GPHBGIEWHZFU"), 
            Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999HAA9UAMHCGKEUGYFUBIARAXBFASGLCHCBEVGTBDCSAEBTBM"), 
            CurrentIndex = 1, 
            LastIndex = 7, 
            Value = 10, 
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
          });

      this.bundle.Transactions.Add(
        new Transaction
          {
            Fragment = Fragment.FromAsciiString("Hello, world!"), 
            Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999D99HEAM9XADCPFJDFANCIHR9OBDHTAGGE9TGCI9EO9ZCRBN"), 
            CurrentIndex = 2, 
            LastIndex = 7, 
            Value = 20, 
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
          });

      this.bundle.Transactions.Add(
        new Transaction
          {
            Fragment =
              new Fragment(
              "LAQBCDCDSCEAADCDFDBDXCBDVCQALAEAGDPCXCSCEANBTCTCDDEACCWCCDIDVCWCHDEAPCHDEA9DPCGDHDSALAOBFDSASASAEAQBCDCDSCEAADCDFDBDXCBDVCQAEAYBEANBTCTCDDEACCWCCDIDVCWCHDQALAEAGDPCXCSCEAVBCDCDBDEDIDPCKD9DEABDTCFDJDCDIDGD9DMDSALANBCDEAMDCDIDEAWCPCJDTCSASASAEATCFDQAEAHDWCPCHDEAXCGDSASASALASASASAEAPCBDEAPCBDGDKDTCFDEAUCCDFDEAMDCDIDIBLAEAXCBDHDTCFDFDIDDDHDTCSCEANBTCTCDDEACCWCCDIDVCWCHDEAADPCYCTCGDHDXCRCPC9D9DMDSAEALAHCTCGDSAEASBEAWCPCJDTCSALACCWCTCEAHDKDCDEAADTCBDEAGDWCXCJDTCFDTCSCEAKDXCHDWCEATCLDDDTCRCHDPCBDRCMDSAEACCWCTCXCFDEAKDPCXCHDXCBDVCEAWCPCSCEABDCDHDEAQCTCTCBDEAXCBDEAJDPCXCBDSALACCWCTCFDTCEAFDTCPC9D9DMDEAXCGDEACDBDTCIBLAEAQCFDTCPCHDWCTCSCEAZBWCCDIDRCWCVCSALACCWCTCFDTCEAFDTCPC9D9DMDEAXCGDEACDBDTCQALAEARCCDBDUCXCFDADTCSCEANBTCTCDDEACCWCCDIDVCWCHDSALACCCDEAOBJDTCFDMDHDWCXCBDVCIBEACCCDEAHDWCTCEAVCFDTCPCHDEA9CIDTCGDHDXCCDBDEACDUCEAVBXCUCTCQAEAHDWCTCEADCBDXCJDTCFDGDTCEAPCBDSCEAOBJDTCFDMDHDWCXCBDVCIBLALAHCTCGDSALAEALBCDHDWCEACDUCEAHDWCTCEAADTCBDEAWCPCSCEAQCTCTCBDEAHDFDPCXCBDTCSCEAUCCDFDEAHDWCXCGDEAADCDADTCBDHDEBEAHDWCTCXCFDEA9DXCJDTCGDEAWCPCSCEAQCTCTCBDEAPCEAEADDFDTCDDPCFDPCHDXCCDBDEAUCCDFDEAXCHDEBEAHDWCTCMDEAWCPCSCEAQCTCTCBDEAGDTC9DTCRCHDTCSCEAPCHDEAQCXCFDHDWCEAPCGDEAHDWCCDGDTCEAKDWCCDEAKDCDID9DSCEAEAKDXCHDBDTCGDGDEAHDWCTCEAPCBDGDKDTCFDEBEAQCIDHDEATCJDTCBDEAGDCDEAHDWCTCMDEAUCCDIDBDSCEAHDWCTCADGDTC9DJDTCGDEAVCPCGDDDXCBDVCEAPCBDSCEAGDEDIDXCFDADXCBDVCEA9DXCZCTCEATCLDRCXCHDTCSCEARCWCXC9DSCFDTCBDSALAKBBDSCEAMDCDIDLAFDTCEAFDTCPCSCMDEAHDCDEAVCXCJDTCEAXCHDEAHDCDEAIDGDIBLAEAIDFDVCTCSCEAVBCDCDBDEDIDPCKD9DSALASBEAPCADSALALAXBCDKDIBLALAXBCDKDQALAEAGDPCXCSCEANBTCTCDDEACCWCCDIDVCWCHDSACCWCTCMDEAQCCDHDWCEA9DXCRCZCTCSCEAHDWCTCXCFDEASCFDMDEA9DXCDDGDSALACCWCCDIDVCWCEASBEASCCDBDLAHDEAHDWCXCBDZCQALAEAPCSCSCTCSCEANBTCTCDDEACCWCCDIDVCWCHDQAEALAHDWCPCHDEAMDCDIDLAFDTCEAVCCDXCBDVCEAHDCDEA9DXCZCTCEAXCHDSALALANBCDTCGDBDLAHDEAADPCHDHDTCFDQALAEAGDPCXCSCEAZBWCCDIDRCWCVCSAEALAFCTCEAADIDGDHDEAZCBDCDKDEAXCHDFAEAXBCDKDFALALAXBCDKDIBLAEATCBDEDIDXCFDTCSCEANBTCTCDDEACCWCCDIDVCWCHDSALAHCTCGDFAEAXBCDKDFALALAKB9D9DEAFDXCVCWCHDQALAEAGDPCXCSCEAHDWCTCEARCCDADDDIDHDTCFDEAPCBDSCEAGDTCHDHD9DTCSCEAXCBDHDCDEAGDXC9DTCBDRCTCEAPCVCPCXCBDSAEACCWCTCEAHDKDCDEAADTCBDEAUCXCSCVCTCHDTCSCSAEACCWCTCEAHDTCBDGDXCCDBDEAKDPCGDEAI"), 
            Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999A9PG9AXCQANAWGJBTFWEAEQCN9WBZB9BJAIIY9UDLIGFOAA"), 
            CurrentIndex = 3, 
            LastIndex = 7, 
            Value = 30, 
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
          });

      this.bundle.Transactions.Add(
        new Transaction
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
          });

      this.bundle.Transactions.Add(
        new Transaction
          {
            Fragment = Fragment.FromAsciiString("This is a signature, not a message!"), 
            Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999WGSBUAHDVHYHOBHGP9VCGIZHNCAAQFJGE9YHEHEFTDAGXHY"), 
            CurrentIndex = 5, 
            LastIndex = 7, 
            Value = -100, 
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
          });

      this.bundle.Transactions.Add(
        new Transaction
          {
            Fragment = Fragment.FromAsciiString("This is a signature, not a message!"), 
            Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999WGSBUAHDVHYHOBHGP9VCGIZHNCAAQFJGE9YHEHEFTDAGXHY"), 
            CurrentIndex = 6, 
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
          });

      this.bundle.Transactions.Add(
        new Transaction
          {
            Fragment = Fragment.FromAsciiString("I can haz change?"), // Yes PyOTA you can!
            Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999FFYALHN9ACYCP99GZBSDK9CECFI9RAIH9BRCCAHAIAWEFAN"), 
            CurrentIndex = 7, 
            LastIndex = 7, 
            Value = 40, 
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
          });
    }

    /// <summary>
    /// The test pow service adds time stamps.
    /// </summary>
    [TestMethod]
    public void TestPowServiceAddsTimeStamps()
    {
      var service = new PoWService(new PoWDiverStub());
      var result = service.DoPoW(new Hash("BRANCH"), new Hash("TRUNK"), this.bundle.Transactions);

      foreach (var transaction in result)
      {
        Assert.AreEqual(0, transaction.AttachmentTimestampLowerBound);
        Assert.AreEqual(PoWService.MaxTimestampValue, transaction.AttachmentTimestampUpperBound);
        var currentTime = Timestamp.UnixSecondsTimestamp * 1000;
        Assert.IsTrue(transaction.AttachmentTimestamp > currentTime - 10000 && transaction.AttachmentTimestamp < currentTime + 10000);
      }
    }

    /// <summary>
    /// The test pow service chains transactions correctly.
    /// </summary>
    [TestMethod]
    public void TestPowServiceChainsTransactionsCorrectly()
    {
      var service = new PoWService(new PoWDiverStub());
      var result = service.DoPoW(new Hash("BRANCH"), new Hash("TRUNK"), this.bundle.Transactions);

      for (var i = 0; i < result.Count; i++)
      {
        if (result[i].CurrentIndex == result[i].LastIndex)
        {
          Assert.AreEqual(new Hash("BRANCH").Value, result[i].BranchTransaction.Value);
          Assert.AreEqual(new Hash("TRUNK").Value, result[i].TrunkTransaction.Value);
        }
        else
        {
          Assert.AreEqual(new Hash("TRUNK").Value, result[i].BranchTransaction.Value);
          Assert.AreEqual(result[i + 1].Hash.Value, result[i].TrunkTransaction.Value);
        }
      }
    }

    #endregion
  }
}