namespace Tangle.Net.Mam.Mam
{
  using System;
  using System.Diagnostics.CodeAnalysis;

  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Utils;

  /// <inheritdoc />
  public class CurlMamFactory : IMamFactory
  {
    /// <inheritdoc />
    public MaskedAuthenticatedMessage Create(MerkleTree tree, int index, TryteString message, Hash nextRoot)
    {
      var keyIndex = index % tree.Size;
      var subtree = tree.GetSubtreeByIndex(keyIndex);
      var preparedSubtree = subtree.ToTryteString().Concat(Hash.Empty);

      var indexTrytes = index.ToTrytes(Hash.Length);
      var messageTrytes = nextRoot.Concat(message);
      var salt = Seed.Random().GetChunk(0, 27);
      var checksum = new Checksum("999999999");

      var bufferLength = GetBufferLength(messageTrytes.TrytesLength + indexTrytes.TrytesLength + preparedSubtree.TrytesLength + checksum.TrytesLength);
      messageTrytes = messageTrytes.Concat(TryteString.GetEmpty(bufferLength));

      return new MaskedAuthenticatedMessage();
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
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
    private static int GetBufferLength(int length)
    {
      // ReSharper disable once PossibleLossOfFraction
      return (int)(Fragment.Length - (length - Math.Floor((decimal)(length / Fragment.Length)) * Fragment.Length));
    }
  }
}