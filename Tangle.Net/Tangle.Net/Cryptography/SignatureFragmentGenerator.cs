namespace Tangle.Net.Cryptography
{
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Entity;

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
        var normalizedHashChunk = normalizedHash.Skip((i % 3) * 27).Take(27).ToArray(); // TODO - replace magic numbers
        var signatureFragmentTrits = chunk.ToTrits();
        var finalizedSignatureFragmentTrits = new List<int>();

        var count = chunk.GetChunks(Hash.Length).Count;
        for (var j = 0; j < count; j++)
        {
          var buffer = signatureFragmentTrits.Skip(j * AbstractCurl.HashLength).Take(AbstractCurl.HashLength).ToArray();

          for (var k = 0; k < 13 - normalizedHashChunk[j]; k++)
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
  }
}