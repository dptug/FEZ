// Type: FezGame.Components.PivotsHost
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
  public class PivotsHost : GameComponent
  {
    private readonly List<PivotsHost.PivotState> TrackedPivots = new List<PivotsHost.PivotState>();
    private SoundEffect LeftSound;
    private SoundEffect RightSound;

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    public PivotsHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanging += new Action(this.TryInitialize);
      this.TryInitialize();
      this.LeftSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/PivotLeft");
      this.RightSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Industrial/PivotRight");
    }

    private void TryInitialize()
    {
      this.TrackedPivots.Clear();
      foreach (ArtObjectInstance handleAo in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
      {
        if (handleAo.ArtObject.ActorType == ActorType.PivotHandle || handleAo.ArtObject.ActorType == ActorType.Bookcase)
          this.TrackedPivots.Add(new PivotsHost.PivotState(this, handleAo));
      }
      if (this.TrackedPivots.Count <= 0)
        return;
      this.LevelMaterializer.CullInstances();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Loading || this.EngineState.Paused || (this.GameState.InMap || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)) || !this.CameraManager.ActionRunning)
        return;
      float num1 = float.MaxValue;
      PivotsHost.PivotState pivotState1 = (PivotsHost.PivotState) null;
      foreach (PivotsHost.PivotState pivotState2 in this.TrackedPivots)
      {
        if (pivotState2.Update(gameTime.ElapsedGameTime))
        {
          float num2 = FezMath.Dot(pivotState2.HandleAo.Position, FezMath.ForwardVector(this.CameraManager.Viewpoint));
          if ((double) num2 < (double) num1)
          {
            pivotState1 = pivotState2;
            num1 = num2;
          }
        }
      }
      if (pivotState1 == null)
        return;
      pivotState1.Spin();
    }

    private class PivotState
    {
      private readonly List<TrileInstance> TopLayer = new List<TrileInstance>();
      private readonly List<TrileInstance> AttachedTriles = new List<TrileInstance>();
      private const float SpinTime = 1.5f;
      private readonly PivotsHost Host;
      private readonly TrileGroup Group;
      public readonly ArtObjectInstance HandleAo;
      private readonly ArtObjectInstance[] AttachedArtObjects;
      private readonly Vector3[] AttachedAoOrigins;
      private readonly Quaternion[] AttachedAoRotations;
      private Vector4[] OriginalStates;
      private Quaternion OriginalAoRotation;
      private SpinAction State;
      private TimeSpan SinceChanged;
      private int SpinSign;
      private bool HasShaken;

      [ServiceDependency]
      public IPivotService PivotService { private get; set; }

      [ServiceDependency]
      public IGameLevelManager LevelManager { private get; set; }

      [ServiceDependency]
      public ILevelMaterializer LevelMaterializer { private get; set; }

      [ServiceDependency]
      public IInputManager InputManager { private get; set; }

      [ServiceDependency]
      public IDefaultCameraManager CameraManager { private get; set; }

      [ServiceDependency]
      public IPlayerManager PlayerManager { private get; set; }

      [ServiceDependency]
      public IGameStateManager GameState { private get; set; }

      public PivotState(PivotsHost host, ArtObjectInstance handleAo)
      {
        ServiceHelper.InjectServices((object) this);
        this.Host = host;
        this.HandleAo = handleAo;
        this.Group = this.LevelManager.Groups[handleAo.ActorSettings.AttachedGroup.Value];
        this.AttachedArtObjects = Enumerable.ToArray<ArtObjectInstance>(Enumerable.Where<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x =>
        {
          int? local_0 = x.ActorSettings.AttachedGroup;
          int local_1 = this.Group.Id;
          if ((local_0.GetValueOrDefault() != local_1 ? 0 : (local_0.HasValue ? 1 : 0)) != 0)
            return x != this.HandleAo;
          else
            return false;
        })));
        this.AttachedAoOrigins = Enumerable.ToArray<Vector3>(Enumerable.Select<ArtObjectInstance, Vector3>((IEnumerable<ArtObjectInstance>) this.AttachedArtObjects, (Func<ArtObjectInstance, Vector3>) (x => x.Position)));
        this.AttachedAoRotations = Enumerable.ToArray<Quaternion>(Enumerable.Select<ArtObjectInstance, Quaternion>((IEnumerable<ArtObjectInstance>) this.AttachedArtObjects, (Func<ArtObjectInstance, Quaternion>) (x => x.Rotation)));
        foreach (TrileInstance trileInstance in this.Group.Triles)
          trileInstance.ForceSeeThrough = true;
        float num = Enumerable.Max<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.Group.Triles, (Func<TrileInstance, bool>) (x => !x.Trile.Immaterial)), (Func<TrileInstance, float>) (x => x.Position.Y));
        foreach (TrileInstance trileInstance in this.Group.Triles)
        {
          if ((double) trileInstance.Position.Y == (double) num)
            this.TopLayer.Add(trileInstance);
        }
        if (this.LevelManager.Name == "WATER_TOWER" && this.LevelManager.LastLevelName == "LIGHTHOUSE")
        {
          if (this.GameState.SaveData.ThisLevel.PivotRotations.ContainsKey(handleAo.Id))
            this.GameState.SaveData.ThisLevel.PivotRotations[handleAo.Id] = 0;
        }
        else
        {
          int initialSpins;
          if (this.GameState.SaveData.ThisLevel.PivotRotations.TryGetValue(handleAo.Id, out initialSpins) && initialSpins != 0)
            this.ForceSpinTo(initialSpins);
        }
        if (!this.GameState.SaveData.ThisLevel.InactiveArtObjects.Contains(this.HandleAo.Id))
          return;
        this.HandleAo.Enabled = false;
      }

      private void ForceSpinTo(int initialSpins)
      {
        int num = Math.Abs(initialSpins);
        for (int index1 = 0; index1 < num; ++index1)
        {
          this.OriginalAoRotation = this.HandleAo.Rotation;
          this.AttachedTriles.Clear();
          foreach (TrileInstance instance in this.TopLayer)
            this.AddSupportedTrilesOver(instance);
          this.OriginalStates = Enumerable.ToArray<Vector4>(Enumerable.Select<TrileInstance, Vector4>(Enumerable.Union<TrileInstance>((IEnumerable<TrileInstance>) this.Group.Triles, (IEnumerable<TrileInstance>) this.AttachedTriles), (Func<TrileInstance, Vector4>) (x => new Vector4(x.Position, x.Phi))));
          float angle = 1.570796f * (float) Math.Sign(initialSpins);
          Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle(Vector3.UnitY, angle);
          Vector3 position = this.HandleAo.Position;
          for (int index2 = 0; index2 < this.AttachedArtObjects.Length; ++index2)
          {
            this.AttachedAoRotations[index2] = this.AttachedArtObjects[index2].Rotation;
            this.AttachedAoOrigins[index2] = this.AttachedArtObjects[index2].Position;
            this.AttachedArtObjects[index2].Rotation = this.AttachedAoRotations[index2] * fromAxisAngle;
            this.AttachedArtObjects[index2].Position = Vector3.Transform(this.AttachedAoOrigins[index2] - position, fromAxisAngle) + position;
          }
          for (int index2 = 0; index2 < this.OriginalStates.Length; ++index2)
          {
            TrileInstance instance = index2 < this.Group.Triles.Count ? this.Group.Triles[index2] : this.AttachedTriles[index2 - this.Group.Triles.Count];
            Vector4 vector = this.OriginalStates[index2];
            instance.Position = Vector3.Transform(FezMath.XYZ(vector) + new Vector3(0.5f) - position, fromAxisAngle) + position - new Vector3(0.5f);
            instance.Phi = FezMath.WrapAngle(vector.W + angle);
            this.LevelMaterializer.GetTrileMaterializer(instance.Trile).UpdateInstance(instance);
          }
          this.RotateTriles();
        }
        this.HandleAo.Rotation = this.HandleAo.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitY, 1.570796f * (float) initialSpins);
      }

      public bool Update(TimeSpan elapsed)
      {
        this.SinceChanged += elapsed;
        switch (this.State)
        {
          case SpinAction.Idle:
            bool flag;
            if (this.HandleAo.ArtObject.ActorType == ActorType.Bookcase)
            {
              Vector3 vector3 = this.PlayerManager.Position - this.HandleAo.Position;
              flag = (double) vector3.Z < 2.75 && (double) vector3.Z > -0.25 && ((double) vector3.Y < -3.0 && (double) vector3.Y > -4.0) && this.CameraManager.Viewpoint == Viewpoint.Left;
            }
            else
            {
              Vector3 vector = (this.PlayerManager.Position - this.HandleAo.Position - new Vector3(0.0f, 1.5f, 0.0f)) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
              vector.X += vector.Z;
              Vector3 vector3 = FezMath.Abs(vector);
              flag = (double) vector3.X > 0.75 && (double) vector3.X < 1.75 && (double) vector3.Y < 1.0;
            }
            if (this.HandleAo.Enabled && flag && (this.PlayerManager.Grounded && this.PlayerManager.Action != ActionType.PushingPivot) && (FezButtonStateExtensions.IsDown(this.InputManager.GrabThrow) && this.PlayerManager.Action != ActionType.ReadingSign && (this.PlayerManager.Action != ActionType.FreeFalling && this.PlayerManager.Action != ActionType.Dying)))
            {
              this.SinceChanged = TimeSpan.Zero;
              return true;
            }
            else
              break;
          case SpinAction.Spinning:
            double num = FezMath.Saturate(this.SinceChanged.TotalSeconds / 1.5);
            float angle = Easing.EaseIn(num < 0.799999997019768 ? num / 0.799999997019768 : 1.0 + Math.Sin((num - 0.799999997019768) / 0.200000002980232 * 6.28318548202515 * 2.0) * 0.00999999977648258 * (1.0 - num) / 0.200000002980232, EasingType.Quartic) * 1.570796f * (float) this.SpinSign;
            Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle(Vector3.UnitY, angle);
            Vector3 position = this.HandleAo.Position;
            for (int index = 0; index < this.OriginalStates.Length; ++index)
            {
              TrileInstance instance = index < this.Group.Triles.Count ? this.Group.Triles[index] : this.AttachedTriles[index - this.Group.Triles.Count];
              Vector4 vector = this.OriginalStates[index];
              instance.Position = Vector3.Transform(FezMath.XYZ(vector) + new Vector3(0.5f) - position, fromAxisAngle) + position - new Vector3(0.5f);
              instance.Phi = FezMath.WrapAngle(vector.W + angle);
              this.LevelMaterializer.GetTrileMaterializer(instance.Trile).UpdateInstance(instance);
            }
            if (!this.HasShaken && num > 0.799999997019768)
            {
              ServiceHelper.AddComponent((IGameComponent) new CamShake(ServiceHelper.Game)
              {
                Distance = 0.25f,
                Duration = TimeSpan.FromSeconds(0.200000002980232)
              });
              this.HasShaken = true;
            }
            this.HandleAo.Rotation = this.OriginalAoRotation * fromAxisAngle;
            for (int index = 0; index < this.AttachedArtObjects.Length; ++index)
            {
              this.AttachedArtObjects[index].Rotation = this.AttachedAoRotations[index] * fromAxisAngle;
              this.AttachedArtObjects[index].Position = Vector3.Transform(this.AttachedAoOrigins[index] - position, fromAxisAngle) + position;
            }
            if (this.SinceChanged.TotalSeconds >= 1.5)
            {
              this.RotateTriles();
              this.SinceChanged -= TimeSpan.FromSeconds(1.5);
              this.State = SpinAction.Idle;
              break;
            }
            else
              break;
        }
        return false;
      }

      public void Spin()
      {
        this.PlayerManager.Action = ActionType.PushingPivot;
        Waiters.Wait(0.5, (Func<float, bool>) (_ => this.PlayerManager.Action != ActionType.PushingPivot), (Action) (() =>
        {
          if (this.PlayerManager.Action != ActionType.PushingPivot)
            return;
          this.SinceChanged = TimeSpan.Zero;
          this.OriginalAoRotation = this.HandleAo.Rotation;
          foreach (TrileInstance item_0 in this.Group.Triles)
          {
            if (item_0.InstanceId == -1)
              this.LevelMaterializer.CullInstanceIn(item_0);
          }
          Vector3 local_1 = (this.PlayerManager.Position - this.HandleAo.Position - new Vector3(0.0f, 1.5f, 0.0f)) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
          local_1.X += local_1.Z;
          this.SpinSign = this.HandleAo.ArtObject.ActorType != ActorType.Bookcase ? (int) ((double) FezMath.Dot(FezMath.Sign(FezMath.RightVector(this.CameraManager.Viewpoint)), Vector3.One) * (double) Math.Sign(local_1.X)) : 1;
          if (this.SpinSign == 1)
            SoundEffectExtensions.Emit(this.Host.RightSound);
          else
            SoundEffectExtensions.Emit(this.Host.LeftSound);
          this.AttachedTriles.Clear();
          foreach (TrileInstance item_1 in this.TopLayer)
            this.AddSupportedTrilesOver(item_1);
          this.OriginalStates = Enumerable.ToArray<Vector4>(Enumerable.Select<TrileInstance, Vector4>(Enumerable.Union<TrileInstance>((IEnumerable<TrileInstance>) this.Group.Triles, (IEnumerable<TrileInstance>) this.AttachedTriles), (Func<TrileInstance, Vector4>) (x => new Vector4(x.Position, x.Phi))));
          for (int local_3 = 0; local_3 < this.AttachedArtObjects.Length; ++local_3)
          {
            this.AttachedAoRotations[local_3] = this.AttachedArtObjects[local_3].Rotation;
            this.AttachedAoOrigins[local_3] = this.AttachedArtObjects[local_3].Position;
          }
          int local_4;
          if (!this.GameState.SaveData.ThisLevel.PivotRotations.TryGetValue(this.HandleAo.Id, out local_4))
            this.GameState.SaveData.ThisLevel.PivotRotations.Add(this.HandleAo.Id, this.SpinSign);
          else
            this.GameState.SaveData.ThisLevel.PivotRotations[this.HandleAo.Id] = local_4 + this.SpinSign;
          if (this.SpinSign == 1)
            this.PivotService.OnRotateRight(this.HandleAo.Id);
          else
            this.PivotService.OnRotateLeft(this.HandleAo.Id);
          this.HasShaken = false;
          this.State = SpinAction.Spinning;
          if (this.HandleAo.ArtObject.ActorType != ActorType.Bookcase)
            return;
          this.HandleAo.Enabled = false;
          this.GameState.SaveData.ThisLevel.InactiveArtObjects.Add(this.HandleAo.Id);
        }));
      }

      private void AddSupportedTrilesOver(TrileInstance instance)
      {
        TrileEmplacement id = new TrileEmplacement(instance.Emplacement.X, instance.Emplacement.Y + 1, instance.Emplacement.Z);
        TrileInstance instance1 = this.LevelManager.TrileInstanceAt(ref id);
        if (instance1 == null)
          return;
        this.AddSupportedTrile(instance1);
        if (!instance1.Overlaps)
          return;
        foreach (TrileInstance instance2 in instance1.OverlappedTriles)
          this.AddSupportedTrile(instance2);
      }

      private void AddSupportedTrile(TrileInstance instance)
      {
        if (this.AttachedTriles.Contains(instance) || this.Group.Triles.Contains(instance) || instance.PhysicsState == null && !ActorTypeExtensions.IsPickable(instance.Trile.ActorSettings.Type))
          return;
        this.AttachedTriles.Add(instance);
        this.AddSupportedTrilesOver(instance);
        TrileGroup trileGroup;
        if (!this.LevelManager.PickupGroups.TryGetValue(instance, out trileGroup))
          return;
        foreach (TrileInstance instance1 in trileGroup.Triles)
          this.AddSupportedTrile(instance1);
      }

      private void RotateTriles()
      {
        foreach (TrileInstance instance in Enumerable.ToArray<TrileInstance>(Enumerable.Union<TrileInstance>((IEnumerable<TrileInstance>) this.Group.Triles, (IEnumerable<TrileInstance>) this.AttachedTriles)))
          this.LevelManager.UpdateInstance(instance);
        if (!this.LevelManager.Groups.ContainsKey(this.Group.Id))
          throw new InvalidOperationException("Group was lost after pivot rotation!");
      }
    }
  }
}
