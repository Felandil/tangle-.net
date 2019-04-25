namespace Tangle.Net.Examples.Examples.Services
{
  using System.Collections.Generic;

  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Repository.Factory;

  public class IotaRepositoryFactoryExample
  {
    public void Execute()
    {
      // Create a repository with a single node and with PoW
      var repository = IotaRepositoryFactory.Create("http://somenode.uri:12345");

      // Create a repository with a single node with PoW on the node
      repository = IotaRepositoryFactory.Create("http://somenode.uri:12345", PoWType.Remote);

      // Create a repository with a single node with PoW done by PoWSrv
      repository = IotaRepositoryFactory.Create("http://somenode.uri:12345", PoWType.PoWSrv);

      // Create a repository with multiple circuited nodes with local PoW
      repository = IotaRepositoryFactory.CreateWithFallback(new List<string> { "http://somenode.uri:12345", "http://someothernode.uri:12345" });

      // Create a repository with multiple circuited nodes with PoW on the nodes
      repository = IotaRepositoryFactory.CreateWithFallback(new List<string> { "http://somenode.uri:12345", "http://someothernode.uri:12345" }, PoWType.Remote);

      // Create a repository with multiple circuited nodes with PoW done by PoWSrv
      repository = IotaRepositoryFactory.CreateWithFallback(new List<string> { "http://somenode.uri:12345", "http://someothernode.uri:12345" }, PoWType.PoWSrv);
    }
  }
}