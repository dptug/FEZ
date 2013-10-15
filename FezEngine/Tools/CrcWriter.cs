// Type: FezEngine.Tools.CrcWriter
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FezEngine.Tools
{
  public class CrcWriter
  {
    private readonly List<byte> writtenBytes = new List<byte>();
    private readonly BinaryWriter writer;

    public CrcWriter(BinaryWriter writer)
    {
      this.writer = writer;
    }

    public void Write(bool value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(value));
      this.writer.Write(value);
    }

    public void Write(byte value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes((short) value));
      this.writer.Write(value);
    }

    public void Write(byte[] buffer)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) buffer);
      this.writer.Write(buffer);
    }

    public void Write(double value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(value));
      this.writer.Write(value);
    }

    public void Write(float value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(value));
      this.writer.Write(value);
    }

    public void Write(int value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(value));
      this.writer.Write(value);
    }

    public void Write(long value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(value));
      this.writer.Write(value);
    }

    public void Write(sbyte value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes((short) value));
      this.writer.Write(value);
    }

    public void Write(short value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(value));
      this.writer.Write(value);
    }

    public void Write(string value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) Encoding.Unicode.GetBytes(value));
      this.writer.Write(value);
    }

    public void Write(uint value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(value));
      this.writer.Write(value);
    }

    public void Write(ulong value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(value));
      this.writer.Write(value);
    }

    public void Write(ushort value)
    {
      this.writtenBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(value));
      this.writer.Write(value);
    }

    public void WriteHash()
    {
      this.Write(Crc32.ComputeChecksum(this.writtenBytes.ToArray()));
    }
  }
}
