// Type: OpenTK.Platform.Windows.TrackMouseEventFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum TrackMouseEventFlags : uint
  {
    HOVER = 1U,
    LEAVE = 2U,
    NONCLIENT = 16U,
    QUERY = 1073741824U,
    CANCEL = 2147483648U,
  }
}
