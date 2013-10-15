// Type: OpenTK.Platform.Windows.ExtendedWindowClass
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal struct ExtendedWindowClass
  {
    public static uint SizeInBytes = (uint) Marshal.SizeOf((object) new ExtendedWindowClass());
    public uint Size;
    public ClassStyle Style;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public WindowProcedure WndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public IntPtr Instance;
    public IntPtr Icon;
    public IntPtr Cursor;
    public IntPtr Background;
    public IntPtr MenuName;
    public IntPtr ClassName;
    public IntPtr IconSm;

    static ExtendedWindowClass()
    {
    }
  }
}
