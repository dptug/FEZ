// Type: FezEngine.Components.NpcHost
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezEngine.Components
{
  public class NpcHost : GameComponent
  {
    protected readonly List<NpcState> NpcStates = new List<NpcState>();

    [ServiceDependency]
    public ILevelManager LevelManager { protected get; set; }

    protected NpcHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.LoadCharacters);
      this.LoadCharacters();
    }

    private void LoadCharacters()
    {
      foreach (NpcState component in this.NpcStates)
        ServiceHelper.RemoveComponent<NpcState>(component);
      this.NpcStates.Clear();
      foreach (NpcInstance npc in (IEnumerable<NpcInstance>) this.LevelManager.NonPlayerCharacters.Values)
      {
        NpcState npcState = this.CreateNpcState(npc);
        if (npcState != null)
        {
          ServiceHelper.AddComponent((IGameComponent) npcState);
          npcState.Initialize();
          this.NpcStates.Add(npcState);
        }
      }
    }

    protected virtual NpcState CreateNpcState(NpcInstance npc)
    {
      return new NpcState(this.Game, npc);
    }
  }
}
