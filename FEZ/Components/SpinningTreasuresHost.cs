// Type: FezGame.Components.SpinningTreasuresHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  public class SpinningTreasuresHost : GameComponent
  {
    private readonly List<TrileInstance> TrackedTreasures = new List<TrileInstance>();
    private TimeSpan SinceCreated;

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    public SpinningTreasuresHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.LevelManager.TrileRestored += (Action<TrileInstance>) (t =>
      {
        if (!t.Enabled || !ActorTypeExtensions.IsTreasure(t.Trile.ActorSettings.Type) && t.Trile.ActorSettings.Type != ActorType.GoldenCube)
          return;
        this.TrackedTreasures.Add(t);
      });
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.TrackedTreasures.Clear();
      foreach (TrileInstance trileInstance in (IEnumerable<TrileInstance>) this.LevelManager.Triles.Values)
      {
        if (trileInstance.Enabled && (ActorTypeExtensions.IsTreasure(trileInstance.Trile.ActorSettings.Type) || trileInstance.Trile.ActorSettings.Type == ActorType.GoldenCube))
          this.TrackedTreasures.Add(trileInstance);
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMenuCube || (this.GameState.InMap || this.GameState.InFpsMode) || (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || !this.CameraManager.ActionRunning || this.GameState.Loading))
        return;
      this.SinceCreated += gameTime.ElapsedGameTime;
      for (int index = 0; index < this.TrackedTreasures.Count; ++index)
      {
        TrileInstance instance = this.TrackedTreasures[index];
        float num1 = (float) Math.Sin(this.SinceCreated.TotalSeconds * 3.14159274101257 + (double) index / 0.142857000231743) * 0.1f;
        float num2 = num1 - instance.LastTreasureSin;
        instance.LastTreasureSin = num1;
        if (instance.Enabled && !instance.Removed)
        {
          if (!instance.Hidden)
          {
            if (instance.Trile.ActorSettings.Type != ActorType.GoldenCube)
              instance.Phi += (float) (gameTime.ElapsedGameTime.TotalSeconds * 2.0);
            instance.Position += num2 * Vector3.UnitY;
            this.LevelManager.UpdateInstance(instance);
            this.LevelMaterializer.GetTrileMaterializer(instance.Trile).UpdateInstance(instance);
          }
        }
        else
          this.TrackedTreasures.RemoveAt(index--);
      }
    }
  }
}
