namespace Tangle.Net.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography;

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
    public static new Fragment FromAsciiString(string input)
    {
      return new Fragment(FromAsciiToTryteString(input));
    }

    /// <summary>
    /// The from utf 8 string.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="Fragment"/>.
    /// </returns>
    public static new Fragment FromUtf8String(string input)
    {
      return new Fragment(TryteString.FromUtf8String(input).Value);
    }

    /// <summary>
    /// The validate fragments.
    /// </summary>
    /// <param name="fragments">
    /// The fragments.
    /// </param>
    /// <param name="hash">
    /// The hash.
    /// </param>
    /// <param name="publicKey">
    /// The public key.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool ValidateFragments(List<Fragment> fragments, Hash hash, TryteString publicKey)
    {
      var checksum = new List<int>();
      var normalizedHash = Hash.Normalize(hash);

      for (var i = 0; i < fragments.Count; i++)
      {
        var normalizedHashChunk = normalizedHash.Skip((i % 3) * 27).Take(27).ToArray();
        var buffer = new int[AbstractCurl.HashLength];

        var outerSponge = new Kerl();
        var fragmentChunks = fragments[i].GetChunks(Hash.Length);
        for (var j = 0; j < fragmentChunks.Count; j++)
        {
          buffer = fragmentChunks[j].ToTrits();
          var innerSponge = new Kerl();

          for (var k = 0; k < normalizedHashChunk[j] + 13; k++)
          {
            innerSponge.Reset();
            innerSponge.Absorb(buffer);
            innerSponge.Squeeze(buffer);
          }

          outerSponge.Absorb(buffer);
        }

        outerSponge.Squeeze(buffer);
        checksum.AddRange(buffer);
      }

      var actualPublicKey = new int[AbstractCurl.HashLength];
      var kerl = new Kerl();
      kerl.Absorb(checksum.ToArray());
      kerl.Squeeze(actualPublicKey);

      var actualPublicKeyTrytes = Converter.TritsToTrytes(actualPublicKey);
      return actualPublicKeyTrytes == publicKey.Value;
    }

    #endregion
  }
}