using System;
using System.Collections.Generic;
using System.Text;
using Tangle.Net.Api;
using Tangle.Net.Api.HighLevel.Request;
using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Examples.Examples
{
  public class SendTransferExample
  {
    public async void Execute()
    {
      // sending data is a high level operation. construct high level client and input the node client
      // (node client can be constructed with a custom URI if needed - default is https://chrysalis-nodes.iota.org)
      var client = new HighLevelClient(new NodeRestClient());

      // the transfer request can be build in multiple ways
      // in any case it needs the seed to get the funds from
      // the receiver can either be input as a bech32 or ed25519 address
      // the bech32 type is the one displayed in Firefly
      // IMPORTANT: Due to dust protection, the network only allows transactions with a value of 1 Mi or more!
      var request = new SendTransferRequest(Ed25519Seed.FromMnemonic("some mnemonic"),
        "iota1qqh6pxkg4huzv506623l6lrt4daraktak6rvxwsvtewakj89vy7mj4enzsp", 1000000);

      //send request
      var response = await client.SendTransferAsync(request);

      // the response contains the id of the message | needed to later retrieve the message again
      Console.WriteLine(response.MessageId);
    }
  }
}
