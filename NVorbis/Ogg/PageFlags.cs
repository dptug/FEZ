// Type: NVorbis.Ogg.PageFlags
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;

namespace NVorbis.Ogg
{
  [Flags]
  internal enum PageFlags
  {
    None = 0,
    ContinuesPacket = 1,
    BeginningOfStream = 2,
    EndOfStream = 4,
  }
}
