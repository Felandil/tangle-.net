namespace Tangle.Net.Mam.Unit.Tests.Mam
{
  using System.Configuration;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Mam.Services;
  using Tangle.Net.Utils;

  /// <summary>
  /// The mam channel test.
  /// </summary>
  [TestClass]
  public class MamChannelTest
  {
    /// <summary>
    /// The test public channel creation.
    /// </summary>
    [TestMethod]
    public void TestPublicChannelCreation()
    {
      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");
      var mamFactory = new CurlMamFactory(new Curl(), new CurlMask());
      var treeFactory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGenerator(seed)));

      var channelFactory = new MamChannelFactory(mamFactory, treeFactory, new InMemoryIotaRepository());
      var channel = channelFactory.Create(Mode.Public, seed);

      Assert.AreEqual(seed.Value, channel.Seed.Value);
      Assert.AreEqual(Mode.Public, channel.Mode);
      Assert.AreEqual(SecurityLevel.Medium, channel.SecurityLevel);
    }

    /// <summary>
    /// The test restricted channel creation.
    /// </summary>
    [TestMethod]
    public void TestRestrictedChannelCreation()
    {
      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");
      var mamFactory = new CurlMamFactory(new Curl(), new CurlMask());
      var treeFactory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGenerator(seed)));

      var channelFactory = new MamChannelFactory(mamFactory, treeFactory, new InMemoryIotaRepository());
      var channelKey = new TryteString("NXRZEZIKWGKIYDPVBRKWLYTWLUVSDLDCHVVSVIWDCIUZRAKPJUIABQDZBV9EGTJWUFTIGAUT9STIENCBC");
      var channel = channelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey);

      Assert.AreEqual(seed.Value, channel.Seed.Value);
      Assert.AreEqual(Mode.Restricted, channel.Mode);
      Assert.AreEqual(SecurityLevel.Medium, channel.SecurityLevel);
      Assert.AreEqual(channelKey.Value, channel.Key.Value);
      Assert.AreEqual(Hash.Empty.Value, channel.NextRoot.Value);
      Assert.AreEqual(0, channel.Index);
      Assert.AreEqual(0, channel.Start);
      Assert.AreEqual(1, channel.Count);
      Assert.AreEqual(1, channel.NextCount);
    }

    /// <summary>
    /// The test restricted message creation.
    /// </summary>
    [TestMethod]
    public void TestRestrictedMessageCreation()
    {
      var bundle = new Bundle();
      bundle.AddTransfer(new Transfer
                           {
                             Address = new Address(),
                             Message = new TryteString("VFVTDOOIYYHGIQQVVIHKJAUWIAEYOVZXYOIJHYGJHLONKJEOECSVWQIVOZAMEKDFBEXWWKFXXDDCKQG9LDFJIKKWWYK9VPVXNVNDWGCCLFFJXIXEAWB9QSTXLEKKSNVHCCDDVHNVIBVJEGD9OWMSYMYP9JVJCYBHHYQNANMOELQVNXICOUJAPX9HMHDKIIVFYRIDLXQZALMMATOXNHGLHGEZJYVARRABNGJR9LIUMOGXEEBFOACIRJPGXFAAEMZUJMCGUFRUAEWQWFRCFTXOBPJOZPOFMNESXXXE9XCTZQBMRIANJTRJAN9JRJZFZABRMQOG9NUHPLRXZIJRYMOCIYJRJQBT9WPPTRRWEK9FBPREPDDKHAXJNWAK9TY99OTUPPQMNKJHZMUKKWLBGATIWMBPTCFLZ9QTTHLCSSED9PQVNCOJZZDGEHYNDEITFXLACVDJTLZEESRWLRHHBMPWLJLPEYJAGZEOXWQKP9NY9KOFBSZBXNADYJKKKMTMYYBRILOOEPHLVJDEMUAUUEUHOCKOMWDYBMOEXTCQRUHBSCUUNFYQMHZLMUHDVTC9NSEALGNREDMOZDY9MHFHCURZUZAOHZLEUXTFKURTZNIRGJRVTTJMLXBRTQLZHMIIMYRGAPZDUIFPSYSXQBXBFKVNTLRLDLYL9UYLJKCRKAKRKMCWYCWPYRHJMMUXKQ9GWHRLVGOYJOLAF9DLOUTECUZSCFPXDWBTOYMORIJJKQROFFMEYCC9MNDAHDDDQHN9IGSXFADVTAZ9VWZVHN9UACSRJXBDNB9NWZILBKZNECSZZYJZRPMCJIMNBNMYQLEMKXSPLMHFZNVOZREBYZTFUKVQCE9VVQRCNRE9JCICMJEQKMNMJLQ9LSEYTOO9MN9RNCLFEHLQLPLQYT9LZEJWKYMKEMMWPWLHGJDIAZFFGCGJM9PAQDBTK9EJUCGELWPZMGRZICKXMMSMCJGUVXHCVRQNFBPIMSKGVRKIGNKTRNVEJLFKUXOQEWRCRHVFLPUYWDMNONIHSXKEUXPZLPCSNXOMHOCPBTCEIDLRAQPPYXXXSNBOGDNKBYZQGSDFSXPHHPCFFDBVPJDMYNOKAOKSZWDFMIHOAUOHJYEPPFAL9CFIKWSIHYMHRDFQDYLGRWRAXSDYSEPNRYQGFMGFEWGXHDNIVOHIZIHKUFJDWQTZZWYSANKNYJ9IHIOR9RVZLFTJHMEJDFZYUTVSHUXQBZIDFLLTMESSFOYXDOMPKGSTANABWEPWATGMVBXQNMFRHF9Z9JIIAUKFORAZRVXIBPKJCRHDHGBQKXSHLYXW9E9WVNB9V9EJALMUPWTBWNFXIZXYTKBNZRYETQICHMUQEAEUBXAZVECLCUWAVJZZV9MNBGQMWLXZNRSMNANTMAKRXV9YKFLIKSPTPEDQESHAWLQYPRABFJXPFAUHPPJFTADKOXMJMUMCVXJCOHZINVYIGHTAXBMGTSWJCGFGPODFQYINOPSGRYWXRA9JXMB9NRTBOCWNGNFUISOJMSZUUDSNJ9MVHERXBCYF9LDCNEBTAQKOGJKPEZVGHLLQDBJMDWHXUDKRDVKRLWVNVUTYMHCE9W9TDSDYAWSWPNMIRRHOYFZMMGIOJMPAPMOKBSUXJJCQAYAXPJGAANPX99WHTLRX9WQPEOYALO9DPQKGTUJEPKFICYPFJCDHQLHHKQRKK9ILZOLHVPBRURFKZUKLCTHIWLLEFYQEZPBPWVTBQIKKMIZFSWCQDJBAHXYXQEWTRYHNRPQUCCB9VWPVRQJPEVOUSHHGYFFFGZN9ERAJYUI9UIP9TYCDEWNOEPQBNSHPCANJYIDLGXGGMDDMDSXRCYMSKDHEGQEYKAWETOFTJRHZWAKIOETVDYVCQHHJSPNUIW9KCDDONPUQJEPEWOHRUZPLQKIXGSOSZNPLMYNHHOC9PGTZCSAMTQFCVSBVUIHGVYVIFSGEKRPHPEEFDTKVXISJQCAI9RTHOPXEGDJIAKLCMMKHAJOFDLBSMHJUINKCBEXFZSLSOUBH9IVKSCBEVMEYFHSEP9NFFCOTONHEIHGJWJBDGVYWLNOHGLOEVNKZVTDZMDYKUFEVZMJH9FXKBBAHQGF9LMVUWPQSXSADLG99IVUMRKQWBVNFJQKJMPWNHMCAERHJCOGBIJPGMLLMXKTYTEKLLGJKIJAHWTKCJHL"),
                             Tag = new Tag("ASDF"),
                             Timestamp = Timestamp.UnixSecondsTimestamp
                           });

      bundle.Finalize();
      bundle.Sign();

      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");  
      var mamFactory = new CurlMamFactory(new Curl(), new CurlMask());
      var treeFactory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGenerator(seed)));

      var channelFactory = new MamChannelFactory(mamFactory, treeFactory, new InMemoryIotaRepository());
      var channelKey = new TryteString("NXRZEZIKWGKIYDPVBRKWLYTWLUVSDLDCHVVSVIWDCIUZRAKPJUIABQDZBV9EGTJWUFTIGAUT9STIENCBC");
      var channel = channelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey);

      var message = channel.CreateMessage(new TryteString("IREALLYWANTTHISTOWORKINCSHARPASWELLPLEASEMAKEITHAPPEN"));

      Assert.AreEqual("ILUOWHDESMHWKVHTRVOF9YERPLWCXEZPJBYGLWOHWCRWMCS9GHICAZTKEZVN9JYQLBIWVXKSTWNZIUAVV", message.Root.Value);
      Assert.AreEqual("HJEDRSLPPDMJQLDWQAGF9CFDVOQTTVXWDRGMVQ9YJYUAQFZWFZZEV9WUP9NVHVPIEHRKZ9UDGAULMHWBK", message.Address.Value);
      Assert.AreEqual("UMESARRQWAHNSWYNZEGQYLBSOULJYCURIOBMMBIPVFKRJBYVMUXXWXZMCKQIPSMVGUVKIG9HROBWOGBIR", message.NextRoot.Value);

      Assert.AreEqual("UMESARRQWAHNSWYNZEGQYLBSOULJYCURIOBMMBIPVFKRJBYVMUXXWXZMCKQIPSMVGUVKIG9HROBWOGBIR", channel.NextRoot.Value);

      for (var i = 0; i < bundle.Transactions.Count; i++)
      {
        Assert.AreEqual(bundle.Transactions[i].Fragment.Value, message.Payload.Transactions[i].Fragment.Value);
      }
    }

    /// <summary>
    /// The test public message creation.
    /// </summary>
    [TestMethod]
    public void TestPublicMessageCreation()
    {
      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");
      var curlMask = new CurlMask();
      var mamFactory = new CurlMamFactory(new Curl(), curlMask);
      var treeFactory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGenerator(seed)));

      var channelFactory = new MamChannelFactory(mamFactory, treeFactory, new InMemoryIotaRepository());
      var channel = channelFactory.Create(Mode.Private, seed);

      var message = channel.CreateMessage(new TryteString("IREALLYWANTTHISTOWORKINCSHARPASWELLPLEASEMAKEITHAPPEN"));

      var expectedAddress = curlMask.Hash(message.Root);

      Assert.AreEqual("ILUOWHDESMHWKVHTRVOF9YERPLWCXEZPJBYGLWOHWCRWMCS9GHICAZTKEZVN9JYQLBIWVXKSTWNZIUAVV", message.Root.Value);
      Assert.AreEqual(expectedAddress.Value, message.Address.Value);
      Assert.AreEqual("UMESARRQWAHNSWYNZEGQYLBSOULJYCURIOBMMBIPVFKRJBYVMUXXWXZMCKQIPSMVGUVKIG9HROBWOGBIR", message.NextRoot.Value);

      Assert.AreEqual("UMESARRQWAHNSWYNZEGQYLBSOULJYCURIOBMMBIPVFKRJBYVMUXXWXZMCKQIPSMVGUVKIG9HROBWOGBIR", channel.NextRoot.Value);
    }
  }
}