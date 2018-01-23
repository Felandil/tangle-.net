namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Source.Cryptography;
  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The signature fragment.
  /// </summary>
  public class Fragment : TryteString
  {
    #region Constants

    /// <summary>
    /// The length.
    /// </summary>
    public const int Length = 2187;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Fragment"/> class.
    /// </summary>
    public Fragment()
      : this(string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Fragment"/> class.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    public Fragment(string trytes)
      : base(trytes)
    {
      if (this.TrytesLength > Length)
      {
        throw new ArgumentException("Fragment length must not be longer than " + Length);
      }

      if (this.TrytesLength < Length)
      {
        this.Pad(Length);
      }
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets a value indicating whether is empty.
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        return this.Value.All(c => c == '9');
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The from string.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="Fragment"/>.
    /// </returns>
    public static new Fragment FromString(string input)
    {
      return new Fragment(AsciiToTrytes.FromString(input));
    }

    public static bool ValidateFragments(List<Fragment> fragments, Hash hash, TryteString publicKey)
    {
      var checksum = new List<int>();
      var normalizedHash = Hash.Normalize(hash);

      for (var i = 0; i < fragments.Count; i++)
      {
        var normalizedHashChunk = normalizedHash.Skip((i % 3) * 27).Take(27).ToArray();
        var buffer = new int[Kerl.HashLength];

        var outerSponge = new Kerl();
        var fragmentChunks = fragments[i].GetChunks(Hash.Length);
        for (var j = 0; j < fragmentChunks.Count; j++)
        {
          buffer = fragmentChunks[j].ToTrits();

          for (var k = normalizedHashChunk[j] + 13; k-- > 0;)
          {
            var innerSponge = new Kerl();
            innerSponge.Absorb(buffer);
            innerSponge.Squeeze(buffer);
          }

          outerSponge.Absorb(buffer);
        }

        outerSponge.Squeeze(buffer);
        checksum.AddRange(buffer);
        i++;
      }

      var actualPublicKey = new int[Kerl.HashLength];
      var kerl = new Kerl();
      kerl.Absorb(checksum.ToArray());
      kerl.Squeeze(actualPublicKey);

      var actualPublicKeyTrytes = Converter.TritsToTrytes(actualPublicKey);
      return actualPublicKeyTrytes == publicKey.Value;
    }

    #endregion
  }
}