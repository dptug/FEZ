// Type: FezGame.Components.SpinBlocksHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
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
  internal class SpinBlocksHost : GameComponent
  {
    private readonly List<SpinBlocksHost.SpinBlockState> TrackedBlocks = new List<SpinBlocksHost.SpinBlockState>();
    private SoundEffect smallSound;
    private SoundEffect largeSound;
    private SoundEffect rotatoSound;

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    public SpinBlocksHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.smallSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/SmallSpinblock");
      this.largeSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/LargeSpinblock");
      this.rotatoSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/RotatoSpinblock");
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.TrackedBlocks.Clear();
      if (this.LevelManager.TrileSet == null)
        return;
      TrileInstance[] trileInstanceArray = Enumerable.ToArray<TrileInstance>(Enumerable.SelectMany<Trile, TrileInstance>(Enumerable.Where<Trile>((IEnumerable<Trile>) this.LevelManager.TrileSet.Triles.Values, (Func<Trile, bool>) (x =>
      {
        if (x.Geometry != null)
          return x.Geometry.Empty;
        else
          return false;
      })), (Func<Trile, IEnumerable<TrileInstance>>) (x => (IEnumerable<TrileInstance>) x.Instances)));
      foreach (ArtObjectInstance aoInstance in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
      {
        if (aoInstance.ArtObject.ActorType == ActorType.SpinBlock)
        {
          Vector3 vector3_1 = aoInstance.ActorSettings.OffCenter ? aoInstance.ActorSettings.RotationCenter : aoInstance.Position;
          BoundingBox box = new BoundingBox(FezMath.Floor(vector3_1 - aoInstance.ArtObject.Size / 2f), FezMath.Floor(vector3_1 + aoInstance.ArtObject.Size / 2f));
          List<TrileInstance> triles = new List<TrileInstance>();
          foreach (TrileInstance trileInstance in trileInstanceArray)
          {
            Vector3 center = trileInstance.Center;
            Vector3 vector3_2 = trileInstance.TransformedSize / 2f;
            if (new BoundingBox(center - vector3_2, center + vector3_2).Intersects(box))
            {
              triles.Add(trileInstance);
              trileInstance.ForceTopMaybe = true;
            }
          }
          if (triles.Count > 0)
          {
            SoundEffect soundEffect = aoInstance.ActorSettings.SpinView == Viewpoint.Up || aoInstance.ActorSettings.SpinView == Viewpoint.Down ? this.rotatoSound : (triles.Count < 4 ? this.smallSound : this.largeSound);
            this.TrackedBlocks.Add(new SpinBlocksHost.SpinBlockState(triles, aoInstance, soundEffect));
          }
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || this.GameState.InMenuCube) || (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || !this.CameraManager.ActionRunning && !this.PlayerManager.IsOnRotato))
        return;
      foreach (SpinBlocksHost.SpinBlockState spinBlockState in this.TrackedBlocks)
        spinBlockState.Update(gameTime.ElapsedGameTime);
    }

    private class SpinBlockState
    {
      private Quaternion SpinAccumulatedRotation = Quaternion.Identity;
      private const float WarnTime = 0.1f;
      private const float SpinTime = 0.5f;
      private const float Charge = 0.1f;
      private readonly List<TrileInstance> Triles;
      private readonly ArtObjectInstance ArtObject;
      private readonly Vector3 OriginalPosition;
      private readonly Vector3 RotationOffset;
      private readonly bool IsRotato;
      private SpinBlocksHost.SpinState State;
      private TimeSpan SinceChanged;
      private Quaternion OriginalRotation;
      private Vector3 OriginalPlayerPosition;
      private readonly SoundEffect SoundEffect;
      private SoundEmitter Emitter;
      private bool hasRotated;
      private bool paused;

      [ServiceDependency]
      public ILevelManager LevelManager { private get; set; }

      [ServiceDependency]
      public IPlayerManager PlayerManager { private get; set; }

      [ServiceDependency]
      public IDefaultCameraManager CameraManager { private get; set; }

      [ServiceDependency]
      public ISoundManager SoundManager { private get; set; }

      [ServiceDependency]
      public ITrixelParticleSystems TrixelParticleSystems { private get; set; }

      public SpinBlockState(List<TrileInstance> triles, ArtObjectInstance aoInstance, SoundEffect soundEffect)
      {
        ServiceHelper.InjectServices((object) this);
        this.Triles = triles;
        this.ArtObject = aoInstance;
        this.OriginalPosition = this.ArtObject.Position;
        if (this.ArtObject.ActorSettings.OffCenter)
          this.RotationOffset = this.ArtObject.ActorSettings.RotationCenter - this.ArtObject.Position;
        if (this.ArtObject.ActorSettings.SpinView == Viewpoint.None)
          this.ArtObject.ActorSettings.SpinView = Viewpoint.Front;
        foreach (TrileInstance trileInstance in this.Triles)
          trileInstance.Unsafe = true;
        this.SoundEffect = soundEffect;
        this.SinceChanged -= TimeSpan.FromSeconds((double) this.ArtObject.ActorSettings.SpinOffset);
        this.IsRotato = this.ArtObject.ActorSettings.SpinView == Viewpoint.Up || this.ArtObject.ActorSettings.SpinView == Viewpoint.Down;
      }

      public void Update(TimeSpan elapsed)
      {
        if (this.ArtObject.ActorSettings.Inactive && this.State == SpinBlocksHost.SpinState.Idle)
          return;
        this.SinceChanged += elapsed;
        switch (this.State)
        {
          case SpinBlocksHost.SpinState.Idle:
            if (this.SinceChanged.TotalSeconds < (double) this.ArtObject.ActorSettings.SpinEvery - 0.5 - 0.100000001490116)
              break;
            this.OriginalRotation = this.ArtObject.Rotation;
            this.SinceChanged -= TimeSpan.FromSeconds((double) this.ArtObject.ActorSettings.SpinEvery - 0.5 - 0.100000001490116);
            this.State = SpinBlocksHost.SpinState.Warning;
            Vector3 right = this.CameraManager.InverseView.Right;
            Vector3 interpolatedCenter = this.CameraManager.InterpolatedCenter;
            float num1 = new Vector2()
            {
              X = FezMath.Dot(this.ArtObject.Position - interpolatedCenter, right),
              Y = (interpolatedCenter.Y - this.ArtObject.Position.Y)
            }.Length();
            if (((double) num1 > 10.0 ? 0.600000023841858 / (((double) num1 - 10.0) / 5.0 + 1.0) : 1.0 - (double) Easing.EaseIn((double) num1 / 10.0, EasingType.Quadratic) * 0.400000005960464) <= 0.0500000007450581)
              break;
            this.Emitter = SoundEffectExtensions.EmitAt(this.SoundEffect, this.ArtObject.Position, RandomHelper.Centered(0.0799999982118607));
            if (!this.IsRotato)
              break;
            this.Emitter.PauseViewTransitions = false;
            break;
          case SpinBlocksHost.SpinState.Warning:
            Quaternion fromAxisAngle1 = Quaternion.CreateFromAxisAngle(FezMath.ForwardVector(this.ArtObject.ActorSettings.SpinView), (float) (-1.57079637050629 * Math.Sin(FezMath.Saturate(this.SinceChanged.TotalSeconds / 0.100000001490116) * 0.785398185253143) * 0.100000001490116));
            this.ArtObject.Rotation = fromAxisAngle1 * this.OriginalRotation;
            this.ArtObject.Position = this.OriginalPosition + Vector3.Transform(-this.RotationOffset, this.SpinAccumulatedRotation * fromAxisAngle1) + this.RotationOffset;
            if (this.SinceChanged.TotalSeconds < 0.100000001490116)
              break;
            this.SinceChanged -= TimeSpan.FromSeconds(0.100000001490116);
            this.State = SpinBlocksHost.SpinState.Spinning;
            break;
          case SpinBlocksHost.SpinState.Spinning:
            double num2 = FezMath.Saturate(this.SinceChanged.TotalSeconds / 0.5);
            float num3 = this.IsRotato ? Easing.EaseInOut(FezMath.Saturate(num2 / 0.75), EasingType.Quartic, EasingType.Quadratic) : Easing.EaseIn(num2 < 0.75 ? num2 / 0.75 : 1.0 + Math.Sin((num2 - 0.75) / 0.25 * 6.28318548202515) * 0.0149999996647239, EasingType.Quintic);
            bool flag1 = this.PlayerManager.Grounded && this.Triles.Contains(this.PlayerManager.Ground.First);
            if (flag1)
            {
              if (!this.IsRotato)
              {
                IPlayerManager playerManager = this.PlayerManager;
                Vector3 vector3 = playerManager.Velocity + FezMath.RightVector(this.ArtObject.ActorSettings.SpinView) * num3 * 0.1f;
                playerManager.Velocity = vector3;
                if ((double) num3 > 0.25)
                  this.PlayerManager.Position -= 0.01f * Vector3.UnitY;
              }
              else if ((double) num3 > 0.0 && !this.hasRotated)
              {
                this.PlayerManager.IsOnRotato = true;
                this.Rotate();
                this.OriginalPlayerPosition = this.PlayerManager.Position;
                this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, this.ArtObject.ActorSettings.SpinView == Viewpoint.Up ? 1 : -1), 0.5f);
                this.hasRotated = true;
              }
            }
            if (!this.IsRotato)
            {
              foreach (TrileInstance trileInstance in this.Triles)
                trileInstance.Enabled = (double) num3 <= 0.25;
            }
            bool flag2 = ActionTypeExtensions.IsOnLedge(this.PlayerManager.Action) && this.Triles.Contains(this.PlayerManager.HeldInstance);
            if (flag2)
            {
              if (!this.IsRotato)
              {
                IPlayerManager playerManager = this.PlayerManager;
                Vector3 vector3 = playerManager.Velocity + FezMath.RightVector(this.ArtObject.ActorSettings.SpinView) * num3 * 0.1f;
                playerManager.Velocity = vector3;
                if ((double) num3 > 0.25)
                {
                  this.PlayerManager.Action = ActionType.Falling;
                  this.PlayerManager.HeldInstance = (TrileInstance) null;
                }
              }
              else if ((double) num3 > 0.0 && (double) num3 < 0.5 && !this.hasRotated)
              {
                this.PlayerManager.IsOnRotato = true;
                this.Rotate();
                this.OriginalPlayerPosition = this.PlayerManager.Position;
                this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, this.ArtObject.ActorSettings.SpinView == Viewpoint.Up ? 1 : -1), 0.5f);
                this.hasRotated = true;
              }
            }
            this.TrixelParticleSystems.PropagateEnergy(this.ArtObject.Position - FezMath.RightVector(this.ArtObject.ActorSettings.SpinView), num3 * 0.1f);
            Quaternion fromAxisAngle2 = Quaternion.CreateFromAxisAngle(FezMath.ForwardVector(this.ArtObject.ActorSettings.SpinView), (float) (1.57079637050629 * (double) num3 * 1.10000002384186 - 0.157079637050629));
            this.ArtObject.Rotation = fromAxisAngle2 * this.OriginalRotation;
            this.ArtObject.Position = this.OriginalPosition + Vector3.Transform(-this.RotationOffset, this.SpinAccumulatedRotation * fromAxisAngle2) + this.RotationOffset;
            if (this.IsRotato && (flag1 || flag2))
            {
              Vector3 vector3 = this.ArtObject.ActorSettings.RotationCenter;
              if (!this.ArtObject.ActorSettings.OffCenter)
                vector3 = this.ArtObject.Position;
              this.PlayerManager.Position = Vector3.Transform(this.OriginalPlayerPosition - vector3, fromAxisAngle2) + vector3;
            }
            if (this.SinceChanged.TotalSeconds < 0.5)
              break;
            foreach (TrileInstance trileInstance in this.Triles)
              trileInstance.Enabled = true;
            if ((!this.IsRotato || !this.hasRotated) && (double) this.Triles.Count != (double) this.ArtObject.ArtObject.Size.X * (double) this.ArtObject.ArtObject.Size.Y * (double) this.ArtObject.ArtObject.Size.Z)
              this.Rotate();
            this.SpinAccumulatedRotation = this.SpinAccumulatedRotation * fromAxisAngle2;
            this.State = SpinBlocksHost.SpinState.Idle;
            this.hasRotated = false;
            this.SinceChanged -= TimeSpan.FromSeconds(0.5);
            this.PlayerManager.IsOnRotato = false;
            break;
        }
      }

      private void Rotate()
      {
        Vector3 vector3 = this.ArtObject.ActorSettings.RotationCenter;
        if (!this.ArtObject.ActorSettings.OffCenter)
          vector3 = this.ArtObject.Position;
        Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle(FezMath.ForwardVector(this.ArtObject.ActorSettings.SpinView), 1.570796f);
        TrileInstance[] trileInstanceArray = this.Triles.ToArray();
        foreach (TrileInstance instance in trileInstanceArray)
        {
          Vector3 a = Vector3.Transform(instance.Position + FezMath.HalfVector - vector3, fromAxisAngle) + vector3 - FezMath.HalfVector;
          if (!FezMath.AlmostEqual(a, instance.Position))
          {
            this.LevelManager.ClearTrile(instance, true);
            instance.Position = a;
          }
        }
        foreach (TrileInstance instance in trileInstanceArray)
          this.LevelManager.UpdateInstance(instance);
      }
    }

    private enum SpinState
    {
      Idle,
      Warning,
      Spinning,
    }
  }
}
