// Type: FezEngine.Services.IGamepadsManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Input;
using Microsoft.Xna.Framework;

namespace FezEngine.Services
{
  public interface IGamepadsManager
  {
    GamepadState this[PlayerIndex index] { get; }
  }
}
