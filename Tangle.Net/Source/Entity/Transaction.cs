namespace Tangle.Net.Source.Entity
{
  using System.Globalization;

  using Org.BouncyCastle.Math;

  using Tangle.Net.Source.Cryptography;

  /// <summary>
  /// The transaction.
  /// </summary>
  public class Transaction
  {
    /// <summary>
    /// The max message length.
    /// </summary>
    public const int MaxMessageLength = 2187;

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
    /// Gets or sets the last index.
    /// </summary>
    public int LastIndex { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public TryteString Message { get; set; }

    /// <summary>
    /// Gets or sets the nonce.
    /// </summary>
    public Tag Nonce { get; set; }

    /// <summary>
    /// Gets or sets the obsolete tag.
    /// </summary>
    public Tag ObsoleteTag { get; set; }

    /// <summary>
    /// Gets or sets the signature fragments.
    /// </summary>
    public string SignatureFragment { get; set; }

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
    /// The to trytes.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string ToTrytes()
    {
      var currentIndexTrits = Converter.IntToTrits(this.CurrentIndex, 27);
      var valueTrits = Converter.ConvertBigIntToTrits(new BigInteger(this.Value.ToString(CultureInfo.InvariantCulture)), 81);
      var timestampTrits = Converter.ConvertBigIntToTrits(new BigInteger(this.Timestamp.ToString(CultureInfo.InvariantCulture)), 27);
      var lastIndexTrits = Converter.IntToTrits(this.LastIndex, 27);

      return this.Address.Value + Converter.TritsToTrytes(valueTrits) + this.ObsoleteTag.Value + Converter.TritsToTrytes(timestampTrits)
             + Converter.TritsToTrytes(currentIndexTrits) + Converter.TritsToTrytes(lastIndexTrits);
    }

    #endregion
  }
}