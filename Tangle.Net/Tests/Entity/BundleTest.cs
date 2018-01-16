namespace Tangle.Net.Tests.Entity
{
  using System;
  using System.Collections.Generic;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;
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
        new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 29 },
        new TryteString(), 
        Tag.Empty, 
        999999999L);
      bundle.AddTransaction(
        new Address("TESTVALUE9DONTUSEINPRODUCTION99999OGVEEFBCYAM9ZEAADBGBHH9BPBOHFEGCFAM9DESCCHODZ9Y") { Balance = 13 },
        new TryteString(),
        Tag.Empty, 
        999999999L);

      bundle.AddInput(
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
      bundle.AddRemainder(new Address("TESTVALUE9DONTUSEINPRODUCTION99999KAFGVCIBLHS9JBZCEFDELEGFDCZGIEGCPFEIQEYGA9UFPAE"));

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
        new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 84 },
        new TryteString(),
        Tag.Empty, 
        999999999L);

      bundle.AddInput(
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
                         Address = new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 42 },
                         Message = TryteString.FromString(GetSuperLongMessage()),
                         Tag = Tag.Empty
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
        new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 84 },
        new TryteString(),
        Tag.Empty, 
        999999999L);

      bundle.Finalize();

      bundle.AddInput(
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
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 84 },
        new TryteString(),
        Tag.Empty, 
        999999999L);

      bundle.Finalize();

      bundle.AddRemainder(new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD"));
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
        new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 84 },
        new TryteString(),
        Tag.Empty, 
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
                         Address = new Address("9XV9RJGFJJZWITDPKSQXRTHCKJAIZZY9BYLBEQUXUNCLITRQDR9CCD99AANMXYEKD9GLJGVB9HIAGRIBQ"),
                         Tag = new Tag("PPDIDNQDJZGUQKOWJ9JZRCKOVGP"), 
                         Timestamp = 1509136296, 
                         Message = new TryteString("JKAHSAAS")
                       };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, transfer.Timestamp);
      bundle.Finalize();

      Assert.AreEqual(1, bundle.Transactions.Count);
      Assert.AreEqual("ZTDIDNQDJZGUQKOWJ9JZRCKOVGP", bundle.Transactions[0].ObsoleteTag.Value);
      Assert.AreEqual("PPDIDNQDJZGUQKOWJ9JZRCKOVGP", bundle.Transactions[0].Tag.Value);
      Assert.AreEqual("NYSJSEGCWESDAFLIFCNJFWGZ9PCYDOT9VCSALKBD9UUNKBJAJCB9KVMTHZDPRDDXC9UFJQBJBQFUPJKFC", bundle.Hash.Value);
    }

    /// <summary>
    /// The test transfer message does not fit into one transaction should create second transaction.
    /// </summary>
    [TestMethod]
    public void TestTransferMessageDoesNotFitIntoOneTransactionShouldCreateSecondTransaction()
    {
      var transfer = new Transfer
                       {
                         Address = new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB") { Balance = 42 },
                         Message = TryteString.FromString(GetSuperLongMessage()),
                         Tag = Tag.Empty
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
                         Address = new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB") { Balance = 42 },
                         Message = TryteString.FromString("Hello World!"),
                         Tag = Tag.Empty
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
                         Address = new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB") { Balance = -42 },
                         Message = TryteString.FromString(GetSuperLongMessage()),
                         Tag = Tag.Empty
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
        Address = new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB") { Balance = 42 },
        Message = new TryteString("ASDF"),
        Tag = Tag.Empty
      };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);

      bundle.AddInput(
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
        Address = new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB") { Balance = 42 },
        Message = new TryteString(),
        Tag = Tag.Empty
      };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);

      bundle.AddInput(
        new List<Address>
          {
            new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD")
              {
                Balance = 40,
                KeyIndex = 0,
                SecurityLevel = 1
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
        Address = new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB") { Balance = 42 },
        Message = new TryteString(),
        Tag = Tag.Empty
      };

      var bundle = new Bundle();
      bundle.AddTransaction(transfer.Address, transfer.Message, transfer.Tag, 999999999L);

      bundle.AddInput(
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


      bundle.AddRemainder(new Address("NVGLHFZWLEQAWBDJXCWJBMVBVNXEG9DALNBTAYMKEMMJ9BCDVVHJJLSTQW9JEJXUUX9JNFGALBNASRDUD"));

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
        new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 42 },
        new TryteString("ASDF"),
        Tag.Empty,
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
        new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 42 },
        new TryteString("ASDF"),
        Tag.Empty,
        999999999L);

      bundle.AddInput(
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
        new Address("TESTVALUE9DONTUSEINPRODUCTION99999VELDTFQHDFTHIHFE9II9WFFDFHEATEI99GEDC9BAUH9EBGZ") { Balance = 42 },
        new TryteString("ASDF"),
        Tag.Empty,
        999999999L);

      bundle.AddInput(
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