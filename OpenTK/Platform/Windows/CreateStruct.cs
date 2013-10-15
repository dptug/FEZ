// Type: OpenTK.Platform.Windows.CreateStruct
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  internal struct CreateStruct
  {
    internal IntPtr lpCreateParams;
    internal IntPtr hInstance;
    internal IntPtr hMenu;
    internal IntPtr hwndParent;
    internal int cy;
    internal int cx;
    internal int y;
    internal int x;
    internal int style;
    [MarshalAs(UnmanagedType.LPTStr)]
    internal string lpszName;
    [MarshalAs(UnmanagedType.LPTStr)]
    internal string lpszClass;
    internal int dwExStyle;
  }
}
