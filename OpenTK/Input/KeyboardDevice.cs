// Type: OpenTK.Input.KeyboardDevice
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Input
{
  public sealed class KeyboardDevice : IInputDevice
  {
    private bool[] keys = new bool[Enum.GetValues(typeof (Key)).Length];
    private KeyboardKeyEventArgs args = new KeyboardKeyEventArgs();
    private string description;
    private int numKeys;
    private int numFKeys;
    private int numLeds;
    private IntPtr devID;
    private bool repeat;

    public bool this[Key key]
    {
      get
      {
        return this.keys[(int) key];
      }
      internal set
      {
        if (this.keys[(int) key] == value && !this.KeyRepeat)
          return;
        this.keys[(int) key] = value;
        if (value && this.KeyDown != null)
        {
          this.args.Key = key;
          this.KeyDown((object) this, this.args);
        }
        else
        {
          if (value || this.KeyUp == null)
            return;
          this.args.Key = key;
          this.KeyUp((object) this, this.args);
        }
      }
    }

    public int NumberOfKeys
    {
      get
      {
        return this.numKeys;
      }
      internal set
      {
        this.numKeys = value;
      }
    }

    public int NumberOfFunctionKeys
    {
      get
      {
        return this.numFKeys;
      }
      internal set
      {
        this.numFKeys = value;
      }
    }

    public int NumberOfLeds
    {
      get
      {
        return this.numLeds;
      }
      internal set
      {
        this.numLeds = value;
      }
    }

    public IntPtr DeviceID
    {
      get
      {
        return this.devID;
      }
      internal set
      {
        this.devID = value;
      }
    }

    public bool KeyRepeat
    {
      get
      {
        return this.repeat;
      }
      set
      {
        this.repeat = value;
      }
    }

    public string Description
    {
      get
      {
        return this.description;
      }
      internal set
      {
        this.description = value;
      }
    }

    public InputDeviceType DeviceType
    {
      get
      {
        return InputDeviceType.Keyboard;
      }
    }

    public event EventHandler<KeyboardKeyEventArgs> KeyDown;

    public event EventHandler<KeyboardKeyEventArgs> KeyUp;

    internal KeyboardDevice()
    {
    }

    public override int GetHashCode()
    {
      return this.numKeys ^ this.numFKeys ^ this.numLeds ^ this.devID.GetHashCode() ^ this.description.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("ID: {0} ({1}). Keys: {2}, Function keys: {3}, Leds: {4}", (object) this.DeviceID, (object) this.Description, (object) this.NumberOfKeys, (object) this.NumberOfFunctionKeys, (object) this.NumberOfLeds);
    }

    internal void ClearKeys()
    {
      for (int index = 0; index < this.keys.Length; ++index)
      {
        if (this[(Key) index])
          this[(Key) index] = false;
      }
    }
  }
}
