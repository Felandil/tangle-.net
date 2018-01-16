namespace Tangle.Net.Source.Cryptography
{
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The signature fragment generator.
  /// </summary>
  public class SignatureFragmentGenerator
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureFragmentGenerator"/> class.
    /// </summary>
    /// <param name="privateKey">
    /// The private key.
    /// </param>
    /// <param name="hash">
    /// The hash.
    /// </param>
    public SignatureFragmentGenerator(PrivateKey privateKey, Hash hash)
    {
      this.PrivateKey = privateKey;
      this.Hash = hash;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the hash.
    /// </summary>
    private Hash Hash { get; set; }

    /// <summary>
    /// Gets or sets the private key.
    /// </summary>
    private PrivateKey PrivateKey { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The generate.
    /// </summary>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<TryteString> Generate()
    {
      var result = new List<TryteString>();
      var normalizedHash = Hash.Normalize(this.Hash);

      var i = 0;
      var chunks = this.PrivateKey.GetChunks(PrivateKey.ChunkLength);
      foreach (var chunk in chunks)
      {
        var normalizedHashChunk = normalizedHash.Skip(i * 27).Take(27).ToArray(); // TODO - replace magic numbers
        var signatureFragmentTrits = chunk.ToTrits();
        var finalizedSignatureFragmentTrits = new List<int>();
        var kerl = new Kerl();

        var count = chunk.GetChunks(Hash.Length).Count;
        for (var j = 0; j < count; j++)
        {
          var buffer = signatureFragmentTrits.Skip(j * Kerl.HashLength).Take(Kerl.HashLength).ToArray();

          for (var k = 0; k < 13 - normalizedHashChunk[j]; k++)
          {
            kerl.Reset();
            kerl.Absorb(buffer);
            kerl.Squeeze(buffer);
          }

          finalizedSignatureFragmentTrits.AddRange(buffer);
        }

        result.Add(new TryteString(Converter.TritsToTrytes(finalizedSignatureFragmentTrits.ToArray())));

        i++;
      }

      return result;
    }

    #endregion
  }
}