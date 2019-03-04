Working with the Core
============

Dependencies
-------------

The library currently uses RestSharp for all it's API calls and the official C# Bouncy Castle Port for encryption.

Entities
-------------

Since the library emphasizes an object orientated approach combined with Clean Architecture there are a few objects you have to be familiar with in order to work effectively. 

TryteString
-------------

A TryteString is the ASCII representation of a sequence of trytes. Please note that only the letters A-Z and the number 9 are allowed. The following regular expression verifies if a given string is a TryteString or not.

.. code-block:: bash

    ^[9A-Z]*$

As the TryteString is only the basic class you will propably stumble accross many references for its subclasses:

+ Address

  If you want to sign an input, you should generate the address with the AddressGenerator. That way the private key will be generated aswell, which is needed to sign the transaction.
  For any other cases you can simply instantiate the address with its trytes.

+ Checksum 

  Checksum for a given address

+ Digest 

  Digest to a generated private key for an address. Will be generated along with an address and its private key

+ Fragment 

  Payload part of a transaction. Can either contain parts of the signature or carry data

+ Hash 

  81 trytes long hash to a bundle, transaction or similar

+ Seed

  81 trytes long "master key". Used to derive addresses and private keys

+ Tag 

  27 trytes long part of a transaction. Can be set to any value 
  
+ TransactionTrytes 

  2673 trytes long. Represents a transaction as trytes

Bundle
-------------

A bundle represents a set of transactions that have been or should be attached to the tangle. The entity itself is able to finalize and sign bundles in order to send them to the tangle.

Note that you technically can create all transactions on a bundle manually, but that is not neccessary and more error prone.

The simpliest way to send a bundle to the tangle is to use a bundle without value

.. code-block:: bash

    var bundle = new Bundle();
    bundle.AddTransfer(new Transfer
                        {
                            Address = new Address(Hash.Empty.Value),
                            Tag = new Tag("TANGLENET"),
                            Timestamp = Timestamp.UnixSecondsTimestamp,
                            Message = TryteString.FromUtf8String("Hello #1!")
                        });

    bundle.AddTransfer(new Transfer
                        {
                            Address = new Address(Hash.Empty.Value),
                            Tag = new Tag("TANGLENET"),
                            Timestamp = Timestamp.UnixSecondsTimestamp,
                            Message = TryteString.FromUtf8String("Hello #2!")
                        });

    bundle.Finalize();
    bundle.Sign();

    // see Getting Started to see how a repository is created
    repository.SendTrytes(bundle.Transactions);

Transaction
-------------

As transactions are generated within a bundle or are received via API call you won't need to create them manually (but you can if you want to).

Addresses
-------------

Addresses in IOTA are derived deterministically from your seed. That means that you can access your funds everywhere as long as you know your seed.

Please note that anyone with access to your seed, also has access to your funds. More on security here: https://blog.iota.org/the-secret-to-security-is-secrecy-d32b5b7f25ef

 **Never ever use an online seed generator**

Generating an address
-------------

.. code-block:: bash

    var seed = new Seed("SOMESEEDHERE")
    var addressGenerator = new AddressGenerator(seed, SecurityLevel.Medium);
    var address = addressGenerator.GetAddress(0);
    var addresses = addressGenerator.GetAddresses(0, 10);

When you generate an address you will need to provide an index. Since addresses are generated deterministically the first address index will always result in the same address. For generating more than one address use the GetAddresses method, provided with a count.

Security Levels
-------------

The higher the security level the longer the private key for your address (used to sign spending of funds) will be. Even though address generation is deterministically a different security level will result in a different address even if the index is the same.

There currently are three security levels (range 1-3). You can either use the numbers directly of access them via the SecurityLevel class.

Clients
-------------

For most use cases it should be fine to instantiate the repository as displayed in Getting Started. 

Anyway it sometimes may be useful to have some kind of fallback mechanism in place to handle unresponsive or out of sync nodes. To handle this you can use the fallback client, that will handle node errors internally.

.. code-block:: bash

    var repository = new RestIotaRepository(
    new FallbackIotaClient(
        new List<string>
        {
            "https://invalid.node.com:443",
            "https://peanut.iotasalad.org:14265",
            "http://node04.iotatoken.nl:14265",
            "http://node05.iotatoken.nl:16265",
        },
        5000),
    new PoWSrvService());

Besides the timeout for calls against the node you can specify an error threshold along with a reset timeout. The client behaves similar to a [circuit breaker](https://martinfowler.com/bliki/CircuitBreaker.html).

If you are using the fallback client, make sure that the calls you run against the nodes are correctly formed, since there is no distinction between exceptions.