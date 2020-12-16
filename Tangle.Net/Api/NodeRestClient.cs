namespace Tangle.Net.Api
{
  using System.Net.Http;
  using System.Text;
  using System.Threading.Tasks;

  using Newtonsoft.Json;

  using Tangle.Net.Api.Responses;
  using Tangle.Net.Api.Responses.Message;
  using Tangle.Net.Models;
  using Tangle.Net.Models.Message;
  using Tangle.Net.Models.Message.MessagePayload;
  using Tangle.Net.Utils;

  public class NodeRestClient : IClient
  {
    public NodeRestClient()
      : this("https://api.lb-0.testnet.chrysalis2.com")
    {
    }

    public NodeRestClient(string nodeUrl)
    {
      this.NodeUrl = nodeUrl;
    }

    private string NodeUrl { get; }

    /// <inheritdoc />
    public async Task<Message<T>> GetMessageAsync<T>(string messageId)
      where T : PayloadBase
    {
      return await ExecuteRequestAsync<Message<T>>($"{this.NodeUrl}/api/v1/messages/{messageId}");
    }

    /// <inheritdoc />
    public async Task<MessageChildrenResponse> GetMessageChildrenAsync(string messageId)
    {
      return await ExecuteRequestAsync<MessageChildrenResponse>($"{this.NodeUrl}/api/v1/messages/{messageId}/children");
    }

    /// <inheritdoc />
    public async Task<MessageIdsByIndexResponse> GetMessageIdsByIndexAsync(string index)
    {
      return await ExecuteRequestAsync<MessageIdsByIndexResponse>($"{this.NodeUrl}/api/v1/messages?index={index}");
    }

    /// <inheritdoc />
    public async Task<MessageMetadata> GetMessageMetadataAsync(string messageId)
    {
      return await ExecuteRequestAsync<MessageMetadata>($"{this.NodeUrl}/api/v1/messages/{messageId}/metadata");
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
    public async Task<NodeInfo> GetNodeInfoAsync()
    {
      return await ExecuteRequestAsync<NodeInfo>($"{this.NodeUrl}/api/v1/info");
    }

    /// <inheritdoc />
    public async Task<TipsResponse> GetTipsAsync()
    {
      return await ExecuteRequestAsync<TipsResponse>($"{this.NodeUrl}/api/v1/tips");
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

    private static async Task<T> ExecuteRequestAsync<T>(string uri)
    {
      using (var client = new HttpClient())
      {
        var responseStream = await client.GetAsync(uri);
        var response = JsonConvert.DeserializeObject<ClientResponse<T>>(await responseStream.Content.ReadAsStringAsync());

        return response.Data;
      }
    }
  }
}