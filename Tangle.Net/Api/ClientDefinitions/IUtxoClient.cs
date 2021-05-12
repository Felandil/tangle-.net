using System.Threading.Tasks;
using Tangle.Net.Api.Response.Utxo;
using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Api.ClientDefinitions
{
  public interface IUtxoClient
  {
    Task<OutputResponse> FindOutputByIdAsync(string outputId);

    Task<Ed25519Address> GetAddressFromBech32Async(string addressBech32);

    Task<Ed25519Address> GetAddressFromEd25519Async(string addressEd25519);

    Task<OutputsResponse> GetOutputsFromBech32Async(string addressBech32, bool includeSpent, int type = 0);

    Task<OutputsResponse> GetOutputsFromEd25519Async(string addressEd25519, bool includeSpent, int type = 0);
  }
}
