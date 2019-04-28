namespace Tangle.Net.Area.Codes.Example
{
  using System;
  using System.Linq;
  using System.Threading.Tasks;

  using Google.OpenLocationCode;

  using Tangle.Net.Area.Codes.Entity;
  using Tangle.Net.Area.Codes.Repository;
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Factory;

  public static class Repository
  {
    private static IIotaRepository IotaRepository => IotaRepositoryFactory.Create("https://nodes.thetangle.org:443", PoWType.PoWSrv);

    public static async Task PublishAsync()
    {
      // Create area code for a region by lat/lng and the precision
      // This example points to Hannover, Germany with a precision of 3.5m x 3.5m
      var areaCode = IotaAreaCode.Encode(52.37052, 9.73322, OpenLocationCode.CodePrecisionExtra);
      var bundle = await IotaRepository.PublishWithAreaCodeAsync(
                     TryteString.FromAsciiString("Hello from Hannover! \n Have fun with geo locations on the tangle!"),
                     areaCode);

      Console.WriteLine($"Bundle published. Hash: {bundle.Hash}");
    }

    public static async Task FindAsync()
    {
      // Create area code for a region by lat/lng and the precision
      // This example points to Hannover, Germany with a precision of 3.5m x 3.5m
      var areaCode = IotaAreaCode.Encode(52.37052, 9.73322, OpenLocationCode.CodePrecisionExtra);
      var bundles = await IotaRepository.FindByAreaCodeAsync(areaCode);

      Console.WriteLine($"Bundle message:\n {bundles.First().GetMessages().First()}");
    }
  }
}