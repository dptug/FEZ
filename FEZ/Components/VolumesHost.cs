// Type: FezGame.Components.VolumesHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Services.Scripting;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  public class VolumesHost : GameComponent
  {
    private readonly List<CodeInput> Input = new List<CodeInput>();
    private Volume[] levelVolumes;
    private TimeSpan SinceInput;
    private bool deferredScripts;
    private bool checkForContainment;

    [ServiceDependency]
    public ILevelService LevelService { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IVolumeService VolumeService { private get; set; }

    public VolumesHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.RegisterVolumes();
      this.TestVolumes(false);
      this.LevelManager.LevelChanged += new Action(this.DisableTriggeredVolumes);
      this.LevelManager.LevelChanged += new Action(this.RegisterVolumes);
      this.LevelManager.LevelChanged += (Action) (() => this.TestVolumes(true));
    }

    private void DisableTriggeredVolumes()
    {
      foreach (Volume volume in (IEnumerable<Volume>) this.LevelManager.Volumes.Values)
      {
        if (volume.ActorSettings != null && volume.ActorSettings.NeedsTrigger)
          volume.Enabled = false;
      }
      foreach (int index in this.GameState.SaveData.ThisLevel.InactiveVolumes)
      {
        if (!(this.LevelManager.Name == "ZU_CITY_RUINS") || index != 2)
          this.LevelManager.Volumes[index].Enabled = !this.LevelManager.Volumes[index].Enabled;
      }
      this.checkForContainment = this.LevelManager.Name == "RITUAL";
      this.Input.Clear();
    }

    public override void Update(GameTime gameTime)
    {
      if (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.GameState.InMap || (this.GameState.Paused || this.GameState.Loading) || this.LevelManager.IsInvalidatingScreen)
        return;
      if (this.LevelManager.Volumes.Count != this.levelVolumes.Length || this.VolumeService.RegisterNeeded)
        this.RegisterVolumes();
      if (this.levelVolumes.Length == 0)
        return;
      this.TestVolumes(false);
      this.SinceInput += gameTime.ElapsedGameTime;
    }

    private void HeightCheck()
    {
      SoundService.ImmediateEffect = true;
      foreach (Volume volume in this.levelVolumes)
      {
        bool flag = (double) this.CameraManager.Center.Y > ((double) volume.From.Y + (double) volume.To.Y) / 2.0;
        if (flag)
          this.VolumeService.OnGoHigher(volume.Id);
        else
          this.VolumeService.OnGoLower(volume.Id);
        volume.PlayerIsHigher = new bool?(flag);
      }
    }

    private void RegisterVolumes()
    {
      this.VolumeService.RegisterNeeded = false;
      this.levelVolumes = Enumerable.ToArray<Volume>((IEnumerable<Volume>) this.LevelManager.Volumes.Values);
    }

    private void TestVolumes(bool force)
    {
      if (!force && this.GameState.Loading)
        return;
      if (!force && this.deferredScripts)
      {
        foreach (Volume volume in this.PlayerManager.CurrentVolumes)
          this.VolumeService.OnEnter(volume.Id);
        this.HeightCheck();
        this.deferredScripts = false;
      }
      else
        SoundService.ImmediateEffect = false;
      if (force)
        this.deferredScripts = true;
      Vector3 mask = FezMath.GetMask(FezMath.VisibleAxis(this.CameraManager.Viewpoint));
      Vector3 vector3 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      if (this.PlayerManager.Background)
        vector3 *= -1f;
      Ray ray = new Ray()
      {
        Position = this.PlayerManager.Center * (Vector3.One - mask) - vector3 * this.LevelManager.Size,
        Direction = vector3
      };
      if (this.PlayerManager.Action == ActionType.PullUpBack || this.PlayerManager.Action == ActionType.PullUpFront || this.PlayerManager.Action == ActionType.PullUpCornerLedge)
        ray.Position += new Vector3(0.0f, 0.5f, 0.0f);
      foreach (Volume volume in this.levelVolumes)
      {
        if (volume.Enabled)
        {
          if (!this.GameState.FarawaySettings.InTransition)
          {
            bool flag = (double) this.CameraManager.Center.Y > ((double) volume.From.Y + (double) volume.To.Y) / 2.0;
            if (!volume.PlayerIsHigher.HasValue || flag != volume.PlayerIsHigher.Value)
            {
              if (flag)
                this.VolumeService.OnGoHigher(volume.Id);
              else
                this.VolumeService.OnGoLower(volume.Id);
              volume.PlayerIsHigher = new bool?(flag);
            }
          }
          if (this.checkForContainment && (volume.Id == 1 || volume.Id == 2))
          {
            if (volume.BoundingBox.Contains(this.PlayerManager.Position) != ContainmentType.Disjoint)
              this.PlayerIsInside(volume, force);
          }
          else
          {
            float? nullable = volume.BoundingBox.Intersects(ray);
            if (volume.ActorSettings != null && volume.ActorSettings.IsBlackHole)
            {
              if (!nullable.HasValue)
                nullable = volume.BoundingBox.Intersects(new Ray(ray.Position + new Vector3(0.0f, 0.3f, 0.0f), ray.Direction));
              if (!nullable.HasValue)
                nullable = volume.BoundingBox.Intersects(new Ray(ray.Position - new Vector3(0.0f, 0.3f, 0.0f), ray.Direction));
            }
            if (nullable.HasValue)
            {
              bool flag = false;
              bool isBlackHole = volume.ActorSettings != null && volume.ActorSettings.IsBlackHole;
              if (this.PlayerManager.CarriedInstance != null)
                this.PlayerManager.CarriedInstance.PhysicsState.UpdatingPhysics = true;
              NearestTriles nearestTriles = this.LevelManager.NearestTrile(ray.Position, this.PlayerManager.Background ? QueryOptions.Background : QueryOptions.None);
              if (this.LevelManager.Name != "PIVOT_TWO" && nearestTriles.Surface != null)
                flag = flag | this.TestObstruction(nearestTriles.Surface, nullable.Value, ray.Position, isBlackHole);
              if (nearestTriles.Deep != null)
                flag = flag | this.TestObstruction(nearestTriles.Deep, nullable.Value, ray.Position, isBlackHole);
              if (this.PlayerManager.CarriedInstance != null)
                this.PlayerManager.CarriedInstance.PhysicsState.UpdatingPhysics = false;
              if (!flag && (volume.ActorSettings != null && volume.ActorSettings.IsBlackHole || volume.Orientations.Contains(this.CameraManager.VisibleOrientation)))
                this.PlayerIsInside(volume, force);
            }
          }
        }
      }
      for (int index = this.PlayerManager.CurrentVolumes.Count - 1; index >= 0; --index)
      {
        Volume volume = this.PlayerManager.CurrentVolumes[index];
        if (!volume.PlayerInside)
        {
          if (!force)
            this.VolumeService.OnExit(volume.Id);
          this.PlayerManager.CurrentVolumes.RemoveAt(index);
        }
        volume.PlayerInside = false;
      }
      if (this.PlayerManager.CurrentVolumes.Count <= 0)
        return;
      if (this.PlayerManager.Action == ActionType.LesserWarp || this.PlayerManager.Action == ActionType.GateWarp)
        this.Input.Clear();
      if (!this.GrabInput())
        return;
      foreach (Volume volume in this.PlayerManager.CurrentVolumes)
      {
        if (volume.ActorSettings != null && volume.ActorSettings.CodePattern != null && volume.ActorSettings.CodePattern.Length > 0)
          this.TestCodePattern(volume);
      }
    }

    private bool GrabInput()
    {
      CodeInput codeInput = CodeInput.None;
      if (this.InputManager.Jump == FezButtonState.Pressed)
        codeInput = CodeInput.Jump;
      else if (this.InputManager.RotateRight == FezButtonState.Pressed)
        codeInput = CodeInput.SpinRight;
      else if (this.InputManager.RotateLeft == FezButtonState.Pressed)
        codeInput = CodeInput.SpinLeft;
      else if (this.InputManager.Left == FezButtonState.Pressed)
        codeInput = CodeInput.Left;
      else if (this.InputManager.Right == FezButtonState.Pressed)
        codeInput = CodeInput.Right;
      else if (this.InputManager.Up == FezButtonState.Pressed)
        codeInput = CodeInput.Up;
      else if (this.InputManager.Down == FezButtonState.Pressed)
        codeInput = CodeInput.Down;
      if (codeInput == CodeInput.None)
        return false;
      this.Input.Add(codeInput);
      if (this.Input.Count > 16)
        this.Input.RemoveAt(0);
      return true;
    }

    private void TestCodePattern(Volume volume)
    {
      if (PatternTester.Test((IList<CodeInput>) this.Input, volume.ActorSettings.CodePattern))
      {
        this.Input.Clear();
        Waiters.Wait((Func<bool>) (() => this.CameraManager.ViewTransitionReached), (Action) (() =>
        {
          this.VolumeService.OnCodeAccepted(volume.Id);
          this.GameState.SaveData.AnyCodeDeciphered = true;
          this.LevelService.ResolvePuzzle();
        }));
      }
      this.SinceInput = TimeSpan.Zero;
    }

    private bool TestObstruction(TrileInstance trile, float hitDistance, Vector3 hitStart, bool isBlackHole)
    {
      Vector3 b = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      if (this.PlayerManager.Background)
        b *= -1f;
      if (trile != null && trile.Enabled && !trile.Trile.Immaterial && (trile.Trile.ActorSettings.Type != ActorType.Hole || isBlackHole))
        return (double) FezMath.Dot(trile.Emplacement.AsVector + Vector3.One / 2f + b * -0.5f - hitStart, b) <= (double) hitDistance + 0.25;
      else
        return false;
    }

    private void PlayerIsInside(Volume volume, bool force)
    {
      volume.PlayerInside = true;
      if (this.PlayerManager.CurrentVolumes.Contains(volume))
        return;
      this.PlayerManager.CurrentVolumes.Add(volume);
      if (force)
        return;
      this.VolumeService.OnEnter(volume.Id);
    }
  }
}
