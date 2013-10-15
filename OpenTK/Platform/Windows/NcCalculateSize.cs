// Type: OpenTK.Platform.Windows.NcCalculateSize
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  internal struct NcCalculateSize
  {
    public Win32Rectangle NewBounds;
    public Win32Rectangle OldBounds;
    public Win32Rectangle OldClientRectangle;
    public unsafe WindowPosition* Position;
  }
}
