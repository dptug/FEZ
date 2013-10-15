// Type: FezGame.Components.Actions.Suffer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  public class Suffer : PlayerAction
  {
    private static readonly TimeSpan HurtTime = TimeSpan.FromSeconds(1.0);
    private const float RepelStrength = 0.0625f;
    private TimeSpan sinceHurt;
    private bool causedByHurtActor;
    private bool doneFor;
    private ScreenFade fade;

    static Suffer()
    {
    }

    public Suffer(Game game)
      : base(game)
    {
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.Dying:
          break;
        case ActionType.Suffering:
          break;
        case ActionType.SuckedIn:
          break;
        default:
          bool flag = false;
          foreach (PointCollision pointCollision in this.PlayerManager.CornerCollision)
          {
            flag = ((flag ? 1 : 0) | (pointCollision.Instances.Surface == null ? 0 : (pointCollision.Instances.Surface.Trile.ActorSettings.Type == ActorType.Hurt ? 1 : 0))) != 0;
            if (flag)
              break;
          }
          if (!flag)
            break;
          this.PlayerManager.Action = ActionType.Suffering;
          this.causedByHurtActor = true;
          this.doneFor = (double) this.PlayerManager.RespawnPosition.Y < (double) this.LevelManager.WaterHeight - 0.25;
          this.fade = (ScreenFade) null;
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      if (!this.PlayerManager.CanControl)
        return;
      if (this.PlayerManager.HeldInstance != null)
      {
        this.PlayerManager.HeldInstance = (TrileInstance) null;
        this.PlayerManager.Action = ActionType.Idle;
        this.PlayerManager.Action = ActionType.Suffering;
      }
      this.PlayerManager.CarriedInstance = (TrileInstance) null;
      if (!this.causedByHurtActor)
        this.PlayerManager.Velocity = Vector3.Zero;
      else
        this.PlayerManager.Velocity = 1.0 / 16.0 * (FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(FezMath.GetOpposite(this.PlayerManager.LookingDirection)) + Vector3.UnitY);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (!this.PlayerManager.CanControl)
        return true;
      if (this.fade == null && this.sinceHurt.TotalSeconds > (this.doneFor ? 1.25 : 1.0))
      {
        this.sinceHurt = TimeSpan.Zero;
        this.causedByHurtActor = false;
        if (this.doneFor)
        {
          this.fade = new ScreenFade(ServiceHelper.Game)
          {
            FromColor = ColorEx.TransparentBlack,
            ToColor = Color.Black,
            Duration = 1f
          };
          ServiceHelper.AddComponent((IGameComponent) this.fade);
          this.fade.Faded += new Action(this.Respawn);
        }
        else
          this.PlayerManager.Action = ActionType.Idle;
      }
      else
      {
        this.sinceHurt += elapsed;
        this.PlayerManager.BlinkSpeed = Easing.EaseIn(this.sinceHurt.TotalSeconds / 1.25, EasingType.Cubic) * 1.5f;
      }
      return true;
    }

    private void Respawn()
    {
      ServiceHelper.AddComponent((IGameComponent) new ScreenFade(ServiceHelper.Game)
      {
        FromColor = Color.Black,
        ToColor = ColorEx.TransparentBlack,
        Duration = 1.5f
      });
      this.GameState.LoadSaveFile((Action) (() =>
      {
        this.GameState.Loading = true;
        this.LevelManager.ChangeLevel(this.LevelManager.Name);
        this.GameState.ScheduleLoadEnd = true;
        this.PlayerManager.RespawnAtCheckpoint();
        this.LevelMaterializer.ForceCull();
      }));
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Suffering;
    }
  }
}
