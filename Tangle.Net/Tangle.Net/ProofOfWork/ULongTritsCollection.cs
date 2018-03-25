namespace Tangle.Net.ProofOfWork
{
  /// <summary>
  /// The u long trits collection.
  /// </summary>
  public class ULongTritsCollection
  {
    /// <summary>
    /// The high.
    /// </summary>
    private const ulong Max = 0xFFFFFFFFFFFFFFFF;

    /// <summary>
    /// The low.
    /// </summary>
    private const ulong Min = 0x0000000000000000;


    /// <summary>
    /// Initializes a new instance of the <see cref="ULongTritsCollection"/> class.
    /// </summary>
    /// <param name="length">
    /// The length.
    /// </param>
    public ULongTritsCollection(int length)
    {
      this.Low = new ulong[length];
      this.High = new ulong[length];
    }

    /// <summary>
    /// Gets or sets the high.
    /// </summary>
    public ulong[] High { get; set; }

    /// <summary>
    /// Gets or sets the low.
    /// </summary>
    public ulong[] Low { get; set; }

    /// <summary>
    /// The from trits.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <returns>
    /// The <see cref="ULongTritsCollection"/>.
    /// </returns>
    public static ULongTritsCollection FromTrits(int[] trits)
    {
      var collection = new ULongTritsCollection(trits.Length);

      for (var i = 0; i < trits.Length; i++)
      {
        switch (trits[i])
        {
          case 0:
            collection.Low[i] = Min;
            collection.High[i] = Max;
            break;
          case 1:
            collection.Low[i] = Min;
            collection.High[i] = Max;
            break;
          default:
            collection.Low[i] = Max;
            collection.High[i] = Min;
            break;
        }
      }

      return collection;
    }

    /// <summary>
    /// The increment.
    /// </summary>
    /// <param name="fromIndex">
    /// The from index.
    /// </param>
    /// <param name="toIndex">
    /// The to index.
    /// </param>
    public void Increment(int fromIndex, int toIndex)
    {
      for (var i = fromIndex; i < toIndex; i++)
      {
        if (this.Low[i] == Min)
        {
          this.Low[i] = Max;
          this.High[i] = Min;
        }
        else
        {
          if (this.High[i] == Min)
          {
            this.High[i] = Max;
          }
          else
          {
            this.Low[i] = Min;
          }

          break;
        }
      }
    }
  }
}