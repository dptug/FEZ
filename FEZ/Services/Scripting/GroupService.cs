// Type: FezGame.Services.Scripting.GroupService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezGame.Services.Scripting
{
  internal class GroupService : IGroupService, IScriptingBase
  {
    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    public void MovePathToEnd(int id)
    {
      this.LevelManager.Groups[id].MoveToEnd = true;
    }

    public void StartPath(int id, bool backwards)
    {
      MovementPath path = this.LevelManager.Groups[id].Path;
      path.Backwards = backwards;
      path.NeedsTrigger = false;
      if (path.SaveTrigger && this.LevelManager.IsPathRecorded(id) || !path.SaveTrigger)
        return;
      this.LevelManager.RecordMoveToEnd(id);
    }

    public void RunPathOnce(int id, bool backwards)
    {
      MovementPath path = this.LevelManager.Groups[id].Path;
      path.Backwards = backwards;
      path.NeedsTrigger = false;
      path.RunOnce = true;
      if (path.SaveTrigger && this.LevelManager.IsPathRecorded(id) || !path.SaveTrigger)
        return;
      this.LevelManager.RecordMoveToEnd(id);
    }

    public void RunSingleSegment(int id, bool backwards)
    {
      this.LevelManager.Groups[id].Path.Backwards = backwards;
      this.LevelManager.Groups[id].Path.NeedsTrigger = false;
      this.LevelManager.Groups[id].Path.RunSingleSegment = true;
    }

    public void Stop(int id)
    {
      this.LevelManager.Groups[id].Path.NeedsTrigger = true;
    }

    public void SetEnabled(int id, bool enabled)
    {
      foreach (TrileInstance trileInstance in this.LevelManager.Groups[id].Triles)
        trileInstance.Enabled = enabled;
      this.LevelMaterializer.CullInstances();
    }

    public void GlitchyDespawn(int id, bool permanent)
    {
      foreach (TrileInstance instance in this.LevelManager.Groups[id].Triles)
      {
        if (permanent)
          this.GameState.SaveData.ThisLevel.DestroyedTriles.Add(instance.OriginalEmplacement);
        ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(ServiceHelper.Game, instance));
      }
    }

    public LongRunningAction Move(int id, float dX, float dY, float dZ)
    {
      TrileGroup group = this.LevelManager.Groups[id];
      group.Triles.Sort((IComparer<TrileInstance>) new MovingTrileInstanceComparer(new Vector3(dX, dY, dZ)));
      foreach (TrileInstance instance in group.Triles)
      {
        if (instance.PhysicsState == null)
          instance.PhysicsState = new InstancePhysicsState(instance);
      }
      List<ArtObjectInstance> attachedAos = new List<ArtObjectInstance>();
      foreach (ArtObjectInstance artObjectInstance in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
      {
        int? attachedGroup = artObjectInstance.ActorSettings.AttachedGroup;
        int num = id;
        if ((attachedGroup.GetValueOrDefault() != num ? 0 : (attachedGroup.HasValue ? 1 : 0)) != 0)
          attachedAos.Add(artObjectInstance);
      }
      Vector3 velocity = new Vector3(dX, dY, dZ);
      return new LongRunningAction((Func<float, float, bool>) ((elapsedSeconds, _) =>
      {
        foreach (TrileInstance item_2 in group.Triles)
        {
          item_2.PhysicsState.Velocity = velocity * elapsedSeconds;
          item_2.Position += velocity * elapsedSeconds;
          this.LevelManager.UpdateInstance(item_2);
        }
        foreach (ArtObjectInstance item_3 in attachedAos)
          item_3.Position += velocity * elapsedSeconds;
        return false;
      }));
    }

    public void ResetEvents()
    {
    }
  }
}
