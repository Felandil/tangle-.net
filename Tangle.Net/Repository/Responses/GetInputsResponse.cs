namespace Tangle.Net.Repository.Responses
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The get inputs response.
  /// </summary>
  public class GetInputsResponse
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the addresses.
    /// </summary>
    public List<Address> Addresses { get; set; }

    /// <summary>
    /// Gets or sets the balance.
    /// </summary>
    public long Balance { get; set; }

    #endregion
  }
}