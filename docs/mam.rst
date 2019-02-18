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

  var channelSubscription = this.SubscriptionFactory.Create(new Hash("CHANNELROOT"), Mode.Restricted, "yourchannelkey");
  var publishedMessages = await channelSubscription.FetchAsync();

Serialization and State
--------------

Code example
--------------


.. code-block:: python

  // code here