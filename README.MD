C# .NET 4.6 Port of the IOTA lib (https://github.com/iotaledger/), build to fit into Clean Architecture (see below).

![cleanarch](http://i.imgur.com/WkBAATy.png)

More information:
http://blog.8thlight.com/uncle-bob/2012/08/13/the-clean-architecture.html

# Build Status 

<img src="https://felandilprojects.visualstudio.com/_apis/public/build/definitions/8112d254-cde8-46da-add3-75cab5fb802c/1/badge"/>

## Motivation

As a C# developer it was very frustrating that the current IOTA C# lib is pretty much outdated. Therefore this project was born.
The project was set up a little different from most (except the python one) IOTA libs, since Clean Architecture enables you to follow the SOLID principles (https://en.wikipedia.org/wiki/SOLID_(object-oriented_design) pretty easy.

## Supported Operations

Next Up: 

	**MAM support**


| Operation (Core)				 | Supported       | Operation (Extended)			  | Supported       | 
| ------------------------------ | --------------- | -------------------------------- | --------------- | 
| getNeighbors					 | Yes		       | broadcastAndStoreTransaction     | Yes		        |
| getNodeInfo					 | Yes             | getAccountData				      | Yes		        |
| addNeighbors					 | Yes			   | getBundle					      | Yes		        |
| removeNeighbors				 | Yes			   | getInputs					      | Yes		        |
| getTips						 | Yes             | getLatestInclusion			      | Yes             |
| findTransactions				 | Yes	           | getNewAddresses				  | Yes		        |
| getTrytes						 | Yes			   | getTransfers					  | Yes			    |
| getInclusionStates			 | Yes             | prepareTransfers				  | Yes             |
| getBalances					 | Yes    		   | replayBundle					  | Yes			    |
| getTransactionsToApprove		 | Yes             | sendTransfer					  | Yes             |
| attachToTangle				 | Yes             | sendTrytes						  | Yes		        |
| interruptAttachingToTangle	 | Yes             |
| broadcastTransactions			 | Yes			   |
| storeTransactions				 | Yes			   |

In addition to all operations you can also do the Proof of Work locally via CPU. Please note that this is very time consuming and might not be feasible for large bundles.


## Contributing

If you want to contribute, please feel free to grab an issue. To get your code into the repository please fork it and create a pull request with your changes.

## Encryption

The library uses the C# Port of Bouncy Castle (http://www.bouncycastle.org/csharp/).

## NuGet

https://www.nuget.org/packages/Tangle.Net


## Note

The project is still in development. Therefore breaking changes may occur.
You can reach me via iota Discord (@Felandil) if you've got any questions.

## Donate

```javascript
YTXCUUWTXIXVRQIDSECVFRTKAFOEZITGDPLWYVUVFURMNVDPIRXEIQN9JHNFNVKVJMQVMA9GDZJROTSFZHIVJOVAEC
```

## Getting started

Have a look at the console application or start by instantiating a new repository!

```javascript
var repository = new RestIotaRepository(new RestClient("https://localhost:14265"));
var nodeInfo = repository.GetNodeInfo();
var neighbours = repository.GetNeighbors();
```

