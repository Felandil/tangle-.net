namespace Tangle.Net.Entity
{
  using System;

  /// <summary>
  /// The transaction trytes.
  /// </summary>
  public class TransactionTrytes : TryteString
  {
    #region Constants

    /// <summary>
    /// The length.
    /// </summary>
    public const int Length = 2673;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionTrytes"/> class.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    public TransactionTrytes(string value)
      : base(value)
    {
      if (this.TrytesLength > Length)
      {
        throw new ArgumentException("Transaction tryte length must not be bigger than " + Length);
      }

      if (this.TrytesLength < Length)
      {
        this.Pad(Length);
      }
    }

    #endregion
  }
}