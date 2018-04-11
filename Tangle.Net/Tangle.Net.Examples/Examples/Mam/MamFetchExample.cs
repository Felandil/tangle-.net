namespace Tangle.Net.Examples.Examples.Mam
{
  using System;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Services;
  using Tangle.Net.Repository.Factory;

  /// <summary>
  /// The mam fetch example.
  /// </summary>
  public class MamFetchExample
  {
    // Javascript Code to send MAM payload to the tangle (https://github.com/iotaledger/mam.client.js)

    // var Mam = require('../lib/mam.node.js')
    // var IOTA = require('iota.lib.js')

    // var iota = new IOTA({ provider: `https://field.carriota.com:443` })

    // let seed = "OWKIIGMOPZBNDBHEJCDXWIMKLCUPBGGUMQZ9YYWLZCUNIJZJLWSX9NYNKQEFPLKWWKJFIRRIODWKX9LYF"
    // let security = 2; // Medium
    // let sideKey = "FA9KZAARRNBVZTZUTBGSTXKPHHAZOZRX9FNSIERZKQQBPDGKMAYOXUAPTMHHCJ9CTJPLIQKZ9GKYQTEHI"
    // let mode = 'restricted'

    //// Initialise MAM State
    // var mamState = Mam.init(iota, seed, security)

    //// Set channel mode
    // mamState = Mam.changeMode(mamState, mode, sideKey)

    // var message = Mam.create(mamState, "CCWCXCGDEAXCGDEAADMDEAUCXCFDGDHDEAADTCGDGDPCVCTCEAKDXCHDWCEAWBKBWBEAUCFDCDADEAMBBCWCPCFDDDFA") // Tryte encoded message data to send

    // console.log('Root: ', message.root)
    // console.log('Address: ', message.address)

    // Mam.attach(message.payload, message.address)

    /// <summary>
    /// Initializes a new instance of the <see cref="MamFetchExample"/> class.
    /// </summary>
    public MamFetchExample()
    {
      // Create all needed objects to create a channel and subscription factory. 
      // This would be done via DI (e.g. Ninject) in production environment
      var repository = new RestIotaRepositoryFactory().CreateAsync().Result;

      this.SubscriptionFactory = new MamChannelSubscriptionFactory(repository, CurlMamParser.Default, CurlMask.Default);
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
      var root = new Hash("RWSAMBJMEENBNBUBOD9SFZARWXNXOETJQOYZNJXAHETBUJWQPVPALBOVYBRNUGOOIVZQODPQGDZRDEQWR");
      var channelKey = new TryteString("FA9KZAARRNBVZTZUTBGSTXKPHHAZOZRX9FNSIERZKQQBPDGKMAYOXUAPTMHHCJ9CTJPLIQKZ9GKYQTEHI");

      // Messages published on a channel, can be retrieved by subscribing to the channel.
      // To find and decrypt messages published on a restricted stream, we need the root of the first message in our stream and the channelKey
      var channelSubscription = this.SubscriptionFactory.Create(root, Mode.Restricted, channelKey);

      // Fetch the messages published on that stream
      var publishedMessages = await channelSubscription.FetchAsync();

      Console.WriteLine(publishedMessages[0].Message.ToUtf8String());
    }
  }
}