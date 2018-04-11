namespace Tangle.Net.Mam.Services
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The mask.
  /// </summary>
  public class CurlMask : IMask
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMask"/> class.
    /// </summary>
    public CurlMask()
    {
      this.Curl = new Curl();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMask"/> class.
    /// </summary>
    /// <param name="curl">
    /// The curl.
    /// </param>
    public CurlMask(AbstractCurl curl)
    {
      this.Curl = curl;
    }

    /// <summary>
    /// The default.
    /// </summary>
    public static CurlMask Default => new CurlMask();

    /// <summary>
    /// Gets or sets the curl.
    /// </summary>
    private AbstractCurl Curl { get; set; }

    /// <inheritdoc />
    public Hash Hash(TryteString key, TryteString salt = null)
    {
      var keyTrits = new int[Constants.TritHashLength];

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
    public void Mask(int[] payload, AbstractCurl curl)
    {
      var keyChunk = curl.Rate(Constants.TritHashLength);

      var round = 0;
      foreach (var chunk in payload.GetChunks(Constants.TritHashLength))
      {
        curl.Absorb(chunk);
        var curlState = curl.Rate(Constants.TritHashLength);
        var length = chunk.Length;

        for (var i = 0; i < length; i++)
        {
          payload[round * Constants.TritHashLength + i] = Converter.Sum(chunk[i], keyChunk[i]);
          keyChunk[i] = curlState[i];
        }

        round++;
      }
    }

    /// <inheritdoc />
    public int[] Unmask(int[] payload, AbstractCurl curl)
    {
      var unmasked = new List<int>();
      foreach (var chunk in payload.GetChunks(Constants.TritHashLength))
      {
        for (var i = 0; i < chunk.Length; i++)
        {
          chunk[i] = Converter.Sum(chunk[i], -curl.Rate(Constants.TritHashLength)[i]);
        }

        unmasked.AddRange(chunk);
        curl.Absorb(chunk);
      }

      return unmasked.ToArray();
    }
  }
}