// Type: Microsoft.Xna.Framework.Input.Mouse
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Input
{
  public static class Mouse
  {
    private static MouseDevice _mouse = (MouseDevice) null;
    internal static MouseState State;
    private static GameWindow Window;

    public static IntPtr WindowHandle
    {
      get
      {
        return IntPtr.Zero;
      }
    }

    static Mouse()
    {
    }

    internal static void setWindows(GameWindow window)
    {
      Mouse.Window = window;
      Mouse._mouse = window.Mouse;
    }

    public static MouseState GetState()
    {
      if (Mouse._mouse == null)
        return Mouse.State;
      Mouse.POINT lpPoint = new Mouse.POINT();
      Mouse.GetCursorPos(out lpPoint);
      Point point = Mouse.Window.PointToClient(lpPoint.ToPoint());
      Mouse.State.X = point.X;
      Mouse.State.Y = point.Y;
      Mouse.State.LeftButton = Mouse._mouse[MouseButton.Left] ? ButtonState.Pressed : ButtonState.Released;
      Mouse.State.RightButton = Mouse._mouse[MouseButton.Right] ? ButtonState.Pressed : ButtonState.Released;
      Mouse.State.MiddleButton = Mouse._mouse[MouseButton.Middle] ? ButtonState.Pressed : ButtonState.Released;
      Mouse.State.ScrollWheelValue = (int) ((double) Mouse._mouse.WheelPrecise * 120.0);
      return Mouse.State;
    }

    public static void SetPosition(int x, int y)
    {
      Mouse.UpdateStatePosition(x, y);
      Point point = Mouse.Window.PointToScreen(new Point(x, y));
      Mouse.SetCursorPos(point.X, point.Y);
    }

    private static void UpdateStatePosition(int x, int y)
    {
      Mouse.State.X = x;
      Mouse.State.Y = y;
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    public static bool GetCursorPos(out Mouse.POINT lpPoint);

    public struct POINT
    {
      public int X;
      public int Y;

      public Point ToPoint()
      {
        return new Point(this.X, this.Y);
      }
    }
  }
}
