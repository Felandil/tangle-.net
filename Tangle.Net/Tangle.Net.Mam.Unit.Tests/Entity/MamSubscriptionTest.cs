namespace Tangle.Net.Mam.Unit.Tests.Mam
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
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
      var mask = new CurlMask();
      var subscriptionFactory = new MamChannelSubscriptionFactory(new InMemoryIotaRepository(), CurlMamParser.Default, mask);

      var root = new Hash(Seed.Random().Value);
      var channelKey = Seed.Random();
      var subscription = subscriptionFactory.Create(root, Mode.Restricted, channelKey.Value, true);

      var serializedSubscription = subscription.ToJson();

      var recreatedSubscription = subscriptionFactory.CreateFromJson(serializedSubscription);

      Assert.AreEqual(root.Value, recreatedSubscription.MessageRoot.Value);
    }
  }
}