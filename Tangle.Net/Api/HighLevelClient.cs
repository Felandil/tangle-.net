using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tangle.Net.Api.Exception;
using Tangle.Net.Api.HighLevel;
using Tangle.Net.Api.HighLevel.Request;
using Tangle.Net.Api.HighLevel.Response;
using Tangle.Net.Crypto.Bip44;
using Tangle.Net.Entity.Bech32;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Entity.Message;
using Tangle.Net.Entity.Message.Payload;
using Tangle.Net.Utils;

namespace Tangle.Net.Api
{
  public class HighLevelClient : IHighLevelClient
  {
    public HighLevelClient(IClient client)
    {
      Client = client;
    }

    private IClient Client { get; }

    public async Task<GetBalanceResponse> GetBalanceAsync(GetBalanceRequest request)
    {
      var unspentAddressResponse =
        await this.GetUnspentAddressesAsync(new GetUnspentAddressesRequest(request.Seed, request.AccountIndex,
          request.AddressOptions));

      return new GetBalanceResponse(unspentAddressResponse.UnspentAddresses.Sum(a => a.Balance));
    }

    public async Task<GetUnspentAddressesResponse> GetUnspentAddressesAsync(GetUnspentAddressesRequest request)
    {
      var nodeInfo = await this.Client.GetNodeInfoAsync();
      var state = new InMemoryBip44GeneratorState
      {
        AccountIndex = request.AccountIndex,
        AddressIndex = request.AddressOptions.StartIndex,
        IsInternal = false
      };

      var unspentAddresses = new List<Bech32Address>();
      var isFirst = true;
      var zeroCount = 0;

      var foundAll = false;
      do
      {
        var bip32Path = Bip44AddressGenerator.GenerateAddress(state, isFirst);
        isFirst = false;

        var addressSeed = request.Seed.GenerateSeedFromPath(bip32Path);
        var address = Ed25519Address.FromPublicKey(addressSeed.KeyPair.PublicKey);

        var addressWithBalance = await this.Client.GetAddressFromEd25519Async(address.Address);
        if (addressWithBalance.Balance == 0)
        {
          zeroCount++;
          if (zeroCount >= request.AddressOptions.ZeroCount)
          {
            foundAll = true;
          }
        }
        else
        {
          unspentAddresses.Add(Bech32Address.FromEd25519Address(addressWithBalance, bip32Path, nodeInfo.Bech32Hrp));

          if (unspentAddresses.Count == request.AddressOptions.RequiredCount)
          {
            foundAll = true;
          }
        }

      } while (!foundAll);

      return new GetUnspentAddressesResponse(unspentAddresses);
    }

    public async Task<MessageResponse<IndexationPayload>> SendDataAsync(SendDataRequest request)
    {
      request.Validate();

      var message = new Message<IndexationPayload>
      {
        Payload = new IndexationPayload { Index = request.IndexationKey.ToHex(), Data = request.Data.ToHex() }
      };

      var response = await this.Client.SendMessageAsync(message);

      return new MessageResponse<IndexationPayload>(message, response.MessageId);
    }

    public async Task<RetrieveDataResponse> RetrieveDataAsync(MessageRequest request)
    {
      var message = await this.Client.GetMessageAsync<IndexationPayload>(request.MessageId);
      return new RetrieveDataResponse(message.Payload.Index.HexToString(), message.Payload.Data.HexToString());
    }

    public async Task<MessageResponse<T>> ReattachAsync<T>(MessageRequest request) where T : Payload
    {
      var message = await this.Client.GetMessageAsync<T>(request.MessageId);
      await this.Client.SendMessageAsync(message);

      return new MessageResponse<T>(message, request.MessageId);
    }

    public async Task<MessageResponse<T>> PromoteAsync<T>(MessageRequest request) where T : Payload
    {
      var message = await this.Client.GetMessageAsync<T>(request.MessageId);
      var tips = await this.Client.GetTipsAsync();

      if (tips.TipMessageIds.Contains(request.MessageId))
      {
        tips.TipMessageIds.RemoveAt(tips.TipMessageIds.IndexOf(request.MessageId));
      }

      message.ParentMessageIds = tips.TipMessageIds;

      await this.Client.SendMessageAsync(message);

      return new MessageResponse<T>(message, request.MessageId);
    }

    public async Task<MessageResponse<T>> RetryAsync<T>(MessageRequest request) where T : Payload
    {
      var messageMetadata = await this.Client.GetMessageMetadataAsync(request.MessageId);

      if (messageMetadata.ShouldPromote)
      {
        return await this.PromoteAsync<T>(request);
      }

      if (messageMetadata.ShouldReattach)
      {
        return await this.ReattachAsync<T>(request);
      }

      throw new MessageNotRetriableException();
    }
  }
}