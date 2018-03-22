namespace Tangle.Net.Examples.Examples.Api
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Utils;

  /// <inheritdoc />
  public class PrepareTransferExample : Example<Bundle>
  {
    /// <inheritdoc />
    public PrepareTransferExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// PrepareTransfer prepares a bundle so it can be send to the tangle. Note that it does not attach the bundle to the tangle. Use ReplayBundleAsync in async context.
    /// 
    /// 1. Get the seed that contains the value to be sent
    /// 2. Create a bundle with a transfer that contains the value and receiver address
    /// 3. Finalize and sign the bundle. You've got to provide the KeyGenerator in order to sign a bundle that contains value
    /// 4. Prepare the transfer. The method handles input addresses and remainder internally if not provided. Alternatively you can provide them manually
    /// 
    /// The bundle can now be attached to the tangle via SendTrytes.
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

      bundle.Finalize();
      bundle.Sign();

      return this.Repository.PrepareTransfer(seed, bundle, SecurityLevel.Medium);
    }
  }
}