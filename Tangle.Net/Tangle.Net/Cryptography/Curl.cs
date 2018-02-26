namespace Tangle.Net.Cryptography
{
  using System;
  using System.Threading.Tasks;

  /// <summary>
  /// The curl.
  /// </summary>
  public class Curl : AbstractCurl
  {
    #region Constants

    /// <summary>
    /// The number of rounds.
    /// </summary>
    public const int NumberOfRounds = 81;

    /// <summary>
    /// The state length.
    /// </summary>
    public const int StateLength = 3 * HashLength;

    #endregion

    #region Static Fields

    /// <summary>
    /// The truth table.
    /// </summary>
    public static readonly int[] TruthTable = { 1, 0, -1, 2, 1, -1, 0, 2, -1, 1, 0 };

    #endregion

    #region Fields

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Curl"/> class.
    /// </summary>
    public Curl()
    {
      this.Reset();
    }

    #endregion

    #region Public Methods and Operators

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

        Parallel.For(
          0,
          length < AbstractCurl.HashLength ? length : AbstractCurl.HashLength,
          (i) => { this.State[i] = trits[offset + i]; });

        this.Transform();

        offset += AbstractCurl.HashLength;
      }
    }

    /// <summary>
    /// The reset.
    /// </summary>
    public override sealed void Reset()
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
      do
      {
        Parallel.For(
          0,
          length < AbstractCurl.HashLength ? length : AbstractCurl.HashLength,
          (i) => { trits[i] = this.State[i]; });

        this.Transform();
      }
      while ((length -= AbstractCurl.HashLength) > 0);
    }

    #endregion

    #region Methods

    /// <summary>
    /// The transform.
    /// </summary>
    private void Transform()
    {
      var stateCopy = new int[StateLength];
      var index = 0;

      for (var round = 0; round < NumberOfRounds; round++)
      {
        Parallel.For(
          0,
          StateLength,
          (i) =>
            {
              stateCopy[i] = this.State[i];
            });

        for (var i = 0; i < StateLength; i++)
        {
          this.State[i] = TruthTable[stateCopy[index] + (stateCopy[index += index < 365 ? 364 : -365] << 2) + 5];
        }
      }
    }

    #endregion
  }
}