namespace Tangle.Net.Mam.Services
{
  using System;

  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Utils;

  /// <summary>
  /// The hamming nonce seeker.
  /// </summary>
  public class HammingNonceSeeker
  {
    /// <summary>
    /// The seek.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <param name="security">
    /// The security.
    /// </param>
    /// <param name="offset">
    /// The offset.
    /// </param>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public int[] Seek(int[] trits, int security, int offset, int length)
    {
      var ulongTritsCollection = NonceCurl.FromTrits(trits);
      var size = Math.Min(length, Constants.TritHashLength) - offset;

      //let mut size = min(length, HASH_LENGTH) - offset;
      //for _ in 0..group {
      //  (&mut curl.state_mut()[offset + size / 3..offset + size * 2 / 3]).incr(); // GROUP = 0
      //}

      var index = 0;
      while (index == 0)
      {
        var round = Pascal.RoundThird(offset + size * 2 / 3 + 0);
      }

      //let mut index: Option<usize> = None;
      //while index.is_none() {
      //  size = min(
      //    num::round_third(
      //      (offset + size* 2 / 3 +
      //       (&mut curl.state_mut()[offset + size * 2 / 3..offset + size]).incr()) as i64,
      //    ) as usize,
      //  HASH_LENGTH,
      //    ) - offset;
      //  let mut cpy = curl.clone();
      //  cpy.transform();
      //  index = check(&cpy.state()[..HASH_LENGTH]);
      //}

      //let mux = TrinaryDemultiplexer::new(&curl.rate()[0..size]);

      //for (i, v) in mux.get(index.unwrap()).enumerate()
      //{
      //  out[i] = v;
      //}

      //Some(size)

      return trits;
    }
  }
}