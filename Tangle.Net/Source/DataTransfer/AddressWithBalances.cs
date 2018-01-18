namespace Tangle.Net.Source.DataTransfer
{
  using System.Collections.Generic;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The address balances.
  /// </summary>
  public class AddressWithBalances
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the balances.
    /// </summary>
    public List<Address> Addresses { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the milestone index.
    /// </summary>
    public int MilestoneIndex { get; set; }

    /// <summary>
    /// Gets or sets the references.
    /// </summary>
    public List<TryteString> References { get; set; }

    #endregion
  }
}