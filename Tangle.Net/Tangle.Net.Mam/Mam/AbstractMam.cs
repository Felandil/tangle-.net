namespace Tangle.Net.Mam.Mam
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The abstract mam.
  /// </summary>
  public abstract class AbstractMam
  {
    /// <summary>
    /// Gets or sets the curl.
    /// </summary>
    protected AbstractCurl Curl { get; set; }

    /// <summary>
    /// Gets or sets the mask.
    /// </summary>
    protected IMask Mask { get; set; }

    /// <summary>
    /// The get message address.
    /// </summary>
    /// <returns>
    /// The <see cref="Address"/>.
    /// </returns>
    /// <summary>
    /// The get message hash.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>
    /// The <see cref="Hash"/>.
    /// </returns>
    protected Hash GetMessageHash(TryteString message)
    {
      var hash = new int[AbstractCurl.HashLength];

      this.Curl.Reset();
      this.Curl.Absorb(message.ToTrits());
      this.Curl.Squeeze(hash);

      return new Hash(Converter.TritsToTrytes(hash));
    }

    /// <summary>
    /// The get channel key.
    /// </summary>
    /// <param name="lastChannelKey">
    /// The lastChannelKey.
    /// </param>
    /// <param name="salt">
    /// The salt.
    /// </param>
    /// <returns>
    /// The <see cref="Hash"/>.
    /// </returns>
    protected Hash GetChannelKey(TryteString lastChannelKey, TryteString salt)
    {
      var lastChannelKeyTrits = lastChannelKey.ToTrits();
      Converter.Increment(lastChannelKeyTrits, AbstractCurl.HashLength);
      return this.Mask.Hash(new TryteString(Converter.TritsToTrytes(lastChannelKeyTrits)), salt);
    }
  }
}
