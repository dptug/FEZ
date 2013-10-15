// Type: FezGame.Components.RumblerHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class RumblerHost : GameComponent
  {
    private static readonly TimeSpan SignalDuration = TimeSpan.FromSeconds(0.25);
    private static readonly TimeSpan SilenceDuration = TimeSpan.FromSeconds(0.4);
    private readonly List<VibrationMotor> Input = new List<VibrationMotor>();
    private ArtObjectInstance ArtObject;
    private int CurrentIndex;
    private VibrationMotor CurrentSignal;
    private TimeSpan SinceChanged;
    private SoundEmitter eForkRumble;
    private SoundEffect ActivateSound;

    private TimeSpan CurrentDuration
    {
      get
      {
        if (this.CurrentSignal != VibrationMotor.None)
          return RumblerHost.SignalDuration;
        else
          return RumblerHost.SilenceDuration;
      }
    }

    [ServiceDependency]
    public ILevelService LevelService { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IInputManager InputManager { get; set; }

    [ServiceDependency]
    public ICodePatternService RumblerService { get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    static RumblerHost()
    {
    }

    public RumblerHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.CameraManager.ViewpointChanged += new Action(this.CheckForPattern);
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.Enabled = false;
      this.ArtObject = Enumerable.FirstOrDefault<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x =>
      {
        if (x.ArtObject.ActorType == ActorType.Rumbler)
          return x.ActorSettings.VibrationPattern != null;
        else
          return false;
      }));
      if (this.ArtObject != null)
      {
        this.Enabled = true;
        if (this.GameState.SaveData.ThisLevel.InactiveArtObjects.Contains(this.ArtObject.Id))
        {
          if (this.ArtObject.ActorSettings.AttachedGroup.HasValue)
          {
            int key = this.ArtObject.ActorSettings.AttachedGroup.Value;
            foreach (TrileInstance instance in this.LevelManager.Groups[this.ArtObject.ActorSettings.AttachedGroup.Value].Triles.ToArray())
              this.LevelManager.ClearTrile(instance);
            this.LevelManager.Groups.Remove(key);
          }
          this.LevelManager.ArtObjects.Remove(this.ArtObject.Id);
          this.ArtObject.Dispose();
          this.LevelMaterializer.RegisterSatellites();
          Vector3 position1 = this.ArtObject.Position;
          if (!this.GameState.SaveData.ThisLevel.DestroyedTriles.Contains(new TrileEmplacement(position1 - Vector3.One / 2f)))
          {
            Trile trile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.SecretCube));
            if (trile != null)
            {
              Vector3 position2 = position1 - Vector3.One / 2f;
              this.LevelManager.ClearTrile(new TrileEmplacement(position2));
              TrileInstance toAdd;
              this.LevelManager.RestoreTrile(toAdd = new TrileInstance(position2, trile.Id)
              {
                OriginalEmplacement = new TrileEmplacement(position2)
              });
              if (toAdd.InstanceId == -1)
                this.LevelMaterializer.CullInstanceIn(toAdd);
            }
          }
          this.Enabled = false;
        }
      }
      if (this.Enabled)
      {
        this.eForkRumble = SoundEffectExtensions.Emit(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Zu/ForkRumble"), true, 0.0f, 0.0f);
        this.ActivateSound = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/ForkActivate");
        this.ArtObject.ActorSettings.VibrationPattern = Util.JoinArrays<VibrationMotor>(this.ArtObject.ActorSettings.VibrationPattern, new VibrationMotor[3]);
      }
      else
      {
        this.ActivateSound = (SoundEffect) null;
        this.eForkRumble = (SoundEmitter) null;
      }
    }

    private void CheckForPattern()
    {
      if (!this.Enabled || this.GameState.Loading)
        return;
      this.Input.Add(FezMath.GetDistance(this.CameraManager.Viewpoint, this.CameraManager.LastViewpoint) == 1 ? VibrationMotor.LeftLow : VibrationMotor.RightHigh);
      if (this.Input.Count > 16)
        this.Input.RemoveAt(0);
      if (!PatternTester.Test((IList<VibrationMotor>) this.Input, this.ArtObject.ActorSettings.VibrationPattern))
        return;
      this.Input.Clear();
      this.RumblerService.OnActivate(this.ArtObject.Id);
      this.Enabled = false;
      Waiters.Wait((Func<bool>) (() => this.CameraManager.ViewTransitionReached), new Action(this.Solve));
    }

    private void Solve()
    {
      foreach (Volume volume in (IEnumerable<Volume>) this.LevelManager.Volumes.Values)
      {
        if (volume.ActorSettings != null && volume.ActorSettings.IsPointOfInterest && (volume.BoundingBox.Contains(this.ArtObject.Bounds) != ContainmentType.Disjoint && volume.Enabled))
        {
          volume.Enabled = false;
          this.GameState.SaveData.ThisLevel.InactiveVolumes.Add(volume.Id);
        }
      }
      this.SoundManager.MusicVolumeFactor = 1f;
      this.eForkRumble.Cue.Stop(false);
      SoundEffectExtensions.EmitAt(this.ActivateSound, this.ArtObject.Position);
      ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(this.Game, this.ArtObject, this.ArtObject.Position)
      {
        FlashOnSpawn = true
      });
      this.GameState.SaveData.ThisLevel.InactiveArtObjects.Add(this.ArtObject.Id);
      this.LevelService.ResolvePuzzle();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.Loading || (this.GameState.InMap || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)))
        return;
      this.SinceChanged += gameTime.ElapsedGameTime;
      if (this.SinceChanged >= this.CurrentDuration)
      {
        ++this.CurrentIndex;
        if (this.CurrentIndex >= this.ArtObject.ActorSettings.VibrationPattern.Length || this.CurrentIndex < 0)
          this.CurrentIndex = 0;
        this.SinceChanged -= this.CurrentDuration;
        this.CurrentSignal = this.ArtObject.ActorSettings.VibrationPattern.Length != 0 ? this.ArtObject.ActorSettings.VibrationPattern[this.CurrentIndex] : VibrationMotor.None;
      }
      float num = FezMath.Saturate(1f - FezMath.Saturate((FezMath.Abs((this.PlayerManager.Center - this.ArtObject.Position) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint)) - new Vector3(0.5f, 2f, 0.5f)) / 3f).Length());
      if (this.CurrentSignal == VibrationMotor.None)
      {
        this.eForkRumble.VolumeFactor *= Math.Max((float) (1.0 - Math.Pow(this.SinceChanged.TotalSeconds / this.CurrentDuration.TotalSeconds, 4.0)), 0.75f);
        this.SoundManager.MusicVolumeFactor = (float) (1.0 - (double) num * 0.600000023841858 - (double) this.eForkRumble.VolumeFactor * 0.200000002980232);
      }
      else
      {
        this.eForkRumble.VolumeFactor = num;
        this.eForkRumble.Pan = this.CurrentSignal == VibrationMotor.RightHigh ? 1f : -1f;
        this.SoundManager.MusicVolumeFactor = (float) (1.0 - (double) num * 0.800000011920929);
        if ((double) num != 1.0)
          num *= 0.5f;
        if (this.CurrentSignal == VibrationMotor.LeftLow)
          num *= 0.5f;
        if ((double) num > 0.0)
          this.InputManager.ActiveGamepad.Vibrate(this.CurrentSignal, (double) num, this.CurrentDuration - this.SinceChanged, EasingType.None);
        else
          this.Input.Clear();
      }
    }
  }
}
