namespace Tangle.Net.ProofOfWork
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;

  /// <summary>
  /// The PoWService interface.
  /// </summary>
  public interface IPoWService
  {
    /// <summary>
    /// The do po w.
    /// </summary>
    /// <param name="branchTransaction">
    /// The branch transaction.
    /// </param>
    /// <param name="trunkTransaction">
    /// The trunk transaction.
    /// </param>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<Transaction> DoPoW(Hash branchTransaction, Hash trunkTransaction, List<Transaction> transactions, int minWeightMagnitude = 18);

    /// <summary>
    /// The do po w async.
    /// </summary>
    /// <param name="branchTransaction">
    /// The branch transaction.
    /// </param>
    /// <param name="trunkTransaction">
    /// The trunk transaction.
    /// </param>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<List<Transaction>> DoPoWAsync(Hash branchTransaction, Hash trunkTransaction, List<Transaction> transactions, int minWeightMagnitude = 18);
  }
}