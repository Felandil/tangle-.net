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
    public Hash Hash
    {
      get
      {
        return this.Transactions.Count > 0 ? this.Transactions[0].BundleHash : null;
      }
    }

    /// <summary>
    /// Gets or sets the transactions.
    /// </summary>
    public List<Transaction> Transactions { get; set; }

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
    /// <param name="transfer">
    /// The transfer.
    /// </param>
    public void AddTransaction(Transfer transfer)
    {
      if (this.Hash != null)
      {
        throw new InvalidOperationException("BundleHash is already finalized!");
      }

      if (transfer.Address.Balance < 0)
      {
        throw new ArgumentException("Use AddInputs add transfers for spending tokens.");
      }

      if (transfer.Message != null && transfer.Message.TrytesLength > Fragment.Length)
      {
        var i = 0;
        while (transfer.Message.TrytesLength > 0)
        {
          var chunkLength = transfer.Message.TrytesLength > Fragment.Length ? Fragment.Length : transfer.Message.TrytesLength;
          var fragment = transfer.Message.GetChunk<Fragment>(0, chunkLength);
          this.Transactions.Add(
            new Transaction
              {
                Address = transfer.Address, 
                Fragment = fragment, 
                ObsoleteTag = transfer.Tag, 
                Timestamp = transfer.Timestamp, 
                Value = i == 0 ? transfer.Address.Balance : 0, 
                Tag = transfer.Tag
              });

          transfer.Message = transfer.Message.GetChunk(chunkLength, transfer.Message.TrytesLength - chunkLength);

          i++;
        }
      }
      else
      {
        this.Transactions.Add(
          new Transaction
            {
              Address = transfer.Address, 
              Fragment = new Fragment(transfer.Message == null ? string.Empty : transfer.Message.Value), 
              ObsoleteTag = transfer.Tag, 
              Timestamp = transfer.Timestamp, 
              Value = transfer.Address.Balance, 
              Tag = transfer.Tag
            });
      }
    }

    /// <summary>
    /// The finalize.
    /// </summary>
    public void Finalize()
    {
      if (this.Hash != null)
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

      foreach (var transaction in this.Transactions)
      {
        transaction.BundleHash = bundleHash;
      }
    }

    /// <summary>
    /// The get messages.
    /// </summary>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<string> GetMessages()
    {
      var messages = new List<string>();
      var groupTransactions = this.GroupTransactions();
      foreach (var transactionGroup in groupTransactions)
      {
        if (transactionGroup[0].Value < 0)
        {
          continue;
        }

        var messageTrytes = string.Empty;
        foreach (var transaction in transactionGroup)
        {
          if (!transaction.Fragment.IsEmpty)
          {
            messageTrytes += transaction.Fragment.Value;
          }
        }

        var message = new TryteString(messageTrytes);

        if (!string.IsNullOrEmpty(message.Value))
        {
          messages.Add(message.ToString());
        }
      }

      return messages;
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
        throw new InvalidOperationException("Bundle must be finalized in order to sign it!");
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

        // alternative to AddTrytes from js library
        transaction.AttachmentTimestamp = 999999999;
        transaction.AttachmentTimestampLowerBound = 999999999;
        transaction.AttachmentTimestampUpperBound = 999999999;
        transaction.Nonce = new Tag();
        transaction.TrunkTransaction = new Hash();
        transaction.BranchTransaction = new Hash();
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// The group transactions.
    /// </summary>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    private IEnumerable<List<Transaction>> GroupTransactions()
    {
      return this.Transactions.GroupBy(t => t.Address.Value).Select(group => group.Select(transaction => transaction).ToList()).ToList();
    }

    #endregion
  }
}