using System.Collections.Generic;
using Tangle.Net.Entity.Bech32;
using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Api.HighLevel.Request
{
  public class SendTransferRequest : SendDataRequest
  {
    public SendTransferRequest(Ed25519Seed seed, List<TransferOutput> outputs, int accountIndex = 0,
      AddressOptions addressOptions = null,
      string indexationKey = "", string data = "") : base(indexationKey, data)
    {
      Seed = seed;
      AccountIndex = accountIndex;
      Outputs = outputs;
      AddressOptions = addressOptions ?? new AddressOptions();
    }

    public SendTransferRequest(Ed25519Seed seed, TransferOutput output, int accountIndex = 0,
      AddressOptions addressOptions = null,
      string indexationKey = "", string data = "") : this(seed, new List<TransferOutput> {output}, accountIndex,
      addressOptions, indexationKey, data)
    {
    }

    public SendTransferRequest(Ed25519Seed seed, Ed25519Address receiver, long amount) : this(seed,
      new TransferOutput(receiver, amount))
    {
    }

    public SendTransferRequest(Ed25519Seed seed, string receiverBech32Address, long amount) : this(seed,
      receiverBech32Address, amount, "", "")
    {
    }

    public SendTransferRequest(Ed25519Seed seed, string receiverBech32Address, long amount, string indexationKey,
      string data) : base(indexationKey, data)
    {
      Seed = seed;
      AccountIndex = 0;
      Outputs = new List<TransferOutput>
        {new TransferOutput(new Bech32Address(receiverBech32Address), amount)};
      AddressOptions = new AddressOptions();
    }

    public Ed25519Seed Seed { get; }
    public int AccountIndex { get; }

    public AddressOptions AddressOptions { get; }

    public List<TransferOutput> Outputs { get; }
  }
}