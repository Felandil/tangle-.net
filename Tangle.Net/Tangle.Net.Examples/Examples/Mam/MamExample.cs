namespace Tangle.Net.Examples.Examples.Mam
{
  using System;
  using System.Threading.Tasks;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Mam;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Repository.Factory;

  /// <summary>
  /// The mam example.
  /// </summary>
  public class MamExample
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MamExample"/> class.
    /// </summary>
    public MamExample()
    {
      // Create all needed objects to create a channel and subscription factory. 
      // This would be done via DI (e.g. Ninject) in production environment
      var repository = new RestIotaRepositoryFactory().CreateAsync().Result;
      var curl = new Curl();
      var mask = new CurlMask();
      var treeFactory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(curl), new CurlMerkleLeafFactory(new AddressGenerator()));
      var mamFactory = new CurlMamFactory(curl, mask);
      var mamParser = new CurlMamParser(mask, treeFactory, curl);

      this.ChannelFactory = new MamChannelFactory(mamFactory, treeFactory, repository);
      this.SubscriptionFactory = new MamChannelSubscriptionFactory(repository, mamParser);
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
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="channelKey">
    /// The channel key.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task Execute(Seed seed, TryteString channelKey)
    {
      // To send a message we first create a channel via factory. Note that only one seed should be used per channel
      var channel = this.ChannelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey);

      // Creating a message is rather easy. The channel keeps track of everything internal
      var message = channel.CreateMessage(TryteString.FromUtf8String("This is my first message with MAM from CSharp!"));

      // Nevertheless we still need to publish the message after creating it
      await channel.PublishAsync(message);

      // Messages published on a channel, can be retrieved by subscribing to the channel.
      // To find and decrypt messages published on a restricted stream, we need the root of the first message in our stream and the channelKey
      var channelSubscription = this.SubscriptionFactory.Create(message.Root, Mode.Restricted, channelKey, SecurityLevel.Medium);

      // Fetch the messages published on that stream
      var publishedMessages = await channelSubscription.FetchAsync();

      Console.WriteLine(publishedMessages[0].Message.ToUtf8String());
    }
  }
}