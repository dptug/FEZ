// Type: FezGame.Components.TombstonesHost
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
  public class TombstonesHost : DrawableGameComponent
  {
    private readonly List<TombstonesHost.TombstoneState> TrackedStones = new List<TombstonesHost.TombstoneState>();
    private ArtObjectInstance SkullAo;
    private Vector4[] SkullAttachedTrilesOriginalStates;
    private TrileInstance[] SkullTopLayer;
    private TrileInstance[] SkullAttachedTriles;
    private Quaternion InterpolatedRotation;
    private Quaternion OriginalRotation;
    private bool SkullRotates;
    private bool StopSkullRotations;
    private SoundEffect GrabSound;
    private SoundEffect TurnLeft;
    private SoundEffect TurnRight;
    private SoundEffect sRumble;
    private SoundEmitter eRumble;
    private float lastAngle;

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { get; set; }

    public TombstonesHost(Game game)
      : base(game)
    {
      this.DrawOrder = 6;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.CameraManager.ViewpointChanged += new Action(this.OnViewpointChanged);
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void OnViewpointChanged()
    {
      foreach (TombstonesHost.TombstoneState tombstoneState in this.TrackedStones)
        tombstoneState.TrySpin();
    }

    private void TryInitialize()
    {
      this.TrackedStones.Clear();
      this.GrabSound = (SoundEffect) null;
      this.TurnLeft = (SoundEffect) null;
      this.TurnRight = (SoundEffect) null;
      this.sRumble = (SoundEffect) null;
      this.eRumble = (SoundEmitter) null;
      foreach (ArtObjectInstance ao in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
      {
        if (ao.ArtObject.ActorType == ActorType.Tombstone)
          this.TrackedStones.Add(new TombstonesHost.TombstoneState(this, ao));
      }
      this.SkullAo = Enumerable.SingleOrDefault<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "GIANT_SKULLAO"));
      this.Enabled = this.Visible = this.TrackedStones.Count > 0 && this.SkullAo != null;
      if (!this.Enabled)
        return;
      int index = this.SkullAo.ActorSettings.AttachedGroup.Value;
      this.SkullTopLayer = Enumerable.ToArray<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Groups[index].Triles, (Func<TrileInstance, bool>) (x => x.Trile.Faces[FaceOrientation.Back] == CollisionType.TopOnly)));
      this.SkullAttachedTriles = Enumerable.ToArray<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Groups[index].Triles, (Func<TrileInstance, bool>) (x => x.Trile.Immaterial)));
      this.SkullAttachedTrilesOriginalStates = Enumerable.ToArray<Vector4>(Enumerable.Select<TrileInstance, Vector4>((IEnumerable<TrileInstance>) this.SkullAttachedTriles, (Func<TrileInstance, Vector4>) (x => new Vector4(x.Position, x.Phi))));
      this.GrabSound = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/GrabLever");
      this.TurnLeft = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Graveyard/TombRotateLeft");
      this.TurnRight = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Graveyard/TombRotateRight");
      this.sRumble = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/Rumble");
      this.eRumble = SoundEffectExtensions.Emit(this.sRumble, true, true);
      this.SkullRotates = Enumerable.Count<TombstonesHost.TombstoneState>((IEnumerable<TombstonesHost.TombstoneState>) this.TrackedStones, (Func<TombstonesHost.TombstoneState, bool>) (x => x.LastViewpoint == this.TrackedStones[0].LastViewpoint)) < 4;
      this.OriginalRotation = this.SkullAo.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitY, 1.570796f);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Loading || this.EngineState.InMap || (this.EngineState.Paused || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)))
        return;
      float num1 = float.MaxValue;
      TombstonesHost.TombstoneState tombstoneState1 = (TombstonesHost.TombstoneState) null;
      foreach (TombstonesHost.TombstoneState tombstoneState2 in this.TrackedStones)
      {
        if (tombstoneState2.Update(gameTime.ElapsedGameTime))
        {
          float num2 = FezMath.Dot(tombstoneState2.ArtObject.Position, FezMath.ForwardVector(this.CameraManager.Viewpoint));
          if ((double) num2 < (double) num1)
          {
            tombstoneState1 = tombstoneState2;
            num1 = num2;
          }
        }
      }
      if (tombstoneState1 != null)
        tombstoneState1.GrabOnto();
      if (!this.SkullRotates)
        return;
      this.RotateSkull();
    }

    private void RotateSkull()
    {
      this.InterpolatedRotation = Quaternion.Slerp(this.InterpolatedRotation, this.StopSkullRotations ? this.OriginalRotation : this.CameraManager.Rotation, 0.05f);
      if (this.InterpolatedRotation == this.CameraManager.Rotation)
      {
        if (this.eRumble.Cue.State != SoundState.Paused)
          this.eRumble.Cue.Pause();
        if (!this.StopSkullRotations)
          return;
        this.SkullRotates = false;
        this.StopSkullRotations = false;
      }
      else
      {
        if (FezMath.AlmostEqual(this.InterpolatedRotation, this.CameraManager.Rotation) || FezMath.AlmostEqual(-this.InterpolatedRotation, this.CameraManager.Rotation))
          this.InterpolatedRotation = this.CameraManager.Rotation;
        this.SkullAo.Rotation = this.InterpolatedRotation * Quaternion.CreateFromAxisAngle(Vector3.UnitY, -1.570796f);
        Vector3 axis;
        float angle1;
        TombstonesHost.ToAxisAngle(ref this.InterpolatedRotation, out axis, out angle1);
        float angle2 = this.lastAngle - angle1;
        if ((double) Math.Abs(angle2) > 0.100000001490116)
        {
          this.lastAngle = angle1;
        }
        else
        {
          for (int index = 0; index < this.SkullAttachedTriles.Length; ++index)
          {
            TrileInstance instance = this.SkullAttachedTriles[index];
            Vector4 vector = this.SkullAttachedTrilesOriginalStates[index];
            instance.Position = Vector3.Transform(FezMath.XYZ(vector) + new Vector3(0.5f) - this.SkullAo.Position, this.InterpolatedRotation) + this.SkullAo.Position - new Vector3(0.5f);
            instance.Phi = FezMath.WrapAngle(vector.W + ((double) axis.Y > 0.0 ? -1f : 1f) * angle1);
            this.LevelMaterializer.GetTrileMaterializer(instance.Trile).UpdateInstance(instance);
          }
          if (Enumerable.Contains<TrileInstance>((IEnumerable<TrileInstance>) this.SkullTopLayer, this.PlayerManager.Ground.First))
          {
            Vector3 position = this.PlayerManager.Position;
            this.PlayerManager.Position = Vector3.Transform(this.PlayerManager.Position - this.SkullAo.Position, Quaternion.CreateFromAxisAngle(axis, angle2)) + this.SkullAo.Position;
            IGameCameraManager cameraManager = this.CameraManager;
            Vector3 vector3 = cameraManager.Center + this.PlayerManager.Position - position;
            cameraManager.Center = vector3;
          }
          if ((double) Math.Abs(axis.Y) > 0.5)
          {
            float num = angle2 * 5f;
            IGameCameraManager cameraManager = this.CameraManager;
            Vector3 vector3 = cameraManager.InterpolatedCenter + new Vector3(RandomHelper.Between(-(double) num, (double) num), RandomHelper.Between(-(double) num, (double) num), RandomHelper.Between(-(double) num, (double) num));
            cameraManager.InterpolatedCenter = vector3;
            if (this.eRumble.Cue.State == SoundState.Paused)
              this.eRumble.Cue.Resume();
            this.eRumble.VolumeFactor = FezMath.Saturate(Math.Abs(num) * 25f);
          }
          if (this.InterpolatedRotation == this.CameraManager.Rotation)
            this.RotateSkullTriles();
          this.lastAngle = angle1;
        }
      }
    }

    private void RotateSkullTriles()
    {
      foreach (TrileInstance instance in this.SkullAttachedTriles)
        this.LevelManager.UpdateInstance(instance);
    }

    private static void ToAxisAngle(ref Quaternion q, out Vector3 axis, out float angle)
    {
      angle = (float) Math.Acos((double) MathHelper.Clamp(q.W, -1f, 1f));
      float num1 = (float) Math.Sin((double) angle);
      float num2 = (float) (1.0 / ((double) num1 == 0.0 ? 1.0 : (double) num1));
      angle *= 2f;
      axis = new Vector3(-q.X * num2, -q.Y * num2, -q.Z * num2);
    }

    public override void Draw(GameTime gameTime)
    {
      int num = this.EngineState.Loading ? 1 : 0;
    }

    private class TombstoneState
    {
      private const float SpinTime = 0.75f;
      private readonly TombstonesHost Host;
      private SpinAction State;
      private TimeSpan SinceChanged;
      private int SpinSign;
      private Vector3 OriginalPlayerPosition;
      private Quaternion OriginalAoRotation;
      internal Viewpoint LastViewpoint;
      public readonly ArtObjectInstance ArtObject;

      [ServiceDependency]
      public IPhysicsManager PhysicsManager { private get; set; }

      [ServiceDependency]
      public IInputManager InputManager { private get; set; }

      [ServiceDependency]
      public IDefaultCameraManager CameraManager { private get; set; }

      [ServiceDependency]
      public IPlayerManager PlayerManager { private get; set; }

      [ServiceDependency]
      public IGameStateManager GameState { private get; set; }

      [ServiceDependency]
      public ITombstoneService TombstoneService { private get; set; }

      public TombstoneState(TombstonesHost host, ArtObjectInstance ao)
      {
        ServiceHelper.InjectServices((object) this);
        this.Host = host;
        this.ArtObject = ao;
        int num1;
        if (this.GameState.SaveData.ThisLevel.PivotRotations.TryGetValue(this.ArtObject.Id, out num1) && num1 != 0)
        {
          int num2 = Math.Abs(num1);
          for (int index = 0; index < num2; ++index)
          {
            this.OriginalAoRotation = this.ArtObject.Rotation;
            this.ArtObject.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, 1.570796f * (float) Math.Sign(num1));
          }
        }
        this.LastViewpoint = FezMath.AsViewpoint(FezMath.OrientationFromDirection(FezMath.MaxClampXZ(Vector3.Transform(Vector3.Forward, this.ArtObject.Rotation))));
      }

      public bool Update(TimeSpan elapsed)
      {
        this.SinceChanged += elapsed;
        switch (this.State)
        {
          case SpinAction.Idle:
            Vector3 vector = (this.PlayerManager.Position - this.ArtObject.Position - new Vector3(0.0f, 1f, 0.0f)) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
            vector.X += vector.Z;
            Vector3 vector3 = FezMath.Abs(vector);
            if (FezMath.AlmostEqual(FezMath.Abs(Vector3.Transform(Vector3.UnitZ, this.ArtObject.Rotation)), FezMath.DepthMask(this.CameraManager.Viewpoint)) && ((double) vector3.X < 0.899999976158142 && (double) vector3.Y < 1.0) && (this.PlayerManager.CarriedInstance == null && this.PlayerManager.Grounded) && (this.PlayerManager.Action != ActionType.GrabTombstone && this.InputManager.GrabThrow == FezButtonState.Pressed && this.PlayerManager.Action != ActionType.ReadingSign))
            {
              this.SinceChanged = TimeSpan.Zero;
              return true;
            }
            else
              break;
          case SpinAction.Spinning:
            double num = FezMath.Saturate(this.SinceChanged.TotalSeconds / 0.75);
            Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle(Vector3.UnitY, Easing.EaseIn(num < 0.949999999254942 ? num / 0.949999999254942 : 1.0 + Math.Sin((num - 0.949999999254942) / 0.0500000007450581 * 6.28318548202515 * 2.0) * 0.00999999977648258 * (1.0 - num) / 0.0500000007450581, EasingType.Linear) * 1.570796f * (float) this.SpinSign);
            this.ArtObject.Rotation = this.OriginalAoRotation * fromAxisAngle;
            this.PlayerManager.Position = Vector3.Transform(this.OriginalPlayerPosition - this.ArtObject.Position, fromAxisAngle) + this.ArtObject.Position;
            if (this.SinceChanged.TotalSeconds >= 0.75)
            {
              this.LastViewpoint = FezMath.AsViewpoint(FezMath.OrientationFromDirection(FezMath.MaxClampXZ(Vector3.Transform(Vector3.Forward, this.ArtObject.Rotation))));
              int count = Enumerable.Count<TombstonesHost.TombstoneState>((IEnumerable<TombstonesHost.TombstoneState>) this.Host.TrackedStones, (Func<TombstonesHost.TombstoneState, bool>) (x => x.LastViewpoint == this.LastViewpoint));
              this.TombstoneService.UpdateAlignCount(count);
              if (count > 1)
                this.TombstoneService.OnMoreThanOneAligned();
              this.Host.StopSkullRotations = count == 4;
              this.PlayerManager.Action = ActionType.GrabTombstone;
              this.PlayerManager.Position += 0.5f * Vector3.UnitY;
              this.PlayerManager.Velocity = Vector3.Down;
              this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
              this.SinceChanged -= TimeSpan.FromSeconds(0.75);
              this.State = SpinAction.Grabbed;
              break;
            }
            else
              break;
          case SpinAction.Grabbed:
            if (this.PlayerManager.Action != ActionType.GrabTombstone)
            {
              this.State = SpinAction.Idle;
              break;
            }
            else
              break;
        }
        return false;
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
          if (this.SpinSign == 1)
            SoundEffectExtensions.EmitAt(this.Host.TurnRight, this.ArtObject.Position);
          else
            SoundEffectExtensions.EmitAt(this.Host.TurnLeft, this.ArtObject.Position);
          int num;
          if (!this.GameState.SaveData.ThisLevel.PivotRotations.TryGetValue(this.ArtObject.Id, out num))
            this.GameState.SaveData.ThisLevel.PivotRotations.Add(this.ArtObject.Id, this.SpinSign);
          else
            this.GameState.SaveData.ThisLevel.PivotRotations[this.ArtObject.Id] = num + this.SpinSign;
          this.PlayerManager.Position = this.PlayerManager.Position * FezMath.ScreenSpaceMask(this.CameraManager.LastViewpoint) + this.ArtObject.Position * FezMath.DepthMask(this.CameraManager.LastViewpoint) + -FezMath.ForwardVector(this.CameraManager.LastViewpoint);
          this.OriginalPlayerPosition = this.PlayerManager.Position;
          this.OriginalAoRotation = this.ArtObject.Rotation;
          this.SinceChanged = TimeSpan.Zero;
          this.State = SpinAction.Spinning;
          this.PlayerManager.Action = ActionType.PivotTombstone;
        }
      }
    }
  }
}
