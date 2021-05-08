using System;

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

      Console.WriteLine("Getting Node Info-----------------------");
      var nodeinfo = await client.GetNodeInfoAsync();
      Console.WriteLine(JsonConvert.SerializeObject(nodeinfo, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Getting Tips --------------------------");
      var tips = await client.GetTipsAsync();
      Console.WriteLine(JsonConvert.SerializeObject(tips, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

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
      var milestone = await client.GetMessageAsync<MilestonePayload>("fb97ba6a265db83c927aceaf2dce9815730306132bd96040fe8793e76e23aac3");
      Console.WriteLine(JsonConvert.SerializeObject(milestone, Formatting.Indented));
      Console.WriteLine($"Issued at: {milestone.Payload.Timestamp.UnixTimestampToDateTime():F}");
      Console.WriteLine("---------------------------------------");

      Console.WriteLine("Reading Milestone Raw ---------------------");
      var rawMilestone = await client.GetMessageRawAsync("fb97ba6a265db83c927aceaf2dce9815730306132bd96040fe8793e76e23aac3");
      var parsedRawMilestone = Message<MilestonePayload>.Deserialize(rawMilestone.MessageRaw);
      Console.WriteLine(JsonConvert.SerializeObject(parsedRawMilestone, Formatting.Indented));
      Console.WriteLine($"Issued at: {milestone.Payload.Timestamp.UnixTimestampToDateTime():F}");
      Console.WriteLine("---------------------------------------");
      Console.WriteLine(rawMilestone.MessageRaw.ToHex());
      Console.WriteLine("---------------------------------------");

      Console.ReadKey();
    }
  }
}
