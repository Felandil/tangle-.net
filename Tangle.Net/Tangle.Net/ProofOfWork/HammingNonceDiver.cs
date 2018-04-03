namespace Tangle.Net.ProofOfWork
{
  using Tangle.Net.Cryptography.Curl;

  /// <summary>
  /// The hamming nonce diver.
  /// </summary>
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
    public override ulong High0 => 13176245766935394011;

    /// <inheritdoc />
    public override ulong High1 => 14403622084951293727;

    /// <inheritdoc />
    public override ulong High2 => 18445620372817592319;

    /// <inheritdoc />
    public override ulong High3 => 2199023255551;

    /// <inheritdoc />
    public override ulong Low0 => 15811494920322472813;

    /// <inheritdoc />
    public override ulong Low1 => 17941353825114769379;

    /// <inheritdoc />
    public override ulong Low2 => 576458557575118879;

    /// <inheritdoc />
    public override ulong Low3 => 18446741876833779711;
  }
}