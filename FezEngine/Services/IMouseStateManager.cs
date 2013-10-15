// Type: FezEngine.Services.IMouseStateManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Input;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public interface IMouseStateManager
  {
    MouseButtonState LeftButton { get; }

    MouseButtonState MiddleButton { get; }

    MouseButtonState RightButton { get; }

    int WheelTurns { get; }

    FezButtonState WheelTurnedUp { get; }

    FezButtonState WheelTurnedDown { get; }

    Point Position { get; }

    Point Movement { get; }

    IntPtr RenderPanelHandle { set; }

    IntPtr ParentFormHandle { set; }

    void Update(GameTime time);
  }
}
