using System;
using System.Collections.Generic;

namespace Tangle.Net.Crypto
{
  public class Bip32Buffer
  {
    private readonly List<byte> buffer = new List<byte>();

    public void WriteUInt(uint i)
    {
      buffer.Add((byte) ((i >> 0x18) & 0xff));
      buffer.Add((byte) ((i >> 0x10) & 0xff));
      buffer.Add((byte) ((i >> 8) & 0xff));
      buffer.Add((byte) (i & 0xff));
    }

    public void Write(byte @byte)
    {
      buffer.Add(@byte);
    }

    public void Write(byte[] bytes)
    {
      Write(bytes, 0, bytes.Length);
    }

    public void Write(byte[] bytes, int offset, int count)
    {
      var byteCopy = new byte[count];
      Array.Copy(bytes, offset, byteCopy, 0, count);

      this.buffer.AddRange(byteCopy);
    }

    public byte[] ToArray()
    {
      return buffer.ToArray();
    }
  }
}