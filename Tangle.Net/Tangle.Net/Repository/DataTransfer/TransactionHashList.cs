namespace Tangle.Net.Repository.DataTransfer
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The transaction hash list.
  /// </summary>
  public class TransactionHashList
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the hashes.
    /// </summary>
    public List<Hash> Hashes { get; set; }

    #endregion
  }
}