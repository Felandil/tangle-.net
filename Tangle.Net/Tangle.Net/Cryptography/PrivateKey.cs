namespace Tangle.Net.Cryptography
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;

  /// <summary>
  /// The private key.
  /// </summary>
  public class PrivateKey : AbstractPrivateKey
  {
    /// <summary>
    /// The fragment length.
    /// </summary>
    public const int FragmentLength = 6561;

    /// <summary>
    /// The digest.
    /// </summary>
    private Digest digest;

    /// <inheritdoc />
    public PrivateKey(string privateKey, int securityLevel, int keyIndex, ISignatureFragmentGenerator signatureFragmentGenerator, AbstractCurl curl)
      : base(privateKey, securityLevel, keyIndex)
    {
      this.SignatureFragmentGenerator = signatureFragmentGenerator;
      this.Curl = curl;
    }

    /// <inheritdoc />
    public override Digest Digest
    {
      get
      {
        if (this.digest != null)
        {
          return this.digest;
        }

        var buffer = new int[AbstractCurl.HashLength];
        var digests = new List<int>();
        var privateKeyAsTrits = Converter.TrytesToTrits(this.Value);

        for (var i = 0; i < privateKeyAsTrits.Length / FragmentLength; i++)
        {
          var keyFragment = privateKeyAsTrits.Skip(i * FragmentLength).Take(FragmentLength).ToArray();

          for (var j = 0; j < 27; j++)
          {
            buffer = keyFragment.Skip(j * AbstractCurl.HashLength).Take(AbstractCurl.HashLength).ToArray();

            for (var k = 0; k < 26; k++)
            {
              this.Curl.Reset();
              this.Curl.Absorb(buffer);
              this.Curl.Squeeze(buffer);
            }

            for (var k = 0; k < AbstractCurl.HashLength; k++)
            {
              keyFragment[(j * AbstractCurl.HashLength) + k] = buffer[k];
            }
          }

          this.Curl.Reset();
          this.Curl.Absorb(keyFragment);
          this.Curl.Squeeze(buffer);

          for (var j = 0; j < AbstractCurl.HashLength; j++)
          {
            digests.Insert((i * AbstractCurl.HashLength) + j, buffer[j]);
          }
        }

        this.digest = new Digest(Converter.TritsToTrytes(digests.ToArray()), this.KeyIndex, this.SecurityLevel);

        return this.digest;
      }
    }

    /// <summary>
    /// Gets the signature fragment generator.
    /// </summary>
    private ISignatureFragmentGenerator SignatureFragmentGenerator { get; }

    /// <summary>
    /// Gets the curl.
    /// </summary>
    private AbstractCurl Curl { get; }

    /// <inheritdoc />
    public override void SignInputTransactions(Bundle bundle, int startIndex)
    {
      if (bundle.Hash == null)
      {
        throw new ArgumentException("Bundle must contain valid Hash in order to be signed!");
      }

      var signatureFragments = this.SignatureFragmentGenerator.Generate(this, bundle.Hash);

      for (var i = 0; i < this.SecurityLevel; i++)
      {
        var transaction = bundle.Transactions[i + startIndex];
        transaction.Fragment = signatureFragments[i];
      }
    }
  }
}