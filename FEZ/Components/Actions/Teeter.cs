// Type: FezGame.Components.Actions.Teeter
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class Teeter : PlayerAction
  {
    private SoundEffect sBegin;
    private SoundEffect sMouthOpen;
    private SoundEffect sMouthClose;
    private SoundEmitter eLast;
    private int lastFrame;

    public Teeter(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.sBegin = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/TeeterBegin");
      this.sMouthOpen = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/TeeterMouthOpen");
      this.sMouthClose = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/TeeterMouthClose");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.Grabbing:
        case ActionType.Pushing:
        case ActionType.IdlePlay:
        case ActionType.IdleSleep:
        case ActionType.IdleLookAround:
        case ActionType.IdleYawn:
        case ActionType.Idle:
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Dropping:
        case ActionType.Sliding:
          if (this.PlayerManager.PushedInstance != null || this.PlayerManager.CarriedInstance != null || (!this.PlayerManager.Grounded || this.PlayerManager.Ground.FarHigh != null) || (double) this.InputManager.Movement.X != 0.0)
            break;
          Vector3 b = FezMath.SideMask(this.CameraManager.Viewpoint);
          TrileInstance trileInstance = this.PlayerManager.Ground.NearLow;
          float num = Math.Abs(FezMath.Dot(trileInstance.Center, b) - FezMath.Dot(this.PlayerManager.Position, b));
          if ((double) num > 1.0 || (double) num <= 0.449999988079071 || BoxCollisionResultExtensions.AnyHit(this.CollisionManager.CollideEdge(trileInstance.Center, Vector3.Down * (float) Math.Sign(this.CollisionManager.GravityFactor), this.PlayerManager.Size * FezMath.XZMask / 2f, Direction2D.Vertical)))
            break;
          IPlayerManager playerManager = this.PlayerManager;
          Vector3 vector3 = playerManager.Velocity * new Vector3(0.5f, 1f, 0.5f);
          playerManager.Velocity = vector3;
          this.PlayerManager.Action = ActionType.Teetering;
          break;
      }
    }

    protected override void Begin()
    {
      this.lastFrame = -1;
      base.Begin();
    }

    protected override bool Act(TimeSpan elapsed)
    {
      int frame = this.PlayerManager.Animation.Timing.Frame;
      if (this.lastFrame != frame)
      {
        if (frame == 0)
          this.eLast = SoundEffectExtensions.EmitAt(this.sBegin, this.PlayerManager.Position);
        else if (frame == 6)
          this.eLast = SoundEffectExtensions.EmitAt(this.sMouthOpen, this.PlayerManager.Position);
        else if (frame == 9)
          this.eLast = SoundEffectExtensions.EmitAt(this.sMouthClose, this.PlayerManager.Position);
      }
      this.lastFrame = frame;
      if (this.eLast != null && !this.eLast.Dead)
        this.eLast.Position = this.PlayerManager.Position;
      return true;
    }

    protected override void End()
    {
      if (this.eLast != null && !this.eLast.Dead)
      {
        this.eLast.Cue.Stop(false);
        this.eLast = (SoundEmitter) null;
      }
      base.End();
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Teetering;
    }
  }
}
