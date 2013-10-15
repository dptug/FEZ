// Type: NVorbis.VorbisTime
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System.IO;

namespace NVorbis
{
  internal abstract class VorbisTime
  {
    private VorbisStreamDecoder _vorbis;

    protected VorbisTime(VorbisStreamDecoder vorbis)
    {
      this._vorbis = vorbis;
    }

    internal static VorbisTime Init(VorbisStreamDecoder vorbis, DataPacket packet)
    {
      int num = (int) packet.ReadBits(16);
      VorbisTime vorbisTime = (VorbisTime) null;
      if (num == 0)
        vorbisTime = (VorbisTime) new VorbisTime.Time0(vorbis);
      if (vorbisTime == null)
        throw new InvalidDataException();
      vorbisTime.Init(packet);
      return vorbisTime;
    }

    protected abstract void Init(DataPacket packet);

    private class Time0 : VorbisTime
    {
      internal Time0(VorbisStreamDecoder vorbis)
        : base(vorbis)
      {
      }

      protected override void Init(DataPacket packet)
      {
      }
    }
  }
}
