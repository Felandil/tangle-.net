namespace Tangle.Net.ProofOfWork
{
  using Tangle.Net.ProofOfWork.Entity;

  /// <summary>
  /// The abstract pearl diver.
  /// </summary>
  public abstract class AbstractPearlDiver : IPearlDiver
  {
    /// <summary>
    /// The high 0.
    /// </summary>
    public const ulong High0 = 0xB6DB6DB6DB6DB6DB;

    /// <summary>
    /// The high 1.
    /// </summary>
    public const ulong High1 = 0x8FC7E3F1F8FC7E3F;

    /// <summary>
    /// The high 2.
    /// </summary>
    public const ulong High2 = 0xFFC01FFFF803FFFF;

    /// <summary>
    /// The high 3.
    /// </summary>
    public const ulong High3 = 0x003FFFFFFFFFFFFF;

    /// <summary>
    /// The low 0.
    /// </summary>
    public const ulong Low0 = 0xDB6DB6DB6DB6DB6D;

    /// <summary>
    /// The low 1.
    /// </summary>
    public const ulong Low1 = 0xF1F8FC7E3F1F8FC7;

    /// <summary>
    /// The low 2.
    /// </summary>
    public const ulong Low2 = 0x7FFFE00FFFFC01FF;

    /// <summary>
    /// The low 3.
    /// </summary>
    public const ulong Low3 = 0xFFC0000007FFFFFF;

    /// <inheritdoc />
    public int[] Search(int[] trits, int minWeightMagnitude, int length, int offset)
    {
      return new int[] { };
    }

    /// <summary>
    /// The prepare trits.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <param name="offset">
    /// The offset.
    /// </param>
    /// <returns>
    /// The <see cref="UlongTritTouple"/>.
    /// </returns>
    protected UlongTritTouple PrepareTrits(int[] trits, int offset)
    {
      var ulongTrits = UlongTritConverter.TritsToUlong(trits);

      ulongTrits.Low[offset] = Low0;
      ulongTrits.Low[offset + 1] = Low1;
      ulongTrits.Low[offset + 2] = Low2;
      ulongTrits.Low[offset + 3] = Low3;

      ulongTrits.High[offset] = High0;
      ulongTrits.High[offset + 1] = High1;
      ulongTrits.High[offset + 2] = High2;
      ulongTrits.High[offset + 3] = High3;

      return ulongTrits;
    }
  }
}