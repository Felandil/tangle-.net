namespace Tangle.Net.Cryptography.Signing
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The signature fragment generator.
  /// </summary>
  public class SignatureFragmentGenerator : ISignatureFragmentGenerator
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureFragmentGenerator"/> class.
    /// </summary>
    /// <param name="curl">
    /// The curl.
    /// </param>
    public SignatureFragmentGenerator(AbstractCurl curl)
    {
      this.Curl = curl;
    }

    /// <summary>
    /// Gets the curl.
    /// </summary>
    private AbstractCurl Curl { get; }

    /// <inheritdoc />
    public List<Fragment> Generate(AbstractPrivateKey privateKey, Hash hash)
    {
      var result = new List<Fragment>();
      var normalizedHash = Hash.Normalize(hash);

      var i = 0;
      var chunks = privateKey.GetChunks(AbstractPrivateKey.ChunkLength);
      foreach (var chunk in chunks)
      {
        var normalizedHashChunk = normalizedHash.Skip((i % Converter.Radix) * 27).Take(27).ToArray();
        var signatureFragmentTrits = chunk.ToTrits();
        var finalizedSignatureFragmentTrits = new List<int>();

        var count = chunk.GetChunks(Hash.Length).Count;
        for (var j = 0; j < count; j++)
        {
          var buffer = signatureFragmentTrits.Skip(j * Constants.TritHashLength).Take(Constants.TritHashLength).ToArray();

          for (var k = 0; k < Hash.MaxTryteValue - normalizedHashChunk[j]; k++)
          {
            this.Curl.Reset();
            this.Curl.Absorb(buffer);
            this.Curl.Squeeze(buffer);
          }

          finalizedSignatureFragmentTrits.AddRange(buffer);
        }

        result.Add(new Fragment(Converter.TritsToTrytes(finalizedSignatureFragmentTrits.ToArray())));

        i++;
      }

      return result;
    }

    /// <inheritdoc />
    public async Task<List<Fragment>> GenerateAsync(AbstractPrivateKey privateKey, Hash hash)
    {
      return await Task.Run(() => this.Generate(privateKey, hash));
    }
  }
}