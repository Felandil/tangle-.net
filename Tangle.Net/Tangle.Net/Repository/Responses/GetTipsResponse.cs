namespace Tangle.Net.Repository.Responses
{
  using System.Collections.Generic;

  /// <summary>
  /// The get tips response.
  /// </summary>
  public class GetTipsResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the hashes.
    /// </summary>
    public List<string> Hashes { get; set; }

    #endregion
  }
}