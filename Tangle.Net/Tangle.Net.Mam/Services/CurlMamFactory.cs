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
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.ProofOfWork.HammingNonce;
  using Tangle.Net.Utils;

  using Mode = Tangle.Net.ProofOfWork.HammingNonce.Mode;

  /// <inheritdoc cref="IMamFactory"/>
  public class CurlMamFactory : IMamFactory
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
        new HammingNonceDiver(CurlMode.CurlP27, Mode._32bit));

    /// <summary>
    /// Gets the hamming nonce diver.
    /// </summary>
    private AbstractPearlDiver HammingNonceDiver { get; }

    /// <summary>
    /// Gets the signature fragment generator.
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
    public MaskedAuthenticatedMessage Create(
      MerkleTree tree,
      int index,
      TryteString message,
      Hash nextRoot,
      TryteString channelKey,
      Entity.Mode mode,
      int securityLevel)
    {
      var nextRootTrits = nextRoot.ToTrits();

      var messageTrits = message.ToTrits();
      var indexTrits = Pascal.Encode(index);
      var messageLengthTrits = Pascal.Encode(messageTrits.Length);

      var subtree = tree.GetSubtreeByIndex(index);

      this.Curl.Reset();
      this.Curl.Absorb(channelKey.ToTrits());
      this.Curl.Absorb(tree.Root.Hash.ToTrits());

      var payload = new List<int>();
      payload.InsertRange(0, indexTrits);
      payload.InsertRange(indexTrits.Length, messageLengthTrits);

      var nextRootStart = indexTrits.Length + messageLengthTrits.Length;
      this.Curl.Absorb(payload.Take(nextRootStart).ToArray());

      // encrypt next root together with message trits
      payload.InsertRange(nextRootStart, this.Mask.Mask(nextRootTrits.Concat(messageTrits).ToArray(), this.Curl));

      // calculate message end and add nonce
      var messageEnd = nextRootStart + nextRootTrits.Length + messageTrits.Length;
      this.AddNonce(securityLevel, messageEnd, payload);

      // create signature, encrypt signature + sibling count (get trits from pascal) + siblings
      var signature = this.SigningHelper.Signature(this.Curl.Rate(Constants.TritHashLength), subtree.Key.ToTrits());
      var subtreeTrits = subtree.ToTryteString().ToTrits();
      var siblingsCount = subtreeTrits.Length / Constants.TritHashLength;
      var encryptedSignature = this.Mask.Mask(signature.Concat(Pascal.Encode(siblingsCount)).Concat(subtreeTrits).ToArray(), this.Curl);

      // insert signature and pad to correct length (% 3 == 0)
      payload.InsertRange(messageEnd + NonceLength, encryptedSignature);
      PadPayload(payload);

      this.Curl.Reset();

      var messageAddress = this.GetMessageAddress(tree.Root.Hash, mode);
      return new MaskedAuthenticatedMessage
               {
                 Payload = CreateBundleFromPayload(messageAddress, payload),
                 Root = tree.Root.Hash,
                 Address = messageAddress,
                 NextRoot = nextRoot
               };
    }

    /// <summary>
    /// The pad payload.
    /// </summary>
    /// <param name="payload">
    /// The payload.
    /// </param>
    private static void PadPayload(List<int> payload)
    {
      var nextThirdRound = payload.Count % Converter.Radix;
      if (nextThirdRound != 0)
      {
        payload.InsertRange(payload.Count, new int[Converter.Radix - nextThirdRound]);
      }
    }

    /// <summary>
    /// The create bundle from payload.
    /// </summary>
    /// <param name="messageAddress">
    /// The message address.
    /// </param>
    /// <param name="payload">
    /// The payload.
    /// </param>
    /// <returns>
    /// The <see cref="Bundle"/>.
    /// </returns>
    private static Bundle CreateBundleFromPayload(Address messageAddress, List<int> payload)
    {
      var bundle = new Bundle();
      bundle.AddTransfer(
        new Transfer
          {
            Address = messageAddress,
            Message = new TryteString(Converter.TritsToTrytes(payload.ToArray())),
            Timestamp = Timestamp.UnixSecondsTimestamp
          });

      bundle.Finalize();
      bundle.Sign();

      return bundle;
    }

    /// <summary>
    /// The add nonce.
    /// </summary>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="messageEnd">
    /// The message end.
    /// </param>
    /// <param name="payload">
    /// The payload.
    /// </param>
    private void AddNonce(int securityLevel, int messageEnd, List<int> payload)
    {
      var nonceTrits = this.HammingNonceDiver.Search(this.Curl.Rate(this.Curl.State.Length), securityLevel, Constants.TritHashLength / 3, 0)
        .ToArray();
      this.Mask.Mask(nonceTrits, this.Curl);
      payload.InsertRange(messageEnd, nonceTrits);
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
    private Address GetMessageAddress(TryteString rootHash, Entity.Mode mode)
    {
      if (mode == Entity.Mode.Public)
      {
        return new Address(rootHash.Value);
      }

      var addressHash = this.Mask.Hash(rootHash);
      return new Address(addressHash.Value);
    }
  }
}