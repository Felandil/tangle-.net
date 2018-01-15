namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;

  using Castle.Core.Internal;

  using Org.BouncyCastle.Math;

  using Tangle.Net.Source.Cryptography;

  /// <summary>
  /// The bundle.
  /// </summary>
  public class Bundle
  {
    #region Constants

    /// <summary>
    /// The empty hash.
    /// </summary>
    public const string EmptyHash = "999999999999999999999999999999999999999999999999999999999999999999999999999999999";

    /// <summary>
    /// The empty tag.
    /// </summary>
    public const string EmptyTag = "999999999999999999999999999";

    /// <summary>
    /// The empty timestamp.
    /// </summary>
    public const long EmptyTimestamp = 999999999L;

    #endregion

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
        var trx = new Transaction { Address = address, ObsoleteTag = tag, Timestamp = timestamp, Value = i == 0 ? value : 0, Tag = tag };
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
      var bundleHashTrytes = string.Empty;
      var valid = false;
      var kerl = new Kerl();

      do
      {
        kerl.Reset();
        for (var i = 0; i < this.Transactions.Count; i++)
        {
          this.Transactions[i].CurrentIndex = i;
          this.Transactions[i].LastIndex = this.Transactions.Count - 1;

          var transactionTrits = Converter.TrytesToTrits(this.Transactions[i].ToTrytes());
          kerl.Absorb(transactionTrits);
        }

        var hash = new int[Kerl.HashLength];
        kerl.Squeeze(hash);
        bundleHashTrytes = Converter.TritsToTrytes(hash);
        var normalizedBundleValue = NormalizeBundle(bundleHashTrytes);

        if (Array.IndexOf(normalizedBundleValue, 13) != -1)
        {
          var obsoleteTagTrits = Converter.TrytesToTrits(this.Transactions[0].ObsoleteTag);
          Converter.Increment(obsoleteTagTrits, 81);
          this.Transactions[0].ObsoleteTag = Converter.TritsToTrytes(obsoleteTagTrits);
        }
        else
        {
          valid = true;
        }
      }
      while (!valid);

      foreach (var transaction in this.Transactions)
      {
        transaction.Bundle = bundleHashTrytes;
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
    private static int[] NormalizeBundle(string bundleHash)
    {
      var sourceBundle = bundleHash.Select(hashTryte => Converter.TritsToInt(Converter.TrytesToTrits(string.Empty + hashTryte))).ToList();
      var normalizedBundle = new List<int>();
      const int ChunkSize = 27;

      for (var i = 0; i < 3; i++)
      {
        var chunk = sourceBundle.GetRange(i * ChunkSize, ChunkSize);
        long sum = chunk.Sum();

        while (sum > 0)
        {
          sum -= 1;
          for (var j = 0; j < ChunkSize; j++)
          {
            if (chunk[j] <= -13)
            {
              continue;
            }

            chunk[j]--;
            break;
          }
        }

        while (sum < 0)
        {
          sum += 1;
          for (var j = 0; j < ChunkSize; j++)
          {
            if (chunk[j] >= 13)
            {
              continue;
            }

            chunk[j]++;
            break;
          }
        }

        normalizedBundle.AddRange(chunk);
      }

      return normalizedBundle.ToArray();
    }

    #endregion
  }
}