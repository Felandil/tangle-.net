namespace Tangle.Net.Examples
{
  using System.Collections.Generic;

  using Tangle.Net.ProofOfWork.Service;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Client;

  public static class Utils
  {
    public static IIotaRepository Repository =>
      new RestIotaRepository(
        new FallbackIotaClient(
          new List<string>
            {
              "https://pow2.iota.community:443",
              "https://pow3.iota.community:443",
              "https://pow4.iota.community:443",
              "https://pow5.iota.community:443",
            },
          5000),
        new PoWSrvService());
  }
}