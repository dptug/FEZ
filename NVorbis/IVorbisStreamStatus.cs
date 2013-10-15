// Type: NVorbis.IVorbisStreamStatus
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;

namespace NVorbis
{
  public interface IVorbisStreamStatus
  {
    int EffectiveBitRate { get; }

    int InstantBitRate { get; }

    TimeSpan PageLatency { get; }

    TimeSpan PacketLatency { get; }

    TimeSpan SecondLatency { get; }

    long OverheadBits { get; }

    long AudioBits { get; }

    int PagesRead { get; }

    int TotalPages { get; }

    bool Clipped { get; }

    void ResetStats();
  }
}
