namespace Tangle.Net.Crypto.Bip44
{
  public interface IBip44GeneratorState
  {
    int AccountIndex { get; set; }

    bool IsInternal { get; set; }

    int AddressIndex { get; set; }
  }
}