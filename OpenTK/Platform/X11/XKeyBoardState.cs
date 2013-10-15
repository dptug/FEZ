// Type: OpenTK.Platform.X11.XKeyBoardState
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  internal struct XKeyBoardState
  {
    public int key_click_percent;
    public int bell_percent;
    public uint bell_pitch;
    public uint bell_duration;
    public IntPtr led_mask;
    public int global_auto_repeat;
    public XKeyBoardState.AutoRepeats auto_repeats;

    [StructLayout(LayoutKind.Explicit)]
    internal struct AutoRepeats
    {
      [FieldOffset(0)]
      public byte first;
      [FieldOffset(31)]
      public byte last;
    }
  }
}
