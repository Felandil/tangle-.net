// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KerlTest.cs" company="Felandil IT">
//    Copyright (c) 2008 -2018 Felandil IT. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------------------------
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

    #endregion
  }
}