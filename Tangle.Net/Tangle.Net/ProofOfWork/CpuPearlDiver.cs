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
    /// The hig h_ bits.
    /// </summary>
    private const ulong HighBits = 0xFFFFFFFFFFFFFFFF;

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
    /// The lo w_ bits.
    /// </summary>
    private const ulong LowBits = 0x0000000000000000;

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

      var midCurlCollection = new ULongTritsCollection(Curl.StateLength);
      {
        for (var i = Constants.TritHashLength; i < Curl.StateLength; i++)
        {
          midCurlCollection.Low[i] = HighBits;
          midCurlCollection.High[i] = HighBits;
        }

        var offset = 0;
        var curlScratchpadLow = new ulong[Curl.StateLength]; 
        var curlScratchpadHigh = new ulong[Curl.StateLength];
        for (var i = (TransactionLength - Constants.TritHashLength) / Constants.TritHashLength; i-- > 0; )
        {
          for (var j = 0; j < Constants.TritHashLength; j++)
          {
            switch (transactionTrits[offset++])
            {
              case 0:
                midCurlCollection.Low[j] = HighBits;
                midCurlCollection.High[j] = HighBits;

                break;

              case 1:
                midCurlCollection.Low[j] = LowBits;
                midCurlCollection.High[j] = HighBits;

                break;

              default:
                midCurlCollection.Low[j] = HighBits;
                midCurlCollection.High[j] = LowBits;
                break;
            }
          }

          midCurlCollection.Transform(this.Rounds);
        }

        for (int i = 0; i < 162; i++)
        {
          switch (transactionTrits[offset++])
          {
            case 0:
              midCurlCollection.Low[i] = HighBits;
              midCurlCollection.High[i] = HighBits;
              break;
            case 1:
              midCurlCollection.Low[i] = LowBits;
              midCurlCollection.High[i] = HighBits;
              break;
            default:
              midCurlCollection.Low[i] = HighBits;
              midCurlCollection.High[i] = LowBits;
              break;
          }
        }

        midCurlCollection.Low[162 + 0] = Low0;
        midCurlCollection.High[162 + 0] = High0;
        midCurlCollection.Low[162 + 1] = Low1;
        midCurlCollection.High[162 + 1] = High1;
        midCurlCollection.Low[162 + 2] = Low2;
        midCurlCollection.High[162 + 2] = High2;
        midCurlCollection.Low[162 + 3] = Low3;
        midCurlCollection.High[162 + 3] = High3;
      }

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
              var collection = midCurlCollection.Clone();
              for (var i = threadIndex; i-- > 0;)
              {
                collection.Increment(162 + (Constants.TritHashLength / 9), 162 + ((Constants.TritHashLength / 9) * 2));
              }

              ulong[] curlScratchpadLow = new ulong[Curl.StateLength], curlScratchpadHigh = new ulong[Curl.StateLength];
              ulong outMask = 1;
              while (this.state == State.Running)
              {
                collection.Increment(162 + ((Constants.TritHashLength / 9) * 2), Constants.TritHashLength);
                var curlState = collection.Clone();

                //curlState.Transform(this.Rounds);
                this.Transform(curlState.Low, curlState.High, curlScratchpadLow, curlScratchpadHigh);

                var mask = HighBits;
                for (var i = minWeightMagnitude; i-- > 0;)
                {
                  mask &= ~(curlState.Low[Constants.TritHashLength - 1 - i] ^ curlState.High[Constants.TritHashLength - 1 - i]);
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
                      transactionTrits[TransactionLength - Constants.TritHashLength + i] = (collection.Low[i] & outMask) == 0
                                                                                     ? 1
                                                                                     : (collection.High[i] & outMask) == 0 ? -1 : 0;
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