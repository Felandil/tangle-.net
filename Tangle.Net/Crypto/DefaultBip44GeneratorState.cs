namespace Tangle.Net.Crypto
{
  public class DefaultBip44GeneratorState : IBip44GeneratorState
  {
    public int AccountIndex { get; set; }
    public bool IsInternal { get; set; }
    public int AddressIndex { get; set; }
  }
}