namespace Tangle.Net.Tests.Entity
{
  using System;
  using System.Collections.Generic;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Repository;
  using Tangle.Net.Source.Utils;
  using Tangle.Net.Tests.Cryptography;

  /// <summary>
  /// The bundle test.
  /// </summary>
  [TestClass]
  public class BundleTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test add inputs covers the exact amount spent should create correct transaction count.
    /// </summary>
    [TestMethod]
    public void TestAddInputsCoversTheExactAmountSpentShouldCreateCorrectTransactionCount()
    {
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Address { Balance = 29, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ" },
        string.Empty,
        Bundle.EmptyTag,
        999999999L);
      bundle.AddTransaction(
        new Address { Balance = 13, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999OGVEEFBCYAM9ZEAADBGBHH9BPBOHFEGCFAM9DESCCHODZ9Y" },
        string.Empty, 
        Bundle.EmptyTag, 
        999999999L);

      bundle.AddInput(
        new List<Address>
          {
            new Address
              {
                Balance = 40, 
                KeyIndex = 1, 
                SecurityLevel = 1, 
                Trytes = "KHWHSTISMVVSDCOMHVFIFCTINWZT9EHJUATYSMCXDSMZXPL9KXREBBYHJGRBCYVGPJQEHEDPXLBDJNQNX"
              }, 
            new Address
              {
                Balance = 2, 
                KeyIndex = 2, 
                SecurityLevel = 1, 
                Trytes = "GOAAMRU9EALPO9GKBOWUVZVQEJMB9CSGIZJATHRBTRRJPNTSQRZTASRBTQCRFAIDOGTWSHIDGOUUULQIG"
              }
          });

      // since balance and spent tokens are even, this should be ignored
      bundle.AddRemainder(new Address { Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999KAFGVCIBLHS9JBZCEFDELEGFDCZGIEGCPFEIQEYGA9UFPAE" });

      bundle.Finalize();

      Assert.AreEqual(4, bundle.Transactions.Count);
    }

    /// <summary>
    /// The test add inputs with security level higher than one should add multiple transactions to hold signature.
    /// </summary>
    [TestMethod]
    public void TestAddInputsWithSecurityLevelHigherThanOneShouldAddMultipleTransactionsToHoldSignature()
    {
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Address { Balance = 84, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ" },
        string.Empty, 
        Bundle.EmptyTag, 
        999999999L);

      bundle.AddInput(
        new List<Address>
          {
            new Address
              {
                Balance = 42, 
                KeyIndex = 4, 
                SecurityLevel = 2, 
                Trytes = "NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD"
              }, 
            new Address
              {
                Balance = 42, 
                KeyIndex = 5, 
                SecurityLevel = 3, 
                Trytes = "XXYRPQ9BDZGKZZQLYNSBDD9HZLI9OFRK9TZCTU9PFAJYXZIZGO9BWLOCNGVMTLFQFMGJWYRMLXSCW9UTQ"
              }
          });

      bundle.Finalize();

      Assert.AreEqual(6, bundle.Transactions.Count);
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
                         Address = new Address { Balance = 42, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ" },
                         Message = AsciiToTrytes.FromString(GetSuperLongMessage()), 
                         Tag = Bundle.EmptyTag
                       };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);
      bundle.Finalize();

      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);
    }

    /// <summary>
    /// The test bundle is finalized should throw exception on add input.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleIsFinalizedShouldThrowExceptionOnAddInput()
    {
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Address { Balance = 84, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ" },
        string.Empty, 
        Bundle.EmptyTag, 
        999999999L);

      bundle.Finalize();

      bundle.AddInput(
        new List<Address>
          {
            new Address
              {
                Balance = 42, 
                KeyIndex = 4, 
                SecurityLevel = 2, 
                Trytes = "NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD"
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
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Address { Balance = 84, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ" },
        string.Empty, 
        Bundle.EmptyTag, 
        999999999L);

      bundle.Finalize();

      bundle.AddRemainder(new Address { Trytes = "NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD" });
    }

    /// <summary>
    /// The test bundle is finalized should throw exception on finalize.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleIsFinalizedShouldThrowExceptionOnFinalize()
    {
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Address { Balance = 84, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ" },
        string.Empty, 
        Bundle.EmptyTag, 
        999999999L);

      bundle.Finalize();
      bundle.Finalize();
    }

    /// <summary>
    /// The test insecure bundle hash is manipulated to be secure.
    /// </summary>
    [TestMethod]
    public void TestInsecureBundleHashIsManipulatedToBeSecure()
    {
      var transfer = new Transfer
                       {
                         Address = new Address { Balance = 0, Trytes = "9XV9RJGFJJZWITDPKSQXRTHCKJAIZZY9BYLBEQUXUNCLITRQDR9CCD99AANMXYEKD9GLJGVB9HIAGRIBQ" },
                         Tag = "PPDIDNQDJZGUQKOWJ9JZRCKOVGP", 
                         Timestamp = 1509136296, 
                         Message = "Insecure Bundle Test"
                       };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, transfer.Timestamp);
      bundle.Finalize();

      Assert.AreEqual(1, bundle.Transactions.Count);
      Assert.AreEqual("ZTDIDNQDJZGUQKOWJ9JZRCKOVGP", bundle.Transactions[0].ObsoleteTag);
      Assert.AreEqual("PPDIDNQDJZGUQKOWJ9JZRCKOVGP", bundle.Transactions[0].Tag);
      Assert.AreEqual("NYSJSEGCWESDAFLIFCNJFWGZ9PCYDOT9VCSALKBD9UUNKBJAJCB9KVMTHZDPRDDXC9UFJQBJBQFUPJKFC", bundle.Hash);
    }

    /// <summary>
    /// The test transfer message does not fit into one transaction should create second transaction.
    /// </summary>
    [TestMethod]
    public void TestTransferMessageDoesNotFitIntoOneTransactionShouldCreateSecondTransaction()
    {
      var transfer = new Transfer
                       {
                         Address = new Address { Balance = 42, Trytes = "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB" },
                         Message = AsciiToTrytes.FromString(GetSuperLongMessage()), 
                         Tag = Bundle.EmptyTag
                       };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);

      Assert.AreEqual(2, bundle.Transactions.Count);

      var transactionOne = bundle.Transactions[0];
      Assert.AreEqual(transfer.Address, transactionOne.Address);
      Assert.AreEqual(transfer.Tag, transactionOne.Tag);
      Assert.AreEqual(transfer.Address.Balance, transactionOne.Value);

      var transactionTwo = bundle.Transactions[1];
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
                         Address = new Address { Balance = 42, Trytes = "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB" },
                         Message = AsciiToTrytes.FromString("Hello World!"), 
                         Tag = Bundle.EmptyTag
                       };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);

      Assert.AreEqual(1, bundle.Transactions.Count);
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
                         Address = new Address { Balance = -42, Trytes = "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB" },
                         Message = AsciiToTrytes.FromString(GetSuperLongMessage()), 
                         Tag = Bundle.EmptyTag
                       };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);
    }

    /// <summary>
    /// The test bundle has no transactions should throw exception on finalize.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestBundleHasNoTransactionsShouldThrowExceptionOnFinalize()
    {
      var bundle = new Bundle();
      bundle.Finalize();
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
        Address = new Address { Balance = 42, Trytes = "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB" },
        Message = string.Empty,
        Tag = Bundle.EmptyTag
      };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);

      bundle.AddInput(
        new List<Address>
          {
            new Address
              {
                Balance = 42,
                KeyIndex = 0,
                SecurityLevel = 1,
                Trytes = "NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD"
              },
            new Address
              {
                Balance = 2,
                KeyIndex = 2,
                SecurityLevel = 1,
                Trytes = "XXYRPQ9BDZGKZZQLYNSBDD9HZLI9OFRK9TZCTU9PFAJYXZIZGO9BWLOCNGVMTLFQFMGJWYRMLXSCW9UTQ"
              }
          });

      Assert.AreEqual(-2, bundle.Balance);

      bundle.Finalize();
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
        Address = new Address { Balance = 42, Trytes = "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB" },
        Message = string.Empty,
        Tag = Bundle.EmptyTag
      };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);

      bundle.AddInput(
        new List<Address>
          {
            new Address
              {
                Balance = 40,
                KeyIndex = 0,
                SecurityLevel = 1,
                Trytes = "NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD"
              }
          });

      Assert.AreEqual(2, bundle.Balance);

      bundle.Finalize();
    }

    /// <summary>
    /// The test bundle has unspent inputs but remainder is given should be finalizable.
    /// </summary>
    [TestMethod]
    public void TestBundleHasUnspentInputsButRemainderIsGivenShouldBeFinalizable()
    {
      var transfer = new Transfer
      {
        Address = new Address { Balance = 42, Trytes = "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB" },
        Message = string.Empty,
        Tag = Bundle.EmptyTag
      };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);

      bundle.AddInput(
        new List<Address>
          {
            new Address
              {
                Balance = 42,
                KeyIndex = 0,
                SecurityLevel = 1,
                Trytes = "NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD"
              },
            new Address
              {
                Balance = 2,
                KeyIndex = 2,
                SecurityLevel = 1,
                Trytes = "XXYRPQ9BDZGKZZQLYNSBDD9HZLI9OFRK9TZCTU9PFAJYXZIZGO9BWLOCNGVMTLFQFMGJWYRMLXSCW9UTQ"
              }
          });


      bundle.AddRemainder(new Address { Trytes = "NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD" });

      bundle.Finalize();

      Assert.AreEqual(0, bundle.Balance);
      Assert.AreEqual(4, bundle.Transactions.Count);
    }

    /// <summary>
    /// The test bundle is not finalized should not be signable.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBundleIsNotFinalizedShouldNotBeSignable()
    {
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Address { Balance = 42, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ" },
        string.Empty,
        Bundle.EmptyTag,
        999999999L);

      bundle.Sign(new KeyGeneratorStub());
    }

    /// <summary>
    /// The test bundle is finalized should apply signing fragment.
    /// </summary>
    [TestMethod]
    public void TestBundleIsFinalizedShouldApplySigningFragmentAndNoExtraTransactionsForSecurityLevelOne()
    {
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Address { Balance = 42, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ" },
        string.Empty,
        Bundle.EmptyTag,
        999999999L);

      bundle.AddInput(
        new List<Address>
          {
            new Address
              {
                Balance = 40, 
                KeyIndex = 1, 
                SecurityLevel = 1, 
                Trytes = "KHWHSTISMVVSDCOMHVFIFCTINWZT9EHJUATYSMCXDSMZXPL9KXREBBYHJGRBCYVGPJQEHEDPXLBDJNQNX"
              }, 
            new Address
              {
                Balance = 2, 
                KeyIndex = 2, 
                SecurityLevel = 1, 
                Trytes = "GOAAMRU9EALPO9GKBOWUVZVQEJMB9CSGIZJATHRBTRRJPNTSQRZTASRBTQCRFAIDOGTWSHIDGOUUULQIG"
              }
          });

      bundle.Finalize();
      bundle.Sign(new KeyGeneratorStub());

      Assert.AreEqual(3, bundle.Transactions.Count);

      Assert.IsTrue(string.IsNullOrEmpty(bundle.Transactions[0].SignatureFragment));

      for (var i = 1; i < bundle.Transactions.Count; i++)
      {
        Assert.IsTrue(!string.IsNullOrEmpty(bundle.Transactions[i].SignatureFragment));
      }
    }

    /// <summary>
    /// The test bundle is finalized should apply signing fragment and with extra transactions for security level above one.
    /// </summary>
    [TestMethod]
    public void TestBundleIsFinalizedShouldApplySigningFragmentAndWithExtraTransactionsForSecurityLevelAboveOne()
    {
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Address { Balance = 42, Trytes = "TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ" },
        string.Empty,
        Bundle.EmptyTag,
        999999999L);

      bundle.AddInput(
        new List<Address>
          {
            new Address
              {
                Balance = 40, 
                KeyIndex = 1, 
                SecurityLevel = 2, 
                Trytes = "KHWHSTISMVVSDCOMHVFIFCTINWZT9EHJUATYSMCXDSMZXPL9KXREBBYHJGRBCYVGPJQEHEDPXLBDJNQNX"
              }, 
            new Address
              {
                Balance = 2, 
                KeyIndex = 2, 
                SecurityLevel = 3, 
                Trytes = "GOAAMRU9EALPO9GKBOWUVZVQEJMB9CSGIZJATHRBTRRJPNTSQRZTASRBTQCRFAIDOGTWSHIDGOUUULQIG"
              }
          });

      bundle.Finalize();
      bundle.Sign(new KeyGeneratorStub());

      Assert.AreEqual(6, bundle.Transactions.Count);

      Assert.IsTrue(string.IsNullOrEmpty(bundle.Transactions[0].SignatureFragment));

      for (var i = 1; i < bundle.Transactions.Count; i++)
      {
        Assert.IsTrue(!string.IsNullOrEmpty(bundle.Transactions[i].SignatureFragment));
      }
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