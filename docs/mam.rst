MAM
============
Masked Authenticated Messaging (MAM) is a second layer data communication protocol which adds functionality to emit and access an encrypted data stream.
You can read more about it here: https://blog.iota.org/introducing-masked-authenticated-messaging-e55c1822d50e 

Channels
--------------
In the context of MAM, channels represent the sender. A channel manages the its seed, tracks its state, creates and signs messages.
More about the statefullness of channels can be read below.

Creating a message through a channel and publishing it, can be done with a few lines of code. Channels should not be instantiated directly, rather they are the 
product of a channel factory.

.. code-block:: python
  
  var factory = new MamChannelFactory(CurlMamFactory.Default, CurlMerkleTreeFactory.Default, iotaRepository);
  var channel = factory.Create(Mode.Restricted, seed, SecurityLevel.Medium, "yourchannelkey");

  var message = channel.CreateMessage(TryteString.FromAsciiString("This is my first message with MAM from CSharp!"));
  await channel.PublishAsync(message);

Subscriptions
--------------
Subscriptions are used to listen to certain channels and retrieve messages from it. Listening do a channel can be done from any point (root) on, but not backwards.
For a subscription it is not needed to know the channels seed.

.. code-block:: python

  var factory = new MamChannelSubscriptionFactory(iotaRepository, CurlMamParser.Default, CurlMask.Default);

  var channelSubscription = factory.Create(new Hash("CHANNELROOT"), Mode.Restricted, "yourchannelkey");
  var publishedMessages = await channelSubscription.FetchAsync();

Serialization and State
--------------
Given the statefullness of channels and subscriptions, any application should persist the state of them. This is especially true for channels, where each message has its
own index. No second message should be published to that index (similar to the address reuse issie).

The state of a channel/subscription can be retrieved by simply calling the .ToJson method. This generates a JSON representation of the channel/subscription.
When recreating the channel/subscription, simply use the factories CreateFromJson method. 

.. code-block:: python

  var subscrptionJson = subscription.ToJson();
  var factory = new MamChannelSubscriptionFactory(iotaRepository, CurlMamParser.Default, CurlMask.Default);

  var channelSubscription = factory.CreateFromJson(subscrptionJson)

.. code-block:: python

  var channelJson = channel.ToJson();
  var factory = new MamChannelFactory(CurlMamFactory.Default, CurlMerkleTreeFactory.Default, iotaRepository);

  var channel = factory.CreateFromJson(channelJson)

Code examples
--------------

.. code-block:: python

  // code here