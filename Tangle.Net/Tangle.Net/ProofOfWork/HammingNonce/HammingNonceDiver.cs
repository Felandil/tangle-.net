namespace Tangle.Net.ProofOfWork.HammingNonce
{
  using Tangle.Net.Cryptography.Curl;

  public class HammingNonceDiver : AbstractPearlDiver
  {
    /// <inheritdoc />
    public HammingNonceDiver()
      : base(CurlMode.CurlP81)
    {
    }

    /// <inheritdoc />
    public HammingNonceDiver(CurlMode mode)
      : base(mode)
    {
    }

    /// <inheritdoc />
    public override ulong High0 => 0xB6DB6DB6DB6DB6DB;

    /// <inheritdoc />
    public override ulong High1 => 0x8FC7E3F1F8FC7E3F;

    /// <inheritdoc />
    public override ulong High2 => 0xFFC01FFFF803FFFF;

    /// <inheritdoc />
    public override ulong High3 => 0x003FFFFFFFFFFFFF;

    /// <inheritdoc />
    public override ulong Low0 => 0xDB6DB6DB6DB6DB6D;

    /// <inheritdoc />
    public override ulong Low1 => 0xF1F8FC7E3F1F8FC7;

    /// <inheritdoc />
    public override ulong Low2 => 0x7FFFE00FFFFC01FF;

    /// <inheritdoc />
    public override ulong Low3 => 0xFFC0000007FFFFFF;
  }
}