using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Entity.Message.Payload;
using Tangle.Net.Entity.Message.Payload.Transaction;
using Tangle.Net.Utils;

namespace Tangle.Net.Tests.Entity.Message.Payload.Transaction
{
  [TestClass]
  public class TransactionEssenceTest
  {
    [TestMethod]
    public void TestTransactionHashGeneration()
    {
      var expected = "41e3c1329ffb610aa30587e9fd6ca3ea3367a684ae20ee5d1cfacc3b6226584c";

      var essence = new TransactionEssence
        {
          Inputs = new List<UTXOInput>
          {
            new UTXOInput
            {
              TransactionId =
                "c08141e6f09d8da0c7d5604e3e3bab20731948c120af42d58826817eaec9d186",
              TransactionOutputIndex = 0
            }
          },
          Outputs = new List<SigLockedSingleOutput>
          {
            new SigLockedSingleOutput
            {
              Amount = 1000,
              Address = new Ed25519Address
              {
                Address =
                  "3f8530482fd6d0e3a198c327f5ce0423f2eb9e22be8c3ab0554d081467b20a5b"
              }
            }
          },
          Payload = new IndexationPayload
          {
            Index = "746573740a",
            Data = "746573740a"
          }
        };

      var actual = essence.CalculateHash().ToHex();

      Assert.AreEqual(expected, actual);
    }
  }
}
