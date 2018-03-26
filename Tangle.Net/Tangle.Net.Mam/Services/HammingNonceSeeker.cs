namespace Tangle.Net.Mam.Services
{
  using System;

  using Tangle.Net.Cryptography.Curl;
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
      var nonceCurl = new NonceCurl(trits.Length, (int) CurlMode.CurlP27);
      nonceCurl.Absorb(trits, trits.Length, 0);

      nonceCurl.High[0] = 13176245766935394011;
      nonceCurl.High[1] = 14403622084951293727;
      nonceCurl.High[2] = 18445620372817592319;
      nonceCurl.High[3] = 2199023255551;

      nonceCurl.Low[0] = 15811494920322472813;
      nonceCurl.Low[1] = 17941353825114769379;
      nonceCurl.Low[2] = 576458557575118879;
      nonceCurl.Low[3] = 18446741876833779711;

      var size = Math.Min(length, Constants.TritHashLength) - offset;

      //let mut size = min(length, HASH_LENGTH) - offset;
      //for _ in 0..group {
      //  (&mut curl.state_mut()[offset + size / 3..offset + size * 2 / 3]).incr(); // GROUP = 0, MUST NOT IMPLEMENT! (?)
      //}

      var index = 0;
      while (index == 0)
      {
        var round = Pascal.RoundThird(offset + size * 2 / 3 + nonceCurl.Increment(offset + size * 2 / 3, offset + size));
        size = Math.Min(round, Constants.TritHashLength) - offset;
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