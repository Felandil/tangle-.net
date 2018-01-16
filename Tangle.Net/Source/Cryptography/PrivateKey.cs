namespace Tangle.Net.Source.Cryptography
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The private key.
  /// </summary>
  public class PrivateKey : TryteString, IPrivateKey
  {
    #region Constants

    /// <summary>
    /// The chunk length.
    /// </summary>
    public const int ChunkLength = 2187;

    /// <summary>
    /// The fragment length.
    /// </summary>
    public const int FragmentLength = 6561;

    #endregion

    #region Fields

    /// <summary>
    /// The digest.
    /// </summary>
    private Digest digest;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PrivateKey"/> class.
    /// </summary>
    /// <param name="privateKey">
    /// The private key.
    /// </param>
    public PrivateKey(string privateKey)
      : base(privateKey)
    {
      this.SecurityLevel = 1;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PrivateKey"/> class.
    /// </summary>
    /// <param name="privateKey">
    /// The private key.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    public PrivateKey(string privateKey, int securityLevel)
      : base(privateKey)
    {
      this.SecurityLevel = securityLevel;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the digest.
    /// </summary>
    public Digest Digest
    {
      get
      {
        if (this.digest != null)
        {
          return this.digest;
        }

        var buffer = new int[Kerl.HashLength];
        var digests = new List<int>();
        var privateKeyAsTrits = Converter.TrytesToTrits(this.Value);

        for (var i = 0; i < privateKeyAsTrits.Length / FragmentLength; i++)
        {
          var keyFragment = privateKeyAsTrits.Skip(i * FragmentLength).Take(FragmentLength).ToArray();

          for (var j = 0; j < 27; j++)
          {
            buffer = keyFragment.Skip(j * Kerl.HashLength).Take(Kerl.HashLength).ToArray();

            for (var k = 0; k < 26; k++)
            {
              var innerKerl = new Kerl();
              innerKerl.Absorb(buffer);
              innerKerl.Squeeze(buffer);
            }

            for (var k = 0; k < Kerl.HashLength; k++)
            {
              keyFragment[(j * Kerl.HashLength) + k] = buffer[k];
            }
          }

          var kerl = new Kerl();
          kerl.Absorb(keyFragment);
          kerl.Squeeze(buffer);

          for (var j = 0; j < Kerl.HashLength; j++)
          {
            digests.Insert((i * Kerl.HashLength) + j, buffer[j]);
          }
        }

        this.digest = new Digest(Converter.TritsToTrytes(digests.ToArray()));

        return this.digest;
      }
    }

    /// <summary>
    /// Gets the security level.
    /// </summary>
    public int SecurityLevel { get; private set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The sign input transactions.
    /// </summary>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    public void SignInputTransactions(Bundle bundle, int startIndex)
    {
      if (bundle.Hash == null)
      {
        throw new ArgumentException("Bundle must contain valid Hash in order to be signed!");
      }

      var signatureFragmentGenerator = new SignatureFragmentGenerator(this, bundle.Hash);
      var signatureFragments = signatureFragmentGenerator.Generate();

      for (var i = 0; i < this.SecurityLevel; i++)
      {
        var transaction = bundle.Transactions[i + startIndex];
        transaction.SignatureFragment = signatureFragments[i];
      }
    }

    #endregion
  }
}