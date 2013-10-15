// Type: FezGame.Components.Actions.LowerToCornerLedge
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class LowerToCornerLedge : PlayerAction
  {
    private Vector3 camOrigin;
    private Vector3 playerOrigin;
    private SoundEffect sound;
    private SoundEffect sLowerToLedge;

    public LowerToCornerLedge(Game game)
      : base(game)
    {
      this.UpdateOrder = 3;
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.sound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LedgeGrab");
      this.sLowerToLedge = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LowerToLedge");
    }

    protected override void TestConditions()
    {
      if (this.PlayerManager.Background)
        return;
      switch (this.PlayerManager.Action)
      {
        case ActionType.Teetering:
        case ActionType.IdlePlay:
        case ActionType.IdleSleep:
        case ActionType.IdleLookAround:
        case ActionType.IdleYawn:
        case ActionType.Idle:
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Sliding:
        case ActionType.Landing:
          if (!this.PlayerManager.Grounded || this.InputManager.Down != FezButtonState.Pressed)
            break;
          TrileInstance trileInstance1 = this.PlayerManager.Ground.NearLow;
          TrileInstance trileInstance2 = this.PlayerManager.Ground.FarHigh;
          Trile trile = trileInstance1 == null ? (Trile) null : trileInstance1.Trile;
          Vector3 vector3 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
          TrileInstance trileInstance3 = this.PlayerManager.Ground.NearLow ?? this.PlayerManager.Ground.FarHigh;
          if (BoxCollisionResultExtensions.AnyHit(this.CollisionManager.CollideEdge(trileInstance3.Center + trileInstance3.TransformedSize * (Vector3.UnitY * 0.498f + vector3 * 0.5f), Vector3.Down * (float) Math.Sign(this.CollisionManager.GravityFactor), this.PlayerManager.Size * FezMath.XZMask / 2f, Direction2D.Vertical)) || trileInstance1 == null || (trileInstance1.GetRotatedFace(this.CameraManager.VisibleOrientation) == CollisionType.None || trile.ActorSettings.Type == ActorType.Ladder) || (trileInstance1 == trileInstance2 || trileInstance2 != null && trileInstance2.GetRotatedFace(this.CameraManager.VisibleOrientation) != CollisionType.None))
            break;
          TrileInstance trileInstance4 = this.LevelManager.ActualInstanceAt(trileInstance1.Position + vector3 + new Vector3(0.5f));
          TrileInstance trileInstance5 = this.LevelManager.NearestTrile(trileInstance1.Position + vector3 + new Vector3(0.5f)).Deep;
          if (trileInstance5 != null && trileInstance5.Enabled && trileInstance5.GetRotatedFace(this.CameraManager.VisibleOrientation) != CollisionType.None || trileInstance4 != null && trileInstance4.Enabled && (!trileInstance4.Trile.Immaterial && trileInstance4.Trile.ActorSettings.Type != ActorType.Vine))
            break;
          this.WalkTo.Destination = new Func<Vector3>(this.GetDestination);
          this.WalkTo.NextAction = ActionType.LowerToCornerLedge;
          this.PlayerManager.Action = ActionType.WalkingTo;
          this.PlayerManager.HeldInstance = trileInstance1;
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      this.PlayerManager.Velocity = Vector3.Zero;
      this.camOrigin = this.CameraManager.Center;
      SoundEffectExtensions.EmitAt(this.sLowerToLedge, this.PlayerManager.Position);
      Waiters.Wait(0.579999983310699, (Action) (() => SoundEffectExtensions.EmitAt(this.sound, this.PlayerManager.Position)));
    }

    private Vector3 GetDestination()
    {
      Vector3 vector3_1 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
      if (this.PlayerManager.Action != ActionType.LowerToCornerLedge)
        return this.PlayerManager.HeldInstance.Center + (vector3_1 + Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor)) * this.PlayerManager.HeldInstance.TransformedSize / 2f + -5.0 / 16.0 * vector3_1;
      this.playerOrigin = this.PlayerManager.Position;
      Vector3 vector3_2 = this.PlayerManager.HeldInstance.Center + (vector3_1 + Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor)) * this.PlayerManager.HeldInstance.TransformedSize / 2f;
      this.PlayerManager.SplitUpCubeCollectorOffset = this.playerOrigin - vector3_2;
      return vector3_2;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Action != ActionType.LowerToCornerLedge)
        return false;
      Vector3 vector3_1 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
      float num = (float) (4.0 * (this.LevelManager.Descending ? -1.0 : 1.0)) / this.CameraManager.PixelsPerTrixel;
      Vector3 vector3_2 = this.PlayerManager.HeldInstance.Center + (vector3_1 + Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor)) * this.PlayerManager.HeldInstance.TransformedSize / 2f + num * Vector3.UnitY;
      if (!this.CameraManager.StickyCam && !this.CameraManager.Constrained)
        this.CameraManager.Center = Vector3.Lerp(this.camOrigin, vector3_2, this.PlayerManager.Animation.Timing.NormalizedStep);
      this.PlayerManager.Position = this.PlayerManager.HeldInstance.Center + (vector3_1 + Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor)) * this.PlayerManager.HeldInstance.TransformedSize / 2f;
      this.PlayerManager.SplitUpCubeCollectorOffset = (this.playerOrigin - this.PlayerManager.Position) * (1f - this.PlayerManager.Animation.Timing.NormalizedStep);
      if (this.PlayerManager.Animation.Timing.Ended)
      {
        this.PlayerManager.LookingDirection = FezMath.GetOpposite(this.PlayerManager.LookingDirection);
        this.PlayerManager.SplitUpCubeCollectorOffset = Vector3.Zero;
        this.PlayerManager.Action = ActionType.GrabCornerLedge;
      }
      this.PlayerManager.Animation.Timing.Update(elapsed, 1.25f);
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.LowerToCornerLedge;
    }
  }
}
