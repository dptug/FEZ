// Type: OpenTK.Platform.Windows.WindowPosition
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  internal struct WindowPosition
  {
    internal IntPtr hwnd;
    internal IntPtr hwndInsertAfter;
    internal int x;
    internal int y;
    internal int cx;
    internal int cy;
    [MarshalAs(UnmanagedType.U4)]
    internal SetWindowPosFlags flags;
  }
}
