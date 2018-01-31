namespace Tangle.Net.Repository.Responses
{
  using System.Collections.Generic;

  /// <summary>
  /// The transactions.
  /// </summary>
  public class GetTransactionsResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the hashes.
    /// </summary>
    public List<string> Hashes { get; set; }

    #endregion
  }
}