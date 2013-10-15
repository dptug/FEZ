// Type: NVorbis.Ogg.Packet
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using NVorbis;
using System;
using System.IO;

namespace NVorbis.Ogg
{
  internal class Packet : DataPacket
  {
    private Stream _stream;
    private long _offset;
    private long _length;
    private Packet _mergedPacket;
    private byte[] _savedBuffer;
    private int _bufOffset;
    private int _curOfs;

    internal Packet Next { get; set; }

    internal Packet Prev { get; set; }

    protected override bool CanReset
    {
      get
      {
        return true;
      }
    }

    internal Packet(Stream stream, long streamOffset, int length)
      : base(length)
    {
      this._stream = stream;
      this._offset = streamOffset;
      this._length = (long) length;
      this._curOfs = 0;
    }

    internal void SetBuffer(byte[] savedBuf, int offset)
    {
      this._savedBuffer = savedBuf;
      this._bufOffset = offset;
    }

    protected override void DoMergeWith(DataPacket continuation)
    {
      Packet packet1 = continuation as Packet;
      if (packet1 == null)
        throw new ArgumentException("Incorrect packet type!");
      Packet packet2 = this;
      int num = packet2.Length + continuation.Length;
      packet2.Length = num;
      if (this._mergedPacket == null)
        this._mergedPacket = packet1;
      else
        this._mergedPacket.DoMergeWith(continuation);
      this.PageGranulePosition = continuation.PageGranulePosition;
      this.PageSequenceNumber = continuation.PageSequenceNumber;
    }

    protected override void DoReset()
    {
      this._curOfs = 0;
      if (this._mergedPacket == null)
        return;
      this._mergedPacket.DoReset();
    }

    protected override int ReadNextByte()
    {
      if ((long) this._curOfs == this._length)
      {
        if (this._mergedPacket == null)
          return -1;
        else
          return this._mergedPacket.ReadNextByte();
      }
      else
      {
        if (this._savedBuffer != null)
          return (int) this._savedBuffer[this._bufOffset + this._curOfs++];
        this._stream.Seek((long) this._curOfs + this._offset, SeekOrigin.Begin);
        int num = this._stream.ReadByte();
        ++this._curOfs;
        return num;
      }
    }

    public override void Done()
    {
      if (this._savedBuffer == null)
        return;
      this._savedBuffer = (byte[]) null;
    }
  }
}
