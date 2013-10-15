// Type: NVorbis.VorbisReader
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using NVorbis.Ogg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NVorbis
{
  public class VorbisReader : IDisposable
  {
    private IPacketProvider _packetProvider;
    private int _streamIdx;
    private Stream _sourceStream;
    private bool _closeSourceOnDispose;
    private List<VorbisStreamDecoder> _decoders;
    private List<int> _serials;

    public int Channels
    {
      get
      {
        return this._decoders[this._streamIdx]._channels;
      }
    }

    public int SampleRate
    {
      get
      {
        return this._decoders[this._streamIdx]._sampleRate;
      }
    }

    public int UpperBitrate
    {
      get
      {
        return this._decoders[this._streamIdx]._upperBitrate;
      }
    }

    public int NominalBitrate
    {
      get
      {
        return this._decoders[this._streamIdx]._nominalBitrate;
      }
    }

    public int LowerBitrate
    {
      get
      {
        return this._decoders[this._streamIdx]._lowerBitrate;
      }
    }

    public string Vendor
    {
      get
      {
        return this._decoders[this._streamIdx]._vendor;
      }
    }

    public string[] Comments
    {
      get
      {
        return this._decoders[this._streamIdx]._comments;
      }
    }

    public long ContainerOverheadBits
    {
      get
      {
        return this._packetProvider.ContainerBits;
      }
    }

    public IVorbisStreamStatus[] Stats
    {
      get
      {
        return Enumerable.ToArray<IVorbisStreamStatus>(Enumerable.Cast<IVorbisStreamStatus>((IEnumerable) Enumerable.Select<VorbisStreamDecoder, VorbisStreamDecoder>((IEnumerable<VorbisStreamDecoder>) this._decoders, (Func<VorbisStreamDecoder, VorbisStreamDecoder>) (d => d))));
      }
    }

    public int StreamIndex
    {
      get
      {
        return this._streamIdx;
      }
    }

    public int StreamCount
    {
      get
      {
        return this._decoders.Count;
      }
    }

    public TimeSpan DecodedTime
    {
      get
      {
        return TimeSpan.FromSeconds((double) this._decoders[this._streamIdx].CurrentPosition / (double) this._decoders[this._streamIdx]._sampleRate);
      }
      set
      {
        this.SeekTo((long) (value.TotalSeconds * (double) this.SampleRate));
      }
    }

    public TimeSpan TotalTime
    {
      get
      {
        return TimeSpan.FromSeconds((double) this._packetProvider.GetLastGranulePos(this._serials[this._streamIdx]) / (double) this._decoders[this._streamIdx]._sampleRate);
      }
    }

    public VorbisReader(string fileName)
      : this((Stream) File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read), true)
    {
    }

    public VorbisReader(Stream stream, bool closeStreamOnDispose)
    {
      this._decoders = new List<VorbisStreamDecoder>();
      this._serials = new List<int>();
      if (!stream.CanSeek)
        throw new NotSupportedException("The specified stream is not seekable.");
      this._packetProvider = (IPacketProvider) new ContainerReader(stream, new Action<int>(this.NewStream));
      this._packetProvider.Init();
      this._sourceStream = stream;
      this._closeSourceOnDispose = closeStreamOnDispose;
      if (this._decoders.Count == 0)
        throw new InvalidDataException("No Vorbis data is available in the specified file.");
      this._streamIdx = 0;
    }

    private void NewStream(int streamSerial)
    {
      DataPacket nextPacket = this._packetProvider.GetNextPacket(streamSerial);
      if ((int) nextPacket.PeekByte() != (int) VorbisStreamDecoder.InitialPacketMarker)
        return;
      VorbisStreamDecoder vorbisStreamDecoder = new VorbisStreamDecoder((Func<DataPacket>) (() =>
      {
        IPacketProvider local_0 = this._packetProvider;
        if (local_0 != null)
          return local_0.GetNextPacket(streamSerial);
        else
          return (DataPacket) null;
      }), (Func<int>) (() =>
      {
        IPacketProvider local_0 = this._packetProvider;
        if (local_0 != null)
          return local_0.GetTotalPageCount(streamSerial);
        else
          return 0;
      }));
      try
      {
        if (!vorbisStreamDecoder.TryInit(nextPacket))
          return;
        this._decoders.Add(vorbisStreamDecoder);
        this._serials.Add(streamSerial);
      }
      catch (InvalidDataException ex)
      {
      }
    }

    public void Dispose()
    {
      if (this._packetProvider != null)
      {
        this._packetProvider.Dispose();
        this._packetProvider = (IPacketProvider) null;
      }
      if (this._closeSourceOnDispose)
        this._sourceStream.Close();
      this._sourceStream = (Stream) null;
    }

    private void SeekTo(long granulePos)
    {
      if (!this._packetProvider.CanSeek)
        throw new NotSupportedException();
      if (granulePos < 0L)
        throw new ArgumentOutOfRangeException("granulePos");
      int packetIndex = 3;
      if (granulePos > 0L)
      {
        int packet = this._packetProvider.FindPacket(this._serials[this._streamIdx], granulePos, (Func<DataPacket, DataPacket, DataPacket, int>) ((prevPacket, curPacket, nextPacket) => this._decoders[this._streamIdx].GetPacketLength(curPacket, prevPacket)));
        if (packet == -1)
          throw new ArgumentOutOfRangeException("granulePos");
        packetIndex = packet - 1;
      }
      DataPacket packet1 = this._packetProvider.GetPacket(this._serials[this._streamIdx], packetIndex);
      this._packetProvider.SeekToPacket(this._serials[this._streamIdx], packetIndex);
      this._decoders[this._streamIdx].CurrentPosition = packet1.GranulePosition;
      int count = (int) ((granulePos - this._decoders[this._streamIdx].CurrentPosition) * (long) this.Channels);
      if (count <= 0)
        return;
      float[] buffer = new float[count];
      while (count > 0)
      {
        int num = this._decoders[this._streamIdx].ReadSamples(buffer, 0, count);
        if (num == 0)
          break;
        count -= num;
      }
    }

    public int ReadSamples(float[] buffer, int offset, int count)
    {
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset");
      if (count < 0 || offset + count > buffer.Length)
        throw new ArgumentOutOfRangeException("count");
      else
        return this._decoders[this._streamIdx].ReadSamples(buffer, offset, count);
    }

    public bool SwitchStreams(int index)
    {
      if (index < 0 || index >= this.StreamCount)
        throw new ArgumentOutOfRangeException("index");
      if (this._streamIdx == index)
        return false;
      VorbisStreamDecoder vorbisStreamDecoder1 = this._decoders[this._streamIdx];
      this._streamIdx = index;
      VorbisStreamDecoder vorbisStreamDecoder2 = this._decoders[this._streamIdx];
      if (vorbisStreamDecoder1._channels == vorbisStreamDecoder2._channels)
        return vorbisStreamDecoder1._sampleRate != vorbisStreamDecoder2._sampleRate;
      else
        return true;
    }
  }
}
