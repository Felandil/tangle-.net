using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangle.Net.Entity;

namespace Tangle.Net.Mam.Mam
{
  using Tangle.Net.Cryptography;

  public class CurlMamParser : IMamParser
  {
    /// <summary>
    /// Gets the mask.
    /// </summary>
    private IMask Mask { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMamParser"/> class.
    /// </summary>
    /// <param name="mask">
    /// The mask.
    /// </param>
    public CurlMamParser(IMask mask)
    {
      this.Mask = mask;
    }

    /// <inheritdoc />
    public UnmaskedAuthenticatedMessage Unmask(Bundle payload, TryteString channelKey)
    {
      var salt = payload.Transactions[0].Tag;
      var maskedMessage = new TryteString();
      foreach (var transaction in payload.Transactions)
      {
        maskedMessage = maskedMessage.Concat(transaction.Fragment);
      }

      var unmaskedMessage = new TryteString(Converter.TritsToTrytes(this.Mask.Unmask(maskedMessage.ToTrits(), channelKey.ToTrits())));
      var signature = unmaskedMessage.GetChunk(0, Fragment.Length);
      var unmaskedMessageWithoutSignature = unmaskedMessage.GetChunk(
        Fragment.Length,
        unmaskedMessage.TrytesLength - Fragment.Length);
      var index = Converter.TritsToInt(unmaskedMessageWithoutSignature.GetChunk(0, 27).ToTrits());
      var messageHashes = unmaskedMessageWithoutSignature
        .GetChunk(27, unmaskedMessageWithoutSignature.TrytesLength - 27).GetChunks(Hash.Length);
      var nextRoot = Hash.Empty;
      var messageTrytes = new List<TryteString>();

      for (var i = 0; i < messageHashes.Count; i++)
      {
        if (messageHashes[i].Value != Hash.Empty.Value)
        {
          continue;
        }

        nextRoot = new Hash(messageHashes[i + 1].Value);
        messageTrytes.AddRange(messageHashes.Skip(i + 2).Take(messageHashes.Count - i + 2));
        break;
      }

      var chainedMessageTrytes = new TryteString();
      foreach (var messageTryte in messageTrytes)
      {
        chainedMessageTrytes = chainedMessageTrytes.Concat(messageTryte);
      }

      return new UnmaskedAuthenticatedMessage
               {
                 NextRoot = nextRoot,
                 Message = chainedMessageTrytes
      };
    }
  }
}
