namespace Tangle.Net.Source.Utils
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;

  using Tangle.Net.Source.Entity;

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
    /// The are valid inputs.
    /// </summary>
    /// <param name="inputs">
    /// The inputs.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool AreValidInputs(IEnumerable<Input> inputs)
    {
      return inputs.All(input => IsAddress(input.Address));
    }

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
    /// The is transfers array.
    /// </summary>
    /// <param name="transfers">
    /// The transfers.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool IsTransfersArray(IEnumerable<Transfer> transfers)
    {
      foreach (var transfer in transfers)
      {
        if (!IsAddress(transfer.Address.Trytes))
        {
          return false;
        }

        if (!IsTrytes(transfer.Message))
        {
          return false;
        }

        if (!IsTrytes(transfer.Tag, 0, 27))
        {
          return false;
        }
      }

      return true;
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

    /// <summary>
    /// The is trytes.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    /// <param name="start">
    /// The start.
    /// </param>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool IsTrytes(string trytes, int start, int length)
    {
      var regex = new Regex("^[9A-Z]{" + start + "," + length + "}$");
      return regex.IsMatch(trytes);
    }

    #endregion

    #region Methods

    /// <summary>
    /// The is address.
    /// </summary>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    private static bool IsAddress(string address)
    {
      if (address.Length == 90 && !IsTrytes(address, 90))
      {
        return false;
      }

      return IsTrytes(address, 81);
    }

    #endregion
  }
}