namespace Tangle.Net.Source.Entity
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
    public new const int Length = 27;

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
      if (trytes.Length > Length)
      {
        throw new ArgumentException("Tag length must not be longer than " + Length);
      }

      while (trytes.Length < Length)
      {
        trytes += '9';
      }

      this.Value = trytes;
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