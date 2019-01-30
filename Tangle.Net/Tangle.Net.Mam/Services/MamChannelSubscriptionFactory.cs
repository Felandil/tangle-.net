namespace Tangle.Net.Mam.Services
{
  using Newtonsoft.Json;

  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Repository;

  /// <summary>
  /// The mam channel subscription factory.
  /// </summary>
  public class MamChannelSubscriptionFactory
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MamChannelSubscriptionFactory"/> class.
    /// </summary>
    /// <param name="repository">
    /// The repository.
    /// </param>
    /// <param name="parser">
    /// The parser.
    /// </param>
    /// <param name="mask">
    /// The mask.
    /// </param>
    public MamChannelSubscriptionFactory(IIotaRepository repository, IMamParser parser, IMask mask)
    {
      this.Repository = repository;
      this.Parser = parser;
      this.Mask = mask;
    }

    /// <summary>
    /// Gets the parser.
    /// </summary>
    private IMamParser Parser { get; }

    /// <summary>
    /// Gets the mask.
    /// </summary>
    private IMask Mask { get; }

    /// <summary>
    /// Gets the repository.
    /// </summary>
    private IIotaRepository Repository { get; }

    /// <summary>
    /// The create.
    /// </summary>
    /// <param name="root">
    /// The message root.
    /// </param>
    /// <param name="mode">
    /// The mode.
    /// </param>
    /// <param name="channelKey">
    /// The channel key.
    /// </param>
    /// <returns>
    /// The <see cref="MamChannelSubscription"/>.
    /// </returns>
    public MamChannelSubscription Create(Hash root, Mode mode, string channelKey = null)
    {
      var channelSubscription = new MamChannelSubscription(this.Repository, this.Parser, this.Mask);
      channelSubscription.Init(root, mode, channelKey != null ? TryteString.FromAsciiString(channelKey) : Hash.Empty);

      return channelSubscription;
    }

    /// <summary>
    /// The create from json.
    /// </summary>
    /// <param name="serializedSubscription">
    /// The serialized subscription.
    /// </param>
    /// <returns>
    /// The <see cref="MamChannelSubscription"/>.
    /// </returns>
    public MamChannelSubscription CreateFromJson(string serializedSubscription)
    {
      var unserializedSubscriptionData = JsonConvert.DeserializeObject<dynamic>(serializedSubscription);

      var channelSubscription = new MamChannelSubscription(this.Repository, this.Parser, this.Mask);

      var nextRootValue = (string)unserializedSubscriptionData.NextRoot.Value;

      if (nextRootValue == null)
      {
        channelSubscription.Init(
          new Hash((string)unserializedSubscriptionData.MessageRoot.Value),
          (Mode)unserializedSubscriptionData.Mode,
          new TryteString((string)unserializedSubscriptionData.Key.Value));
      }
      else
      {
        channelSubscription.Init(
          new Hash((string)unserializedSubscriptionData.MessageRoot.Value),
          (Mode)unserializedSubscriptionData.Mode,
          new Hash((string)unserializedSubscriptionData.Key.Value),
          new Hash(nextRootValue));
      }



      return channelSubscription;
    }
  }
}