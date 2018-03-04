namespace Tangle.Net.Examples.Examples
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Responses;

  /// <summary>
  /// The find used addresses with transactions example.
  /// </summary>
  public class FindUsedAddressesWithTransactionsExample : Example<FindUsedAddressesResponse>
  {
    /// <inheritdoc />
    public FindUsedAddressesWithTransactionsExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// FindUsedAddressesWithTransactions can be used to get any address that transactions are associated with. Use FindUsedAddressesWithTransactionsAsync in async context.
    public override FindUsedAddressesResponse Execute()
    {
      var seed = new Seed("SOMENICENOTONLINEGENERATEDSEED");
      return this.Repository.FindUsedAddressesWithTransactions(seed, SecurityLevel.Medium, 0);
    }
  }
}