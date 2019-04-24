namespace Tangle.Net.Repository.Client
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net;
  using System.Threading.Tasks;

  using RestSharp;

  using Tangle.Net.Utils;

  /// <summary>
  /// The rest iota client.
  /// </summary>
  public class RestIotaClient : IIotaClient
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RestIotaClient"/> class.
    /// </summary>
    /// <param name="client">
    /// The client.
    /// </param>
    public RestIotaClient(IRestClient client)
    {
      this.Client = client;
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    protected IRestClient Client { get; }

    /// <inheritdoc />
    public virtual T ExecuteParameterizedCommand<T>(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
      where T : new()
    {
      var response = this.Client.Execute<T>(CreateRequest(parameters));
      return ValidateResponse(response, CommandNameFromParameters(parameters));
    }

    /// <inheritdoc />
    public virtual void ExecuteParameterizedCommand(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
    {
      var response = this.Client.Execute<object>(CreateRequest(parameters));
      ValidateResponse(response, CommandNameFromParameters(parameters));
    }

    /// <inheritdoc />
    public virtual async Task<T> ExecuteParameterizedCommandAsync<T>(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
      where T : new()
    {
      var response = await this.Client.ExecuteTaskAsync<T>(CreateRequest(parameters));
      return ValidateResponse(response, CommandNameFromParameters(parameters));
    }

    /// <inheritdoc />
    public virtual async Task ExecuteParameterizedCommandAsync(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
    {
      var response = await this.Client.ExecuteTaskAsync<object>(CreateRequest(parameters));
      ValidateResponse(response, CommandNameFromParameters(parameters));
    }

    /// <inheritdoc />
    public virtual T ExecuteParameterlessCommand<T>(string commandName)
      where T : new()
    {
      return this.ExecuteParameterizedCommand<T>(new Dictionary<string, object> { { "command", commandName } });
    }

    /// <inheritdoc />
    public virtual async Task<T> ExecuteParameterlessCommandAsync<T>(string commandName)
      where T : new()
    {
      return await this.ExecuteParameterizedCommandAsync<T>(new Dictionary<string, object> { { "command", commandName } });
    }

    /// <summary>
    /// The command name from parameters.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <returns>
    /// The <see cref="object"/>.
    /// </returns>
    private static string CommandNameFromParameters(IEnumerable<KeyValuePair<string, object>> parameters)
    {
      return parameters.First(p => p.Key == "command").Value.ToString();
    }

    /// <summary>
    /// The create request.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <returns>
    /// The <see cref="RestRequest"/>.
    /// </returns>
    private static RestRequest CreateRequest(IEnumerable<KeyValuePair<string, object>> parameters)
    {
      var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
      request.AddHeader("X-IOTA-API-Version", "1");
      request.AddJsonBody(parameters);

      return request;
    }

    /// <summary>
    /// The validate response.
    /// </summary>
    /// <param name="response">
    /// The response.
    /// </param>
    /// <param name="commandName">
    /// The command name.
    /// </param>
    /// <typeparam name="T">
    /// The response type.
    /// </typeparam>
    /// <returns>
    /// The <see cref="T"/>.
    /// </returns>
    private static T ValidateResponse<T>(IRestResponse<T> response, string commandName)
      where T : new()
    {
      var nullResponse = response == null;

      if (!nullResponse && response.StatusCode == HttpStatusCode.OK)
      {
        return response.Data;
      }

      if (nullResponse)
      {
        throw new Exception($"Command {commandName} failed!");
      }

      if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
      {
        throw new IotaApiException(response.ToNodeError().Error);
      }

      if (response.ErrorException != null)
      {
        throw response.ErrorException;
      }

      throw new Exception($"Command {commandName} failed! Status Code: {(int)response.StatusCode}");
    }
  }
}