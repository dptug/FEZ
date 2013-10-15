// Type: Microsoft.Xna.Framework.Input.Input
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using Tao.Sdl;

namespace Microsoft.Xna.Framework.Input
{
  public class Input
  {
    public int ID;
    public InputType Type;

    public bool Negative { get; set; }

    internal bool ReadBool(IntPtr device, short DeadZone)
    {
      switch (this.Type)
      {
        case InputType.Button:
          return (int) Sdl.SDL_JoystickGetButton(device, this.ID) > 0 ^ this.Negative;
        case InputType.Axis:
          short axis = Sdl.SDL_JoystickGetAxis(device, this.ID);
          if (this.Negative)
            return (int) axis < (int) -DeadZone;
          else
            return (int) axis > (int) DeadZone;
        case InputType.PovUp:
        case InputType.PovRight:
        case InputType.PovDown:
        case InputType.PovLeft:
          return ((InputType) Sdl.SDL_JoystickGetHat(device, this.ID) & this.Type) > (InputType) 0 ^ this.Negative;
        default:
          return false;
      }
    }

    internal float ReadFloat(IntPtr device)
    {
      float num1 = this.Negative ? -1f : 1f;
      switch (this.Type)
      {
        case InputType.Button:
          return (float) Sdl.SDL_JoystickGetButton(device, this.ID) * num1;
        case InputType.Axis:
          float num2 = this.Negative ? (float) short.MinValue : (float) short.MaxValue;
          return (float) Sdl.SDL_JoystickGetAxis(device, this.ID) / num2;
        case InputType.PovUp:
        case InputType.PovRight:
        case InputType.PovDown:
        case InputType.PovLeft:
          return (float) ((InputType) Sdl.SDL_JoystickGetHat(device, this.ID) & this.Type) * num1;
        default:
          return 0.0f;
      }
    }
  }
}
