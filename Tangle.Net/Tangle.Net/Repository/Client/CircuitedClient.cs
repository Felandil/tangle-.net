namespace Tangle.Net.Repository.Client
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using RestSharp;

  public class CircuitedClient : RestIotaClient
  {
    /// <inheritdoc />
    public CircuitedClient(IRestClient client)
      : base(client)
    {
    }

    public bool CircuitOpen =>
      this.TripDateTime != null && this.TripDateTime.Value.AddMilliseconds(this.ResetTimeoutMilliseconds).Ticks > DateTime.UtcNow.Ticks;

    public int FailedRequestCount { get; private set; }

    public int FailureThresholdPercentage { get; set; }

    public int RequestCount { get; private set; }

    public double ResetTimeoutMilliseconds { get; set; }

    private DateTime? TripDateTime { get; set; }

    /// <inheritdoc />
    public override void ExecuteParameterizedCommand(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
    {
      try
      {
        this.CheckCircuit();
        this.RequestCount++;
        base.ExecuteParameterizedCommand(parameters);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (Exception)
      {
        this.HandleException();
        throw;
      }
    }

    /// <inheritdoc />
    public override T ExecuteParameterizedCommand<T>(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
    {
      try
      {
        this.CheckCircuit();
        this.RequestCount++;
        return base.ExecuteParameterizedCommand<T>(parameters);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (Exception)
      {
        this.HandleException();
        throw;
      }
    }

    /// <inheritdoc />
    public override async Task ExecuteParameterizedCommandAsync(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
    {
      try
      {
        this.CheckCircuit();
        this.RequestCount++;
        await base.ExecuteParameterizedCommandAsync(parameters);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (CircuitOpenException)
      {
        throw;
      }
      catch (Exception)
      {
        this.HandleException();
        throw;
      }
    }

    /// <inheritdoc />
    public override async Task<T> ExecuteParameterizedCommandAsync<T>(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
    {
      try
      {
        this.CheckCircuit();
        this.RequestCount++;
        return await base.ExecuteParameterizedCommandAsync<T>(parameters);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (CircuitOpenException)
      {
        throw;
      }
      catch (Exception)
      {
        this.HandleException();
        throw;
      }
    }

    /// <inheritdoc />
    public override T ExecuteParameterlessCommand<T>(string commandName)
    {
      try
      {
        this.CheckCircuit();
        this.RequestCount++;
        return base.ExecuteParameterlessCommand<T>(commandName);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (CircuitOpenException)
      {
        throw;
      }
      catch (Exception)
      {
        this.HandleException();
        throw;
      }
    }

    /// <inheritdoc />
    public override async Task<T> ExecuteParameterlessCommandAsync<T>(string commandName)
    {
      try
      {
        this.CheckCircuit();
        this.RequestCount++;
        return await base.ExecuteParameterlessCommandAsync<T>(commandName);
      }
      catch (IotaApiException)
      {
        throw;
      }
      catch (CircuitOpenException)
      {
        throw;
      }
      catch (Exception)
      {
        this.HandleException();
        throw;
      }
    }

    private void CheckCircuit()
    {
      if (this.CircuitOpen)
      {
        throw new CircuitOpenException($"Circuit open. Base Uri: {this.Client.BaseUrl}");
      }
    }

    private void HandleException()
    {
      this.FailedRequestCount++;

      if ((double)this.FailedRequestCount / this.RequestCount * 100 >= this.FailureThresholdPercentage)
      {
        this.TripDateTime = DateTime.UtcNow;
      }
    }
  }
}