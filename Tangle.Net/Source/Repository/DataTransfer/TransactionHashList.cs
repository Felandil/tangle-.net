namespace Tangle.Net.Source.Repository.DataTransfer
{
  using System.Collections.Generic;

  using Tangle.Net.Source.Entity;

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