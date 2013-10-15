// Type: OpenTK.Platform.X11.X11Input
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  internal sealed class X11Input : IInputDriver, IKeyboardDriver, IMouseDriver, IJoystickDriver, IDisposable
  {
    private X11Joystick joystick_driver = new X11Joystick();
    private KeyboardDevice keyboard = new KeyboardDevice();
    private MouseDevice mouse = new MouseDevice();
    private List<KeyboardDevice> dummy_keyboard_list = new List<KeyboardDevice>(1);
    private List<MouseDevice> dummy_mice_list = new List<MouseDevice>(1);
    private X11KeyMap keymap = new X11KeyMap();
    private int firstKeyCode;
    private int lastKeyCode;
    private int keysyms_per_keycode;
    private IntPtr[] keysyms;

    public IList<KeyboardDevice> Keyboard
    {
      get
      {
        return (IList<KeyboardDevice>) this.dummy_keyboard_list;
      }
    }

    public IList<MouseDevice> Mouse
    {
      get
      {
        return (IList<MouseDevice>) this.dummy_mice_list;
      }
    }

    public IList<JoystickDevice> Joysticks
    {
      get
      {
        return this.joystick_driver.Joysticks;
      }
    }

    public X11Input(IWindowInfo attach)
    {
      if (attach == null)
        throw new ArgumentException("A valid parent window must be defined, in order to create an X11Input driver.");
      X11WindowInfo x11WindowInfo = (X11WindowInfo) attach;
      this.mouse.Description = "Default X11 mouse";
      this.mouse.DeviceID = IntPtr.Zero;
      this.mouse.NumberOfButtons = 5;
      this.mouse.NumberOfWheels = 1;
      this.dummy_mice_list.Add(this.mouse);
      using (new XLock(x11WindowInfo.Display))
      {
        API.DisplayKeycodes(x11WindowInfo.Display, ref this.firstKeyCode, ref this.lastKeyCode);
        IntPtr keyboardMapping = API.GetKeyboardMapping(x11WindowInfo.Display, (byte) this.firstKeyCode, this.lastKeyCode - this.firstKeyCode + 1, ref this.keysyms_per_keycode);
        this.keysyms = new IntPtr[(this.lastKeyCode - this.firstKeyCode + 1) * this.keysyms_per_keycode];
        Marshal.PtrToStructure(keyboardMapping, (object) this.keysyms);
        API.Free(keyboardMapping);
        KeyboardDevice keyboardDevice = new KeyboardDevice();
        this.keyboard.Description = "Default X11 keyboard";
        this.keyboard.NumberOfKeys = this.lastKeyCode - this.firstKeyCode + 1;
        this.keyboard.DeviceID = IntPtr.Zero;
        this.dummy_keyboard_list.Add(this.keyboard);
        bool supported;
        Functions.XkbSetDetectableAutoRepeat(x11WindowInfo.Display, true, out supported);
      }
    }

    internal void ProcessEvent(ref XEvent e)
    {
      switch (e.type)
      {
        case XEventName.KeyPress:
        case XEventName.KeyRelease:
          bool flag = e.type == XEventName.KeyPress;
          IntPtr num1 = API.LookupKeysym(ref e.KeyEvent, 0);
          IntPtr num2 = API.LookupKeysym(ref e.KeyEvent, 1);
          if (this.keymap.ContainsKey((XKey) (int) num1))
          {
            this.keyboard[this.keymap[(XKey) (int) num1]] = flag;
            break;
          }
          else
          {
            if (!this.keymap.ContainsKey((XKey) (int) num2))
              break;
            this.keyboard[this.keymap[(XKey) (int) num2]] = flag;
            break;
          }
        case XEventName.ButtonPress:
          if (e.ButtonEvent.button == 1)
          {
            this.mouse[MouseButton.Left] = true;
            break;
          }
          else if (e.ButtonEvent.button == 2)
          {
            this.mouse[MouseButton.Middle] = true;
            break;
          }
          else if (e.ButtonEvent.button == 3)
          {
            this.mouse[MouseButton.Right] = true;
            break;
          }
          else if (e.ButtonEvent.button == 4)
          {
            ++this.mouse.Wheel;
            break;
          }
          else if (e.ButtonEvent.button == 5)
          {
            --this.mouse.Wheel;
            break;
          }
          else if (e.ButtonEvent.button == 6)
          {
            this.mouse[MouseButton.Button1] = true;
            break;
          }
          else if (e.ButtonEvent.button == 7)
          {
            this.mouse[MouseButton.Button2] = true;
            break;
          }
          else if (e.ButtonEvent.button == 8)
          {
            this.mouse[MouseButton.Button3] = true;
            break;
          }
          else if (e.ButtonEvent.button == 9)
          {
            this.mouse[MouseButton.Button4] = true;
            break;
          }
          else if (e.ButtonEvent.button == 10)
          {
            this.mouse[MouseButton.Button5] = true;
            break;
          }
          else if (e.ButtonEvent.button == 11)
          {
            this.mouse[MouseButton.Button6] = true;
            break;
          }
          else if (e.ButtonEvent.button == 12)
          {
            this.mouse[MouseButton.Button7] = true;
            break;
          }
          else if (e.ButtonEvent.button == 13)
          {
            this.mouse[MouseButton.Button8] = true;
            break;
          }
          else
          {
            if (e.ButtonEvent.button != 14)
              break;
            this.mouse[MouseButton.Button9] = true;
            break;
          }
        case XEventName.ButtonRelease:
          if (e.ButtonEvent.button == 1)
          {
            this.mouse[MouseButton.Left] = false;
            break;
          }
          else if (e.ButtonEvent.button == 2)
          {
            this.mouse[MouseButton.Middle] = false;
            break;
          }
          else if (e.ButtonEvent.button == 3)
          {
            this.mouse[MouseButton.Right] = false;
            break;
          }
          else if (e.ButtonEvent.button == 6)
          {
            this.mouse[MouseButton.Button1] = false;
            break;
          }
          else if (e.ButtonEvent.button == 7)
          {
            this.mouse[MouseButton.Button2] = false;
            break;
          }
          else if (e.ButtonEvent.button == 8)
          {
            this.mouse[MouseButton.Button3] = false;
            break;
          }
          else if (e.ButtonEvent.button == 9)
          {
            this.mouse[MouseButton.Button4] = false;
            break;
          }
          else if (e.ButtonEvent.button == 10)
          {
            this.mouse[MouseButton.Button5] = false;
            break;
          }
          else if (e.ButtonEvent.button == 11)
          {
            this.mouse[MouseButton.Button6] = false;
            break;
          }
          else if (e.ButtonEvent.button == 12)
          {
            this.mouse[MouseButton.Button7] = false;
            break;
          }
          else if (e.ButtonEvent.button == 13)
          {
            this.mouse[MouseButton.Button8] = false;
            break;
          }
          else
          {
            if (e.ButtonEvent.button != 14)
              break;
            this.mouse[MouseButton.Button9] = false;
            break;
          }
        case XEventName.MotionNotify:
          this.mouse.Position = new Point(e.MotionEvent.x, e.MotionEvent.y);
          break;
      }
    }

    public void Poll()
    {
      this.joystick_driver.Poll();
    }

    public void Dispose()
    {
    }
  }
}
