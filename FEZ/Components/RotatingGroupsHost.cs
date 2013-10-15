// Type: FezGame.Components.RotatingGroupsHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
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
  internal class RotatingGroupsHost : GameComponent, IRotatingGroupService, IScriptingBase
  {
    private readonly List<RotatingGroupsHost.RotatingGroupState> RotatingGroups = new List<RotatingGroupsHost.RotatingGroupState>();

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    public RotatingGroupsHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.TryInitialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
    }

    private void TryInitialize()
    {
      this.RotatingGroups.Clear();
      this.RotatingGroups.AddRange(Enumerable.Select<TrileGroup, RotatingGroupsHost.RotatingGroupState>(Enumerable.Where<TrileGroup>((IEnumerable<TrileGroup>) this.LevelManager.Groups.Values, (Func<TrileGroup, bool>) (x => x.ActorType == ActorType.RotatingGroup)), (Func<TrileGroup, RotatingGroupsHost.RotatingGroupState>) (x => new RotatingGroupsHost.RotatingGroupState(x))));
      this.Enabled = this.RotatingGroups.Count > 0;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || this.GameState.InMenuCube) || this.GameState.InFpsMode)
        return;
      float elapsedSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      foreach (RotatingGroupsHost.RotatingGroupState rotatingGroupState in this.RotatingGroups)
        rotatingGroupState.Update(elapsedSeconds);
    }

    public void ResetEvents()
    {
    }

    public void Rotate(int id, bool clockwise, int turns)
    {
      if (!this.Enabled)
        return;
      foreach (RotatingGroupsHost.RotatingGroupState rotatingGroupState in this.RotatingGroups)
      {
        if (rotatingGroupState.Group.Id == id)
        {
          RotatingGroupsHost.RotatingGroupState cached = rotatingGroupState;
          Waiters.Wait((Func<bool>) (() => cached.Action == SpinAction.Idle), (Action) (() =>
          {
            cached.Rotate(clockwise, turns);
            cached.SinceChanged = 0.0f;
          }));
        }
      }
    }

    public void SetEnabled(int id, bool enabled)
    {
      if (!this.Enabled)
        return;
      foreach (RotatingGroupsHost.RotatingGroupState rotatingGroupState in this.RotatingGroups)
      {
        if (rotatingGroupState.Group.Id == id)
        {
          rotatingGroupState.Enabled = enabled;
          rotatingGroupState.SinceChanged = 0.0f;
        }
      }
    }

    private class RotatingGroupState
    {
      private readonly List<TrileInstance> TopLayer = new List<TrileInstance>();
      private const float SpinTime = 0.75f;
      public readonly TrileGroup Group;
      private readonly Vector3 Center;
      private readonly ArtObjectInstance[] AttachedArtObjects;
      private readonly Vector3[] AttachedAoOrigins;
      private readonly Quaternion[] AttachedAoRotations;
      private readonly TrileMaterializer[] CachedMaterializers;
      public bool Enabled;
      public float SinceChanged;
      private SoundEffect sSpin;
      private Vector3 OriginalForward;
      private Vector4[] OriginalStates;
      private Vector3 OriginalPlayerPosition;
      private int SpinSign;
      private int Turns;
      private bool HeldOnto;
      private bool GroundedOn;

      public SpinAction Action { get; private set; }

      [ServiceDependency]
      public IGameLevelManager LevelManager { private get; set; }

      [ServiceDependency]
      public ILevelMaterializer LevelMaterializer { private get; set; }

      [ServiceDependency]
      public IDefaultCameraManager CameraManager { private get; set; }

      [ServiceDependency]
      public IPlayerManager PlayerManager { private get; set; }

      [ServiceDependency]
      public IContentManagerProvider CMProvider { private get; set; }

      public RotatingGroupState(TrileGroup group)
      {
        ServiceHelper.InjectServices((object) this);
        this.Group = group;
        this.AttachedArtObjects = Enumerable.ToArray<ArtObjectInstance>(Enumerable.Where<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x =>
        {
          int? local_0 = x.ActorSettings.AttachedGroup;
          int local_1 = this.Group.Id;
          if (local_0.GetValueOrDefault() == local_1)
            return local_0.HasValue;
          else
            return false;
        })));
        this.AttachedAoOrigins = Enumerable.ToArray<Vector3>(Enumerable.Select<ArtObjectInstance, Vector3>((IEnumerable<ArtObjectInstance>) this.AttachedArtObjects, (Func<ArtObjectInstance, Vector3>) (x => x.Position)));
        this.AttachedAoRotations = Enumerable.ToArray<Quaternion>(Enumerable.Select<ArtObjectInstance, Quaternion>((IEnumerable<ArtObjectInstance>) this.AttachedArtObjects, (Func<ArtObjectInstance, Quaternion>) (x => x.Rotation)));
        this.CachedMaterializers = Enumerable.ToArray<TrileMaterializer>(Enumerable.Select<TrileInstance, TrileMaterializer>((IEnumerable<TrileInstance>) group.Triles, (Func<TrileInstance, TrileMaterializer>) (x => this.LevelMaterializer.GetTrileMaterializer(x.Trile))));
        foreach (TrileInstance trileInstance in this.Group.Triles)
        {
          trileInstance.ForceSeeThrough = true;
          trileInstance.Unsafe = true;
        }
        float num = Enumerable.Max<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.Group.Triles, (Func<TrileInstance, bool>) (x => !x.Trile.Immaterial)), (Func<TrileInstance, float>) (x => x.Position.Y));
        foreach (TrileInstance trileInstance in this.Group.Triles)
        {
          if ((double) trileInstance.Position.Y == (double) num)
            this.TopLayer.Add(trileInstance);
        }
        if (this.Group.SpinCenter != Vector3.Zero)
        {
          this.Center = this.Group.SpinCenter;
        }
        else
        {
          foreach (TrileInstance trileInstance in this.Group.Triles)
            this.Center += trileInstance.Position + FezMath.HalfVector;
          this.Center /= (float) this.Group.Triles.Count;
        }
        this.Enabled = !this.Group.SpinNeedsTriggering;
        this.SinceChanged = -this.Group.SpinOffset;
        if ((double) this.SinceChanged != 0.0)
          this.SinceChanged -= 0.375f;
        if (string.IsNullOrEmpty(group.AssociatedSound))
          return;
        try
        {
          this.sSpin = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/" + group.AssociatedSound);
        }
        catch (Exception ex)
        {
          Logger.Log("RotatingGroups", LogSeverity.Warning, "Could not find associated sound '" + group.AssociatedSound + "'");
        }
      }

      public void Update(float elapsedSeconds)
      {
        if (!this.Enabled && this.Action == SpinAction.Idle)
          return;
        this.SinceChanged += elapsedSeconds;
        float spinFrequency = this.Group.SpinFrequency;
        switch (this.Action)
        {
          case SpinAction.Idle:
            if ((double) this.SinceChanged < (double) spinFrequency)
              break;
            this.SinceChanged -= spinFrequency;
            this.Rotate(this.Group.SpinClockwise, this.Group.Spin180Degrees ? 2 : 1);
            break;
          case SpinAction.Spinning:
            float num1 = Easing.EaseInOut((double) FezMath.Saturate(FezMath.Saturate(this.SinceChanged / (0.75f * (float) this.Turns)) / 0.75f), EasingType.Quartic, EasingType.Quadratic);
            float angle = num1 * 1.570796f * (float) this.SpinSign * (float) this.Turns;
            Matrix fromAxisAngle1 = Matrix.CreateFromAxisAngle(Vector3.UnitY, angle);
            Quaternion fromAxisAngle2 = Quaternion.CreateFromAxisAngle(Vector3.UnitY, angle);
            if (!this.PlayerManager.IsOnRotato && (double) num1 < 0.5)
            {
              RotatingGroupsHost.RotatingGroupState rotatingGroupState1 = this;
              int num2 = rotatingGroupState1.HeldOnto | this.Group.Triles.Contains(this.PlayerManager.HeldInstance) ? 1 : 0;
              rotatingGroupState1.HeldOnto = num2 != 0;
              RotatingGroupsHost.RotatingGroupState rotatingGroupState2 = this;
              int num3 = (rotatingGroupState2.GroundedOn ? 1 : 0) | (!this.PlayerManager.Grounded ? 0 : (this.TopLayer.Contains(this.PlayerManager.Ground.First) ? 1 : 0));
              rotatingGroupState2.GroundedOn = num3 != 0;
              if (this.GroundedOn || this.HeldOnto)
              {
                this.OriginalPlayerPosition = this.PlayerManager.Position;
                this.PlayerManager.IsOnRotato = true;
              }
            }
            if ((this.GroundedOn || this.HeldOnto) && ((double) num1 > 0.100000001490116 && !this.CameraManager.ForceTransition))
            {
              if (this.HeldOnto && this.Group.FallOnRotate)
              {
                this.PlayerManager.Action = ActionType.Idle;
                this.PlayerManager.HeldInstance = (TrileInstance) null;
                this.PlayerManager.IsOnRotato = false;
                this.HeldOnto = false;
              }
              else
              {
                this.CameraManager.ForceTransition = true;
                this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, this.SpinSign * this.Turns), -1f);
                this.CameraManager.Direction = -FezMath.ForwardVector(this.CameraManager.LastViewpoint);
                this.CameraManager.RebuildView();
              }
            }
            Vector3 vector3_1 = new Vector3(this.Center.X - 0.5f, this.Center.Y - 0.5f, this.Center.Z - 0.5f);
            for (int index = 0; index < this.OriginalStates.Length; ++index)
            {
              TrileInstance instance = this.Group.Triles[index];
              Vector4 vector4 = this.OriginalStates[index];
              Vector3 vector3_2 = Vector3.Transform(new Vector3(vector4.X, vector4.Y, vector4.Z), fromAxisAngle1);
              instance.Position = new Vector3(vector3_2.X + vector3_1.X, vector3_2.Y + vector3_1.Y, vector3_2.Z + vector3_1.Z);
              instance.SetPhiLight(vector4.W + angle);
              this.CachedMaterializers[index].UpdateInstance(instance);
            }
            for (int index = 0; index < this.AttachedArtObjects.Length; ++index)
            {
              this.AttachedArtObjects[index].Rotation = this.AttachedAoRotations[index] * fromAxisAngle2;
              this.AttachedArtObjects[index].Position = Vector3.Transform(this.AttachedAoOrigins[index] - this.Center, fromAxisAngle1) + this.Center;
            }
            if (this.GroundedOn || this.HeldOnto)
            {
              Vector3 position = this.PlayerManager.Position;
              Vector3 vector3_2 = Vector3.Transform(this.OriginalPlayerPosition - this.Center, fromAxisAngle1) + this.Center;
              if (!this.HeldOnto || !this.Group.FallOnRotate)
              {
                this.CameraManager.Center += vector3_2 - position;
                this.CameraManager.Direction = Vector3.Transform(-this.OriginalForward, fromAxisAngle1);
              }
              this.PlayerManager.Position += vector3_2 - position;
            }
            if ((double) this.SinceChanged < 0.75 * (double) this.Turns)
              break;
            if (this.GroundedOn || this.HeldOnto)
            {
              this.PlayerManager.IsOnRotato = false;
              this.RotateTriles();
              this.CameraManager.ForceTransition = false;
              this.PlayerManager.ForceOverlapsDetermination();
            }
            else
              this.RotateTriles();
            this.SinceChanged -= 0.75f;
            this.Action = SpinAction.Idle;
            break;
        }
      }

      public void Rotate(bool clockwise, int turns)
      {
        this.SpinSign = clockwise ? 1 : -1;
        this.Turns = turns;
        foreach (TrileInstance instance in this.Group.Triles)
        {
          this.LevelMaterializer.UnregisterViewedInstance(instance);
          if (instance.InstanceId == -1)
            this.LevelMaterializer.CullInstanceInNoRegister(instance);
          instance.SkipCulling = true;
        }
        this.LevelMaterializer.CommitBatchesIfNeeded();
        this.RecordStates();
        for (int index = 0; index < this.AttachedArtObjects.Length; ++index)
        {
          this.AttachedAoRotations[index] = this.AttachedArtObjects[index].Rotation;
          this.AttachedAoOrigins[index] = this.AttachedArtObjects[index].Position;
        }
        this.HeldOnto = this.Group.Triles.Contains(this.PlayerManager.HeldInstance);
        this.GroundedOn = this.PlayerManager.Grounded && this.TopLayer.Contains(this.PlayerManager.Ground.First);
        if (this.GroundedOn || this.HeldOnto)
          this.PlayerManager.IsOnRotato = true;
        this.OriginalForward = FezMath.ForwardVector(this.CameraManager.Viewpoint);
        this.OriginalPlayerPosition = this.PlayerManager.Position;
        this.Action = SpinAction.Spinning;
        if (this.sSpin == null)
          return;
        SoundEffectExtensions.EmitAt(this.sSpin, this.Center, false, RandomHelper.Centered(0.100000001490116), false).FadeDistance = 50f;
      }

      private void RecordStates()
      {
        this.OriginalStates = Enumerable.ToArray<Vector4>(Enumerable.Select<TrileInstance, Vector4>((IEnumerable<TrileInstance>) this.Group.Triles, (Func<TrileInstance, Vector4>) (x => new Vector4(x.Position + FezMath.HalfVector - this.Center, x.Phi))));
      }

      private void RotateTriles()
      {
        float angle = 1.570796f * (float) this.SpinSign * (float) this.Turns;
        Matrix fromAxisAngle = Matrix.CreateFromAxisAngle(Vector3.UnitY, angle);
        Vector3 vector3 = new Vector3(this.Center.X - 0.5f, this.Center.Y - 0.5f, this.Center.Z - 0.5f);
        for (int index = 0; index < this.OriginalStates.Length; ++index)
        {
          TrileInstance instance = this.Group.Triles[index];
          Vector4 vector = this.OriginalStates[index];
          instance.Position = Vector3.Transform(FezMath.XYZ(vector), fromAxisAngle) + vector3;
          instance.Phi = FezMath.WrapAngle(vector.W + angle);
          this.LevelManager.UpdateInstance(instance);
          instance.SkipCulling = false;
        }
        this.LevelMaterializer.CommitBatchesIfNeeded();
      }
    }
  }
}
