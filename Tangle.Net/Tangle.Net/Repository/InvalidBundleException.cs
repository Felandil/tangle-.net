namespace Tangle.Net.Repository
{
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// The invalid bundle exception.
  /// </summary>
  public class InvalidBundleException : IotaApiException
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidBundleException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="validationErrors">
    /// The validation errors.
    /// </param>
    public InvalidBundleException(string message, List<string> validationErrors)
      : base(message)
    {
      this.ValidationErrors = validationErrors;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidBundleException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="validationErrors">
    /// The validation errors.
    /// </param>
    /// <param name="innerException">
    /// The inner exception.
    /// </param>
    public InvalidBundleException(string message, List<string> validationErrors, Exception innerException)
      : base(message, innerException)
    {
      this.ValidationErrors = validationErrors;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the validation errors.
    /// </summary>
    public List<string> ValidationErrors { get; private set; }

    #endregion
  }
}