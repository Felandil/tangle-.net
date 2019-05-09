namespace Tangle.Net.Zmq.Events
{
  using System;

  using Tangle.Net.Entity;

  public class TransactionConfirmedEventArgs : EventArgs
  {
    public TransactionConfirmedEventArgs(Address address, Hash branch, Hash bundleHash, Hash hash, int milestone, Hash trunk)
    {
      this.Address = address;
      this.Branch = branch;
      this.BundleHash = bundleHash;
      this.Hash = hash;
      this.Milestone = milestone;
      this.Trunk = trunk;
    }

    public Address Address { get; }

    public Hash Branch { get; }

    public Hash BundleHash { get; }

    public Hash Hash { get; }

    public int Milestone { get; }

    public Hash Trunk { get; }

    public static TransactionConfirmedEventArgs FromZmqMessage(string message)
    {
      var splitMessage = message.Split(' ');

      return new TransactionConfirmedEventArgs(
        new Address(splitMessage[3]),
        new Hash(splitMessage[5]),
        new Hash(splitMessage[6]),
        new Hash(splitMessage[2]),
        int.Parse(splitMessage[1]),
        new Hash(splitMessage[4]));
    }
  }
}