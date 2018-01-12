namespace Tangle.Net.Source.Utils
{
  /// <summary>
  /// The string extensions.
  /// </summary>
  public static class StringExtensions
  {
    #region Public Methods and Operators

    /// <summary>
    /// The fill trytes.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string FillTrytes(this string value, int length)
    {
      while (value.Length < length)
      {
        value += '9';
      }

      return value;
    }

    #endregion
  }
}