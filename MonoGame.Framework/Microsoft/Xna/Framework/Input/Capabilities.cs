// Type: Microsoft.Xna.Framework.Input.Capabilities
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using Tao.Sdl;

namespace Microsoft.Xna.Framework.Input
{
  public class Capabilities
  {
    public int NumberOfAxis { get; private set; }

    public int NumberOfButtons { get; private set; }

    public int NumberOfPovHats { get; private set; }

    public Capabilities(IntPtr device)
    {
      this.NumberOfAxis = Sdl.SDL_JoystickNumAxes(device);
      this.NumberOfButtons = Sdl.SDL_JoystickNumButtons(device);
      this.NumberOfPovHats = Sdl.SDL_JoystickNumHats(device);
    }
  }
}
