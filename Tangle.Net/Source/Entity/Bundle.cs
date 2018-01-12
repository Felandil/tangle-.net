using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangle.Net.Source.Entity
{
  public class Bundle
  {
    public void AddEntry(int signatureMessageLength, string address, long value, string tag, long timestamp)
    {
      throw new NotImplementedException();
    }

    public void Finalize()
    {
      throw new NotImplementedException();
    }

    public void AddTrytes(List<string> signatureFragments)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Transaction> GetTransactions()
    {
      throw new NotImplementedException();
    }
  }
}
