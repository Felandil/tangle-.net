namespace Tangle.Net.Utils
{
  using System;

  /// <summary>
  /// The timestamp.
  /// </summary>
  public static class Timestamp
  {
    #region Public Properties

    /// <summary>
    /// Gets the unix seconds timestamp.
    /// </summary>
    public static long UnixSecondsTimestamp => Convert(DateTime.UtcNow);

    public static long Convert(DateTime dateTime)
    {
      return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    #endregion
  }
}