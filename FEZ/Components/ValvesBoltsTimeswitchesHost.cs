// Type: FezGame.Components.ValvesBoltsTimeswitchesHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  public class ValvesBoltsTimeswitchesHost : DrawableGameComponent
  {
    private readonly List<ValvesBoltsTimeswitchesHost.ValveState> TrackedValves = new List<ValvesBoltsTimeswitchesHost.ValveState>();
    private SoundEffect GrabSound;
    private SoundEffect ValveUnscrew;
    private SoundEffect ValveScrew;
    private SoundEffect BoltScrew;
    private SoundEffect BoltUnscrew;
    private SoundEffect TimeSwitchWind;
    private SoundEffect TimeswitchWindBackSound;
    private SoundEffect TimeswitchEndWindBackSound;

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public ValvesBoltsTimeswitchesHost(Game game)
      : base(game)
    {
      this.DrawOrder = 6;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.GrabSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/GrabLever");
      this.ValveUnscrew = this.CMProvider.Global.Load<SoundEffect>("Sounds/Sewer/ValveUnscrew");
      this.ValveScrew = this.CMProvider.Global.Load<SoundEffect>("Sounds/Sewer/ValveScrew");
      this.BoltUnscrew = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/BoltUnscrew");
      this.BoltScrew = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/BoltScrew");
      this.TimeswitchWindBackSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Nature/TimeswitchWindBack");
      this.TimeswitchEndWindBackSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Nature/TimeswitchEndWindBack");
      this.TimeSwitchWind = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/TimeswitchWindUp");
      this.CameraManager.PreViewpointChanged += new Action(this.OnViewpointChanged);
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void OnViewpointChanged()
    {
      foreach (ValvesBoltsTimeswitchesHost.ValveState valveState in this.TrackedValves)
        valveState.TrySpin();
    }

    private void TryInitialize()
    {
      this.TrackedValves.Clear();
      foreach (ArtObjectInstance ao in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
      {
        if (ao.ArtObject.ActorType == ActorType.Valve || ao.ArtObject.ActorType == ActorType.BoltHandle || ao.ArtObject.ActorType == ActorType.Timeswitch)
          this.TrackedValves.Add(new ValvesBoltsTimeswitchesHost.ValveState(this, ao));
      }
      this.Enabled = this.Visible = this.TrackedValves.Count > 0;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Loading || this.EngineState.InMap || (this.EngineState.Paused || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)))
        return;
      float num1 = float.MaxValue;
      ValvesBoltsTimeswitchesHost.ValveState valveState1 = (ValvesBoltsTimeswitchesHost.ValveState) null;
      foreach (ValvesBoltsTimeswitchesHost.ValveState valveState2 in this.TrackedValves)
      {
        if (valveState2.ArtObject.ActorSettings.ShouldMoveToEnd)
          valveState2.MoveToEnd();
        if (valveState2.ArtObject.ActorSettings.ShouldMoveToHeight.HasValue)
          valveState2.MoveToHeight();
        if (valveState2.Update(gameTime.ElapsedGameTime))
        {
          float num2 = FezMath.Dot(valveState2.ArtObject.Position, FezMath.ForwardVector(this.CameraManager.Viewpoint));
          if ((double) num2 < (double) num1)
          {
            valveState1 = valveState2;
            num1 = num2;
          }
        }
      }
      if (valveState1 == null)
        return;
      valveState1.GrabOnto();
    }

    private class ValveState
    {
      private const float SpinTime = 0.75f;
      private readonly ValvesBoltsTimeswitchesHost Host;
      public readonly ArtObjectInstance TimeswitchScrewAo;
      private SpinAction State;
      private TimeSpan SinceChanged;
      private int SpinSign;
      private Vector3 OriginalPlayerPosition;
      private Vector3 OriginalAoPosition;
      private Quaternion OriginalAoRotation;
      private Quaternion OriginalScrewRotation;
      private Vector3[] OriginalGroupTrilePositions;
      private float ScrewHeight;
      private float RewindSpeed;
      private bool MovingToHeight;
      private readonly SoundEmitter eTimeswitchWindBack;
      private readonly bool IsBolt;
      private readonly bool IsTimeswitch;
      private readonly TrileGroup AttachedGroup;
      private readonly Vector3 CenterOffset;
      public readonly ArtObjectInstance ArtObject;

      [ServiceDependency]
      public ISoundManager SoundManager { private get; set; }

      [ServiceDependency]
      public IPhysicsManager PhysicsManager { private get; set; }

      [ServiceDependency]
      public ILevelManager LevelManager { private get; set; }

      [ServiceDependency]
      public IInputManager InputManager { private get; set; }

      [ServiceDependency]
      public IGameCameraManager CameraManager { private get; set; }

      [ServiceDependency]
      public IPlayerManager PlayerManager { private get; set; }

      [ServiceDependency]
      public IGameStateManager GameState { private get; set; }

      [ServiceDependency]
      public IValveService ValveService { private get; set; }

      [ServiceDependency]
      public ITimeswitchService TimeswitchService { private get; set; }

      [ServiceDependency]
      public IDebuggingBag DebuggingBag { private get; set; }

      public ValveState(ValvesBoltsTimeswitchesHost host, ArtObjectInstance ao)
      {
        ServiceHelper.InjectServices((object) this);
        this.Host = host;
        this.ArtObject = ao;
        this.IsBolt = this.ArtObject.ArtObject.ActorType == ActorType.BoltHandle;
        this.IsTimeswitch = this.ArtObject.ArtObject.ActorType == ActorType.Timeswitch;
        BoundingBox boundingBox = new BoundingBox(this.ArtObject.Position - this.ArtObject.ArtObject.Size / 2f, this.ArtObject.Position + this.ArtObject.ArtObject.Size / 2f);
        if (this.ArtObject.ActorSettings.AttachedGroup.HasValue)
          this.AttachedGroup = this.LevelManager.Groups[this.ArtObject.ActorSettings.AttachedGroup.Value];
        if (this.IsTimeswitch)
        {
          this.eTimeswitchWindBack = SoundEffectExtensions.EmitAt(this.Host.TimeswitchWindBackSound, ao.Position, true, true);
          foreach (ArtObjectInstance artObjectInstance in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
          {
            if (artObjectInstance != ao && artObjectInstance.ArtObject.ActorType == ActorType.TimeswitchMovingPart)
            {
              BoundingBox box = new BoundingBox(artObjectInstance.Position - artObjectInstance.ArtObject.Size / 2f, artObjectInstance.Position + artObjectInstance.ArtObject.Size / 2f);
              if (boundingBox.Intersects(box))
              {
                this.TimeswitchScrewAo = artObjectInstance;
                break;
              }
            }
          }
        }
        int num1;
        if (!this.IsBolt && !this.IsTimeswitch && (this.GameState.SaveData.ThisLevel.PivotRotations.TryGetValue(this.ArtObject.Id, out num1) && num1 != 0))
        {
          int num2 = Math.Abs(num1);
          int num3 = Math.Sign(num1);
          for (int index = 0; index < num2; ++index)
            this.ArtObject.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, 1.570796f * (float) num3);
        }
        if (this.IsBolt)
        {
          foreach (TrileInstance instance in this.AttachedGroup.Triles)
            instance.PhysicsState = new InstancePhysicsState(instance);
        }
        foreach (Volume volume in (IEnumerable<Volume>) this.LevelManager.Volumes.Values)
        {
          Vector3 vector3 = FezMath.Abs(volume.To - volume.From);
          if ((double) vector3.X == 3.0 && (double) vector3.Z == 3.0 && ((double) vector3.Y == 1.0 && boundingBox.Contains(volume.BoundingBox) == ContainmentType.Contains))
          {
            this.CenterOffset = (volume.From + volume.To) / 2f - this.ArtObject.Position;
            break;
          }
        }
      }

      public bool Update(TimeSpan elapsed)
      {
        if (this.MovingToHeight)
          return false;
        this.SinceChanged += elapsed;
        Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
        switch (this.State)
        {
          case SpinAction.Idle:
            Vector3 vector = (this.PlayerManager.Position - this.ArtObject.Position - new Vector3(0.0f, 1f, 0.0f) + this.CenterOffset) * vector3_1;
            vector.X += vector.Z;
            Vector3 vector3_2 = FezMath.Abs(vector);
            bool flag = this.IsBolt || this.IsTimeswitch ? (double) vector3_2.X > 0.75 && (double) vector3_2.X < 1.75 && (double) vector3_2.Y < 1.0 : (double) vector3_2.X < 1.0 && (double) vector3_2.Y < 1.0;
            if (this.LevelManager.Flat)
              flag = (double) vector3_2.X < 1.5 && (double) vector3_2.Y < 1.0;
            if (flag && this.PlayerManager.CarriedInstance == null && (this.PlayerManager.Grounded && this.PlayerManager.Action != ActionType.GrabTombstone) && (this.InputManager.GrabThrow == FezButtonState.Pressed && this.PlayerManager.Action != ActionType.ReadingSign && (this.PlayerManager.Action != ActionType.Dying && this.PlayerManager.Action != ActionType.FreeFalling)))
            {
              Vector3 vector3_3 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
              Vector3 vector3_4 = FezMath.DepthMask(this.CameraManager.Viewpoint);
              Vector3 vector3_5 = (this.ArtObject.Position + this.CenterOffset) * vector3_4;
              this.PlayerManager.Position = this.PlayerManager.Position * vector3_1 + vector3_4 * vector3_5 - vector3_3 * 1.5f;
              this.SinceChanged = TimeSpan.Zero;
              return true;
            }
            else if (this.IsTimeswitch && (double) this.ScrewHeight >= 0.0 && (double) this.ScrewHeight <= 2.0)
            {
              float num1 = (double) this.ArtObject.ActorSettings.TimeswitchWindBackSpeed == 0.0 ? 4f : this.ArtObject.ActorSettings.TimeswitchWindBackSpeed;
              float num2 = (float) (elapsed.TotalSeconds / ((double) num1 - 0.25) * 2.0);
              this.RewindSpeed = this.SinceChanged.TotalSeconds < 0.5 ? MathHelper.Lerp(0.0f, num2, (float) this.SinceChanged.TotalSeconds * 2f) : num2;
              float num3 = this.ScrewHeight;
              this.ScrewHeight = MathHelper.Clamp(this.ScrewHeight - this.RewindSpeed, 0.0f, 2f);
              float num4 = num3 - this.ScrewHeight;
              if ((double) this.ScrewHeight == 0.0 && (double) num4 != 0.0)
              {
                SoundEffectExtensions.EmitAt(this.Host.TimeswitchEndWindBackSound, this.ArtObject.Position);
                this.TimeswitchService.OnHitBase(this.ArtObject.Id);
                if (this.eTimeswitchWindBack != null && !this.eTimeswitchWindBack.Dead && this.eTimeswitchWindBack.Cue.State == SoundState.Playing)
                  this.eTimeswitchWindBack.Cue.Pause();
              }
              else if ((double) num4 != 0.0)
              {
                if (this.eTimeswitchWindBack != null && !this.eTimeswitchWindBack.Dead && this.eTimeswitchWindBack.Cue.State == SoundState.Paused)
                  this.eTimeswitchWindBack.Cue.Resume();
                this.eTimeswitchWindBack.VolumeFactor = FezMath.Saturate(num4 * 20f * this.ArtObject.ActorSettings.TimeswitchWindBackSpeed);
              }
              else
              {
                this.eTimeswitchWindBack.VolumeFactor = 0.0f;
                if (this.eTimeswitchWindBack != null && !this.eTimeswitchWindBack.Dead && this.eTimeswitchWindBack.Cue.State == SoundState.Playing)
                  this.eTimeswitchWindBack.Cue.Pause();
              }
              this.TimeswitchScrewAo.Position -= Vector3.UnitY * num4;
              this.TimeswitchScrewAo.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) ((double) num4 * 1.57079637050629 * -4.0));
              break;
            }
            else
              break;
          case SpinAction.Spinning:
            float num5 = (float) FezMath.Saturate(this.SinceChanged.TotalSeconds / 0.75);
            Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle(Vector3.UnitY, num5 * 1.570796f * (float) this.SpinSign);
            this.ArtObject.Rotation = this.OriginalAoRotation * fromAxisAngle;
            this.PlayerManager.Position = Vector3.Transform(this.OriginalPlayerPosition - this.ArtObject.Position, fromAxisAngle) + this.ArtObject.Position;
            if (this.IsBolt)
            {
              Vector3 vector3_3 = num5 * (this.SpinSign == 1 ? 1f : -1f) * Vector3.Up;
              this.ArtObject.Position = this.OriginalAoPosition + vector3_3;
              int num1 = 0;
              foreach (TrileInstance instance in this.AttachedGroup.Triles)
              {
                instance.Position = this.OriginalGroupTrilePositions[num1++] + vector3_3;
                this.LevelManager.UpdateInstance(instance);
              }
              this.PlayerManager.Position += vector3_3;
            }
            if (this.IsTimeswitch)
            {
              float num1 = num5;
              if (this.SpinSign == -1 && (double) this.ScrewHeight <= 0.5)
                num1 = Math.Min(this.ScrewHeight, num5 / 2f) * 2f;
              else if (this.SpinSign == 1 && (double) this.ScrewHeight >= 1.5)
                num1 = Math.Min(2f - this.ScrewHeight, num5 / 2f) * 2f;
              this.TimeswitchScrewAo.Position = this.OriginalAoPosition + num1 * (this.SpinSign == 1 ? 1f : -1f) * Vector3.Up / 2f;
              this.TimeswitchScrewAo.Rotation = this.OriginalScrewRotation * Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) ((double) num1 * 1.57079637050629 * (double) this.SpinSign * 2.0));
            }
            if (this.SinceChanged.TotalSeconds >= 0.75)
            {
              this.PlayerManager.Position += 0.5f * Vector3.UnitY;
              IPlayerManager playerManager = this.PlayerManager;
              Vector3 vector3_3 = playerManager.Velocity - Vector3.UnitY;
              playerManager.Velocity = vector3_3;
              this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
              FezMath.ForwardVector(this.CameraManager.Viewpoint);
              Vector3 vector3_4 = (this.ArtObject.Position + this.CenterOffset) * FezMath.DepthMask(this.CameraManager.Viewpoint);
              this.ScrewHeight = MathHelper.Clamp(this.ScrewHeight + (float) this.SpinSign / 2f, 0.0f, 2f);
              if ((double) this.ScrewHeight == 0.0 && this.SpinSign == -1)
                this.TimeswitchService.OnHitBase(this.ArtObject.Id);
              this.PlayerManager.Action = ActionType.GrabTombstone;
              this.SinceChanged -= TimeSpan.FromSeconds(0.75);
              this.State = SpinAction.Grabbed;
              break;
            }
            else
              break;
          case SpinAction.Grabbed:
            this.RewindSpeed = 0.0f;
            if (this.PlayerManager.Action != ActionType.GrabTombstone)
              this.State = SpinAction.Idle;
            if (this.IsTimeswitch)
            {
              this.SinceChanged = TimeSpan.Zero;
              this.eTimeswitchWindBack.VolumeFactor = 0.0f;
              if (this.eTimeswitchWindBack != null && !this.eTimeswitchWindBack.Dead && this.eTimeswitchWindBack.Cue.State == SoundState.Playing)
              {
                this.eTimeswitchWindBack.Cue.Pause();
                break;
              }
              else
                break;
            }
            else
              break;
        }
        return false;
      }

      public void MoveToHeight()
      {
        if (this.MovingToHeight)
          return;
        this.MovingToHeight = true;
        float y = this.ArtObject.ActorSettings.ShouldMoveToHeight.Value;
        this.ArtObject.ActorSettings.ShouldMoveToHeight = new float?();
        Vector3 vector3 = this.ArtObject.Position + this.CenterOffset;
        Vector3 movement = (new Vector3(0.0f, y, 0.0f) - vector3) * Vector3.UnitY + Vector3.UnitY / 2f;
        Vector3 origin = vector3;
        Vector3 destination = vector3 + movement;
        float lastHeight = origin.Y;
        if (this.PlayerManager.Action == ActionType.PivotTombstone || this.PlayerManager.Grounded && this.AttachedGroup.Triles.Contains(this.PlayerManager.Ground.First))
          this.MovingToHeight = false;
        else if ((double) Math.Abs(movement.Y) < 1.0)
        {
          this.MovingToHeight = false;
        }
        else
        {
          IWaiter waiter = Waiters.Interpolate((double) Math.Abs(movement.Y / 2f), (Action<float>) (step =>
          {
            float local_0 = Easing.EaseInOut((double) step, EasingType.Sine);
            this.ArtObject.Position = Vector3.Lerp(origin, destination, local_0);
            this.ArtObject.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, local_0 * 1.570796f * (float) (int) Math.Round((double) movement.Y / 2.0));
            foreach (TrileInstance item_0 in this.AttachedGroup.Triles)
            {
              item_0.Position += Vector3.UnitY * (this.ArtObject.Position.Y - lastHeight);
              item_0.PhysicsState.Velocity = Vector3.UnitY * (this.ArtObject.Position.Y - lastHeight);
              this.LevelManager.UpdateInstance(item_0);
            }
            lastHeight = this.ArtObject.Position.Y;
          }), (Action) (() =>
          {
            this.ArtObject.Position = destination;
            this.ArtObject.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, 1.570796f * (float) (int) Math.Round((double) movement.Y / 2.0));
            foreach (TrileInstance item_1 in this.AttachedGroup.Triles)
              item_1.PhysicsState.Velocity = Vector3.Zero;
            this.MovingToHeight = false;
          }));
          waiter.AutoPause = true;
          waiter.UpdateOrder = -2;
        }
      }

      public void MoveToEnd()
      {
        this.ArtObject.ActorSettings.ShouldMoveToEnd = false;
        Vector3 vector3_1 = new Vector3(2f, 100f, 2f);
        Vector3 vector3_2 = this.ArtObject.Position + this.CenterOffset;
        BoundingBox box = new BoundingBox(vector3_2 - vector3_1, vector3_2 + vector3_1);
        foreach (ArtObjectInstance artObjectInstance in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
        {
          if (artObjectInstance.ArtObject.ActorType == ActorType.BoltNutTop)
          {
            Vector3 vector3_3 = artObjectInstance.Position + Vector3.Up * 3.5f;
            Vector3 vector3_4 = artObjectInstance.ArtObject.Size / 2f + Vector3.Up / 32f;
            if (new BoundingBox(vector3_3 - vector3_4, vector3_3 + vector3_4).Intersects(box))
            {
              Vector3 vector3_5 = artObjectInstance.Position - vector3_2 + Vector3.UnitY / 2f;
              this.ArtObject.Position += vector3_5;
              using (List<TrileInstance>.Enumerator enumerator = this.AttachedGroup.Triles.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  TrileInstance current = enumerator.Current;
                  current.Position += vector3_5;
                  this.LevelManager.UpdateInstance(current);
                }
                break;
              }
            }
          }
        }
      }

      public void GrabOnto()
      {
        this.PlayerManager.Action = ActionType.GrabTombstone;
        Waiters.Wait(0.4, (Func<float, bool>) (_ => this.PlayerManager.Action != ActionType.GrabTombstone), (Action) (() =>
        {
          if (this.PlayerManager.Action != ActionType.GrabTombstone)
            return;
          SoundEffectExtensions.EmitAt(this.Host.GrabSound, this.ArtObject.Position);
          this.State = SpinAction.Grabbed;
        }));
      }

      public void TrySpin()
      {
        if (this.State != SpinAction.Grabbed)
          return;
        if (this.PlayerManager.Action != ActionType.GrabTombstone)
        {
          this.State = SpinAction.Idle;
        }
        else
        {
          if (!this.PlayerManager.Animation.Timing.Ended || this.CameraManager.Viewpoint == Viewpoint.Perspective || this.CameraManager.LastViewpoint == this.CameraManager.Viewpoint)
            return;
          this.SpinSign = FezMath.GetDistance(this.CameraManager.LastViewpoint, this.CameraManager.Viewpoint);
          if (this.IsBolt)
          {
            Vector3 vector3_1 = new Vector3(2f);
            Vector3 vector3_2 = this.ArtObject.Position + this.CenterOffset;
            BoundingBox box = new BoundingBox(vector3_2 - vector3_1, vector3_2 + vector3_1);
            foreach (ArtObjectInstance artObjectInstance in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
            {
              if (artObjectInstance.ArtObject.ActorType == ActorType.BoltNutBottom && this.SpinSign == -1)
              {
                Vector3 vector3_3 = artObjectInstance.ArtObject.Size / 2f + Vector3.Up / 32f;
                if (new BoundingBox(artObjectInstance.Position - vector3_3, artObjectInstance.Position + vector3_3).Intersects(box))
                {
                  this.CameraManager.CancelViewTransition();
                  return;
                }
              }
              else if (artObjectInstance.ArtObject.ActorType == ActorType.BoltNutTop && this.SpinSign == 1)
              {
                Vector3 vector3_3 = artObjectInstance.Position + Vector3.Up * 3.5f;
                Vector3 vector3_4 = artObjectInstance.ArtObject.Size / 2f + Vector3.Up / 32f;
                if (new BoundingBox(vector3_3 - vector3_4, vector3_3 + vector3_4).Intersects(box))
                {
                  this.CameraManager.CancelViewTransition();
                  return;
                }
              }
            }
          }
          if (this.IsTimeswitch && this.SpinSign == -1)
          {
            this.CameraManager.CancelViewTransition();
          }
          else
          {
            if (this.IsBolt)
            {
              if (this.SpinSign == 1)
                SoundEffectExtensions.EmitAt(this.Host.BoltScrew, this.ArtObject.Position);
              else
                SoundEffectExtensions.EmitAt(this.Host.BoltUnscrew, this.ArtObject.Position);
            }
            else if (this.IsTimeswitch)
              SoundEffectExtensions.EmitAt(this.Host.TimeSwitchWind, this.ArtObject.Position);
            else if (this.SpinSign == 1)
              SoundEffectExtensions.EmitAt(this.Host.ValveScrew, this.ArtObject.Position);
            else
              SoundEffectExtensions.EmitAt(this.Host.ValveUnscrew, this.ArtObject.Position);
            int num;
            if (!this.GameState.SaveData.ThisLevel.PivotRotations.TryGetValue(this.ArtObject.Id, out num))
              this.GameState.SaveData.ThisLevel.PivotRotations.Add(this.ArtObject.Id, this.SpinSign);
            else
              this.GameState.SaveData.ThisLevel.PivotRotations[this.ArtObject.Id] = num + this.SpinSign;
            Viewpoint lastViewpoint = this.CameraManager.LastViewpoint;
            Vector3 vector3_1 = FezMath.ScreenSpaceMask(lastViewpoint);
            Vector3 vector3_2 = FezMath.ForwardVector(lastViewpoint);
            Vector3 vector3_3 = FezMath.DepthMask(lastViewpoint);
            Vector3 vector3_4 = (this.ArtObject.Position + this.CenterOffset) * vector3_3;
            this.OriginalPlayerPosition = this.PlayerManager.Position = this.PlayerManager.Position * vector3_1 + vector3_3 * vector3_4 - vector3_2 * 2f;
            this.OriginalAoRotation = this.ArtObject.Rotation;
            this.OriginalAoPosition = this.IsTimeswitch ? this.TimeswitchScrewAo.Position : this.ArtObject.Position;
            if (this.IsTimeswitch)
              this.OriginalScrewRotation = this.TimeswitchScrewAo.Rotation;
            if (this.AttachedGroup != null)
              this.OriginalGroupTrilePositions = Enumerable.ToArray<Vector3>(Enumerable.Select<TrileInstance, Vector3>((IEnumerable<TrileInstance>) this.AttachedGroup.Triles, (Func<TrileInstance, Vector3>) (x => x.Position)));
            this.SinceChanged = TimeSpan.Zero;
            this.State = SpinAction.Spinning;
            this.PlayerManager.Action = ActionType.PivotTombstone;
            if (this.IsTimeswitch)
            {
              if (this.SpinSign != 1 || (double) this.ScrewHeight > 0.0)
                return;
              this.TimeswitchService.OnScrewedOut(this.ArtObject.Id);
            }
            else if (this.SpinSign == -1)
              this.ValveService.OnUnscrew(this.ArtObject.Id);
            else
              this.ValveService.OnScrew(this.ArtObject.Id);
          }
        }
      }
    }
  }
}
