﻿using Tangle.Net.Entity.Message.Payload.Receipt;

namespace Tangle.Net.Entity.Message.Payload
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public abstract class Payload : PayloadType, ISerializable
  {
    public const int TransactionPayloadType = 0;
    public const int MilestonePayloadType = 1;
    public const int IndexationPayloadType = 2;
    public const int ReceiptPayloadType = 3;
    public const int TreasuryTransactionPayloadType = 4;

    protected abstract byte[] SerializeImplementation();

    public byte[] Serialize()
    {
      var result = new List<byte>();

      var serializedImplementation = this.SerializeImplementation();
      result.AddRange(BitConverter.GetBytes(serializedImplementation.Length));
      result.AddRange(serializedImplementation);

      return result.ToArray();
    }

    public static T Deserialize<T>(byte[] payload)
      where T : Payload
    {
      var payloadType = BitConverter.ToInt32(payload.Take(4).ToArray(), 0);
      switch (payloadType)
      {
        case IndexationPayloadType:
          return IndexationPayload.Deserialize(payload) as T;
        case MilestonePayloadType:
          return MilestonePayload.Deserialize(payload) as T;
        case ReceiptPayloadType:
          return ReceiptPayload.Deserialize(payload) as T;
        case TreasuryTransactionPayloadType:
          return TreasuryTransactionPayload.Deserialize(payload) as T;
      }

      return default(T);
    }
  }
}