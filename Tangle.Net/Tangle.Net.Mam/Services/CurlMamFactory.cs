namespace Tangle.Net.Mam.Services
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Utils;

  /// <inheritdoc cref="AbstractMam"/>
  /// <inheritdoc cref="IMamFactory"/>
  public class CurlMamFactory : AbstractMam, IMamFactory
  {
    /// <summary>
    /// The nonce length.
    /// </summary>
    private const int NonceLength = Constants.TritHashLength / Converter.Radix;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMamFactory"/> class.
    /// </summary>
    /// <param name="curl">
    /// The curl.
    /// </param>
    /// <param name="mask">
    /// The mask.
    /// </param>
    /// <param name="signingHelper">
    /// The signing helper.
    /// </param>
    /// <param name="hammingNonceDiver">
    /// The hamming nonce diver.
    /// </param>
    public CurlMamFactory(AbstractCurl curl, IMask mask, ISigningHelper signingHelper, AbstractPearlDiver hammingNonceDiver)
    {
      this.SigningHelper = signingHelper;
      this.HammingNonceDiver = hammingNonceDiver;
      this.Mask = mask;
      this.Curl = curl;
    }

    /// <summary>
    /// The default.
    /// </summary>
    public static CurlMamFactory Default =>
      new CurlMamFactory(
        new Curl(CurlMode.CurlP27),
        new CurlMask(),
        new IssSigningHelper(new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27)),
        new HammingNonceDiver(CurlMode.CurlP27));

    /// <summary>
    /// Gets the signature fragment generator.
    /// </summary>
    private ISigningHelper SigningHelper { get; }

    /// <summary>
    /// Gets the hamming nonce diver.
    /// </summary>
    private AbstractPearlDiver HammingNonceDiver { get; }

    /// <inheritdoc />
    public MaskedAuthenticatedMessage Create(MerkleTree tree, int index, TryteString message, Hash nextRoot, TryteString channelKey, Mode mode, int securityLevel)
    {
      var nextRootTrits = nextRoot.ToTrits();

      var messageTrits = message.ToTrits();
      var indexTrits = Pascal.Encode(index);
      var messageLengthTrits = Pascal.Encode(messageTrits.Length);

      var subtree = tree.GetSubtreeByIndex(index);
      var subtreeTrytes = subtree.ToTryteString();
      var subtreeTrits = subtreeTrytes.ToTrits();

      var siblingsLength = subtreeTrits.Length;
      var siblingsCount = siblingsLength / Constants.TritHashLength;
      var siblingsCountTrits = Pascal.Encode(siblingsCount);
      var signatureLength = subtree.Key.TrytesLength * Converter.Radix;

      var nextRootStart = indexTrits.Length + messageLengthTrits.Length;
      var nextRootEnd = nextRootStart + nextRootTrits.Length;

      var messageEnd = nextRootStart + nextRootTrits.Length + messageTrits.Length;
      var nonceEnd = messageEnd + NonceLength;
      var signatureEnd = nonceEnd + signatureLength;

      var siblingsCountTritsEnd = signatureEnd + siblingsCountTrits.Length;

      this.Curl.Reset();
      this.Curl.Absorb(channelKey.ToTrits());
      this.Curl.Absorb(tree.Root.Hash.ToTrits());

      var payload = new List<int>();
      payload.InsertRange(0, indexTrits);
      payload.InsertRange(indexTrits.Length, messageLengthTrits);

      this.Curl.Absorb(payload.Take(nextRootStart).ToArray());

      payload.InsertRange(nextRootStart, nextRootTrits);
      payload.InsertRange(nextRootEnd, messageTrits);

      var encryptablePayloadPart = payload.Skip(nextRootStart).Take(nextRootTrits.Length + messageTrits.Length).ToArray();
      this.Mask.Mask(encryptablePayloadPart, this.Curl);

      for (var i = 0; i < encryptablePayloadPart.Length; i++)
      {
        payload[nextRootStart + i] = encryptablePayloadPart[i];
      }

      var nonceTrits = this.HammingNonceDiver.Search(this.Curl.Rate(this.Curl.State.Length), securityLevel, Constants.TritHashLength / 3, 0)
        .ToArray();
      this.Mask.Mask(nonceTrits, this.Curl);
      payload.InsertRange(messageEnd, nonceTrits);

      var signature = this.SigningHelper.Signature(this.Curl.Rate(Constants.TritHashLength), subtree.Key.ToTrits());

      payload.InsertRange(nonceEnd, signature);
      payload.InsertRange(signatureEnd, siblingsCountTrits);
      payload.InsertRange(siblingsCountTritsEnd, subtreeTrits);

      var encryptableSignaturePart = payload.Skip(nonceEnd).ToArray();
      this.Mask.Mask(encryptableSignaturePart, this.Curl);

      for (var i = 0; i < encryptableSignaturePart.Length; i++)
      {
        payload[nonceEnd + i] = encryptableSignaturePart[i];
      }

      var nextThirdRound = payload.Count % Converter.Radix;
      if (nextThirdRound != 0)
      {
        payload.InsertRange(payload.Count, new int[Converter.Radix - nextThirdRound]);
      }

      this.Curl.Reset();

      var bundle = new Bundle();
      bundle.AddTransfer(
        new Transfer
          {
            Address = this.GetMessageAddress(tree.Root.Hash, mode),
            Message = new TryteString(Converter.TritsToTrytes(payload.ToArray())),
            Tag = new Tag("999999"),
            Timestamp = Timestamp.UnixSecondsTimestamp
          });

      bundle.Finalize();
      bundle.Sign();

      return new MaskedAuthenticatedMessage
               {
                 Payload = bundle,
                 Root = tree.Root.Hash,
                 Address = this.GetMessageAddress(tree.Root.Hash, mode),
                 NextRoot = nextRoot
               };
    }

    /// <summary>
    /// The get buffer length.
    /// </summary>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    [SuppressMessage(
      "StyleCop.CSharp.MaintainabilityRules",
      "SA1407:ArithmeticExpressionsMustDeclarePrecedence",
      Justification = "Reviewed. Suppression is OK here.")]
    private static int GetBufferLength(int length)
    {
      // ReSharper disable once PossibleLossOfFraction
      return (int)(Fragment.Length - (length - Math.Floor((decimal)(length / Fragment.Length)) * Fragment.Length));
    }

    /// <summary>
    /// The get message address.
    /// </summary>
    /// <param name="rootHash">
    /// The root Hash.
    /// </param>
    /// <param name="mode">
    /// The mode.
    /// </param>
    /// <returns>
    /// The <see cref="Address"/>.
    /// </returns>
    private Address GetMessageAddress(TryteString rootHash, Mode mode)
    {
      if (mode == Mode.Public)
      {
        return new Address(rootHash.Value);
      }

      var addressHash = this.Mask.Hash(rootHash);
      return new Address(addressHash.Value);
    }
  }
}