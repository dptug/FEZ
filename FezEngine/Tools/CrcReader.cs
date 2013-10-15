// Type: FezEngine.Tools.CrcReader
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
  public class CrcReader
  {
    private readonly List<byte> readBytes = new List<byte>();
    private readonly BinaryReader reader;

    public CrcReader(BinaryReader reader)
    {
      this.reader = reader;
    }

    public bool ReadBoolean()
    {
      bool flag = this.reader.ReadBoolean();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(flag));
      return flag;
    }

    public byte ReadByte()
    {
      byte num = this.reader.ReadByte();
      this.readBytes.Add(num);
      return num;
    }

    public byte[] ReadBytes(int count)
    {
      byte[] numArray = this.reader.ReadBytes(count);
      this.readBytes.AddRange((IEnumerable<byte>) numArray);
      return numArray;
    }

    public char ReadChar()
    {
      char ch = this.reader.ReadChar();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(ch));
      return ch;
    }

    public double ReadDouble()
    {
      double num = this.reader.ReadDouble();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num));
      return num;
    }

    public short ReadInt16()
    {
      short num = this.reader.ReadInt16();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num));
      return num;
    }

    public int ReadInt32()
    {
      int num = this.reader.ReadInt32();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num));
      return num;
    }

    public long ReadInt64()
    {
      long num = this.reader.ReadInt64();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num));
      return num;
    }

    public sbyte ReadSByte()
    {
      sbyte num = this.reader.ReadSByte();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes((short) num));
      return num;
    }

    public float ReadSingle()
    {
      float num = this.reader.ReadSingle();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num));
      return num;
    }

    public string ReadString()
    {
      string s = this.reader.ReadString();
      this.readBytes.AddRange((IEnumerable<byte>) Encoding.Unicode.GetBytes(s));
      return s;
    }

    public ushort ReadUInt16()
    {
      ushort num = this.reader.ReadUInt16();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num));
      return num;
    }

    public uint ReadUInt32()
    {
      uint num = this.reader.ReadUInt32();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num));
      return num;
    }

    public ulong ReadUInt64()
    {
      ulong num = this.reader.ReadUInt64();
      this.readBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num));
      return num;
    }

    public bool CheckHash()
    {
      return (int) this.ReadUInt32() == (int) Crc32.ComputeChecksum(this.readBytes.ToArray());
    }
  }
}
