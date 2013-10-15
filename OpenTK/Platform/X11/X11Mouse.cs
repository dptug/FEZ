// Type: OpenTK.Platform.X11.X11Mouse
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using System;

namespace OpenTK.Platform.X11
{
  internal sealed class X11Mouse : IMouseDriver2
  {
    private MouseState mouse = new MouseState();
    private readonly IntPtr display;
    private readonly IntPtr root_window;
    private bool mouse_detached;
    private int mouse_detached_x;
    private int mouse_detached_y;

    public X11Mouse()
    {
      this.mouse.IsConnected = true;
      this.display = API.DefaultDisplay;
      this.root_window = Functions.XRootWindow(this.display, Functions.XDefaultScreen(this.display));
    }

    public MouseState GetState()
    {
      this.ProcessEvents();
      return this.mouse;
    }

    public MouseState GetState(int index)
    {
      this.ProcessEvents();
      if (index == 0)
        return this.mouse;
      else
        return new MouseState();
    }

    public void SetPosition(double x, double y)
    {
      this.ProcessEvents();
      using (new XLock(this.display))
      {
        this.mouse_detached = true;
        this.mouse_detached_x = (int) x;
        this.mouse_detached_y = (int) y;
        int num = (int) Functions.XWarpPointer(this.display, IntPtr.Zero, this.root_window, 0, 0, 0U, 0U, (int) x, (int) y);
      }
    }

    private void WriteBit(MouseButton offset, int enabled)
    {
      if (enabled != 0)
        this.mouse.EnableBit((int) offset);
      else
        this.mouse.DisableBit((int) offset);
    }

    private void ProcessEvents()
    {
      using (new XLock(this.display))
      {
        IntPtr root;
        IntPtr child;
        int root_x;
        int root_y;
        int win_x;
        int win_y;
        int keys_buttons;
        Functions.XQueryPointer(this.display, this.root_window, out root, out child, out root_x, out root_y, out win_x, out win_y, out keys_buttons);
        if (!this.mouse_detached)
        {
          this.mouse.X = root_x;
          this.mouse.Y = root_y;
        }
        else
        {
          this.mouse.X += root_x - this.mouse_detached_x;
          this.mouse.Y += root_y - this.mouse_detached_y;
          this.mouse_detached_x = root_x;
          this.mouse_detached_y = root_y;
        }
        this.WriteBit(MouseButton.Left, keys_buttons & 256);
        this.WriteBit(MouseButton.Middle, keys_buttons & 512);
        this.WriteBit(MouseButton.Right, keys_buttons & 1024);
        this.WriteBit(MouseButton.Button1, keys_buttons & 8192);
        this.WriteBit(MouseButton.Button2, keys_buttons & 16384);
        this.WriteBit(MouseButton.Button3, keys_buttons & 32768);
      }
    }
  }
}
