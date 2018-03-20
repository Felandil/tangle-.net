namespace Tangle.Net.Mam.Entity
{
  using System.Threading.Tasks;

  using Newtonsoft.Json;

  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Mam.Services;
  using Tangle.Net.Repository;

  /// <summary>
  /// The mam channel.
  /// </summary>
  public class MamChannel
  { 
    /// <summary>
    /// Initializes a new instance of the <see cref="MamChannel"/> class.
    /// </summary>
    /// <param name="mamFactory">
    /// The mam Factory.
    /// </param>
    /// <param name="treeFactory">
    /// The tree Factory.
    /// </param>
    /// <param name="repository">
    /// The repository.
    /// </param>
    public MamChannel(IMamFactory mamFactory, IMerkleTreeFactory treeFactory, IIotaRepository repository)
    {
      this.MamFactory = mamFactory;
      this.TreeFactory = treeFactory;
      this.Repository = repository;
    }

    /// <summary>
    /// Gets the seed.
    /// </summary>
    public Seed Seed { get; private set; }

    /// <summary>
    /// Gets the mode.
    /// </summary>
    public Mode Mode { get; private set; }

    /// <summary>
    /// Gets the security level.
    /// </summary>
    public int SecurityLevel { get; private set; }

    /// <summary>
    /// Gets the key.
    /// </summary>
    public TryteString Key { get; private set; }

    /// <summary>
    /// Gets the next root.
    /// </summary>
    public Hash NextRoot { get; private set; }

    /// <summary>
    /// Gets the start.
    /// </summary>
    public int Start { get; private set; }

    /// <summary>
    /// Gets the next count.
    /// </summary>
    public int NextCount { get; private set; }

    /// <summary>
    /// Gets the count.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Gets the index.
    /// </summary>
    public int Index { get; private set; }

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
    /// The create message.
    /// </summary>
    /// <param name="message">
    /// The tryte string.
    /// </param>
    /// <returns>
    /// The <see cref="MaskedAuthenticatedMessage"/>.
    /// </returns>
    public MaskedAuthenticatedMessage CreateMessage(TryteString message)
    {
      var tree = this.TreeFactory.Create(this.Seed, this.Start, this.Count, Cryptography.SecurityLevel.Medium);
      var nextRootTree = this.TreeFactory.Create(this.Seed, this.Start + this.Count, this.NextCount, Cryptography.SecurityLevel.Medium);
      var maskedAutheticatedMessage = this.MamFactory.Create(tree, this.Index, message, nextRootTree.Root.Hash, this.Key);

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


    /// <summary>
    /// The publish.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task PublishAsync(MaskedAuthenticatedMessage message)
    {
      await this.Repository.SendTrytesAsync(message.Payload.Transactions, 6, 14);
    }

    /// <summary>
    /// The to json.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string ToJson()
    {
      return JsonConvert.SerializeObject(this);
    }

    /// <summary>
    /// The init.
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
  }
}