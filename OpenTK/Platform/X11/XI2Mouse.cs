// Type: OpenTK.Platform.X11.XI2Mouse
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  internal sealed class XI2Mouse : IMouseDriver2
  {
    private static readonly Functions.EventPredicate PredicateImpl = new Functions.EventPredicate(XI2Mouse.IsEventValid);
    private List<MouseState> mice = new List<MouseState>();
    private Dictionary<int, int> rawids = new Dictionary<int, int>();
    private readonly IntPtr Predicate = Marshal.GetFunctionPointerForDelegate((Delegate) XI2Mouse.PredicateImpl);
    internal readonly X11WindowInfo window;
    private static int XIOpCode;
    private XI2Mouse.MouseWarp? mouse_warp_event;
    private int mouse_warp_event_count;

    static XI2Mouse()
    {
    }

    public XI2Mouse()
    {
      using (new XLock(API.DefaultDisplay))
      {
        this.window = new X11WindowInfo();
        this.window.Display = API.DefaultDisplay;
        this.window.Screen = Functions.XDefaultScreen(this.window.Display);
        this.window.RootWindow = Functions.XRootWindow(this.window.Display, this.window.Screen);
        this.window.WindowHandle = this.window.RootWindow;
      }
      if (!XI2Mouse.IsSupported(this.window.Display))
        throw new NotSupportedException("XInput2 not supported.");
      using (XIEventMask mask = new XIEventMask(1, (XIEventMasks) 229376))
      {
        Functions.XISelectEvents(this.window.Display, this.window.WindowHandle, mask);
        Functions.XISelectEvents(this.window.Display, this.window.RootWindow, mask);
      }
    }

    internal static bool IsSupported(IntPtr display)
    {
      if (display == IntPtr.Zero)
        display = API.DefaultDisplay;
      using (new XLock(display))
      {
        int major;
        int first_event;
        int first_error;
        if (Functions.XQueryExtension(display, "XInputExtension", out major, out first_event, out first_error) == 0)
          return false;
        XI2Mouse.XIOpCode = major;
      }
      return true;
    }

    public MouseState GetState()
    {
      this.ProcessEvents();
      MouseState mouseState = new MouseState();
      foreach (MouseState other in this.mice)
        mouseState.MergeBits(other);
      return mouseState;
    }

    public MouseState GetState(int index)
    {
      this.ProcessEvents();
      if (this.mice.Count > index)
        return this.mice[index];
      else
        return new MouseState();
    }

    public void SetPosition(double x, double y)
    {
      using (new XLock(this.window.Display))
      {
        int num = (int) Functions.XWarpPointer(this.window.Display, IntPtr.Zero, this.window.RootWindow, 0, 0, 0U, 0U, (int) x, (int) y);
        if (!this.mouse_warp_event.HasValue)
          this.mouse_warp_event_count = 0;
        ++this.mouse_warp_event_count;
        this.mouse_warp_event = new XI2Mouse.MouseWarp?(new XI2Mouse.MouseWarp((double) (int) x, (double) (int) y));
      }
      this.ProcessEvents();
    }

    private bool CheckMouseWarp(double x, double y)
    {
      bool flag = this.mouse_warp_event.HasValue && this.mouse_warp_event.Value.Equals(new XI2Mouse.MouseWarp((double) (int) x, (double) (int) y));
      if (flag && --this.mouse_warp_event_count <= 0)
        this.mouse_warp_event = new XI2Mouse.MouseWarp?();
      return flag;
    }

    private void ProcessEvents()
    {
      while (true)
      {
        XEvent e = new XEvent();
        using (new XLock(this.window.Display))
        {
          if (!Functions.XCheckIfEvent(this.window.Display, ref e, this.Predicate, new IntPtr(XI2Mouse.XIOpCode)))
            break;
          XGenericEventCookie cookie = e.GenericEventCookie;
          if (Functions.XGetEventData(this.window.Display, ref cookie) != 0)
          {
            XIRawEvent xiRawEvent = (XIRawEvent) Marshal.PtrToStructure(cookie.data, typeof (XIRawEvent));
            if (!this.rawids.ContainsKey(xiRawEvent.deviceid))
            {
              this.mice.Add(new MouseState());
              this.rawids.Add(xiRawEvent.deviceid, this.mice.Count - 1);
            }
            MouseState mouseState = this.mice[this.rawids[xiRawEvent.deviceid]];
            switch (xiRawEvent.evtype)
            {
              case XIEventType.RawButtonPress:
                switch (xiRawEvent.detail)
                {
                  case 1:
                    mouseState.EnableBit(0);
                    break;
                  case 2:
                    mouseState.EnableBit(1);
                    break;
                  case 3:
                    mouseState.EnableBit(2);
                    break;
                  case 4:
                    ++mouseState.WheelPrecise;
                    break;
                  case 5:
                    --mouseState.WheelPrecise;
                    break;
                  case 6:
                    mouseState.EnableBit(3);
                    break;
                  case 7:
                    mouseState.EnableBit(4);
                    break;
                  case 8:
                    mouseState.EnableBit(5);
                    break;
                  case 9:
                    mouseState.EnableBit(6);
                    break;
                  case 10:
                    mouseState.EnableBit(7);
                    break;
                  case 11:
                    mouseState.EnableBit(8);
                    break;
                  case 12:
                    mouseState.EnableBit(9);
                    break;
                  case 13:
                    mouseState.EnableBit(10);
                    break;
                  case 14:
                    mouseState.EnableBit(11);
                    break;
                }
              case XIEventType.RawButtonRelease:
                switch (xiRawEvent.detail)
                {
                  case 1:
                    mouseState.DisableBit(0);
                    break;
                  case 2:
                    mouseState.DisableBit(1);
                    break;
                  case 3:
                    mouseState.DisableBit(2);
                    break;
                  case 6:
                    mouseState.DisableBit(3);
                    break;
                  case 7:
                    mouseState.DisableBit(4);
                    break;
                  case 8:
                    mouseState.DisableBit(5);
                    break;
                  case 9:
                    mouseState.DisableBit(6);
                    break;
                  case 10:
                    mouseState.DisableBit(7);
                    break;
                  case 11:
                    mouseState.DisableBit(8);
                    break;
                  case 12:
                    mouseState.DisableBit(9);
                    break;
                  case 13:
                    mouseState.DisableBit(10);
                    break;
                  case 14:
                    mouseState.DisableBit(11);
                    break;
                }
              case XIEventType.RawMotion:
                double x = 0.0;
                double y = 0.0;
                if (XI2Mouse.IsBitSet(xiRawEvent.valuators.mask, 0))
                  x = BitConverter.Int64BitsToDouble(Marshal.ReadInt64(xiRawEvent.raw_values, 0));
                if (XI2Mouse.IsBitSet(xiRawEvent.valuators.mask, 1))
                  y = BitConverter.Int64BitsToDouble(Marshal.ReadInt64(xiRawEvent.raw_values, 8));
                if (!this.CheckMouseWarp(x, y))
                {
                  mouseState.X += (int) x;
                  mouseState.Y += (int) y;
                  break;
                }
                else
                  break;
            }
            this.mice[this.rawids[xiRawEvent.deviceid]] = mouseState;
          }
          Functions.XFreeEventData(this.window.Display, ref cookie);
        }
      }
    }

    private static bool IsEventValid(IntPtr display, ref XEvent e, IntPtr arg)
    {
      if (e.GenericEventCookie.extension != arg.ToInt32())
        return false;
      if (e.GenericEventCookie.evtype != 17 && e.GenericEventCookie.evtype != 15)
        return e.GenericEventCookie.evtype == 16;
      else
        return true;
    }

    private static unsafe bool IsBitSet(IntPtr mask, int bit)
    {
      return ((int) *(byte*) ((IntPtr) (void*) mask + (bit >> 3)) & 1 << (bit & 7)) != 0;
    }

    private struct MouseWarp : IEquatable<XI2Mouse.MouseWarp>
    {
      private double X;
      private double Y;

      public MouseWarp(double x, double y)
      {
        this.X = x;
        this.Y = y;
      }

      public bool Equals(XI2Mouse.MouseWarp warp)
      {
        if (this.X == warp.X)
          return this.Y == warp.Y;
        else
          return false;
      }
    }
  }
}
