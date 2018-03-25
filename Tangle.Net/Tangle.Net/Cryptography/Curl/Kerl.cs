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
    #region Constants

    /// <summary>
    /// The bit hash length.
    /// </summary>
    public const int BitHashLength = 384;

    /// <summary>
    /// The byte hash length.
    /// </summary>
    public const int ByteHashLength = BitHashLength / 8;

    #endregion

    #region Fields

    /// <summary>
    /// The byte state.
    /// </summary>
    private readonly byte[] byteState;

    /// <summary>
    /// The digest.
    /// </summary>
    private readonly KeccakDigest digest;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Kerl"/> class.
    /// </summary>
    public Kerl()
    {
      this.State = new int[Constants.TritHashLength];
      this.byteState = new byte[ByteHashLength];
      this.digest = new KeccakDigest(BitHashLength);
    }

    #endregion

    #region Public Methods and Operators

    /// <inheritdoc />
    public override void Absorb(int[] trits)
    {
      ValidateLength(trits.Length);
      var offset = 0;

      while (offset < trits.Length)
      {
        Parallel.For(
          0,
          Constants.TritHashLength,
          (i) =>
            {
              this.State[i] = trits[offset + i];
            });

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

    /// <summary>
    /// The squeeze.
    /// </summary>
    /// <param name="trits">
    /// The checksum trits.
    /// </param>
    public override void Squeeze(int[] trits)
    {
      ValidateLength(trits.Length);
      var offset = 0;

      while (offset < trits.Length)
      {
        this.digest.DoFinal(this.byteState, 0);
        this.State = Converter.ConvertBytesToTrits(this.byteState);
        this.State[Constants.TritHashLength - 1] = 0;

        Parallel.For(
          0,
          Constants.TritHashLength,
          (i) =>
            {
              trits[offset + i] = this.State[i];
            });

        this.digest.Reset();

        Parallel.For(
          0,
          this.byteState.Length,
          (i) =>
            {
              this.byteState[i] = (byte)(this.byteState[i] ^ 0xFF);
            });

        this.digest.BlockUpdate(this.byteState, 0, this.byteState.Length);
        offset += Constants.TritHashLength;
      }
    }

    #endregion

    #region Methods

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

    #endregion
  }
}