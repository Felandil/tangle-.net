namespace Tangle.Net.Examples.Examples.Mam
{
  using System;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Services;

  /// <summary>
  /// The mam fetch example.
  /// </summary>
  public class MamFetchExample
  {
    // Javascript Code to send MAM payload to the tangle (https://www.npmjs.com/package/mam.ts)

    // var mam = require('mam.ts');

    // var writer = new mam.MamWriter(
    //   "https://pow2.iota.community:443",
    //   "AAARYFWTM9F9QHUGYCPP9FORGQNDVEUNYZKTMUAZSV9ESK9SNXORESIFIMHCJWXVGYNFVSGHKZOGVJVFZ",
    //   mam.MAM_MODE.RESTRICTED,
    //   "PCSRCZBAAABIYUTHVZAEEPDEQKGQOAFIB9SPBTXIUUMDSSKBGU9OVHZ9KWKNANJHOPCDIOBDEMFNMTEGB",
    //   mam.MAM_SECURITY.LEVEL_2
    // );

    // console.log(writer.getNextRoot());

    // var transaction = writer.createAndAttach("HI");
    // transaction.then(t => console.log(writer));

    // var transaction2 = writer.createAndAttach("HI 2");
    // transaction2.then(t => console.log(writer));

    /// <summary>
    /// Initializes a new instance of the <see cref="MamFetchExample"/> class.
    /// </summary>
    public MamFetchExample()
    {
      // Create all needed objects to create a channel and subscription factory. 
      // This would be done via DI (e.g. Ninject) in production environment
      this.SubscriptionFactory = new MamChannelSubscriptionFactory(Utils.Repository, CurlMamParser.Default, CurlMask.Default);
    }

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
      // Take root and channelKey (sideKey) from js code above. (to cleanly test things you can change the seed above and generate your own messages via js lib) 
      var root = new Hash("IKMRBBSHKBW9VZLYHRIASMFDQISJUZKAWPQSHRVPSWYPJRYXRJYMYPNCZBODMMYLAMDLEKMUISCRZC9FB");
      var channelKey = new TryteString("PCSRCZBAAABIYUTHVZAEEPDEQKGQOAFIB9SPBTXIUUMDSSKBGU9OVHZ9KWKNANJHOPCDIOBDEMFNMTEGB");

      // Messages published on a channel, can be retrieved by subscribing to the channel.
      // To find and decrypt messages published on a restricted stream, we need the root of the first message in our stream and the channelKey
      var channelSubscription = this.SubscriptionFactory.Create(root, Mode.Restricted, channelKey.Value);

      // Fetch the messages published on that stream
      var publishedMessages = await channelSubscription.FetchAsync();

      publishedMessages.ForEach(m => Console.WriteLine(m.Message.ToAsciiString()));
    }
  }
}