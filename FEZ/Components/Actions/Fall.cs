// Type: FezGame.Components.Actions.Fall
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class Fall : PlayerAction
  {
    private static readonly TimeSpan DoubleJumpTime = TimeSpan.FromSeconds(0.1);
    private const float MaxVelocity = 5.093625f;
    public const float AirControl = 0.15f;
    public const float Gravity = 3.15f;
    private SoundEffect sFall;
    private SoundEmitter eFall;

    static Fall()
    {
    }

    public Fall(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.sFall = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/FallThroughAir");
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      if (FezMath.AlmostEqual(this.PlayerManager.Velocity.Y, 0.0f))
      {
        if (this.eFall != null && !this.eFall.Dead)
        {
          this.eFall.FadeOutAndDie(0.1f);
          this.eFall = (SoundEmitter) null;
        }
      }
      else
      {
        if (this.eFall == null || this.eFall.Dead)
          this.eFall = SoundEffectExtensions.EmitAt(this.sFall, this.PlayerManager.Position, true, 0.0f, 0.0f);
        this.eFall.Position = this.PlayerManager.Position;
        this.eFall.VolumeFactor = Easing.EaseIn((double) FezMath.Saturate((float) (-(double) this.PlayerManager.Velocity.Y / 0.400000005960464)), EasingType.Quadratic);
      }
      base.Update(gameTime);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      this.PlayerManager.AirTime += elapsed;
      bool flag1 = (double) this.CollisionManager.GravityFactor < 0.0;
      Vector3 vector3_1 = (float) (3.15000009536743 * (double) this.CollisionManager.GravityFactor * 0.150000005960464) * (float) elapsed.TotalSeconds * -Vector3.UnitY;
      if (this.PlayerManager.Action == ActionType.Suffering)
        vector3_1 /= 2f;
      IPlayerManager playerManager1 = this.PlayerManager;
      Vector3 vector3_2 = playerManager1.Velocity + vector3_1;
      playerManager1.Velocity = vector3_2;
      bool flag2 = this.PlayerManager.CarriedInstance != null;
      if (!this.PlayerManager.Grounded && this.PlayerManager.Action != ActionType.Suffering)
      {
        float num1 = this.InputManager.Movement.X;
        IPlayerManager playerManager2 = this.PlayerManager;
        Vector3 vector3_3 = playerManager2.Velocity + Vector3.Transform(Vector3.UnitX * num1, this.CameraManager.Rotation) * 0.15f * 4.7f * (float) elapsed.TotalSeconds * 0.15f;
        playerManager2.Velocity = vector3_3;
        if ((flag1 ? ((double) this.PlayerManager.Velocity.Y > 0.0 ? 1 : 0) : ((double) this.PlayerManager.Velocity.Y < 0.0 ? 1 : 0)) != 0)
        {
          IPlayerManager playerManager3 = this.PlayerManager;
          int num2 = playerManager3.CanDoubleJump & this.PlayerManager.AirTime < Fall.DoubleJumpTime ? 1 : 0;
          playerManager3.CanDoubleJump = num2 != 0;
        }
      }
      else
      {
        this.PlayerManager.CanDoubleJump = true;
        this.PlayerManager.AirTime = TimeSpan.Zero;
      }
      if (!this.PlayerManager.Grounded && ((flag1 ? ((double) this.PlayerManager.Velocity.Y > 0.0 ? 1 : 0) : ((double) this.PlayerManager.Velocity.Y < 0.0 ? 1 : 0)) != 0 && !flag2 && (!ActionTypeExtensions.PreventsFall(this.PlayerManager.Action) && this.PlayerManager.Action != ActionType.Falling)))
        this.PlayerManager.Action = ActionType.Falling;
      if (this.PlayerManager.GroundedVelocity.HasValue)
      {
        float val1 = 5.093625f * (float) elapsed.TotalSeconds;
        float val2 = (float) ((double) val1 / 1.5 * (0.5 + (double) Math.Abs(this.CollisionManager.GravityFactor) * 1.5) / 2.0);
        if (this.PlayerManager.CarriedInstance != null && ActorTypeExtensions.IsHeavy(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type))
        {
          val1 *= 0.7f;
          val2 *= 0.7f;
        }
        Vector3 vector3_3 = new Vector3(Math.Min(val1, Math.Max(Math.Max(Math.Abs(this.PlayerManager.GroundedVelocity.Value.X), val2), Math.Max(Math.Abs(this.PlayerManager.GroundedVelocity.Value.Z), val2))));
        this.PlayerManager.Velocity = new Vector3(MathHelper.Clamp(this.PlayerManager.Velocity.X, -vector3_3.X, vector3_3.X), this.PlayerManager.Velocity.Y, MathHelper.Clamp(this.PlayerManager.Velocity.Z, -vector3_3.Z, vector3_3.Z));
      }
      return this.PlayerManager.Action == ActionType.Falling;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (!ActionTypeExtensions.DefiesGravity(type))
        return !this.PlayerManager.Hidden;
      else
        return false;
    }
  }
}
