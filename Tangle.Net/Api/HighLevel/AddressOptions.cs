namespace Tangle.Net.Api.HighLevel
{
  public class AddressOptions
  {
    public AddressOptions(int startIndex = 0, int zeroCount = 20, int requiredCount = int.MaxValue)
    {
      StartIndex = startIndex;
      ZeroCount = zeroCount;
      RequiredCount = requiredCount;
    }

    /// <summary>
    ///   The start index for the wallet count address  | default: 0
    /// </summary>
    public int StartIndex { get; set; }

    /// <summary>
    ///   The number of addresses with 0 balance during lookup before aborting | default: 20
    /// </summary>
    public int ZeroCount { get; set; }

    /// <summary>
    /// The max number of addresses to find | default: int.MaxValue
    /// </summary>
    public int RequiredCount { get; set; }
  }
}