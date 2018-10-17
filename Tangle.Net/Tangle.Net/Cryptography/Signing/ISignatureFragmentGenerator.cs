namespace Tangle.Net.Cryptography.Signing
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;

  public interface ISignatureFragmentGenerator
  {
    List<Fragment> Generate(AbstractPrivateKey privateKey, Hash hash);

    Task<List<Fragment>> GenerateAsync(AbstractPrivateKey privateKey, Hash hash);
  }
}