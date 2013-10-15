// Type: FezGame.Components.Actions.ClimbOverLadder
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class ClimbOverLadder : PlayerAction
  {
    private Vector3 camOrigin;
    private SoundEffect climbOverSound;
    private SoundEffect sLedgeLand;
    private int lastFrame;

    public ClimbOverLadder(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.climbOverSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/ClimbOverLadder");
      this.sLedgeLand = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LedgeLand");
    }

    protected override void Begin()
    {
      this.PlayerManager.Velocity = Vector3.Zero;
      this.PlayerManager.Position += FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection) * 1f / 16f;
      this.PlayerManager.Position -= Vector3.UnitY * 0.5f / 16f;
      this.PlayerManager.Position *= 16f;
      this.PlayerManager.Position = FezMath.Round(this.PlayerManager.Position);
      this.PlayerManager.Position /= 16f;
      this.PlayerManager.Position -= Vector3.UnitY * 0.5f / 16f;
      this.camOrigin = this.CameraManager.Center;
      SoundEffectExtensions.EmitAt(this.climbOverSound, this.PlayerManager.Position);
      this.lastFrame = -1;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.HeldInstance.PhysicsState != null)
      {
        this.PlayerManager.Velocity = this.PlayerManager.HeldInstance.PhysicsState.Velocity;
        this.camOrigin += this.PlayerManager.HeldInstance.PhysicsState.Velocity;
      }
      Vector3 vector3 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection) * 10f / 16f;
      if (this.lastFrame != this.PlayerManager.Animation.Timing.Frame && this.PlayerManager.Animation.Timing.Frame == 5)
        SoundEffectExtensions.EmitAt(this.sLedgeLand, this.PlayerManager.Position);
      this.lastFrame = this.PlayerManager.Animation.Timing.Frame;
      if (!this.CameraManager.StickyCam)
      {
        Vector3 constrainedCenter = this.CameraManager.ConstrainedCenter;
      }
      if (this.PlayerManager.Animation.Timing.Ended)
      {
        this.PlayerManager.HeldInstance = (TrileInstance) null;
        this.PlayerManager.Action = ActionType.Idle;
        this.PlayerManager.Position += vector3;
        Vector3 position = this.PlayerManager.Position;
        this.PlayerManager.Position += 0.5f * Vector3.UnitY;
        this.PlayerManager.Velocity = Vector3.Down;
        this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
        if (!this.PlayerManager.Grounded)
        {
          this.PlayerManager.Velocity = Vector3.Zero;
          this.PlayerManager.Position = position;
        }
      }
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.ClimbOverLadder;
    }
  }
}
