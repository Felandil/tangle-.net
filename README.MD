C# .NET 4.6 / .NET Standard Port of the IOTA lib (https://github.com/iotaledger/), build to fit into Clean Architecture (see below).

![cleanarch](http://i.imgur.com/WkBAATy.png)

More information:
http://blog.8thlight.com/uncle-bob/2012/08/13/the-clean-architecture.html

# Build Status 

<img src="https://felandilprojects.visualstudio.com/_apis/public/build/definitions/8112d254-cde8-46da-add3-75cab5fb802c/1/badge"/>

## About

This is the inofficial IOTA C# library. Currently "work in progress" to implement the changes introduced in IOTA 1.5 (Chrysalis)

## Contributing

If you want to contribute, please feel free to grab an issue or implement more features. Alternatively you can support the development by sending a few bucks to
EAGVYUFNOKIBECWJXQHFQQIQLAGSBYWXTLFLDZDGSLVDT9XMCKNOOUBDXCZHTJYYQWPKPIEUWVLXBBJJWKZBQRBNIX

## Supported REST Operations

**Proof of work** can currently only be done remotely. Signing transactions etc. is still work in progress.

| <div style="width:350px">Operation - Node</div>			 			   | Supported       | 
| ------------------------------------------------ | --------------- | 
| GET /health					 				   | Yes		     |
| GET /api/v1/info					 			   | Yes             |		
| GET /api/v1/tips					 			   | Yes			 |	

<br/>

| <div style="width:350px">Operation - Tangle</div>				 				   | Supported       | 
| ------------------------------------------------ | --------------- | 	
| GET /api/v1/tips					 			   | Yes			 |	

<br/>

| <div style="width:350px">Operation - Messages</div>				 				   | Supported       | 
| ------------------------------------------------ | --------------- | 	
| POST /api/v1/messages				 | Partially			   | 			
| GET /api/v1/messages						 | Yes             | 			
| GET /api/v1/messages/{messageId}			 | Partially	           |  
| GET /api/v1/messages/{messageId}/metadata						 | Yes	| 	
| GET /api/v1/messages/{messageId}/raw			 | Yes             | 		
| GET /api/v1/messages/{messageId}/children					 | Yes  |

<br/>

| <div style="width:350px">Operation - UTXO</div>				 				   | Supported       | 
| ------------------------------------------------ | --------------- | 	
| GET /api/v1/outputs/{outputId}				 			   | No			 |	
| GET /api/v1/addresses/{address}				 			   | No			 |	
| GET /api/v1/addresses/ed25519/{address}				 			   | No			 |	
| GET /api/v1/addresses/{address}/outputs				 			   | No			 |	
| GET /api/v1/addresses/ed25519/{address}/outputs				 			   | No			 |	

## Documentation

**OUDATED** https://tangle-net.readthedocs.io/en/latest/


## Encryption

**TBD**

## NuGet

**OUDATED** 

### Tangle.Net
.NET 4.6.1: https://www.nuget.org/packages/Tangle.Net <br>
.NET Standard 2.0: https://www.nuget.org/packages/Tangle.Net.Standard/

### Tangle.Net.Mam
.NET 4.6.1: https://www.nuget.org/packages/Tangle.Net.Mam/ <br>
.NET Standard 2.0: https://www.nuget.org/packages/Tangle.Net.Standard.Mam/

## Getting started

https://tangle-net.readthedocs.io/en/latest/getting_started.html

