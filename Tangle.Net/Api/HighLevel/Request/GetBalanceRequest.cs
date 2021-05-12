using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Api.HighLevel.Request
{
  public class GetBalanceRequest
  {
    public GetBalanceRequest(Ed25519Seed seed, int accountIndex, AddressOptions addressOptions)
    {
      Seed = seed;
      AccountIndex = accountIndex;
      AddressOptions = addressOptions;
    }

    public Ed25519Seed Seed { get; }

    /// <summary>
    /// The account index in the wallet
    /// </summary>
    public int AccountIndex { get; }
    public AddressOptions AddressOptions { get; }
  }
}