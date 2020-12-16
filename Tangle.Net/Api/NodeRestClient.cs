using System.Text;

namespace Tangle.Net.Api
{
  using System.Net.Http;
  using System.Threading.Tasks;

  using Tangle.Net.Api.Responses;

  using Newtonsoft.Json;

  using Tangle.Net.Models;
  using Tangle.Net.Models.MessagePayload;
  using Tangle.Net.Utils;

  public class NodeRestClient : IClient
  {
    private string NodeUrl { get; }

    public NodeRestClient() : this("https://api.lb-0.testnet.chrysalis2.com")
    {
    }

    public NodeRestClient(string nodeUrl)
    {
      this.NodeUrl = nodeUrl;
    }

    /// <inheritdoc />
    public async Task<MessageIdResponse> SendDataAsync(string payload, string index)
    {
      var tips = await this.GetTipsAsync();
      var message = new Message<IndexationPayload>
                      {
                        Parent1MessageId = tips.TipOneMessageId,
                        Parent2MessageId = tips.TipTwoMessageId,
                        Payload = new IndexationPayload { Index = index, Data = payload.ToHex() }
                      };

      return await this.SendMessageAsync(message);
    }

    /// <inheritdoc />
    public async Task<MessageIdResponse> SendMessageAsync<T>(Message<T> message)
      where T : PayloadBase
    {
      using (var client = new HttpClient())
      {
        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
        var responseStream = await client.PostAsync($"{this.NodeUrl}/api/v1/messages", content);

        var response = JsonConvert.DeserializeObject<ClientResponse<MessageIdResponse>>(await responseStream.Content.ReadAsStringAsync());

        return response.Data;
      }
    }

    /// <inheritdoc />
    public async Task<Message<T>> GetMessageAsync<T>(string messageId)
      where T : PayloadBase
    {
      using (var client = new HttpClient())
      {
        var responseStream = await client.GetAsync($"{this.NodeUrl}/api/v1/messages/{messageId}");
        var response = JsonConvert.DeserializeObject<ClientResponse<Message<T>>>(await responseStream.Content.ReadAsStringAsync());

        return response.Data;
      }
    }

    /// <inheritdoc />
    public async Task<MessageMetadataResponse> GetMessageMetadataAsync(string messageId)
    {
      using (var client = new HttpClient())
      {
        var responseStream = await client.GetAsync($"{this.NodeUrl}/api/v1/messages/{messageId}/metadata");
        var response = JsonConvert.DeserializeObject<ClientResponse<MessageMetadataResponse>>(await responseStream.Content.ReadAsStringAsync());

        return response.Data;
      }
    }

    /// <inheritdoc />
    public async Task<MessageRawResponse> GetMessageRawAsync(string messageId)
    {
      using (var client = new HttpClient())
      {
        var responseStream = await client.GetAsync($"{this.NodeUrl}/api/v1/messages/{messageId}/raw");

        return new MessageRawResponse { MessageRaw = await responseStream.Content.ReadAsByteArrayAsync() };
      }
    }

    /// <inheritdoc />
    public async Task<MessageIdsByIndexResponse> GetMessageIdsByIndexAsync(string index)
    {
      using (var client = new HttpClient())
      {
        var responseStream = await client.GetAsync($"{this.NodeUrl}/api/v1/messages?index={index}");
        var response = JsonConvert.DeserializeObject<ClientResponse<MessageIdsByIndexResponse>>(await responseStream.Content.ReadAsStringAsync());

        return response.Data;
      }
    }

    /// <inheritdoc />
    public async Task<TipsResponse> GetTipsAsync()
    {
      using (var client = new HttpClient())
      {
        var responseStream = await client.GetAsync($"{this.NodeUrl}/api/v1/tips");
        var response = JsonConvert.DeserializeObject<ClientResponse<TipsResponse>>(await responseStream.Content.ReadAsStringAsync());

        return response.Data;
      }
    }
  }
}
