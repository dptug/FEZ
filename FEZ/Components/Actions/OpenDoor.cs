// Type: FezGame.Components.Actions.OpenDoor
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components.Actions
{
  internal class OpenDoor : PlayerAction
  {
    private static readonly TimeSpan OpeningDuration = TimeSpan.FromSeconds(1.25);
    private TimeSpan sinceOpened;
    private TrileInstance doorBottom;
    private TrileInstance doorTop;
    private TrileInstance holeBottom;
    private TrileInstance holeTop;
    private TrileInstance tempBottom;
    private TrileInstance tempTop;
    private ArtObjectInstance aoInstance;
    private Quaternion aoInitialRotation;
    private float initialPhi;
    private Vector3 initialPosition;
    private Vector3 initialAoPosition;
    private bool isUnlocked;
    private SoundEffect unlockSound;
    private SoundEffect openSound;
    private SoundEffect turnSound;
    private int lastFrame;

    [ServiceDependency]
    public IDotManager DotManager { private get; set; }

    static OpenDoor()
    {
    }

    public OpenDoor(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanging += (Action) (() =>
      {
        bool local_0 = false;
        foreach (TrileEmplacement item_0 in this.GameState.SaveData.ThisLevel.InactiveTriles)
        {
          TrileEmplacement local_2 = item_0;
          TrileInstance bottom = this.LevelManager.TrileInstanceAt(ref local_2);
          if (bottom != null && ActorTypeExtensions.IsDoor(bottom.Trile.ActorSettings.Type))
          {
            local_2 = bottom.Emplacement + Vector3.UnitY;
            TrileInstance local_3 = this.LevelManager.TrileInstanceAt(ref local_2);
            ArtObjectInstance local_4;
            TrileGroup group;
            if ((group = Enumerable.FirstOrDefault<TrileGroup>((IEnumerable<TrileGroup>) this.LevelManager.Groups.Values, (Func<TrileGroup, bool>) (x => x.Triles.Contains(bottom)))) != null && (local_4 = Enumerable.FirstOrDefault<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x =>
            {
              int? local_0 = x.ActorSettings.AttachedGroup;
              int local_1 = group.Id;
              if (local_0.GetValueOrDefault() == local_1)
                return local_0.HasValue;
              else
                return false;
            }))) != null)
              this.LevelManager.RemoveArtObject(local_4);
            this.LevelManager.ClearTrile(bottom);
            this.LevelManager.ClearTrile(local_3);
            local_0 = true;
          }
        }
        if (!local_0 || this.GameState.FarawaySettings.InTransition)
          return;
        this.LevelMaterializer.CullInstances();
      });
      base.Initialize();
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.unlockSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/UnlockDoor");
      this.openSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/OpenDoor");
      this.turnSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/TurnAway");
    }

    protected override void TestConditions()
    {
      if (!this.PlayerManager.Grounded)
        this.UnDotize();
      else if (this.PlayerManager.CarriedInstance != null)
      {
        this.UnDotize();
      }
      else
      {
        bool flag = false;
        foreach (NearestTriles nearestTriles in this.PlayerManager.AxisCollision.Values)
        {
          TrileInstance trileInstance = nearestTriles.Surface;
          if (trileInstance != null)
          {
            Trile trile = trileInstance.Trile;
            FaceOrientation faceOrientation = FezMath.OrientationFromPhi(FezMath.ToPhi(trile.ActorSettings.Face) + trileInstance.Phi);
            if (ActorTypeExtensions.IsDoor(trile.ActorSettings.Type) && faceOrientation == FezMath.VisibleOrientation(this.CameraManager.Viewpoint))
            {
              flag = this.GameState.SaveData.Keys > 0 && this.LevelManager.Name != "VILLAGEVILLE_2D" || trile.ActorSettings.Type == ActorType.UnlockedDoor;
              this.isUnlocked = trile.ActorSettings.Type == ActorType.UnlockedDoor;
              this.doorBottom = trileInstance;
              break;
            }
          }
        }
        if (!flag)
          this.UnDotize();
        else if (this.doorBottom.ActorSettings.Inactive)
        {
          this.UnDotize();
        }
        else
        {
          if (!this.PlayerManager.HideFez && this.PlayerManager.CanControl && (!this.DotManager.PreventPoI && this.doorBottom.Trile.ActorSettings.Type == ActorType.Door))
          {
            this.DotManager.Behaviour = DotHost.BehaviourType.ThoughtBubble;
            this.DotManager.FaceButton = DotFaceButton.Up;
            this.DotManager.ComeOut();
            if (this.DotManager.Owner != this)
              this.DotManager.Hey();
            this.DotManager.Owner = (object) this;
          }
          if (this.GameState.IsTrialMode && this.LevelManager.Name == "trial/VILLAGEVILLE_2D" || this.InputManager.ExactUp != FezButtonState.Pressed)
            return;
          TrileGroup trileGroup1 = (TrileGroup) null;
          this.aoInstance = (ArtObjectInstance) null;
          foreach (TrileGroup trileGroup2 in (IEnumerable<TrileGroup>) this.LevelManager.Groups.Values)
          {
            if (trileGroup2.Triles.Contains(this.doorBottom))
            {
              trileGroup1 = trileGroup2;
              break;
            }
          }
          if (trileGroup1 != null)
          {
            foreach (ArtObjectInstance artObjectInstance in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
            {
              int? attachedGroup = artObjectInstance.ActorSettings.AttachedGroup;
              int id = trileGroup1.Id;
              if ((attachedGroup.GetValueOrDefault() != id ? 0 : (attachedGroup.HasValue ? 1 : 0)) != 0)
              {
                this.aoInstance = artObjectInstance;
                break;
              }
            }
            if (this.aoInstance != null)
            {
              this.aoInitialRotation = this.aoInstance.Rotation;
              this.initialAoPosition = this.aoInstance.Position;
            }
          }
          this.WalkTo.Destination = new Func<Vector3>(this.GetDestination);
          this.PlayerManager.Action = ActionType.WalkingTo;
          this.WalkTo.NextAction = ActionType.OpeningDoor;
        }
      }
    }

    private void UnDotize()
    {
      if (this.DotManager.Owner != this)
        return;
      this.DotManager.Owner = (object) null;
      this.DotManager.Burrow();
    }

    private Vector3 GetDestination()
    {
      Viewpoint viewpoint = this.CameraManager.Viewpoint;
      return this.PlayerManager.Position * (Vector3.UnitY + FezMath.DepthMask(viewpoint)) + this.doorBottom.Center * FezMath.SideMask(viewpoint);
    }

    protected override void Begin()
    {
      if (this.DotManager.Owner == this)
        this.DotManager.Burrow();
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity * Vector3.UnitY;
      playerManager.Velocity = vector3;
      this.PlayerManager.LookingDirection = HorizontalDirection.Right;
      this.GameState.SaveData.ThisLevel.InactiveTriles.Add(this.doorBottom.Emplacement);
      this.doorBottom.ActorSettings.Inactive = true;
      TrileEmplacement id1 = this.doorBottom.Emplacement + Vector3.UnitY;
      this.doorTop = this.LevelManager.TrileInstanceAt(ref id1);
      this.sinceOpened = TimeSpan.FromSeconds(-1.0);
      this.initialPhi = this.doorBottom.Phi;
      this.initialPosition = this.doorBottom.Position;
      if (this.doorBottom.Trile.ActorSettings.Type == ActorType.Door)
      {
        ++this.GameState.SaveData.ThisLevel.FilledConditions.LockedDoorCount;
        if (this.doorTop.Trile.ActorSettings.Type == ActorType.Door)
          ++this.GameState.SaveData.ThisLevel.FilledConditions.LockedDoorCount;
        --this.GameState.SaveData.Keys;
        this.GameState.OnHudElementChanged();
      }
      else
      {
        ++this.GameState.SaveData.ThisLevel.FilledConditions.UnlockedDoorCount;
        if (this.doorTop.Trile.ActorSettings.Type == ActorType.UnlockedDoor)
          ++this.GameState.SaveData.ThisLevel.FilledConditions.UnlockedDoorCount;
      }
      TrileEmplacement id2 = this.doorBottom.Emplacement + FezMath.ForwardVector(this.CameraManager.Viewpoint);
      this.holeBottom = this.LevelManager.TrileInstanceAt(ref id2);
      TrileEmplacement id3 = this.doorTop.Emplacement + FezMath.ForwardVector(this.CameraManager.Viewpoint);
      this.holeTop = this.LevelManager.TrileInstanceAt(ref id3);
      id3 = this.doorBottom.Emplacement + FezMath.ForwardVector(this.CameraManager.Viewpoint) * 2f;
      this.tempBottom = this.LevelManager.TrileInstanceAt(ref id3);
      id3 = this.doorTop.Emplacement + FezMath.ForwardVector(this.CameraManager.Viewpoint) * 2f;
      this.tempTop = this.LevelManager.TrileInstanceAt(ref id3);
      if (this.tempBottom != null)
        this.LevelManager.ClearTrile(this.tempBottom);
      if (this.tempTop != null)
        this.LevelManager.ClearTrile(this.tempTop);
      SoundEffectExtensions.EmitAt(this.turnSound, this.PlayerManager.Position);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      this.sinceOpened += elapsed;
      float num1 = FezMath.Saturate((float) this.sinceOpened.Ticks / (float) OpenDoor.OpeningDuration.Ticks);
      if (this.doorBottom.InstanceId != -1)
      {
        float num2 = Easing.EaseInOut((double) num1, EasingType.Sine);
        float angle = num2 * 1.570796f;
        this.doorBottom.Phi = this.doorTop.Phi = this.initialPhi + angle;
        if (this.aoInstance != null)
          this.aoInstance.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, angle) * this.aoInitialRotation;
        Vector3 vector3_1 = FezMath.RightVector(this.CameraManager.Viewpoint);
        Vector3 vector3_2 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
        Vector3 vector3_3 = FezMath.DepthMask(this.CameraManager.Viewpoint);
        Vector3 vector3_4 = vector3_2 * 1.125f * num2 + vector3_1 * (float) (Math.Sin((double) angle * 2.0) * ((Math.Sqrt(2.0) - 1.0) / 2.0) - 3.0 / 16.0 * (double) num2);
        this.doorBottom.Position = new Vector3(this.initialPosition.X, this.doorBottom.Position.Y, this.initialPosition.Z) + vector3_4;
        this.LevelManager.UpdateInstance(this.doorBottom);
        this.doorTop.Position = new Vector3(this.initialPosition.X, this.doorTop.Position.Y, this.initialPosition.Z) + vector3_4;
        this.LevelManager.UpdateInstance(this.doorTop);
        if (this.holeBottom != null)
        {
          this.holeBottom.Position = new Vector3(this.initialPosition.X, this.doorBottom.Position.Y, this.initialPosition.Z) + vector3_4 * vector3_3 + vector3_2;
          this.LevelManager.UpdateInstance(this.holeBottom);
        }
        if (this.holeTop != null)
        {
          this.holeTop.Position = new Vector3(this.initialPosition.X, this.doorTop.Position.Y, this.initialPosition.Z) + vector3_4 * vector3_3 + vector3_2;
          this.LevelManager.UpdateInstance(this.holeTop);
        }
        if (this.aoInstance != null)
          this.aoInstance.Position = this.initialAoPosition + vector3_2 * 1.125f * num2 + vector3_1 * (float) (Math.Sin((double) angle * 2.0) * ((Math.Sqrt(2.0) - 1.0) / 2.0) + 3.0 / 16.0 * (double) num2);
        if ((double) num1 == 1.0)
        {
          this.LevelManager.ClearTrile(this.doorBottom);
          this.LevelManager.ClearTrile(this.doorTop);
          if (this.holeBottom != null)
          {
            this.holeBottom.Position = new Vector3(this.initialPosition.X, this.doorBottom.Position.Y, this.initialPosition.Z) + vector3_2;
            this.LevelManager.UpdateInstance(this.holeBottom);
          }
          if (this.holeTop != null)
          {
            this.holeTop.Position = new Vector3(this.initialPosition.X, this.doorTop.Position.Y, this.initialPosition.Z) + vector3_2;
            this.LevelManager.UpdateInstance(this.holeTop);
          }
          if (this.tempBottom != null)
            this.LevelManager.RestoreTrile(this.tempBottom);
          if (this.tempTop != null)
            this.LevelManager.RestoreTrile(this.tempTop);
          this.LevelMaterializer.CullInstances();
        }
      }
      if (this.PlayerManager.Animation.Timing.Ended)
      {
        if (!this.isUnlocked)
          this.PlayerManager.Action = ActionType.Walking;
        this.PlayerManager.Action = ActionType.Idle;
      }
      int frame = this.PlayerManager.Animation.Timing.Frame;
      if (this.lastFrame != frame)
      {
        if (frame == 7)
        {
          if (this.doorBottom.Trile.ActorSettings.Type == ActorType.Door)
            SoundEffectExtensions.EmitAt(this.unlockSound, this.PlayerManager.Position);
          else
            SoundEffectExtensions.EmitAt(this.openSound, this.PlayerManager.Position);
        }
        this.lastFrame = frame;
      }
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.OpeningDoor;
    }
  }
}
