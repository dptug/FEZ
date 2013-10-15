// Type: FezGame.Components.BigWaterfallHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components.Scripting;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  public class BigWaterfallHost : GameComponent, IBigWaterfallService, IScriptingBase
  {
    private BackgroundPlane WaterfallPlane;
    private BackgroundPlane SplashPlane;
    private BackgroundPlane MoriaPlane;
    private int ScriptId;
    private AnimatedTexture OpenSplash;
    private AnimatedTexture OpeningSplash;
    private AnimatedTexture OpenWaterfall;
    private AnimatedTexture OpeningWaterfall;
    private SoundEffect sWaterfallOpening;
    private SoundEmitter eWaterfallClosed;
    private SoundEmitter eWaterfallOpen;
    private float sinceAlive;
    private bool opening;
    private float Top;
    private Vector3 TerminalPosition;

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    [ServiceDependency]
    internal IScriptingManager ScriptingManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    public BigWaterfallHost(Game game)
      : base(game)
    {
      this.Enabled = false;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.OpenSplash = this.OpeningSplash = this.OpenWaterfall = this.OpeningWaterfall = (AnimatedTexture) null;
      this.sWaterfallOpening = (SoundEffect) null;
      this.eWaterfallClosed = (SoundEmitter) null;
      this.eWaterfallOpen = (SoundEmitter) null;
      this.WaterfallPlane = Enumerable.FirstOrDefault<BackgroundPlane>((IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values, (Func<BackgroundPlane, bool>) (x => x.ActorType == ActorType.BigWaterfall));
      this.Enabled = this.WaterfallPlane != null;
      if (!this.Enabled)
        return;
      this.MoriaPlane = Enumerable.FirstOrDefault<BackgroundPlane>((IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values, (Func<BackgroundPlane, bool>) (x => x.TextureName == "MORIA_GLOW"));
      if (this.GameState.SaveData.ThisLevel.InactiveGroups.Contains(1) || this.GameState.SaveData.ThisLevel.InactiveVolumes.Contains(19))
        this.MoriaPlane.Opacity = 0.0f;
      this.MoriaPlane.Position -= Vector3.UnitX * (1.0 / 1000.0);
      Comparison<Group> oldGo = this.LevelMaterializer.StaticPlanesMesh.GroupOrder;
      this.LevelMaterializer.StaticPlanesMesh.GroupOrder = (Comparison<Group>) ((x, y) =>
      {
        if (x == this.MoriaPlane.Group)
          return 1;
        if (y != this.MoriaPlane.Group)
          return oldGo(x, y);
        else
          return -1;
      });
      bool flag = this.GameState.SaveData.ThisLevel.ScriptingState == "WATERFALL_OPEN";
      if (flag)
      {
        this.OpenSplash = this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/water_giant_splash_open");
        this.OpenWaterfall = this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/water_giant_open");
        Waiters.Wait((Func<bool>) (() => !this.GameState.Loading), (Action) (() => this.LevelManager.Volumes[7].Enabled = true));
      }
      else
        this.ForkLoad(false);
      this.SplashPlane = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, flag ? this.OpenSplash : this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/water_giant_splash"))
      {
        Doublesided = true
      };
      this.LevelManager.AddPlane(this.SplashPlane);
      this.Top = FezMath.Dot(this.WaterfallPlane.Position + this.WaterfallPlane.Scale * this.WaterfallPlane.Size / 2f, Vector3.UnitY);
      this.TerminalPosition = this.WaterfallPlane.Position - this.WaterfallPlane.Scale * this.WaterfallPlane.Size / 2f * Vector3.UnitY + Vector3.Transform(Vector3.UnitZ, this.WaterfallPlane.Rotation) / 16f;
      this.sinceAlive = 0.0f;
      if (flag)
      {
        this.SwapOpened();
        this.eWaterfallOpen = SoundEffectExtensions.EmitAt(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/BigWaterfallOpen"), this.WaterfallPlane.Position, true, 0.0f, 0.0f);
      }
      else
        this.eWaterfallClosed = SoundEffectExtensions.EmitAt(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/BigWaterfallClosed"), this.WaterfallPlane.Position, true, 0.0f, 0.0f);
    }

    private void ForkLoad(bool dummy)
    {
      this.OpenSplash = this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/water_giant_splash_open");
      this.OpenWaterfall = this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/water_giant_open");
      this.OpeningSplash = this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/water_giant_splash_opening");
      this.OpeningWaterfall = this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/water_giant_opening");
      this.sWaterfallOpening = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/BigWaterfallOpening");
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || !this.CameraManager.ActionRunning))
        return;
      bool flag = !this.GameState.FarawaySettings.InTransition && !ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action);
      this.sinceAlive = FezMath.Saturate(this.sinceAlive + (float) (gameTime.ElapsedGameTime.TotalSeconds / 2.0 * (flag ? 1.0 : -1.0)));
      if (!this.opening)
      {
        if (this.eWaterfallClosed != null)
          this.eWaterfallClosed.VolumeFactor = this.sinceAlive;
        if (this.eWaterfallOpen != null)
          this.eWaterfallOpen.VolumeFactor = this.sinceAlive;
      }
      if (!this.GameState.SaveData.ThisLevel.InactiveGroups.Contains(1) && !this.GameState.SaveData.ThisLevel.InactiveVolumes.Contains(19))
        this.MoriaPlane.Opacity = this.TimeManager.NightContribution;
      float num1 = (float) ((double) this.LevelManager.WaterHeight - 1.0 + 5.0 / 16.0);
      if ((double) this.TerminalPosition.Y <= (double) num1)
      {
        float num2 = this.Top - num1;
        if ((double) num2 <= 0.0)
        {
          if ((double) this.SplashPlane.Opacity == 0.0)
            return;
          this.SplashPlane.Opacity = 0.0f;
          this.WaterfallPlane.Opacity = 0.0f;
        }
        else
        {
          if ((double) this.SplashPlane.Opacity != 1.0)
          {
            this.SplashPlane.Opacity = 1f;
            this.WaterfallPlane.Opacity = 1f;
          }
          this.SplashPlane.Position = num1 * Vector3.UnitY + this.SplashPlane.Size / 2f * Vector3.UnitY + FezMath.XZMask * this.TerminalPosition;
          this.WaterfallPlane.Scale = new Vector3(this.WaterfallPlane.Scale.X, num2 / this.WaterfallPlane.Size.Y, this.WaterfallPlane.Scale.Z);
          this.WaterfallPlane.Position = new Vector3(this.WaterfallPlane.Position.X, num1 + num2 / 2f, this.WaterfallPlane.Position.Z);
        }
      }
      else
      {
        if (this.SplashPlane == null || (double) this.SplashPlane.Opacity == 0.0)
          return;
        this.SplashPlane.Opacity = 0.0f;
      }
    }

    public void ResetEvents()
    {
    }

    public LongRunningAction Open(int id)
    {
      if (id != this.WaterfallPlane.Id)
        return (LongRunningAction) null;
      this.GameState.SaveData.ThisLevel.ScriptingState = "WATERFALL_OPEN";
      this.GameState.Save();
      SoundEmitter eWaterfallOpen = SoundEffectExtensions.EmitAt(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/BigWaterfallOpen"), this.WaterfallPlane.Position, true, true);
      eWaterfallOpen.VolumeFactor = 0.0f;
      eWaterfallOpen.Cue.Play();
      this.opening = true;
      Waiters.Interpolate(6.0, (Action<float>) (s =>
      {
        if (this.eWaterfallClosed == null || this.eWaterfallClosed.Dead)
          return;
        this.eWaterfallClosed.VolumeFactor = (1f - s) * this.sinceAlive;
        if (eWaterfallOpen.Dead)
          return;
        eWaterfallOpen.VolumeFactor = s * this.sinceAlive;
      })).AutoPause = true;
      SoundEffectExtensions.EmitAt(this.sWaterfallOpening, this.WaterfallPlane.Position);
      this.ScriptId = this.ScriptingManager.EvaluatedScript.Script.Id;
      this.LevelManager.RemovePlane(this.WaterfallPlane);
      this.LevelManager.RemovePlane(this.SplashPlane);
      Vector3 position1 = this.SplashPlane.Position;
      Vector3 scale1 = this.SplashPlane.Scale;
      Quaternion rotation1 = this.SplashPlane.Rotation;
      this.SplashPlane = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.OpeningSplash)
      {
        Doublesided = true
      };
      this.LevelManager.AddPlane(this.SplashPlane);
      this.SplashPlane.Timing.Restart();
      this.WaterfallPlane.Timing.Loop = false;
      this.SplashPlane.Position = position1;
      this.SplashPlane.Scale = scale1;
      this.SplashPlane.Rotation = rotation1;
      Vector3 position2 = this.WaterfallPlane.Position;
      Vector3 scale2 = this.WaterfallPlane.Scale;
      Quaternion rotation2 = this.WaterfallPlane.Rotation;
      this.WaterfallPlane = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.OpeningWaterfall)
      {
        Doublesided = true
      };
      this.LevelManager.AddPlane(this.WaterfallPlane);
      this.WaterfallPlane.Timing.Restart();
      this.WaterfallPlane.Timing.Loop = false;
      this.WaterfallPlane.YTextureRepeat = true;
      this.WaterfallPlane.Position = position2;
      this.WaterfallPlane.Scale = scale2;
      this.WaterfallPlane.Rotation = rotation2;
      return new LongRunningAction((Func<float, float, bool>) ((_, __) =>
      {
        if (!this.WaterfallPlane.Timing.Ended)
          return false;
        this.opening = false;
        this.SwapOpened();
        return true;
      }));
    }

    private void SwapOpened()
    {
      this.LevelManager.RemovePlane(this.WaterfallPlane);
      this.LevelManager.RemovePlane(this.SplashPlane);
      Vector3 position1 = this.SplashPlane.Position;
      Vector3 scale1 = this.SplashPlane.Scale;
      Quaternion rotation1 = this.SplashPlane.Rotation;
      this.SplashPlane = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.OpenSplash)
      {
        Doublesided = true
      };
      this.LevelManager.AddPlane(this.SplashPlane);
      this.SplashPlane.Timing.Restart();
      this.WaterfallPlane.Timing.Loop = true;
      this.SplashPlane.Position = position1;
      this.SplashPlane.Scale = scale1;
      this.SplashPlane.Rotation = rotation1;
      Vector3 position2 = this.WaterfallPlane.Position;
      Vector3 scale2 = this.WaterfallPlane.Scale;
      Quaternion rotation2 = this.WaterfallPlane.Rotation;
      this.WaterfallPlane = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.OpenWaterfall)
      {
        Doublesided = true
      };
      this.LevelManager.AddPlane(this.WaterfallPlane);
      this.WaterfallPlane.Timing.Restart();
      this.WaterfallPlane.Timing.Loop = true;
      this.WaterfallPlane.YTextureRepeat = true;
      this.WaterfallPlane.Position = position2;
      this.WaterfallPlane.Scale = scale2;
      this.WaterfallPlane.Rotation = rotation2;
    }
  }
}
