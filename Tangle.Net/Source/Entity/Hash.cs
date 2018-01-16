namespace Tangle.Net.Source.Entity
{
  using System;

  using Tangle.Net.Source.Cryptography;

  /// <summary>
  /// The hash.
  /// </summary>
  public class Hash : TryteString
  {
    #region Constants

    /// <summary>
    /// The length.
    /// </summary>
    public new const int Length = Kerl.HashLength / Converter.Radix;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Hash"/> class.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    public Hash(string trytes)
      : base(trytes)
    {
      if (trytes.Length != Length)
      {
        throw new ArgumentException("Hash must be exactly of length " + Length);
      }
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the empty.
    /// </summary>
    public static Hash Empty
    {
      get
      {
        return new Hash("999999999999999999999999999999999999999999999999999999999999999999999999999999999");
      }
    }

    #endregion
  }
}