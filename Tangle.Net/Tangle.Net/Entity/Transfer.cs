namespace Tangle.Net.Entity
{
  using System;

  /// <summary>
  /// The transfer.
  /// </summary>
  public class Transfer
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Transfer"/> class.
    /// </summary>
    public Transfer()
    {
      this.Tag = new Tag();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Transfer"/> class.
    /// </summary>
    /// <param name="address">Address to send to as string</param>
    /// <param name="tag">Tag to use as string</param>
    /// <param name="message">Message to send. Will be handled as ASCII string</param>
    /// <param name="timestamp">Transfer timestamp</param>
    /// <param name="value">Value to attach to transfer</param>
    public Transfer(string address, string tag, string message, DateTime timestamp, long value = 0)
    {
      this.Address = new Address(address);
      this.Tag = new Tag(tag);
      this.Message = TryteString.FromAsciiString(message);
      this.Timestamp = Utils.Timestamp.Convert(timestamp);
      this.ValueToTransfer = value;
    }

    public Transfer(Address address, Tag tag, TryteString message, DateTime timestamp, long value = 0)
    {
      this.Address = address;
      this.Tag = tag;
      this.Message = message;
      this.Timestamp = Utils.Timestamp.Convert(timestamp);
      this.ValueToTransfer = value;
    }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public Address Address { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public TryteString Message { get; set; }

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    public Tag Tag { get; set; }

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the value to transfer.
    /// </summary>
    public long ValueToTransfer { get; set; }
  }
}