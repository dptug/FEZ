// Type: NVorbis.VorbisStreamDecoder
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NVorbis
{
  internal class VorbisStreamDecoder : IVorbisStreamStatus
  {
    internal Stopwatch _sw = new Stopwatch();
    internal int _upperBitrate;
    internal int _nominalBitrate;
    internal int _lowerBitrate;
    internal string _vendor;
    internal string[] _comments;
    internal int _channels;
    internal int _sampleRate;
    internal int Block0Size;
    internal int Block1Size;
    internal VorbisCodebook[] Books;
    internal VorbisTime[] Times;
    internal VorbisFloor[] Floors;
    internal VorbisResidue[] Residues;
    internal VorbisMapping[] Maps;
    internal VorbisMode[] Modes;
    private int _modeFieldBits;
    internal long _glueBits;
    internal long _metaBits;
    internal long _bookBits;
    internal long _timeHdrBits;
    internal long _floorHdrBits;
    internal long _resHdrBits;
    internal long _mapHdrBits;
    internal long _modeHdrBits;
    internal long _wasteHdrBits;
    internal long _modeBits;
    internal long _floorBits;
    internal long _resBits;
    internal long _wasteBits;
    internal long _samples;
    internal int _packetCount;
    private Func<DataPacket> _getNextPacket;
    private Func<int> _getTotalPages;
    private List<int> _pagesSeen;
    private int _lastPageSeen;
    private bool _eosFound;
    private float[] _prevBuffer;
    private RingBuffer _outputBuffer;
    private Queue<int> _bitsPerPacketHistory;
    private Queue<int> _sampleCountHistory;
    private int _preparedLength;
    private bool _clipped;
    private Stack<DataPacket> _resyncQueue;
    private long _currentPosition;

    internal static byte InitialPacketMarker
    {
      get
      {
        return (byte) 1;
      }
    }

    internal long CurrentPosition
    {
      get
      {
        return this._currentPosition - (long) this._preparedLength;
      }
      set
      {
        this._currentPosition = value;
        this._preparedLength = 0;
        this._eosFound = false;
        this.ResetDecoder();
        ACache.Return<float>(ref this._prevBuffer);
      }
    }

    public int EffectiveBitRate
    {
      get
      {
        if (this._samples == 0L)
          return 0;
        else
          return (int) ((double) this.AudioBits / ((double) (this._currentPosition - (long) this._preparedLength) / (double) this._sampleRate));
      }
    }

    public int InstantBitRate
    {
      get
      {
        try
        {
          return (int) ((long) Enumerable.Sum((IEnumerable<int>) this._bitsPerPacketHistory) * (long) this._sampleRate / (long) Enumerable.Sum((IEnumerable<int>) this._sampleCountHistory));
        }
        catch (DivideByZeroException ex)
        {
          return -1;
        }
      }
    }

    public TimeSpan PageLatency
    {
      get
      {
        return TimeSpan.FromTicks(this._sw.ElapsedTicks / (long) this.PagesRead);
      }
    }

    public TimeSpan PacketLatency
    {
      get
      {
        return TimeSpan.FromTicks(this._sw.ElapsedTicks / (long) this._packetCount);
      }
    }

    public TimeSpan SecondLatency
    {
      get
      {
        return TimeSpan.FromTicks(this._sw.ElapsedTicks / this._samples * (long) this._sampleRate);
      }
    }

    public long OverheadBits
    {
      get
      {
        return this._glueBits + this._metaBits + this._timeHdrBits + this._wasteHdrBits + this._wasteBits;
      }
    }

    public long AudioBits
    {
      get
      {
        return this._bookBits + this._floorHdrBits + this._resHdrBits + this._mapHdrBits + this._modeHdrBits + this._modeBits + this._floorBits + this._resBits;
      }
    }

    public int PagesRead
    {
      get
      {
        return this._pagesSeen.IndexOf(this._lastPageSeen) + 1;
      }
    }

    public int TotalPages
    {
      get
      {
        return this._getTotalPages();
      }
    }

    public bool Clipped
    {
      get
      {
        return this._clipped;
      }
    }

    internal VorbisStreamDecoder(Func<DataPacket> getNextPacket, Func<int> getTotalPages)
    {
      this._getNextPacket = getNextPacket;
      this._getTotalPages = getTotalPages;
      this._pagesSeen = new List<int>();
      this._lastPageSeen = -1;
    }

    internal bool TryInit(DataPacket initialPacket)
    {
      if (!Enumerable.SequenceEqual<byte>((IEnumerable<byte>) initialPacket.ReadBytes(7), (IEnumerable<byte>) new byte[7]
      {
        (byte) 1,
        (byte) 118,
        (byte) 111,
        (byte) 114,
        (byte) 98,
        (byte) 105,
        (byte) 115
      }))
      {
        this._glueBits += (long) (initialPacket.Length * 8);
        return false;
      }
      else
      {
        this._glueBits += 56L;
        this.ProcessStreamHeader(initialPacket);
        bool flag1 = false;
        bool flag2 = false;
        while (!(flag1 & flag2))
        {
          DataPacket packet = this._getNextPacket();
          if (packet.IsResync)
            throw new InvalidDataException("Missing header packets!");
          if (!this._pagesSeen.Contains(packet.PageSequenceNumber))
            this._pagesSeen.Add(packet.PageSequenceNumber);
          switch (packet.PeekByte())
          {
            case (byte) 1:
              throw new InvalidDataException("Found second init header!");
            case (byte) 3:
              this.LoadComments(packet);
              flag1 = true;
              continue;
            case (byte) 5:
              this.LoadBooks(packet);
              flag2 = true;
              continue;
            default:
              continue;
          }
        }
        this.InitDecoder();
        return true;
      }
    }

    private void ProcessStreamHeader(DataPacket packet)
    {
      this._pagesSeen.Add(packet.PageSequenceNumber);
      long bitsRead = packet.BitsRead;
      if (packet.ReadInt32() != 0)
        throw new InvalidDataException("Only Vorbis stream version 0 is supported.");
      this._channels = (int) packet.ReadByte();
      this._sampleRate = packet.ReadInt32();
      this._upperBitrate = packet.ReadInt32();
      this._nominalBitrate = packet.ReadInt32();
      this._lowerBitrate = packet.ReadInt32();
      this.Block0Size = 1 << (int) packet.ReadBits(4);
      this.Block1Size = 1 << (int) packet.ReadBits(4);
      if (this._nominalBitrate == 0 && this._upperBitrate > 0 && this._lowerBitrate > 0)
        this._nominalBitrate = (this._upperBitrate + this._lowerBitrate) / 2;
      this._metaBits += packet.BitsRead - bitsRead + 8L;
      this._wasteHdrBits += (long) (8 * packet.Length) - packet.BitsRead;
    }

    private void LoadComments(DataPacket packet)
    {
      packet.SkipBits(8);
      if (!Enumerable.SequenceEqual<byte>((IEnumerable<byte>) packet.ReadBytes(6), (IEnumerable<byte>) new byte[6]
      {
        (byte) 118,
        (byte) 111,
        (byte) 114,
        (byte) 98,
        (byte) 105,
        (byte) 115
      }))
        throw new InvalidDataException("Corrupted comment header!");
      this._glueBits += 56L;
      this._vendor = Encoding.UTF8.GetString(packet.ReadBytes(packet.ReadInt32()));
      this._comments = new string[packet.ReadInt32()];
      for (int index = 0; index < this._comments.Length; ++index)
        this._comments[index] = Encoding.UTF8.GetString(packet.ReadBytes(packet.ReadInt32()));
      this._metaBits += packet.BitsRead - 56L;
      this._wasteHdrBits += (long) (8 * packet.Length) - packet.BitsRead;
    }

    private void LoadBooks(DataPacket packet)
    {
      packet.SkipBits(8);
      if (!Enumerable.SequenceEqual<byte>((IEnumerable<byte>) packet.ReadBytes(6), (IEnumerable<byte>) new byte[6]
      {
        (byte) 118,
        (byte) 111,
        (byte) 114,
        (byte) 98,
        (byte) 105,
        (byte) 115
      }))
        throw new InvalidDataException("Corrupted book header!");
      long bitsRead1 = packet.BitsRead;
      this._glueBits += packet.BitsRead;
      this.Books = new VorbisCodebook[(int) packet.ReadByte() + 1];
      for (int number = 0; number < this.Books.Length; ++number)
        this.Books[number] = VorbisCodebook.Init(this, packet, number);
      this._bookBits += packet.BitsRead - bitsRead1;
      long bitsRead2 = packet.BitsRead;
      this.Times = new VorbisTime[(int) packet.ReadBits(6) + 1];
      for (int index = 0; index < this.Times.Length; ++index)
        this.Times[index] = VorbisTime.Init(this, packet);
      this._timeHdrBits += packet.BitsRead - bitsRead2;
      long bitsRead3 = packet.BitsRead;
      this.Floors = new VorbisFloor[(int) packet.ReadBits(6) + 1];
      for (int index = 0; index < this.Floors.Length; ++index)
        this.Floors[index] = VorbisFloor.Init(this, packet);
      this._floorHdrBits += packet.BitsRead - bitsRead3;
      long bitsRead4 = packet.BitsRead;
      this.Residues = new VorbisResidue[(int) packet.ReadBits(6) + 1];
      for (int index = 0; index < this.Residues.Length; ++index)
        this.Residues[index] = VorbisResidue.Init(this, packet);
      this._resHdrBits += packet.BitsRead - bitsRead4;
      long bitsRead5 = packet.BitsRead;
      this.Maps = new VorbisMapping[(int) packet.ReadBits(6) + 1];
      for (int index = 0; index < this.Maps.Length; ++index)
        this.Maps[index] = VorbisMapping.Init(this, packet);
      this._mapHdrBits += packet.BitsRead - bitsRead5;
      long bitsRead6 = packet.BitsRead;
      this.Modes = new VorbisMode[(int) packet.ReadBits(6) + 1];
      for (int index = 0; index < this.Modes.Length; ++index)
        this.Modes[index] = VorbisMode.Init(this, packet);
      this._modeHdrBits += packet.BitsRead - bitsRead6;
      if (!packet.ReadBit())
        throw new InvalidDataException();
      ++this._glueBits;
      this._wasteHdrBits += (long) (8 * packet.Length) - packet.BitsRead;
      this._modeFieldBits = Utils.ilog(this.Modes.Length - 1);
    }

    private void InitDecoder()
    {
      if (this._outputBuffer != null)
        this.SaveBuffer();
      this._outputBuffer = new RingBuffer(this.Block1Size * 2 * this._channels);
      this._outputBuffer.Channels = this._channels;
      this._preparedLength = 0;
      this._currentPosition = 0L;
      this._resyncQueue = new Stack<DataPacket>();
      this._bitsPerPacketHistory = new Queue<int>();
      this._sampleCountHistory = new Queue<int>();
    }

    private void ResetDecoder()
    {
      this.SaveBuffer();
      this._outputBuffer.Clear();
      this._preparedLength = 0;
    }

    private void SaveBuffer()
    {
      float[] buffer = ACache.Get<float>(this._preparedLength * this._channels, false);
      this.ReadSamples(buffer, 0, buffer.Length);
      this._prevBuffer = buffer;
    }

    private VorbisStreamDecoder.PacketDecodeInfo UnpackPacket(DataPacket packet)
    {
      if (packet.ReadBit())
        return (VorbisStreamDecoder.PacketDecodeInfo) null;
      VorbisStreamDecoder.PacketDecodeInfo packetDecodeInfo = new VorbisStreamDecoder.PacketDecodeInfo();
      int num1 = this._modeFieldBits;
      try
      {
        packetDecodeInfo.Mode = this.Modes[(int) packet.ReadBits(this._modeFieldBits)];
        if (packetDecodeInfo.Mode.BlockFlag)
        {
          packetDecodeInfo.PrevFlag = packet.ReadBit();
          packetDecodeInfo.NextFlag = packet.ReadBit();
          num1 += 2;
        }
      }
      catch (EndOfStreamException ex)
      {
        return (VorbisStreamDecoder.PacketDecodeInfo) null;
      }
      try
      {
        long bitsRead1 = packet.BitsRead;
        packetDecodeInfo.FloorData = ACache.Get<VorbisFloor.PacketData>(this._channels);
        bool[] buffer1 = ACache.Get<bool>(this._channels);
        for (int index = 0; index < this._channels; ++index)
        {
          packetDecodeInfo.FloorData[index] = packetDecodeInfo.Mode.Mapping.ChannelSubmap[index].Floor.UnpackPacket(packet, packetDecodeInfo.Mode.BlockSize);
          buffer1[index] = !packetDecodeInfo.FloorData[index].ExecuteChannel;
        }
        foreach (VorbisMapping.CouplingStep couplingStep in packetDecodeInfo.Mode.Mapping.CouplingSteps)
        {
          if (packetDecodeInfo.FloorData[couplingStep.Angle].ExecuteChannel || packetDecodeInfo.FloorData[couplingStep.Magnitude].ExecuteChannel)
          {
            packetDecodeInfo.FloorData[couplingStep.Angle].ForceEnergy = true;
            packetDecodeInfo.FloorData[couplingStep.Magnitude].ForceEnergy = true;
          }
        }
        long num2 = packet.BitsRead - bitsRead1;
        long bitsRead2 = packet.BitsRead;
        packetDecodeInfo.Residue = ACache.Get<float>(this._channels, packetDecodeInfo.Mode.BlockSize);
        foreach (VorbisMapping.Submap submap in packetDecodeInfo.Mode.Mapping.Submaps)
        {
          for (int index = 0; index < this._channels; ++index)
          {
            if (packetDecodeInfo.Mode.Mapping.ChannelSubmap[index] != submap)
              packetDecodeInfo.FloorData[index].ForceNoEnergy = true;
          }
          float[][] buffer2 = submap.Residue.Decode(packet, buffer1, this._channels, packetDecodeInfo.Mode.BlockSize);
          for (int index1 = 0; index1 < this._channels; ++index1)
          {
            float[] numArray1 = packetDecodeInfo.Residue[index1];
            float[] numArray2 = buffer2[index1];
            for (int index2 = 0; index2 < packetDecodeInfo.Mode.BlockSize; ++index2)
              numArray1[index2] += numArray2[index2];
          }
          ACache.Return<float>(ref buffer2);
        }
        ACache.Return<bool>(ref buffer1);
        ++this._glueBits;
        this._modeBits += (long) num1;
        this._floorBits += num2;
        this._resBits += packet.BitsRead - bitsRead2;
        this._wasteBits += (long) (8 * packet.Length) - packet.BitsRead;
        ++this._packetCount;
      }
      catch (EndOfStreamException ex)
      {
        this.ResetDecoder();
        packetDecodeInfo = (VorbisStreamDecoder.PacketDecodeInfo) null;
      }
      catch (InvalidDataException ex)
      {
        packetDecodeInfo = (VorbisStreamDecoder.PacketDecodeInfo) null;
      }
      packet.Done();
      return packetDecodeInfo;
    }

    private int DecodePacket(VorbisStreamDecoder.PacketDecodeInfo pdi)
    {
      VorbisMapping.CouplingStep[] couplingStepArray = pdi.Mode.Mapping.CouplingSteps;
      int num1 = pdi.Mode.BlockSize / 2;
      for (int index1 = couplingStepArray.Length - 1; index1 >= 0; --index1)
      {
        float[] numArray1 = pdi.Residue[couplingStepArray[index1].Magnitude];
        float[] numArray2 = pdi.Residue[couplingStepArray[index1].Angle];
        for (int index2 = 0; index2 < num1; ++index2)
        {
          float num2;
          float num3;
          if ((double) numArray1[index2] > 0.0)
          {
            if ((double) numArray2[index2] > 0.0)
            {
              num2 = numArray1[index2];
              num3 = numArray1[index2] - numArray2[index2];
            }
            else
            {
              num3 = numArray1[index2];
              num2 = numArray1[index2] + numArray2[index2];
            }
          }
          else if ((double) numArray2[index2] > 0.0)
          {
            num2 = numArray1[index2];
            num3 = numArray1[index2] + numArray2[index2];
          }
          else
          {
            num3 = numArray1[index2];
            num2 = numArray1[index2] - numArray2[index2];
          }
          numArray1[index2] = num2;
          numArray2[index2] = num3;
        }
      }
      for (int index = 0; index < this._channels; ++index)
      {
        VorbisFloor.PacketData packetData = pdi.FloorData[index];
        float[] numArray = pdi.Residue[index];
        if (packetData.ExecuteChannel)
        {
          pdi.Mode.Mapping.ChannelSubmap[index].Floor.Apply(packetData, numArray);
          Mdct.Reverse(numArray);
        }
      }
      return this.WindowSamples(pdi);
    }

    private int WindowSamples(VorbisStreamDecoder.PacketDecodeInfo pdi)
    {
      float[] window = pdi.Mode.GetWindow(pdi.PrevFlag, pdi.NextFlag);
      int num1 = pdi.Mode.BlockSize;
      int end = num1;
      int switchPoint = end >> 1;
      int start = 0;
      int num2 = -switchPoint;
      int num3 = switchPoint;
      if (pdi.Mode.BlockFlag)
      {
        if (!pdi.PrevFlag)
        {
          start = this.Block1Size / 4 - this.Block0Size / 4;
          switchPoint = start + this.Block0Size / 2;
          num2 = this.Block0Size / -2 - start;
        }
        if (!pdi.NextFlag)
        {
          end -= num1 / 4 - this.Block0Size / 4;
          num3 = num1 / 4 + this.Block0Size / 4;
        }
      }
      int index = this._outputBuffer.Length / this._channels + num2;
      for (int channel = 0; channel < this._channels; ++channel)
        this._outputBuffer.Write(channel, index, start, switchPoint, end, pdi.Residue[channel], window);
      int num4 = this._outputBuffer.Length / this._channels - num3;
      int num5 = num4 - this._preparedLength;
      this._preparedLength = num4;
      return num5;
    }

    private void UpdatePosition(int samplesDecoded, DataPacket packet)
    {
      this._samples += (long) samplesDecoded;
      if (packet.IsResync)
      {
        this._currentPosition = -packet.PageGranulePosition;
        this._resyncQueue.Push(packet);
      }
      else
      {
        if (samplesDecoded <= 0)
          return;
        this._currentPosition += (long) samplesDecoded;
        packet.GranulePosition = this._currentPosition;
        if (this._currentPosition < 0L)
        {
          if (packet.PageGranulePosition > -this._currentPosition)
          {
            long num1 = this._currentPosition - (long) samplesDecoded;
            while (this._resyncQueue.Count > 0)
            {
              DataPacket dataPacket = this._resyncQueue.Pop();
              long num2 = dataPacket.GranulePosition + num1;
              dataPacket.GranulePosition = num1;
              num1 = num2;
            }
          }
          else
          {
            packet.GranulePosition = (long) -samplesDecoded;
            this._resyncQueue.Push(packet);
          }
        }
        else
        {
          if (!packet.IsEndOfStream || this._currentPosition <= packet.PageGranulePosition)
            return;
          int num = (int) (this._currentPosition - packet.PageGranulePosition);
          if (num >= 0)
          {
            this._preparedLength -= num;
            this._currentPosition -= (long) num;
          }
          else
            this._preparedLength = 0;
          packet.GranulePosition = packet.PageGranulePosition;
          this._eosFound = true;
        }
      }
    }

    private void DecodeNextPacket()
    {
      this._sw.Start();
      try
      {
        DataPacket packet = this._getNextPacket();
        if (packet == null)
        {
          this._eosFound = true;
        }
        else
        {
          if (!this._pagesSeen.Contains(this._lastPageSeen = packet.PageSequenceNumber))
            this._pagesSeen.Add(this._lastPageSeen);
          if (packet.IsResync)
            this.ResetDecoder();
          VorbisStreamDecoder.PacketDecodeInfo pdi = this.UnpackPacket(packet);
          if (pdi == null)
          {
            this._wasteBits += (long) (8 * packet.Length);
          }
          else
          {
            int samplesDecoded = this.DecodePacket(pdi);
            if (!packet.GranuleCount.HasValue)
              packet.GranuleCount = new long?((long) samplesDecoded);
            this.UpdatePosition(samplesDecoded, packet);
            int num = Utils.Sum(this._sampleCountHistory) + samplesDecoded;
            this._bitsPerPacketHistory.Enqueue((int) packet.BitsRead);
            this._sampleCountHistory.Enqueue(samplesDecoded);
            while (num > this._sampleRate)
            {
              this._bitsPerPacketHistory.Dequeue();
              num -= this._sampleCountHistory.Dequeue();
            }
          }
        }
      }
      finally
      {
        this._sw.Stop();
      }
    }

    internal int GetPacketLength(DataPacket curPacket, DataPacket lastPacket)
    {
      if (lastPacket == null || curPacket.IsResync || (curPacket.ReadBit() || lastPacket.ReadBit()))
        return 0;
      int index1 = (int) curPacket.ReadBits(this._modeFieldBits);
      if (index1 < 0 || index1 >= this.Modes.Length)
        return 0;
      VorbisMode vorbisMode1 = this.Modes[index1];
      int index2 = (int) lastPacket.ReadBits(this._modeFieldBits);
      if (index2 < 0 || index2 >= this.Modes.Length)
        return 0;
      VorbisMode vorbisMode2 = this.Modes[index2];
      return vorbisMode1.BlockSize / 4 + vorbisMode2.BlockSize / 4;
    }

    internal int ReadSamples(float[] buffer, int offset, int count)
    {
      int num1 = 0;
      if (this._prevBuffer != null)
      {
        int num2 = Math.Min(count, this._prevBuffer.Length);
        Buffer.BlockCopy((Array) this._prevBuffer, 0, (Array) buffer, offset, num2 * 4);
        if (num2 < this._prevBuffer.Length)
        {
          float[] numArray = ACache.Get<float>(this._prevBuffer.Length - num2, false);
          Buffer.BlockCopy((Array) this._prevBuffer, num2 * 4, (Array) numArray, 0, (this._prevBuffer.Length - num2) * 4);
          ACache.Return<float>(ref this._prevBuffer);
          this._prevBuffer = numArray;
        }
        count -= num2;
        offset += num2;
        num1 = num2;
      }
      this._outputBuffer.EnsureSize(count + this.Block1Size * this._channels);
      while (this._preparedLength * this._channels < count)
      {
        if (!this._eosFound)
        {
          try
          {
            this.DecodeNextPacket();
          }
          catch (EndOfStreamException ex)
          {
            this._eosFound = true;
            break;
          }
        }
        else
          break;
      }
      if (this._preparedLength * this._channels < count)
        count = this._preparedLength * this._channels;
      this._outputBuffer.CopyTo(buffer, offset, count);
      this._preparedLength -= count / this._channels;
      return num1 + count;
    }

    public void ResetStats()
    {
      this._clipped = false;
      this._packetCount = 0;
      this._floorBits = 0L;
      this._glueBits = 0L;
      this._modeBits = 0L;
      this._resBits = 0L;
      this._wasteBits = 0L;
      this._samples = 0L;
      this._sw.Reset();
    }

    private class PacketDecodeInfo
    {
      public VorbisMode Mode;
      public bool PrevFlag;
      public bool NextFlag;
      public VorbisFloor.PacketData[] FloorData;
      public float[][] Residue;
    }
  }
}
