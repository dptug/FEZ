// Type: FezGame.Components.PushSwitchesHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class PushSwitchesHost : GameComponent
  {
    private readonly Dictionary<int, PushSwitchesHost.SwitchState> trackedSwitches = new Dictionary<int, PushSwitchesHost.SwitchState>();
    private SoundEffect chick;
    private SoundEffect poum;
    private SoundEffect release;

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public PushSwitchesHost(Game game)
      : base(game)
    {
      this.UpdateOrder = -2;
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanging += new Action(this.TrackSwitches);
      this.TrackSwitches();
      this.chick = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/SwitchHalfPress");
      this.poum = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/SwitchPress");
      this.release = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/SwitchHalfRelease");
    }

    private void TrackSwitches()
    {
      this.trackedSwitches.Clear();
      foreach (TrileGroup group in Enumerable.Where<TrileGroup>((IEnumerable<TrileGroup>) this.LevelManager.Groups.Values, (Func<TrileGroup, bool>) (x => ActorTypeExtensions.IsPushSwitch(x.ActorType))))
        this.trackedSwitches.Add(group.Id, new PushSwitchesHost.SwitchState(group, this.chick, this.poum, this.release));
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || (!this.CameraManager.ActionRunning || this.GameState.Loading))
        return;
      foreach (PushSwitchesHost.SwitchState switchState in this.trackedSwitches.Values)
        switchState.Update(gameTime.ElapsedGameTime);
    }

    private class SwitchState
    {
      private static readonly TimeSpan HalfPushDuration = TimeSpan.FromSeconds(0.150000005960464);
      private static readonly TimeSpan FullPushDuration = TimeSpan.FromSeconds(0.449999988079071);
      private static readonly TimeSpan ComeBackDuration = TimeSpan.FromSeconds(0.75);
      private TrileInstance[] Hits = new TrileInstance[5];
      private const float HalfPushHeight = 0.1875f;
      private const float FullPushHeight = 0.8125f;
      private readonly SoundEffect ClickSound;
      private readonly SoundEffect ThudSound;
      private readonly SoundEffect ReleaseSound;
      private readonly TrileGroup Group;
      private readonly float OriginalHeight;
      private readonly PushSwitchesHost.SwitchState.SwitchPermanence Permanence;
      private PushSwitchesHost.SwitchState.SwitchAction action;
      private TimeSpan sinceActionStarted;
      private float lastStep;

      [ServiceDependency]
      public IGameStateManager GameState { private get; set; }

      [ServiceDependency]
      public ILevelManager LevelManager { private get; set; }

      [ServiceDependency]
      public IPlayerManager PlayerManager { private get; set; }

      [ServiceDependency]
      public ICollisionManager CollisionManager { private get; set; }

      [ServiceDependency]
      public ISwitchService SwitchService { private get; set; }

      static SwitchState()
      {
      }

      public SwitchState(TrileGroup group, SoundEffect clickSound, SoundEffect thudSound, SoundEffect releaseSound)
      {
        foreach (TrileInstance instance in Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) group.Triles, (Func<TrileInstance, bool>) (x => x.PhysicsState == null)))
          instance.PhysicsState = new InstancePhysicsState(instance)
          {
            Sticky = true
          };
        this.ClickSound = clickSound;
        this.ThudSound = thudSound;
        this.ReleaseSound = releaseSound;
        this.Permanence = group.ActorType == ActorType.PushSwitchPermanent ? PushSwitchesHost.SwitchState.SwitchPermanence.Permanent : (group.ActorType == ActorType.PushSwitchSticky ? PushSwitchesHost.SwitchState.SwitchPermanence.Sticky : PushSwitchesHost.SwitchState.SwitchPermanence.Volatile);
        this.Group = group;
        this.OriginalHeight = group.Triles[0].Position.Y;
        ServiceHelper.InjectServices((object) this);
        if (this.Permanence != PushSwitchesHost.SwitchState.SwitchPermanence.Permanent || !this.GameState.SaveData.ThisLevel.InactiveGroups.Contains(this.Group.Id))
          return;
        this.action = PushSwitchesHost.SwitchState.SwitchAction.HeldDown;
      }

      public void Update(TimeSpan elapsed)
      {
        bool flag1 = false;
        bool flag2 = false;
        if (this.PlayerManager.Grounded && (this.Group.Triles.Contains(this.PlayerManager.Ground.NearLow) || this.Group.Triles.Contains(this.PlayerManager.Ground.FarHigh)))
        {
          flag1 = true;
          flag2 = this.PlayerManager.CarriedInstance != null;
        }
        foreach (TrileInstance trileInstance1 in this.Group.Triles)
        {
          Vector3 transformedSize = trileInstance1.TransformedSize;
          Vector3 center = trileInstance1.Center;
          Vector3 vector3 = new Vector3(0.0f, 0.5f, 0.0f);
          Array.Clear((Array) this.Hits, 0, 5);
          int index = 0;
          TrileInstance trileInstance2 = this.LevelManager.ActualInstanceAt(center + transformedSize * new Vector3(0.0f, 0.5f, 0.0f) + vector3);
          if (trileInstance2 != null && Array.IndexOf<TrileInstance>(this.Hits, trileInstance2) == -1)
            this.Hits[index++] = trileInstance2;
          TrileInstance trileInstance3 = this.LevelManager.ActualInstanceAt(center + transformedSize * new Vector3(0.5f, 0.5f, 0.0f) + vector3);
          if (trileInstance3 != null && Array.IndexOf<TrileInstance>(this.Hits, trileInstance3) == -1)
            this.Hits[index++] = trileInstance3;
          TrileInstance trileInstance4 = this.LevelManager.ActualInstanceAt(center + transformedSize * new Vector3(-0.5f, 0.5f, 0.0f) + vector3);
          if (trileInstance4 != null && Array.IndexOf<TrileInstance>(this.Hits, trileInstance4) == -1)
            this.Hits[index++] = trileInstance4;
          TrileInstance trileInstance5 = this.LevelManager.ActualInstanceAt(center + transformedSize * new Vector3(0.0f, 0.5f, 0.5f) + vector3);
          if (trileInstance5 != null && Array.IndexOf<TrileInstance>(this.Hits, trileInstance5) == -1)
            this.Hits[index++] = trileInstance5;
          TrileInstance trileInstance6 = this.LevelManager.ActualInstanceAt(center + transformedSize * new Vector3(0.0f, 0.5f, -0.5f) + vector3);
          if (trileInstance6 != null && Array.IndexOf<TrileInstance>(this.Hits, trileInstance6) == -1)
            this.Hits[index] = trileInstance6;
          if (index != 0 || this.Hits[0] != null)
          {
            foreach (TrileInstance trileInstance7 in this.Hits)
            {
              if (trileInstance7 != null && trileInstance7.PhysicsState != null && (trileInstance7.PhysicsState.Ground.NearLow == trileInstance1 || trileInstance7.PhysicsState.Ground.FarHigh == trileInstance1))
              {
                if (ActorTypeExtensions.IsHeavy(trileInstance7.Trile.ActorSettings.Type) || flag1)
                  flag2 = true;
                flag1 = true;
              }
            }
          }
        }
        float num1 = 0.0f;
        PushSwitchesHost.SwitchState.SwitchAction switchAction = this.action;
        switch (this.action)
        {
          case PushSwitchesHost.SwitchState.SwitchAction.Up:
            num1 = 0.0f;
            if (flag1)
            {
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.HalfPush;
              break;
            }
            else
              break;
          case PushSwitchesHost.SwitchState.SwitchAction.HalfPush:
            num1 = (float) ((double) this.sinceActionStarted.Ticks / (double) PushSwitchesHost.SwitchState.HalfPushDuration.Ticks * (3.0 / 16.0));
            if (!flag1)
            {
              SoundEffectExtensions.EmitAt(this.ReleaseSound, Enumerable.First<TrileInstance>((IEnumerable<TrileInstance>) this.Group.Triles).Center);
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.ComingBack;
            }
            if (this.sinceActionStarted.Ticks >= PushSwitchesHost.SwitchState.HalfPushDuration.Ticks)
            {
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.HeldAtHalf;
              SoundEffectExtensions.EmitAt(this.ClickSound, Enumerable.First<TrileInstance>((IEnumerable<TrileInstance>) this.Group.Triles).Center);
              break;
            }
            else
              break;
          case PushSwitchesHost.SwitchState.SwitchAction.HeldAtHalf:
            num1 = 3.0 / 16.0;
            if (!flag1)
            {
              SoundEffectExtensions.EmitAt(this.ReleaseSound, Enumerable.First<TrileInstance>((IEnumerable<TrileInstance>) this.Group.Triles).Center);
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.ComingBack;
            }
            if (flag1 && flag2)
            {
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.FullPush;
              break;
            }
            else
              break;
          case PushSwitchesHost.SwitchState.SwitchAction.FullPush:
            num1 = (float) (3.0 / 16.0 + (double) Easing.EaseIn((double) this.sinceActionStarted.Ticks / (double) PushSwitchesHost.SwitchState.FullPushDuration.Ticks, EasingType.Quadratic) * 0.625);
            if (!flag1 && this.Permanence == PushSwitchesHost.SwitchState.SwitchPermanence.Volatile)
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.ComingBack;
            if (this.sinceActionStarted.Ticks >= PushSwitchesHost.SwitchState.FullPushDuration.Ticks)
            {
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.HeldDown;
              this.SwitchService.OnPush(this.Group.Id);
              SoundEffectExtensions.EmitAt(this.ThudSound, Enumerable.First<TrileInstance>((IEnumerable<TrileInstance>) this.Group.Triles).Center);
              if (this.Permanence == PushSwitchesHost.SwitchState.SwitchPermanence.Permanent)
              {
                this.GameState.SaveData.ThisLevel.InactiveGroups.Add(this.Group.Id);
                break;
              }
              else
                break;
            }
            else
              break;
          case PushSwitchesHost.SwitchState.SwitchAction.HeldDown:
            num1 = 13.0 / 16.0;
            if (!flag1 && this.Permanence == PushSwitchesHost.SwitchState.SwitchPermanence.Volatile)
            {
              this.SwitchService.OnLift(this.Group.Id);
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.ComingBack;
            }
            if (flag1 && !flag2 && this.Permanence == PushSwitchesHost.SwitchState.SwitchPermanence.Volatile)
            {
              this.SwitchService.OnLift(this.Group.Id);
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.BackToHalf;
              break;
            }
            else
              break;
          case PushSwitchesHost.SwitchState.SwitchAction.ComingBack:
            num1 = this.lastStep - (float) this.sinceActionStarted.Ticks / ((float) PushSwitchesHost.SwitchState.ComeBackDuration.Ticks * this.lastStep) * this.lastStep;
            if ((double) this.sinceActionStarted.Ticks >= (double) PushSwitchesHost.SwitchState.ComeBackDuration.Ticks * (double) this.lastStep)
            {
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.Up;
              break;
            }
            else
              break;
          case PushSwitchesHost.SwitchState.SwitchAction.BackToHalf:
            num1 = this.lastStep - (float) this.sinceActionStarted.Ticks / ((float) PushSwitchesHost.SwitchState.ComeBackDuration.Ticks * this.lastStep) * this.lastStep;
            if ((double) this.sinceActionStarted.Ticks >= (double) PushSwitchesHost.SwitchState.ComeBackDuration.Ticks * (double) this.lastStep)
            {
              switchAction = PushSwitchesHost.SwitchState.SwitchAction.Up;
              break;
            }
            else
              break;
        }
        if (switchAction != this.action)
        {
          this.action = switchAction;
          if (switchAction == PushSwitchesHost.SwitchState.SwitchAction.ComingBack || switchAction == PushSwitchesHost.SwitchState.SwitchAction.BackToHalf)
            this.lastStep = num1;
          this.sinceActionStarted = TimeSpan.Zero;
        }
        this.sinceActionStarted += elapsed;
        float num2 = this.Group.Triles[0].Position.Y;
        foreach (TrileInstance trileInstance in this.Group.Triles)
          trileInstance.Position = new Vector3(trileInstance.Position.X, this.OriginalHeight - num1, trileInstance.Position.Z);
        float y = this.Group.Triles[0].Position.Y - num2;
        foreach (TrileInstance instance in this.Group.Triles)
        {
          instance.PhysicsState.Velocity = new Vector3(0.0f, y, 0.0f);
          if ((double) y != 0.0)
            this.LevelManager.UpdateInstance(instance);
        }
      }

      private enum SwitchPermanence
      {
        Volatile,
        Sticky,
        Permanent,
      }

      private enum SwitchAction
      {
        Up,
        HalfPush,
        HeldAtHalf,
        FullPush,
        HeldDown,
        ComingBack,
        BackToHalf,
      }
    }
  }
}
