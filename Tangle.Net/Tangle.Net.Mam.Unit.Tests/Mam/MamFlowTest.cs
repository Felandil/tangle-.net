namespace Tangle.Net.Mam.Unit.Tests.Mam
{
  using System.Threading.Tasks;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Mam;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Utils;

  /// <summary>
  /// The curl mam factory test.
  /// </summary>
  [TestClass]
  public class MamFlowTest
  {
    /// <summary>
    /// The test mam creation.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    [TestMethod]
    public async Task TestMamCreationAndDecryption()
    {
      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");
      var mamFactory = new CurlMamFactory(new Curl(), new CurlMask());
      var treeFactory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGenerator(seed)));

      var iotaRepository = new InMemoryIotaRepository();
      var channelFactory = new MamChannelFactory(mamFactory, treeFactory, iotaRepository);
      var channelKey = new TryteString("NXRZEZIKWGKIYDPVBRKWLYTWLUVSDLDCHVVSVIWDCIUZRAKPJUIABQDZBV9EGTJWUFTIGAUT9STIENCBC");
      var channel = channelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey);

      var message = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone!"));
      await channel.PublishAsync(message);

      var messageTwo = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone the second!"));
      await channel.PublishAsync(messageTwo);

      var messageThree = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone the third!"));
      await channel.PublishAsync(messageThree);

      var subcriptionFactory = new MamChannelSubscriptionFactory(iotaRepository, new CurlMamParser(new CurlMask(), treeFactory, new Curl()));
      var subscription = subcriptionFactory.Create(message.Root, Mode.Restricted, channelKey, SecurityLevel.Medium);

      var unmaskedMessages = await subscription.FetchAsync();

      Assert.AreEqual("Hello everyone!", unmaskedMessages[0].Message.ToUtf8String());
      Assert.AreEqual(message.NextRoot.Value, unmaskedMessages[0].NextRoot.Value);
      Assert.AreEqual(message.Root.Value, unmaskedMessages[0].Root.Value);
      Assert.AreEqual("Hello everyone the second!", unmaskedMessages[1].Message.ToUtf8String());
      Assert.AreEqual("Hello everyone the third!", unmaskedMessages[2].Message.ToUtf8String());
    }

    /// <summary>
    /// The test mam channel recreation.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    [TestMethod]
    public async Task TestMamChannelRecreation()
    {
      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");
      var mamFactory = new CurlMamFactory(new Curl(), new CurlMask());
      var treeFactory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGenerator(seed)));

      var channelFactory = new MamChannelFactory(mamFactory, treeFactory, new InMemoryIotaRepository());
      var channelKey = new TryteString("NXRZEZIKWGKIYDPVBRKWLYTWLUVSDLDCHVVSVIWDCIUZRAKPJUIABQDZBV9EGTJWUFTIGAUT9STIENCBC");
      var channel = channelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey);

      var message = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone!"));
      await channel.PublishAsync(message);

      var serializedChannel = channel.ToJson();
      var unserializedChannel = channelFactory.CreateFromJson(serializedChannel);

      Assert.AreEqual(channel.Index, unserializedChannel.Index);
      Assert.AreEqual(channel.Start, unserializedChannel.Start);
      Assert.AreEqual(channel.NextCount, unserializedChannel.NextCount);
      Assert.AreEqual(channel.Count, unserializedChannel.Count);
      Assert.AreEqual(channel.NextRoot.Value, unserializedChannel.NextRoot.Value);
      Assert.AreEqual(channel.Seed.Value, unserializedChannel.Seed.Value);
      Assert.AreEqual(channel.SecurityLevel, unserializedChannel.SecurityLevel);
      Assert.AreEqual(channel.Key.Value, unserializedChannel.Key.Value);
      Assert.AreEqual(channel.Mode, unserializedChannel.Mode);
    }

    /// <summary>
    /// The test invalid message on mam stream should be ignored.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    [TestMethod]
    public async Task TestInvalidMessageOnMamStreamShouldBeIgnored()
    {
      var seed = new Seed("JETCPWLCYRM9XYQMMZIFZLDBZZEWRMRVGWGGNCUH9LFNEHKEMLXAVEOFFVOATCNKVKELNQFAGOVUNWEJI");
      var mamFactory = new CurlMamFactory(new Curl(), new CurlMask());
      var treeFactory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGenerator(seed)));

      var iotaRepository = new InMemoryIotaRepository();
      var channelFactory = new MamChannelFactory(mamFactory, treeFactory, iotaRepository);
      var channelKey = new TryteString("NXRZEZIKWGKIYDPVBRKWLYTWLUVSDLDCHVVSVIWDCIUZRAKPJUIABQDZBV9EGTJWUFTIGAUT9STIENCBC");
      var channel = channelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey);

      var message = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone!"));
      await channel.PublishAsync(message);

      var bundle = new Bundle();
      bundle.AddTransfer(new Transfer
                           {
                             Address = message.Address,
                             Message = TryteString.FromUtf8String("I do not belong here and should be ignored!"),
                             Tag = Tag.Empty,
                             Timestamp = Timestamp.UnixSecondsTimestamp
                           });

      bundle.Finalize();
      bundle.Sign();

      await iotaRepository.SendTrytesAsync(bundle.Transactions, 27, 14);

      var subcriptionFactory = new MamChannelSubscriptionFactory(iotaRepository, new CurlMamParser(new CurlMask(), treeFactory, new Curl()));
      var subscription = subcriptionFactory.Create(message.Root, Mode.Restricted, channelKey, SecurityLevel.Medium);

      var unmaskedMessages = await subscription.FetchAsync();

      Assert.AreEqual(1, unmaskedMessages.Count);
      Assert.AreEqual("Hello everyone!", unmaskedMessages[0].Message.ToUtf8String());
    }
  }
}