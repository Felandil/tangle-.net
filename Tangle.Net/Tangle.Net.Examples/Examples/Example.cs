namespace Tangle.Net.Examples.Examples
{
  using Tangle.Net.Repository;

  /// <summary>
  /// The example.
  /// </summary>
  public abstract class Example
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Example"/> class.
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
    public abstract void Execute();
  }
}