namespace Tangle.Net.Mam.Services
{
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The mask.
  /// </summary>
  public class CurlMask : IMask
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMask"/> class.
    /// </summary>
    public CurlMask()
    {
      this.Curl = new Curl();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the curl.
    /// </summary>
    private AbstractCurl Curl { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <inheritdoc />
    public Hash Hash(TryteString key, TryteString salt = null)
    {
      var keyTrits = new int[AbstractCurl.HashLength];

      this.Curl.Reset();
      this.Curl.Absorb(key.ToTrits());

      if (salt != null)
      {
        this.Curl.Absorb(salt.ToTrits());
      }

      this.Curl.Squeeze(keyTrits);

      return new Hash(Converter.TritsToTrytes(keyTrits));
    }

    /// <inheritdoc />
    public TryteString Mask(TryteString payload, TryteString key)
    {
      this.Curl.Reset();
      this.Curl.Absorb(key.ToTrits());

      var keyChunk = new int[AbstractCurl.HashLength];

      var maskedPayload = new List<int>();
      foreach (var chunk in payload.ToTrits().GetChunks(AbstractCurl.HashLength))
      {
        this.Curl.Squeeze(keyChunk);
        var length = chunk.Length;

        for (var i = 0; i < length; i++)
        {
          keyChunk[i] = Converter.Sum(chunk[i], keyChunk[i]);
        }

        maskedPayload.AddRange(keyChunk.Take(length));
      }

      return new TryteString(Converter.TritsToTrytes(maskedPayload.ToArray()));
    }

    /// <inheritdoc />
    public TryteString Unmask(TryteString payload, TryteString key)
    {
      this.Curl.Reset();
      this.Curl.Absorb(key.ToTrits());

      var keyChunk = new int[AbstractCurl.HashLength];

      var unmasked = new List<int>();
      foreach (var chunk in payload.ToTrits().GetChunks(AbstractCurl.HashLength))
      {
        this.Curl.Squeeze(keyChunk);
        var length = chunk.Length;

        for (var i = 0; i < length; i++)
        {
          keyChunk[i] = Converter.Sum(chunk[i], -keyChunk[i]);
        }

        unmasked.AddRange(keyChunk.Take(length));
      }

      return new TryteString(Converter.TritsToTrytes(unmasked.ToArray()));
    }

    #endregion
  }
}