namespace Tangle.Net.Mam
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography;
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

    /// <summary>
    /// The mask.
    /// </summary>
    /// <param name="payload">
    /// The payload.
    /// </param>
    /// <param name="key">
    /// The auth id trits.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public int[] Mask(int[] payload, int[] key)
    {
      this.Curl.Reset();
      this.Curl.Absorb(key);

      var keyChunk = new int[AbstractCurl.HashLength];

      var maskedPayload = new List<int>();
      foreach (var chunk in payload.GetChunks(AbstractCurl.HashLength))
      {
        this.Curl.Squeeze(keyChunk);
        var length = chunk.Length;

        for (var i = 0; i < length; i++)
        {
          keyChunk[i] = Converter.Sum(chunk[i], keyChunk[i]);
        }

        maskedPayload.AddRange(keyChunk.Take(length));
      }

      return maskedPayload.ToArray();
    }

    /// <summary>
    /// The unmask.
    /// </summary>
    /// <param name="payload">
    /// The payload.
    /// </param>
    /// <param name="key">
    /// The key.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public int[] Unmask(int[] payload, int[] key)
    {
      this.Curl.Reset();
      this.Curl.Absorb(key);

      var keyChunk = new int[AbstractCurl.HashLength];

      var unmasked = new List<int>();
      foreach (var chunk in payload.GetChunks(AbstractCurl.HashLength))
      {
        this.Curl.Squeeze(keyChunk);
        var length = chunk.Length;

        for (var i = 0; i < length; i++)
        {
          keyChunk[i] = Converter.Sum(chunk[i], -keyChunk[i]);
        }

        unmasked.AddRange(keyChunk.Take(length));
      }

      return unmasked.ToArray();
    }

    #endregion
  }
}