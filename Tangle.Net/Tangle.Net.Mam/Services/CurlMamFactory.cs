namespace Tangle.Net.Mam.Services
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Utils;

  /// <inheritdoc />
  public class CurlMamFactory : AbstractMam, IMamFactory
  { 
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
      var address = this.GetMessageAddress(tree.Root.Hash, mode);

      var subtree = tree.GetSubtreeByIndex(index);
      var preparedSubtree = subtree.ToTryteString().Concat(Hash.Empty);

      var indexTrytes = index.ToTrytes(Hash.Length);
      var messageTrytes = nextRoot.Concat(message);
      var salt = Seed.Random().GetChunk(0, 27);
      var checksum = new Checksum("999999999");

      var bufferLength = GetBufferLength(
        messageTrytes.TrytesLength + indexTrytes.TrytesLength + preparedSubtree.TrytesLength + checksum.TrytesLength);
      messageTrytes = messageTrytes.Concat(TryteString.GetEmpty(bufferLength));

      var signature = this.CreateSignature(messageTrytes, subtree.Key).Merge();
      var messageOut = signature.Concat(indexTrytes).Concat(preparedSubtree).Concat(messageTrytes).Concat(checksum);

      var bundle = new Bundle();
      bundle.AddTransfer(
        new Transfer
          {
            Address = address,
            Message = this.Mask.Mask(messageOut, channelKey),
            Tag = new Tag(salt.Value),
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
    private IEnumerable<Fragment> CreateSignature(TryteString message, AbstractPrivateKey privateKey)
    {
      var messageHash = this.GetMessageHash(message);
      return this.SignatureFragmentGenerator.Generate(privateKey, messageHash);
    }
  }
}