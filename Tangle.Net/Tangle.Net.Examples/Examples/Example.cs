namespace Tangle.Net.Examples.Examples
{
  using Tangle.Net.Repository;

  /// <summary>
  /// The example.
  /// </summary>
  /// <typeparam name="T">
  /// The result type.
  /// </typeparam>
  public abstract class Example<T>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Example{T}"/> class. 
    /// </summary>
    /// <param name="repository">
    /// The repository.
    /// </param>
    public Example(IIotaRepository repository)
    {
      this.Repository = repository;
    }

    /// <summary>
    /// Gets the repository.
    /// </summary>
    protected IIotaRepository Repository { get; }

    /// <summary>
    /// The execute.
    /// </summary>
    /// <returns>
    /// The result type <see cref="T"/>.
    /// </returns>
    public abstract T Execute();
  }
}