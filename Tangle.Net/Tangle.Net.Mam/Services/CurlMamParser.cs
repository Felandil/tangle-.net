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
  using Tangle.Net.Repository;
  using Tangle.Net.Utils;

  /// <summary>
  /// The curl mam parser.
  /// </summary>
  public class CurlMamParser : AbstractMam, IMamParser
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
    /// <param name="signatureValidator">
    /// The signature Validator.
    /// </param>
    public CurlMamParser(IMask mask, IMerkleTreeFactory treeFactory, AbstractCurl curl, ISignatureValidator signatureValidator)
    {
      this.TreeFactory = treeFactory;
      this.SignatureValidator = signatureValidator;
      this.Mask = mask;
      this.Curl = curl;
    }

    /// <summary>
    /// The default.
    /// </summary>
    public static CurlMamParser Default =>
      new CurlMamParser(
        new CurlMask(new Curl(CurlMode.CurlP27)),
        CurlMerkleTreeFactory.Default,
        new Curl(CurlMode.CurlP27),
        new SignatureValidator());

    /// <summary>
    /// Gets the tree factory.
    /// </summary>
    private IMerkleTreeFactory TreeFactory { get; }

    /// <summary>
    /// Gets the signature validator.
    /// </summary>
    private ISignatureValidator SignatureValidator { get; }

    /// <inheritdoc />
    public UnmaskedAuthenticatedMessage Unmask(Bundle payload, TryteString root, TryteString channelKey)
    {
      var payloadTrits = payload.Transactions.Select(t => t.Fragment).ToList().Merge().ToTrits();

      var indexData = Pascal.Decode(payloadTrits);
      var index = indexData.Item1;

      var messageData = Pascal.Decode(payloadTrits.Skip(indexData.Item2).ToArray());
      var messageLength = messageData.Item1;
      var nextRootStart = indexData.Item2 + messageData.Item2;
      var messageStart = nextRootStart + Constants.TritHashLength;
      var messageEnd = messageStart + messageLength;

      this.Curl.Reset();
      this.Curl.Absorb(channelKey.ToTrits());
      this.Curl.Absorb(root.ToTrits());
      this.Curl.Absorb(payloadTrits.Take(nextRootStart).ToArray());

      var nextRoot = this.Mask.Unmask(payloadTrits.Skip(nextRootStart).Take(Constants.TritHashLength).ToArray(), this.Curl);
      var message = this.Mask.Unmask(payloadTrits.Skip(messageStart).Take(messageLength).ToArray(), this.Curl);
      var nonce = this.Mask.Unmask(payloadTrits.Skip(messageEnd).Take(Constants.TritHashLength / 3).ToArray(), this.Curl);
      var hmac = this.Curl.Rate(Constants.TritHashLength);

      var securityLevel = new IssSigningHelper().ChecksumSecurity(hmac);

      if (securityLevel == 0)
      {
        throw new ArgumentException("Given payload is invalid. (Security level can not be verified)");
      }

      var decryptedMetadata = this.Mask.Unmask(payloadTrits.Skip(messageEnd + nonce.Length).ToArray(), this.Curl);
      var signatureEnd = (securityLevel * PrivateKey.FragmentLength) + messageEnd + nonce.Length;
      var signature = decryptedMetadata.Take(securityLevel * PrivateKey.FragmentLength).ToArray();

      var digest = new IssSigningHelper().DigestFromSignature(hmac, signature);
      this.Curl.Reset();
      this.Curl.Absorb(digest);

      var siblingsCountData = Pascal.Decode(decryptedMetadata.Skip(securityLevel * PrivateKey.FragmentLength).ToArray());
      var siblingsCount = siblingsCountData.Item1;

      if (siblingsCount != 0)
      {
        var siblings = decryptedMetadata.Skip((securityLevel * PrivateKey.FragmentLength) + siblingsCountData.Item2)
          .Take(siblingsCount * Constants.TritHashLength).ToArray();

        var recalculatedRoot = this.TreeFactory.RecalculateRoot(
          siblings,
          this.Curl.Rate(Constants.TritHashLength),
          index);

        if (recalculatedRoot.Value != root.Value)
        {
          throw new ArgumentException("Given payload is invalid. (Given root does not match payload root)");
        }
      }
      else
      {
        throw new ArgumentException("Given payload is invalid. (No siblings attached to payload)");
      }

      return new UnmaskedAuthenticatedMessage
               {
                 NextRoot = new Hash(Converter.TritsToTrytes(nextRoot)),
                 Message = new TryteString(Converter.TritsToTrytes(message)),
                 Root = new Hash(root.Value)
      };
    }
  }
}