namespace Tangle.Net.Source.Repository.Responses
{
  using System.Collections.Generic;

  /// <summary>
  /// The get balance response.
  /// </summary>
  public class GetBalanceResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the balances.
    /// </summary>
    public List<long> Balances { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the milestone index.
    /// </summary>
    public int MilestoneIndex { get; set; }

    /// <summary>
    /// Gets or sets the references.
    /// </summary>
    public List<string> References { get; set; }

    #endregion
  }
}