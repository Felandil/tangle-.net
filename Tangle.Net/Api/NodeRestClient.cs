using System;
using System.Collections.Generic;
using System.Text;

namespace Tangle.Net.Api
{
  using System.Net.Http;
  using System.Text.Json;
  using System.Threading.Tasks;

  using Tangle.Net.Api.Responses;
  using System.Text.Json.Serialization;

  using Newtonsoft.Json;

  using Tangle.Net.Models;
  using Tangle.Net.Models.MessagePayload;

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
    public async Task<MessageIdResponse> SendMessageAsync(string payload, string index)
    {
      var tips = await this.GetTipsAsync();
      var message = new Message<IndexationPayload>
                      {
                        Parent1MessageId = tips.TipOneMessageId,
                        Parent2MessageId = tips.TipTwoMessageId,
                        Payload = new IndexationPayload { Index = index, Data = "" }
                      };

      using (var client = new HttpClient())
      {

        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
        var responseStream = await client.PostAsync($"{this.NodeUrl}/api/v1/messages", content);

        var response = JsonConvert.DeserializeObject<ClientResponse<MessageIdResponse>>(await responseStream.Content.ReadAsStringAsync());

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
