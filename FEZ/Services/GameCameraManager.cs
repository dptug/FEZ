// Type: FezGame.Services.GameCameraManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Tools;
using FezGame;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Services
{
  public class GameCameraManager : DefaultCameraManager, IGameCameraManager, IDefaultCameraManager, ICameraProvider
  {
    private static readonly float FirstPersonFov = MathHelper.ToRadians(75f);
    private int concurrentChanges;
    private float originalCarriedPhi;
    private bool shouldRotateInstance;

    public override float InterpolationSpeed
    {
      get
      {
        return (float) ((double) base.InterpolationSpeed * (1.5 + (double) Math.Abs(this.CollisionManager.GravityFactor) * 0.5) / 2.0);
      }
      set
      {
        base.InterpolationSpeed = value;
      }
    }

    public override bool ActionRunning
    {
      get
      {
        if (!Fez.LongScreenshot && !base.ActionRunning && this.PlayerManager.Action != ActionType.PivotTombstone)
          return this.PlayerManager.Action == ActionType.GrabTombstone;
        else
          return true;
      }
    }

    public Viewpoint RequestedViewpoint { get; set; }

    public Vector3 OriginalDirection { get; set; }

    public override float Radius
    {
      get
      {
        if (!this.GameState.MenuCubeIsZoomed)
          return base.Radius;
        else
          return 18f;
      }
      set
      {
        base.Radius = value;
      }
    }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ICameraService CameraService { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    static GameCameraManager()
    {
    }

    public GameCameraManager(Game game)
      : base(game)
    {
    }

    public void CancelViewTransition()
    {
      this.directionTransition = (Vector3SplineInterpolation) null;
      this.viewpoint = this.lastViewpoint;
      this.current = this.predefinedViews[this.viewpoint];
      this.current.Direction = -FezMath.ForwardVector(this.viewpoint);
      this.ViewTransitionCancelled = true;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += (Action) (() => this.lastViewpoint = this.viewpoint);
    }

    public override bool ChangeViewpoint(Viewpoint newView, float speedFactor)
    {
      if ((double) speedFactor != 0.0 && (newView == this.viewpoint || this.concurrentChanges >= 1) || this.PlayerManager.Action == ActionType.GrabTombstone && !this.PlayerManager.Animation.Timing.Ended)
        return false;
      if (!this.ViewTransitionReached && (double) speedFactor != 0.0)
        ++this.concurrentChanges;
      this.shouldRotateInstance = FezMath.IsOrthographic(newView) && FezMath.IsOrthographic(this.viewpoint);
      if (newView == Viewpoint.Perspective)
        this.predefinedViews[newView].Direction = this.current.Direction;
      base.ChangeViewpoint(newView, speedFactor);
      if (this.PlayerManager.CarriedInstance != null && this.concurrentChanges == 0)
        this.originalCarriedPhi = this.PlayerManager.CarriedInstance.Phi;
      this.CameraService.OnRotate();
      return true;
    }

    public void RecordNewCarriedInstancePhi()
    {
      this.originalCarriedPhi = this.PlayerManager.CarriedInstance.Phi;
      this.shouldRotateInstance = false;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      base.Update(gameTime);
      if (this.ViewTransitionReached)
        this.concurrentChanges = 0;
      if (this.PlayerManager.CarriedInstance == null || !this.shouldRotateInstance)
        return;
      this.PlayerManager.CarriedInstance.Phi = FezMath.WrapAngle(this.originalCarriedPhi + (FezMath.WrapAngle((float) Math.Atan2((double) FezMath.ForwardVector(this.LastViewpoint).Z, (double) FezMath.ForwardVector(this.LastViewpoint).X)) - FezMath.WrapAngle((float) (3.14159274101257 - Math.Atan2((double) this.View.Forward.Z, (double) this.View.Forward.X)))));
      if (!this.viewNewlyReached)
        return;
      this.PlayerManager.CarriedInstance.Phi = FezMath.SnapPhi(this.PlayerManager.CarriedInstance.Phi);
    }

    protected override void DollyZoom()
    {
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      if (!this.GameState.InFpsMode)
      {
        base.DollyZoom();
      }
      else
      {
        bool flag = FezMath.IsOrthographic(this.viewpoint);
        float amount = (double) this.directionTransition.TotalStep == 0.0 ? 1.0 / 1000.0 : this.directionTransition.TotalStep;
        float num1 = MathHelper.Lerp(flag ? GameCameraManager.FirstPersonFov : 0.0f, flag ? 0.0f : GameCameraManager.FirstPersonFov, amount);
        float num2 = this.radiusBeforeTransition;
        if (this.DollyZoomOut)
          num2 = this.radiusBeforeTransition + (float) ((1.0 - (double) Easing.EaseIn((double) amount, EasingType.Quadratic)) * 15.0);
        float num3 = (float) ((double) num2 / (double) this.AspectRatio / (2.0 * Math.Tan((double) num1 / 2.0))) / viewScale;
        if (this.directionTransition.Reached)
        {
          this.ProjectionTransition = false;
          if (!flag)
          {
            this.predefinedViews[this.lastViewpoint].Direction = -FezMath.ForwardVector(this.lastViewpoint);
            this.current.Radius = 0.1f;
          }
          else
          {
            this.current.Radius = this.radiusBeforeTransition;
            this.NearPlane = 0.1f;
            this.FarPlane = 500f;
            this.GameState.InFpsMode = false;
          }
          this.FogManager.Density = this.LevelManager.Sky == null ? 0.0f : this.LevelManager.Sky.FogDensity;
          this.DollyZoomOut = false;
          this.RebuildProjection();
          this.SnapInterpolation();
        }
        else
        {
          this.FogManager.Density = (this.LevelManager.Sky == null ? 0.0f : this.LevelManager.Sky.FogDensity) * Easing.EaseIn(flag ? 1.0 - (double) amount : (double) amount, EasingType.Quadratic);
          float num4 = (float) ((double) num3 * (flag ? (double) amount : 1.0 - (double) amount) + 0.100000001490116);
          this.NearPlane = Math.Max(0.1f, 0.1f + num4 - num2);
          this.FarPlane = Math.Max(num4 + this.NearPlane, 499.9f);
          this.FieldOfView = num1;
          this.projection = Matrix.CreatePerspectiveFieldOfView(this.FieldOfView, this.AspectRatio, this.NearPlane, this.FarPlane);
          this.OnProjectionChanged();
          this.current.Radius = num4;
          this.view = Matrix.CreateLookAt(this.current.Radius * this.current.Direction + this.current.Center, this.current.Center, Vector3.UnitY);
          this.OnViewChanged();
        }
      }
    }
  }
}
