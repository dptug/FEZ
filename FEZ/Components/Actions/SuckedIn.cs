// Type: FezGame.Components.Actions.SuckedIn
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
  public class SuckedIn : PlayerAction
  {
    private SoundEffect suckedSound;

    public SuckedIn(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.suckedSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/SuckedIn");
    }

    protected override void TestConditions()
    {
      if (this.PlayerManager.Action == ActionType.SuckedIn || ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action) || (this.PlayerManager.Action == ActionType.OpeningTreasure || this.PlayerManager.Action == ActionType.FindingTreasure))
        return;
      foreach (Volume volume in this.PlayerManager.CurrentVolumes)
      {
        if (volume.ActorSettings != null && volume.ActorSettings.IsBlackHole)
        {
          Vector3 vector3_1 = volume.To - volume.From;
          Vector3 vector3_2 = (volume.From + volume.To) / 2f - vector3_1 / 2f * FezMath.ForwardVector(this.CameraManager.Viewpoint);
          this.PlayerManager.Action = ActionType.SuckedIn;
          this.PlayerManager.Position = this.PlayerManager.Position * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint) + vector3_2 * FezMath.DepthMask(this.CameraManager.Viewpoint) + -0.25f * FezMath.ForwardVector(this.CameraManager.Viewpoint);
          volume.ActorSettings.Sucking = true;
          break;
        }
      }
    }

    protected override void Begin()
    {
      base.Begin();
      this.PlayerManager.LookingDirection = FezMath.GetOpposite(this.PlayerManager.LookingDirection);
      this.PlayerManager.CarriedInstance = (TrileInstance) null;
      this.PlayerManager.Action = ActionType.SuckedIn;
      this.PlayerManager.Ground = new MultipleHits<TrileInstance>();
      SoundEffectExtensions.EmitAt(this.suckedSound, this.PlayerManager.Position);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Animation.Timing.Ended)
        this.PlayerManager.Respawn();
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.SuckedIn;
    }
  }
}
