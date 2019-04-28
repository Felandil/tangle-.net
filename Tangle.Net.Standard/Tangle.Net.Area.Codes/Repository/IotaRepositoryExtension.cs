namespace Tangle.Net.Area.Codes.Repository
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Tangle.Net.Area.Codes.Entity;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Utils;

  public static class IotaRepositoryExtension
  {
    public static async Task<List<Bundle>> FindByAreaCodeAsync(this IIotaRepository repository, IotaAreaCode areaCode)
    {
      var result = await repository.FindTransactionsByTagsAsync(new List<Tag> { new Tag(areaCode.Value) });
      if (!result.Hashes.Any())
      {
        return new List<Bundle>();
      }

      var transactionTrytes = await repository.GetTrytesAsync(result.Hashes);
      var transactions = transactionTrytes.Select(t => Transaction.FromTrytes(t)).ToList();

      var bundles = new List<Bundle>();
      foreach (var transaction in transactions)
      {
        if (bundles.Any(b => b.Hash.Value == transaction.BundleHash.Value))
        {
          bundles.First(b => b.Hash.Value == transaction.BundleHash.Value).Transactions.Add(transaction);
        }
        else
        {
          var bundle = new Bundle();
          bundle.Transactions.Add(transaction);

          bundles.Add(bundle);
        }
      }

      bundles.ForEach(b => b.Transactions = b.Transactions.OrderBy(t => t.CurrentIndex).ToList());

      return bundles;
    }

    public static async Task<Bundle> PublishWithAreaCodeAsync(this IIotaRepository repository, TryteString message, IotaAreaCode areaCode, Address address = null)
    {
      if (address == null)
      {
        address = new Address(Seed.Random().Value);
      }

      var bundle = new Bundle();
      bundle.AddTransfer(new Transfer
                           {
                             Address = address,
                             Message = message,
                             Tag = new Tag(areaCode.Value),
                             Timestamp = Timestamp.UnixSecondsTimestamp
                           });

      bundle.Finalize();
      bundle.Sign();

      var transactionTrytes = await repository.SendTrytesAsync(bundle.Transactions, 2);
      return Bundle.FromTransactionTrytes(transactionTrytes, bundle.Hash);
    }
  }
}