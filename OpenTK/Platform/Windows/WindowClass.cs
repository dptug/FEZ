// Type: OpenTK.Platform.Windows.WindowClass
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  internal struct WindowClass
  {
    internal static int SizeInBytes = Marshal.SizeOf((object) new WindowClass());
    internal ClassStyle Style;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    internal WindowProcedure WindowProcedure;
    internal int ClassExtraBytes;
    internal int WindowExtraBytes;
    internal IntPtr Instance;
    internal IntPtr Icon;
    internal IntPtr Cursor;
    internal IntPtr BackgroundBrush;
    internal IntPtr MenuName;
    [MarshalAs(UnmanagedType.LPTStr)]
    internal string ClassName;

    static WindowClass()
    {
    }
  }
}
