// Type: NVorbis.Ogg.ContainerReader
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace NVorbis.Ogg
{
  internal class ContainerReader : IPacketProvider, IDisposable
  {
    private Mutex _pageLock = new Mutex(false);
    private static uint[] crcTable = new uint[256];
    private const uint CRC32_POLY = 79764919U;
    private Stream _stream;
    private Dictionary<int, PacketReader> _packetReaders;
    private Dictionary<int, bool> _eosFlags;
    private List<int> _streamSerials;
    private long _nextPageOffset;
    private int _pageCount;
    private Action<int> _newStreamCallback;
    internal long _containerBits;

    internal int[] StreamSerials
    {
      get
      {
        return this._streamSerials.ToArray();
      }
    }

    bool IPacketProvider.CanSeek
    {
      get
      {
        return true;
      }
    }

    long IPacketProvider.ContainerBits
    {
      get
      {
        return this._containerBits;
      }
    }

    static ContainerReader()
    {
      for (uint index1 = 0U; index1 < 256U; ++index1)
      {
        uint num = index1 << 24;
        for (int index2 = 0; index2 < 8; ++index2)
          num = (uint) ((int) num << 1 ^ (num >= (uint) int.MinValue ? 79764919 : 0));
        ContainerReader.crcTable[(IntPtr) index1] = num;
      }
    }

    internal ContainerReader(Stream stream, Action<int> newStreamCallback)
    {
      if (!stream.CanSeek)
        throw new ArgumentException("stream must be seekable!");
      this._stream = (Stream) new ThreadSafeStream(stream);
      this._packetReaders = new Dictionary<int, PacketReader>();
      this._eosFlags = new Dictionary<int, bool>();
      this._streamSerials = new List<int>();
      this._newStreamCallback = newStreamCallback;
    }

    void IPacketProvider.Init()
    {
      this.GatherNextPage("Not an OGG container!");
    }

    void IDisposable.Dispose()
    {
      this._packetReaders.Clear();
      this._nextPageOffset = 0L;
      this._containerBits = 0L;
      this._stream.Dispose();
    }

    private ContainerReader.PageHeader ReadPageHeader(long position)
    {
      this._stream.Position = position;
      byte[] buffer = new byte[27];
      if (this._stream.Read(buffer, 0, buffer.Length) != buffer.Length)
        return (ContainerReader.PageHeader) null;
      if ((int) buffer[0] != 79 || (int) buffer[1] != 103 || ((int) buffer[2] != 103 || (int) buffer[3] != 83))
        return (ContainerReader.PageHeader) null;
      if ((int) buffer[4] != 0)
        return (ContainerReader.PageHeader) null;
      ContainerReader.PageHeader pageHeader = new ContainerReader.PageHeader();
      pageHeader.Flags = (PageFlags) buffer[5];
      pageHeader.GranulePosition = BitConverter.ToInt64(buffer, 6);
      pageHeader.StreamSerial = BitConverter.ToInt32(buffer, 14);
      pageHeader.SequenceNumber = BitConverter.ToInt32(buffer, 18);
      uint num = BitConverter.ToUInt32(buffer, 22);
      uint crc = 0U;
      for (int index = 0; index < 22; ++index)
        this.UpdateCRC((int) buffer[index], ref crc);
      this.UpdateCRC(0, ref crc);
      this.UpdateCRC(0, ref crc);
      this.UpdateCRC(0, ref crc);
      this.UpdateCRC(0, ref crc);
      this.UpdateCRC((int) buffer[26], ref crc);
      int length1 = (int) buffer[26];
      int[] numArray1 = new int[length1];
      int count = 0;
      int length2 = 0;
      for (int index = 0; index < length1; ++index)
      {
        int nextVal = this._stream.ReadByte();
        this.UpdateCRC(nextVal, ref crc);
        numArray1[length2] += nextVal;
        if (nextVal < (int) byte.MaxValue)
        {
          ++length2;
          pageHeader.LastPacketContinues = false;
        }
        else
          pageHeader.LastPacketContinues = true;
        count += nextVal;
      }
      if (pageHeader.LastPacketContinues)
        ++length2;
      if (length2 < numArray1.Length)
      {
        int[] numArray2 = new int[length2];
        for (int index = 0; index < length2; ++index)
          numArray2[index] = numArray1[index];
        numArray1 = numArray2;
      }
      pageHeader.PacketSizes = numArray1;
      pageHeader.DataOffset = position + 27L + (long) length1;
      pageHeader.SavedBuffer = new byte[count];
      if (this._stream.Read(pageHeader.SavedBuffer, 0, count) != count)
        return (ContainerReader.PageHeader) null;
      int index1 = -1;
      while (++index1 < count)
        this.UpdateCRC((int) pageHeader.SavedBuffer[index1], ref crc);
      if ((int) crc != (int) num)
        return (ContainerReader.PageHeader) null;
      this._containerBits += (long) (8 * (27 + length1));
      ++this._pageCount;
      return pageHeader;
    }

    private void UpdateCRC(int nextVal, ref uint crc)
    {
      crc = crc << 8 ^ ContainerReader.crcTable[(long) nextVal ^ (long) (crc >> 24)];
    }

    private ContainerReader.PageHeader FindNextPageHeader()
    {
      long position = this._nextPageOffset;
      bool flag = false;
      ContainerReader.PageHeader pageHeader;
      while ((pageHeader = this.ReadPageHeader(position)) == null)
      {
        flag = true;
        this._containerBits += 8L;
        this._stream.Position = ++position;
        int num = 0;
        do
        {
          if (this._stream.ReadByte() == 79)
          {
            if (this._stream.ReadByte() == 103 && this._stream.ReadByte() == 103 && this._stream.ReadByte() == 83)
            {
              position += (long) num;
              break;
            }
            else
              this._stream.Seek(-3L, SeekOrigin.Current);
          }
          this._containerBits += 8L;
        }
        while (++num < 65536);
        if (num == 65536)
          return (ContainerReader.PageHeader) null;
      }
      pageHeader.IsResync = flag;
      this._nextPageOffset = pageHeader.DataOffset;
      for (int index = 0; index < pageHeader.PacketSizes.Length; ++index)
        this._nextPageOffset += (long) pageHeader.PacketSizes[index];
      return pageHeader;
    }

    private bool AddPage(ContainerReader.PageHeader hdr)
    {
      PacketReader packetReader;
      if (!this._packetReaders.TryGetValue(hdr.StreamSerial, out packetReader))
        packetReader = new PacketReader(this, hdr.StreamSerial);
      bool flag1 = false;
      bool flag2 = (hdr.Flags & PageFlags.ContinuesPacket) == PageFlags.ContinuesPacket;
      bool flag3 = (hdr.Flags & PageFlags.EndOfStream) == PageFlags.EndOfStream;
      bool flag4 = hdr.IsResync;
      int offset = 0;
      int length1 = hdr.PacketSizes.Length;
      foreach (int length2 in hdr.PacketSizes)
      {
        Packet packet1 = new Packet(this._stream, hdr.DataOffset + (long) offset, length2);
        packet1.PageGranulePosition = hdr.GranulePosition;
        packet1.IsEndOfStream = flag3;
        packet1.PageSequenceNumber = hdr.SequenceNumber;
        packet1.IsContinued = flag1;
        packet1.IsContinuation = flag2;
        packet1.IsResync = flag4;
        Packet packet2 = packet1;
        packet2.SetBuffer(hdr.SavedBuffer, offset);
        packetReader.AddPacket((DataPacket) packet2);
        offset += length2;
        flag2 = false;
        flag4 = false;
        if (--length1 == 1)
          flag1 = hdr.LastPacketContinues;
      }
      if (!this._packetReaders.ContainsKey(hdr.StreamSerial))
      {
        int streamSerial = hdr.StreamSerial;
        this._packetReaders.Add(streamSerial, packetReader);
        this._eosFlags.Add(streamSerial, flag3);
        this._streamSerials.Add(streamSerial);
        return true;
      }
      else
      {
        Dictionary<int, bool> dictionary;
        int streamSerial;
        (dictionary = this._eosFlags)[streamSerial = hdr.StreamSerial] = dictionary[streamSerial] | flag3;
        return false;
      }
    }

    internal ContainerReader.PageReaderLock TakePageReaderLock()
    {
      return new ContainerReader.PageReaderLock(this._pageLock);
    }

    private int GatherNextPage(string noPageErrorMessage)
    {
      ContainerReader.PageHeader nextPageHeader = this.FindNextPageHeader();
      if (nextPageHeader == null)
        throw new InvalidDataException(noPageErrorMessage);
      if (this.AddPage(nextPageHeader))
      {
        Action<int> action = this._newStreamCallback;
        if (action != null)
          action(nextPageHeader.StreamSerial);
      }
      return nextPageHeader.StreamSerial;
    }

    internal void GatherNextPage(int streamSerial, ContainerReader.PageReaderLock pageLock)
    {
      if (pageLock == null)
        throw new ArgumentNullException("pageLock");
      if (!pageLock.Validate(this._pageLock))
        throw new ArgumentException("pageLock");
      if (!this._eosFlags.ContainsKey(streamSerial))
        throw new ArgumentOutOfRangeException("streamSerial");
      while (!this._eosFlags[streamSerial])
      {
        if (this.GatherNextPage("Could not find next page.") == streamSerial)
          return;
      }
      throw new EndOfStreamException();
    }

    DataPacket IPacketProvider.GetNextPacket(int streamSerial)
    {
      return this._packetReaders[streamSerial].GetNextPacket();
    }

    long IPacketProvider.GetLastGranulePos(int streamSerial)
    {
      return this._packetReaders[streamSerial].GetLastPacket().PageGranulePosition;
    }

    bool IPacketProvider.FindNextStream(int currentStreamSerial)
    {
      int num = Array.IndexOf<int>(this.StreamSerials, currentStreamSerial);
      int count = this._packetReaders.Count;
      if (num < count - 1)
        return true;
      using (this.TakePageReaderLock())
      {
        while (this._packetReaders.Count == count)
        {
          try
          {
            this.GatherNextPage(string.Empty);
          }
          catch (InvalidDataException ex)
          {
            break;
          }
        }
        return count > this._packetReaders.Count;
      }
    }

    internal int GetReadPageCount()
    {
      return this._pageCount;
    }

    internal int GetTotalPageCount()
    {
      this._eosFlags.Add(-1, false);
      while (this._stream.Position < this._stream.Length - 28L)
      {
        using (ContainerReader.PageReaderLock pageLock = this.TakePageReaderLock())
          this.GatherNextPage(-1, pageLock);
      }
      this._eosFlags.Remove(-1);
      return this._pageCount;
    }

    int IPacketProvider.GetTotalPageCount(int streamSerial)
    {
      return this._packetReaders[streamSerial].GetTotalPageCount();
    }

    int IPacketProvider.FindPacket(int streamSerial, long granulePos, Func<DataPacket, DataPacket, DataPacket, int> packetGranuleCountCallback)
    {
      return this._packetReaders[streamSerial].FindPacket(granulePos, packetGranuleCountCallback);
    }

    void IPacketProvider.SeekToPacket(int streamSerial, int packetIndex)
    {
      this._packetReaders[streamSerial].SeekToPacket(packetIndex);
    }

    DataPacket IPacketProvider.GetPacket(int streamSerial, int packetIndex)
    {
      return this._packetReaders[streamSerial].GetPacket(packetIndex);
    }

    private class PageHeader
    {
      public int StreamSerial { get; set; }

      public PageFlags Flags { get; set; }

      public long GranulePosition { get; set; }

      public int SequenceNumber { get; set; }

      public long DataOffset { get; set; }

      public int[] PacketSizes { get; set; }

      public bool LastPacketContinues { get; set; }

      public bool IsResync { get; set; }

      public byte[] SavedBuffer { get; set; }
    }

    internal class PageReaderLock : IDisposable
    {
      private Mutex _lock;

      public PageReaderLock(Mutex pageLock)
      {
        (this._lock = pageLock).WaitOne();
      }

      public bool Validate(Mutex pageLock)
      {
        return object.ReferenceEquals((object) pageLock, (object) this._lock);
      }

      public void Dispose()
      {
        this._lock.ReleaseMutex();
      }
    }
  }
}
