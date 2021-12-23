using System;

namespace Tangle.Net.Api.HighLevel.Request
{
  public class SendDataRequest
  {
    private const int IndexationKeyMinLength = 1;
    private const int IndexationKeyMaxLength = 64;

    /// <param name="indexationKey">The index key</param>
    /// <param name="data">The data to send as UTF8 encoded string</param>
    public SendDataRequest(string indexationKey, string data)
    {
      IndexationKey = indexationKey;
      Data = data;
    }

    public string IndexationKey { get; }
    public string Data { get; }

    public bool Validate()
    {
      if (string.IsNullOrEmpty(IndexationKey))
      {
        throw new ArgumentException("Indexation Key can not be null or empty");
      }

      if (IndexationKey.Length > IndexationKeyMaxLength)
      {
        throw new ArgumentException(
          $"Incorrect IndexationKey length. Indexation key length must be between {IndexationKeyMinLength} and {IndexationKeyMaxLength}");
      }

      return true;
    }
  }
}