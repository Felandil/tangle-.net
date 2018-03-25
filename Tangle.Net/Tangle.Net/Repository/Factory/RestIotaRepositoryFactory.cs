namespace Tangle.Net.Repository.Factory
{
  using System.Threading.Tasks;

  using RestSharp;

  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Repository.Client;

  /// <summary>
  /// The rest iota repository factory.
  /// </summary>
  public class RestIotaRepositoryFactory : IIotaRepositoryFactory
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RestIotaRepositoryFactory"/> class.
    /// </summary>
    public RestIotaRepositoryFactory()
    {
      this.NodeSelector = new IotaDanceHealthyNodeSelector();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RestIotaRepositoryFactory"/> class.
    /// </summary>
    /// <param name="nodeSelector">
    /// The node selector.
    /// </param>
    public RestIotaRepositoryFactory(IHealthyNodeSelector nodeSelector)
    {
      this.NodeSelector = nodeSelector;
    }

    /// <summary>
    /// Gets the node selector.
    /// </summary>
    private IHealthyNodeSelector NodeSelector { get; }

    /// <inheritdoc />
    public async Task<IIotaRepository> CreateAsync(bool remotePoW = false)
    {
      string nodeUri;
      if (remotePoW)
      {
        nodeUri = await this.NodeSelector.GetHealthyRemotePoWNodeUriAsync();
      }
      else
      {
        nodeUri = await this.NodeSelector.GetHealthyNodeUriAsync();
      }

      var client = new RestIotaClient(new RestClient(nodeUri));

      IPoWService powService;
      if (remotePoW)
      {
        powService = new RestPoWService(client);
      }
      else
      {
        powService = new PoWService(new CpuPearlDiver());
      }

      return new RestIotaRepository(client, powService);
    }
  }
}