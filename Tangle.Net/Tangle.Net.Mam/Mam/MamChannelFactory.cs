namespace Tangle.Net.Mam.Mam
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
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
    /// <param name="parser">
    /// The parser.
    /// </param>
    /// <param name="treeFactory">
    /// The tree Factory.
    /// </param>
    /// <param name="repository">
    /// The repository.
    /// </param>
    public MamChannelFactory(IMamFactory mamFactory, IMamParser parser, IMerkleTreeFactory treeFactory, IIotaRepository repository)
    {
      this.MamFactory = mamFactory;
      this.Parser = parser;
      this.TreeFactory = treeFactory;
      this.Repository = repository;
    }

    /// <summary>
    /// Gets the mam factory.
    /// </summary>
    private IMamFactory MamFactory { get; }

    /// <summary>
    /// Gets the parser.
    /// </summary>
    private IMamParser Parser { get; }

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
    /// <returns>
    /// The <see cref="MamChannel"/>.
    /// </returns>
    public MamChannel Create(Mode mode, Seed seed, int securityLevel = SecurityLevel.Medium, TryteString channelKey = null)
    {
      var channel = new MamChannel(this.MamFactory, this.Parser, this.TreeFactory, this.Repository);
      channel.Init(mode, seed, securityLevel, channelKey);

      return channel;
    }
  }
}