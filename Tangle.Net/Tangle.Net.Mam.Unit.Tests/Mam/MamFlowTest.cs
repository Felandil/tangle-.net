namespace Tangle.Net.Mam.Unit.Tests.Mam
{
  using System.Threading.Tasks;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Mam;
  using Tangle.Net.Mam.Merkle;

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

      var channelFactory = new MamChannelFactory(mamFactory, new CurlMamParser(new CurlMask()), treeFactory, new InMemoryIotaRepository());
      var channelKey = new TryteString("NXRZEZIKWGKIYDPVBRKWLYTWLUVSDLDCHVVSVIWDCIUZRAKPJUIABQDZBV9EGTJWUFTIGAUT9STIENCBC");
      var channel = channelFactory.Create(Mode.Restricted, seed, SecurityLevel.Medium, channelKey);

      var message = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone!"));
      await channel.PublishAsync(message);

      var messageTwo = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone the second!"));
      await channel.PublishAsync(messageTwo);

      var messageThree = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone the third!"));
      await channel.PublishAsync(messageThree);

      var messageFour = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone the fourth!"));
      await channel.PublishAsync(messageFour);

      var messageFive = channel.CreateMessage(TryteString.FromUtf8String("Hello everyone the fifth!"));
      await channel.PublishAsync(messageFive);

      var unmaskedMessages = await channel.FetchAsync(message.Root, Mode.Restricted, channelKey, SecurityLevel.Medium);

      Assert.AreEqual("Hello everyone!", unmaskedMessages[0].Message.ToUtf8String());
      Assert.AreEqual(message.NextRoot.Value, unmaskedMessages[0].NextRoot.Value);
      Assert.AreEqual(message.Root.Value, unmaskedMessages[0].Root.Value);
      Assert.AreEqual("Hello everyone the second!", unmaskedMessages[1].Message.ToUtf8String());
      Assert.AreEqual("Hello everyone the third!", unmaskedMessages[2].Message.ToUtf8String());
      Assert.AreEqual("Hello everyone the fourth!", unmaskedMessages[3].Message.ToUtf8String());
      Assert.AreEqual("Hello everyone the fifth!", unmaskedMessages[4].Message.ToUtf8String());
    }
  }
}