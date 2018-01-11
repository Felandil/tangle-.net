namespace Tangle.Net.Source.Entity
{
  using System.Collections.Generic;

  /// <summary>
  /// The address balances.
  /// </summary>
  public class AddressBalances
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the balances.
    /// </summary>
    public List<long> Balances { get; set; }

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
    public List<string> References { get; set; }

    #endregion
  }
}