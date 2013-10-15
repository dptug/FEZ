// Type: FezGame.Components.GameSequencer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  internal class GameSequencer : Sequencer
  {
    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    public GameSequencer(Game game)
      : base(game)
    {
    }

    protected override void OnDisappear(TrileInstance crystal)
    {
      if (this.PlayerManager.HeldInstance != crystal)
        return;
      this.PlayerManager.HeldInstance = (TrileInstance) null;
      this.PlayerManager.Action = ActionType.Idle;
    }
  }
}
