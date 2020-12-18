namespace Tangle.Net.Api
{
  using System;
  using System.Net.Http;
  using System.Text;
  using System.Threading.Tasks;

  using Isopoh.Cryptography.Blake2b;

  using Newtonsoft.Json;

  using Tangle.Net.Api.Responses;
  using Tangle.Net.Api.Responses.Message;
  using Tangle.Net.Entity;
  using Tangle.Net.Entity.Message;
  using Tangle.Net.Entity.Message.Payload;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Utils;

  public class NodeRestClient : IClient
  {
    /// <summary>
    /// Use default node (https://api.lb-0.testnet.chrysalis2.com) and remote PoW
    /// </summary>
    public NodeRestClient()
      : this("https://api.lb-0.testnet.chrysalis2.com")
    { }

    /// <summary>
    /// Use specified PoWProvider and default node (https://api.lb-0.testnet.chrysalis2.com)
    /// </summary>
    /// <param name="powProvider">
    /// The PoW Provider
    /// </param>
    public NodeRestClient(IPowProvider powProvider) : this(powProvider, "https://api.lb-0.testnet.chrysalis2.com")
    { }

    /// <summary>
    /// Use the specified node with remote PoW
    /// </summary>
    /// <param name="nodeUrl">Node Url</param>
    public NodeRestClient(string nodeUrl)
    {
      this.NodeUrl = nodeUrl;
    }

    /// <summary>
    /// Use specified PoWProvider and the specified node
    /// </summary>
    /// <param name="powProvider">The PoW Provider</param>
    /// <param name="nodeUrl">Node Url</param>
    public NodeRestClient(IPowProvider powProvider, string nodeUrl)
    {
      this.PowProvider = powProvider;
      this.NodeUrl = nodeUrl;
    }



    private IPowProvider PowProvider { get; set; }

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
    public async Task<bool> IsNodeHealthy()
    {
      using (var client = new HttpClient())
      {
        var response = await client.GetAsync($"{this.NodeUrl}/health");
        return response.IsSuccessStatusCode;
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
      if (this.PowProvider != null)
      {
        var nodeInfo = await this.GetNodeInfoAsync();
        message.NetworkId = nodeInfo.CalculateMessageNetworkId();

        var serialize = message.Serialize();
        var nonce = this.PowProvider.DoPow(serialize, 100);
        message.Nonce = Convert.ToString(nonce, 10);
      }

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