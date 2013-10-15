// Type: OpenTK.Platform.Windows.TrackMouseEventStructure
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  internal struct TrackMouseEventStructure
  {
    public static readonly int SizeInBytes = Marshal.SizeOf(typeof (TrackMouseEventStructure));
    public int Size;
    public TrackMouseEventFlags Flags;
    public IntPtr TrackWindowHandle;
    public int HoverTime;

    static TrackMouseEventStructure()
    {
    }
  }
}
