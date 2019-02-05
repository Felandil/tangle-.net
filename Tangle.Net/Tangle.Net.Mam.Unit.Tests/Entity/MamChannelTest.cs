namespace Tangle.Net.Mam.Unit.Tests.Mam
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Mam.Services;
  using Tangle.Net.ProofOfWork.HammingNonce;
  using Tangle.Net.Utils;

  using Mode = Tangle.Net.Mam.Entity.Mode;

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

      var channelFactory = new MamChannelFactory(CurlMamFactory.Default, CurlMerkleTreeFactory.Default, new InMemoryIotaRepository());
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

      var channelFactory = new MamChannelFactory(CurlMamFactory.Default, CurlMerkleTreeFactory.Default, new InMemoryIotaRepository());
      var channelKey = new TryteString("NXRZEZIKWGKIYDPVBRKWLYTWLUVSDLDCHVVSVIWDCIUZRAKPJUIABQDZBV9EGTJWUFTIGAUT9STIENCBC");
      var channel = channelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey.Value);

      Assert.AreEqual(seed.Value, channel.Seed.Value);
      Assert.AreEqual(Mode.Restricted, channel.Mode);
      Assert.AreEqual(SecurityLevel.Medium, channel.SecurityLevel);
      Assert.AreEqual(TryteString.FromAsciiString(channelKey.Value).Value, channel.Key.Value);
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
    public void TestRestrictedMessageCreation64Bit()
    {
      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");

      var mamFactory = new CurlMamFactory(
        new Curl(CurlMode.CurlP27),
        new CurlMask(),
        new IssSigningHelper(new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27), new Curl(CurlMode.CurlP27)),
        new HammingNonceDiver(CurlMode.CurlP27));
      var channelFactory = new MamChannelFactory(mamFactory, CurlMerkleTreeFactory.Default, new InMemoryIotaRepository());
      var channelKey = new TryteString("NXRZEZIKWGKIYDPVBRKWLYTWLUVSDLDCHVVSVIWDCIUZRAKPJUIABQDZBV9EGTJWUFTIGAUT9STIENCBC");
      var channel = channelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey.Value);

      var message = channel.CreateMessage(new TryteString("IREALLYWANTTHISTOWORKINCSHARPASWELLPLEASEMAKEITHAPPEN"));

      Assert.AreEqual("RRPXQHDJY9BKXC9NGHDCSHRIDYORSUUEPFHXPQVDGSQTVYPCGVIZRWQINOUYFDUXTHFTKHLBOLYLHMKE9", message.Root.Value);
      Assert.AreEqual("BAVSMNXFTVBBEPXVROQYWBFHAELANDS9UFLDEOERJGKMXOGTL9UBEJF9WUDNGKUEDFZYAAFACRRRACDHV", message.Address.Value);
      Assert.AreEqual("OLHRFQPHPPQWTVSZNIZEKFOB9JPWKWQQPUCNLFAVEYCL9QVXRWFTDT9KPIHERRULOOBUKTJZJWKENTPLO", message.NextRoot.Value);

      Assert.AreEqual("OLHRFQPHPPQWTVSZNIZEKFOB9JPWKWQQPUCNLFAVEYCL9QVXRWFTDT9KPIHERRULOOBUKTJZJWKENTPLO", channel.NextRoot.Value);
    }

    /// <summary>
    /// The test restricted message creation.
    /// </summary>
    [TestMethod]
    public void TestRestrictedMessageCreation32Bit()
    {
      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");

      var channelFactory = new MamChannelFactory(CurlMamFactory.Default, CurlMerkleTreeFactory.Default, new InMemoryIotaRepository());
      var channelKey = new TryteString("NXRZEZIKWGKIYDPVBRKWLYTWLUVSDLDCHVVSVIWDCIUZRAKPJUIABQDZBV9EGTJWUFTIGAUT9STIENCBC");
      var channel = channelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey.Value);

      var message = channel.CreateMessage(new TryteString("IREALLYWANTTHISTOWORKINCSHARPASWELLPLEASEMAKEITHAPPEN"));

      Assert.AreEqual("RRPXQHDJY9BKXC9NGHDCSHRIDYORSUUEPFHXPQVDGSQTVYPCGVIZRWQINOUYFDUXTHFTKHLBOLYLHMKE9", message.Root.Value);
      Assert.AreEqual("BAVSMNXFTVBBEPXVROQYWBFHAELANDS9UFLDEOERJGKMXOGTL9UBEJF9WUDNGKUEDFZYAAFACRRRACDHV", message.Address.Value);
      Assert.AreEqual("OLHRFQPHPPQWTVSZNIZEKFOB9JPWKWQQPUCNLFAVEYCL9QVXRWFTDT9KPIHERRULOOBUKTJZJWKENTPLO", message.NextRoot.Value);

      Assert.AreEqual("OLHRFQPHPPQWTVSZNIZEKFOB9JPWKWQQPUCNLFAVEYCL9QVXRWFTDT9KPIHERRULOOBUKTJZJWKENTPLO", channel.NextRoot.Value);
    }

    /// <summary>
    /// The test public message creation.
    /// </summary>
    [TestMethod]
    public void TestPublicMessageCreation()
    {
      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");
      var curlMask = new CurlMask();

      var channelFactory = new MamChannelFactory(CurlMamFactory.Default, CurlMerkleTreeFactory.Default, new InMemoryIotaRepository());
      var channel = channelFactory.Create(Mode.Private, seed);

      var message = channel.CreateMessage(new TryteString("IREALLYWANTTHISTOWORKINCSHARPASWELLPLEASEMAKEITHAPPEN"));
      var expectedAddress = curlMask.Hash(message.Root);

      Assert.AreEqual("RRPXQHDJY9BKXC9NGHDCSHRIDYORSUUEPFHXPQVDGSQTVYPCGVIZRWQINOUYFDUXTHFTKHLBOLYLHMKE9", message.Root.Value);
      Assert.AreEqual(expectedAddress.Value, message.Address.Value);
      Assert.AreEqual("OLHRFQPHPPQWTVSZNIZEKFOB9JPWKWQQPUCNLFAVEYCL9QVXRWFTDT9KPIHERRULOOBUKTJZJWKENTPLO", message.NextRoot.Value);

      Assert.AreEqual("OLHRFQPHPPQWTVSZNIZEKFOB9JPWKWQQPUCNLFAVEYCL9QVXRWFTDT9KPIHERRULOOBUKTJZJWKENTPLO", channel.NextRoot.Value);
    }
  }
}