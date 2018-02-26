namespace Tangle.Net.Repository
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository.DataTransfer;

  /// <summary>
  /// The TangleRepository interface.
  /// </summary>
  public interface IIotaCoreRepository
  {
    #region Public Methods and Operators

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
    /// The attach to tangle async.
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
    Task<List<TransactionTrytes>> AttachToTangleAsync(
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
    /// The broadcast transactions async.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task BroadcastTransactionsAsync(IEnumerable<TransactionTrytes> transactions);

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
    /// The find transactions async.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<TransactionHashList> FindTransactionsAsync(Dictionary<string, IEnumerable<TryteString>> parameters);

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
    /// The find transactions by addresses async.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<TransactionHashList> FindTransactionsByAddressesAsync(IEnumerable<Address> addresses);

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
    /// The find transactions by approvees async.
    /// </summary>
    /// <param name="approveeHashes">
    /// The approvee hashes.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<TransactionHashList> FindTransactionsByApproveesAsync(IEnumerable<Hash> approveeHashes);

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
    /// The find transactions by bundles async.
    /// </summary>
    /// <param name="bundleHashes">
    /// The bundle hashes.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<TransactionHashList> FindTransactionsByBundlesAsync(IEnumerable<Hash> bundleHashes);

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
    /// The find transactions by tags async.
    /// </summary>
    /// <param name="tags">
    /// The tags.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<TransactionHashList> FindTransactionsByTagsAsync(IEnumerable<Tag> tags);

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
    /// The get balances async.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <param name="threshold">
    /// The threshold.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<AddressWithBalances> GetBalancesAsync(List<Address> addresses, int threshold = 100);

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
    /// The get inclusion states async.
    /// </summary>
    /// <param name="transactionHashes">
    /// The transaction hashes.
    /// </param>
    /// <param name="tips">
    /// The tips.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<InclusionStates> GetInclusionStatesAsync(List<Hash> transactionHashes, IEnumerable<Hash> tips);

    /// <summary>
    /// The get tips.
    /// </summary>
    /// <returns>
    /// The <see cref="TipHashList"/>.
    /// </returns>
    TipHashList GetTips();

    /// <summary>
    /// The get tips async.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<TipHashList> GetTipsAsync();

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
    /// The get transactions to approve async.
    /// </summary>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<TransactionsToApprove> GetTransactionsToApproveAsync(int depth = 27);

    /// <summary>
    /// The get trytes.
    /// </summary>
    /// <param name="hashes">
    /// The hashes.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    List<TransactionTrytes> GetTrytes(IEnumerable<Hash> hashes);

    /// <summary>
    /// The get trytes async.
    /// </summary>
    /// <param name="hashes">
    /// The hashes.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<List<TransactionTrytes>> GetTrytesAsync(IEnumerable<Hash> hashes);

    /// <summary>
    /// The interrupt attaching to tangle.
    /// </summary>
    void InterruptAttachingToTangle();

    /// <summary>
    /// The interrupt attaching to tangle async.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task InterruptAttachingToTangleAsync();

    /// <summary>
    /// The store transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    void StoreTransactions(IEnumerable<TransactionTrytes> transactions);

    /// <summary>
    /// The store transactions async.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task StoreTransactionsAsync(IEnumerable<TransactionTrytes> transactions);

    /// <summary>
    /// The where addresses spent from.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<Address> WereAddressesSpentFrom(List<Address> addresses);

    /// <summary>
    /// The were addresses spent from async.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<List<Address>> WereAddressesSpentFromAsync(List<Address> addresses);

    #endregion
  }
}