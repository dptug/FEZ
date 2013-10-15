// Type: NVorbis.Ogg.PacketReader
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using NVorbis;
using System;
using System.IO;

namespace NVorbis.Ogg
{
  internal class PacketReader
  {
    private ContainerReader _container;
    private int _streamSerial;
    private bool _eosFound;
    private Packet _first;
    private Packet _current;
    private Packet _last;

    internal PacketReader(ContainerReader container, int streamSerial)
    {
      this._container = container;
      this._streamSerial = streamSerial;
    }

    internal void AddPacket(DataPacket packet)
    {
      if (packet.IsResync)
      {
        packet.IsContinuation = false;
        if (this._last != null)
          this._last.IsContinued = false;
      }
      if (packet.IsContinuation)
      {
        if (this._last == null)
          throw new InvalidDataException();
        if (!this._last.IsContinued)
          throw new InvalidDataException();
        this._last.MergeWith(packet);
        this._last.IsContinued = packet.IsContinued;
      }
      else
      {
        Packet packet1 = packet as Packet;
        if (packet1 == null)
          throw new ArgumentException("Wrong packet datatype", "packet");
        if (this._first == null)
        {
          this._first = packet1;
          this._last = packet1;
        }
        else
          this._last = (packet1.Prev = this._last).Next = packet1;
      }
      PacketReader packetReader = this;
      int num = packetReader._eosFound | packet.IsEndOfStream ? 1 : 0;
      packetReader._eosFound = num != 0;
    }

    private void GetMorePackets()
    {
      using (ContainerReader.PageReaderLock pageLock = this._container.TakePageReaderLock())
      {
        if (this._eosFound)
          return;
        this._container.GatherNextPage(this._streamSerial, pageLock);
      }
    }

    internal DataPacket GetNextPacket()
    {
      while (this._last == null || this._last.IsContinued || this._current == this._last)
      {
        this.GetMorePackets();
        if (this._eosFound)
        {
          if (this._last.IsContinued)
          {
            this._last = this._last.Prev;
            this._last.Next.Prev = (Packet) null;
            this._last.Next = (Packet) null;
          }
          if (this._current == this._last)
            throw new EndOfStreamException();
        }
      }
      DataPacket dataPacket = this._current != null ? (DataPacket) (this._current = this._current.Next) : (DataPacket) (this._current = this._first);
      if (dataPacket.IsContinued)
        throw new InvalidDataException();
      dataPacket.Reset();
      return dataPacket;
    }

    internal void SeekToPacket(int index)
    {
      this._current = this.GetPacketByIndex(index).Prev;
    }

    private Packet GetPacketByIndex(int index)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException("index");
      while (this._first == null)
      {
        if (this._eosFound)
          throw new InvalidDataException();
        this.GetMorePackets();
      }
      Packet packet = this._first;
      while (--index >= 0)
      {
        while (packet.Next == null)
        {
          if (this._eosFound)
            throw new ArgumentOutOfRangeException("index");
          this.GetMorePackets();
        }
        packet = packet.Next;
      }
      return packet;
    }

    internal void ReadAllPages()
    {
      using (ContainerReader.PageReaderLock pageLock = this._container.TakePageReaderLock())
      {
        while (!this._eosFound)
          this._container.GatherNextPage(this._streamSerial, pageLock);
      }
    }

    internal DataPacket GetLastPacket()
    {
      this.ReadAllPages();
      return (DataPacket) this._last;
    }

    internal int GetTotalPageCount()
    {
      this.ReadAllPages();
      int num1 = 0;
      int num2 = 0;
      for (Packet packet = this._first; packet != null; packet = packet.Next)
      {
        if (packet.PageSequenceNumber != num2)
        {
          ++num1;
          num2 = packet.PageSequenceNumber;
        }
      }
      return num1;
    }

    internal DataPacket GetPacket(int packetIndex)
    {
      Packet packetByIndex = this.GetPacketByIndex(packetIndex);
      packetByIndex.Reset();
      return (DataPacket) packetByIndex;
    }

    internal int FindPacket(long granulePos, Func<DataPacket, DataPacket, DataPacket, int> packetGranuleCountCallback)
    {
      if (granulePos < 0L)
        throw new ArgumentOutOfRangeException("granulePos");
      while (this._last == null || this._last.PageGranulePosition < granulePos)
      {
        if (this._eosFound)
        {
          if (this._first == null)
            throw new InvalidDataException();
          else
            return -1;
        }
        else
          this.GetMorePackets();
      }
      Packet packet = this._last;
      if (packet.IsContinued)
        packet = packet.Prev;
      do
      {
        if (!packet.GranuleCount.HasValue)
        {
          if (packet.Prev != null)
            packet.Prev.Reset();
          packet.Reset();
          if (packet.Next != null)
            packet.Next.Reset();
          packet.GranuleCount = new long?((long) packetGranuleCountCallback((DataPacket) packet.Prev, (DataPacket) packet, (DataPacket) packet.Next));
          if (packet == this._last || this._eosFound && packet == this._last.Prev || (packet.Next.IsContinued || packet.Next.PageSequenceNumber > packet.PageSequenceNumber))
          {
            if (packet.PageGranulePosition == -1L)
              throw new InvalidDataException();
            packet.GranulePosition = packet.PageGranulePosition;
            if (packet == this._last && this._eosFound)
              packet.GranuleCount = new long?(packet.PageGranulePosition - packet.Prev.PageGranulePosition);
          }
          else
            packet.GranulePosition = packet.Next.GranulePosition - packet.Next.GranuleCount.Value;
        }
        if (packet.GranulePosition < granulePos)
        {
          packet = packet.Next;
          break;
        }
        else
          packet = packet.Prev;
      }
      while (packet != null);
      if (packet == null)
        return -1;
      int num = 0;
      while (packet.Prev != null)
      {
        packet = packet.Prev;
        ++num;
      }
      return num;
    }
  }
}
