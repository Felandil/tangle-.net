namespace Tangle.Net.Repository.Factory
{
  using System.Threading.Tasks;

  /// <summary>
  /// The IotaRepositoryFactory interface.
  /// </summary>
  public interface IIotaRepositoryFactory
  {
    /// <summary>
    /// The create async.
    /// </summary>
    /// <param name="remotePoW">
    /// The remote po w.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<IIotaRepository> CreateAsync(bool remotePoW = false);
  }
}