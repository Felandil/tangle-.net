namespace Tangle.Net.Unit.Tests.Utils
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;

  /// <summary>
  /// The signing helper test.
  /// </summary>
  [TestClass]
  public class SigningHelperTest
  {
    /// <summary>
    /// The test subseed generation.
    /// </summary>
    [TestMethod]
    public void TestSubseedGeneration()
    {
      var seed = new Seed("L9DRGFPYDMGVLH9ZCEWHXNEPC9TQQSA9W9FZVYXLBMJTHJC9HZDONEJMMVJVEMHWCIBLAUYBAUFQOMYSN");
      var expectedSubseedValue = "DXAAFFTM9LWSNRYTTCIHCNDVRHJBRKFGDHPAQSJWGHNLGBFJUXW9MELIMXTSUCUYEEBSXTEQHEPSTZWIL";
      var signingHelper = new IssSigningHelper(new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27));

      var subseedTrits = signingHelper.GetSubseed(seed, 0);
      var subseed = Converter.TritsToTrytes(subseedTrits);

      Assert.AreEqual(expectedSubseedValue, subseed);
    }

    /// <summary>
    /// The test digest from subseed.
    /// </summary>
    [TestMethod]
    public void TestDigestFromSubseed()
    {
      var expectedDigest = "DFTTZXYAWRRKHMGYGIUEF9BUCQXXXGGMFZTLC9NKFPPL9HDOLSBITWNHMBZFSXIKIZCVL9KWBMTORBU9U";
      var subseed = new TryteString("DXAAFFTM9LWSNRYTTCIHCNDVRHJBRKFGDHPAQSJWGHNLGBFJUXW9MELIMXTSUCUYEEBSXTEQHEPSTZWIL");

      var signingHelper = new IssSigningHelper(new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27));

      var digest = signingHelper.DigestFromSubseed(subseed.ToTrits(), SecurityLevel.Medium);
      var digestTrytes = Converter.TritsToTrytes(digest);

      Assert.AreEqual(expectedDigest, digestTrytes);
    }

    /// <summary>
    /// The test address from digest.
    /// </summary>
    [TestMethod]
    public void TestAddressFromDigest()
    {
      var expectedAddress = "MYNGVRBGNNKWHSTPOUOCBFWXKIUGZEYRCS9NGO9RYKLSOYAEBPOTNNONK9EVJTXQHYLOCRGCJWTTETSYA";
      var digest = new TryteString("DFTTZXYAWRRKHMGYGIUEF9BUCQXXXGGMFZTLC9NKFPPL9HDOLSBITWNHMBZFSXIKIZCVL9KWBMTORBU9U");

      var signingHelper = new IssSigningHelper(new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27));

      var address = signingHelper.AddressFromDigest(digest.ToTrits());
      var addressTrytes = Converter.TritsToTrytes(address);

      Assert.AreEqual(expectedAddress, addressTrytes);
    }
  }
}