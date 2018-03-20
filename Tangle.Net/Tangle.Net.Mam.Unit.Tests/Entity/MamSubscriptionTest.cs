namespace Tangle.Net.Mam.Unit.Tests.Mam
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Mam.Services;

  /// <summary>
  /// The mam subscription test.
  /// </summary>
  [TestClass]
  public class MamSubscriptionTest
  {
    /// <summary>
    /// The test subscription creation and recreation.
    /// </summary>
    [TestMethod]
    public void TestSubscriptionCreationAndRecreation()
    {
      var subscriptionFactory = new MamChannelSubscriptionFactory(
        new InMemoryIotaRepository(),
        new CurlMamParser(
          new CurlMask(),
          new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGenerator())),
          new Curl()));

      var root = new Hash(Seed.Random().Value);
      var channelKey = Seed.Random();
      var subscription = subscriptionFactory.Create(root, Mode.Restricted, channelKey, SecurityLevel.Medium);

      var serializedSubscription = subscription.ToJson();

      var recreatedSubscription = subscriptionFactory.CreateFromJson(serializedSubscription);

      Assert.AreEqual(root.Value, recreatedSubscription.MessageRoot.Value);
    }
  }
}