namespace Tangle.Net.Mam.Services
{
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
      var maskedMessage = payload.Transactions.Select(t => t.Fragment).ToList().Merge();
      var unmaskedMessage = this.Mask.Unmask(maskedMessage, channelKey);

      var signatureLength = 2 * Fragment.Length;
      var unmaskedMessageWithoutSignature = unmaskedMessage.GetChunk(signatureLength, unmaskedMessage.TrytesLength - signatureLength);

      var index = Converter.TritsToInt(unmaskedMessageWithoutSignature.GetChunk(0, 27).ToTrits());

      var signature = unmaskedMessage.GetChunk(0, signatureLength);
      var signatureFragments = signature.GetChunks(Fragment.Length).Select(c => new Fragment(c.Value)).ToList();

      var messageHashes = unmaskedMessageWithoutSignature.GetChunk(27, unmaskedMessageWithoutSignature.TrytesLength - 27).GetChunks(Hash.Length);
      var nextRoot = Hash.Empty;
      var treeHashes = new List<Hash>();
      var messageTrytes = new List<TryteString>();

      for (var i = 0; i < messageHashes.Count; i++)
      {
        if (messageHashes[i].Value != Hash.Empty.Value)
        {
          continue;
        }

        treeHashes = messageHashes.Take(i).Select(h => new Hash(h.Value)).ToList();
        nextRoot = new Hash(messageHashes[i + 1].Value);
        messageTrytes.AddRange(messageHashes.Skip(i + 2).Take(messageHashes.Count - i + 2));

        break;
      }

      var chainedMessageTrytes = messageTrytes.Merge();
      chainedMessageTrytes = chainedMessageTrytes.GetChunk(0, chainedMessageTrytes.TrytesLength - Checksum.Length);

      var messageHash = this.GetMessageHash(nextRoot.Concat(chainedMessageTrytes));
      if (!this.SignatureValidator.ValidateFragments(signatureFragments, messageHash, treeHashes[0]))
      {
        throw new InvalidBundleException("Mam Bundle signature failed verification", new List<string>());
      }


      var recalculatedTree = this.TreeFactory.FromBranch(treeHashes.Select(t => new MerkleNode { Hash = t }).ToList());

      return new UnmaskedAuthenticatedMessage
               {
                 NextRoot = nextRoot,
                 Message = chainedMessageTrytes,
                 Root = recalculatedTree.Root.Hash
               };
    }
  }
}