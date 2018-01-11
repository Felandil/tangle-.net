namespace Tangle.Net.Source.Repository
{
  using System.Collections.Generic;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The TangleRepository interface.
  /// </summary>
  public interface IIotaRepository
  {
    #region Public Methods and Operators

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
    /// The <see cref="AddressBalances"/>.
    /// </returns>
    AddressBalances GetBalances(IEnumerable<string> addresses, int threshold);

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
    /// The get transactions by addresses.
    /// </summary>
    /// <param name="addresses">
    /// The addresses.
    /// </param>
    /// <returns>
    /// The <see cref="Transactions"/>.
    /// </returns>
    Transactions GetTransactionsByAddresses(IEnumerable<string> addresses);

    /// <summary>
    /// The send transfers.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="depth">
    /// The depth.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <param name="transfers">
    /// The transfers.
    /// </param>
    /// <param name="options">
    /// The options.
    /// </param>
    void SendTransfers(string seed, int depth, int minWeightMagnitude, IEnumerable<Transfer> transfers, Dictionary<string, string> options);

    #endregion
  }
}