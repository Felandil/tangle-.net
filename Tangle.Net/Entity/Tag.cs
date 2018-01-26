namespace Tangle.Net.Entity
{
  using System;

  /// <summary>
  /// The tag.
  /// </summary>
  public class Tag : TryteString
  {
    #region Constants

    /// <summary>
    /// The length.
    /// </summary>
    public const int Length = 27;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Tag"/> class.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    public Tag(string trytes)
      : base(trytes)
    {
      if (this.TrytesLength > Length)
      {
        throw new ArgumentException("Tag length must not be longer than " + Length);
      }

      if (this.TrytesLength < Length)
      {
        this.Pad(Length);
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tag"/> class.
    /// </summary>
    public Tag()
      : this(string.Empty)
    {
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the empty.
    /// </summary>
    public static Tag Empty
    {
      get
      {
        return new Tag();
      }
    }

    #endregion
  }
}