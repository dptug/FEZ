// Type: FezGame.Components.HeavyGroupsHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class HeavyGroupsHost : GameComponent
  {
    private readonly List<HeavyGroupState> trackedGroups = new List<HeavyGroupState>();

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    public HeavyGroupsHost(Game game)
      : base(game)
    {
      this.UpdateOrder = -2;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.Enabled = false;
      this.LevelManager.LevelChanging += new Action(this.TrackNewGroups);
      this.TrackNewGroups();
    }

    private void TrackNewGroups()
    {
      this.trackedGroups.Clear();
      foreach (TrileGroup group in Enumerable.Where<TrileGroup>((IEnumerable<TrileGroup>) this.LevelManager.Groups.Values, (Func<TrileGroup, bool>) (x => x.Heavy)))
        this.trackedGroups.Add(new HeavyGroupState(group));
      this.Enabled = this.trackedGroups.Count > 0;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Paused || this.EngineState.InMap || (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || !this.CameraManager.ActionRunning))
        return;
      foreach (HeavyGroupState heavyGroupState in this.trackedGroups)
        heavyGroupState.Update(gameTime.ElapsedGameTime);
    }
  }
}
