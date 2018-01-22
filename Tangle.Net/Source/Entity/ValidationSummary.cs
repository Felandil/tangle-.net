namespace Tangle.Net.Source.Entity
{
  using System.Collections.Generic;

  /// <summary>
  /// The validation summary.
  /// </summary>
  public class ValidationSummary
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the errors.
    /// </summary>
    public List<string> Errors { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether is valid.
    /// </summary>
    public bool IsValid { get; set; }

    #endregion
  }
}