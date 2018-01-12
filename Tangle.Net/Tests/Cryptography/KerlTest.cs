namespace Tangle.Net.Tests.Cryptography
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Cryptography;

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
      var tritValue = Converter.TrytesToTrits("EMIDYNHBWMBCXVDEFOFWINXTERALUKYYPPHKP9JJFGJEIUY9MUDVNFZHMMWZUYUSWAIOWEVTHNWMHANBH");

      var kerl = new Kerl();
      kerl.Absorb(tritValue, 0, tritValue.Length);

      var hashValue = new int[Kerl.HashLength];
      kerl.Squeeze(hashValue, 0, hashValue.Length);

      var hash = Converter.TritsToTrytes(hashValue);
      Assert.AreEqual("EJEAOOZYSAWFPZQESYDHZCGYNSTWXUMVJOVDWUNZJXDGWCLUFGIMZRMGCAZGKNPLBRLGUNYWKLJTYEAQX", hash);
    }

    /// <summary>
    /// The kurl multi squeeze.
    /// </summary>
    [TestMethod]
    public void KurlMultiSqueeze() 
    {
      var tritValue = Converter.TrytesToTrits("9MIDYNHBWMBCXVDEFOFWINXTERALUKYYPPHKP9JJFGJEIUY9MUDVNFZHMMWZUYUSWAIOWEVTHNWMHANBH");

      var kerl = new Kerl();
      kerl.Absorb(tritValue, 0, tritValue.Length);

      var hashValue = new int[Kerl.HashLength * 2];
      kerl.Squeeze(hashValue, 0, hashValue.Length);

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
      kerl.Absorb(tritValue, 0, tritValue.Length);

      var hashValue = new int[Kerl.HashLength * 2];
      kerl.Squeeze(hashValue, 0, hashValue.Length);

      var hash = Converter.TritsToTrytes(hashValue);
      Assert.AreEqual("LUCKQVACOGBFYSPPVSSOXJEKNSQQRQKPZC9NXFSMQNRQCGGUL9OHVVKBDSKEQEBKXRNUJSRXYVHJTXBPDWQGNSCDCBAIRHAQCOWZEBSNHIJIGPZQITIBJQ9LNTDIBTCQ9EUWKHFLGFUVGGUWJONK9GBCDUIMAYMMQX", hash);
    }

    #endregion
  }
}