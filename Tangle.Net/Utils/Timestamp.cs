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
    public static long UnixSecondsTimestamp
    {
      get
      {
        return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
      }
    }

    #endregion
  }
}