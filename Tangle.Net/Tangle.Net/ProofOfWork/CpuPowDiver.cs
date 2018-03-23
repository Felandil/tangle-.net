namespace Tangle.Net.ProofOfWork
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;

  /// <summary>
  /// The cpu pow diver.
  /// </summary>
  public class CpuPowDiver : IPoWDiver
  {
    #region Constants

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
    /// The number of rounds.
    /// </summary>
    private const int NumberOfRounds = 81;

    /// <summary>
    /// The transactio n_ length.
    /// </summary>
    private const int TransactionLength = 8019;

    #endregion

    #region Fields

    /// <summary>
    /// The sync obj.
    /// </summary>
    private readonly object syncObj = new object();

    /// <summary>
    /// The state.
    /// </summary>
    private State state;

    #endregion

    #region Enums

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

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The cancel.
    /// </summary>
    public void Cancel()
    {
      this.state = State.Cancelled;
    }

    /// <summary>
    /// The do pow.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionTrytes"/>.
    /// </returns>
    public TransactionTrytes DoPow(TransactionTrytes trytes, int minWeightMagnitude)
    {
      var transactionTrits = trytes.ToTrits();
      this.Search(transactionTrits, minWeightMagnitude, 0);

      return new TransactionTrytes(Converter.TritsToTrytes(transactionTrits));
    }

    #endregion

    #region Methods

    /// <summary>
    /// The increment.
    /// </summary>
    /// <param name="midCurlStateCopyLow">
    /// The mid curl state copy low.
    /// </param>
    /// <param name="midCurlStateCopyHigh">
    /// The mid curl state copy high.
    /// </param>
    /// <param name="fromIndex">
    /// The from index.
    /// </param>
    /// <param name="toIndex">
    /// The to index.
    /// </param>
    private static void Increment(ulong[] midCurlStateCopyLow, ulong[] midCurlStateCopyHigh, int fromIndex, int toIndex)
    {
      for (var i = fromIndex; i < toIndex; i++)
      {
        if (midCurlStateCopyLow[i] == LowBits)
        {
          midCurlStateCopyLow[i] = HighBits;
          midCurlStateCopyHigh[i] = LowBits;
        }
        else
        {
          if (midCurlStateCopyHigh[i] == LowBits)
          {
            midCurlStateCopyHigh[i] = HighBits;
          }
          else
          {
            midCurlStateCopyLow[i] = LowBits;
          }

          break;
        }
      }
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
    private static void Transform(ulong[] curlStateLow, ulong[] curlStateHigh, ulong[] curlScratchpadLow, ulong[] curlScratchpadHigh)
    {
      int curlScratchpadIndex = 0;
      for (int round = 0; round < NumberOfRounds; round++)
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

      if (minWeightMagnitude < 0 || minWeightMagnitude > Curl.HashLength)
      {
        throw new Exception("Invalid min weight magnitude: " + minWeightMagnitude);
      }

      lock (this.syncObj)
      {
        this.state = State.Running; 
      }

      ulong[] midCurlStateLow = new ulong[Curl.StateLength], midCurlStateHigh = new ulong[Curl.StateLength];
      {
        for (var i = Curl.HashLength; i < Curl.StateLength; i++)
        {
          midCurlStateLow[i] = HighBits;
          midCurlStateHigh[i] = HighBits;
        }

        var offset = 0;
        var curlScratchpadLow = new ulong[Curl.StateLength]; 
        var curlScratchpadHigh = new ulong[Curl.StateLength];
        for (var i = (TransactionLength - Curl.HashLength) / Curl.HashLength; i-- > 0; )
        {
          for (var j = 0; j < Curl.HashLength; j++)
          {
            switch (transactionTrits[offset++])
            {
              case 0:
                midCurlStateLow[j] = HighBits;
                midCurlStateHigh[j] = HighBits;

                break;

              case 1:
                midCurlStateLow[j] = LowBits;
                midCurlStateHigh[j] = HighBits;

                break;

              default:
                midCurlStateLow[j] = HighBits;
                midCurlStateHigh[j] = LowBits;
                break;
            }
          }

          Transform(midCurlStateLow, midCurlStateHigh, curlScratchpadLow, curlScratchpadHigh);
        }

        for (int i = 0; i < 162; i++)
        {
          switch (transactionTrits[offset++])
          {
            case 0:

              midCurlStateLow[i] = HighBits;
              midCurlStateHigh[i] = HighBits;

              break;

            case 1:

              midCurlStateLow[i] = LowBits;
              midCurlStateHigh[i] = HighBits;

              break;

            default:

              midCurlStateLow[i] = HighBits;
              midCurlStateHigh[i] = LowBits;
              break;
          }
        }

        midCurlStateLow[162 + 0] = Low0;
        midCurlStateHigh[162 + 0] = High0;
        midCurlStateLow[162 + 1] = Low1;
        midCurlStateHigh[162 + 1] = High1;
        midCurlStateLow[162 + 2] = Low2;
        midCurlStateHigh[162 + 2] = High2;
        midCurlStateLow[162 + 3] = Low3;
        midCurlStateHigh[162 + 3] = High3;
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
              ulong[] midCurlStateCopyLow = new ulong[Curl.StateLength], midCurlStateCopyHigh = new ulong[Curl.StateLength];
              Array.Copy(midCurlStateLow, 0, midCurlStateCopyLow, 0, Curl.StateLength);
              Array.Copy(midCurlStateHigh, 0, midCurlStateCopyHigh, 0, Curl.StateLength);
              for (var i = threadIndex; i-- > 0;)
              {
                Increment(midCurlStateCopyLow, midCurlStateCopyHigh, 162 + (Curl.HashLength / 9), 162 + ((Curl.HashLength / 9) * 2));
              }

              ulong[] curlStateLow = new ulong[Curl.StateLength], curlStateHigh = new ulong[Curl.StateLength];
              ulong[] curlScratchpadLow = new ulong[Curl.StateLength], curlScratchpadHigh = new ulong[Curl.StateLength];
              ulong outMask = 1;
              while (this.state == State.Running)
              {
                Increment(midCurlStateCopyLow, midCurlStateCopyHigh, 162 + ((Curl.HashLength / 9) * 2), Curl.HashLength);

                Array.Copy(midCurlStateCopyLow, 0, curlStateLow, 0, Curl.StateLength);
                Array.Copy(midCurlStateCopyHigh, 0, curlStateHigh, 0, Curl.StateLength);
                Transform(curlStateLow, curlStateHigh, curlScratchpadLow, curlScratchpadHigh);

                var mask = HighBits;
                for (var i = minWeightMagnitude; i-- > 0;)
                {
                  mask &= ~(curlStateLow[Curl.HashLength - 1 - i] ^ curlStateHigh[Curl.HashLength - 1 - i]);
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

                    for (var i = 0; i < Curl.HashLength; i++)
                    {
                      transactionTrits[TransactionLength - Curl.HashLength + i] = (midCurlStateCopyLow[i] & outMask) == 0
                                                                                     ? 1
                                                                                     : (midCurlStateCopyHigh[i] & outMask) == 0 ? -1 : 0;
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

    #endregion
  }
}