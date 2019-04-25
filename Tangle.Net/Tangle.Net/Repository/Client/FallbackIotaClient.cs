namespace Tangle.Net.Repository.Client
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;
  using System.Threading.Tasks;

  using RestSharp;

  [ExcludeFromCodeCoverage]
  public class FallbackIotaClient : IIotaClient
  {
    public FallbackIotaClient(ICollection<string> clients, int timeout, int failureThresholdPercentage = 100, int resetTimeoutMilliseconds = 30000)
    {
      this.FailureThresholdPercentage = failureThresholdPercentage;
      this.ResetTimeoutMilliseconds = resetTimeoutMilliseconds;

      if (clients.Count == 0)
      {
        throw new Exception("No nodes provided!");
      }

      this.Clients = this.CreateClients(clients, timeout);
    }

    private List<CircuitedClient> Clients { get; }

    private int FailureThresholdPercentage { get; }

    private int NodePointer { get; set; }

    private int ResetTimeoutMilliseconds { get; }

    /// <inheritdoc />
    public T ExecuteParameterizedCommand<T>(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
      where T : new()
    {
      try
      {
        return this.Clients[this.NodePointer].ExecuteParameterizedCommand<T>(parameters);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (Exception exception)
      {
        this.PickHealthyClient(exception);
        return this.ExecuteParameterizedCommand<T>(parameters);
      }
    }

    /// <inheritdoc />
    public void ExecuteParameterizedCommand(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
    {
      try
      {
        this.Clients[this.NodePointer].ExecuteParameterizedCommand(parameters);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (Exception exception)
      {
        this.PickHealthyClient(exception);
        this.ExecuteParameterizedCommand(parameters);
      }
    }

    /// <inheritdoc />
    public async Task<T> ExecuteParameterizedCommandAsync<T>(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
      where T : new()
    {
      try
      {
        return await this.Clients[this.NodePointer].ExecuteParameterizedCommandAsync<T>(parameters);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (Exception exception)
      {
        this.PickHealthyClient(exception);
        return await this.ExecuteParameterizedCommandAsync<T>(parameters);
      }
    }

    /// <inheritdoc />
    public async Task ExecuteParameterizedCommandAsync(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
    {
      try
      {
        await this.Clients[this.NodePointer].ExecuteParameterizedCommandAsync(parameters);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (Exception exception)
      {
        this.PickHealthyClient(exception);
        await this.ExecuteParameterizedCommandAsync(parameters);
      }
    }

    /// <inheritdoc />
    public T ExecuteParameterlessCommand<T>(string commandName)
      where T : new()
    {
      try
      {
        return this.Clients[this.NodePointer].ExecuteParameterlessCommand<T>(commandName);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (Exception exception)
      {
        this.PickHealthyClient(exception);
        return this.ExecuteParameterlessCommand<T>(commandName);
      }
    }

    /// <inheritdoc />
    public async Task<T> ExecuteParameterlessCommandAsync<T>(string commandName)
      where T : new()
    {
      try
      {
        return await this.Clients[this.NodePointer].ExecuteParameterlessCommandAsync<T>(commandName);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (Exception exception)
      {
        this.PickHealthyClient(exception);
        return await this.ExecuteParameterlessCommandAsync<T>(commandName);
      }
    }

    private List<CircuitedClient> CreateClients(IEnumerable<string> uris, int timeout)
    {
      return uris.Select(
        uri => new CircuitedClient(new RestClient(uri) { Timeout = timeout })
                 {
                   FailureThresholdPercentage = this.FailureThresholdPercentage, ResetTimeoutMilliseconds = this.ResetTimeoutMilliseconds
                 }).ToList();
    }

    private void PickHealthyClient(Exception exception)
    {
      this.NodePointer = 0;

      while (true)
      {
        if (!this.Clients[this.NodePointer].CircuitOpen)
        {
          return;
        }

        this.NodePointer++;
        if (this.NodePointer >= this.Clients.Count)
        {
          throw new Exception("No healthy client available. See inner exception for API errors", exception);
        }
      }
    }
  }
}