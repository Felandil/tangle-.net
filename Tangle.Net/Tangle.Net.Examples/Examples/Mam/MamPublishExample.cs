namespace Tangle.Net.Examples.Examples.Mam
{
  using System;
  using System.Threading.Tasks;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Mam.Services;

  /// <summary>
  /// The mam publish example.
  /// </summary>
  public class MamPublishExample
  {
    // Javascript Code to receive MAM payload from the tangle (https://www.npmjs.com/package/mam.ts)

    // var mam = require('mam.ts');

    // var reader = new mam.MamReader(
    //         "https://pow2.iota.community:443", 
    //         "HYLIUVHXBDWDDDOOMNRGVBLOV9ALZJE9BQAGKGE9ZIABTKBSNYJLWWGSUODVG9UJMDFUENBNYIYYNOPFJ", 
    //         mam.MAM_MODE.PRIVATE,
    //         "KUPRZUQGLQNKYDOJVQKUQE9DWDLERJW9BVPDBGWFJUZ9HMLIOPJECHWISOCK9GOCFJJIABBXUADVUWJUD"
    //     );

    // var messages = reader.fetch();
    // messages.then(m => console.log(m));


    /// <summary>
    /// Initializes a new instance of the <see cref="MamPublishExample"/> class.
    /// </summary>
    public MamPublishExample()
    {
      // Create all needed objects to create a channel and subscription factory. 
      // This would be done via DI (e.g. Ninject) in production environment
      this.ChannelFactory = new MamChannelFactory(CurlMamFactory.Default, CurlMerkleTreeFactory.Default, Utils.Repository);
    }

    /// <summary>
    /// Gets the channel factory.
    /// </summary>
    private MamChannelFactory ChannelFactory { get; }

    /// <summary>
    /// The execute.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task Execute()
    {
      var seed = Seed.Random();
      var channelKey = Seed.Random();

      // To send a message we first create a channel via factory. Note that only one seed should be used per channel
      // If your channel key is not an ASCII string, create the channel like this this.ChannelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey.Value, true);
      var channel = this.ChannelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey.Value);

      // Creating a message is rather easy. The channel keeps track of everything internal
      var message = channel.CreateMessage(TryteString.FromAsciiString("This is my first message with MAM from CSharp!"));

      // Nevertheless we still need to publish the message after creating it
      await channel.PublishAsync(message);

      // Creating a message is rather easy. The channel keeps track of everything internal
      var message2 = channel.CreateMessage(TryteString.FromAsciiString("This is my second message with MAM from CSharp!"));

      // Nevertheless we still need to publish the message after creating it
      await channel.PublishAsync(message2);

      Console.WriteLine($"Seed: {seed.Value}");
      Console.WriteLine($"ChannelKey: {channelKey.Value}");
      Console.WriteLine($"Root: {message.Root.Value}");
    }
  }
}