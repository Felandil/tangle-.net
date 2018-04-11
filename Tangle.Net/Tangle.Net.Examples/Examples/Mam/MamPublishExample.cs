namespace Tangle.Net.Examples.Examples.Mam
{
  using System;
  using System.Threading.Tasks;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Mam.Services;
  using Tangle.Net.Repository.Factory;

  /// <summary>
  /// The mam publish example.
  /// </summary>
  public class MamPublishExample
  {
    // Javascript Code to receive MAM payload from the tangle (https://github.com/iotaledger/mam.client.js)

    // var Mam = require('../lib/mam.node.js')
    // var IOTA = require('iota.lib.js')

    // var iota = new IOTA({ provider: `https://field.carriota.com:443` })

    //// Init State
    // let root = "ROOTHERE" // Enter the root output from the C# example
    // let sideKey = "SIDEKEYHERE" // Enter the channel Key from the C# example
    // let mode = 'restricted'

    //// Initialise MAM State
    // var mamState = Mam.init(iota)

    // var resp = Mam.fetch(root, mode, sideKey, console.log)
    // console.log(resp)


    /// <summary>
    /// Initializes a new instance of the <see cref="MamPublishExample"/> class.
    /// </summary>
    public MamPublishExample()
    {
      // Create all needed objects to create a channel and subscription factory. 
      // This would be done via DI (e.g. Ninject) in production environment
      var repository = new RestIotaRepositoryFactory().CreateAsync().Result;

      this.ChannelFactory = new MamChannelFactory(CurlMamFactory.Default, CurlMerkleTreeFactory.Default, repository);
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
      var channel = this.ChannelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey);

      // Creating a message is rather easy. The channel keeps track of everything internal
      var message = channel.CreateMessage(TryteString.FromUtf8String("This is my first message with MAM from CSharp!"));

      // Nevertheless we still need to publish the message after creating it
      await channel.PublishAsync(message);

      Console.WriteLine($"Seed: {seed.Value}");
      Console.WriteLine($"ChannelKey: {channelKey.Value}");
      Console.WriteLine($"Root: {message.Root.Value}");
    }
  }
}