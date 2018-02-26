namespace Tangle.Net.Repository.Client
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  /// <summary>
  /// The IotaClient interface.
  /// </summary>
  public interface IIotaClient
  {
    /// <summary>
    /// The execute parameterized command.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <typeparam name="T">
    /// The Response type.
    /// </typeparam>
    /// <returns>
    /// The <see cref="T"/>.
    /// </returns>
    T ExecuteParameterizedCommand<T>(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
      where T : new();

    /// <summary>
    /// The execute parameterized command.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    void ExecuteParameterizedCommand(IReadOnlyCollection<KeyValuePair<string, object>> parameters);

    /// <summary>
    /// The execute parameterized command async.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <typeparam name="T">
    /// The Response type.
    /// </typeparam>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<T> ExecuteParameterizedCommandAsync<T>(IReadOnlyCollection<KeyValuePair<string, object>> parameters)
      where T : new();

    /// <summary>
    /// The execute parameterized command async.
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task ExecuteParameterizedCommandAsync(IReadOnlyCollection<KeyValuePair<string, object>> parameters);

    /// <summary>
    /// The execute parameterless command.
    /// </summary>
    /// <param name="commandName">
    /// The command name.
    /// </param>
    /// <typeparam name="T">
    /// The Response type.
    /// </typeparam>
    /// <returns>
    /// The <see cref="T"/>.
    /// </returns>
    T ExecuteParameterlessCommand<T>(string commandName)
      where T : new();

    /// <summary>
    /// The execute parameterless command async.
    /// </summary>
    /// <param name="commandName">
    /// The command name.
    /// </param>
    /// <typeparam name="T">
    /// The Response type.
    /// </typeparam>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<T> ExecuteParameterlessCommandAsync<T>(string commandName)
      where T : new();
  }
}