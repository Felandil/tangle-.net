namespace Tangle.Net.Mam.Services
{
  using Newtonsoft.Json;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Repository;

  /// <summary>
  /// The mam channel factory.
  /// </summary>
  public class MamChannelFactory
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MamChannelFactory"/> class.
    /// </summary>
    /// <param name="mamFactory">
    /// The mam factory.
    /// </param>
    /// <param name="treeFactory">
    /// The tree Factory.
    /// </param>
    /// <param name="repository">
    /// The repository.
    /// </param>
    public MamChannelFactory(IMamFactory mamFactory, IMerkleTreeFactory treeFactory, IIotaRepository repository)
    {
      this.MamFactory = mamFactory;
      this.TreeFactory = treeFactory;
      this.Repository = repository;
    }

    /// <summary>
    /// Gets the mam factory.
    /// </summary>
    private IMamFactory MamFactory { get; }

    /// <summary>
    /// Gets the tree factory.
    /// </summary>
    private IMerkleTreeFactory TreeFactory { get; }

    /// <summary>
    /// Gets the repository.
    /// </summary>
    private IIotaRepository Repository { get; }

    /// <summary>
    /// The create.
    /// </summary>
    /// <param name="mode">
    /// The mode.
    /// </param>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <param name="channelKey">
    /// The channel key.
    /// </param>
    /// <param name="nextRoot">
    /// The next Root.
    /// </param>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <param name="nextCount">
    /// The next Count.
    /// </param>
    /// <param name="start">
    /// The start.
    /// </param>
    /// <returns>
    /// The <see cref="MamChannel"/>.
    /// </returns>
    public MamChannel Create(
      Mode mode,
      Seed seed,
      int securityLevel = SecurityLevel.Medium,
      string channelKey = null,
      Hash nextRoot = null,
      int index = 0,
      int count = 1,
      int nextCount = 1,
      int start = 0)
    {
      var channel = new MamChannel(this.MamFactory, this.TreeFactory, this.Repository);
      channel.Init(
        mode,
        seed,
        securityLevel,
        channelKey != null ? TryteString.FromAsciiString(channelKey) : Hash.Empty,
        nextRoot,
        index,
        count,
        nextCount,
        start);

      return channel;
    }

    /// <summary>
    /// The create from json.
    /// </summary>
    /// <param name="serializedChannel">
    /// The serialized channel.
    /// </param>
    /// <returns>
    /// The <see cref="MamChannel"/>.
    /// </returns>
    public MamChannel CreateFromJson(string serializedChannel)
    {
      var unserializedChannelData = JsonConvert.DeserializeObject<dynamic>(serializedChannel);

      var channel = new MamChannel(this.MamFactory, this.TreeFactory, this.Repository);

      channel.Init(
        (Mode)unserializedChannelData.Mode,
        new Seed((string)unserializedChannelData.Seed.Value),
        (int)unserializedChannelData.SecurityLevel,
        new TryteString((string)unserializedChannelData.Key.Value),
        new Hash((string)unserializedChannelData.NextRoot.Value),
        (int)unserializedChannelData.Index,
        (int)unserializedChannelData.Count,
        (int)unserializedChannelData.NextCount,
        (int)unserializedChannelData.Start);

      return channel;
    }
  }
}