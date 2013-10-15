// Type: FezEngine.Services.DefaultCameraManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezEngine.Services
{
  public class DefaultCameraManager : CameraManager, IDefaultCameraManager, ICameraProvider
  {
    public static float TransitionSpeed = 0.45f;
    protected static readonly float DefaultFov = MathHelper.ToRadians(45f);
    protected DefaultCameraManager.PredefinedView current = new DefaultCameraManager.PredefinedView();
    protected float defaultViewableWidth = 26.66667f;
    protected readonly DefaultCameraManager.PredefinedView interpolated = new DefaultCameraManager.PredefinedView();
    public const float TrixelsPerTrile = 16f;
    protected const float TransitionTiltFactor = 0.0f;
    protected const float DefaultNearPlane = 0.1f;
    protected const float DefaultFarPlane = 500f;
    public static bool NoInterpolation;
    protected readonly Dictionary<Viewpoint, DefaultCameraManager.PredefinedView> predefinedViews;
    protected Vector3SplineInterpolation directionTransition;
    protected Viewpoint viewpoint;
    protected Viewpoint lastViewpoint;
    protected Viewpoint olderViewpoint;
    protected bool viewNewlyReached;
    protected bool projNewlyReached;
    protected bool projReached;
    protected float radiusBeforeTransition;
    private bool transitionNewlyReached;
    private Vector3 viewOffset;
    private float pixelsPerTrixel;
    private bool constrained;

    public virtual float InterpolationSpeed { get; set; }

    public bool ViewTransitionCancelled { get; protected set; }

    public bool ForceTransition { get; set; }

    public Matrix InverseView { get; private set; }

    public Vector3 Center
    {
      get
      {
        return this.current.Center;
      }
      set
      {
        this.current.Center = value;
      }
    }

    public Vector3 InterpolatedCenter
    {
      get
      {
        return this.interpolated.Center;
      }
      set
      {
        this.interpolated.Center = value;
      }
    }

    public virtual float Radius
    {
      get
      {
        if (!this.ProjectionTransition)
          return this.interpolated.Radius;
        else
          return this.current.Radius;
      }
      set
      {
        bool flag = !FezMath.AlmostEqual(this.current.Radius, value);
        this.current.Radius = value;
        if (!flag || !FezMath.IsOrthographic(this.viewpoint))
          return;
        this.RebuildProjection();
      }
    }

    public Vector3 Direction
    {
      get
      {
        return this.current.Direction;
      }
      set
      {
        this.current.Direction = value;
      }
    }

    public float DefaultViewableWidth
    {
      get
      {
        return this.defaultViewableWidth;
      }
      set
      {
        this.defaultViewableWidth = value;
        foreach (DefaultCameraManager.PredefinedView predefinedView in this.predefinedViews.Values)
          predefinedView.Radius = this.defaultViewableWidth;
        this.RebuildProjection();
      }
    }

    public Viewpoint Viewpoint
    {
      get
      {
        return this.viewpoint;
      }
    }

    public FaceOrientation VisibleOrientation
    {
      get
      {
        return FezMath.VisibleOrientation(this.Viewpoint);
      }
    }

    public Viewpoint LastViewpoint
    {
      get
      {
        if (this.lastViewpoint != Viewpoint.Perspective)
          return this.lastViewpoint;
        else
          return this.olderViewpoint;
      }
    }

    public bool ProjectionTransitionNewlyReached
    {
      get
      {
        if (this.transitionNewlyReached)
          return this.lastViewpoint == Viewpoint.Perspective;
        else
          return false;
      }
    }

    public bool DollyZoomOut { protected get; set; }

    public bool ViewTransitionReached
    {
      get
      {
        if (this.directionTransition == null || this.directionTransition.Reached)
          return !this.ForceTransition;
        else
          return false;
      }
    }

    public virtual bool ActionRunning
    {
      get
      {
        return this.ViewTransitionReached;
      }
    }

    public float ViewTransitionStep
    {
      get
      {
        if (this.directionTransition != null && !this.ViewTransitionReached)
          return this.directionTransition.TotalStep;
        else
          return 0.0f;
      }
    }

    public Quaternion Rotation { get; protected set; }

    public bool InterpolationReached { get; protected set; }

    public float FieldOfView { get; protected set; }

    public BoundingFrustum Frustum { get; protected set; }

    public Vector3 Position { get; protected set; }

    public float NearPlane { get; protected set; }

    public float FarPlane { get; protected set; }

    public float AspectRatio { get; set; }

    public bool ProjectionTransition { get; set; }

    public bool ForceInterpolation { get; set; }

    public Vector3 ViewOffset
    {
      get
      {
        return this.viewOffset;
      }
      set
      {
        Vector3 vector3 = this.viewOffset;
        if (this.ProjectionTransition)
        {
          this.current.Center -= this.viewOffset;
          this.viewOffset = value;
          this.current.Center += this.viewOffset;
          if (this.viewOffset == Vector3.Zero && vector3 == Vector3.Zero)
            return;
          this.DollyZoom();
        }
        else
        {
          this.interpolated.Center -= this.viewOffset;
          this.viewOffset = value;
          this.interpolated.Center += this.viewOffset;
          if (this.viewOffset == Vector3.Zero && vector3 == Vector3.Zero)
            return;
          this.RebuildInterpolatedView();
        }
      }
    }

    public float PixelsPerTrixel
    {
      get
      {
        return this.pixelsPerTrixel;
      }
      set
      {
        this.pixelsPerTrixel = value;
        this.DefaultViewableWidth = (float) this.Game.GraphicsDevice.Viewport.Width / (this.PixelsPerTrixel * 16f);
      }
    }

    public bool StickyCam { get; set; }

    public Vector2? PanningConstraints { get; set; }

    public Vector3 ConstrainedCenter { get; set; }

    public bool Constrained
    {
      get
      {
        return this.constrained;
      }
      set
      {
        this.constrained = value;
      }
    }

    [ServiceDependency]
    public IGraphicsDeviceService GraphicsService { protected get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { protected get; set; }

    [ServiceDependency]
    public IFogManager FogManager { protected get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { protected get; set; }

    public event Action ViewpointChanged = new Action(Util.NullAction);

    public event Action PreViewpointChanged = new Action(Util.NullAction);

    static DefaultCameraManager()
    {
    }

    protected DefaultCameraManager(Game game)
      : base(game)
    {
      this.predefinedViews = new Dictionary<Viewpoint, DefaultCameraManager.PredefinedView>((IEqualityComparer<Viewpoint>) ViewpointComparer.Default);
      this.FieldOfView = DefaultCameraManager.DefaultFov;
      this.NearPlane = 0.1f;
      this.FarPlane = 500f;
      this.Frustum = new BoundingFrustum(Matrix.Identity);
    }

    public override void Initialize()
    {
      this.InterpolationSpeed = 10f;
      this.ResetViewpoints();
      this.viewpoint = Viewpoint.Right;
      this.current = this.predefinedViews[Viewpoint.Right];
      this.SnapInterpolation();
      this.GraphicsService.DeviceReset += (EventHandler<EventArgs>) delegate
      {
        this.PixelsPerTrixel = this.pixelsPerTrixel;
        this.RebuildProjection();
      };
    }

    private void RebuildFrustum()
    {
      this.Frustum.Matrix = this.view * this.projection;
    }

    public bool ChangeViewpoint(Viewpoint newView)
    {
      return this.ChangeViewpoint(newView, 1f);
    }

    public virtual bool ChangeViewpoint(Viewpoint newViewpoint, float speedFactor)
    {
      bool flag = FezMath.IsOrthographic(newViewpoint) != FezMath.IsOrthographic(this.viewpoint);
      if (flag && this.ProjectionTransition)
        return false;
      this.ProjectionTransition = flag && (double) speedFactor > 0.0;
      this.radiusBeforeTransition = FezMath.IsOrthographic(this.viewpoint) ? this.current.Radius : this.predefinedViews[this.lastViewpoint].Radius;
      if ((double) speedFactor > 0.0)
      {
        float num = (float) ((double) (Math.Abs(FezMath.GetDistance(newViewpoint, this.Viewpoint)) - 1) / 2.0 + 1.0);
        if (newViewpoint == Viewpoint.Perspective || this.Viewpoint == Viewpoint.Perspective)
          num = 1f;
        Vector3 from = this.current.Direction;
        Vector3 to = this.predefinedViews[newViewpoint].Direction;
        this.directionTransition = new Vector3SplineInterpolation(TimeSpan.FromSeconds((double) DefaultCameraManager.TransitionSpeed * (double) num * (double) speedFactor), new Vector3[3]
        {
          from,
          DefaultCameraManager.GetIntemediateVector(from, to),
          to
        });
        this.directionTransition.Start();
      }
      if (FezMath.IsOrthographic(this.viewpoint))
      {
        this.current.Direction = -FezMath.ForwardVector(this.viewpoint);
        this.current.Radius = this.DefaultViewableWidth;
      }
      this.olderViewpoint = this.lastViewpoint;
      this.lastViewpoint = this.viewpoint;
      this.viewpoint = newViewpoint;
      Vector3 center = this.Center;
      this.current = this.predefinedViews[newViewpoint];
      this.current.Center = center;
      if (this.lastViewpoint != Viewpoint.None)
      {
        this.PreViewpointChanged();
        if (!this.ViewTransitionCancelled)
          this.ViewpointChanged();
      }
      if ((double) speedFactor == 0.0 && !this.ViewTransitionCancelled)
        this.RebuildView();
      bool transitionCancelled = this.ViewTransitionCancelled;
      this.ViewTransitionCancelled = false;
      return !transitionCancelled;
    }

    public void AlterTransition(Vector3 newDestinationDirection)
    {
      this.directionTransition.Points[1] = DefaultCameraManager.GetIntemediateVector(this.directionTransition.Points[0], newDestinationDirection);
      this.directionTransition.Points[2] = newDestinationDirection;
    }

    public void AlterTransition(Viewpoint newTo)
    {
      Viewpoint rotatedView = FezMath.GetRotatedView(FezMath.AsViewpoint(FezMath.OrientationFromDirection(this.directionTransition.Points[0])), FezMath.GetDistance(FezMath.AsViewpoint(FezMath.OrientationFromDirection(this.directionTransition.Points[2])), newTo));
      Vector3 from = this.predefinedViews[rotatedView].Direction;
      Vector3 to = this.predefinedViews[newTo].Direction;
      this.directionTransition.Points[0] = from;
      this.directionTransition.Points[1] = DefaultCameraManager.GetIntemediateVector(from, to);
      this.directionTransition.Points[2] = to;
      this.current = this.predefinedViews[newTo];
      this.lastViewpoint = rotatedView;
      this.viewpoint = newTo;
    }

    private static Vector3 GetIntemediateVector(Vector3 from, Vector3 to)
    {
      return (!FezMath.AlmostEqual(FezMath.AngleBetween(from, to), 3.141593f) ? FezMath.Slerp(from, to, 0.5f) : Vector3.Cross(Vector3.Normalize(to - from), Vector3.UnitY)) + Vector3.UnitY * 0.0f;
    }

    public void SnapInterpolation()
    {
      this.interpolated.Center = this.current.Center;
      this.interpolated.Direction = this.current.Direction;
      this.interpolated.Radius = this.current.Radius;
      this.InterpolationReached = true;
      this.RebuildView();
      this.RebuildProjection();
    }

    public virtual void ResetViewpoints()
    {
      this.predefinedViews.Clear();
      this.predefinedViews.Add(Viewpoint.Perspective, new DefaultCameraManager.PredefinedView(Vector3.Zero, this.defaultViewableWidth, Vector3.One));
      this.predefinedViews.Add(Viewpoint.Front, new DefaultCameraManager.PredefinedView(Vector3.Zero, this.defaultViewableWidth, -FezMath.ForwardVector(Viewpoint.Front)));
      this.predefinedViews.Add(Viewpoint.Right, new DefaultCameraManager.PredefinedView(Vector3.Zero, this.defaultViewableWidth, -FezMath.ForwardVector(Viewpoint.Right)));
      this.predefinedViews.Add(Viewpoint.Back, new DefaultCameraManager.PredefinedView(Vector3.Zero, this.defaultViewableWidth, -FezMath.ForwardVector(Viewpoint.Back)));
      this.predefinedViews.Add(Viewpoint.Left, new DefaultCameraManager.PredefinedView(Vector3.Zero, this.defaultViewableWidth, -FezMath.ForwardVector(Viewpoint.Left)));
      if (this.viewpoint == Viewpoint.None)
        return;
      this.current = this.predefinedViews[this.viewpoint];
    }

    public void RebuildProjection()
    {
      this.AspectRatio = this.GraphicsService.GraphicsDevice.Viewport.AspectRatio;
      float width = this.interpolated.Radius / SettingsManager.GetViewScale(this.GraphicsService.GraphicsDevice);
      if (FezMath.IsOrthographic(this.viewpoint))
        this.projection = Matrix.CreateOrthographic(width, width / this.AspectRatio, this.NearPlane, this.FarPlane);
      else
        this.projection = Matrix.CreatePerspectiveFieldOfView(this.FieldOfView, this.AspectRatio, this.NearPlane, this.FarPlane);
      this.OnProjectionChanged();
    }

    public void RebuildView()
    {
      this.view = Matrix.CreateLookAt((FezMath.IsOrthographic(this.viewpoint) ? (float) (((double) this.FarPlane - (double) this.NearPlane) / 2.0) : this.current.Radius) * this.current.Direction + this.current.Center, this.current.Center, Vector3.UnitY);
      this.OnViewChanged();
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.ViewTransitionReached && !this.ForceTransition)
      {
        this.transitionNewlyReached = true;
        this.directionTransition.Update(gameTime);
        this.current.Direction = this.directionTransition.Current;
      }
      else if (this.directionTransition != null && this.transitionNewlyReached)
      {
        this.transitionNewlyReached = false;
        this.current.Direction = this.directionTransition.Current;
      }
      if (this.ProjectionTransition)
        this.DollyZoom();
      else
        this.Interpolate(gameTime);
    }

    protected virtual void DollyZoom()
    {
      bool flag = FezMath.IsOrthographic(this.viewpoint);
      float amount = (double) this.directionTransition.TotalStep == 0.0 ? 1.0 / 1000.0 : this.directionTransition.TotalStep;
      float num1 = MathHelper.Lerp(flag ? DefaultCameraManager.DefaultFov : 0.0f, flag ? 0.0f : DefaultCameraManager.DefaultFov, amount);
      float num2 = this.radiusBeforeTransition;
      if (this.DollyZoomOut)
        num2 = this.radiusBeforeTransition + (float) ((1.0 - (double) Easing.EaseIn((double) amount, EasingType.Quadratic)) * 15.0);
      float num3 = (float) ((double) num2 / (double) this.AspectRatio / (2.0 * Math.Tan((double) num1 / 2.0)));
      if (this.directionTransition.Reached)
      {
        this.ProjectionTransition = false;
        if (!flag)
        {
          this.predefinedViews[this.lastViewpoint].Direction = -FezMath.ForwardVector(this.lastViewpoint);
          this.current.Radius = num3;
        }
        else
        {
          this.current.Radius = this.radiusBeforeTransition;
          this.NearPlane = 0.1f;
          this.FarPlane = 500f;
        }
        this.FogManager.Density = this.LevelManager.Sky == null ? 0.0f : this.LevelManager.Sky.FogDensity;
        this.DollyZoomOut = false;
        this.RebuildProjection();
        this.SnapInterpolation();
      }
      else
      {
        this.FogManager.Density = (this.LevelManager.Sky == null ? 0.0f : this.LevelManager.Sky.FogDensity) * Easing.EaseIn(flag ? 1.0 - (double) amount : (double) amount, EasingType.Quadratic);
        this.NearPlane = Math.Max(0.1f, 0.1f + num3 - num2);
        this.FarPlane = Math.Max(num3 + this.NearPlane, 499.9f);
        this.FieldOfView = num1;
        this.projection = Matrix.CreatePerspectiveFieldOfView(this.FieldOfView, this.AspectRatio, this.NearPlane, this.FarPlane);
        this.OnProjectionChanged();
        this.current.Radius = num3;
        this.view = Matrix.CreateLookAt(this.current.Radius * this.current.Direction + this.current.Center, this.current.Center, Vector3.UnitY);
        this.OnViewChanged();
      }
    }

    private void Interpolate(GameTime gameTime)
    {
      float num = MathHelper.Clamp((float) gameTime.ElapsedGameTime.TotalSeconds * this.InterpolationSpeed, 0.0f, 1f);
      if (DefaultCameraManager.NoInterpolation)
        num = 1f;
      if (this.current.Direction == Vector3.Zero)
        return;
      this.current.Direction = Vector3.Normalize(this.current.Direction);
      this.interpolated.Center = Vector3.Lerp(this.interpolated.Center, this.current.Center, num);
      this.interpolated.Radius = MathHelper.Lerp(this.interpolated.Radius, this.current.Radius, num);
      this.interpolated.Direction = FezMath.Slerp(this.interpolated.Direction, this.current.Direction, num);
      bool flag1 = (double) DefaultCameraManager.TransitionSpeed < 2.0 && (FezMath.AlmostEqual(this.interpolated.Direction, this.current.Direction) && FezMath.AlmostEqual(this.interpolated.Center, this.current.Center));
      bool flag2 = FezMath.AlmostEqual(this.interpolated.Radius, this.current.Radius);
      if (this.ForceInterpolation)
      {
        flag1 = false;
        flag2 = false;
      }
      this.viewNewlyReached = flag1 && !this.InterpolationReached;
      if (this.viewNewlyReached)
      {
        this.interpolated.Direction = this.current.Direction;
        this.interpolated.Center = this.current.Center;
      }
      this.projNewlyReached = flag2 && !this.projReached;
      if (this.projNewlyReached)
        this.interpolated.Radius = this.current.Radius;
      this.InterpolationReached = flag1 && flag2;
      this.projReached = flag2;
      if (!flag1 || this.viewNewlyReached || (double) num == 1.0)
        this.RebuildInterpolatedView();
      if (flag2 && !this.projNewlyReached && (double) num != 1.0)
        return;
      this.RebuildInterpolatedProj();
    }

    private void RebuildInterpolatedView()
    {
      this.view = Matrix.CreateLookAt((FezMath.IsOrthographic(this.viewpoint) ? 249.95f : this.interpolated.Radius) * this.interpolated.Direction + this.interpolated.Center, this.interpolated.Center, Vector3.UnitY);
      this.OnViewChanged();
    }

    private void RebuildInterpolatedProj()
    {
      if (!FezMath.IsOrthographic(this.viewpoint) || (double) this.interpolated.Radius == (double) this.current.Radius && !this.projNewlyReached || this.ProjectionTransition)
        return;
      float width = this.interpolated.Radius / SettingsManager.GetViewScale(this.GraphicsService.GraphicsDevice);
      this.projection = Matrix.CreateOrthographic(width, width / this.AspectRatio, 0.1f, 500f);
      this.OnProjectionChanged();
    }

    protected override void OnViewChanged()
    {
      this.InverseView = Matrix.Invert(this.view);
      if (!float.IsNaN(this.InverseView.M11) && !float.IsNaN(this.InverseView.M12) && (!float.IsNaN(this.InverseView.M13) && !float.IsNaN(this.InverseView.M14)) && (!float.IsNaN(this.InverseView.M21) && !float.IsNaN(this.InverseView.M22) && (!float.IsNaN(this.InverseView.M23) && !float.IsNaN(this.InverseView.M24))) && (!float.IsNaN(this.InverseView.M31) && !float.IsNaN(this.InverseView.M32) && (!float.IsNaN(this.InverseView.M33) && !float.IsNaN(this.InverseView.M34))))
      {
        Vector3 scale;
        Quaternion rotation;
        Vector3 translation;
        this.InverseView.Decompose(out scale, out rotation, out translation);
        this.Position = translation;
        this.Rotation = rotation;
      }
      this.RebuildFrustum();
      base.OnViewChanged();
    }

    protected override void OnProjectionChanged()
    {
      this.RebuildFrustum();
      base.OnProjectionChanged();
    }

    protected class PredefinedView
    {
      public Vector3 Center;
      public float Radius;
      public Vector3 Direction;

      public PredefinedView()
      {
      }

      public PredefinedView(Vector3 center, float radius, Vector3 direction)
      {
        this.Center = center;
        this.Radius = radius;
        this.Direction = direction;
      }
    }
  }
}
