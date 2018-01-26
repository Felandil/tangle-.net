namespace Tangle.Net.Unit.Tests.Entity
{
  using System;
  using System.Collections.Generic;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;
  using Tangle.Net.Unit.Tests.Cryptography;

  /// <summary>
  /// The bundle test.
  /// </summary>
  [TestClass]
  public class BundleTest
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
            Fragment = Fragment.FromString("Hello, world!"), 
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

      // Make the signature look like a message, so we can verify that
      // the Bundle skips it correctly.
      this.bundle.Transactions.Add(
        new Transaction
          {
            Fragment = Fragment.FromString("This is a signature, not a message!"), 
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

      // Make the signature look like a message, so we can verify that
      // the Bundle skips it correctly.
      this.bundle.Transactions.Add(
        new Transaction
          {
            Fragment = Fragment.FromString("This is a signature, not a message!"), 
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
            Fragment = Fragment.FromString("I can haz change?"), // Yes PyOTA you can!
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
    /// The test get bundle from transaction trytes.
    /// </summary>
    [TestMethod]
    public void TestGetBundleFromTransactionTrytes()
    {
      var transactions = new List<TransactionTrytes>
                           {
                             new TransactionTrytes(
                               "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999WUQXEGBVIECGIWO9IGSYKWWPYCIVUJJGSJPWGIAFJPYSF9NSQOHWAHS9P"
                               + "9PWQHOBXNNQIF9IRHVQXKPZW999999999999999999999999999XZUIENOTTBKJMDP"
                               + "RXWGQYG9PWGTHNLFMVD99A99999999A99999999PDQWLVVDPUU9VIBODGMRIAZPGQX"
                               + "DOGSEXIHKIBWSLDAWUKZCZMK9Z9YZSPCKBDJSVDPRQLJSTKUMTNVSXBGUEHHGAIWWQ"
                               + "BCJZHZAQOWZMAIDAFUZBVMUVPWQJLUGGQKNKLMGTWXXNZKUCBJLEDAMYVRGABAWBY9"
                               + "999MYIYBTGIOQYYZFJBLIAWMPSZEFFTXUZPCDIXSLLQDQSFYGQSQOGSPKCZNLVSZ9L"
                               + "MCUWVNGEN9EJEW9999XZUIENOTTBKJMDPRXWGQYG9PWGTXUO9AXMP9FLMDRMADLRPW"
                               + "CZCJBROYCDRJMYU9HDYJM9NDBFUPIZVTR"),
                             new TransactionTrytes(
                               "NBTCPCFDEACCPCBDVC9DTCQAJ9RBTC9D9DCDQAEAKDCDFD9DSCFAJ9VBCDJDTCQAJ9"
                               + "ZBMDYBCCKB99999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999999999999999999"
                               + "999999999999999999999999999999999999999999999999999SYRABNN9JD9PNDL"
                               + "IKUNCECUELTOHNLFMVD99999999999A99999999PDQWLVVDPUU9VIBODGMRIAZPGQX"
                               + "DOGSEXIHKIBWSLDAWUKZCZMK9Z9YZSPCKBDJSVDPRQLJSTKUMTNVSXFSEWUNJOEGNU"
                               + "I9QOCRFMYSIFAZLJHKZBPQZZYFG9ORYCRDX9TOMJPFCRB9R9KPUUGFPVOWYXFIWEW9"
                               + "999BGUEHHGAIWWQBCJZHZAQOWZMAIDAFUZBVMUVPWQJLUGGQKNKLMGTWXXNZKUCBJL"
                               + "EDAMYVRGABAWBY9999SYRABNN9JD9PNDLIKUNCECUELTOQZPSBDILVHJQVCEOICFAD"
                               + "YKZVGMOAXJRQNTCKMHGTAUMPGJJMX9LNF")
                           };

      var bundleFromTransactionTrytes = Bundle.FromTransactionTrytes(transactions);

      Assert.AreEqual(2, bundleFromTransactionTrytes.Transactions.Count);
      Assert.AreEqual(0, bundleFromTransactionTrytes.Transactions[0].CurrentIndex);
    }

    /// <summary>
    /// The test add inputs covers the exact amount spent should create correct transaction count.
    /// </summary>
    [TestMethod]
    public void TestAddInputsCoversTheExactAmountSpentShouldCreateCorrectTransactionCount()
    {
      this.bundle = new Bundle();
      this.bundle.AddTransfer(
        new Transfer { Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ"), ValueToTransfer = 29});
      this.bundle.AddTransfer(
        new Transfer { Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999OGVEEFBCYAM9ZEAADBGBHH9BPBOHFEGCFAM9DESCCHODZ9Y"), ValueToTransfer = 13});

      this.bundle.AddInput(
        new List<Address>
          {
            new Address("KHWHSTISMVVSDCOMHVFIFCTINWZT9EHJUATYSMCXDSMZXPL9KXREBBYHJGRBCYVGPJQEHEDPXLBDJNQNX")
              {
                Balance = 40, 
                KeyIndex = 1, 
                SecurityLevel = 1
              }, 
            new Address("GOAAMRU9EALPO9GKBOWUVZVQEJMB9CSGIZJATHRBTRRJPNTSQRZTASRBTQCRFAIDOGTWSHIDGOUUULQIG")
              {
                Balance = 2, 
                KeyIndex = 2, 
                SecurityLevel = 1
              }
          });

      // since balance and spent tokens are even, this should be ignored
      this.bundle.AddRemainder(new Address("TESTVALUE9DONTUSEINPRODUCTION99999KAFGVCIBLHS9JBZCEFDELEGFDCZGIEGCPFEIQEYGA9UFPAE"));

      this.bundle.Finalize();

      Assert.AreEqual(4, this.bundle.Transactions.Count);
    }

    /// <summary>
    /// The test add inputs with security level higher than one should add multiple transactions to hold signature.
    /// </summary>
    [TestMethod]
    public void TestAddInputsWithSecurityLevelHigherThanOneShouldAddMultipleTransactionsToHoldSignature()
    {
      this.bundle = new Bundle();
      this.bundle.AddTransfer(
        new Transfer { Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ"), ValueToTransfer = 84});

      this.bundle.AddInput(
        new List<Address>
          {
            new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD")
              {
                Balance = 42, 
                KeyIndex = 4, 
                SecurityLevel = 2
              }, 
            new Address("XXYRPQ9BDZGKZZQLYNSBDD9HZLI9OFRK9TZCTU9PFAJYXZIZGO9BWLOCNGVMTLFQFMGJWYRMLXSCW9UTQ")
              {
                Balance = 42, 
                KeyIndex = 5, 
                SecurityLevel = 3
              }
          });

      this.bundle.Finalize();

      Assert.AreEqual(6, this.bundle.Transactions.Count);
    }

    /// <summary>
    /// The test bundle has unspent inputs should notbe finalizable.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleHasInsufficientInputsShouldNotBeFinalizable()
    {
      var transfer = new Transfer
                       {
                         Address =
                           new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB"), 
                         Message = new TryteString(), 
                         Tag = Tag.Empty,
                         ValueToTransfer = 42
                       };

      this.bundle = new Bundle();
      this.bundle.AddTransfer(transfer);

      this.bundle.AddInput(
        new List<Address>
          {
            new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD")
              {
                Balance = 40, 
                KeyIndex = 0, 
                SecurityLevel = 1
              }
          });

      Assert.AreEqual(2, this.bundle.Balance);

      this.bundle.Finalize();
    }

    /// <summary>
    /// The test bundle has no transactions should throw exception on finalize.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestBundleHasNoTransactionsShouldThrowExceptionOnFinalize()
    {
      this.bundle = new Bundle();
      this.bundle.Finalize();
    }

    /// <summary>
    /// The test bundle has unspent inputs but remainder is given should be finalizable.
    /// </summary>
    [TestMethod]
    public void TestBundleHasUnspentInputsButRemainderIsGivenShouldBeFinalizable()
    {
      var transfer = new Transfer
                       {
                         Address =
                           new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB")
                             {
                               Balance = 42
                             }, 
                         Message = new TryteString(), 
                         Tag = Tag.Empty
                       };

      this.bundle = new Bundle();
      this.bundle.AddTransfer(transfer);

      this.bundle.AddInput(
        new List<Address>
          {
            new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD")
              {
                Balance = 42, 
                KeyIndex = 0, 
                SecurityLevel = 1
              }, 
            new Address("XXYRPQ9BDZGKZZQLYNSBDD9HZLI9OFRK9TZCTU9PFAJYXZIZGO9BWLOCNGVMTLFQFMGJWYRMLXSCW9UTQ")
              {
                Balance = 2, 
                KeyIndex = 2, 
                SecurityLevel = 1
              }
          });

      this.bundle.AddRemainder(new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD"));

      this.bundle.Finalize();

      Assert.AreEqual(0, this.bundle.Balance);
      Assert.AreEqual(4, this.bundle.Transactions.Count);
    }

    /// <summary>
    /// The test bundle has unspent inputs should notbe finalizable.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleHasUnspentInputsShouldNotBeFinalizable()
    {
      var transfer = new Transfer
                       {
                         Address =
                           new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB"), 
                         Message = new TryteString("ASDF"), 
                         Tag = Tag.Empty,
                         ValueToTransfer = 42
                       };

      this.bundle = new Bundle();
      this.bundle.AddTransfer(transfer);

      this.bundle.AddInput(
        new List<Address>
          {
            new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD")
              {
                Balance = 42, 
                KeyIndex = 0, 
                SecurityLevel = 1
              }, 
            new Address("XXYRPQ9BDZGKZZQLYNSBDD9HZLI9OFRK9TZCTU9PFAJYXZIZGO9BWLOCNGVMTLFQFMGJWYRMLXSCW9UTQ")
              {
                Balance = 2, 
                KeyIndex = 2, 
                SecurityLevel = 1
              }
          });

      Assert.AreEqual(-2, this.bundle.Balance);

      this.bundle.Finalize();
    }

    /// <summary>
    /// The test bundle is finalized should apply signing fragment.
    /// </summary>
    [TestMethod]
    public void TestBundleIsFinalizedShouldApplySigningFragmentAndNoExtraTransactionsForSecurityLevelOne()
    {
      this.bundle = new Bundle();
      this.bundle.AddTransfer(
        new Transfer { Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ"), ValueToTransfer = 42});

      this.bundle.AddInput(
        new List<Address>
          {
            new Address("KHWHSTISMVVSDCOMHVFIFCTINWZT9EHJUATYSMCXDSMZXPL9KXREBBYHJGRBCYVGPJQEHEDPXLBDJNQNX")
              {
                Balance = 40, 
                KeyIndex = 1, 
                SecurityLevel = 1
              }, 
            new Address("GOAAMRU9EALPO9GKBOWUVZVQEJMB9CSGIZJATHRBTRRJPNTSQRZTASRBTQCRFAIDOGTWSHIDGOUUULQIG")
              {
                Balance = 2, 
                KeyIndex = 2, 
                SecurityLevel = 1
              }
          });

      this.bundle.Finalize();
      this.bundle.Sign(new KeyGeneratorStub());

      Assert.AreEqual(3, this.bundle.Transactions.Count);

      Assert.IsTrue(this.bundle.Transactions[0].Fragment.Value == new Fragment().Value);

      // check if transactions got filled with empty values
      Assert.IsTrue(this.bundle.Transactions[0].AttachmentTimestamp == 999999999);
      Assert.IsTrue(this.bundle.Transactions[0].AttachmentTimestampLowerBound == 999999999);
      Assert.IsTrue(this.bundle.Transactions[0].AttachmentTimestampUpperBound == 999999999);
      Assert.IsTrue(this.bundle.Transactions[0].Nonce.Value == new Tag().Value);
      Assert.IsTrue(this.bundle.Transactions[0].BranchTransaction.Value == new Hash().Value);
      Assert.IsTrue(this.bundle.Transactions[0].TrunkTransaction.Value == new Hash().Value);

      for (var i = 1; i < this.bundle.Transactions.Count; i++)
      {
        Assert.IsTrue(!string.IsNullOrEmpty(this.bundle.Transactions[i].Fragment.Value));
        Assert.IsTrue(this.bundle.Transactions[i].AttachmentTimestamp == 999999999);
        Assert.IsTrue(this.bundle.Transactions[i].AttachmentTimestampLowerBound == 999999999);
        Assert.IsTrue(this.bundle.Transactions[i].AttachmentTimestampUpperBound == 999999999);
        Assert.IsTrue(this.bundle.Transactions[i].Nonce.Value == new Tag().Value);
        Assert.IsTrue(this.bundle.Transactions[i].BranchTransaction.Value == new Hash().Value);
        Assert.IsTrue(this.bundle.Transactions[i].TrunkTransaction.Value == new Hash().Value);
      }
    }

    /// <summary>
    /// The test bundle is finalized should apply signing fragment and with extra transactions for security level above one.
    /// </summary>
    [TestMethod]
    public void TestBundleIsFinalizedShouldApplySigningFragmentAndWithExtraTransactionsForSecurityLevelAboveOne()
    {
      this.bundle = new Bundle();
      this.bundle.AddTransfer(
        new Transfer { Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ"), ValueToTransfer = 42 });

      this.bundle.AddInput(
        new List<Address>
          {
            new Address("KHWHSTISMVVSDCOMHVFIFCTINWZT9EHJUATYSMCXDSMZXPL9KXREBBYHJGRBCYVGPJQEHEDPXLBDJNQNX")
              {
                Balance = 40, 
                KeyIndex = 1, 
                SecurityLevel = 2
              }, 
            new Address("GOAAMRU9EALPO9GKBOWUVZVQEJMB9CSGIZJATHRBTRRJPNTSQRZTASRBTQCRFAIDOGTWSHIDGOUUULQIG")
              {
                Balance = 2, 
                KeyIndex = 2, 
                SecurityLevel = 3
              }
          });

      this.bundle.Finalize();
      this.bundle.Sign(new KeyGeneratorStub());

      Assert.AreEqual(6, this.bundle.Transactions.Count);

      Assert.IsTrue(this.bundle.Transactions[0].Fragment.Value == new Fragment().Value);

      for (var i = 1; i < this.bundle.Transactions.Count; i++)
      {
        Assert.IsTrue(!string.IsNullOrEmpty(this.bundle.Transactions[i].Fragment.Value));
      }
    }

    /// <summary>
    /// The test add entry while bundle is finalized throws exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleIsFinalizedShouldThrowExceptionOnAddEntry()
    {
      var transfer = new Transfer
                       {
                         Address =
                           new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ")
                             {
                               Balance = 42
                             }, 
                         Message = TryteString.FromString(GetSuperLongMessage()), 
                         Tag = Tag.Empty
                       };

      this.bundle = new Bundle();
      this.bundle.AddTransfer(transfer);
      this.bundle.Finalize();

      this.bundle.AddTransfer(transfer);
    }

    /// <summary>
    /// The test bundle is finalized should throw exception on add input.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleIsFinalizedShouldThrowExceptionOnAddInput()
    {
      this.bundle = new Bundle();
      this.bundle.AddTransfer(
        new Transfer { Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 42 } });

      this.bundle.Finalize();

      this.bundle.AddInput(
        new List<Address>
          {
            new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD")
              {
                Balance = 42, 
                KeyIndex = 4, 
                SecurityLevel = 2
              }
          });
    }

    /// <summary>
    /// The test bundle is finalized should throw exception on add input.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleIsFinalizedShouldThrowExceptionOnAddRemainder()
    {
      this.bundle = new Bundle();
      this.bundle.AddTransfer(
        new Transfer { Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 84 } });

      this.bundle.Finalize();

      this.bundle.AddRemainder(new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD"));
    }

    /// <summary>
    /// The test bundle is finalized should throw exception on finalize.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleIsFinalizedShouldThrowExceptionOnFinalize()
    {
      this.bundle = new Bundle();
      this.bundle.AddTransfer(
        new Transfer { Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 84 } });

      this.bundle.Finalize();
      this.bundle.Finalize();
    }

    /// <summary>
    /// The test bundle is not finalized should not be signable.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleIsNotFinalizedShouldNotBeSignable()
    {
      this.bundle = new Bundle();
      this.bundle.AddTransfer(
        new Transfer { Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 42 } });

      this.bundle.Sign(new KeyGeneratorStub());
    }

    /// <summary>
    /// The test get messages.
    /// </summary>
    [TestMethod]
    public void TestGetMessages()
    {
      var messages = this.bundle.GetMessages();

      Assert.AreEqual(4, messages.Count);

      Assert.AreEqual(GetSuperLongMessage(), messages[0]);
      // Assert.AreEqual("祝你好运\x15", messages[1]); Ignore non UTF8 characters.
      Assert.AreEqual("Hello, world!", messages[2]);
      Assert.AreEqual("I can haz change?", messages[3]);
    }

    /// <summary>
    /// The test insecure bundle hash is manipulated to be secure.
    /// </summary>
    [TestMethod]
    public void TestInsecureBundleHashIsManipulatedToBeSecure()
    {
      var transfer = new Transfer
                       {
                         Address = new Address("9XV9RJGFJJZWITDPKSQXRTHCKJAIZZY9BYLBEQUXUNCLITRQDR9CCD99AANMXYEKD9GLJGVB9HIAGRIBQ"), 
                         Tag = new Tag("PPDIDNQDJZGUQKOWJ9JZRCKOVGP"), 
                         Timestamp = 1509136296, 
                         Message = new TryteString("JKAHSAAS")
                       };

      this.bundle = new Bundle();
      this.bundle.AddTransfer(transfer);
      this.bundle.Finalize();

      Assert.AreEqual(1, this.bundle.Transactions.Count);
      Assert.AreEqual("ZTDIDNQDJZGUQKOWJ9JZRCKOVGP", this.bundle.Transactions[0].ObsoleteTag.Value);
      Assert.AreEqual("PPDIDNQDJZGUQKOWJ9JZRCKOVGP", this.bundle.Transactions[0].Tag.Value);
      Assert.AreEqual("NYSJSEGCWESDAFLIFCNJFWGZ9PCYDOT9VCSALKBD9UUNKBJAJCB9KVMTHZDPRDDXC9UFJQBJBQFUPJKFC", this.bundle.Hash.Value);
    }

    /// <summary>
    /// The test transfer message does not fit into one transaction should create second transaction.
    /// </summary>
    [TestMethod]
    public void TestTransferMessageDoesNotFitIntoOneTransactionShouldCreateSecondTransaction()
    {
      var transfer = new Transfer
                       {
                         Address =
                           new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB"), 
                         Message = TryteString.FromString(GetSuperLongMessage()), 
                         Tag = Tag.Empty,
                         ValueToTransfer = 42
                       };

      this.bundle = new Bundle();
      this.bundle.AddTransfer(transfer);

      Assert.AreEqual(2, this.bundle.Transactions.Count);

      var transactionOne = this.bundle.Transactions[0];
      Assert.AreEqual(transfer.Address, transactionOne.Address);
      Assert.AreEqual(transfer.Tag, transactionOne.Tag);
      Assert.AreEqual(transfer.ValueToTransfer, transactionOne.Value);

      var transactionTwo = this.bundle.Transactions[1];
      Assert.AreEqual(transfer.Address, transactionTwo.Address);
      Assert.AreEqual(transfer.Tag, transactionTwo.Tag);
      Assert.AreEqual(0, transactionTwo.Value);
    }

    /// <summary>
    /// The test transfer message fits into one transaction should return transaction count one.
    /// </summary>
    [TestMethod]
    public void TestTransferMessageFitsIntoOneTransactionShouldAddSingleTransaction()
    {
      var transfer = new Transfer
                       {
                         Address =
                           new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB")
                             {
                               Balance = 42
                             }, 
                         Message = TryteString.FromString("Hello World!"), 
                         Tag = Tag.Empty
                       };

      this.bundle = new Bundle();
      this.bundle.AddTransfer(transfer);

      Assert.AreEqual(1, this.bundle.Transactions.Count);
    }

    /// <summary>
    /// The test value is negative should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestValueIsNegativeShouldThrowException()
    {
      var transfer = new Transfer
                       {
                         Address =
                           new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB"), 
                         Message = TryteString.FromString(GetSuperLongMessage()), 
                         Tag = Tag.Empty,
                         ValueToTransfer = -42
                       };

      this.bundle = new Bundle();
      this.bundle.AddTransfer(transfer);
    }

    #endregion

    #region Methods

    /// <summary>
    /// The super long message.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private static string GetSuperLongMessage()
    {
      return "'Good morning,' said Deep Thought at last.'Er... Good morning, O Deep Thought,' said Loonquawl nervously."
             + "'Do you have... er, that is...'... an answer for you?' interrupted Deep Thought majestically. 'Yes. I have.'"
             + "The two men shivered with expectancy. Their waiting had not been in vain.'There really is one?' breathed Phouchg."
             + "'There really is one,' confirmed Deep Thought.'To Everything? To the great Question of Life, the Universe and Everything?'"
             + "'Yes.' Both of the men had been trained for this moment; their lives had been a"
             + "  preparation for it; they had been selected at birth as those who would"
             + "  witness the answer; but even so they found themselves gasping and squirming like excited children."
             + "'And you're ready to give it to us?' urged Loonquawl.'I am.''Now?''Now,' said Deep Thought."
             + "They both licked their dry lips.'Though I don't think,' added Deep Thought, 'that you're going to like it.'"
             + "'Doesn't matter,' said Phouchg. 'We must know it! Now!''Now?' enquired Deep Thought.'Yes! Now!'"
             + "'All right,' said the computer and settled into silence again. The two men fidgeted. The tension was unbearable."
             + "'You're really not going to like it,' observed Deep Thought.'Tell us!'"
             + "'All right,' said Deep Thought. 'The Answer to the Great Question...''Yes?'"
             + "'Of Life, the Universe and Everything...' said Deep Thought.'Yes??''Is...''Yes?!'"
             + "'Forty-two,' said Deep Thought, with infinite majesty and calm.";
    }

    #endregion
  }
}