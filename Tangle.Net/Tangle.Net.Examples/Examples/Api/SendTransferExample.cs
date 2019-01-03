namespace Tangle.Net.Examples.Examples.Api
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Utils;

  /// <inheritdoc />
  public class SendTransferExample : Example<Bundle>
  {
    /// <inheritdoc />
    public SendTransferExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// SendTransfer is used to send a transaction with value to any address. Use SendTransferAsync in async context.
    /// 
    /// 1. Get the seed that contains the value to be sent
    /// 2. Create a bundle with a transfer that contains the value and receiver address
    /// 3. Send the transfer. The method handles input addresses and remainder internally if not provided. Alternatively you can provide them manually
    public override Bundle Execute()
    {
      var seed = Seed.Random();

      var bundle = new Bundle();
      bundle.AddTransfer(
        new Transfer
          {
            Address = new Address("SOMEADDRESSVALUEGOESHERE"),
            Tag = Tag.Empty,
            ValueToTransfer = 100,
            Timestamp = Timestamp.UnixSecondsTimestamp
          });

      return this.Repository.SendTransfer(seed, bundle, SecurityLevel.Medium, 2);
    }
  }
}