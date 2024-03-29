C# .NET Standard Port of the IOTA lib (https://github.com/iotaledger/), build to fit into Clean Architecture (see below).

![cleanarch](http://i.imgur.com/WkBAATy.png)

More information:
http://blog.8thlight.com/uncle-bob/2012/08/13/the-clean-architecture.html

# Build Status 

<img src="https://felandilprojects.visualstudio.com/_apis/public/build/definitions/8112d254-cde8-46da-add3-75cab5fb802c/1/badge"/>

## About

This is the inofficial IOTA C# library. Currently "work in progress" to implement the changes introduced with IOTA 1.5 (Chrysalis). For any questions on how to get started (and since the documentation is very much outdated) feel free to contact me via discord (Felandil#6471)

## Contributing

If you want to contribute, please feel free to grab an issue or implement more features. Alternatively you can support the development by donating to
```
iota1qqh6pxkg4huzv506623l6lrt4daraktak6rvxwsvtewakj89vy7mj4enzsp
```
## Supported REST Operations

**Proof of work** can currently only be done remotely. Signing transactions etc. is still work in progress.

| Operation - Node<img width=300/>		 			   | Supported       | 
| ------------------------------------------------ | --------------- | 
| GET /health					 				   | Yes		     |
| GET /api/v1/info					 			   | Yes             |		

<br/>

| Operation - Tips<img width=300/>					 				   | Supported       | 
| ------------------------------------------------ | --------------- | 	
| GET /api/v1/tips					 			   | Yes			 |	

<br/>

| Operation - Messages<img width=270/>			 				   | Supported       | 
| ------------------------------------------------ | --------------- | 	
| POST /api/v1/messages				 | Partially			   | 			
| GET /api/v1/messages						 | Yes             | 			
| GET /api/v1/messages/{messageId}			 | Partially	           |  
| GET /api/v1/messages/{messageId}/metadata						 | Yes	| 	
| GET /api/v1/messages/{messageId}/raw			 | Yes             | 		
| GET /api/v1/messages/{messageId}/children					 | Yes  |

<br/>

| Operation - UTXO<img width=295/>				 				   | Supported       | 
| ------------------------------------------------ | --------------- | 	
| GET /api/v1/outputs/{outputId}				 			   | Yes			 |	
| GET /api/v1/addresses/{address}				 			   | Yes			 |	
| GET /api/v1/addresses/ed25519/{address}				 			   | Yes			 |	
| GET /api/v1/addresses/{address}/outputs				 			   | Yes			 |	
| GET /api/v1/addresses/ed25519/{address}/outputs				 			   | Yes			 |	

<br/>

## Supported High Level Operations

| Operation <img width=300/>		 			   | Supported       | 
| ------------------------------------------------ | --------------- | 
| GetBalance					 				   | Yes 	     |
| GetUnspentAddresses					 			   | Yes             |	
| Send					 				   | Yes (untested)	     |
| Retry					 				   | Yes	     |
| Promote					 				   | Yes	     |
| Reattach					 				   | Yes		     |
| SendData					 				   | Yes     |	
| RetrieveData					 				   | Yes	     |


## NuGet

.NET Standard 2.0: https://www.nuget.org/packages/Tangle.Net.Chrysalis


