namespace Tangle.Net.Zmq.Events
{
  using Tangle.Net.Entity;

  public class LatestSolidSubtangleMilestoneTransactionHashEventArgs : ZmqEventArgs
  {
    private Hash milestoneTransactionHash;

    /// <inheritdoc />
    public LatestSolidSubtangleMilestoneTransactionHashEventArgs(string message)
      : base(message)
    {
    }

    public Hash MilestoneTransactionHash
    {
      get
      {
        if (this.milestoneTransactionHash == null)
        {
          this.milestoneTransactionHash = new Hash(this.Message.Split(' ')[1]);
        }

        return this.milestoneTransactionHash;
      }
    }
  }
}