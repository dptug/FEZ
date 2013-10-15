// Type: NVorbis.DataPacket
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NVorbis
{
  internal abstract class DataPacket
  {
    private ulong _bitBucket;
    private int _bitCount;
    private long _readBits;

    protected abstract bool CanReset { get; }

    public bool IsResync { get; internal set; }

    public long GranulePosition { get; set; }

    public long PageGranulePosition { get; internal set; }

    public int Length { get; protected set; }

    public bool IsEndOfStream { get; internal set; }

    public long BitsRead
    {
      get
      {
        return this._readBits;
      }
    }

    public long? GranuleCount { get; set; }

    internal bool IsContinued { get; set; }

    internal bool IsContinuation { get; set; }

    internal int PageSequenceNumber { get; set; }

    protected DataPacket(int length)
    {
      this.Length = length;
    }

    internal void MergeWith(DataPacket continuation)
    {
      this.DoMergeWith(continuation);
    }

    protected abstract void DoMergeWith(DataPacket continuation);

    protected abstract void DoReset();

    protected abstract int ReadNextByte();

    public void Reset()
    {
      if (!this.CanReset)
        throw new NotSupportedException();
      this.DoReset();
      this._bitBucket = 0UL;
      this._bitCount = 0;
      this._readBits = 0L;
    }

    public virtual void Done()
    {
    }

    public ulong TryPeekBits(int count, out int bitsRead)
    {
      int count1 = 0;
      if (count <= 0 || count > 64)
        throw new ArgumentOutOfRangeException("count");
      if (count + this._bitCount > 64)
      {
        count1 = 8;
        count -= 8;
      }
      while (this._bitCount < count)
      {
        int num = this.ReadNextByte();
        if (num == -1)
        {
          count = this._bitCount;
          count1 = 0;
          break;
        }
        else
        {
          this._bitBucket = (ulong) (num & (int) byte.MaxValue) << this._bitCount | this._bitBucket;
          this._bitCount += 8;
        }
      }
      ulong num1 = this._bitBucket;
      if (count < 64)
        num1 &= (ulong) ((1L << count) - 1L);
      if (count1 > 0)
        num1 |= this.PeekBits(count1) << count - count1;
      bitsRead = count;
      return num1;
    }

    public ulong PeekBits(int count)
    {
      int bitsRead;
      ulong num = this.TryPeekBits(count, out bitsRead);
      if (bitsRead < count)
        throw new EndOfStreamException();
      else
        return num;
    }

    public void SkipBits(int count)
    {
      if (this._bitCount > count)
      {
        this._bitBucket >>= count;
        this._bitCount -= count;
        this._readBits += (long) count;
      }
      else if (this._bitCount == count)
      {
        this._bitBucket = 0UL;
        this._bitCount = 0;
        this._readBits += (long) count;
      }
      else
      {
        count -= this._bitCount;
        this._readBits += (long) this._bitCount;
        this._bitCount = 0;
        while (count > 8)
        {
          if (this.ReadNextByte() == -1)
            throw new EndOfStreamException();
          count -= 8;
          this._readBits += 8L;
        }
        if (count <= 0)
          return;
        long num = (long) this.PeekBits(count);
        this._bitBucket >>= count;
        this._bitCount -= count;
        this._readBits += (long) count;
      }
    }

    public ulong ReadBits(int count)
    {
      if (count < 0 || count > 64)
        throw new ArgumentOutOfRangeException("count");
      ulong num = this.PeekBits(count);
      this.SkipBits(count);
      return num;
    }

    public byte PeekByte()
    {
      return (byte) this.PeekBits(8);
    }

    public byte ReadByte()
    {
      return (byte) this.ReadBits(8);
    }

    public byte[] ReadBytes(int count)
    {
      List<byte> list = new List<byte>(count);
      while (list.Count < count)
      {
        try
        {
          list.Add(this.ReadByte());
        }
        catch (EndOfStreamException ex)
        {
          break;
        }
      }
      return list.ToArray();
    }

    public int Read(byte[] buffer, int index, int count)
    {
      if (index < 0 || index + count > buffer.Length)
        throw new ArgumentOutOfRangeException("index");
      for (int index1 = 0; index1 < count; ++index1)
      {
        try
        {
          buffer[index++] = this.ReadByte();
        }
        catch (EndOfStreamException ex)
        {
          return index1;
        }
      }
      return count;
    }

    public bool ReadBit()
    {
      return (long) this.ReadBits(1) == 1L;
    }

    public short ReadInt16()
    {
      return (short) this.ReadBits(16);
    }

    public int ReadInt32()
    {
      return (int) this.ReadBits(32);
    }

    public ulong ReadInt64()
    {
      return this.ReadBits(64);
    }

    public ushort ReadUInt16()
    {
      return (ushort) this.ReadBits(16);
    }

    public uint ReadUInt32()
    {
      return (uint) this.ReadBits(32);
    }

    public ulong ReadUInt64()
    {
      return this.ReadBits(64);
    }

    public void SkipBytes(int count)
    {
      this.SkipBits(count * 8);
    }
  }
}
