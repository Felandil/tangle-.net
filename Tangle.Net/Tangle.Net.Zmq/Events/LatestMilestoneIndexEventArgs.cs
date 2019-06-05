namespace Tangle.Net.Zmq.Events
{
  public class LatestMilestoneIndexEventArgs : ZmqEventArgs
  {
    private int latestMilestoneIndex;

    /// <inheritdoc />
    public LatestMilestoneIndexEventArgs(string message)
      : base(message)
    {
    }

    public int LatestMilestoneIndex
    {
      get
      {
        if (this.latestMilestoneIndex == 0)
        {
          this.latestMilestoneIndex = int.Parse(this.Message.Split(' ')[1]);
        }

        return this.latestMilestoneIndex;
      }
    }
  }
}