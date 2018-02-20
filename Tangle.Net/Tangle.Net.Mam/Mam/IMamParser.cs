using Tangle.Net.Entity;

namespace Tangle.Net.Mam.Mam
{
  public interface IMamParser
  {
    UnmaskedAuthenticatedMessage Unmask(Bundle payload, TryteString channelKey);
  }
}
