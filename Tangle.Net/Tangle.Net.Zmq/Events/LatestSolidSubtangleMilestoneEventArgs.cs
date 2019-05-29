namespace Tangle.Net.Zmq.Events
{
  public class LatestSolidSubtangleMilestoneEventArgs : ZmqEventArgs
  {
    private int latestSolidSubtangleMilestoneIndex;

    private int previousSolidSubtangleMilestoneIndex;

    /// <inheritdoc />
    public LatestSolidSubtangleMilestoneEventArgs(string message)
      : base(message)
    {
    }

    public int LatestSolidSubtangleMilestoneIndex
    {
      get
      {
        if (this.latestSolidSubtangleMilestoneIndex == 0)
        {
          this.latestSolidSubtangleMilestoneIndex = int.Parse(this.Message.Split(' ')[2]);
        }

        return this.latestSolidSubtangleMilestoneIndex;
      }
    }

    public int PreviousSolidSubtangleMilestoneIndex
    {
      get
      {
        if (this.previousSolidSubtangleMilestoneIndex == 0)
        {
          this.previousSolidSubtangleMilestoneIndex = int.Parse(this.Message.Split(' ')[1]);
        }

        return this.previousSolidSubtangleMilestoneIndex;
      }
    }
  }
}