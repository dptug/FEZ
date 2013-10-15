// Type: FezGame.Components.IdleRestarter
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  internal class IdleRestarter : GameComponent
  {
    private const float Timeout = 1f;
    private float counter;

    [ServiceDependency]
    public ISpeechBubbleManager SpeechBubble { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    public IdleRestarter(Game game)
      : base(game)
    {
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (this.InputManager.AnyButtonPressed() || this.InputManager.Down != FezButtonState.Up || (this.InputManager.Up != FezButtonState.Up || this.InputManager.Left != FezButtonState.Up) || this.InputManager.Right != FezButtonState.Up)
      {
        this.counter = 0.0f;
      }
      else
      {
        if (Intro.Instance != null || this.GameState.InCutscene || !this.PlayerManager.CanControl && this.SpeechBubble.Hidden && (!this.GameState.InMenuCube && !this.GameState.InMap))
          return;
        this.counter += (float) gameTime.ElapsedGameTime.TotalMinutes;
        if ((double) this.counter < 1.0)
          return;
        this.GameState.Restart();
        this.counter = 0.0f;
      }
    }
  }
}
