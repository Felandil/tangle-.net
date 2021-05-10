namespace Tangle.Net.Crypto
{
  public interface IBip44GeneratorState
  {
    int AccountIndex { get; set; }

    bool IsInternal { get; set; }

    int AddressIndex { get; set; }
  }
}