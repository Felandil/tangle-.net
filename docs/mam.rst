MAM
============
Masked Authenticated Messaging (MAM) is a second layer data communication protocol which adds functionality to emit and access an encrypted data stream.
You can read more about it here: https://blog.iota.org/introducing-masked-authenticated-messaging-e55c1822d50e 

Channels
--------------
In the context of MAM, channels basically represent the sender. A channel manages the channels seed, tracks the state of a channel, creats and signs messages.
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

Serialization and State
--------------

Code example
--------------


.. code-block:: python

  // code here