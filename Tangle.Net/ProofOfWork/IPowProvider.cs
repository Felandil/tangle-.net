namespace Tangle.Net.ProofOfWork
{
  public interface IPowProvider
  {
    long DoPow(byte[] message, int targetScore);
  }
}