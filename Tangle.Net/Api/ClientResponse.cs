namespace Tangle.Net.Api
{
  using Newtonsoft.Json;

  internal class ClientResponse<T>
  {
    [JsonProperty("data")]
    public T Data { get; set; }
  }
}