// Type: FezGame.Components.Actions.DropDown
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FezGame.Components.Actions
{
  public class DropDown : PlayerAction
  {
    public const float DroppingSpeed = 0.05f;
    private SoundEffect dropLedgeSound;
    private SoundEffect dropVineSound;
    private SoundEffect dropLadderSound;

    public DropDown(Game game)
      : base(game)
    {
      this.UpdateOrder = 2;
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.dropLedgeSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LedgeDrop");
      this.dropVineSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/DropFromVine");
      this.dropLadderSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/DropFromLadder");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.FrontClimbingLadder:
        case ActionType.BackClimbingLadder:
        case ActionType.SideClimbingLadder:
        case ActionType.FrontClimbingVine:
        case ActionType.SideClimbingVine:
        case ActionType.BackClimbingVine:
        case ActionType.GrabCornerLedge:
        case ActionType.GrabLedgeFront:
        case ActionType.GrabLedgeBack:
        case ActionType.LowerToLedge:
        case ActionType.ToCornerFront:
        case ActionType.ToCornerBack:
        case ActionType.FromCornerBack:
          if ((this.InputManager.Jump != FezButtonState.Pressed || !FezButtonStateExtensions.IsDown(this.InputManager.Down)) && (!ActionTypeExtensions.IsOnLedge(this.PlayerManager.Action) || this.InputManager.Down != FezButtonState.Pressed) || !FezMath.AlmostEqual(this.InputManager.Movement.X, 0.0f))
            break;
          this.PlayerManager.HeldInstance = (TrileInstance) null;
          this.PlayerManager.Action = ActionType.Dropping;
          this.PlayerManager.CanDoubleJump = false;
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      if (ActionTypeExtensions.IsClimbingLadder(this.PlayerManager.LastAction))
        SoundEffectExtensions.EmitAt(this.dropLadderSound, this.PlayerManager.Position);
      else if (ActionTypeExtensions.IsClimbingVine(this.PlayerManager.LastAction))
        SoundEffectExtensions.EmitAt(this.dropVineSound, this.PlayerManager.Position);
      else if (ActionTypeExtensions.IsOnLedge(this.PlayerManager.LastAction))
      {
        SoundEffectExtensions.EmitAt(this.dropLedgeSound, this.PlayerManager.Position);
        this.GomezService.OnDropLedge();
        Vector3 position = this.PlayerManager.Position;
        if (this.PlayerManager.LastAction == ActionType.GrabCornerLedge || this.PlayerManager.LastAction == ActionType.LowerToCornerLedge)
          this.PlayerManager.Position += FezMath.RightVector(this.CameraManager.Viewpoint) * (float) -FezMath.Sign(this.PlayerManager.LookingDirection) * 0.5f;
        this.PhysicsManager.DetermineInBackground((IPhysicsEntity) this.PlayerManager, true, false, false);
        this.PlayerManager.Position = position;
      }
      if (this.PlayerManager.Grounded)
      {
        this.PlayerManager.Position -= Vector3.UnitY * 0.01f;
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3 = playerManager.Velocity - 0.0075f * Vector3.UnitY;
        playerManager.Velocity = vector3;
      }
      else
        this.PlayerManager.Velocity = Vector3.Zero;
      if (this.PlayerManager.LastAction != ActionType.GrabCornerLedge)
        return;
      this.PlayerManager.Position += FezMath.RightVector(this.CameraManager.Viewpoint) * (float) -FezMath.Sign(this.PlayerManager.LookingDirection) * (15.0 / 32.0);
      this.PlayerManager.ForceOverlapsDetermination();
      NearestTriles nearestTriles = this.LevelManager.NearestTrile(this.PlayerManager.Position - 1.0 / 500.0 * Vector3.UnitY);
      if (nearestTriles.Surface == null || nearestTriles.Surface.Trile.ActorSettings.Type != ActorType.Vine)
        return;
      this.PlayerManager.Action = ActionType.SideClimbingVine;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Dropping;
    }
  }
}
