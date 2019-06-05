namespace Tangle.Net.Zmq.Events
{
  public class IriCacheInformationEventArgs : ZmqEventArgs
  {
    private string hitCount;

    private string missCount;

    /// <inheritdoc />
    public IriCacheInformationEventArgs(string message)
      : base(message)
    {
    }

    public string HitCount
    {
      get
      {
        if (string.IsNullOrEmpty(this.hitCount))
        {
          this.hitCount = this.Message.Split(' ')[1];
        }

        return this.hitCount;
      }
    }

    public string MissCount
    {
      get
      {
        if (string.IsNullOrEmpty(this.missCount))
        {
          this.missCount = this.Message.Split(' ')[2];
        }

        return this.missCount;
      }
    }
  }
}