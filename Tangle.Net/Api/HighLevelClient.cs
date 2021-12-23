﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tangle.Net.Api.Exception;
using Tangle.Net.Api.HighLevel;
using Tangle.Net.Api.HighLevel.Request;
using Tangle.Net.Api.HighLevel.Response;
using Tangle.Net.Crypto;
using Tangle.Net.Crypto.Bip44;
using Tangle.Net.Entity;
using Tangle.Net.Entity.Bech32;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Entity.Message;
using Tangle.Net.Entity.Message.Payload;
using Tangle.Net.Entity.Message.Payload.Transaction;
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

    public async Task<MessageResponse<TransactionPayload>> SendTransferAsync(SendTransferRequest request)
    {
      var inputs = await this.CalculateInputsAsync(request.Seed, request.Outputs, request.AddressOptions.ZeroCount,
        request.AccountIndex, request.AddressOptions.StartIndex);

      var payload = this.BuildTransactionPayload(inputs, request.Outputs, request.IndexationKey, request.Data);

      var message = new Message<TransactionPayload> {Payload = payload};
      var response = await this.Client.SendMessageAsync(message);

      return new MessageResponse<TransactionPayload>(message, response.MessageId);
    }

    private TransactionPayload BuildTransactionPayload(Dictionary<UTXOInput, Ed25519Seed> inputs, List<TransferOutput> outputs, string indexationKey, string data)
    {
      var mappedOutputs = outputs.Select(output => new SigLockedOutput(output.Receiver.DustAllowed
            ? SigLockedOutput.SigLockedDustAllowanceOutput
            : SigLockedOutput.SigLockedSingleOutputType)
          {Address = output.Receiver, Amount = output.Amount})
        .ToDictionary(mappedOutput => mappedOutput, mappedOutput => mappedOutput.Serialize().ToHex())
        .OrderBy(m => m.Value).ToDictionary(m => m.Key, m => m.Value);

      var mappedInputs = inputs.Select(input =>
          new Tuple<UTXOInput, Ed25519Seed, string>(input.Key, input.Value, input.Key.Serialize().ToHex()))
        .OrderBy(i => i.Item3).ToList();

      var transactionEssence = new TransactionEssence
      {
        Inputs = mappedInputs.Select(i => i.Item1).ToList(),
        Outputs = mappedOutputs.Select(o => o.Key).ToList(),
        Payload = !string.IsNullOrEmpty(indexationKey)
          ? new IndexationPayload {Index = indexationKey.ToHex(), Data = data.ToHex()}
          : null
      };
      var transactionEssenceHash = transactionEssence.CalculateHash();

      var unlockBlocks = new List<UnlockBlock>();
      var addressToUnlockBlocks = new Dictionary<string, short>();
      foreach (var (_, seed, _) in mappedInputs)
      {
        var addressPublicKey = seed.KeyPair.PublicKey.ToHex();
        if (addressToUnlockBlocks.Any(t => t.Key == addressPublicKey))
        {
          unlockBlocks.Add(new ReferenceUnlockBlock
            {Reference = addressToUnlockBlocks.First(t => t.Key == addressPublicKey).Value});
        }
        else
        {
          unlockBlocks.Add(new SignatureUnlockBlock
          {
            Signature = Ed25519.Sign(transactionEssenceHash, seed)
          });

          addressToUnlockBlocks.Add(addressPublicKey, (short) (unlockBlocks.Count - 1));
        }
      }

      return new TransactionPayload
      {
        Essence = transactionEssence,
        UnlockBlocks = unlockBlocks
      };
    }

    private async Task<Dictionary<UTXOInput, Ed25519Seed>> CalculateInputsAsync(Ed25519Seed seed, List<TransferOutput> outputs, int zeroCount, int accountIndex, int startIndex)
    {
      var state = new InMemoryBip44GeneratorState
      {
        AccountIndex = accountIndex,
        AddressIndex = startIndex,
        IsInternal = false
      };

      var inputs = new Dictionary<UTXOInput, Ed25519Seed>();
      var consumedBalance = 0L;
      var isFirst = true;
      var zeroBalanceCount = 0;
      var requiredBalance = outputs.Sum(o => o.Amount);

      var foundAll = false;
      do
      {
        var bip32Path = Bip44AddressGenerator.GenerateAddress(state, isFirst);
        isFirst = false;

        var addressSeed = seed.GenerateSeedFromPath(bip32Path);
        var address = Ed25519Address.FromPublicKey(addressSeed.KeyPair.PublicKey);

        var addressOutputs = await this.Client.GetOutputsFromEd25519Async(address.Address);
        if (addressOutputs.Count == 0)
        {
          zeroBalanceCount++;
          if (zeroBalanceCount >= zeroCount)
          {
            foundAll = true;
          }
        }
        else
        {
          foreach (var outputId in addressOutputs.OutputIds)
          {
            var addressOutput = await this.Client.FindOutputByIdAsync(outputId);

            if (!addressOutput.IsSpent &&
                consumedBalance < requiredBalance)
            {
              if (addressOutput.Output.Amount == 0)
              {
                zeroBalanceCount++;
                if (zeroBalanceCount >= zeroCount)
                {
                  foundAll = true;
                }
              }
              else
              {
                consumedBalance += addressOutput.Output.Amount;

                inputs.Add(
                  new UTXOInput
                    {TransactionId = addressOutput.TransactionId, TransactionOutputIndex = addressOutput.OutputIndex},
                  addressSeed);

                if (consumedBalance < requiredBalance)
                {
                  continue;
                }

                if (consumedBalance - requiredBalance > 0)
                {
                  outputs.Add(new TransferOutput(addressOutput.Output.Address, consumedBalance - requiredBalance));
                }

                foundAll = true;
              }
            }
          }
        }

      } while (!foundAll);

      return inputs;
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

      throw new MessageNotRetryableException();
    }
  }
}