// Type: FezEngine.Components.AnimatedPlanesHost
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezEngine.Components
{
  public class AnimatedPlanesHost : GameComponent
  {
    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    public AnimatedPlanesHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        foreach (BackgroundPlane item_0 in (IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values)
          item_0.OriginalPosition = new Vector3?(item_0.Position);
      });
    }

    public override void Update(GameTime gameTime)
    {
      if (this.LevelMaterializer.LevelPlanes.Count == 0 || this.EngineState.Paused || (this.EngineState.InMap || this.EngineState.Loading))
        return;
      bool flag = FezMath.IsOrthographic(this.CameraManager.Viewpoint) && this.CameraManager.ActionRunning;
      bool inEditor = this.EngineState.InEditor;
      foreach (BackgroundPlane backgroundPlane in this.LevelMaterializer.LevelPlanes)
      {
        if (backgroundPlane.Visible || backgroundPlane.ActorType == ActorType.Bomb)
        {
          if (flag && backgroundPlane.Animated)
          {
            int frame = backgroundPlane.Timing.Frame;
            backgroundPlane.Timing.Update(gameTime.ElapsedGameTime);
            if (!backgroundPlane.Loop && frame > backgroundPlane.Timing.Frame)
            {
              this.LevelManager.BackgroundPlanes.Remove(backgroundPlane.Id);
              backgroundPlane.Dispose();
            }
            else
              backgroundPlane.MarkDirty();
          }
          if (backgroundPlane.Billboard)
            backgroundPlane.Rotation = this.CameraManager.Rotation * backgroundPlane.OriginalRotation;
          if (!inEditor && (double) backgroundPlane.ParallaxFactor != 0.0 && flag)
          {
            Viewpoint view = FezMath.AsViewpoint(backgroundPlane.Orientation);
            if (!backgroundPlane.OriginalPosition.HasValue)
              backgroundPlane.OriginalPosition = new Vector3?(backgroundPlane.Position);
            float num = (float) ((double) (-4 * (this.LevelManager.Descending ? -1 : 1)) / (double) this.CameraManager.PixelsPerTrixel - 15.0 / 32.0 + 1.0);
            Vector3 vector3 = this.CameraManager.InterpolatedCenter - backgroundPlane.OriginalPosition.Value + num * Vector3.UnitY;
            backgroundPlane.Position = backgroundPlane.OriginalPosition.Value + vector3 * FezMath.ScreenSpaceMask(view) * backgroundPlane.ParallaxFactor;
          }
          else if (!inEditor && (double) backgroundPlane.ParallaxFactor != 0.0 && backgroundPlane.OriginalPosition.HasValue && backgroundPlane.Position != backgroundPlane.OriginalPosition.Value)
            backgroundPlane.Position = backgroundPlane.OriginalPosition.Value;
        }
      }
    }
  }
}
