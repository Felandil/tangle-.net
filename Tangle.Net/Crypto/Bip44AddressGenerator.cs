namespace Tangle.Net.Crypto
{
  public class Bip44AddressGenerator
  {
    public const string IotaBip44BasePath = "m/44'/4218'";

    public static Bip32Path GenerateAddress(int accountIndex, int addressIndex, bool isInternal)
    {
      var path = new Bip32Path(IotaBip44BasePath);

      path.PushHardened(accountIndex);
      path.PushHardened(isInternal ? 1 : 0);
      path.PushHardened(addressIndex);

      return path;
    }

    public static Bip32Path GenerateAddress(IBip44GeneratorState generatorState, bool isFirst)
    {
      if (!isFirst)
      {
        if (!generatorState.IsInternal)
        {
          generatorState.IsInternal = true;
        }
        else
        {
          generatorState.IsInternal = false;
          generatorState.AddressIndex++;
        }
      }

      var path = new Bip32Path(IotaBip44BasePath);

      path.PushHardened(generatorState.AccountIndex);
      path.PushHardened(generatorState.IsInternal ? 1 : 0);
      path.PushHardened(generatorState.AddressIndex);

      return path;
    }
  }
}