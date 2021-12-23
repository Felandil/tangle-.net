namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  public abstract class UnlockBlock : PayloadType, ISerializable
  {
    /// <inheritdoc />
    public abstract byte[] Serialize();
  }
}