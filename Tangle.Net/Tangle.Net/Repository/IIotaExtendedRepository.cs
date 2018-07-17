namespace Tangle.Net.Repository
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository.DataTransfer;
  using Tangle.Net.Repository.Responses;

  /// <summary>
  /// The IotaExtendedRepository interface.
  /// </summary>
  public interface IIotaExtendedRepository
  {
    #region Public Methods and Operators

    Task<bool> IsPromotableAsync(Hash tailTransaction, int depth = 6);

    Task PromoteTransactionAsync(Hash tailTransaction, int depth = 8, int minWeightMagnitude = 14, int attempts = 10);

    /// <summary>
    /// The broadcast and store transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    void BroadcastAndStoreTransactions(List<TransactionTrytes> transactions);

    /// <summary>
    /// The broadcast and store transactions async.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task BroadcastAndStoreTransactionsAsync(List<TransactionTrytes> transactions);

    /// <summary>
    /// The find used addresses.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="start">
    /// The start.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    FindUsedAddressesResponse FindUsedAddressesWithTransactions(Seed seed, int securityLevel, int start);

    /// <summary>
    /// The find used addresses with transactions async.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="start">
    /// The start.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<FindUsedAddressesResponse> FindUsedAddressesWithTransactionsAsync(Seed seed, int securityLevel, int start);

    /// <summary>
    /// The get account data.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="includeInclusionStates">
    /// The inclusion state.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="addressStartIndex">
    /// The address start index.
    /// </param>
    /// <param name="addressStopIndex">
    /// The address stop index.
    /// </param>
    /// <returns>
    /// The <see cref="GetAccountDataResponse"/>.
    /// </returns>
    GetAccountDataResponse GetAccountData(Seed seed, bool includeInclusionStates, int securityLevel, int addressStartIndex, int addressStopIndex = 0);

    /// <summary>
    /// The get account data async.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="includeInclusionStates">
    /// The include inclusion states.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="addressStartIndex">
    /// The address start index.
    /// </param>
    /// <param name="addressStopIndex">
    /// The address stop index.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<GetAccountDataResponse> GetAccountDataAsync(Seed seed, bool includeInclusionStates, int securityLevel, int addressStartIndex, int addressStopIndex = 0);

    /// <summary>
    /// The get bundle.
    /// </summary>
    /// <param name="transactionHash">
    /// The transaction hash.
    /// </param>
    /// <returns>
    /// The <see cref="Bundle"/>.
    /// </returns>
    Bundle GetBundle(Hash transactionHash);

    /// <summary>
    /// The get bundle async.
    /// </summary>
    /// <param name="transactionHash">
    /// The transaction hash.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<Bundle> GetBundleAsync(Hash transactionHash);

    /// <summary>
    /// The get bundles.
    /// </summary>
    /// <param name="transactionHashes">
    /// The transaction hashes.
    /// </param>
    /// <param name="includeInclusionStates">
    /// The include inclusion states.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<Bundle> GetBundles(List<Hash> transactionHashes, bool includeInclusionStates);

    /// <summary>
    /// The get bundles async.
    /// </summary>
    /// <param name="transactionHashes">
    /// The transaction hashes.
    /// </param>
    /// <param name="includeInclusionStates">
    /// The include inclusion states.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<List<Bundle>> GetBundlesAsync(List<Hash> transactionHashes, bool includeInclusionStates);

    /// <summary>
    /// The get inputs.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="threshold">
    /// The threshold.
    /// </param>
    /// <param name="securityLevel">
    /// The security Level.
    /// </param>
    /// <param name="startIndex">
    /// The start addressStartIndex.
    /// </param>
    /// <param name="stopIndex">
    /// The stop addressStartIndex.
    /// </param>
    /// <returns>
    /// The <see cref="GetInputsResponse"/>.
    /// </returns>
    GetInputsResponse GetInputs(Seed seed, long threshold, int securityLevel, int startIndex, int stopIndex = 0);

    /// <summary>
    /// The get inputs async.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="threshold">
    /// The threshold.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    /// <param name="stopIndex">
    /// The stop index.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<GetInputsResponse> GetInputsAsync(Seed seed, long threshold, int securityLevel, int startIndex, int stopIndex = 0);

    /// <summary>
    /// The get latest inclusion.
    /// </summary>
    /// <param name="hashes">
    /// The hashes.
    /// </param>
    /// <returns>
    /// The <see cref="InclusionStates"/>.
    /// </returns>
    InclusionStates GetLatestInclusion(List<Hash> hashes);

    /// <summary>
    /// The get latest inclusion async.
    /// </summary>
    /// <param name="hashes">
    /// The hashes.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<InclusionStates> GetLatestInclusionAsync(List<Hash> hashes);

    /// <summary>
    /// The get new addresses.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="addressStartIndex">
    /// The addressStartIndex.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<Address> GetNewAddresses(Seed seed, int addressStartIndex, int count, int securityLevel);

    /// <summary>
    /// The get new addresses async.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="addressStartIndex">
    /// The address start index.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<List<Address>> GetNewAddressesAsync(Seed seed, int addressStartIndex, int count, int securityLevel);

    /// <summary>
    /// The get transfers.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="includeInclusionStates">
    /// The include inclusion states.
    /// </param>
    /// <param name="addressStartIndex">
    /// The address start index.
    /// </param>
    /// <param name="addressStopIndex">
    /// The address stop index.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<Bundle> GetTransfers(Seed seed, int securityLevel, bool includeInclusionStates, int addressStartIndex, int addressStopIndex = 0);

    /// <summary>
    /// The get transfers async.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="includeInclusionStates">
    /// The include inclusion states.
    /// </param>
    /// <param name="addressStartIndex">
    /// The address start index.
    /// </param>
    /// <param name="addressStopIndex">
    /// The address stop index.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<List<Bundle>> GetTransfersAsync(Seed seed, int securityLevel, bool includeInclusionStates, int addressStartIndex, int addressStopIndex = 0);

    /// <summary>
    /// The prepare transfer.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="securityLevel">
    /// The security Level.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder address.
    /// </param>
    /// <param name="inputAddresses">
    /// The input addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Bundle"/>.
    /// </returns>
    Bundle PrepareTransfer(Seed seed, Bundle bundle, int securityLevel, Address remainderAddress = null, List<Address> inputAddresses = null);

    /// <summary>
    /// The prepare transfer async.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder address.
    /// </param>
    /// <param name="inputAddresses">
    /// The input addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<Bundle> PrepareTransferAsync(Seed seed, Bundle bundle, int securityLevel, Address remainderAddress = null, List<Address> inputAddresses = null);

    /// <summary>
    /// The replay bundle.
    /// </summary>
    /// <param name="transactionHash">
    /// The transaction hash.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<TransactionTrytes> ReplayBundle(Hash transactionHash, int depth = 8, int minWeightMagnitude = 14);

    /// <summary>
    /// The replay bundle async.
    /// </summary>
    /// <param name="transactionHash">
    /// The transaction hash.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<List<TransactionTrytes>> ReplayBundleAsync(Hash transactionHash, int depth = 8, int minWeightMagnitude = 14);

    /// <summary>
    /// The send transfer.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder address.
    /// </param>
    /// <param name="inputAddresses">
    /// The input addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Bundle"/>.
    /// </returns>
    Bundle SendTransfer(
      Seed seed, 
      Bundle bundle, 
      int securityLevel, 
      int depth = 8, 
      int minWeightMagnitude = 14, 
      Address remainderAddress = null, 
      List<Address> inputAddresses = null);

    /// <summary>
    /// The send transfer async.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder address.
    /// </param>
    /// <param name="inputAddresses">
    /// The input addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<Bundle> SendTransferAsync(
      Seed seed,
      Bundle bundle,
      int securityLevel,
      int depth = 8,
      int minWeightMagnitude = 14,
      Address remainderAddress = null,
      List<Address> inputAddresses = null);

    /// <summary>
    /// The send trytes.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<TransactionTrytes> SendTrytes(IEnumerable<Transaction> transactions, int depth = 8, int minWeightMagnitude = 14);

    /// <summary>
    /// The send trytes async.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<List<TransactionTrytes>> SendTrytesAsync(IEnumerable<Transaction> transactions, int depth = 8, int minWeightMagnitude = 14);

    #endregion
  }
}