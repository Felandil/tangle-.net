// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputValidator.cs" company="Felandil IT">
//    Copyright (c) 2008 -2018 Felandil IT. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Tangle.Net.Source.Utils
{
  using System.Text.RegularExpressions;

  /// <summary>
  /// The input validator.
  /// </summary>
  public static class InputValidator
  {
    #region Constants

    /// <summary>
    /// The hash length.
    /// </summary>
    public const int HashLength = 81;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The is hash.
    /// </summary>
    /// <param name="hash">
    /// The hash.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool IsHash(string hash)
    {
      return IsTrytes(hash, HashLength);
    }

    /// <summary>
    /// The is trytes.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool IsTrytes(string trytes)
    {
      var regex = new Regex("^[9A-Z]*$");
      return regex.IsMatch(trytes);
    }

    /// <summary>
    /// The is trytes.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool IsTrytes(string trytes, int length)
    {
      var regex = new Regex("^[9A-Z]{" + length + "}$");
      return regex.IsMatch(trytes);
    }

    #endregion
  }
}