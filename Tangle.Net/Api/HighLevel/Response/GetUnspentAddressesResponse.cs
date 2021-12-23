using System.Collections.Generic;
using Tangle.Net.Entity.Bech32;

namespace Tangle.Net.Api.HighLevel.Response
{
  public class GetUnspentAddressesResponse
  {
    public GetUnspentAddressesResponse(List<Bech32Address> unspentAddresses)
    {
      UnspentAddresses = unspentAddresses;
    }

    public List<Bech32Address> UnspentAddresses { get; }
  }
}