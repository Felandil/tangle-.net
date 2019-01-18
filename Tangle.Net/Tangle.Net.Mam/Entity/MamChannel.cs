namespace Tangle.Net.Mam.Entity
{
  using System.Threading.Tasks;

  using Newtonsoft.Json;

  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Mam.Services;
  using Tangle.Net.Repository;

  public class MamChannel
  {
    public MamChannel(IMamFactory mamFactory, IMerkleTreeFactory treeFactory, IIotaRepository repository)
    {
      this.MamFactory = mamFactory;
      this.TreeFactory = treeFactory;
      this.Repository = repository;
    }

    public int Count { get; private set; }

    public int Index { get; private set; }

    public TryteString Key { get; private set; }

    public Mode Mode { get; private set; }

    public int NextCount { get; private set; }

    public Hash NextRoot { get; private set; }

    public int SecurityLevel { get; private set; }

    public Seed Seed { get; private set; }

    public int Start { get; private set; }

    private IMamFactory MamFactory { get; }

    private IIotaRepository Repository { get; }

    private IMerkleTreeFactory TreeFactory { get; }

    public MaskedAuthenticatedMessage CreateMessage(TryteString message)
    {
      var tree = this.TreeFactory.Create(this.Seed, this.Start, this.Count, this.SecurityLevel);
      var nextRootTree = this.TreeFactory.Create(this.Seed, this.Start + this.Count, this.NextCount, this.SecurityLevel);

      var maskedAutheticatedMessage = this.MamFactory.Create(
        tree,
        this.Index,
        message,
        nextRootTree.Root.Hash,
        this.Mode != Mode.Restricted ? tree.Root.Hash : this.Key,
        this.Mode,
        this.SecurityLevel);

      if (this.Index == this.Count - 1)
      {
        this.Start = this.NextCount + this.Start;
        this.Index = 0;
      }
      else
      {
        this.Index++;
      }

      this.NextRoot = maskedAutheticatedMessage.NextRoot;

      return maskedAutheticatedMessage;
    }

    public void Init(
      Mode mode,
      Seed seed,
      int securityLevel = Cryptography.SecurityLevel.Medium,
      TryteString channelKey = null,
      Hash nextRoot = null,
      int index = 0,
      int count = 1,
      int nextCount = 1,
      int start = 0)
    {
      this.Mode = mode;
      this.Seed = seed;
      this.SecurityLevel = securityLevel;
      this.Key = channelKey;
      this.NextRoot = nextRoot ?? Hash.Empty;
      this.Index = index;
      this.Count = count;
      this.NextCount = nextCount;
      this.Start = start;
    }

    public async Task PublishAsync(MaskedAuthenticatedMessage message, int minWeightMagnitude = 14, int depth = 2)
    {
      await this.Repository.SendTrytesAsync(message.Payload.Transactions, depth, minWeightMagnitude);
    }

    public string ToJson()
    {
      return JsonConvert.SerializeObject(this);
    }
  }
}