// Type: OpenTK.Platform.Windows.WinMMJoystick
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenTK.Platform.Windows
{
  internal sealed class WinMMJoystick : IJoystickDriver, IGamePadDriver
  {
    private List<JoystickDevice> sticks = new List<JoystickDevice>();
    private IList<JoystickDevice> sticks_readonly;
    private bool disposed;

    public int DeviceCount
    {
      get
      {
        return this.sticks.Count;
      }
    }

    public IList<JoystickDevice> Joysticks
    {
      get
      {
        return this.sticks_readonly;
      }
    }

    public WinMMJoystick()
    {
      this.sticks_readonly = (IList<JoystickDevice>) this.sticks.AsReadOnly();
      int num = 0;
      while (num < WinMMJoystick.UnsafeNativeMethods.joyGetNumDevs())
      {
        JoystickDevice<WinMMJoystick.WinMMJoyDetails> joystickDevice = this.OpenJoystick(num++);
        if (joystickDevice != null)
          this.sticks.Add((JoystickDevice) joystickDevice);
      }
    }

    ~WinMMJoystick()
    {
      this.Dispose(false);
    }

    private JoystickDevice<WinMMJoystick.WinMMJoyDetails> OpenJoystick(int number)
    {
      WinMMJoystick.JoyCaps pjc;
      if (WinMMJoystick.UnsafeNativeMethods.joyGetDevCaps(number, out pjc, WinMMJoystick.JoyCaps.SizeInBytes) != WinMMJoystick.JoystickError.NoError)
        return (JoystickDevice<WinMMJoystick.WinMMJoyDetails>) null;
      int num1 = pjc.NumAxes;
      if ((pjc.Capabilities & WinMMJoystick.JoystCapsFlags.HasPov) != (WinMMJoystick.JoystCapsFlags) 0)
        num1 += 2;
      JoystickDevice<WinMMJoystick.WinMMJoyDetails> joystickDevice = new JoystickDevice<WinMMJoystick.WinMMJoyDetails>(number, num1, pjc.NumButtons);
      joystickDevice.Details = new WinMMJoystick.WinMMJoyDetails(num1);
      int index = 0;
      if (index < pjc.NumAxes)
      {
        joystickDevice.Details.Min[index] = (float) pjc.XMin;
        joystickDevice.Details.Max[index] = (float) pjc.XMax;
        ++index;
      }
      if (index < pjc.NumAxes)
      {
        joystickDevice.Details.Min[index] = (float) pjc.YMax;
        joystickDevice.Details.Max[index] = (float) pjc.YMin;
        ++index;
      }
      if (index < pjc.NumAxes)
      {
        joystickDevice.Details.Min[index] = (float) pjc.ZMax;
        joystickDevice.Details.Max[index] = (float) pjc.ZMin;
        ++index;
      }
      if (index < pjc.NumAxes)
      {
        joystickDevice.Details.Min[index] = (float) pjc.RMin;
        joystickDevice.Details.Max[index] = (float) pjc.RMax;
        ++index;
      }
      if (index < pjc.NumAxes)
      {
        joystickDevice.Details.Min[index] = (float) pjc.UMin;
        joystickDevice.Details.Max[index] = (float) pjc.UMax;
        ++index;
      }
      if (index < pjc.NumAxes)
      {
        joystickDevice.Details.Min[index] = (float) pjc.VMax;
        joystickDevice.Details.Max[index] = (float) pjc.VMin;
        int num2 = index + 1;
      }
      if ((pjc.Capabilities & WinMMJoystick.JoystCapsFlags.HasPov) != (WinMMJoystick.JoystCapsFlags) 0)
      {
        joystickDevice.Details.PovType = WinMMJoystick.PovType.Exists;
        if ((pjc.Capabilities & WinMMJoystick.JoystCapsFlags.HasPov4Dir) != (WinMMJoystick.JoystCapsFlags) 0)
          joystickDevice.Details.PovType |= WinMMJoystick.PovType.Discrete;
        if ((pjc.Capabilities & WinMMJoystick.JoystCapsFlags.HasPovContinuous) != (WinMMJoystick.JoystCapsFlags) 0)
          joystickDevice.Details.PovType |= WinMMJoystick.PovType.Continuous;
      }
      joystickDevice.Description = string.Format("Joystick/Joystick #{0} ({1} axes, {2} buttons)", (object) number, (object) joystickDevice.Axis.Count, (object) joystickDevice.Button.Count);
      return joystickDevice;
    }

    public void Poll()
    {
      foreach (JoystickDevice<WinMMJoystick.WinMMJoyDetails> joystickDevice1 in this.sticks)
      {
        WinMMJoystick.JoyInfoEx pji = new WinMMJoystick.JoyInfoEx();
        pji.Size = WinMMJoystick.JoyInfoEx.SizeInBytes;
        pji.Flags = WinMMJoystick.JoystickFlags.All;
        int num1 = (int) WinMMJoystick.UnsafeNativeMethods.joyGetPosEx(joystickDevice1.Id, ref pji);
        int count = joystickDevice1.Axis.Count;
        if ((joystickDevice1.Details.PovType & WinMMJoystick.PovType.Exists) != WinMMJoystick.PovType.None)
          count -= 2;
        int axis = 0;
        if (axis < count)
        {
          joystickDevice1.SetAxis((JoystickAxis) axis, joystickDevice1.Details.CalculateOffset((float) pji.XPos, axis));
          ++axis;
        }
        if (axis < count)
        {
          joystickDevice1.SetAxis((JoystickAxis) axis, joystickDevice1.Details.CalculateOffset((float) pji.YPos, axis));
          ++axis;
        }
        if (axis < count)
        {
          joystickDevice1.SetAxis((JoystickAxis) axis, joystickDevice1.Details.CalculateOffset((float) pji.ZPos, axis));
          ++axis;
        }
        if (axis < count)
        {
          joystickDevice1.SetAxis((JoystickAxis) axis, joystickDevice1.Details.CalculateOffset((float) pji.RPos, axis));
          ++axis;
        }
        if (axis < count)
        {
          joystickDevice1.SetAxis((JoystickAxis) axis, joystickDevice1.Details.CalculateOffset((float) pji.UPos, axis));
          ++axis;
        }
        if (axis < count)
        {
          joystickDevice1.SetAxis((JoystickAxis) axis, joystickDevice1.Details.CalculateOffset((float) pji.VPos, axis));
          ++axis;
        }
        if ((joystickDevice1.Details.PovType & WinMMJoystick.PovType.Exists) != WinMMJoystick.PovType.None)
        {
          float num2 = 0.0f;
          float num3 = 0.0f;
          if ((int) (ushort) pji.Pov != (int) ushort.MaxValue)
          {
            if (pji.Pov > 27000 || pji.Pov < 9000)
              num3 = 1f;
            if (pji.Pov > 0 && pji.Pov < 18000)
              num2 = 1f;
            if (pji.Pov > 9000 && pji.Pov < 27000)
              num3 = -1f;
            if (pji.Pov > 18000)
              num2 = -1f;
          }
          JoystickDevice<WinMMJoystick.WinMMJoyDetails> joystickDevice2 = joystickDevice1;
          int num4 = axis;
          int num5 = 1;
          int num6 = num4 + num5;
          double num7 = (double) num2;
          joystickDevice2.SetAxis((JoystickAxis) num4, (float) num7);
          JoystickDevice<WinMMJoystick.WinMMJoyDetails> joystickDevice3 = joystickDevice1;
          int num8 = num6;
          int num9 = 1;
          int num10 = num8 + num9;
          double num11 = (double) num3;
          joystickDevice3.SetAxis((JoystickAxis) num8, (float) num11);
        }
        for (int index = 0; index < joystickDevice1.Button.Count; ++index)
          joystickDevice1.SetButton((JoystickButton) index, ((long) pji.Buttons & (long) (1 << index)) != 0L);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool manual)
    {
      if (this.disposed)
        return;
      int num = manual ? 1 : 0;
      this.disposed = true;
    }

    [Flags]
    private enum JoystickFlags
    {
      X = 1,
      Y = 2,
      Z = 4,
      R = 8,
      U = 16,
      V = 32,
      Pov = 64,
      Buttons = 128,
      All = Buttons | Pov | V | U | R | Z | Y | X,
    }

    private enum JoystickError : uint
    {
      NoError = 0U,
      InvalidParameters = 165U,
      NoCanDo = 166U,
      Unplugged = 167U,
    }

    [Flags]
    private enum JoystCapsFlags
    {
      HasZ = 1,
      HasR = 2,
      HasU = 4,
      HasV = 8,
      HasPov = 22,
      HasPov4Dir = 50,
      HasPovContinuous = 100,
    }

    private enum JoystickPovPosition : ushort
    {
      Forward = (ushort) 0,
      Right = (ushort) 9000,
      Backward = (ushort) 18000,
      Left = (ushort) 27000,
      Centered = (ushort) 65535,
    }

    private struct JoyCaps
    {
      public static readonly int SizeInBytes = Marshal.SizeOf((object) new WinMMJoystick.JoyCaps());
      public ushort Mid;
      public ushort ProductId;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string ProductName;
      public int XMin;
      public int XMax;
      public int YMin;
      public int YMax;
      public int ZMin;
      public int ZMax;
      public int NumButtons;
      public int PeriodMin;
      public int PeriodMax;
      public int RMin;
      public int RMax;
      public int UMin;
      public int UMax;
      public int VMin;
      public int VMax;
      public WinMMJoystick.JoystCapsFlags Capabilities;
      public int MaxAxes;
      public int NumAxes;
      public int MaxButtons;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string RegKey;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      public string OemVxD;

      static JoyCaps()
      {
      }
    }

    private struct JoyInfoEx
    {
      public static readonly int SizeInBytes = Marshal.SizeOf((object) new WinMMJoystick.JoyInfoEx());
      public int Size;
      [MarshalAs(UnmanagedType.I4)]
      public WinMMJoystick.JoystickFlags Flags;
      public int XPos;
      public int YPos;
      public int ZPos;
      public int RPos;
      public int UPos;
      public int VPos;
      public uint Buttons;
      public uint ButtonNumber;
      public int Pov;
      private uint Reserved1;
      private uint Reserved2;

      static JoyInfoEx()
      {
      }
    }

    private static class UnsafeNativeMethods
    {
      [SuppressUnmanagedCodeSecurity]
      [DllImport("Winmm.dll")]
      public static WinMMJoystick.JoystickError joyGetDevCaps(int uJoyID, out WinMMJoystick.JoyCaps pjc, int cbjc);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("Winmm.dll")]
      public static uint joyGetPosEx(int uJoyID, ref WinMMJoystick.JoyInfoEx pji);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("Winmm.dll")]
      public static int joyGetNumDevs();
    }

    [Flags]
    private enum PovType
    {
      None = 0,
      Exists = 1,
      Discrete = 2,
      Continuous = 4,
    }

    private struct WinMMJoyDetails
    {
      public readonly float[] Min;
      public readonly float[] Max;
      public WinMMJoystick.PovType PovType;

      public WinMMJoyDetails(int num_axes)
      {
        this.Min = new float[num_axes];
        this.Max = new float[num_axes];
        this.PovType = WinMMJoystick.PovType.None;
      }

      public float CalculateOffset(float pos, int axis)
      {
        float num = (float) (2.0 * ((double) pos - (double) this.Min[axis]) / ((double) this.Max[axis] - (double) this.Min[axis]) - 1.0);
        if ((double) num > 1.0)
          return 1f;
        if ((double) num < -1.0)
          return -1f;
        if ((double) num < 1.0 / 1000.0 && (double) num > -1.0 / 1000.0)
          return 0.0f;
        else
          return num;
      }
    }
  }
}
