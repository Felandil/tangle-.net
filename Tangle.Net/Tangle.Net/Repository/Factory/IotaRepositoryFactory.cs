namespace Tangle.Net.Repository.Factory
{
  using System.Collections.Generic;

  using RestSharp;

  using Tangle.Net.ProofOfWork;
  using Tangle.Net.ProofOfWork.Service;
  using Tangle.Net.Repository.Client;

  public static class IotaRepositoryFactory
  {
    /// <summary>
    /// Creates the repository for a single node
    /// </summary>
    /// <param name="uri">Node Uri</param>
    /// <param name="powType">"Where" to do the proof of work</param>
    /// <param name="timeout">Timeout for node calls</param>
    /// <returns>
    /// The iota repository
    /// </returns>
    /// <exception>
    /// Throws an UriFormatException if the node uri is invalid
    /// </exception>
    public static RestIotaRepository Create(string uri, PoWType powType = PoWType.Local, int timeout = 5000)
    {
      var client = new RestIotaClient(new RestClient(uri) { Timeout = timeout });

      return new RestIotaRepository(client, ResolvePoWType(powType, client));
    }

    /// <summary>
    /// Creates the repository for multiple circuited nodes
    /// see: https://martinfowler.com/bliki/CircuitBreaker.html
    /// </summary>
    /// <param name="uris">Node Uri List</param>
    /// <param name="powType">"Where" to do the proof of work</param>
    /// <param name="timeout">Timeout for node calls</param>
    /// <returns>
    /// The iota repository
    /// </returns>
    /// <exception>
    /// Throws an UriFormatException if one of the node uris is invalid
    /// </exception>
    public static RestIotaRepository CreateWithFallback(List<string> uris, PoWType powType = PoWType.Local, int timeout = 5000)
    {
      var client = new FallbackIotaClient(uris, timeout);

      return new RestIotaRepository(client, ResolvePoWType(powType));
    }

    private static IPoWService ResolvePoWType(PoWType powType, IIotaClient client = null)
    {
      switch (powType)
      {
        case PoWType.Remote:
          return new RestPoWService(client);
        case PoWType.PoWSrv:
          return new PoWSrvService();
        default:
          return new PoWService(new CpuPearlDiver());
      }
    }
  }
}