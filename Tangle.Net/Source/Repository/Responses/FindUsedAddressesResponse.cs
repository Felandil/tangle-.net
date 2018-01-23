namespace Tangle.Net.Source.Repository.Responses
{
  using System.Collections.Generic;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The find used addresses response.
  /// </summary>
  public class FindUsedAddressesResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the associated transaction hashes.
    /// </summary>
    public List<Hash> AssociatedTransactionHashes { get; set; }

    /// <summary>
    /// Gets or sets the used addresses.
    /// </summary>
    public List<Address> UsedAddresses { get; set; }

    #endregion
  }
}