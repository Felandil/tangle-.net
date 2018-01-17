namespace Tangle.Net.Source.Repository
{
  using System;
  using System.Collections.Generic;

  using Tangle.Net.Source.DataTransfer;
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
    /// <param name="security">
    /// The security.
    /// </param>
    /// <param name="remainderAddress">
    /// The remainder address.
    /// </param>
    /// <param name="transfers">
    /// The transfers.
    /// </param>
    /// <param name="inputs">
    /// The inputs.
    /// </param>
    /// <param name="validateInputs">
    /// The validate inputs.
    /// </param>
    /// <param name="validateInputAddresses">
    /// The validate input addresses.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<Tuple<Transaction, bool>> SendTransfers(
      string seed, 
      int depth, 
      int minWeightMagnitude, 
      int security, 
      string remainderAddress, 
      IReadOnlyCollection<Transfer> transfers, 
      List<Input> inputs, 
      bool validateInputs, 
      bool validateInputAddresses);

    #endregion
  }
}