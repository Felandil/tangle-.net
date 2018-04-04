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
    /// Gets the security level.
    /// </summary>
    public int SecurityLevel { get; private set; }

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
    /// The init.
    /// </summary>
    /// <param name="messageRoot">
    /// The message root.
    /// </param>
    /// <param name="mode">
    /// The mode.
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
    public void Init(Hash messageRoot, Mode mode, int securityLevel = 2, TryteString channelKey = null, Hash nextRoot = null)
    {
      this.MessageRoot = messageRoot;
      this.Mode = mode;
      this.Key = channelKey;
      this.SecurityLevel = securityLevel;
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
        Address address;
        TryteString decryptionKey;

        if (this.Mode == Mode.Public)
        {
          address = new Address(this.NextRoot.Value);
          decryptionKey = this.NextRoot;
        }
        else
        {
          address = new Address(this.Mask.Hash(this.NextRoot).Value);
          decryptionKey = this.Mode == Mode.Restricted ? this.Key : this.NextRoot;
        }

        var transactionHashList = await this.Repository.FindTransactionsByAddressesAsync(new List<Address> { address });

        if (!transactionHashList.Hashes.Any())
        {
          break;
        }

        var bundles = await this.Repository.GetBundlesAsync(transactionHashList.Hashes, false);
        for (var i = 0; i < bundles.Count; i++)
        {
          try
          {
            var unmaskedMessage = this.Parser.Unmask(bundles[i], this.MessageRoot, decryptionKey);
            this.NextRoot = unmaskedMessage.NextRoot;
            result.Add(unmaskedMessage);
          }
          catch (Exception exception)
          {
            if (exception is InvalidBundleException)
            {
              // TODO: Add invalid bundle handler
            }

            if (i + 1 == bundles.Count)
            {
              return result;
            }
          }
        }
      }

      return result;
    }
  }
}