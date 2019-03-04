Getting Started
============

Installation
-------------
Tangle.Net is compatible with .NET Standard 2.0 and .NET Framework 4.6.1.

You can install the packages via nuget

.. code-block:: bash

   https://www.nuget.org/packages/Tangle.Net/
   https://www.nuget.org/packages/Tangle.Net.Standard/

The most simple way to start using the library is by instantiating the repository via factory. Note that you could also create instances of the repository via any dependency injection framework.

.. code-block:: bash

    var repository = new RestIotaRepository(new RestClient("https://localhost:14265"));
    var nodeInfo = repository.GetNodeInfo();
    var neighbours = repository.GetNeighbors();

