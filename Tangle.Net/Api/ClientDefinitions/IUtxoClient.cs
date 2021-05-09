using System.Threading.Tasks;
using Tangle.Net.Api.Responses.Utxo;

namespace Tangle.Net.Api.ClientDefinitions
{
  public interface IUtxoClient
  {
    Task<OutputResponse> FindOutputByIdAsync(string outputId);
  }
}
