namespace Tangle.Net.Crypto.Bip44
{
  public class DefaultBip44GeneratorState : IBip44GeneratorState
  {
    public int AccountIndex { get; set; }
    public bool IsInternal { get; set; }
    public int AddressIndex { get; set; }
  }
}