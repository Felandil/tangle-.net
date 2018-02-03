namespace Tangle.Net.Entity
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Utils;

  /// <summary>
  /// The transaction.
  /// </summary>
  public class Transaction
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public Address Address { get; set; }

    /// <summary>
    /// Gets or sets the attachment timestamp.
    /// </summary>
    public long AttachmentTimestamp { get; set; }

    /// <summary>
    /// Gets or sets the attachment timestamp lower bound.
    /// </summary>
    public long AttachmentTimestampLowerBound { get; set; }

    /// <summary>
    /// Gets or sets the attachment timestamp upper bound.
    /// </summary>
    public long AttachmentTimestampUpperBound { get; set; }

    /// <summary>
    /// Gets or sets the branch transaction.
    /// </summary>
    public Hash BranchTransaction { get; set; }

    /// <summary>
    /// Gets or sets the bundle.
    /// </summary>
    public Hash BundleHash { get; set; }

    /// <summary>
    /// Gets or sets the current index.
    /// </summary>
    public int CurrentIndex { get; set; }

    /// <summary>
    /// Gets or sets the signature fragments.
    /// </summary>
    public Fragment Fragment { get; set; }

    /// <summary>
    /// Gets or sets the hash.
    /// </summary>
    public Hash Hash { get; set; }

    /// <summary>
    /// Gets a value indicating whether is tail.
    /// </summary>
    public bool IsTail
    {
      get
      {
        return this.CurrentIndex == 0;
      }
    }

    /// <summary>
    /// Gets or sets the last index.
    /// </summary>
    public int LastIndex { get; set; }

    /// <summary>
    /// Gets or sets the nonce.
    /// </summary>
    public Tag Nonce { get; set; }

    /// <summary>
    /// Gets or sets the obsolete tag.
    /// </summary>
    public Tag ObsoleteTag { get; set; }

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    public Tag Tag { get; set; }

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the trunk transaction.
    /// </summary>
    public Hash TrunkTransaction { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public long Value { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The from trytes.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    /// <param name="hash">
    /// The hash.
    /// </param>
    /// <returns>
    /// The <see cref="Transaction"/>.
    /// </returns>
    public static Transaction FromTrytes(TransactionTrytes trytes, Hash hash = null)
    {
      if (hash == null)
      {
        var hashTrits = new int[Kerl.HashLength];
        var kerl = new Curl();
        kerl.Absorb(trytes.ToTrits());
        kerl.Squeeze(hashTrits);

        hash = new Hash(Converter.TritsToTrytes(hashTrits));
      }

      var valueTrits = trytes.GetChunk(2268, 27).ToTrits();
      var attachementTimestamp = trytes.GetChunk(2619, 9).ToTrits();
      var attachementTimestampLower = trytes.GetChunk(2628, 9).ToTrits();
      var attachementTimestampUpper = trytes.GetChunk(2637, 9).ToTrits();
      var timestamp = trytes.GetChunk(2322, 9).ToTrits();
      return new Transaction
               {
                 Address = trytes.GetChunk<Address>(2187, Address.Length), 
                 Hash = hash, 
                 Fragment = trytes.GetChunk<Fragment>(0, 2187), 
                 Value = Converter.ConvertTritsToBigInt(valueTrits, 0, valueTrits.Length).LongValue, 
                 ObsoleteTag = trytes.GetChunk<Tag>(2295, Tag.Length),
                 Timestamp = Converter.ConvertTritsToBigInt(timestamp, 0, 27).LongValue, 
                 CurrentIndex = Converter.TritsToInt(trytes.GetChunk(2331, 9).ToTrits()), 
                 LastIndex = Converter.TritsToInt(trytes.GetChunk(2340, 9).ToTrits()), 
                 BundleHash = trytes.GetChunk<Hash>(2349, Hash.Length), 
                 TrunkTransaction = trytes.GetChunk<Hash>(2430, Hash.Length), 
                 BranchTransaction = trytes.GetChunk<Hash>(2511, Hash.Length), 
                 Tag = trytes.GetChunk<Tag>(2592, Tag.Length), 
                 Nonce = trytes.GetChunk<Tag>(2646, Tag.Length),
                 AttachmentTimestamp = Converter.ConvertTritsToBigInt(attachementTimestamp, 0, attachementTimestamp.Length).LongValue,
                 AttachmentTimestampLowerBound = Converter.ConvertTritsToBigInt(attachementTimestampLower, 0, attachementTimestampLower.Length).LongValue,
                 AttachmentTimestampUpperBound = Converter.ConvertTritsToBigInt(attachementTimestampUpper, 0, attachementTimestampUpper.Length).LongValue, 
               };
    }

    /// <summary>
    /// The to trytes.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string SignatureValidationTrytes()
    {
      return this.Address.Value + this.Value.ToTrytes(81).Value + this.ObsoleteTag.Value + this.Timestamp.ToTrytes(27).Value
             + this.CurrentIndex.ToTrytes(27).Value + this.LastIndex.ToTrytes(27).Value;
    }

    /// <summary>
    /// The to trytes.
    /// </summary>
    /// <returns>
    /// The <see cref="TransactionTrytes"/>.
    /// </returns>
    public TransactionTrytes ToTrytes()
    {
      return
        new TransactionTrytes(
          this.Fragment.Value + this.Address.Value + this.Value.ToTrytes(81).Value + this.ObsoleteTag.Value + this.Timestamp.ToTrytes(27).Value
          + this.CurrentIndex.ToTrytes(27).Value + this.LastIndex.ToTrytes(27).Value + this.BundleHash.Value + this.TrunkTransaction.Value
          + this.BranchTransaction.Value + this.Tag.Value + this.AttachmentTimestamp.ToTrytes(27).Value
          + this.AttachmentTimestampLowerBound.ToTrytes(27).Value + this.AttachmentTimestampUpperBound.ToTrytes(27).Value + this.Nonce.Value);
    }

    #endregion
  }
}