namespace Tangle.Net.Source.Utils
{
  using System;

  using Tangle.Net.Source.Cryptography;

  /// <summary>
  /// The checksum.
  /// </summary>
  public static class Checksum
  {
    #region Constants

    /// <summary>
    /// The checksum length.
    /// </summary>
    public const int ChecksumLength = 9;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The add.
    /// </summary>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string Add(string address)
    {
      if (!InputValidator.IsTrytes(address))
      {
        throw new ArgumentException("An address does contain non tryte characters.");
      }

      var addressTrits = Converter.TrytesToTrits(address);

      var kerl = new Kerl();
      kerl.Absorb(addressTrits);

      var checksumTrits = new int[Kerl.HashLength];
      kerl.Squeeze(checksumTrits);

      var tritsToTrytes = Converter.TritsToTrytes(checksumTrits);
      var checksum = tritsToTrytes.Substring(81 - ChecksumLength, ChecksumLength);

      return address + checksum;
    }

    /// <summary>
    /// The has valid checksum.
    /// </summary>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool HasValidChecksum(string address)
    {
      var addressWithoutChecksum = Strip(address);
      var addressWithChecksum = Add(addressWithoutChecksum);

      return addressWithChecksum == address;
    }

    /// <summary>
    /// The strip.
    /// </summary>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown on incorrect address length
    /// </exception>
    public static string Strip(string address)
    {
      if (address.Length < 81)
      {
        throw new ArgumentException("Address Length should not be smaller than 81.");
      }

      return address.Substring(0, 81);
    }

    #endregion
  }
}