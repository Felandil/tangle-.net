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
    protected virtual ulong High0 => 0xB6DB6DB6DB6DB6DB;
    protected virtual ulong High1 => 0x8FC7E3F1F8FC7E3F;
    protected virtual ulong High2 => 0xFFC01FFFF803FFFF;
    protected virtual ulong High3 => 0x003FFFFFFFFFFFFF;
    protected virtual ulong Low0 => 0xDB6DB6DB6DB6DB6D;
    protected virtual ulong Low1 => 0xF1F8FC7E3F1F8FC7;
    protected virtual ulong Low2 => 0x7FFFE00FFFFC01FF;
    protected virtual ulong Low3 => 0xFFC0000007FFFFFF;

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

      nonceCurl.Low[this.Offset + 0] = this.Low0;
      nonceCurl.High[this.Offset + 0] = this.High0;
      nonceCurl.Low[this.Offset + 1] = this.Low1;
      nonceCurl.High[this.Offset + 1] = this.High1;
      nonceCurl.Low[this.Offset + 2] = this.Low2;
      nonceCurl.High[this.Offset + 2] = this.High2;
      nonceCurl.Low[this.Offset + 3] = this.Low3;
      nonceCurl.High[this.Offset + 3] = this.High3;

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
                    for (var i = 0; i < this.Size; i++)
                    {
                      this.TritState[this.TritState.Length - this.Size + i] = (nonceThreadCurl.Low[i] & mask) == 0 ? 1 : (nonceThreadCurl.High[i] & mask) == 0 ? -1 : 0;
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
        Array.Copy(curlStateLow, curlScratchpadLow, Curl.StateLength);
        Array.Copy(curlStateHigh, curlScratchpadHigh, Curl.StateLength);

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