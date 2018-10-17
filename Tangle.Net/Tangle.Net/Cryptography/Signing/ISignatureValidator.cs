namespace Tangle.Net.Cryptography.Signing
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;

  public interface ISignatureValidator
  {
    bool ValidateFragments(List<Fragment> fragments, Hash hash, TryteString publicKey);

    Task<bool> ValidateFragmentsAsync(List<Fragment> fragments, Hash hash, TryteString publicKey);
  }
}