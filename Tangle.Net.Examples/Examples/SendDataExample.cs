using System;
using Tangle.Net.Api;
using Tangle.Net.Api.HighLevel.Request;

namespace Tangle.Net.Examples.Examples
{
  public class SendDataExample
  {
    public async void Execute()
    {
      // sending data is a high level operation. construct high level client and input the node client
      // (node client can be constructed with a custom URI if needed - default is https://chrysalis-nodes.iota.org)
      var client = new HighLevelClient(new NodeRestClient());

      // send data with indexation key (can be searched for e.g. with the IOTA explorer)
      var response = await client.SendDataAsync(new SendDataRequest("Tangle .Net",
        "Data to send. Can be anything string encoded. E.g. JSON or Base64"));


      // the response contains the id of the message | needed to later retrieve the message again
      Console.WriteLine(response.MessageId);
    }
  }
}
