// Type: OpenTK.Platform.X11.X11WindowInfo
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform;
using System;

namespace OpenTK.Platform.X11
{
  internal sealed class X11WindowInfo : IWindowInfo, IDisposable
  {
    private IntPtr handle;
    private IntPtr rootWindow;
    private IntPtr display;
    private X11WindowInfo parent;
    private int screen;
    private XVisualInfo visualInfo;
    private EventMask eventMask;

    public IntPtr WindowHandle
    {
      get
      {
        return this.handle;
      }
      set
      {
        this.handle = value;
      }
    }

    public X11WindowInfo Parent
    {
      get
      {
        return this.parent;
      }
      set
      {
        this.parent = value;
      }
    }

    public IntPtr RootWindow
    {
      get
      {
        return this.rootWindow;
      }
      set
      {
        this.rootWindow = value;
      }
    }

    public IntPtr Display
    {
      get
      {
        return this.display;
      }
      set
      {
        this.display = value;
      }
    }

    public int Screen
    {
      get
      {
        return this.screen;
      }
      set
      {
        this.screen = value;
      }
    }

    public XVisualInfo VisualInfo
    {
      get
      {
        return this.visualInfo;
      }
      set
      {
        this.visualInfo = value;
      }
    }

    public EventMask EventMask
    {
      get
      {
        return this.eventMask;
      }
      set
      {
        this.eventMask = value;
      }
    }

    public X11WindowInfo()
    {
    }

    public X11WindowInfo(IntPtr handle, X11WindowInfo parent)
    {
      this.handle = handle;
      this.parent = parent;
      if (parent == null)
        return;
      this.rootWindow = parent.rootWindow;
      this.display = parent.display;
      this.screen = parent.screen;
      this.visualInfo = parent.visualInfo;
    }

    public void Dispose()
    {
    }

    public override string ToString()
    {
      return string.Format("X11.WindowInfo: Display {0}, Screen {1}, Handle {2}, Parent: ({3})", (object) this.Display, (object) this.Screen, (object) this.WindowHandle, this.Parent != null ? (object) this.Parent.ToString() : (object) "null");
    }

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      X11WindowInfo x11WindowInfo = (X11WindowInfo) obj;
      if (x11WindowInfo == null || !object.Equals((object) this.display, (object) x11WindowInfo.display))
        return false;
      else
        return this.handle.Equals((object) x11WindowInfo.handle);
    }

    public override int GetHashCode()
    {
      return this.handle.GetHashCode() ^ this.display.GetHashCode();
    }
  }
}
