namespace Tangle.Net.ProofOfWork.HammingNonce
{
  using Tangle.Net.Cryptography.Curl;

  /// <summary>
  /// The hamming nonce diver.
  /// </summary>
  public class HammingNonceDiver : AbstractPearlDiver
  {
    private Mode ScratchMode { get; }

    /// <inheritdoc />
    public HammingNonceDiver()
      : base(CurlMode.CurlP81)
    {
    }

    /// <inheritdoc />
    public HammingNonceDiver(CurlMode mode, Mode scratchMode)
      : base(mode)
    {
      this.ScratchMode = scratchMode;
    }

    /// <inheritdoc />
    public override ulong High0 => this.ScratchMode == Mode._32bit ? 1533916891 : 13176245766935394011;

    /// <inheritdoc />
    public override ulong High1 => this.ScratchMode == Mode._32bit ? 1676802300 : 14403622084951293727;

    /// <inheritdoc />
    public override ulong High2 => this.ScratchMode == Mode._32bit ? 2147352831 : 18445620372817592319;

    /// <inheritdoc />
    public override ulong High3 => this.ScratchMode == Mode._32bit ? 256 : (ulong)2199023255551;

    /// <inheritdoc />
    public override ulong Low0 => this.ScratchMode == Mode._32bit ? 1840700269 : 15811494920322472813;

    /// <inheritdoc />
    public override ulong Low1 => this.ScratchMode == Mode._32bit ? 2088648479 : 17941353825114769379;

    /// <inheritdoc />
    public override ulong Low2 => this.ScratchMode == Mode._32bit ? 67108608 : (ulong)576458557575118879;

    /// <inheritdoc />
    public override ulong Low3 => this.ScratchMode == Mode._32bit ? 2147483391 : 18446741876833779711;
  }
}

//18446744073709551615				2147483647
//H
//0	13176245766935394011			1533916891
//1	14403622084951293727			1676802300	
//2	18445620372817592319			2147352831	
//3	2199023255551				      256	
//L
//0	15811494920322472813			1840700269	
//1	17941353825114769379			2088648479	
//2	576458557575118879			  67108608
//3	18446741876833779711			2147483391	