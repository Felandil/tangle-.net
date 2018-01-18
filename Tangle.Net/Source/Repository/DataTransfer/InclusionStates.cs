namespace Tangle.Net.Source.Repository.DataTransfer
{
  using System.Collections.Generic;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The inclusion state.
  /// </summary>
  public class InclusionStates
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the states.
    /// </summary>
    public Dictionary<Hash, bool> States { get; set; }

    #endregion
  }
}