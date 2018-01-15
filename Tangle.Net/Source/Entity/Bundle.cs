namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Castle.Core.Internal;

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
    /// Gets the balance.
    /// </summary>
    public long Balance
    {
      get
      {
        var inputValue = this.Transactions.Where(transaction => transaction.Value > 0).Sum(transaction => transaction.Value);
        var outputValue = this.Transactions.Where(transaction => transaction.Value < 0).Sum(transaction => transaction.Value);

        return inputValue + outputValue;
      }
    }

    /// <summary>
    /// Gets the hash.
    /// </summary>
    public string Hash { get; private set; }

    /// <summary>
    /// Gets the transactions.
    /// </summary>
    public List<Transaction> Transactions { get; private set; }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the remainder address.
    /// </summary>
    private Address RemainderAddress { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The add input.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    public void AddInput(IEnumerable<Address> addresses)
    {
      if (!string.IsNullOrEmpty(this.Hash))
      {
        throw new InvalidOperationException("Bundle is already finalized!");
      }

      foreach (var address in addresses)
      {
        for (var i = 0; i < address.SecurityLevel; i++)
        {
          this.Transactions.Add(
            new Transaction { Address = address, Value = i == 0 ? -address.Balance : 0, Tag = EmptyTag, ObsoleteTag = EmptyTag });
        }
      }
    }

    /// <summary>
    /// The add remainder.
    /// </summary>
    /// <param name="address">
    /// The address.
    /// </param>
    public void AddRemainder(Address address)
    {
      if (!string.IsNullOrEmpty(this.Hash))
      {
        throw new InvalidOperationException("Bundle is already finalized!");
      }

      this.RemainderAddress = address;
    }

    /// <summary>
    /// The add entry.
    /// </summary>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="tag">
    /// The tag.
    /// </param>
    /// <param name="timestamp">
    /// The timestamp.
    /// </param>
    public void AddTransaction(Address address, string message, string tag, long timestamp)
    {
      if (!string.IsNullOrEmpty(this.Hash))
      {
        throw new InvalidOperationException("Bundle is already finalized!");
      }

      if (address.Balance < 0)
      {
        throw new ArgumentException("Use AddInputs add transfers for spending tokens.");
      }

      if (message.Length > Transaction.MaxMessageLength)
      {
        var i = 0;
        while (message.Length > 0)
        {
          var chunkLength = message.Length > Transaction.MaxMessageLength ? Transaction.MaxMessageLength : message.Length;
          var fragment = message.Substring(0, chunkLength);
          this.Transactions.Add(
            new Transaction { Address = address, Message = fragment, ObsoleteTag = tag, Timestamp = timestamp, Value = i == 0 ? address.Balance : 0, Tag = tag });

          message = message.Substring(chunkLength, message.Length - chunkLength);

          i++;
        }
      }
      else
      {
        this.Transactions.Add(
          new Transaction { Address = address, Message = message, ObsoleteTag = tag, Timestamp = timestamp, Value = address.Balance, Tag = tag });
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
        this.Transactions[i].SignatureFragment = (signatureFragments.Count <= i || signatureFragments[i].IsNullOrEmpty())
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
      if (!string.IsNullOrEmpty(this.Hash))
      {
        throw new InvalidOperationException("Bundle is already finalized!");
      }

      if (this.Transactions.Count == 0)
      {
        throw new ArgumentException("At least one transaction must be added before finalizing bundle.");
      }

      var balance = this.Balance;
      if (balance < 0)
      {
        if (this.RemainderAddress != null && !string.IsNullOrEmpty(this.RemainderAddress.Trytes))
        {
          this.Transactions.Add(new Transaction { Address = this.RemainderAddress, Tag = EmptyTag, Value = -balance, ObsoleteTag = EmptyTag });
        }
        else
        {
          throw new InvalidOperationException("Bundle balance is not even. Add remainder address.");
        }
      }

      if (balance > 0)
      {
        throw new InvalidOperationException("Insufficient value submitted.");
      }

      string bundleHashTrytes;
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

      this.Hash = bundleHashTrytes;
      foreach (var transaction in this.Transactions)
      {
        transaction.Bundle = bundleHashTrytes;
      }
    }

    /// <summary>
    /// The sign.
    /// </summary>
    /// <param name="keyGenerator">
    /// The key Generator.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when bundle is not finalized.
    /// </exception>
    public void Sign(IKeyGenerator keyGenerator)
    {
      if (string.IsNullOrEmpty(this.Hash))
      {
        throw new InvalidOperationException("Bundle must be finalized in order to sign it!");
      }

      var i = 0;
      while (i < this.Transactions.Count)
      {
        var transaction = this.Transactions[i];

        if (transaction.Value < 0)
        {
          var privateKey = keyGenerator.GetKeyFor(transaction.Address);
          privateKey.SignInputTransactions(this.Transactions, i);

          i += transaction.Address.SecurityLevel;
        }
        else
        {
          i += 1;
        }
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