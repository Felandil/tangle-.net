namespace Tangle.Net.Source.Repository.Responses
{
  /// <summary>
  /// The remove neighbors response.
  /// </summary>
  public class RemoveNeighborsResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the added neighbors.
    /// </summary>
    public int RemovedNeighbors { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    #endregion
  }
}