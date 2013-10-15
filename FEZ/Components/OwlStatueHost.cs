// Type: FezGame.Components.OwlStatueHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FezGame.Components
{
  internal class OwlStatueHost : GameComponent
  {
    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IOwlService OwlService { get; set; }

    public OwlStatueHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.Enabled = this.LevelManager.Name == "OWL";
      if (!this.Enabled)
        return;
      int num1;
      try
      {
        num1 = int.Parse(this.GameState.SaveData.ThisLevel.ScriptingState);
      }
      catch (Exception ex)
      {
        num1 = 0;
      }
      int num2 = this.GameState.SaveData.CollectedOwls;
      int num3 = 0;
      foreach (NpcInstance npcInstance in (IEnumerable<NpcInstance>) this.LevelManager.NonPlayerCharacters.Values)
      {
        if (npcInstance.ActorType == ActorType.Owl)
        {
          if (num2 <= num3)
          {
            ServiceHelper.RemoveComponent<NpcState>(npcInstance.State);
          }
          else
          {
            (npcInstance.State as GameNpcState).ForceVisible = true;
            (npcInstance.State as GameNpcState).IsNightForOwl = num3 < num1;
          }
          ++num3;
        }
      }
      if (num1 == 4 && this.GameState.SaveData.ThisLevel.FilledConditions.SecretCount == 0)
      {
        Waiters.Wait((Func<bool>) (() =>
        {
          if (!this.GameState.Loading)
            return !this.GameState.FarawaySettings.InTransition;
          else
            return false;
        }), (Action) (() => this.OwlService.OnOwlLanded()));
        this.Enabled = false;
      }
      this.GameState.SaveData.ThisLevel.ScriptingState = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || this.GameState.InMap)
        return;
      foreach (NpcInstance npcInstance in (IEnumerable<NpcInstance>) this.LevelManager.NonPlayerCharacters.Values)
      {
        if (npcInstance.State.CurrentAction == NpcAction.Land)
        {
          this.OwlService.OnOwlLanded();
          this.Enabled = false;
          break;
        }
      }
    }
  }
}
