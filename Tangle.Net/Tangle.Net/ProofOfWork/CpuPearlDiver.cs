namespace Tangle.Net.ProofOfWork
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The cpu pow diver.
  /// </summary>
  public class CpuPearlDiver : IPearlDiver
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CpuPearlDiver"/> class.
    /// </summary>
    public CpuPearlDiver()
    {
      this.Rounds = (int)CurlMode.CurlP81;
    }

    /// <summary>
    /// Gets or sets the rounds.
    /// </summary>
    public int Rounds { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CpuPearlDiver"/> class.
    /// </summary>
    /// <param name="mode">
    /// The mode.
    /// </param>
    public CpuPearlDiver(CurlMode mode)
    {
      this.Rounds = (int)mode;
    }

    /// <summary>
    /// The hig h_0.
    /// </summary>
    private const ulong High0 = 0xB6DB6DB6DB6DB6DB;

    /// <summary>
    /// The hig h_1.
    /// </summary>
    private const ulong High1 = 0x8FC7E3F1F8FC7E3F;

    /// <summary>
    /// The hig h_2.
    /// </summary>
    private const ulong High2 = 0xFFC01FFFF803FFFF;

    /// <summary>
    /// The hig h_3.
    /// </summary>
    private const ulong High3 = 0x003FFFFFFFFFFFFF;

    /// <summary>
    /// The lo w_0.
    /// </summary>
    private const ulong Low0 = 0xDB6DB6DB6DB6DB6D;

    /// <summary>
    /// The lo w_1.
    /// </summary>
    private const ulong Low1 = 0xF1F8FC7E3F1F8FC7;

    /// <summary>
    /// The lo w_2.
    /// </summary>
    private const ulong Low2 = 0x7FFFE00FFFFC01FF;

    /// <summary>
    /// The lo w_3.
    /// </summary>
    private const ulong Low3 = 0xFFC0000007FFFFFF;

    /// <summary>
    /// The transactio n_ length.
    /// </summary>
    public const int TransactionLength = 8019;

    /// <summary>
    /// The sync obj.
    /// </summary>
    private readonly object syncObj = new object();

    /// <summary>
    /// The state.
    /// </summary>
    private State state;


    /// <summary>
    /// The state.
    /// </summary>
    private enum State
    {
      /// <summary>
      /// The running.
      /// </summary>
      Running, 

      /// <summary>
      /// The cancelled.
      /// </summary>
      Cancelled, 

      /// <summary>
      /// The completed.
      /// </summary>
      Completed
    }

    /// <summary>
    /// The cancel.
    /// </summary>
    public void Cancel()
    {
      this.state = State.Cancelled;
    }

    /// <inheritdoc />
    public int[] Search(int[] trits, int minWeightMagnitude)
    {
      this.Search(trits, minWeightMagnitude, 0);
      return trits;
    }

    /// <summary>
    /// The transform.
    /// </summary>
    /// <param name="curlStateLow">
    /// The curl state low.
    /// </param>
    /// <param name="curlStateHigh">
    /// The curl state high.
    /// </param>
    /// <param name="curlScratchpadLow">
    /// The curl scratchpad low.
    /// </param>
    /// <param name="curlScratchpadHigh">
    /// The curl scratchpad high.
    /// </param>
    private void Transform(ulong[] curlStateLow, ulong[] curlStateHigh, ulong[] curlScratchpadLow, ulong[] curlScratchpadHigh)
    {
      var curlScratchpadIndex = 0;
      for (var round = 0; round < this.Rounds; round++)
      {
        Array.Copy(curlStateLow, 0, curlScratchpadLow, 0, Curl.StateLength);
        Array.Copy(curlStateHigh, 0, curlScratchpadHigh, 0, Curl.StateLength);

        for (var curlStateIndex = 0; curlStateIndex < Curl.StateLength; curlStateIndex++)
        {
          var alpha = curlScratchpadLow[curlScratchpadIndex];
          var beta = curlScratchpadHigh[curlScratchpadIndex];
          if (curlScratchpadIndex < 365)
          {
            curlScratchpadIndex += 364;
          }
          else
          {
            curlScratchpadIndex += -365;
          }

          ulong gamma = curlScratchpadHigh[curlScratchpadIndex];
          ulong delta = (alpha | (~gamma)) & (curlScratchpadLow[curlScratchpadIndex] ^ beta);

          curlStateLow[curlStateIndex] = ~delta;
          curlStateHigh[curlStateIndex] = (alpha ^ gamma) | delta;
        }
      }
    }

    /// <summary>
    /// The search.
    /// </summary>
    /// <param name="transactionTrits">
    /// The transaction trits.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <param name="numberOfThreads">
    /// The number of threads.
    /// </param>
    private void Search(int[] transactionTrits, int minWeightMagnitude, int numberOfThreads)
    {
      if (transactionTrits.Length != TransactionLength)
      {
        throw new Exception("Invalid transaction trits length: " + transactionTrits.Length);
      }

      if (minWeightMagnitude < 0 || minWeightMagnitude > Constants.TritHashLength)
      {
        throw new Exception("Invalid min weight magnitude: " + minWeightMagnitude);
      }

      lock (this.syncObj)
      {
        this.state = State.Running; 
      }

      var nonceCurl = new NonceCurl(Curl.StateLength, this.Rounds);
      nonceCurl.Init(Constants.TritHashLength, Curl.StateLength);

      var offset = 0;
      for (var i = (TransactionLength - Constants.TritHashLength) / Constants.TritHashLength; i-- > 0; )
      {
        offset = nonceCurl.Absorb(transactionTrits, Constants.TritHashLength, offset);
        nonceCurl.Transform();
      }

      nonceCurl.Absorb(transactionTrits, 162, offset);

      nonceCurl.Low[162 + 0] = Low0;
      nonceCurl.High[162 + 0] = High0;
      nonceCurl.Low[162 + 1] = Low1;
      nonceCurl.High[162 + 1] = High1;
      nonceCurl.Low[162 + 2] = Low2;
      nonceCurl.High[162 + 2] = High2;
      nonceCurl.Low[162 + 3] = Low3;
      nonceCurl.High[162 + 3] = High3;


      if (numberOfThreads <= 0)
      {
        // Use one thread for each processor on the system except one
        // We must ensure we use at least 1
        numberOfThreads = Math.Max(Environment.ProcessorCount - 1, 1);
      }

      var tasks = new List<Task>();

      while (numberOfThreads-- > 0)
      {
        var threadIndex = numberOfThreads;
        var task = Task.Factory.StartNew(
          () =>
            {
              var nonceThreadCurl = nonceCurl.Clone();
              for (var i = threadIndex; i-- > 0;)
              {
                nonceThreadCurl.Increment(162 + (Constants.TritHashLength / 9), 162 + ((Constants.TritHashLength / 9) * 2));
              }

              var curlScratchpadLow = new ulong[Curl.StateLength];
              var curlScratchpadHigh = new ulong[Curl.StateLength];

              ulong outMask = 1;
              while (this.state == State.Running)
              {
                nonceThreadCurl.Increment(162 + ((Constants.TritHashLength / 9) * 2), Constants.TritHashLength);
                var threadCurlClone = nonceThreadCurl.Clone();

                // it is significantly faster do do the transform process in here. (only csharp things)
                this.Transform(threadCurlClone.Low, threadCurlClone.High, curlScratchpadLow, curlScratchpadHigh);

                var mask = NonceCurl.Max;
                for (var i = minWeightMagnitude; i-- > 0;)
                {
                  mask &= ~(threadCurlClone.Low[Constants.TritHashLength - 1 - i] ^ threadCurlClone.High[Constants.TritHashLength - 1 - i]);
                  if (mask == 0)
                  {
                    break;
                  }
                }

                if (mask == 0)
                {
                  continue;
                }

                lock (this.syncObj)
                {
                  if (this.state == State.Running)
                  {
                    this.state = State.Completed;
                    while ((outMask & mask) == 0)
                    {
                      outMask <<= 1;
                    }

                    for (var i = 0; i < Constants.TritHashLength; i++)
                    {
                      transactionTrits[TransactionLength - Constants.TritHashLength + i] = (nonceThreadCurl.Low[i] & outMask) == 0
                                                                                     ? 1
                                                                                     : (nonceThreadCurl.High[i] & outMask) == 0 ? -1 : 0;
                    }
                  }
                }

                break;
              }
            });
        tasks.Add(task);
      }

      Task.WaitAll(tasks.ToArray());
    }
  }
}