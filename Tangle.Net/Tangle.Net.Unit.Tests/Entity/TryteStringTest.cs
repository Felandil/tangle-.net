namespace Tangle.Net.Unit.Tests.Entity
{
  using System;
  using System.Text;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The tryte string test.
  /// </summary>
  [TestClass]
  public class TryteStringTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test chunks are sliced correctly.
    /// </summary>
    [TestMethod]
    public void TestChunksAreSlicedCorrectly()
    {
      var tryteString = new TryteString("IAMGROOTU");
      var chunks = tryteString.GetChunks(2);

      Assert.AreEqual(5, chunks.Count);
    }

    /// <summary>
    /// The test to bytes does generate correct length.
    /// </summary>
    [TestMethod]
    public void TestToBytesDoesGenerateCorrectLength()
    {
      var tryteString = new TryteString("FFWEHDTAQFFDNEUGLAIDHHSAEHTGEDQEVBGHVHGFLCWDM9L99GOFWAA99EDBQCJCACZEZAC9UEYGZ9Q9DFNBEDRB9GGAZAV9VH9FTAODRAMGMDSHUGX9LH9HVBOFRBLENFN9YEEFVGI9QDJBF9KHCH9IEFYAZHMBPDEFNDHBPBLACHEIAECBZFHHLAQBCILGEDQDAB9HVDOFNBXFMBNHVFOBZA9CIGCCGHO9DCUDFEFCPFIHJDK9VDTGRGGEF9CBVDQAAECERF9D9CYHCHPBDFTDXEFEN9EEOBFHXDBBPANCPBVDTGGFUGGHXGACKBF9M9FHMFKDR9GCRBEAFFWEKB9IMBTALFKDXBBAPEXETCZCM9B9QECBGDKCQDNEDF9BDFLCX9YHMG9HC9MDSCCBSCKATCD9VAFHPHZDG9DCTD9FYGP9WE9GHIL9BHNGSEC9E9WCKHIDYFY9EBWEQBYHXCO9HFQHWFTDODIDKHDCMFMCLBVCVAH99FECEIEGOFKCC9N9Y9GEN9LGZCIGDDAARHEI9BGGSBGFJEBFJFLHOH9HF9U9QAGCLBQAECBIBATEJIWDNDKGVCLIVGABKFEBXCADQHY9L9MFCFT9LETCXASFEF9GWCMH9DTAQDIACEZBR9XABAZEZBZCKCG9FBIDVELEDETANHFCLCVFEGFDT9GD9E9FJ9Y9FFJAWGAID9UDLDQAEFIAMAXCW99IWAVBM9DAZEQ9ZHKBYEOESBABJEYE9AGFZDEGG9MGXAEBVEYESCIGBFGAGHKIGIY9RBVBDFTELFICQEMASEUBAHNCUCJEC9BAGBECVFTCVELBC9SHICKAF9KAMDVBT9V9X9YBCHHCQFOBCFLDICLAXEKER9IFACCAIIRGB9VFMDWCTGRFQHXDQHL9T9FGIIICA9E9UFRAWALGZEBHOBPCNCK9LEKIBHZAJHUBGICBLBCDUHNAQHJBJ9KGS9TEGEXHBGEHBBSEJBRBJANDB9GGRDCGZBRFTETEK9IEK9TDZBABUFE9IGABEDTFGFDGBFXHECY9Z9ZBCD9DOCJCCBWHQGJHCALATHB9VDYC9CDHAGB9GC9B9HEEBCHCIEUGHFJHCGLEUFDDAGKFG9SCXBLHUCV9ADPESBXBLFRFLGKHZGZBZEXFMES9CBHAEHQGWCVBZFKFM9IHYHRA9GPAL9NAV9AEJHGILAQDKDICJHRAABUDN9YGKHBIGAG9ICIIYEZ9WGCANHCGBEBCBGQBJCN9EGFCLAIBCDHHHHRFR9D9DGEALCPAECDEY9LFIFFIEEJDHEDDF9JGVHAISDW9NBFAQ9IA9FNAZH9EY9AEDCY9NHS9TEJ9AEQFCEGFHGHFYGNFQAVHAHT9EFXHWAIBVCFGJFLHAD9AKAXE9HAHVCWAIFIBAFVBC9PBDBVFDAT9LFKFCACEKFACUEHDHEHDOBSFSHJEACODUFKDPCVCMHEAHHJBDBGGPBHFIASBJDJIGGGFGFIEJ9XBABQEEBLFSAGAYFK9AIYFCFBIEESDZCXDFFUDVGHAFEFDJGAISELIVFAADDPCFCICLBKHDHIEIEGHG9KCTETDFCGEN9VCV9ACYELAVERHPFLIT9KDYFQHOBH9PFEAIIFIMBDALECECDTBCGOCLGUCK9SBFALHRETFV9GGQHTGKAGELHOEYGYHECCFZCNCIHDIF9TGOBVCVDWCXHQFYDDEAISGGATEPEHGW9LFAITEYCYDMG9DRGFAYCJ9OCVHWHLAGBRHKIFIY9JFTFSCV9PAVHTFA9A999EGIAAIEHPEECCBWDM9OCAADFIAWFAGSDJGOAVCCD9ACADHIIMGJGP9OCNGIDNDSCHCX9IEMCLATGBEKGZFXGDFYABGE9LEN9RAJIMASBI9YATHREFDNEYDFCLEDFGCKAIFDCODFEOBCDPDIGO9Y9DHUAADAFBAZE99UHBEUGQAVDK99COGT9XATGLIMFSEYDD9XCYBJ9FGXANAWDJ9C9GFCAXAGEBDGFLEODVAM9MFSHTBTGJHEDC9GCIBLCSBKCNGHA99");
      var bytes = tryteString.ToBytes();

      Assert.AreEqual(1022, bytes.Length);
    }

    /// <summary>
    /// The test from byte array and back.
    /// </summary>
    [TestMethod]
    public void TestFromByteArrayAndBack()
    {
      var bytes = Encoding.UTF8.GetBytes("┬");
      var tryteString = bytes.ToTrytes();
      var bytesBack = tryteString.ToBytes();

      for (var i = 0; i < bytes.Length; i++)
      {
        Assert.AreEqual(bytes[i], bytesBack[i]);
      }

      tryteString = TryteString.FromBytes(bytes);
      bytesBack = tryteString.ToBytes();

      for (var i = 0; i < bytes.Length; i++)
      {
        Assert.AreEqual(bytes[i], bytesBack[i]);
      }

    }

    /// <summary>
    /// The test given string is no tryte string should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestGivenStringIsNoTryteStringShouldThrowException()
    {
      var tryteString = new TryteString("jasda87678");
    }

    /// <summary>
    /// The test given string is trytes string should set value.
    /// </summary>
    [TestMethod]
    public void TestGivenStringIsTrytesStringShouldSetValue()
    {
      var tryteString = new TryteString("IAMGROOT");
      Assert.AreEqual("IAMGROOT", tryteString.Value);
    }

    /// <summary>
    /// The test utf 8 string conversion.
    /// </summary>
    [TestMethod]
    public void TestUtf8StringConversion()
    {
      var stringValue = "┬";
      var tryteString = TryteString.FromUtf8String(stringValue);

      Assert.AreEqual(stringValue, tryteString.ToUtf8String());
    }

    #endregion
  }
}