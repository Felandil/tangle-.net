namespace Tangle.Net.Mam.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Newtonsoft.Json;

  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Services;
  using Tangle.Net.Repository;

  /// <summary>
  /// The mam channel subscription.
  /// </summary>
  public class MamChannelSubscription
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MamChannelSubscription"/> class.
    /// </summary>
    /// <param name="repository">
    /// The iota repository.
    /// </param>
    /// <param name="parser">
    /// The parser.
    /// </param>
    /// <param name="mask">
    /// The mask.
    /// </param>
    public MamChannelSubscription(IIotaRepository repository, IMamParser parser, IMask mask)
    {
      this.Repository = repository;
      this.Parser = parser;
      this.Mask = mask;
    }

    /// <summary>
    /// Getsthe channel key.
    /// </summary>
    public TryteString Key { get; private set; }

    /// <summary>
    /// Gets the message root.
    /// </summary>
    public Hash MessageRoot { get; private set; }

    /// <summary>
    /// Gets the next root.
    /// </summary>
    public Hash NextRoot { get; private set; }

    /// <summary>
    /// Gets the mode.
    /// </summary>
    public Mode Mode { get; private set; }

    /// <summary>
    /// Gets the parser.
    /// </summary>
    private IMamParser Parser { get; }

    /// <summary>
    /// Gets the mask.
    /// </summary>
    private IMask Mask { get; }

    /// <summary>
    /// Gets the iota repository.
    /// </summary>
    private IIotaRepository Repository { get; }

    /// <summary>
    /// The fetch.
    /// </summary>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public async Task<List<UnmaskedAuthenticatedMessage>> FetchAsync()
    {
      this.NextRoot = this.MessageRoot;
      return await this.InternalFetch();
    }

    /// <summary>
    /// The fetch next.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task<List<UnmaskedAuthenticatedMessage>> FetchNext()
    {
      return await this.InternalFetch();
    }

    /// <summary>
    /// The fetch single.
    /// </summary>
    /// <param name="root">
    /// The root.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task<UnmaskedAuthenticatedMessage> FetchSingle(Hash root)
    {
      var address = this.Mode == Mode.Public ? new Address(root.Value) : new Address(this.Mask.Hash(root).Value);
      var transactionHashList = await this.Repository.FindTransactionsByAddressesAsync(new List<Address> { address });

      if (!transactionHashList.Hashes.Any())
      {
        return null;
      }

      var bundles = await this.Repository.GetBundlesAsync(transactionHashList.Hashes, false);
      foreach (var bundle in bundles)
      {
        try
        {
          return this.Parser.Unmask(bundle, root, this.Key);
        }
        catch (Exception exception)
        {
          if (exception is InvalidBundleException)
          {
            // TODO: Add invalid bundle handler
          }
        }
      }

      return null;
    }

    /// <summary>
    /// The init.
    /// </summary>
    /// <param name="messageRoot">
    /// The message root.
    /// </param>
    /// <param name="mode">
    /// The mode.
    /// </param>
    /// <param name="channelKey">
    /// The channel key.
    /// </param>
    /// <param name="nextRoot">
    /// The next Root.
    /// </param>
    public void Init(Hash messageRoot, Mode mode, TryteString channelKey = null, Hash nextRoot = null)
    {
      this.MessageRoot = messageRoot;
      this.Mode = mode;
      this.Key = channelKey;
      this.NextRoot = nextRoot;
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
    /// The internal fetch.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    private async Task<List<UnmaskedAuthenticatedMessage>> InternalFetch()
    {
      var result = new List<UnmaskedAuthenticatedMessage>();

      while (true)
      {
        var unmasked = await this.FetchSingle(this.NextRoot);

        if (unmasked != null)
        {
          this.NextRoot = unmasked.NextRoot;
          result.Add(unmasked);
        }
        else
        {
          break;
        }
      }

      return result;
    }
  }
}