namespace Tangle.Net.Cryptography.Signing
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The signature validator.
  /// </summary>
  public class SignatureValidator : ISignatureValidator
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureValidator"/> class.
    /// </summary>
    public SignatureValidator()
    {
      this.InnerCurl = new Kerl();
      this.OuterCurl = new Kerl();
      this.Curl = new Kerl();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureValidator"/> class.
    /// </summary>
    /// <param name="innerCurl">
    /// The inner curl.
    /// </param>
    /// <param name="outerCurl">
    /// The outer curl.
    /// </param>
    /// <param name="curl">
    /// The curl.
    /// </param>
    public SignatureValidator(AbstractCurl innerCurl, AbstractCurl outerCurl, AbstractCurl curl)
    {
      this.InnerCurl = innerCurl;
      this.OuterCurl = outerCurl;
      this.Curl = curl;
    }

    /// <summary>
    /// Gets the curl.
    /// </summary>
    private AbstractCurl Curl { get; }

    /// <summary>
    /// Gets the inner curl.
    /// </summary>
    private AbstractCurl InnerCurl { get; }

    /// <summary>
    /// Gets the outer curl.
    /// </summary>
    private AbstractCurl OuterCurl { get; }

    /// <inheritdoc />
    public bool ValidateFragments(List<Fragment> fragments, Hash hash, TryteString publicKey)
    {
      var checksum = new List<int>();
      var normalizedHash = Hash.Normalize(hash);

      for (var i = 0; i < fragments.Count; i++)
      {
        var normalizedHashChunk = normalizedHash.Skip((i % 3) * 27).Take(27).ToArray();
        var buffer = new int[Constants.TritHashLength];

        this.OuterCurl.Reset();

        var fragmentChunks = fragments[i].GetChunks(Hash.Length);
        for (var j = 0; j < fragmentChunks.Count; j++)
        {
          buffer = fragmentChunks[j].ToTrits();

          for (var k = 0; k < normalizedHashChunk[j] + 13; k++)
          {
            this.InnerCurl.Reset();
            this.InnerCurl.Absorb(buffer);
            this.InnerCurl.Squeeze(buffer);
          }

          this.OuterCurl.Absorb(buffer);
        }

        this.OuterCurl.Squeeze(buffer);
        checksum.AddRange(buffer);
      }

      var actualPublicKey = new int[Constants.TritHashLength];

      this.Curl.Reset();
      this.Curl.Absorb(checksum.ToArray());
      this.Curl.Squeeze(actualPublicKey);

      var actualPublicKeyTrytes = Converter.TritsToTrytes(actualPublicKey);
      return actualPublicKeyTrytes == publicKey.Value;
    }

    /// <inheritdoc />
    public async Task<bool> ValidateFragmentsAsync(List<Fragment> fragments, Hash hash, TryteString publicKey)
    {
      return await Task.Run(() => this.ValidateFragments(fragments, hash, publicKey));
    }
  }
}