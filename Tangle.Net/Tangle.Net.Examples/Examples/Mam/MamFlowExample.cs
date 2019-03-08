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
  /// The mam example.
  /// </summary>
  public class MamFlowExample
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MamFlowExample"/> class.
    /// </summary>
    public MamFlowExample()
    {
      // Create all needed objects to create a channel and subscription factory. 
      // This would be done via DI (e.g. Ninject) in production environment
      this.ChannelFactory = new MamChannelFactory(CurlMamFactory.Default, CurlMerkleTreeFactory.Default, Utils.Repository);
      this.SubscriptionFactory = new MamChannelSubscriptionFactory(Utils.Repository, CurlMamParser.Default, CurlMask.Default);
    }

    /// <summary>
    /// Gets the channel factory.
    /// </summary>
    private MamChannelFactory ChannelFactory { get; }

    /// <summary>
    /// Gets the subscription factory.
    /// </summary>
    private MamChannelSubscriptionFactory SubscriptionFactory { get; }

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
      var channel = this.ChannelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey.Value);

      // Creating a message is rather easy. The channel keeps track of everything internal
      var message = channel.CreateMessage(TryteString.FromUtf8String("This is my first message with MAM from CSharp!"));

      // Nevertheless we still need to publish the message after creating it
      await channel.PublishAsync(message);

      // Messages published on a channel, can be retrieved by subscribing to the channel.
      // To find and decrypt messages published on a restricted stream, we need the root of the first message in our stream and the channelKey
      // If your channel key is not an ASCII string, create the subscription like this this.SubscriptionFactory.Create(message.Root, Mode.Restricted, channelKey.Value, true);
      var channelSubscription = this.SubscriptionFactory.Create(message.Root, Mode.Restricted, channelKey.Value);

      // Fetch the messages published on that stream
      var publishedMessages = await channelSubscription.FetchAsync();

      Console.WriteLine(publishedMessages[0].Message.ToUtf8String());
    }
  }
}