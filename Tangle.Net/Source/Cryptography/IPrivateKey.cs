namespace Tangle.Net.Source.Cryptography
{
  using System.Collections.Generic;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The PrivateKey interface.
  /// </summary>
  public interface IPrivateKey
  {
    #region Public Methods and Operators

    /// <summary>
    /// The sign input transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    void SignInputTransactions(List<Transaction> transactions, int startIndex);

    #endregion
  }
}