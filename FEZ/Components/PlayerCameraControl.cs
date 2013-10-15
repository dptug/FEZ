// Type: FezGame.Components.PlayerCameraControl
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  public class PlayerCameraControl : GameComponent
  {
    private const float TrilesBeforeScreenMove = 1.5f;
    public const float VerticalOffset = 4f;
    private Vector3 StickyCenter;
    private float MinimumStickDistance;
    private SoundEffect swooshLeft;
    private SoundEffect swooshRight;
    private SoundEffect slowSwooshLeft;
    private SoundEffect slowSwooshRight;
    private Vector2 lastFactors;

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IMouseStateManager MouseState { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    public PlayerCameraControl(Game game)
      : base(game)
    {
      this.UpdateOrder = 10;
    }

    public override void Initialize()
    {
      this.swooshLeft = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/RotateLeft");
      this.swooshRight = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/RotateRight");
      this.slowSwooshLeft = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/RotateLeftHalfSpeed");
      this.slowSwooshRight = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/RotateRightHalfSpeed");
      this.CameraManager.Radius = this.CameraManager.DefaultViewableWidth;
      this.LevelManager.LevelChanged += (Action) (() => this.CameraManager.StickyCam = false);
      this.CameraManager.ViewpointChanged += (Action) (() =>
      {
        this.MinimumStickDistance = 2f;
        if (this.CameraManager.Viewpoint != Viewpoint.Perspective && !this.GameState.InMap)
          return;
        this.CameraManager.OriginalDirection = this.CameraManager.Direction;
      });
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.InCutscene)
        return;
      if (!this.PlayerManager.CanControl && !this.GameState.InMenuCube && !this.GameState.InMap)
      {
        this.InputManager.SaveState();
        this.InputManager.Reset();
      }
      if ((!ActionTypeExtensions.PreventsRotation(this.PlayerManager.Action) || this.GameState.InMap || (this.GameState.InMenuCube || this.GameState.InFpsMode)) && this.PlayerManager.CanRotate && (!this.LevelManager.Flat || this.PlayerManager.Action == ActionType.GrabTombstone || (this.GameState.InMap || this.GameState.InFpsMode) || this.GameState.InMenuCube) && (!this.GameState.Paused && !this.GameState.InCutscene))
      {
        bool flag = this.PlayerManager.Action == ActionType.GrabTombstone;
        if (!this.GameState.InFpsMode && !this.GameState.DisallowRotation)
        {
          if (this.InputManager.RotateLeft == FezButtonState.Pressed || flag && this.InputManager.RotateLeft == FezButtonState.Down)
            this.RotateViewLeft();
          if (this.InputManager.RotateRight == FezButtonState.Pressed || flag && this.InputManager.RotateRight == FezButtonState.Down)
            this.RotateViewRight();
        }
        if (this.CameraManager.Viewpoint == Viewpoint.Perspective || this.CameraManager.RequestedViewpoint == Viewpoint.Perspective || this.GameState.InMap)
        {
          if (this.CameraManager.Viewpoint == Viewpoint.Perspective && !this.GameState.InMenuCube && !this.GameState.InMap)
            this.StickyCenter = this.CameraManager.Center = Vector3.Lerp(this.CameraManager.Center, this.PlayerManager.Position + (float) (4.0 * (this.LevelManager.Descending ? -1.0 : 1.0)) / this.CameraManager.PixelsPerTrixel * Vector3.UnitY, 0.075f);
          if (this.GameState.InFpsMode)
          {
            if (this.InputManager.FreeLook != Vector2.Zero)
            {
              Vector3 vector3 = Vector3.Transform(this.CameraManager.Direction, Quaternion.CreateFromAxisAngle(this.CameraManager.InverseView.Right, this.InputManager.FreeLook.Y * 0.4f));
              if ((double) vector3.Y > 0.7 || (double) vector3.Y < -0.7)
              {
                float num = 0.7f / new Vector2(vector3.X, vector3.Z).Length();
                vector3 = new Vector3(vector3.X * num, 0.7f * (float) Math.Sign(vector3.Y), vector3.Z * num);
              }
              vector3 = Vector3.Transform(vector3, Quaternion.CreateFromAxisAngle(this.CameraManager.InverseView.Up, (float) (-(double) this.InputManager.FreeLook.X * 0.5)));
              if (!this.CameraManager.ActionRunning)
                this.CameraManager.AlterTransition(FezMath.Slerp(this.CameraManager.Direction, vector3, 0.1f));
              else
                this.CameraManager.Direction = FezMath.Slerp(this.CameraManager.Direction, vector3, 0.1f);
              if ((double) this.CameraManager.Direction.Y < -0.625)
                this.CameraManager.Direction = new Vector3(this.CameraManager.Direction.X, -0.625f, this.CameraManager.Direction.Z);
            }
          }
          else if (this.InputManager.FreeLook != Vector2.Zero || this.GameState.InMap)
          {
            Vector3 vector3 = Vector3.Transform(this.CameraManager.OriginalDirection, Matrix.CreateFromAxisAngle(Vector3.Up, 1.570796f));
            Vector3 to1 = Vector3.Transform(this.CameraManager.OriginalDirection, Matrix.CreateFromAxisAngle(vector3, -1.570796f));
            Vector2 vector2 = this.InputManager.FreeLook / (this.GameState.MenuCubeIsZoomed ? 1.75f : 6.875f);
            float step = 0.1f;
            if (this.GameState.InMap && this.MouseState.LeftButton.State == MouseButtonStates.Dragging)
            {
              vector2 = Vector2.Clamp(new Vector2((float) -this.MouseState.LeftButton.DragState.Movement.X, (float) this.MouseState.LeftButton.DragState.Movement.Y) / (300f * SettingsManager.GetViewScale(this.GraphicsDevice)), -Vector2.One, Vector2.One) / (55.0 / 16.0);
              step = 0.2f;
              this.lastFactors = vector2;
            }
            if (this.GameState.InMap && this.MouseState.LeftButton.State == MouseButtonStates.DragEnded)
            {
              if ((double) this.lastFactors.X > 0.174999997019768)
                this.RotateViewRight();
              else if ((double) this.lastFactors.X < -0.174999997019768)
                this.RotateViewLeft();
            }
            if (this.GameState.InMap)
            {
              vector2 *= new Vector2(3.425f, 1.725f);
              vector2.Y += 0.25f;
              vector2.X += 0.5f;
            }
            Vector3 to2 = FezMath.Slerp(FezMath.Slerp(this.CameraManager.OriginalDirection, vector3, vector2.X), to1, vector2.Y);
            if (!this.CameraManager.ActionRunning)
              this.CameraManager.AlterTransition(FezMath.Slerp(this.CameraManager.Direction, to2, step));
            else
              this.CameraManager.Direction = FezMath.Slerp(this.CameraManager.Direction, to2, step);
          }
          else if (!this.CameraManager.ActionRunning)
            this.CameraManager.AlterTransition(FezMath.Slerp(this.CameraManager.Direction, this.CameraManager.OriginalDirection, 0.1f));
          else
            this.CameraManager.Direction = FezMath.Slerp(this.CameraManager.Direction, this.CameraManager.OriginalDirection, 0.1f);
        }
      }
      if (this.CameraManager.RequestedViewpoint != Viewpoint.None)
      {
        if (this.CameraManager.RequestedViewpoint != this.CameraManager.Viewpoint)
          this.RotateTo(this.CameraManager.RequestedViewpoint);
        this.CameraManager.RequestedViewpoint = Viewpoint.None;
      }
      if (!this.GameState.Paused && (!this.CameraManager.Constrained || (this.CameraManager.PanningConstraints.HasValue || this.GameState.InMap)) && !this.PlayerManager.Hidden)
      {
        if ((this.CameraManager.ActionRunning || this.CameraManager.ForceTransition) && this.CameraManager.Viewpoint != Viewpoint.Perspective)
        {
          this.FollowGomez();
          if (this.InputManager.FreeLook != Vector2.Zero)
          {
            if (!this.CameraManager.StickyCam)
              this.StickyCenter = this.CameraManager.Center;
            this.MinimumStickDistance = float.MaxValue;
            Vector2 vector2_1 = this.InputManager.FreeLook;
            if (this.MouseState.LeftButton.State == MouseButtonStates.Dragging)
              vector2_1 = -vector2_1;
            this.CameraManager.StickyCam = true;
            Vector2 vector2_2 = new Vector2(this.CameraManager.Radius, this.CameraManager.Radius / this.CameraManager.AspectRatio) * 0.4f / SettingsManager.GetViewScale(this.GraphicsDevice);
            this.StickyCenter = Vector3.Lerp(this.StickyCenter, this.PlayerManager.Position + vector2_1.X * FezMath.RightVector(this.CameraManager.Viewpoint) * vector2_2.X + vector2_1.Y * Vector3.UnitY * vector2_2.Y, 0.05f);
          }
          if (this.InputManager.ClampLook == FezButtonState.Pressed)
            this.CameraManager.StickyCam = false;
        }
      }
      else
        this.CameraManager.StickyCam = false;
      if (this.PlayerManager.CanControl || this.GameState.InMenuCube || this.GameState.InMap)
        return;
      this.InputManager.RecoverState();
    }

    private void FollowGomez()
    {
      float num1 = this.CameraManager.PixelsPerTrixel;
      if (this.GameState.FarawaySettings.InTransition && FezMath.AlmostEqual(this.GameState.FarawaySettings.DestinationCrossfadeStep, 1f))
        num1 = MathHelper.Lerp(this.CameraManager.PixelsPerTrixel, this.GameState.FarawaySettings.DestinationPixelsPerTrixel, (float) (((double) this.GameState.FarawaySettings.TransitionStep - 0.875) / 0.125));
      float num2 = (float) (4.0 * (this.LevelManager.Descending ? -1.0 : 1.0)) / num1;
      Vector3 vector3_1 = new Vector3(this.CameraManager.Center.X, this.PlayerManager.Position.Y + num2, this.CameraManager.Center.Z);
      Vector3 center = this.CameraManager.Center;
      if (this.CameraManager.StickyCam)
      {
        Vector3 vector3_2 = this.PlayerManager.Position + Vector3.UnitY * num2;
        Vector3 vector3_3 = this.StickyCenter * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint) - vector3_2 * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
        float val1 = vector3_3.Length() + 1f;
        if (this.InputManager.FreeLook == Vector2.Zero)
        {
          this.MinimumStickDistance = Math.Min(val1, this.MinimumStickDistance);
          float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
          if ((double) Math.Abs(vector3_3.X + vector3_3.Z) > (double) this.CameraManager.Radius * 0.400000005960464 / (double) viewScale || (double) Math.Abs(vector3_3.Y) > (double) this.CameraManager.Radius * 0.400000005960464 / (double) this.CameraManager.AspectRatio / (double) viewScale)
            this.MinimumStickDistance = 2.5f;
          if ((double) this.MinimumStickDistance < 4.0)
            this.StickyCenter = Vector3.Lerp(this.StickyCenter, vector3_2, (float) Math.Pow(1.0 / (double) this.MinimumStickDistance, 4.0));
        }
        vector3_1 = this.StickyCenter;
        if ((double) val1 <= 1.1)
          this.CameraManager.StickyCam = false;
      }
      else
      {
        if ((double) MathHelper.Clamp(this.PlayerManager.Position.X, center.X - 1.5f, center.X + 1.5f) != (double) this.PlayerManager.Position.X)
        {
          float num3 = this.PlayerManager.Position.X - center.X;
          vector3_1.X += num3 - 1.5f * (float) Math.Sign(num3);
        }
        if ((double) MathHelper.Clamp(this.PlayerManager.Position.Z, center.Z - 1.5f, center.Z + 1.5f) != (double) this.PlayerManager.Position.Z)
        {
          float num3 = this.PlayerManager.Position.Z - this.CameraManager.Center.Z;
          vector3_1.Z += num3 - 1.5f * (float) Math.Sign(num3);
        }
      }
      if (this.CameraManager.PanningConstraints.HasValue && !this.GameState.InMap)
      {
        Vector3 vector3_2 = FezMath.DepthMask(this.CameraManager.Viewpoint);
        Vector3 vector3_3 = FezMath.SideMask(this.CameraManager.Viewpoint);
        Vector3 vector3_4 = new Vector3(MathHelper.Lerp(this.CameraManager.ConstrainedCenter.X, vector3_1.X, this.CameraManager.PanningConstraints.Value.X), MathHelper.Lerp(this.CameraManager.ConstrainedCenter.Y, vector3_1.Y, this.CameraManager.PanningConstraints.Value.Y), MathHelper.Lerp(this.CameraManager.ConstrainedCenter.Z, vector3_1.Z, this.CameraManager.PanningConstraints.Value.X));
        this.CameraManager.Center = this.CameraManager.Center * vector3_2 + vector3_3 * vector3_4 + Vector3.UnitY * vector3_4;
      }
      else
      {
        if (this.GameState.InMenuCube || this.GameState.InMap || FezMath.In<ActionType>(this.PlayerManager.Action, ActionType.PullUpCornerLedge, ActionType.LowerToCornerLedge, ActionType.Victory, (IEqualityComparer<ActionType>) ActionTypeComparer.Default))
          return;
        this.CameraManager.Center = vector3_1;
      }
    }

    private void RotateViewLeft()
    {
      bool flag = this.PlayerManager.Action == ActionType.GrabTombstone;
      if (this.CameraManager.Viewpoint == Viewpoint.Perspective || this.GameState.InMap)
      {
        this.CameraManager.OriginalDirection = Vector3.Transform(this.CameraManager.OriginalDirection, Quaternion.CreateFromAxisAngle(Vector3.Up, -1.570796f));
        if (!this.GameState.InMenuCube && !this.GameState.InMap)
          this.EmitLeft();
      }
      else if (this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, -1), (flag ? 2f : 1f) * Math.Abs(1f / this.CollisionManager.GravityFactor)) && !flag)
        this.EmitLeft();
      if (this.LevelManager.NodeType != LevelNodeType.Lesser || !(this.PlayerManager.AirTime != TimeSpan.Zero))
        return;
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      playerManager.Velocity = vector3;
    }

    private void RotateViewRight()
    {
      bool flag = this.PlayerManager.Action == ActionType.GrabTombstone;
      if (this.CameraManager.Viewpoint == Viewpoint.Perspective || this.GameState.InMap)
      {
        this.CameraManager.OriginalDirection = Vector3.Transform(this.CameraManager.OriginalDirection, Quaternion.CreateFromAxisAngle(Vector3.Up, 1.570796f));
        if (!this.GameState.InMenuCube && !this.GameState.InMap)
          this.EmitRight();
      }
      else if (this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, 1), (flag ? 2f : 1f) * Math.Abs(1f / this.CollisionManager.GravityFactor)) && !flag)
        this.EmitRight();
      if (this.LevelManager.NodeType != LevelNodeType.Lesser || !(this.PlayerManager.AirTime != TimeSpan.Zero))
        return;
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      playerManager.Velocity = vector3;
    }

    private void EmitLeft()
    {
      if (Fez.LongScreenshot)
        return;
      if ((double) this.CollisionManager.GravityFactor == 1.0)
        SoundEffectExtensions.Emit(this.swooshLeft);
      else
        SoundEffectExtensions.Emit(this.slowSwooshLeft);
    }

    private void EmitRight()
    {
      if (Fez.LongScreenshot)
        return;
      if ((double) this.CollisionManager.GravityFactor == 1.0)
        SoundEffectExtensions.Emit(this.swooshRight);
      else
        SoundEffectExtensions.Emit(this.slowSwooshRight);
    }

    private void RotateTo(Viewpoint view)
    {
      if (Math.Abs(FezMath.GetDistance(this.CameraManager.Viewpoint, view)) > 1)
        this.EmitRight();
      this.CameraManager.ChangeViewpoint(view);
    }
  }
}
