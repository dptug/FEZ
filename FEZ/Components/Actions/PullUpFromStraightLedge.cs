// Type: FezGame.Components.Actions.PullUpFromStraightLedge
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class PullUpFromStraightLedge : PlayerAction
  {
    private Vector3 camOrigin;
    private SoundEffect pullSound;
    private SoundEffect landSound;

    public PullUpFromStraightLedge(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.pullSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/StraightLedgeHoist");
      this.landSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LedgeLand");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.GrabLedgeFront:
        case ActionType.GrabLedgeBack:
          if ((this.InputManager.Jump != FezButtonState.Pressed || FezButtonStateExtensions.IsDown(this.InputManager.Down)) && this.InputManager.Up != FezButtonState.Pressed && (this.InputManager.Up != FezButtonState.Down || !this.PlayerManager.Animation.Timing.Ended) || this.LevelManager.NearestTrile(this.PlayerManager.HeldInstance.Center).Deep == null)
            break;
          this.PlayerManager.Action = ActionTypeExtensions.FacesBack(this.PlayerManager.Action) ? ActionType.PullUpBack : ActionType.PullUpFront;
          break;
        case ActionType.ShimmyFront:
        case ActionType.ShimmyBack:
          if ((this.InputManager.Jump != FezButtonState.Pressed || FezButtonStateExtensions.IsDown(this.InputManager.Down)) && this.InputManager.Up != FezButtonState.Pressed || this.LevelManager.NearestTrile(this.PlayerManager.HeldInstance.Center).Deep == null)
            break;
          this.PlayerManager.Action = ActionTypeExtensions.FacesBack(this.PlayerManager.Action) ? ActionType.PullUpBack : ActionType.PullUpFront;
          break;
      }
    }

    protected override void Begin()
    {
      SoundEffectExtensions.EmitAt(this.pullSound, this.PlayerManager.Position);
      this.camOrigin = this.CameraManager.Center;
      this.PlayerManager.Velocity = Vector3.Zero;
      Waiters.Wait(0.5, (Action) (() => SoundEffectExtensions.EmitAt(this.landSound, this.PlayerManager.Position)));
      this.GomezService.OnHoist();
    }

    protected override bool Act(TimeSpan elapsed)
    {
      Vector3 vector3_1 = this.PlayerManager.Size.Y / 2f * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor);
      if (this.PlayerManager.HeldInstance.PhysicsState != null)
      {
        this.PlayerManager.Position += this.PlayerManager.HeldInstance.PhysicsState.Velocity;
        this.camOrigin += this.PlayerManager.HeldInstance.PhysicsState.Velocity;
      }
      if (!this.CameraManager.StickyCam && !this.CameraManager.Constrained)
        this.CameraManager.Center = Vector3.Lerp(this.camOrigin, this.camOrigin + vector3_1, this.PlayerManager.Animation.Timing.NormalizedStep);
      this.PlayerManager.SplitUpCubeCollectorOffset = vector3_1 * this.PlayerManager.Animation.Timing.NormalizedStep;
      if (!this.PlayerManager.Animation.Timing.Ended)
        return true;
      this.PlayerManager.Position += vector3_1;
      this.PlayerManager.SplitUpCubeCollectorOffset = Vector3.Zero;
      this.PlayerManager.Position += 0.5f * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor);
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3_2 = playerManager.Velocity - Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor);
      playerManager.Velocity = vector3_2;
      this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
      this.PlayerManager.HeldInstance = (TrileInstance) null;
      this.PlayerManager.Action = ActionType.Idle;
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.PullUpFront)
        return type == ActionType.PullUpBack;
      else
        return true;
    }
  }
}
