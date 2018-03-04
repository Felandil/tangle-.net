namespace Tangle.Net.Repository.Factory
{
  using System.Threading.Tasks;

  /// <summary>
  /// The HealthyNodeSelector interface.
  /// </summary>
  public interface IHealthyNodeSelector
  {
    /// <summary>
    /// The get healthy node uri async.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<string> GetHealthyNodeUriAsync();

    /// <summary>
    /// The get healthy remote po w node uri async.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<string> GetHealthyRemotePoWNodeUriAsync();
  }
}