// Type: OpenTK.Platform.Windows.WMInput
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;

namespace OpenTK.Platform.Windows
{
  internal sealed class WMInput : IInputDriver2, IMouseDriver2, IKeyboardDriver2, IGamePadDriver
  {
    private readonly object MouseLock = new object();
    private readonly object KeyboardLock = new object();
    private readonly WinMMJoystick gamepad_driver = new WinMMJoystick();
    private KeyboardState keyboard = new KeyboardState();
    private MouseState mouse = new MouseState();
    private readonly WinKeyMap KeyMap = new WinKeyMap();

    public IKeyboardDriver2 KeyboardDriver
    {
      get
      {
        return (IKeyboardDriver2) this;
      }
    }

    public IMouseDriver2 MouseDriver
    {
      get
      {
        return (IMouseDriver2) this;
      }
    }

    public IGamePadDriver GamePadDriver
    {
      get
      {
        return (IGamePadDriver) this;
      }
    }

    private void UpdateKeyboard()
    {
      for (int index1 = 0; index1 < 256; ++index1)
      {
        VirtualKeys index2 = (VirtualKeys) index1;
        bool flag = (int) Functions.GetAsyncKeyState(index2) >> 8 != 0;
        if (this.KeyMap.ContainsKey(index2))
          this.keyboard[this.KeyMap[index2]] = flag;
      }
    }

    private void UpdateMouse()
    {
      POINT point = new POINT();
      Functions.GetCursorPos(ref point);
      this.mouse.X = point.X;
      this.mouse.Y = point.Y;
      this.mouse[MouseButton.Left] = (int) Functions.GetAsyncKeyState(VirtualKeys.LBUTTON) >> 8 != 0;
      this.mouse[MouseButton.Middle] = (int) Functions.GetAsyncKeyState(VirtualKeys.RBUTTON) >> 8 != 0;
      this.mouse[MouseButton.Right] = (int) Functions.GetAsyncKeyState(VirtualKeys.MBUTTON) >> 8 != 0;
      this.mouse[MouseButton.Button1] = (int) Functions.GetAsyncKeyState(VirtualKeys.XBUTTON1) >> 8 != 0;
      this.mouse[MouseButton.Button2] = (int) Functions.GetAsyncKeyState(VirtualKeys.XBUTTON2) >> 8 != 0;
    }

    public MouseState GetState()
    {
      lock (this.MouseLock)
      {
        this.UpdateMouse();
        return this.mouse;
      }
    }

    public MouseState GetState(int index)
    {
      lock (this.MouseLock)
      {
        this.UpdateMouse();
        if (index == 0)
          return this.mouse;
        else
          return new MouseState();
      }
    }

    public void SetPosition(double x, double y)
    {
      Functions.SetCursorPos((int) x, (int) y);
    }

    KeyboardState IKeyboardDriver2.GetState()
    {
      lock (this.KeyboardLock)
      {
        this.UpdateKeyboard();
        return this.keyboard;
      }
    }

    KeyboardState IKeyboardDriver2.GetState(int index)
    {
      lock (this.KeyboardLock)
      {
        this.UpdateKeyboard();
        if (index == 0)
          return this.keyboard;
        else
          return new KeyboardState();
      }
    }

    string IKeyboardDriver2.GetDeviceName(int index)
    {
      return "Default Windows Keyboard";
    }
  }
}
