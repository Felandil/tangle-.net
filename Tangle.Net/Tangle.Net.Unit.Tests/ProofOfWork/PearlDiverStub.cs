namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using Tangle.Net.ProofOfWork;

  /// <summary>
  /// The nonce seeker stub.
  /// </summary>
  public class PearlDiverStub : IPearlDiver
  {
    /// <inheritdoc />
    public int[] Search(int[] trits, int minWeightMagnitude)
    {
      return trits;
    }
  }
}