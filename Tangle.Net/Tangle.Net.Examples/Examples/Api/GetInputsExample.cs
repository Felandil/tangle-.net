namespace Tangle.Net.Examples.Examples.Api
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Responses;

  /// <inheritdoc />
  public class GetInputsExample : Example<GetInputsResponse>
  {
    /// <inheritdoc />
    public GetInputsExample(IIotaRepository repository)
      : base(repository)
    {
    }

    /// <inheritdoc />
    /// GetInputs can be used to get the balance of a given seed. Use GetInputsAsync in async context.
    /// 
    /// 1. Get the seed
    /// 2. Call GetInputs. You can specify a threshold for the balance. In addition you can control the start and stop index of the seeds addresses.
    public override GetInputsResponse Execute()
    {
      var seed = new Seed("SOMENICEANDSECURESEEDGOESHERE");
      return this.Repository.GetInputs(seed, 1000000, SecurityLevel.Medium, 0);
    }
  }
}