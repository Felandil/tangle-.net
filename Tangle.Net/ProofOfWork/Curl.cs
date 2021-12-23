namespace Tangle.Net.ProofOfWork
{
  /// <summary>
  /// The curl.
  /// </summary>
  public class Curl : AbstractCurl
  {
    public const int TritHashLength = 243;

    /// <summary>
    /// The number of rounds.
    /// </summary>
    public const int DefaultNumberOfRounds = 81;

    /// <summary>
    /// The state length.
    /// </summary>
    public const int StateLength = 3 * TritHashLength;

    /// <summary>
    /// The truth table.
    /// </summary>
    public static readonly int[] TruthTable = { 1, 0, -1, 2, 1, -1, 0, 2, -1, 1, 0 };


    /// <summary>
    /// Initializes a new instance of the <see cref="Curl"/> class.
    /// </summary>
    public Curl()
    {
      this.Reset();
      this.NumberOfRounds = DefaultNumberOfRounds;
    }

    /// <summary>
    /// Gets the number of rounds.
    /// </summary>
    private int NumberOfRounds { get; }

    /// <summary>
    /// The absorb.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    public override void Absorb(int[] trits)
    {
      var offset = 0;
      while (offset < trits.Length)
      {
        var length = trits.Length - offset;

        for (var i = 0; i < (length < TritHashLength ? length : TritHashLength); i++)
        {
          this.State[i] = trits[offset + i];
        }

        this.Transform();

        offset += TritHashLength;
      }
    }

    /// <summary>
    /// The reset.
    /// </summary>
    public sealed override void Reset()
    {
      this.State = new int[StateLength];
    }

    /// <summary>
    /// The squeeze.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    public override void Squeeze(int[] trits)
    {
      var length = trits.Length;
      var round = 0;

      do
      {
        for (var i = 0; i < (length < TritHashLength ? length : TritHashLength); i++)
        {
          trits[i + (round * TritHashLength)] = this.State[i];
        }

        this.Transform();
        round++;
      }
      while ((length -= TritHashLength) > 0);
    }

    /// <summary>
    /// The transform.
    /// </summary>
    private void Transform()
    {
      var stateCopy = new int[StateLength];
      var index = 0;

      for (var round = 0; round < this.NumberOfRounds; round++)
      {
        for (var i = 0; i < StateLength; i++)
        {
          stateCopy[i] = this.State[i];
        }

        for (var i = 0; i < StateLength; i++)
        {
          this.State[i] = TruthTable[stateCopy[index] + (stateCopy[index += index < 365 ? 364 : -365] << 2) + 5];
        }
      }
    }
  }
}