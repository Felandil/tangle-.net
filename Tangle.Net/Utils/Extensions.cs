namespace Tangle.Net.Utils
{
  public static class Extensions
  {
    public static T[] Slice<T>(this T[] source, int start, int end)
    {
      if (end < 0)
      {
        end = source.Length;
      }

      var length = end - start;
      var result = new T[length];
      for (var i = 0; i < length; i++)
      {
        result[i] = source[i + start];
      }

      return result;
    }

    public static T[] Slice<T>(this T[] source, int start)
    {
      return Slice(source, start, -1);
    }
  }
}