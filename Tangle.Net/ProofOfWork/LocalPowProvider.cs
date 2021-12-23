namespace Tangle.Net.ProofOfWork
{
  using System;
  using System.Linq;

  using Isopoh.Cryptography.Blake2b;

  public class LocalPowProvider : IPowProvider
  {
    // ReSharper disable once InconsistentNaming
    private readonly double LN3 = 1.098612288668109691395245236922525704647490557822749451734694333;

    /// <inheritdoc />
    public long DoPow(byte[] message, int targetScore)
    {
      var relevantMessagePart = message.Take(message.Length - 8).ToArray();

      var digest = Blake2B.ComputeHash(relevantMessagePart, new Blake2BConfig { OutputSizeInBytes = 32 }, null);
      var targetZeros = (int)Math.Ceiling(Math.Log((relevantMessagePart.Length + 8) * targetScore) / this.LN3);

      return this.DoWork(digest, targetZeros);
    }

    private static int TritTrailingZeros(int[] trits)
    {
      var z = 0;
      for (var i = trits.Length - 1; i >= 0 && trits[i] == 0; i--) z++;
      return z;
    }

    private long DoWork(byte[] digest, int targetZeros)
    {
      long nonce = 0;

      var buffer = new int[AbstractCurl.HashLength];
      var hash = new int[AbstractCurl.HashLength];

      var digestTritLength = TritConverter.Encode(digest, ref buffer);

      var curl = new Curl();
      do
      {
        var nonceBuffer = BitConverter.GetBytes(nonce);
        TritConverter.Encode(nonceBuffer, ref buffer, digestTritLength);

        curl.Reset();
        curl.Absorb(buffer);
        curl.Squeeze(hash);

        if (TritTrailingZeros(hash) >= targetZeros) return nonce;
        nonce++;
      }
      while (true);
    }
  }
}