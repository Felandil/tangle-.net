namespace Tangle.Net.Source.Repository
{
  using System.Collections.Generic;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Repository.DataTransfer;
  using Tangle.Net.Source.Repository.Responses;

  /// <summary>
  /// The IotaExtendedRepository interface.
  /// </summary>
  public interface IIotaExtendedRepository
  {
    #region Public Methods and Operators

    /// <summary>
    /// The broadcast and store transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    void BroadcastAndStoreTransactions(List<TransactionTrytes> transactions);

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
    List<Address> FindUsedAddresses(Seed seed, int securityLevel, int start);

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
    /// The start index.
    /// </param>
    /// <param name="stopIndex">
    /// The stop index.
    /// </param>
    /// <returns>
    /// The <see cref="GetInputsResponse"/>.
    /// </returns>
    GetInputsResponse GetInputs(Seed seed, long threshold, int securityLevel, int startIndex, int stopIndex = 0);

    /// <summary>
    /// The get new addresses.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="index">
    /// The index.
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
    List<Address> GetNewAddresses(Seed seed, int index, int count, int securityLevel);

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
    List<TransactionTrytes> SendTrytes(IEnumerable<Transaction> transactions, int depth = 27, int minWeightMagnitude = 18);

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

    #endregion
  }
}