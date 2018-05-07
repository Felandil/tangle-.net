namespace Tangle.Net.Cryptography.Curl
{
  using System;
  using System.Linq;
  using System.Threading.Tasks;

  using Org.BouncyCastle.Crypto.Digests;

  using Tangle.Net.Utils;

  /// <summary>
  /// The kerl.
  /// </summary>
  public class Kerl : AbstractCurl
  {
    /// <summary>
    /// The bit hash length.
    /// </summary>
    public const int BitHashLength = 384;

    /// <summary>
    /// The byte hash length.
    /// </summary>
    public const int ByteHashLength = BitHashLength / 8;

    /// <summary>
    /// The digest.
    /// </summary>
    private readonly KeccakDigest digest;


    /// <summary>
    /// Initializes a new instance of the <see cref="Kerl"/> class.
    /// </summary>
    public Kerl()
    {
      this.State = new int[Constants.TritHashLength];
      this.digest = new KeccakDigest(BitHashLength);
    }

    /// <inheritdoc />
    public override void Absorb(int[] trits)
    {
      ValidateLength(trits.Length);
      var offset = 0;

      while (offset < trits.Length)
      {
        this.State = trits.Skip(offset).Take(Constants.TritHashLength).ToArray();
        this.State[Constants.TritHashLength - 1] = 0;

        var bytes = Converter.ConvertTritsToBytes(this.State);
        this.digest.BlockUpdate(bytes, 0, bytes.Length);

        offset += Constants.TritHashLength;
      }
    }

    /// <summary>
    /// The reset.
    /// </summary>
    public override void Reset()
    {
      this.digest.Reset();
    }

    /// <inheritdoc />
    public override void Squeeze(int[] trits)
    {
      ValidateLength(trits.Length);
      var offset = 0;
      var byteState = new byte[ByteHashLength];

      while (offset < trits.Length)
      {
        this.digest.DoFinal(byteState, 0);
        this.State = Converter.ConvertBytesToTrits(byteState);
        this.State[Constants.TritHashLength - 1] = 0;

        for (var i = 0; i < Constants.TritHashLength; i++)
        {
          trits[offset + i] = this.State[i];
        }

        this.digest.Reset();

        for (var i = byteState.Length; i-- > 0;)
        {
          byteState[i] = (byte)(byteState[i] ^ 0xFF);
        }

        this.digest.BlockUpdate(byteState, 0, byteState.Length);
        offset += Constants.TritHashLength;
      }
    }

    /// <summary>
    /// The validate length.
    /// </summary>
    /// <param name="length">
    /// The length.
    /// </param>
    private static void ValidateLength(int length)
    {
      if (length % Constants.TritHashLength != 0)
      {
        throw new ArgumentException("Illegal length provided'.");
      }
    }
  }
}