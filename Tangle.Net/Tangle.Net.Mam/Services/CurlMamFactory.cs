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
    /// <param name="signatureFragmentGenerator">
    /// The signature Fragment Generator.
    /// </param>
    public CurlMamFactory(AbstractCurl curl, IMask mask, ISignatureFragmentGenerator signatureFragmentGenerator)
    {
      this.SignatureFragmentGenerator = signatureFragmentGenerator;
      this.Mask = mask;
      this.Curl = curl;
    }

    /// <summary>
    /// Gets the signature fragment generator.
    /// </summary>
    private ISignatureFragmentGenerator SignatureFragmentGenerator { get; }

    /// <inheritdoc />
    public MaskedAuthenticatedMessage Create(MerkleTree tree, int index, TryteString message, Hash nextRoot, TryteString channelKey, Mode mode)
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

      var payloadMinimumLength = messageLengthTrits.Length + Constants.TritHashLength + messageTrits.Length
                                 + NonceLength + signatureLength + siblingsCountTrits.Length + siblingsLength
                                 + indexTrits.Length;

      var nextRootStart = indexTrits.Length + messageLengthTrits.Length;
      var nextRootEnd = nextRootStart + nextRootTrits.Length;

      var messageEnd = nextRootStart + nextRootTrits.Length + messageTrits.Length;
      var nonceEnd = messageEnd + NonceLength;
      var signatureEnd = nonceEnd + signatureLength;

      var siblingsCountTritsEnd = signatureEnd + siblingsCountTrits.Length;
      var siblingsEnd = siblingsCountTritsEnd + siblingsLength;

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

      // Please no look here
      var nonceDummy = new int[CpuPearlDiver.TransactionLength];
      var curlRate = this.Curl.Rate(Constants.TritHashLength);
      for (var i = 0; i < Constants.TritHashLength; i++)
      {
        nonceDummy[CpuPearlDiver.TransactionLength - Constants.TritHashLength + i] = curlRate[i];
      }
      var poW = new CpuPearlDiver(CurlMode.CurlP81).Search(nonceDummy, subtree.Key.TrytesLength / PrivateKey.FragmentLength);
      var powTrytes = new TransactionTrytes(Converter.TritsToTrytes(poW));
      var nonce = powTrytes.GetChunk<Tag>(2646, Tag.Length);
      var nonceTrits = nonce.ToTrits(); //new int[NonceLength]; // TODO
      payload.InsertRange(messageEnd, nonceTrits);

      this.Mask.Mask(nonceTrits, this.Curl);

      for (var i = 0; i < nonceTrits.Length; i++)
      {
        payload[messageEnd + i] = nonceTrits[i];
      }

      var signatureFragment = this.SignatureFragmentGenerator.Generate(
        subtree.Key,
        new Hash(Converter.TritsToTrytes(this.Curl.Rate(Constants.TritHashLength)))).Merge();

      payload.InsertRange(nonceEnd, signatureFragment.ToTrits());
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
        payload.InsertRange(payload.Count - 1, new int[Converter.Radix - nextThirdRound]);
      }

      this.Curl.Reset();
      var address = this.GetMessageAddress(tree.Root.Hash, mode);

      var bundle = new Bundle();
      bundle.AddTransfer(
        new Transfer
          {
            Address = address,
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
                 Address = address,
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