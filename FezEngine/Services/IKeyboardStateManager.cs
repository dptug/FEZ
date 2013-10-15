// Type: FezEngine.Services.IKeyboardStateManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FezEngine.Services
{
  public interface IKeyboardStateManager
  {
    bool IgnoreMapping { get; set; }

    FezButtonState Jump { get; }

    FezButtonState GrabThrow { get; }

    FezButtonState CancelTalk { get; }

    FezButtonState Up { get; }

    FezButtonState Down { get; }

    FezButtonState Left { get; }

    FezButtonState Right { get; }

    FezButtonState LookUp { get; }

    FezButtonState LookDown { get; }

    FezButtonState LookRight { get; }

    FezButtonState LookLeft { get; }

    FezButtonState OpenMap { get; }

    FezButtonState MapZoomIn { get; }

    FezButtonState MapZoomOut { get; }

    FezButtonState Pause { get; }

    FezButtonState OpenInventory { get; }

    FezButtonState RotateLeft { get; }

    FezButtonState RotateRight { get; }

    FezButtonState FpViewToggle { get; }

    FezButtonState ClampLook { get; }

    FezButtonState GetKeyState(Keys key);

    void RegisterKey(Keys key);

    void UpdateMapping();

    void Update(KeyboardState state, GameTime time);
  }
}
