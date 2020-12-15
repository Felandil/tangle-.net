using System;

namespace Tangle.Net.Console
{
  using System.Threading.Tasks;

  using Newtonsoft.Json;

  using Tangle.Net.Api;

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

      Console.WriteLine("Getting Tips --------------------------");
      var tips = await client.GetTipsAsync();
      Console.WriteLine(JsonConvert.SerializeObject(tips, Formatting.Indented));
      Console.WriteLine("---------------------------------------");


      Console.WriteLine("Sending Message -----------------------");
      var messageId = await client.SendMessageAsync("Hello world!", "Tangle.Net");
      Console.WriteLine(JsonConvert.SerializeObject(messageId, Formatting.Indented));
      Console.WriteLine("---------------------------------------");

      Console.ReadKey();
    }
  }
}
