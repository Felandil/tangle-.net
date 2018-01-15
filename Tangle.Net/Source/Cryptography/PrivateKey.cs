namespace Tangle.Net.Source.Cryptography
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.UI;

  using Castle.DynamicProxy.Contributors;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The private key.
  /// </summary>
  public class PrivateKey : IPrivateKey
  {
    /// <summary>
    /// The fragment length.
    /// </summary>
    public const int FragmentLength = 6561;

    #region Fields

    /// <summary>
    /// The digest.
    /// </summary>
    private string digest = string.Empty;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PrivateKey"/> class.
    /// </summary>
    /// <param name="privateKey">
    /// The private key.
    /// </param>
    public PrivateKey(string privateKey)
    {
      this.PrivateKeyValue = privateKey;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the digest.
    /// </summary>
    public string Digest
    {
      get
      {
        if (!string.IsNullOrEmpty(this.digest))
        {
          return this.digest;
        }

        var buffer = new int[243];
        var digests = new List<int>();
        var privateKeyAsTrits = Converter.TrytesToTrits(this.PrivateKeyValue);

        for (var i = 0; i < privateKeyAsTrits.Length / FragmentLength; i++)
        {
          var keyFragment = privateKeyAsTrits.Skip(i * FragmentLength).Take(FragmentLength).ToArray();

          for (var j = 0; j < 27; j++)
          {
            buffer = keyFragment.Skip(j * 243).Take(243).ToArray();

            for (var k = 0; k < 26; k++)
            {
              var innerKerl = new Kerl();
              innerKerl.Absorb(buffer);
              innerKerl.Squeeze(buffer);
            }

            for (var k = 0; k < 243; k++)
            {
              keyFragment[(j * 243) + k] = buffer[k];
            }
          }

          var kerl = new Kerl();
          kerl.Absorb(keyFragment);
          kerl.Squeeze(buffer);

          for (var j = 0; j < 243; j++)
          {
            digests.Insert((i * 243) + j, buffer[j]);
          }
        }

        this.digest = Converter.TritsToTrytes(digests.ToArray());

        return this.digest;
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the private key.
    /// </summary>
    private string PrivateKeyValue { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The sign input transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    public void SignInputTransactions(List<Transaction> transactions, int startIndex)
    {
    }

    #endregion
  }
}