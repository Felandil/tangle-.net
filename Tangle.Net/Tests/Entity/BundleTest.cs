namespace Tangle.Net.Tests.Entity
{
  using System.Collections.Generic;

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
    public void TestTransferMessageFitsIntoOneTransactionShouldAddSingleTransaction()
    {
      var transfer = new Transfer
                       {
                         Address = "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB", 
                         Value = 42, 
                         Message = AsciiToTrytes.FromString("Hello World!"),
                         Tag = Bundle.EmptyTag
                       };

      var bundle = new Bundle();
      bundle.AddEntry(1, transfer.Address, transfer.Value, transfer.Tag, 999999999L);

      Assert.AreEqual(1, bundle.Transactions.Count);
    }

    /// <summary>
    /// The test insecure bundle hash is manipulated to be secure.
    /// </summary>
    [TestMethod]
    public void TestInsecureBundleHashIsManipulatedToBeSecure()
    {
      var transfer = new Transfer
      {
        Address = "9XV9RJGFJJZWITDPKSQXRTHCKJAIZZY9BYLBEQUXUNCLITRQDR9CCD99AANMXYEKD9GLJGVB9HIAGRIBQ",
        Value = 0,
        Tag = "PPDIDNQDJZGUQKOWJ9JZRCKOVGP",
        Timestamp = 1509136296
      };

      var bundle = new Bundle();
      bundle.AddEntry(1, transfer.Address, transfer.Value, transfer.Tag, transfer.Timestamp);
      bundle.Finalize();

      Assert.AreEqual(1, bundle.Transactions.Count);
      Assert.AreEqual("ZTDIDNQDJZGUQKOWJ9JZRCKOVGP", bundle.Transactions[0].ObsoleteTag);
      Assert.AreEqual("PPDIDNQDJZGUQKOWJ9JZRCKOVGP", bundle.Transactions[0].Tag);
      Assert.AreEqual("NYSJSEGCWESDAFLIFCNJFWGZ9PCYDOT9VCSALKBD9UUNKBJAJCB9KVMTHZDPRDDXC9UFJQBJBQFUPJKFC", bundle.Transactions[0].Bundle);
    }

    /// <summary>
    /// The super long message.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private static string GetSuperLongMessage()
    {
      return "'Good morning,' said Deep Thought at last."
          + "'Er... Good morning, O Deep Thought,' said Loonquawl nervously."
          + "'Do you have... er, that is..."
          + "'... an answer for you?' interrupted Deep Thought majestically. 'Yes. I have.'"
          + "The two men shivered with expectancy. Their waiting had not been in vain."
          + "'There really is one?' breathed Phouchg."
          + "'There really is one,' confirmed Deep Thought."
          + "'To Everything? To the great Question of Life, the Universe and Everything?'"
          + "'Yes.'"
          + "Both of the men had been trained for this moment; their lives had been a"
          + "  preparation for it; they had been selected at birth as those who would"
          + "  witness the answer; but even so they found themselves gasping and squirming"
          + "  like excited children."
          + "'And you're ready to give it to us?' urged Loonquawl."
          + "'I am.'"
          + "'Now?'"
          + "'Now,' said Deep Thought."
          + "They both licked their dry lips."
          + "'Though I don't think,' added Deep Thought, 'that you're going to like it.'"
          + "'Doesn't matter,' said Phouchg. 'We must know it! Now!'"
          + "'Now?' enquired Deep Thought."
          + "'Yes! Now!'"
          + "'All right,' said the computer and settled into silence again."
          + "  The two men fidgeted. The tension was unbearable."
          + "'You're really not going to like it,' observed Deep Thought."
          + "'Tell us!'"
          + "'All right,' said Deep Thought. 'The Answer to the Great Question...'"
          + "'Yes?'"
          + "'Of Life, the Universe and Everything...' said Deep Thought."
          + "'Yes??'"
          + "'Is...'"
          + "'Yes?!'"
          + "'Forty-two,' said Deep Thought, with infinite majesty and calm.";
    }

    #endregion
  }
}