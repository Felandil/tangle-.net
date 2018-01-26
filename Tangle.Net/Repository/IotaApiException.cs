namespace Tangle.Net.Repository
{
  using System;

  /// <summary>
  /// The iri api exception.
  /// </summary>
  public class IotaApiException : ApplicationException
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IotaApiException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    public IotaApiException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IotaApiException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="innerException">
    /// The inner exception.
    /// </param>
    public IotaApiException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    #endregion
  }
}