using System;
using System.Collections.Generic;
using Tangle.Net.Api.HighLevel;
using Tangle.Net.Api.HighLevel.Request;
using Tangle.Net.Crypto;
using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Console
{
  using System.Threading.Tasks;

  using Newtonsoft.Json;

  using Tangle.Net.Api;
  using Tangle.Net.Entity.Message;
  using Tangle.Net.Entity.Message.Payload;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Utils;

  using Console = System.Console;

  class Program
  {
    static void Main(string[] args)
    {
      MainAsync(args).GetAwaiter().GetResult();
    }

    static async Task MainAsync(string[] args)
    {
      var client = new NodeRestClient();

      //await NodeOperations(client);
      //await TipsOperations(client);
      await MessageOperations(client);
      //await UtxoOperations(client);
      //await HighLevelOperations(client);

      Console.ReadKey();
    }

    private static async Task HighLevelOperations(NodeRestClient client)
    {
      var highLevelClient = new HighLevelClient(client);

      Console.WriteLine("Sending Transfer -----------------------");
      var seed = Ed25519Seed.FromMnemonic("");
      var sendTransferResponse = await highLevelClient.SendTransferAsync(new SendTransferRequest(seed, "iota1qqh6pxkg4huzv506623l6lrt4daraktak6rvxwsvtewakj89vy7mj4enzsp", 1000000, "Tangle .Net", "Test"));
      Console.WriteLine(JsonConvert.SerializeObject(sendTransferResponse, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Sending High Level Data -----------------------");
      var sendDataResponse = await highLevelClient.SendDataAsync(new SendDataRequest("Tangle .Net", "High Level Data"));
      Console.WriteLine(JsonConvert.SerializeObject(sendDataResponse, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Retrieving High Level Data -----------------------");
      var retrieveDataResponse = await highLevelClient.RetrieveDataAsync(new MessageRequest(sendDataResponse.MessageId));
      Console.WriteLine(JsonConvert.SerializeObject(retrieveDataResponse, Formatting.Indented));
      Console.WriteLine("---------------------------------------");
    }

    private static async Task UtxoOperations(NodeRestClient client)
    {
      Console.WriteLine("Getting Output Information -----------------------");
      //var outputResponse =
      //  await client.FindOutputByIdAsync("someoutputid");
      //Console.WriteLine(JsonConvert.SerializeObject(outputResponse, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Getting Address Information -----------------------");
      var addressFromBech32 =
        await client.GetAddressFromBech32Async("iota1qp7k8gdfhqqz48csulkla6m67a9chjlsqq5ujysteqqqpcpqntugu4smx92");
      Console.WriteLine(JsonConvert.SerializeObject(addressFromBech32, Formatting.Indented));
      Console.WriteLine("---------------------------------------");


      Console.WriteLine("Getting Address Outputs -----------------------");
      var addressOutputs =
        await client.GetOutputsFromBech32Async("someaddresswithoutputs", true);
      Console.WriteLine(JsonConvert.SerializeObject(addressOutputs, Formatting.Indented));
      Console.WriteLine("---------------------------------------");
      
    }

    private static async Task MessageOperations(NodeRestClient client)
    {
      Console.WriteLine("Sending Message -----------------------");
      var sendResponse = await client.SendDataAsync("Hello world!", "Tangle.Net");
      Console.WriteLine(JsonConvert.SerializeObject(sendResponse, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Reading Message -----------------------");
      var message = await client.GetMessageAsync<IndexationPayload>(sendResponse.MessageId);
      Console.WriteLine(JsonConvert.SerializeObject(message, Formatting.Indented));
      Console.WriteLine(message.Payload.Data.HexToString());
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Reading Message Metadata---------------");
      var messageMetadata = await client.GetMessageMetadataAsync(sendResponse.MessageId);
      Console.WriteLine(JsonConvert.SerializeObject(messageMetadata, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Reading Message Raw--------------------");
      var messageRaw = await client.GetMessageRawAsync(sendResponse.MessageId);
      Console.WriteLine(messageRaw.MessageRaw.ToHex());
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Converting Message Raw--------------------");
      var messageRawParsed = Message<IndexationPayload>.Deserialize(messageRaw.MessageRaw);
      Console.WriteLine(JsonConvert.SerializeObject(messageRawParsed, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Reading Message Ids -------------------");
      var messageIds = await client.GetMessageIdsByIndexAsync("Tangle.Net");
      Console.WriteLine(JsonConvert.SerializeObject(messageIds, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Reading Milestone ---------------------");
      var milestone =
        await client.GetMessageAsync<MilestonePayload>("fb97ba6a265db83c927aceaf2dce9815730306132bd96040fe8793e76e23aac3");
      Console.WriteLine(JsonConvert.SerializeObject(milestone, Formatting.Indented));
      Console.WriteLine($"Issued at: {milestone.Payload.Timestamp.UnixTimestampToDateTime():F}");
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Reading Milestone Raw ---------------------");
      var rawMilestone =
        await client.GetMessageRawAsync("fb97ba6a265db83c927aceaf2dce9815730306132bd96040fe8793e76e23aac3");
      var parsedRawMilestone = Message<MilestonePayload>.Deserialize(rawMilestone.MessageRaw);
      Console.WriteLine(JsonConvert.SerializeObject(parsedRawMilestone, Formatting.Indented));
      Console.WriteLine($"Issued at: {milestone.Payload.Timestamp.UnixTimestampToDateTime():F}");
      Console.WriteLine("---------------------------------------");
      Console.WriteLine(rawMilestone.MessageRaw.ToHex());
      Console.WriteLine("---------------------------------------");
    }

    private static async Task TipsOperations(NodeRestClient client)
    {
      Console.WriteLine("Getting Tips --------------------------");
      var tips = await client.GetTipsAsync();
      Console.WriteLine(JsonConvert.SerializeObject(tips, Formatting.Indented));
      Console.WriteLine("---------------------------------------");
    }

    private static async Task NodeOperations(NodeRestClient client)
    {
      Console.WriteLine("Getting Node Info-----------------------");
      var nodeinfo = await client.GetNodeInfoAsync();
      Console.WriteLine(JsonConvert.SerializeObject(nodeinfo, Formatting.Indented));
      Console.WriteLine("---------------------------------------");
    }
  }
}
