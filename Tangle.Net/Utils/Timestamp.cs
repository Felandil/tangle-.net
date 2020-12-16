namespace Tangle.Net.Utils
{
  using System;

  public static class Timestamp
  {
    public static long UnixMillisecondsTimestamp => (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

    public static long UnixSecondsTimestamp => Convert(DateTime.UtcNow);

    public static long Convert(DateTime dateTime)
    {
      return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public static DateTime UnixTimestampToDateTime(this long value)
    {
      return new DateTime(1970, 1, 1).AddSeconds(value);
    }
  }
}