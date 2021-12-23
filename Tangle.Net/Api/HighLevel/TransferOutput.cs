using Tangle.Net.Entity.Bech32;
using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Api.HighLevel
{
  public class TransferOutput
  {
    public TransferOutput(Bech32Address receiver, long amount)
    {
      Receiver = receiver.ToEd25519Address();
      Amount = amount;
    }

    public TransferOutput(Ed25519Address receiver, long amount)
    {
      Receiver = receiver;
      Amount = amount;
    }

    public long Amount { get; }
    public Ed25519Address Receiver { get; }
  }
}