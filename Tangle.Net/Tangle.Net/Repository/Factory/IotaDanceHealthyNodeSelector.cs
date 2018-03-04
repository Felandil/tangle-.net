namespace Tangle.Net.Repository.Factory
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using RestSharp;

  /// <inheritdoc />
  public class IotaDanceHealthyNodeSelector : IHealthyNodeSelector
  {
    /// <inheritdoc />
    public async Task<string> GetHealthyNodeUriAsync()
    {
      var healthiestNode = (await LoadNodes()).First();

      return healthiestNode.First(n => n.Key == "node").Value + ":" + healthiestNode.First(n => n.Key == "port").Value;
    }

    /// <inheritdoc />
    public async Task<string> GetHealthyRemotePoWNodeUriAsync()
    {
      var nodes = await LoadNodes();
      var healthiestPoWNode = new Dictionary<string, object>();

      foreach (var node in nodes)
      {
        if (!(bool)node.First(n => n.Key == "pow").Value)
        {
          continue;
        }

        healthiestPoWNode = node;
        break;
      }

      return healthiestPoWNode.First(n => n.Key == "node").Value + ":" + healthiestPoWNode.First(n => n.Key == "port").Value;
    }

    /// <summary>
    /// The load nodes.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    private static async Task<List<Dictionary<string, object>>> LoadNodes()
    {
      var client = new RestClient("https://iota.dance");

      var response = await client.ExecuteTaskAsync<List<Dictionary<string, object>>>(new RestRequest("/data/node-stats"));
      return response.Data;
    }
  }
}