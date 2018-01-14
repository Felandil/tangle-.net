namespace Tangle.Net.Source.Entity
{
  using System;

  /// <summary>
  /// The transaction.
  /// </summary>
  public class Transaction
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public string Address { get; set; }

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
    public string BranchTransaction { get; set; }

    /// <summary>
    /// Gets or sets the bundle.
    /// </summary>
    public string Bundle { get; set; }

    /// <summary>
    /// Gets or sets the current index.
    /// </summary>
    public int CurrentIndex { get; set; }

    /// <summary>
    /// Gets or sets the last index.
    /// </summary>
    public int LastIndex { get; set; }

    /// <summary>
    /// Gets or sets the nonce.
    /// </summary>
    public string Nonce { get; set; }

    /// <summary>
    /// Gets or sets the obsolete tag.
    /// </summary>
    public string ObsoleteTag { get; set; }

    /// <summary>
    /// Gets or sets the signature fragments.
    /// </summary>
    public string SignatureFragments { get; set; }

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the trunk transaction.
    /// </summary>
    public string TrunkTransaction { get; set; }

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
      throw new NotImplementedException();
    }

    #endregion
  }
}