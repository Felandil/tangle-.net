namespace Tangle.Net.Examples.Examples.Api
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Utils;

  /// <inheritdoc />
  public class SendTrytesExample : Example<List<TransactionTrytes>>
  {
    /// <inheritdoc />
    public SendTrytesExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// Send Trytes can be used to send messages to any address without providing a seed. Call SenTrytesAsync in async context
    /// 
    /// 1. Create a bundle and add a transfer without any value, but with a message
    /// 2. Finalize and sign the bundle
    /// 3. Send the bundle. Note that the minWeightMagnitude was set to 14 for faster PoW
    public override List<TransactionTrytes> Execute()
    {
      var bundle = new Bundle();
      bundle.AddTransfer(new Transfer
                           {
                             Address = new Address("ENTERAVALIDADDRESSHERE"),
                             Message = TryteString.FromUtf8String("Hello there!"),
                             Tag = Tag.Empty,
                             Timestamp = Timestamp.UnixSecondsTimestamp
                           });

      bundle.Finalize();
      bundle.Sign();

      return this.Repository.SendTrytes(bundle.Transactions, 2);
    }
  }
}