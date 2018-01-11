namespace Tangle.Net.Source.Utils
{
  using System;
  using System.Linq;
  using System.Text;

  /// <summary>
  /// The ascii to trytes.
  /// </summary>
  public static class AsciiToTrytes
  {
    #region Constants

    /// <summary>
    /// The tryte values.
    /// </summary>
    public const string TryteAlphabet = "9ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The convert from string.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string FromString(string input)
    {
      var trytes = string.Empty;

      if (input.Any(c => c > 255))
      {
        throw new ArgumentException("Detected non ASCII string input.");
      }

      foreach (var asciiValue in Encoding.ASCII.GetBytes(input))
      {
        var firstValue = asciiValue % 27;
        var secondValue = (asciiValue - firstValue) / 27;

        var trytesValue = string.Format("{0}{1}", TryteAlphabet[firstValue], TryteAlphabet[secondValue]);

        trytes += trytesValue;
      }

      return trytes;
    }

    /// <summary>
    /// The from trytes.
    /// </summary>
    /// <param name="inputTrytes">
    /// The input trytes.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if inputTrytes length is odd
    /// </exception>
    public static string FromTrytes(string inputTrytes)
    {
      if (inputTrytes.Length % 2 == 1)
      {
        throw new ArgumentException("Trytes are of odd length.");
      }

      var outputString = string.Empty;

      for (var i = 0; i < inputTrytes.Length; i += 2)
      {
        var asciiValue = TryteAlphabet.IndexOf(inputTrytes[i]) + (TryteAlphabet.IndexOf(inputTrytes[i + 1]) * 27);
        outputString += (char)asciiValue;
      }

      return outputString;
    }

    #endregion
  }
}