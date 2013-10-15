// Type: FezGame.Components.GameNpcHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  internal class GameNpcHost : NpcHost
  {
    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public GameNpcHost(Game game)
      : base(game)
    {
    }

    protected override NpcState CreateNpcState(NpcInstance npc)
    {
      if (npc.ActorType == ActorType.Owl && this.GameState.SaveData.ThisLevel.InactiveNPCs.Contains(npc.Id))
        return (NpcState) null;
      else
        return (NpcState) new GameNpcState(this.Game, npc);
    }
  }
}
