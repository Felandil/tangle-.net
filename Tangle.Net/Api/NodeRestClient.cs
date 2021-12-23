﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tangle.Net.Api.Exception;
using Tangle.Net.Api.Response;
using Tangle.Net.Api.Response.Message;
using Tangle.Net.Api.Response.Utxo;
using Tangle.Net.Entity;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Entity.Message;
using Tangle.Net.Entity.Message.Payload;
using Tangle.Net.ProofOfWork;
using Tangle.Net.Utils;

namespace Tangle.Net.Api
{
  public class NodeRestClient : IClient
  {
    /// <summary>
    ///   Use default node (https://chrysalis-nodes.iota.org) and remote PoW
    /// </summary>
    public NodeRestClient()
      : this("https://chrysalis-nodes.iota.org")
    {
    }

    /// <summary>
    ///   Use specified PoWProvider and default node (https://chrysalis-nodes.iota.org)
    /// </summary>
    /// <param name="powProvider">
    ///   The PoW Provider
    /// </param>
    public NodeRestClient(IPowProvider powProvider) : this(powProvider, "https://chrysalis-nodes.iota.org")
    {
    }

    /// <summary>
    ///   Use the specified node with remote PoW
    /// </summary>
    /// <param name="nodeUrl">Node Url</param>
    public NodeRestClient(string nodeUrl)
    {
      NodeUrl = nodeUrl;
    }

    /// <summary>
    ///   Use specified PoWProvider and the specified node
    /// </summary>
    /// <param name="powProvider">The PoW Provider</param>
    /// <param name="nodeUrl">Node Url</param>
    public NodeRestClient(IPowProvider powProvider, string nodeUrl)
    {
      PowProvider = powProvider;
      NodeUrl = nodeUrl;
    }

    private IPowProvider PowProvider { get; }

    private string NodeUrl { get; }

    /// <inheritdoc />
    public async Task<Message<T>> GetMessageAsync<T>(string messageId)
      where T : Payload
    {
      var message = await ExecuteRequestAsync<Message<T>>($"{NodeUrl}/api/v1/messages/{messageId}");
      if (message == null)
      {
        throw new MessageNotFoundException(messageId);
      }

      // check if retrieved payload type is requested payload type
      var defaultPayload = (T)Activator.CreateInstance(typeof(T));
      if (message.Payload.Type != defaultPayload.Type)
      {
        throw new MessageTypeMismatchException(defaultPayload);
      }

      return message;
    }

    /// <inheritdoc />
    public async Task<MessageChildrenResponse> GetMessageChildrenAsync(string messageId)
    {
      return await ExecuteRequestAsync<MessageChildrenResponse>($"{NodeUrl}/api/v1/messages/{messageId}/children");
    }

    /// <inheritdoc />
    public async Task<MessageIdsByIndexResponse> GetMessageIdsByIndexAsync(string index)
    {
      return await ExecuteRequestAsync<MessageIdsByIndexResponse>($"{NodeUrl}/api/v1/messages?index={index.ToHex()}");
    }

    /// <inheritdoc />
    public async Task<MessageMetadata> GetMessageMetadataAsync(string messageId)
    {
      return await ExecuteRequestAsync<MessageMetadata>($"{NodeUrl}/api/v1/messages/{messageId}/metadata");
    }

    /// <inheritdoc />
    public async Task<MessageRawResponse> GetMessageRawAsync(string messageId)
    {
      using (var client = new HttpClient())
      {
        var responseStream = await client.GetAsync($"{NodeUrl}/api/v1/messages/{messageId}/raw");
        return new MessageRawResponse {MessageRaw = await responseStream.Content.ReadAsByteArrayAsync()};
      }
    }

    /// <inheritdoc />
    public async Task<bool> IsNodeHealthy()
    {
      using (var client = new HttpClient())
      {
        var response = await client.GetAsync($"{NodeUrl}/health");
        return response.IsSuccessStatusCode;
      }
    }

    /// <inheritdoc />
    public async Task<NodeInfo> GetNodeInfoAsync()
    {
      return await ExecuteRequestAsync<NodeInfo>($"{NodeUrl}/api/v1/info");
    }

    /// <inheritdoc />
    public async Task<TipsResponse> GetTipsAsync()
    {
      return await ExecuteRequestAsync<TipsResponse>($"{NodeUrl}/api/v1/tips");
    }

    /// <inheritdoc />
    public async Task<MessageIdResponse> SendDataAsync(string payload, string index)
    {
      var message = new Message<IndexationPayload>
      {
        Payload = new IndexationPayload { Index = index.ToHex(), Data = payload.ToHex() }
      };

      return await SendMessageAsync(message);
    }

    /// <inheritdoc />
    public async Task<MessageIdResponse> SendMessageAsync<T>(Message<T> message)
      where T : Payload
    {
      if (message.ParentMessageIds == null || message.ParentMessageIds.Count == 0)
      {
        var tips = await GetTipsAsync();
        message.ParentMessageIds = tips.TipMessageIds;
      }

      if (PowProvider != null)
      {
        var nodeInfo = await GetNodeInfoAsync();
        message.NetworkId = nodeInfo.CalculateMessageNetworkId();

        var serialize = message.Serialize();
        var nonce = PowProvider.DoPow(serialize, 4000);
        message.Nonce = Convert.ToString(nonce, 10);
      }

      using (var client = new HttpClient())
      {
        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
        var responseStream = await client.PostAsync($"{NodeUrl}/api/v1/messages", content);

        var response =
          JsonConvert.DeserializeObject<ClientResponse<MessageIdResponse>>(await responseStream.Content
            .ReadAsStringAsync());

        return response.Data;
      }
    }

    public async Task<OutputResponse> FindOutputByIdAsync(string outputId)
    {
      return await ExecuteRequestAsync<OutputResponse>($"{NodeUrl}/api/v1/outputs/{outputId}");
    }

    public async Task<Ed25519Address> GetAddressFromBech32Async(string addressBech32)
    {
      return await ExecuteRequestAsync<Ed25519Address>($"{NodeUrl}/api/v1/addresses/{addressBech32}");
    }

    public async Task<Ed25519Address> GetAddressFromEd25519Async(string addressEd25519)
    {
      return await ExecuteRequestAsync<Ed25519Address>($"{NodeUrl}/api/v1/addresses/ed25519/{addressEd25519}");
    }

    public async Task<OutputsResponse> GetOutputsFromBech32Async(string addressBech32, bool includeSpent = false, int type = 0)
    {
      return await ExecuteRequestAsync<OutputsResponse>($"{NodeUrl}/api/v1/addresses/{addressBech32}/outputs?include-spent={includeSpent}&type={type}");
    }

    public async Task<OutputsResponse> GetOutputsFromEd25519Async(string addressEd25519, bool includeSpent = false, int type = 0)
    {
      return await ExecuteRequestAsync<OutputsResponse>($"{NodeUrl}/api/v1/addresses/ed25519/{addressEd25519}/outputs?include-spent={includeSpent}&type={type}");
    }

    private static async Task<T> ExecuteRequestAsync<T>(string uri)
    {
      using (var client = new HttpClient())
      {
        var responseStream = await client.GetAsync(uri);
        var response =
          JsonConvert.DeserializeObject<ClientResponse<T>>(await responseStream.Content.ReadAsStringAsync());

        return response.Data;
      }
    }
  }
}