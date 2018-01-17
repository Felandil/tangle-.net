namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

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
    /// Gets the balance.
    /// </summary>
    public int Balance
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
    public Hash Hash { get; private set; }

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
      if (this.Hash != null)
      {
        throw new InvalidOperationException("BundleHash is already finalized!");
      }

      foreach (var address in addresses)
      {
        for (var i = 0; i < address.SecurityLevel; i++)
        {
          this.Transactions.Add(
            new Transaction { Address = address, Value = i == 0 ? -address.Balance : 0, Tag = Tag.Empty, ObsoleteTag = Tag.Empty });
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
      if (this.Hash != null)
      {
        throw new InvalidOperationException("BundleHash is already finalized!");
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
    public void AddTransaction(Address address, TryteString message, Tag tag, int timestamp)
    {
      if (this.Hash != null)
      {
        throw new InvalidOperationException("BundleHash is already finalized!");
      }

      if (address.Balance < 0)
      {
        throw new ArgumentException("Use AddInputs add transfers for spending tokens.");
      }

      if (message.TrytesLength > Transaction.MaxMessageLength)
      {
        var i = 0;
        while (message.TrytesLength > 0)
        {
          var chunkLength = message.TrytesLength > Transaction.MaxMessageLength ? Transaction.MaxMessageLength : message.TrytesLength;
          var fragment = message.GetChunk(0, chunkLength);
          this.Transactions.Add(
            new Transaction { Address = address, Message = fragment, ObsoleteTag = tag, Timestamp = timestamp, Value = i == 0 ? address.Balance : 0, Tag = tag });

          message = message.GetChunk(chunkLength, message.TrytesLength - chunkLength);

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
    /// The finalize.
    /// </summary>
    public void Finalize()
    {
      if (this.Hash != null)
      {
        throw new InvalidOperationException("BundleHash is already finalized!");
      }

      if (this.Transactions.Count == 0)
      {
        throw new ArgumentException("At least one transaction must be added before finalizing bundle.");
      }

      var balance = this.Balance;
      if (balance < 0)
      {
        if (this.RemainderAddress != null && !string.IsNullOrEmpty(this.RemainderAddress.Value))
        {
          this.Transactions.Add(new Transaction { Address = this.RemainderAddress, Tag = Tag.Empty, Value = -balance, ObsoleteTag = Tag.Empty });
        }
        else
        {
          throw new InvalidOperationException("BundleHash balance is not even. Add remainder address.");
        }
      }

      if (balance > 0)
      {
        throw new InvalidOperationException("Insufficient value submitted.");
      }

      Hash bundleHash;
      var valid = false;
      var kerl = new Kerl();

      do
      {
        kerl.Reset();
        for (var i = 0; i < this.Transactions.Count; i++)
        {
          this.Transactions[i].CurrentIndex = i;
          this.Transactions[i].LastIndex = this.Transactions.Count - 1;

          var transactionTrits = Converter.TrytesToTrits(this.Transactions[i].SignatureValidationTrytes());
          kerl.Absorb(transactionTrits);
        }

        var hashTrits = new int[Kerl.HashLength];
        kerl.Squeeze(hashTrits);
        bundleHash = new Hash(Converter.TritsToTrytes(hashTrits));
        var normalizedBundleValue = Hash.Normalize(bundleHash);

        if (Array.IndexOf(normalizedBundleValue, 13) != -1)
        {
          var obsoleteTagTrits = Converter.TrytesToTrits(this.Transactions[0].ObsoleteTag.Value);
          Converter.Increment(obsoleteTagTrits, 81);
          this.Transactions[0].ObsoleteTag = new Tag(Converter.TritsToTrytes(obsoleteTagTrits));
        }
        else
        {
          valid = true;
        }
      }
      while (!valid);

      this.Hash = bundleHash;
      foreach (var transaction in this.Transactions)
      {
        transaction.BundleHash = this.Hash;
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
      if (this.Hash == null)
      {
        throw new InvalidOperationException("BundleHash must be finalized in order to sign it!");
      }

      var i = 0;
      while (i < this.Transactions.Count)
      {
        var transaction = this.Transactions[i];

        if (transaction.Value < 0)
        {
          var privateKey = keyGenerator.GetKeyFor(transaction.Address);
          privateKey.SignInputTransactions(this, i);

          i += transaction.Address.SecurityLevel;
        }
        else
        {
          i += 1;
        }
      }
    }

    #endregion
  }
}