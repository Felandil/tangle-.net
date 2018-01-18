namespace Tangle.Net.Source.Repository.Responses
{
  using System.Collections.Generic;

  /// <summary>
  /// The get inclusion states response.
  /// </summary>
  public class GetInclusionStatesResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the states.
    /// </summary>
    public List<bool> States { get; set; }

    #endregion
  }
}