namespace Tangle.Net.Tests.Cryptography
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Cryptography;
  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The address generator test.
  /// </summary>
  [TestClass]
  public class AddressGeneratorTest
  {
    #region Fields

    /// <summary>
    /// The seed one.
    /// </summary>
    private readonly Seed seedOne = new Seed("TESTVALUE9DONTUSEINPRODUCTION999999GFDDCPFIIEHBCWFN9KHRBEIHHREFCKBVGUGEDXCFHDFPAL");

    /// <summary>
    /// The seed two.
    /// </summary>
    private Seed seedTwo = new Seed("TESTVALUE9DONTUSEINPRODUCTION99999DCZGVEJIZEKEGEEHYE9DOHCHLHMGAFDGEEQFUDVGGDGHRDR");

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The test get single address.
    /// </summary>
    [TestMethod]
    public void TestGetSingleAddress()
    {
      var generator = new AddressGenerator(this.seedOne);

      var address = generator.GetAddress(0);
      Assert.AreEqual("DLEIS9XU9V9T9OURAKDUSQWBQEYFGJLRPRVEWKN9SSUGIHBEIPBPEWISSAURGTQKWKWNHXGCBQTWNOGIY", address.Value);
      Assert.AreEqual(2, address.SecurityLevel);

      var addressTwo = generator.GetAddress(10);
      Assert.AreEqual("XLXFTFBXUOOHRJDVBDBFEBDQDUKSLSOCLUYWGLAPR9FUROUHPFINIUFKYSRTFMNWKNEPDZATWXIVWJMDD", addressTwo.Value);
      Assert.AreEqual(10, addressTwo.KeyIndex);
    }

    #endregion
  }
}