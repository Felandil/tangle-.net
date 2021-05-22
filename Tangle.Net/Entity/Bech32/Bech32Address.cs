using System.Collections.Generic;
using System.Linq;
using Tangle.Net.Crypto;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Utils;

namespace Tangle.Net.Entity.Bech32
{
  public class Bech32Address
  {
    public Bech32Address(string address)
    {
      this.Address = address;
    }

    public Bech32Address()
    {
    }

    public Ed25519Address ToEd25519Address()
    {
      Crypto.Bech32.Decode(this.Address, out _, out var data);

      return new Ed25519Address {Address = data.Skip(1).Take(32).ToHex(), Balance = this.Balance};
    }

    public static Bech32Address FromEd25519Address(Ed25519Address address, Bip32Path path, string humanReadablePart)
    {
      var addressBytes = new List<byte>() {(byte) address.Type};
      addressBytes.AddRange(address.Address.HexToBytes());

      return new Bech32Address
      {
        Balance = address.Balance,
        Path = path,
        Address = Crypto.Bech32.Encode(humanReadablePart, addressBytes.ToArray())
      };
    }

    public string Address { get; set; }
    public Bip32Path Path { get; set; }
    public long Balance { get; set; }
  }
}