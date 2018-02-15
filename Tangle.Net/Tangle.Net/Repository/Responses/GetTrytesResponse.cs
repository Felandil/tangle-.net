namespace Tangle.Net.Repository.Responses
{
  using System.Collections.Generic;

  /// <summary>
  /// The get trytes response.
  /// </summary>
  public class GetTrytesResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the trytes.
    /// </summary>
    public List<string> Trytes { get; set; }

    #endregion
  }
}