namespace Tangle.Net.ProofOfWork
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The po w service.
  /// </summary>
  public class PoWService : IPoWService
  {
    #region Constants

    /// <summary>
    /// The max timestamp value.
    /// </summary>
    public const long MaxTimestampValue = 3812798742493;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PoWService"/> class.
    /// </summary>
    /// <param name="powDiver">
    /// The pow diver.
    /// </param>
    public PoWService(IPoWDiver powDiver)
    {
      this.PowDiver = powDiver;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the pow diver.
    /// </summary>
    private IPoWDiver PowDiver { get; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The do po w.
    /// </summary>
    /// <param name="branchTransaction">
    /// The branch Transaction.
    /// </param>
    /// <param name="trunkTransaction">
    /// The trunk Transaction.
    /// </param>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min Weight Magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<Transaction> DoPoW(Hash branchTransaction, Hash trunkTransaction, List<Transaction> transactions, int minWeightMagnitude = 18)
    {
      var resultTransactions = new List<Transaction>();
      var lastTransactionHash = new Hash();

      transactions.Reverse();
      for (var i = 0; i < transactions.Count(); i++)
      {
        transactions[i].AttachmentTimestamp = Timestamp.UnixSecondsTimestamp * 1000;
        transactions[i].AttachmentTimestampLowerBound = 0;
        transactions[i].AttachmentTimestampUpperBound = MaxTimestampValue;

        if (transactions[i].CurrentIndex == transactions[i].LastIndex)
        {
          transactions[i].BranchTransaction = branchTransaction;
          transactions[i].TrunkTransaction = trunkTransaction;
        }
        else
        {
          transactions[i].BranchTransaction = trunkTransaction;
          transactions[i].TrunkTransaction = lastTransactionHash;
        }

        if (transactions[i].Tag.Value == Tag.Empty.Value)
        {
          transactions[i].Tag = transactions[i].ObsoleteTag;
        }

        var resultTransactionTrytes = this.PowDiver.DoPow(transactions[i].ToTrytes(), minWeightMagnitude);
        var resultTransaction = Transaction.FromTrytes(resultTransactionTrytes);
        lastTransactionHash = resultTransaction.Hash;

        resultTransactions.Add(resultTransaction);
      }

      resultTransactions.Reverse();

      return resultTransactions;
    }

    /// <inheritdoc />
    public async Task<List<Transaction>> DoPoWAsync(Hash branchTransaction, Hash trunkTransaction, List<Transaction> transactions, int minWeightMagnitude = 18)
    {
      return await Task.Factory.StartNew(() => this.DoPoW(branchTransaction, trunkTransaction, transactions, minWeightMagnitude));
    }

    #endregion
  }
}