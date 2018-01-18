namespace Tangle.Net.Source.Repository.DataTransfer
{
  /// <summary>
  /// The neighbor.
  /// </summary>
  public class Neighbor
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the connection type.
    /// </summary>
    public string ConnectionType { get; set; }

    /// <summary>
    /// Gets or sets the number of all transactions.
    /// </summary>
    public int NumberOfAllTransactions { get; set; }

    /// <summary>
    /// Gets or sets the number of invalid transactions.
    /// </summary>
    public int NumberOfInvalidTransactions { get; set; }

    /// <summary>
    /// Gets or sets the number of new transactions.
    /// </summary>
    public int NumberOfNewTransactions { get; set; }

    /// <summary>
    /// Gets or sets the number of random transaction requests.
    /// </summary>
    public int NumberOfRandomTransactionRequests { get; set; }

    /// <summary>
    /// Gets or sets the number of sent transactions.
    /// </summary>
    public int NumberOfSentTransactions { get; set; }

    #endregion
  }
}