namespace Tangle.Net.Examples.Examples
{
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Utils;

  /// <summary>
  /// The send trytes example.
  /// </summary>
  public class SendTrytesExample : Example
  {
    /// <inheritdoc />
    public SendTrytesExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// 1. Create a bundle and add a transfer without any value, but with a message
    /// 2. Finalize and sign the bundle
    /// 3. Send the bundle. Note that the minWeightMagnitude was set to 14 for faster PoW
    /// 
    /// Note: Send Trytes can be used to send messages to any address without providing a seed.
    public override void Execute()
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

      this.Repository.SendTrytes(bundle.Transactions, 27, 14);
    }
  }
}