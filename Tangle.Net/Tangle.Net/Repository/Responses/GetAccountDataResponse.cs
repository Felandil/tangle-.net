namespace Tangle.Net.Repository.Responses
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The get account data response.
  /// </summary>
  public class GetAccountDataResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the associated bundles.
    /// </summary>
    public List<Bundle> AssociatedBundles { get; set; }

    /// <summary>
    /// Gets or sets the balance.
    /// </summary>
    public long Balance { get; set; }

    /// <summary>
    /// Gets or sets the latest unused address.
    /// </summary>
    public Address LatestUnusedAddress { get; set; }

    /// <summary>
    /// Gets or sets the used addresses.
    /// </summary>
    public List<Address> UsedAddresses { get; set; }

    #endregion
  }
}