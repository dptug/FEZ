// Type: FezEngine.Components.IInputManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Input;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Components
{
  public interface IInputManager
  {
    ControllerIndex ActiveControllers { get; }

    FezButtonState Up { get; }

    FezButtonState Down { get; }

    FezButtonState Left { get; }

    FezButtonState Right { get; }

    Vector2 Movement { get; }

    Vector2 FreeLook { get; }

    FezButtonState GrabThrow { get; }

    FezButtonState Jump { get; }

    FezButtonState CancelTalk { get; }

    FezButtonState Start { get; }

    FezButtonState Back { get; }

    FezButtonState OpenInventory { get; }

    FezButtonState RotateLeft { get; }

    FezButtonState RotateRight { get; }

    FezButtonState ClampLook { get; }

    FezButtonState FpsToggle { get; }

    FezButtonState MapZoomIn { get; }

    FezButtonState MapZoomOut { get; }

    FezButtonState ExactUp { get; }

    bool StrictRotation { get; set; }

    GamepadState ActiveGamepad { get; }

    event Action<PlayerIndex> ActiveControllerDisconnected;

    bool AnyButtonPressed();

    void ForceActiveController(ControllerIndex ci);

    void DetermineActiveController();

    void ClearActiveController();

    void SaveState();

    void RecoverState();

    void Reset();

    void PressedToDown();
  }
}
