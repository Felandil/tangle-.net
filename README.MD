C# .NET 4.6 Port of the IOTA lib (https://github.com/iotaledger/), build to fit into Clean Architecture (see below).

![cleanarch](http://i.imgur.com/WkBAATy.png)

More information:
http://blog.8thlight.com/uncle-bob/2012/08/13/the-clean-architecture.html

## Motivation

As a C# developer it was very frustrating that the current IOTA C# lib is pretty much outdated. Therefore this project was born.
The project was set up a little different from most (except the python one) IOTA libs, since Clean Architecture enables you to follow the SOLID principles (https://en.wikipedia.org/wiki/SOLID_(object-oriented_design) pretty easy.

## Development Cycle

If you want to contribute at the moment, please feel free to bombard me with testcases via Slack (@felandil). 
As soon as the library reaches a stable status (or one which I am happy with) you can also contribute Code if you want to. 

Current development focuses on getting things from the python lib (I found it to be much cleaner than the JAVA one) into this.

Cycle is
1) Get Tests in as good as possible
2) Make Tests pass
3) Refactor until happy

As a result it can happen that some parts of the library look like spaghetti, please be forgiving :)

## Encryption

The library uses the C# Port of Bouncy Castle (http://www.bouncycastle.org/csharp/).

## NuGet

Even though I like package managers, I decided to not make the project available via NuGet, since sensitive data (your seed) plays a crucial role.

Here is why:
https://hackernoon.com/im-harvesting-credit-card-numbers-and-passwords-from-your-site-here-s-how-9a8cb347c5b5

If you want to use the package for your own development I suggest to to check it out as a submodule and compile it within your project.

## Note

The project is still in development. Therefore breaking changes may occur.
You can reach me via iota slack (@felandil) if you've got any questions.

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

