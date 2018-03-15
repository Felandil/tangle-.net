namespace Tangle.Net.Mam.Mam
{
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Merkle;

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
    internal MamChannel(IMamFactory mamFactory, IMerkleTreeFactory treeFactory)
    {
      this.MamFactory = mamFactory;
      this.TreeFactory = treeFactory;
    }

    /// <summary>
    /// Gets the seed.
    /// </summary>
    public Seed Seed { get; private set; }

    /// <summary>
    /// Gets the mode.
    /// </summary>
    public MamMode Mode { get; private set; }

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
    internal void Init(MamMode mode, Seed seed, int securityLevel = Cryptography.SecurityLevel.Medium, TryteString channelKey = null)
    {
      this.Mode = mode;
      this.Seed = seed;
      this.SecurityLevel = securityLevel;
      this.Key = channelKey;
      this.NextRoot = Hash.Empty;
      this.Index = 0;
      this.Count = 1;
      this.NextCount = 1;
      this.Start = 0;
    }
  }
}