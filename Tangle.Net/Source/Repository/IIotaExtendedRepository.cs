namespace Tangle.Net.Source.Repository
{
  using System.Collections.Generic;

  using Tangle.Net.Source.Entity;

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

    #endregion
  }
}