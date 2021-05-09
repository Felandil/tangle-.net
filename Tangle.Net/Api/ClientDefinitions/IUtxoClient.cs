using System.Threading.Tasks;
using Tangle.Net.Api.Responses.Utxo;
using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Api.ClientDefinitions
{
  public interface IUtxoClient
  {
    Task<OutputResponse> FindOutputByIdAsync(string outputId);

    Task<Ed25519Address> GetAddressFromBech32Async(string addressBech32);

    Task<Ed25519Address> GetAddressFromEd25519Async(string addressEd25519);
  }
}
