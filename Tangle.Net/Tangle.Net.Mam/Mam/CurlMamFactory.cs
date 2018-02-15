namespace Tangle.Net.Mam.Mam
{
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Utils;

  /// <inheritdoc />
  public class CurlMamFactory : IMamFactory
  {
    /// <summary>
    /// Gets or sets the curl.
    /// </summary>
    private AbstractCurl Curl { get; set; }

    /// <summary>
    /// Gets or sets the mask.
    /// </summary>
    private IMask Mask { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMamFactory"/> class.
    /// </summary>
    /// <param name="curl">
    /// The curl.
    /// </param>
    /// <param name="mask">
    /// The mask.
    /// </param>
    public CurlMamFactory(AbstractCurl curl, IMask mask)
    {
      this.Curl = curl;
      this.Mask = mask;
    }

    /// <inheritdoc />
    public MaskedAuthenticatedMessage Create(MerkleTree tree, int index, TryteString message, Hash nextRoot, TryteString channelKey)
    {
      var keyIndex = index % tree.Size;
      var subtree = tree.GetSubtreeByIndex(keyIndex);
      var preparedSubtree = subtree.ToTryteString().Concat(Hash.Empty);

      var indexTrytes = index.ToTrytes(Hash.Length);
      var messageTrytes = nextRoot.Concat(message);
      var salt = Seed.Random().GetChunk(0, 27);
      var checksum = new Checksum("999999999");

      var bufferLength = GetBufferLength(
        messageTrytes.TrytesLength + indexTrytes.TrytesLength + preparedSubtree.TrytesLength + checksum.TrytesLength);
      messageTrytes = messageTrytes.Concat(TryteString.GetEmpty(bufferLength));

      var signature = this.CreateSignature(messageTrytes, subtree.Key);
      var messageOut = signature.Concat(indexTrytes).Concat(preparedSubtree).Concat(messageTrytes).Concat(checksum);
      var maskedMessage = this.Mask.Mask(messageOut.ToTrits(), channelKey.ToTrits());
      var address = this.GetMessageAddress(channelKey);

      var bundle = new Bundle();
      bundle.AddTransfer(
        new Transfer
          {
            Address = address,
            Message = new TryteString(Converter.TritsToTrytes(maskedMessage)),
            Tag = new Tag(salt.Value),
            Timestamp = Timestamp.UnixSecondsTimestamp
          });

      bundle.Finalize();
      bundle.Sign();

      return new MaskedAuthenticatedMessage
               {
                 Payload = bundle,
                 NextChannelKey = this.GetChannelKey(channelKey, salt)
               };
    }

    /// <summary>
    /// The get message address.
    /// </summary>
    /// <param name="channelKey">
    /// The channel key.
    /// </param>
    /// <returns>
    /// The <see cref="Address"/>.
    /// </returns>
    private Address GetMessageAddress(TryteString channelKey)
    {
      var addressHash = this.Mask.Hash(channelKey);
      addressHash = this.Mask.Hash(addressHash);

      return new Address(addressHash.Value);
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
    /// The create signature.
    /// </summary>
    /// <param name="message">
    /// The message trytes.
    /// </param>
    /// <param name="privateKey">
    /// The subtree key.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    private Fragment CreateSignature(TryteString message, IPrivateKey privateKey)
    {
      var messageHash = this.GetMessageHash(message);
      var signatureFragmentGenerator = new SignatureFragmentGenerator(privateKey as PrivateKey, messageHash);

      var fragments = signatureFragmentGenerator.Generate();
      return fragments.First();
    }

    /// <summary>
    /// The get message hash.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>
    /// The <see cref="Hash"/>.
    /// </returns>
    private Hash GetMessageHash(TryteString message)
    {
      var hash = new int[AbstractCurl.HashLength];

      this.Curl.Reset();
      this.Curl.Absorb(message.ToTrits());
      this.Curl.Squeeze(hash);

      return new Hash(Converter.TritsToTrytes(hash));
    }

    /// <summary>
    /// The get channel key.
    /// </summary>
    /// <param name="lastChannelKey">
    /// The lastChannelKey.
    /// </param>
    /// <param name="salt">
    /// The salt.
    /// </param>
    /// <returns>
    /// The <see cref="Hash"/>.
    /// </returns>
    private Hash GetChannelKey(TryteString lastChannelKey, TryteString salt)
    {
      var lastChannelKeyTrits = lastChannelKey.ToTrits();
      Converter.Increment(lastChannelKeyTrits, AbstractCurl.HashLength);
      return this.Mask.Hash(new TryteString(Converter.TritsToTrytes(lastChannelKeyTrits)), salt);
    }
  }
}