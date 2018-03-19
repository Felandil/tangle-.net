namespace Tangle.Net.Mam.Mam
{
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
    public MamChannelSubscriptionFactory(IIotaRepository repository, IMamParser parser)
    {
      this.Repository = repository;
      this.Parser = parser;
    }

    /// <summary>
    /// Gets the parser.
    /// </summary>
    private IMamParser Parser { get; }

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
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="MamChannelSubscription"/>.
    /// </returns>
    public MamChannelSubscription Create(Hash root, Mode mode, TryteString channelKey, int securityLevel)
    {
      var channelSubscription = new MamChannelSubscription(this.Repository, this.Parser);
      channelSubscription.Init(root, mode, channelKey, securityLevel);

      return channelSubscription;
    }
  }
}