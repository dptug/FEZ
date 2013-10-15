// Type: Microsoft.Xna.Framework.Input.Joystick
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;
using Tao.Sdl;

namespace Microsoft.Xna.Framework.Input
{
  public class Joystick
  {
    private int id;
    private IntPtr device;

    public bool Open { get; private set; }

    public string Name { get; private set; }

    public PadConfig Config { get; private set; }

    public Capabilities Details { get; private set; }

    public int ID
    {
      get
      {
        return this.id;
      }
    }

    public Joystick(int id)
    {
      this.id = id;
      this.device = Sdl.SDL_JoystickOpen(id);
      this.Open = true;
      this.Name = Sdl.SDL_JoystickName(id);
      this.Details = new Capabilities(this.device);
      this.Config = new PadConfig(this.Name, id);
      this.SetDefaults(this.Details);
    }

    private void SetDefaults(Capabilities capabilities)
    {
      if (capabilities == null)
        return;
      if (capabilities.NumberOfAxis > 1)
      {
        this.Config.LeftStick.X.AssignAxis(0, false);
        this.Config.LeftStick.Y.AssignAxis(1, false);
      }
      if (capabilities.NumberOfPovHats > 0)
        this.Config.Dpad.AssignPovHat(0);
      if (capabilities.NumberOfButtons <= 0)
        return;
      for (int index = 0; index < capabilities.NumberOfButtons; ++index)
      {
        Input input = this.Config[index];
        if (input != null)
        {
          input.ID = index;
          input.Negative = false;
          input.Type = InputType.Button;
        }
      }
    }

    public static bool Init()
    {
      return Sdl.SDL_Init(512) == 0;
    }

    public static List<Joystick> GrabJoysticks()
    {
      int num = Sdl.SDL_NumJoysticks();
      List<Joystick> list = new List<Joystick>();
      Sdl.SDL_JoystickEventState(0);
      for (int id = 0; id < num; ++id)
        list.Add(new Joystick(id));
      return list;
    }

    internal void Cleanup()
    {
    }
  }
}
