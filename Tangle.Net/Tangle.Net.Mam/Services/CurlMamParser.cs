namespace Tangle.Net.Mam.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Utils;

  /// <inheritdoc />
  public class CurlMamParser : IMamParser
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMamParser"/> class.
    /// </summary>
    /// <param name="mask">
    /// The mask.
    /// </param>
    /// <param name="treeFactory">
    /// The tree Factory.
    /// </param>
    /// <param name="curl">
    /// The curl.
    /// </param>
    /// <param name="signingHelper">
    /// The signature Validator.
    /// </param>
    public CurlMamParser(IMask mask, IMerkleTreeFactory treeFactory, AbstractCurl curl, ISigningHelper signingHelper)
    {
      this.TreeFactory = treeFactory;
      this.SigningHelper = signingHelper;
      this.Mask = mask;
      this.Curl = curl;
    }

    /// <summary>
    /// The default.
    /// </summary>
    public static CurlMamParser Default =>
      new CurlMamParser(
        new CurlMask(new Curl(CurlMode.CurlP81)),
        CurlMerkleTreeFactory.Default,
        new Curl(CurlMode.CurlP27),
        new IssSigningHelper(new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27)));

    /// <summary>
    /// Gets the tree factory.
    /// </summary>
    private IMerkleTreeFactory TreeFactory { get; }

    /// <summary>
    /// Gets the signature validator.
    /// </summary>
    private ISigningHelper SigningHelper { get; }

    /// <summary>
    /// Gets the curl.
    /// </summary>
    private AbstractCurl Curl { get; }

    /// <summary>
    /// Gets the mask.
    /// </summary>
    private IMask Mask { get; }

    /// <inheritdoc />
    public UnmaskedAuthenticatedMessage Unmask(Bundle payload, TryteString root, TryteString channelKey)
    {
      var payloadTrits = payload.Transactions.Select(t => t.Fragment).ToList().Merge().ToTrits();

      // Get data indices
      var indexData = Pascal.Decode(payloadTrits);
      var index = indexData.Item1;
      var messageData = Pascal.Decode(payloadTrits.Skip(indexData.Item2).ToArray());
      var messageLength = messageData.Item1;
      var nextRootStart = indexData.Item2 + messageData.Item2;
      var messageStart = nextRootStart + Constants.TritHashLength;
      var messageEnd = messageStart + messageLength;

      // Absorb key, root, index and message trits
      this.Curl.Reset();
      this.Curl.Absorb(channelKey.ToTrits());
      this.Curl.Absorb(root.ToTrits());
      this.Curl.Absorb(payloadTrits.Take(nextRootStart).ToArray());

      // decrypt metadata and create hmac
      var nextRoot = this.Mask.Unmask(payloadTrits.Skip(nextRootStart).Take(Constants.TritHashLength).ToArray(), this.Curl);
      var message = this.Mask.Unmask(payloadTrits.Skip(messageStart).Take(messageLength).ToArray(), this.Curl);
      var nonce = this.Mask.Unmask(payloadTrits.Skip(messageEnd).Take(Constants.TritHashLength / 3).ToArray(), this.Curl);
      var hmac = this.Curl.Rate(Constants.TritHashLength);

      // verify security level with hmac
      var securityLevel = this.SigningHelper.ChecksumSecurity(hmac);

      if (securityLevel == 0)
      {
        throw new ArgumentException("Given payload is invalid. (Security level can not be verified)");
      }

      // decrypt remaining payload
      var decryptedMetadata = this.Mask.Unmask(payloadTrits.Skip(messageEnd + nonce.Length).ToArray(), this.Curl);
      this.Curl.Reset();

      // get signature, derive digest and absorb it
      var signature = decryptedMetadata.Take(securityLevel * PrivateKey.FragmentLength).ToArray();
      this.Curl.Absorb(this.SigningHelper.DigestFromSignature(hmac, signature));

      // decode sibling information and recalculate root to verify payload
      var siblingsCountData = Pascal.Decode(decryptedMetadata.Skip(securityLevel * PrivateKey.FragmentLength).ToArray());
      var siblingsCount = siblingsCountData.Item1;
      var recalculatedRoot = siblingsCount != 0
                           ? this.RecalculateRootWithSiblings(decryptedMetadata, securityLevel, siblingsCountData.Item2, siblingsCount, index)
                           : new Hash(Converter.TritsToTrytes(this.Curl.Rate(Constants.TritHashLength)));

      if (recalculatedRoot.Value != root.Value)
      {
        throw new ArgumentException("Given payload is invalid. (Given root does not match payload root)");
      }

      return new UnmaskedAuthenticatedMessage
               {
                 NextRoot = new Hash(Converter.TritsToTrytes(nextRoot)),
                 Message = new TryteString(Converter.TritsToTrytes(message)),
                 Root = new Hash(root.Value)
      };
    }

    /// <summary>
    /// The recalculate root with siblings.
    /// </summary>
    /// <param name="decryptedMetadata">
    /// The decrypted metadata.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="siblingsCountLength">
    /// The siblings count length.
    /// </param>
    /// <param name="siblingsCount">
    /// The siblings count.
    /// </param>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="Hash"/>.
    /// </returns>
    private Hash RecalculateRootWithSiblings(IEnumerable<int> decryptedMetadata, int securityLevel, int siblingsCountLength, int siblingsCount, int index)
    {
      var siblings = decryptedMetadata.Skip((securityLevel * PrivateKey.FragmentLength) + siblingsCountLength)
        .Take(siblingsCount * Constants.TritHashLength).ToArray();

      return this.TreeFactory.RecalculateRoot(siblings, this.Curl.Rate(Constants.TritHashLength), index);
    }
  }
}