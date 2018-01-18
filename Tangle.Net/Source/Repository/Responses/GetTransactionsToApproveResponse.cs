namespace Tangle.Net.Source.Repository.Responses
{
  /// <summary>
  /// The get transactions to approve response.
  /// </summary>
  public class GetTransactionsToApproveResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the branch transaction.
    /// </summary>
    public string BranchTransaction { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the trunk transaction.
    /// </summary>
    public string TrunkTransaction { get; set; }

    #endregion
  }
}