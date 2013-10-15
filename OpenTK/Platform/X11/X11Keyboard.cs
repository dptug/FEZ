// Type: OpenTK.Platform.X11.X11Keyboard
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using System;

namespace OpenTK.Platform.X11
{
  internal sealed class X11Keyboard : IKeyboardDriver2
  {
    private static readonly X11KeyMap keymap = new X11KeyMap();
    private static readonly string name = "Core X11 keyboard";
    private readonly byte[] keys = new byte[32];
    private KeyboardState state = new KeyboardState();
    private readonly int KeysymsPerKeycode;

    static X11Keyboard()
    {
    }

    public X11Keyboard()
    {
      this.state.IsConnected = true;
      IntPtr defaultDisplay = API.DefaultDisplay;
      using (new XLock(defaultDisplay))
      {
        int min_keycodes_return = 0;
        int max_keycodes_return = 0;
        API.DisplayKeycodes(defaultDisplay, ref min_keycodes_return, ref max_keycodes_return);
        Functions.XFree(API.GetKeyboardMapping(defaultDisplay, (byte) min_keycodes_return, max_keycodes_return - min_keycodes_return + 1, ref this.KeysymsPerKeycode));
        try
        {
          bool supported;
          Functions.XkbSetDetectableAutoRepeat(defaultDisplay, true, out supported);
        }
        catch
        {
        }
      }
    }

    public KeyboardState GetState()
    {
      this.ProcessEvents();
      return this.state;
    }

    public KeyboardState GetState(int index)
    {
      this.ProcessEvents();
      if (index == 0)
        return this.state;
      else
        return new KeyboardState();
    }

    public string GetDeviceName(int index)
    {
      if (index == 0)
        return X11Keyboard.name;
      else
        return string.Empty;
    }

    private void ProcessEvents()
    {
      IntPtr defaultDisplay = API.DefaultDisplay;
      using (new XLock(defaultDisplay))
      {
        Functions.XQueryKeymap(defaultDisplay, this.keys);
        for (int index1 = 8; index1 < 256; ++index1)
        {
          bool flag = ((int) this.keys[index1 >> 3] >> (index1 & 7) & 1) != 0;
          for (int index2 = 0; index2 < this.KeysymsPerKeycode; ++index2)
          {
            IntPtr num = Functions.XKeycodeToKeysym(defaultDisplay, (byte) index1, index2);
            Key key;
            if (num != IntPtr.Zero && X11Keyboard.keymap.TryGetValue((XKey) (int) num, out key))
            {
              if (flag)
              {
                this.state.EnableBit((int) key);
                break;
              }
              else
              {
                this.state.DisableBit((int) key);
                break;
              }
            }
          }
        }
      }
    }
  }
}
