namespace Tangle.Net.Cryptography
{
  using System;

  using Org.BouncyCastle.Crypto.Digests;

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
      this.State = new int[HashLength];
      this.byteState = new byte[ByteHashLength];
      this.digest = new KeccakDigest(BitHashLength);
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The absorb.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    public override void Absorb(int[] trits)
    {
      ValidateLength(trits.Length);
      var offset = 0;

      while (offset < trits.Length)
      {
        Array.Copy(trits, offset, this.State, 0, HashLength);
        this.State[HashLength - 1] = 0;

        var bytes = Converter.ConvertTritsToBytes(this.State);
        this.digest.BlockUpdate(bytes, 0, bytes.Length);

        offset += HashLength;
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
        this.State[HashLength - 1] = 0;
        Array.Copy(this.State, 0, trits, offset, HashLength);

        this.digest.Reset();

        for (var i = this.byteState.Length; i-- > 0;)
        {
          this.byteState[i] = (byte)(this.byteState[i] ^ 0xFF);
        }

        this.digest.BlockUpdate(this.byteState, 0, this.byteState.Length);
        offset += HashLength;
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
      if (length % HashLength != 0)
      {
        throw new ArgumentException("Illegal length provided'.");
      }
    }

    #endregion
  }
}