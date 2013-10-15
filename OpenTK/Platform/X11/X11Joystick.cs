// Type: OpenTK.Platform.X11.X11Joystick
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  internal sealed class X11Joystick : IJoystickDriver
  {
    private static readonly string JoystickPath = "/dev/input/js";
    private static readonly string JoystickPathLegacy = "/dev/js";
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

    static X11Joystick()
    {
    }

    public X11Joystick()
    {
      this.sticks_readonly = (IList<JoystickDevice>) this.sticks.AsReadOnly();
      int num1 = 0;
      int num2 = 25;
      while (num1 < num2)
      {
        JoystickDevice joystickDevice = (JoystickDevice) this.OpenJoystick(X11Joystick.JoystickPath, num1++);
        if (joystickDevice != null)
        {
          joystickDevice.Description = string.Format("USB Joystick {0} ({1} axes, {2} buttons, {3}{0})", (object) num1, (object) joystickDevice.Axis.Count, (object) joystickDevice.Button.Count, (object) X11Joystick.JoystickPath);
          this.sticks.Add(joystickDevice);
        }
      }
      int num3 = 0;
      while (num3 < num2)
      {
        JoystickDevice joystickDevice = (JoystickDevice) this.OpenJoystick(X11Joystick.JoystickPathLegacy, num3++);
        if (joystickDevice != null)
        {
          joystickDevice.Description = string.Format("USB Joystick {0} ({1} axes, {2} buttons, {3}{0})", (object) num3, (object) joystickDevice.Axis.Count, (object) joystickDevice.Button.Count, (object) X11Joystick.JoystickPathLegacy);
          this.sticks.Add(joystickDevice);
        }
      }
    }

    ~X11Joystick()
    {
      this.Dispose(false);
    }

    public unsafe void Poll()
    {
      foreach (JoystickDevice joystickDevice in this.sticks)
      {
        X11Joystick.JoystickEvent joystickEvent;
        while ((long) X11Joystick.UnsafeNativeMethods.read(joystickDevice.Id, (void*) &joystickEvent, (UIntPtr) ((ulong) sizeof (X11Joystick.JoystickEvent))) > 0L)
        {
          joystickEvent.Type &= ~X11Joystick.JoystickEventType.Init;
          switch (joystickEvent.Type)
          {
            case X11Joystick.JoystickEventType.Button:
              joystickDevice.SetButton((JoystickButton) joystickEvent.Number, (int) joystickEvent.Value != 0);
              continue;
            case X11Joystick.JoystickEventType.Axis:
              if ((int) joystickEvent.Number % 2 == 0)
              {
                joystickDevice.SetAxis((JoystickAxis) joystickEvent.Number, (float) joystickEvent.Value / (float) short.MaxValue);
                continue;
              }
              else
              {
                joystickDevice.SetAxis((JoystickAxis) joystickEvent.Number, (float) -joystickEvent.Value / (float) short.MaxValue);
                continue;
              }
            default:
              continue;
          }
        }
      }
    }

    private JoystickDevice<X11JoyDetails> OpenJoystick(string base_path, int number)
    {
      string pathname = base_path + number.ToString();
      JoystickDevice<X11JoyDetails> joystickDevice = (JoystickDevice<X11JoyDetails>) null;
      int num = -1;
      try
      {
        num = X11Joystick.UnsafeNativeMethods.open(pathname, X11Joystick.OpenFlags.NonBlock);
        if (num == -1)
          return (JoystickDevice<X11JoyDetails>) null;
        int data1 = 2048;
        X11Joystick.UnsafeNativeMethods.ioctl(num, X11Joystick.JoystickIoctlCode.Version, ref data1);
        if (data1 < 65536)
          return (JoystickDevice<X11JoyDetails>) null;
        int data2 = 0;
        X11Joystick.UnsafeNativeMethods.ioctl(num, X11Joystick.JoystickIoctlCode.Axes, ref data2);
        int data3 = 0;
        X11Joystick.UnsafeNativeMethods.ioctl(num, X11Joystick.JoystickIoctlCode.Buttons, ref data3);
        joystickDevice = new JoystickDevice<X11JoyDetails>(num, data2, data3);
        return joystickDevice;
      }
      finally
      {
        if (joystickDevice == null && num != -1)
          X11Joystick.UnsafeNativeMethods.close(num);
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
      foreach (JoystickDevice joystickDevice in this.sticks)
        X11Joystick.UnsafeNativeMethods.close(joystickDevice.Id);
      this.disposed = true;
    }

    private struct JoystickEvent
    {
      public uint Time;
      public short Value;
      public X11Joystick.JoystickEventType Type;
      public byte Number;
    }

    [Flags]
    private enum JoystickEventType : byte
    {
      Button = (byte) 1,
      Axis = (byte) 2,
      Init = (byte) 128,
    }

    private enum JoystickIoctlCode : uint
    {
      Axes = 2147576337U,
      Buttons = 2147576338U,
      Version = 2147772929U,
    }

    [Flags]
    private enum OpenFlags
    {
      NonBlock = 2048,
    }

    private static class UnsafeNativeMethods
    {
      [DllImport("libc", SetLastError = true)]
      public static int ioctl(int d, X11Joystick.JoystickIoctlCode request, ref int data);

      [DllImport("libc", SetLastError = true)]
      public static int open([MarshalAs(UnmanagedType.LPStr)] string pathname, X11Joystick.OpenFlags flags);

      [DllImport("libc", SetLastError = true)]
      public static int close(int fd);

      [DllImport("libc", SetLastError = true)]
      public static IntPtr read(int fd, void* buffer, UIntPtr count);
    }
  }
}
