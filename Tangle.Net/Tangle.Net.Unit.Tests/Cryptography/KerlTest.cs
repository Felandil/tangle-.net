namespace Tangle.Net.Unit.Tests.Cryptography
{
  using System.IO;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;

  /// <summary>
  /// The kerl test.
  /// </summary>
  [TestClass]
  public class KerlTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test kerl one absorb.
    /// </summary>
    [TestMethod]
    public void TestKerlOneAbsorb()
    {
      var tritValue = Converter.TrytesToTrits("KFNNRVYTYYYNHJLBTXOEFYBZTHGXHTX9XKXB9KUZDHGLKBQGPQCNHPGDSGYKWGHVXVLHPOEAWREBIVK99");

      var kerl = new Kerl();
      kerl.Absorb(tritValue);

      var hashValue = new int[Kerl.HashLength];
      kerl.Squeeze(hashValue);

      var hash = Converter.TritsToTrytes(hashValue);
      Assert.AreEqual("SHTKPLZWIXLDVHAEAGFSVWNDGVIX9SDVGEHAFGXEIMLWSHDTQYNZZKPBGMUF9GNEWIGIFYWWMSCLJ9RCD", hash);
    }

    /// <summary>
    /// The kurl multi squeeze.
    /// </summary>
    [TestMethod]
    public void KurlMultiSqueeze() 
    {
      var tritValue = Converter.TrytesToTrits("9MIDYNHBWMBCXVDEFOFWINXTERALUKYYPPHKP9JJFGJEIUY9MUDVNFZHMMWZUYUSWAIOWEVTHNWMHANBH");

      var kerl = new Kerl();
      kerl.Absorb(tritValue);

      var hashValue = new int[Kerl.HashLength * 2];
      kerl.Squeeze(hashValue);

      var hash = Converter.TritsToTrytes(hashValue);
      Assert.AreEqual("G9JYBOMPUXHYHKSNRNMMSSZCSHOFYOYNZRSZMAAYWDYEIMVVOGKPJBVBM9TDPULSFUNMTVXRKFIDOHUXXVYDLFSZYZTWQYTE9SPYYWYTXJYQ9IFGYOLZXWZBKWZN9QOOTBQMWMUBLEWUEEASRHRTNIQWJQNDWRYLCA", hash);
    }

    /// <summary>
    /// The kurl multi absorb multi squeeze.
    /// </summary>
    [TestMethod]
    public void KurlMultiAbsorbMultiSqueeze() 
    {
      var tritValue = Converter.TrytesToTrits("G9JYBOMPUXHYHKSNRNMMSSZCSHOFYOYNZRSZMAAYWDYEIMVVOGKPJBVBM9TDPULSFUNMTVXRKFIDOHUXXVYDLFSZYZTWQYTE9SPYYWYTXJYQ9IFGYOLZXWZBKWZN9QOOTBQMWMUBLEWUEEASRHRTNIQWJQNDWRYLCA");

      var kerl = new Kerl();
      kerl.Absorb(tritValue);

      var hashValue = new int[Kerl.HashLength * 2];
      kerl.Squeeze(hashValue);

      var hash = Converter.TritsToTrytes(hashValue);
      Assert.AreEqual("LUCKQVACOGBFYSPPVSSOXJEKNSQQRQKPZC9NXFSMQNRQCGGUL9OHVVKBDSKEQEBKXRNUJSRXYVHJTXBPDWQGNSCDCBAIRHAQCOWZEBSNHIJIGPZQITIBJQ9LNTDIBTCQ9EUWKHFLGFUVGGUWJONK9GBCDUIMAYMMQX", hash);
    }

    /// <summary>
    /// The test generate trytes and multi squeeze.
    /// </summary>
    [TestMethod]
    public void TestGenerateTrytesAndMultiSqueeze()
    {
      var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var isNetStandard = path.Contains("Standard");
      var netStandardExtension = isNetStandard ? "../" : "";


      // CSV from Python lib. Thanks alot!
      using (var reader = new StreamReader(@"../" + netStandardExtension + "../Cryptography/generate_trytes_and_multi_squeeze.csv"))
      {
        var i = 0;
        while (!reader.EndOfStream)
        {
          var line = reader.ReadLine();
          if (line == null || i == 0)
          {
            i++;
            continue;
          }

          var values = line.Split(',');

          var trytes = values[0];
          var hashes1 = values[1];
          var hashes2 = values[2];
          var hashes3 = values[3];

          var trits = Converter.TrytesToTrits(trytes);

          var kerl = new Kerl();
          kerl.Absorb(trits);

          var tritsOut = new int[Kerl.HashLength];
          kerl.Squeeze(tritsOut);
          var trytesOut = Converter.TritsToTrytes(tritsOut);

          Assert.AreEqual(hashes1, trytesOut);

          tritsOut = new int[Kerl.HashLength];
          kerl.Squeeze(tritsOut);
          trytesOut = Converter.TritsToTrytes(tritsOut);

          Assert.AreEqual(hashes2, trytesOut);

          tritsOut = new int[Kerl.HashLength];
          kerl.Squeeze(tritsOut);
          trytesOut = Converter.TritsToTrytes(tritsOut);

          Assert.AreEqual(hashes3, trytesOut);
          i++;
        }
      }
    }

    #endregion
  }
}