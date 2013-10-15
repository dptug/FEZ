// Type: FezGame.Components.AttachedPlanesHost
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
  public class AttachedPlanesHost : GameComponent
  {
    private readonly List<AttachedPlanesHost.AttachedPlaneState> TrackedPlanes = new List<AttachedPlanesHost.AttachedPlaneState>();

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { get; set; }

    public AttachedPlanesHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.TrackedPlanes.Clear();
      foreach (BackgroundPlane backgroundPlane in (IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values)
      {
        if (backgroundPlane.AttachedGroup.HasValue)
        {
          TrileInstance trileInstance = Enumerable.FirstOrDefault<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Groups[backgroundPlane.AttachedGroup.Value].Triles);
          if (trileInstance != null)
          {
            Vector3 vector3 = backgroundPlane.Position - trileInstance.Position;
            this.TrackedPlanes.Add(new AttachedPlanesHost.AttachedPlaneState()
            {
              FirstTrile = trileInstance,
              Offset = vector3,
              Plane = backgroundPlane
            });
          }
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Paused || this.EngineState.InMap || (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || !this.CameraManager.ActionRunning) || this.EngineState.Loading)
        return;
      foreach (AttachedPlanesHost.AttachedPlaneState attachedPlaneState in this.TrackedPlanes)
        attachedPlaneState.Plane.Position = attachedPlaneState.FirstTrile.Position + attachedPlaneState.Offset;
    }

    private class AttachedPlaneState
    {
      public BackgroundPlane Plane;
      public TrileInstance FirstTrile;
      public Vector3 Offset;
    }
  }
}
