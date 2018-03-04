# IOTA C# Library Documentation

This is a inoffical port of the IOTA Client library.

It implements all standard API calls, as described in the API documentation (https://iota.readme.io/v1.3.0/reference) and the extended methods for signing, sending and receiving bundles.

## Dependencies

The library currently uses RestSharp for all it's API calls and the official C# Bouncy Castle Port for encryption.

## Usage

The library is currently not available on nuGet. Therefore you will need to reference it as a submodule and build it together with your project.

## Getting Started

The most simple way to start using the library is by instantiating the repository via factory. Note that you could also create instances of the repository via any dependency injection framework.

```javascript
var factory = new RestIotaRepositoryFactory();
var repository = await factory.CreateAsync();
var nodeInfo = repository.GetNodeInfo();
```

Please note that the sandbox and HTTP Authentication against nodes is currently not supported.

## Entities

Since the library emphasizes an object orientated approach combined with Clean Architecture there are a few objects you have to be familiar with in order to work effectively. 

### TryteString

A TryteString is the ASCII representation of a sequence of trytes. Please note that only the letters A-Z and the number 9 are allowed. The following regular expression verifies if a given string is a TryteString or not.

```javascript
^[9A-Z]*$
```

As the TryteString is only the basic class you will propably stumble accross many references for its subclasses:

+ Address
+ Checksum
+ Digest
+ Fragment
+ Hash
+ Seed
+ Tag
+ TransactionTrytes

### Transaction

As transactions are generated within a bundle or are received via API call you won't need to create them manually (but you can if you want to).

- TBD

### Bundle

A bundle represents a set of transactions that have been or should be attached to the tangle. The entity itself is able to finalize and sign bundles in order to send them to the tangle.

Note that you technically can create all transactions on a bundle manually, but that is not neccessary and more error prone.

- TBD

## Addresses

Addresses in IOTA are derived deterministically from your seed. That means that you can access your funds everywhere as long as you know your seed.

Please note that anyone with access to your seed, also has access to your funds. More on security here: https://blog.iota.org/the-secret-to-security-is-secrecy-d32b5b7f25ef

 **Never ever use an online seed generator**

 ### Generating an address

 ```javascript
var seed = new Seed("SOMESEEDHERE")
var addressGenerator = new AddressGenerator(seed, SecurityLevel.Medium);
var address = addressGenerator.GetAddress(0);
var addresses = addressGenerator.GetAddresses(0, 10);
```

When you generate an address you will need to provide an index. Since addresses are generated deterministically the first address index will always result in the same address. For generating more than one address use the GetAddresses method, provided with a count.

### Security Levels

The higher the security level the longer the private key for your address (used to sign spending of funds) will be. Even though address generation is deterministically a different security level will result in a different address even if the index is the same.

There currently are three security levels (range 1-3). You can either use the numbers directly of access them via the SecurityLevel class.


