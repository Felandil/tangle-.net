namespace Tangle.Net.ProofOfWork.Entity
{
  using System;

  /// <summary>
  /// The ulong trit touple.
  /// </summary>
  public class UlongTritTouple : Tuple<ulong[], ulong[]>
  {
    /// <inheritdoc />
    public UlongTritTouple(ulong[] low, ulong[] high)
      : base(low, high)
    {
    }

    /// <summary>
    /// The high.
    /// </summary>
    public ulong[] High => this.Item2;

    /// <summary>
    /// The low.
    /// </summary>
    public ulong[] Low => this.Item1;
  }
}