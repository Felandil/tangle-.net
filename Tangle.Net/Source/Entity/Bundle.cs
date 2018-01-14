namespace Tangle.Net.Source.Entity
{
  using System.Collections.Generic;
  using System.Linq;

  using Castle.Core.Internal;

  using Org.BouncyCastle.Math;

  using Tangle.Net.Source.Cryptography;

  /// <summary>
  /// The bundle.
  /// </summary>
  public class Bundle
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Bundle"/> class.
    /// </summary>
    public Bundle()
    {
      this.Transactions = new List<Transaction>();
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the transactions.
    /// </summary>
    public List<Transaction> Transactions { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The add entry.
    /// </summary>
    /// <param name="signatureMessageLength">
    /// The signature message length.
    /// </param>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <param name="tag">
    /// The tag.
    /// </param>
    /// <param name="timestamp">
    /// The timestamp.
    /// </param>
    public void AddEntry(int signatureMessageLength, string address, long value, string tag, long timestamp)
    {
      for (var i = 0; i < signatureMessageLength; i++)
      {
        var trx = new Transaction { Address = address, ObsoleteTag = tag, Timestamp = timestamp, Value = i == 0 ? value : 0 };
        this.Transactions.Add(trx);
      }
    }

    /// <summary>
    /// The add trytes.
    /// </summary>
    /// <param name="signatureFragments">
    /// The signature fragments.
    /// </param>
    public void AddTrytes(List<string> signatureFragments)
    {
      const string EmptyHash = "999999999999999999999999999999999999999999999999999999999999999999999999999999999";
      const string EmptyTag = "999999999999999999999999999";
      const long EmptyTimestamp = 999999999L;
      var emptySignatureFragment = "9";

      for (var j = 0; emptySignatureFragment.Length < 2187; j++)
      {
        emptySignatureFragment += '9';
      }

      for (var i = 0; i < this.Transactions.Count; i++)
      {
        // Fill empty signatureMessageFragment
        this.Transactions[i].SignatureFragments = (signatureFragments.Count <= i || signatureFragments[i].IsNullOrEmpty())
                                                    ? emptySignatureFragment
                                                    : signatureFragments[i];

        // Fill empty trunkTransaction
        this.Transactions[i].TrunkTransaction = EmptyHash;

        // Fill empty branchTransaction
        this.Transactions[i].BranchTransaction = EmptyHash;

        this.Transactions[i].AttachmentTimestamp = EmptyTimestamp;
        this.Transactions[i].AttachmentTimestampLowerBound = EmptyTimestamp;
        this.Transactions[i].AttachmentTimestampUpperBound = EmptyTimestamp;

        // Fill empty nonce
        this.Transactions[i].Nonce = EmptyTag;
      }
    }

    /// <summary>
    /// The finalize.
    /// </summary>
    public void Finalize()
    {
      int[] normalizedBundleValue;
      var hash = new int[243];
      var obsoleteTagTrits = new int[81];
      string hashInTrytes;
      bool valid;
      var kerl = new Kerl();
      do
      {
        kerl.Reset();

        for (var i = 0; i < this.Transactions.Count; i++)
        {
          var valueTrits = Converter.ConvertBigIntToTrits(new BigInteger(this.Transactions[i].Value.ToString()), 81);
          var timestampTrits = Converter.ConvertBigIntToTrits(new BigInteger(this.Transactions[i].Timestamp.ToString()), 27);

          this.Transactions[i].CurrentIndex = i;
          var currentIndexTrits = Converter.IntToTrits(this.Transactions[i].CurrentIndex, 27);

          this.Transactions[i].LastIndex = this.Transactions.Count - 1;

          var lastIndexTrits = Converter.IntToTrits(this.Transactions[i].LastIndex, 27);

          var t =
            Converter.TrytesToTrits(
              this.Transactions[i].Address + Converter.TritsToTrytes(valueTrits) + this.Transactions[i].ObsoleteTag
              + Converter.TritsToTrytes(timestampTrits) + Converter.TritsToTrytes(currentIndexTrits) + Converter.TritsToTrytes(lastIndexTrits));

          kerl.Absorb(t, 0, t.Length);
        }

        kerl.Squeeze(hash, 0, hash.Length);
        hashInTrytes = Converter.TritsToTrytes(hash);
        normalizedBundleValue = this.NormalizeBundle(hashInTrytes);

        var foundValue = false;
        foreach (var normalizedBundleValueEntry in normalizedBundleValue.Where(normalizedBundleValueEntry => normalizedBundleValueEntry == 13))
        {
          foundValue = true;
          obsoleteTagTrits = Converter.TrytesToTrits(this.Transactions[0].ObsoleteTag);
          Converter.Increment(obsoleteTagTrits, 81);
          this.Transactions[0].ObsoleteTag = Converter.TritsToTrytes(obsoleteTagTrits);
        }

        valid = !foundValue;
      }
      while (!valid);

      foreach (var transaction in this.Transactions)
      {
        transaction.Bundle = hashInTrytes;
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// The normalize bundle.
    /// </summary>
    /// <param name="bundleHash">
    /// The bundle hash.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    private int[] NormalizeBundle(string bundleHash)
    {
      var normalizedBundle = new int[81];

      for (var i = 0; i < 3; i++)
      {
        long sum = 0;
        for (var j = 0; j < 27; j++)
        {
          sum += normalizedBundle[i * 27 + j] = Converter.TritsToInt(Converter.TrytesToTrits(string.Empty + bundleHash[i * 27 + j]));
        }

        if (sum >= 0)
        {
          while (sum-- > 0)
          {
            for (var j = 0; j < 27; j++)
            {
              if (normalizedBundle[i * 27 + j] > -13)
              {
                normalizedBundle[i * 27 + j]--;
                break;
              }
            }
          }
        }
        else
        {
          while (sum++ < 0)
          {
            for (int j = 0; j < 27; j++)
            {
              if (normalizedBundle[i * 27 + j] < 13)
              {
                normalizedBundle[i * 27 + j]++;
                break;
              }
            }
          }
        }
      }

      return normalizedBundle;
    }

    #endregion
  }
}