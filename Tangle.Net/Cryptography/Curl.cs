namespace Tangle.Net.Cryptography
{
  using System;

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
        Array.Copy(trits, offset, this.State, 0, trits.Length < HashLength ? trits.Length : HashLength);

        this.Transform();

        offset += HashLength;
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
      var offset = 0;
      do
      {
        Array.Copy(this.State, 0, trits, offset, length < HashLength ? length : HashLength);
        this.Transform();
        offset += HashLength;
      }
      while ((length -= HashLength) > 0);
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
        Array.Copy(this.State, 0, stateCopy, 0, StateLength);

        for (var i = 0; i < StateLength; i++)
        {
          this.State[i] = TruthTable[stateCopy[index] + (stateCopy[index += index < 365 ? 364 : -365] << 2) + 5];
        }
      }
    }

    #endregion
  }
}