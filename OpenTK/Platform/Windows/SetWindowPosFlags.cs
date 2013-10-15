// Type: OpenTK.Platform.Windows.SetWindowPosFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum SetWindowPosFlags
  {
    NOSIZE = 1,
    NOMOVE = 2,
    NOZORDER = 4,
    NOREDRAW = 8,
    NOACTIVATE = 16,
    FRAMECHANGED = 32,
    SHOWWINDOW = 64,
    HIDEWINDOW = 128,
    NOCOPYBITS = 256,
    NOOWNERZORDER = 512,
    NOSENDCHANGING = 1024,
    DRAWFRAME = FRAMECHANGED,
    NOREPOSITION = NOOWNERZORDER,
    DEFERERASE = 8192,
    ASYNCWINDOWPOS = 16384,
  }
}
