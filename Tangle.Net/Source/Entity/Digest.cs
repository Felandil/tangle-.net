namespace Tangle.Net.Source.Entity
{
  using System;

  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The digest.
  /// </summary>
  public class Digest : TryteString
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Digest"/> class.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    public Digest(string trytes)
      : base(trytes)
    {
      ValidateTryteLength(trytes);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Digest"/> class.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    /// <param name="keyIndex">
    /// The key index.
    /// </param>
    public Digest(string trytes, int keyIndex)
      : base(trytes)
    {
      ValidateTryteLength(trytes);
      this.KeyIndex = keyIndex;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the key index.
    /// </summary>
    public int KeyIndex { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// The validate tryte length.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    private static void ValidateTryteLength(string trytes)
    {
      if (trytes.Length % Hash.Length != 0)
      {
        throw new ArgumentException("Tryte length has to be a multiple of " + Hash.Length);
      }
    }

    #endregion
  }
}