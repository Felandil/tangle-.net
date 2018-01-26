namespace Tangle.Net.Repository.DataTransfer
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The inclusion state.
  /// </summary>
  public class InclusionStates
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InclusionStates"/> class.
    /// </summary>
    public InclusionStates()
    {
      this.States = new Dictionary<Hash, bool>();
    }

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