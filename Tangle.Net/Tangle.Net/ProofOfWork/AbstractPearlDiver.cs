namespace Tangle.Net.ProofOfWork
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Cryptography.Curl;

  /// <summary>
  /// The abstract pearl diver.
  /// </summary>
  public abstract class AbstractPearlDiver : IPearlDiver
  {
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
    /// The sync obj.
    /// </summary>
    private readonly object syncObj = new object();

    /// <summary>
    /// The state.
    /// </summary>
    private State state;


    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPearlDiver"/> class.
    /// </summary>
    protected AbstractPearlDiver()
    {
      this.Rounds = (int)CurlMode.CurlP81;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPearlDiver"/> class. 
    /// </summary>
    /// <param name="mode">
    /// The mode.
    /// </param>
    protected AbstractPearlDiver(CurlMode mode)
    {
      this.Rounds = (int)mode;
    }

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
    /// Gets or sets the trit state.
    /// </summary>
    protected int[] TritState { get; set; }

    /// <summary>
    /// Gets or sets the offset.
    /// </summary>
    private int Offset { get; set; }

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    private int Size { get; set; }

    /// <summary>
    /// Gets or sets the rounds.
    /// </summary>
    private int Rounds { get; set; }

    /// <summary>
    /// The cancel.
    /// </summary>
    public void Cancel()
    {
      this.state = State.Cancelled;
    }

    /// <inheritdoc />
    public int[] Search(int[] trits, int minWeightMagnitude, int offset, int size)
    {
      this.Offset = offset;
      this.Size = size;
      this.TritState = trits;
      this.Search(minWeightMagnitude, 0);
      return this.TritState;
    }

    /// <summary>
    /// The demux.
    /// </summary>
    /// <param name="curl">
    /// The curl.
    /// </param>
    /// <param name="mask">
    /// The mask.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    protected int[] Demux(NonceCurl curl, ulong mask)
    {
      var length = this.TritState.Length;
      var demuxedState = new int[this.Size];

      for (var i = 0; i < this.Size; i++)
      {
        this.TritState[length - this.Size + i] = (curl.Low[i] & mask) == 0 ? 1 : (curl.High[i] & mask) == 0 ? -1 : 0;
        demuxedState[i] = this.TritState[length - this.Size + i];
      }

      return demuxedState;
    }

    /// <summary>
    /// The check.
    /// </summary>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <param name="curl">
    /// The thread curl clone.
    /// </param>
    /// <returns>
    /// The <see cref="ulong"/>.
    /// </returns>
    protected abstract ulong Check(int minWeightMagnitude, NonceCurl curl);

    /// <summary>
    /// The search.
    /// </summary>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <param name="numberOfThreads">
    /// The number of threads.
    /// </param>
    private void Search(int minWeightMagnitude, int numberOfThreads)
    {
      if (minWeightMagnitude < 0 || minWeightMagnitude > this.Size)
      {
        throw new Exception("Invalid min weight magnitude: " + minWeightMagnitude);
      }

      lock (this.syncObj)
      {
        this.state = State.Running;
      }

      var nonceCurl = new NonceCurl(Curl.StateLength, this.Rounds);
      nonceCurl.Init(this.Size, Curl.StateLength);

      var offset = 0;
      var nonceCurlScratchpadLow = new ulong[Curl.StateLength];
      var nonceCurlScratchpadHigh = new ulong[Curl.StateLength];
      for (var i = (this.TritState.Length - this.Size) / this.Size; i-- > 0;)
      {
        offset = nonceCurl.Absorb(this.TritState, this.Size, offset);
        this.Transform(nonceCurl.Low, nonceCurl.High, nonceCurlScratchpadLow, nonceCurlScratchpadHigh);
      }

      nonceCurl.Absorb(this.TritState, this.Offset, offset);

      nonceCurl.Low[this.Offset + 0] = Low0;
      nonceCurl.High[this.Offset + 0] = High0;
      nonceCurl.Low[this.Offset + 1] = Low1;
      nonceCurl.High[this.Offset + 1] = High1;
      nonceCurl.Low[this.Offset + 2] = Low2;
      nonceCurl.High[this.Offset + 2] = High2;
      nonceCurl.Low[this.Offset + 3] = Low3;
      nonceCurl.High[this.Offset + 3] = High3;

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
                nonceThreadCurl.Increment(this.Offset + (this.Size / 9), this.Offset + ((this.Size / 9) * 2));
              }

              var curlScratchpadLow = new ulong[Curl.StateLength];
              var curlScratchpadHigh = new ulong[Curl.StateLength];

              while (this.state == State.Running)
              {
                nonceThreadCurl.Increment(this.Offset + ((this.Size / 9) * 2), this.Size);
                var threadCurlClone = nonceThreadCurl.Clone();

                this.Transform(threadCurlClone.Low, threadCurlClone.High, curlScratchpadLow, curlScratchpadHigh);

                var mask = this.Check(minWeightMagnitude, threadCurlClone);
                if (mask == 0)
                {
                  continue;
                }

                lock (this.syncObj)
                {
                  if (this.state == State.Running)
                  {
                    this.state = State.Completed;
                    this.Demux(nonceThreadCurl, mask);
                  }
                }

                break;
              }
            });
        tasks.Add(task);
      }

      Task.WaitAll(tasks.ToArray());
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

          var gamma = curlScratchpadHigh[curlScratchpadIndex];
          var delta = (alpha | ~gamma) & (curlScratchpadLow[curlScratchpadIndex] ^ beta);

          curlStateLow[curlStateIndex] = ~delta;
          curlStateHigh[curlStateIndex] = (alpha ^ gamma) | delta;
        }
      }
    }
  }
}