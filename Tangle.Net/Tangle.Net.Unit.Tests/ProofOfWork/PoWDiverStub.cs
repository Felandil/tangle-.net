using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;

  public class PoWDiverStub : IPoWDiver
  {
    public TransactionTrytes DoPow(TransactionTrytes trytes, int minWeightMagnitude)
    {
      return trytes;
    }
  }
}
