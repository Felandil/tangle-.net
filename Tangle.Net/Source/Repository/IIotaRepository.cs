namespace Tangle.Net.Source.Repository
{
  using System.Collections.Generic;

  using Tangle.Net.Source.DataTransfer;
  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Repository.Responses;

  /// <summary>
  /// The TangleRepository interface.
  /// </summary>
  public interface IIotaRepository
  {
    #region Public Methods and Operators

    /// <summary>
    /// The add neighbor.
    /// </summary>
    /// <param name="neighbors">
    /// The neighbors.
    /// </param>
    /// <returns>
    /// The <see cref="AddNeighborsResponse"/>.
    /// </returns>
    AddNeighborsResponse AddNeighbor(IEnumerable<Neighbor> neighbors);

    /// <summary>
    /// The attach to tangle.
    /// </summary>
    /// <param name="branchTransaction">
    /// The branch transactions.
    /// </param>
    /// <param name="trunkTransaction">
    /// The trunk transactions.
    /// </param>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    List<TransactionTrytes> AttachToTangle(
      Hash branchTransaction, 
      Hash trunkTransaction, 
      IEnumerable<Transaction> transactions, 
      int minWeightMagnitude = 18);

    /// <summary>
    /// The broadcast transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    void BroadcastTransactions(IEnumerable<TransactionTrytes> transactions);

    /// <summary>
    /// The find transactions.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionHashList"/>.
    /// </returns>
    TransactionHashList FindTransactions(Dictionary<string, IEnumerable<TryteString>> parameters);

    /// <summary>
    /// The get transactions by addresses.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <returns>
    /// The <see cref="GetTransactionsResponse"/>.
    /// </returns>
    TransactionHashList FindTransactionsByAddresses(IEnumerable<Address> addresses);

    /// <summary>
    /// The find transactions by approvees.
    /// </summary>
    /// <param name="approveeHashes">
    /// The approvee hashes.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionHashList"/>.
    /// </returns>
    TransactionHashList FindTransactionsByApprovees(IEnumerable<Hash> approveeHashes);

    /// <summary>
    /// The find transactions by bundles.
    /// </summary>
    /// <param name="bundleHashes">
    /// The bundle hashes.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionHashList"/>.
    /// </returns>
    TransactionHashList FindTransactionsByBundles(IEnumerable<Hash> bundleHashes);

    /// <summary>
    /// The find transactions by tags.
    /// </summary>
    /// <param name="tags">
    /// The tags.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionHashList"/>.
    /// </returns>
    TransactionHashList FindTransactionsByTags(IEnumerable<Tag> tags);

    /// <summary>
    /// The get balances.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <param name="threshold">
    /// The threshold.
    /// </param>
    /// <returns>
    /// The <see cref="AddressWithBalances"/>.
    /// </returns>
    AddressWithBalances GetBalances(List<Address> addresses, int threshold = 100);

    /// <summary>
    /// The get inclusion states.
    /// </summary>
    /// <param name="transactionHashes">
    /// The transactions hashes.
    /// </param>
    /// <param name="tips">
    /// The tips.
    /// </param>
    /// <returns>
    /// The <see cref="InclusionStates"/>.
    /// </returns>
    InclusionStates GetInclusionStates(List<Hash> transactionHashes, IEnumerable<Hash> tips);

    /// <summary>
    /// The get neighbors.
    /// </summary>
    /// <returns>
    /// The <see cref="NeighborList"/>.
    /// </returns>
    NeighborList GetNeighbors();

    /// <summary>
    /// The get node info.
    /// </summary>
    /// <returns>
    /// The <see cref="NodeInfo"/>.
    /// </returns>
    NodeInfo GetNodeInfo();

    /// <summary>
    /// The get tips.
    /// </summary>
    /// <returns>
    /// The <see cref="TipHashList"/>.
    /// </returns>
    TipHashList GetTips();

    /// <summary>
    /// The get transactions to approve.
    /// </summary>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionsToApprove"/>.
    /// </returns>
    TransactionsToApprove GetTransactionsToApprove(int depth = 27);

    /// <summary>
    /// The get trytes.
    /// </summary>
    /// <param name="hashes">
    /// The hashes.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    IEnumerable<TransactionTrytes> GetTrytes(IEnumerable<Hash> hashes);

    /// <summary>
    /// The interrupt attaching to tangle.
    /// </summary>
    void InterruptAttachingToTangle();

    /// <summary>
    /// The remove neighbors.
    /// </summary>
    /// <param name="neighbors">
    /// The neighbors.
    /// </param>
    /// <returns>
    /// The <see cref="RemoveNeighborsResponse"/>.
    /// </returns>
    RemoveNeighborsResponse RemoveNeighbors(List<Neighbor> neighbors);

    /// <summary>
    /// The store transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    void StoreTransactions(IEnumerable<TransactionTrytes> transactions);

    #endregion
  }
}