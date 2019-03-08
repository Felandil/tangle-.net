namespace Tangle.Net.Mam.Services
{
  using Newtonsoft.Json;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Repository;

  public class MamChannelFactory
  {
    public MamChannelFactory(IMamFactory mamFactory, IMerkleTreeFactory treeFactory, IIotaRepository repository)
    {
      this.MamFactory = mamFactory;
      this.TreeFactory = treeFactory;
      this.Repository = repository;
    }

    private IMamFactory MamFactory { get; }

    private IIotaRepository Repository { get; }

    private IMerkleTreeFactory TreeFactory { get; }

    public MamChannel Create(
      Mode mode,
      Seed seed,
      int securityLevel = SecurityLevel.Medium,
      string channelKey = null,
      bool keyIsTrytes = false,
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
        channelKey != null ? keyIsTrytes ? new TryteString(channelKey) : TryteString.FromAsciiString(channelKey) : Hash.Empty,
        nextRoot,
        index,
        count,
        nextCount,
        start);

      return channel;
    }

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