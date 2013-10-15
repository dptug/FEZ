// Type: FezEngine.Services.SoundManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using ContentSerialization;
using FezEngine.Components;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FezEngine.Services
{
  public class SoundManager : GameComponent, ISoundManager
  {
    private readonly List<SoundEmitter> toUpdate = new List<SoundEmitter>(5);
    private const string MusicFolderName = "3rcqng1i.djk";
    private const float VolumeFadeSeconds = 2f;
    private const float LowFrequency = 0.025f;
    private const float MasterMusicVolume = 1f;
    private const int SoundPerUpdate = 5;
    public static bool NoMusic;
    private string MusicTempDir;
    private Dictionary<string, string> MusicAliases;
    private OggStreamer Streamer;
    private ActiveTrackedSong ActiveSong;
    private ActiveTrackedSong ShelvedSong;
    private ActiveAmbience ActiveAmbience;
    private VolumeLevels VolumeLevels;
    private static bool VolumeLevelsFirstLoadDone;
    private float FrequencyStep;
    public static bool NoMoreSounds;
    private bool initialized;
    private int firstIndex;
    private float musicVolume;
    private float musicVolumeFactor;
    private float soundEffectVolume;
    private float globalVolumeFactor;
    private IWaiter freqTransitionWaiter;
    private IWaiter volTransitionWaiter;

    public bool IsLowPass { get; private set; }

    public List<SoundEmitter> Emitters { get; private set; }

    public Vector2 LimitDistance { get; private set; }

    public bool ScriptChangedSong { get; set; }

    public float MusicVolume
    {
      get
      {
        return this.musicVolume;
      }
      set
      {
        this.musicVolume = FezMath.Saturate(value);
        if (!OggStreamer.HasInstance)
          return;
        OggStreamer.Instance.MusicVolume = FezMath.Saturate(Easing.EaseIn((double) this.musicVolume * (double) this.globalVolumeFactor, EasingType.Quadratic) * 1f * this.musicVolumeFactor);
      }
    }

    public float MusicVolumeFactor
    {
      get
      {
        return this.musicVolumeFactor;
      }
      set
      {
        this.musicVolumeFactor = FezMath.Saturate(value);
        this.MusicVolume = this.musicVolume;
        if (!OggStreamer.HasInstance)
          return;
        OggStreamer.Instance.AmbienceVolume = SoundEffect.MasterVolume * this.musicVolumeFactor;
      }
    }

    public float SoundEffectVolume
    {
      get
      {
        return this.soundEffectVolume;
      }
      set
      {
        this.soundEffectVolume = FezMath.Saturate(value);
        SoundEffect.MasterVolume = FezMath.Saturate(Easing.EaseIn((double) this.soundEffectVolume * (double) this.globalVolumeFactor, EasingType.Quadratic));
        if (!OggStreamer.HasInstance)
          return;
        OggStreamer.Instance.AmbienceVolume = SoundEffect.MasterVolume * this.musicVolumeFactor;
      }
    }

    public float GlobalVolumeFactor
    {
      get
      {
        return this.globalVolumeFactor;
      }
      set
      {
        this.globalVolumeFactor = value;
        this.SoundEffectVolume = this.soundEffectVolume;
        this.MusicVolume = this.musicVolume;
      }
    }

    public TimeSpan PlayPosition
    {
      get
      {
        if (this.ActiveSong != null)
          return this.ActiveSong.PlayPosition;
        else
          return TimeSpan.Zero;
      }
    }

    public TrackedSong CurrentlyPlayingSong
    {
      get
      {
        if (this.ActiveSong != null)
          return this.ActiveSong.Song;
        else
          return (TrackedSong) null;
      }
    }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { protected get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    public event Action SongChanged;

    public SoundManager(Game game)
      : base(game)
    {
      this.Emitters = new List<SoundEmitter>();
      this.UpdateOrder = 100;
    }

    public override void Initialize()
    {
      this.Streamer = new OggStreamer(22050, 60f);
      this.musicVolumeFactor = 1f;
      this.MusicVolume = SettingsManager.Settings.MusicVolume;
      this.SoundEffectVolume = SettingsManager.Settings.SoundVolume;
      this.GlobalVolumeFactor = 1f;
      this.CameraManager.ProjectionChanged += new Action(this.UpdateLimitDistance);
      this.UpdateLimitDistance();
      this.LevelManager.LevelChanging += new Action(this.KillSounds);
      this.EngineState.PauseStateChanged += (Action) (() =>
      {
        if (this.EngineState.Paused)
          this.Pause();
        else
          this.Resume();
      });
    }

    public void InitializeLibrary()
    {
      if (this.initialized)
        return;
      this.initialized = true;
      this.MusicTempDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ\\3rcqng1i.djk");
      if (Directory.Exists(this.MusicTempDir))
        Directory.Delete(this.MusicTempDir, true);
      Directory.CreateDirectory(this.MusicTempDir);
      using (FileStream fileStream1 = File.OpenRead(Path.Combine("Content", "Music.pak")))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream1))
        {
          int capacity = binaryReader.ReadInt32();
          this.MusicAliases = new Dictionary<string, string>(capacity);
          for (int index = 0; index < capacity; ++index)
          {
            string key = binaryReader.ReadString();
            int count = binaryReader.ReadInt32();
            string path = Path.Combine(this.MusicTempDir, Path.GetRandomFileName());
            using (FileStream fileStream2 = File.Create(path))
              fileStream2.Write(binaryReader.ReadBytes(count), 0, count);
            if (this.MusicAliases.ContainsKey(key))
              Logger.Log("SoundManager", "Skipped " + key + " track because it was already loaded");
            else
              this.MusicAliases.Add(key, path);
          }
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.Streamer != null)
        this.Streamer.Dispose();
      this.Game.Disposed += (EventHandler<EventArgs>) ((_, __) =>
      {
        try
        {
          Directory.Delete(this.MusicTempDir, true);
        }
        catch (Exception exception_0)
        {
        }
      });
    }

    private void UpdateLimitDistance()
    {
      this.LimitDistance = new Vector2(1f, 1f / this.CameraManager.AspectRatio) * this.CameraManager.Radius / 2f;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Loading)
        return;
      this.UpdateEmitters();
    }

    private void UpdateEmitters()
    {
      for (int index = this.Emitters.Count - 1; index >= 0; --index)
      {
        if (this.Emitters[index].Dead)
        {
          this.Emitters[index].Dispose();
          this.Emitters.RemoveAt(index);
        }
      }
      this.FactorizeVolume();
    }

    public void FactorizeVolume()
    {
      if (this.Emitters.Count == 0)
        return;
      float num1 = 0.0f;
      int num2 = 0;
      this.firstIndex = this.firstIndex % this.Emitters.Count;
      this.toUpdate.Clear();
      int index = this.firstIndex;
      int num3 = 0;
      while (num3 < this.Emitters.Count)
      {
        if (index == this.Emitters.Count)
          index -= this.Emitters.Count;
        SoundEmitter soundEmitter = this.Emitters[index];
        num1 += soundEmitter.NonFactorizedVolume;
        if ((double) soundEmitter.NonFactorizedVolume != 0.0)
          ++num2;
        if (num3 < 5 || soundEmitter.New)
        {
          this.toUpdate.Add(soundEmitter);
          soundEmitter.New = false;
        }
        ++num3;
        ++index;
      }
      this.firstIndex += 5;
      float num4 = (float) (-1.0 / (num2 == 0 ? 1.0 : (double) num2) + 2.0);
      foreach (SoundEmitter soundEmitter in this.toUpdate)
      {
        if (soundEmitter.FactorizeVolume)
          soundEmitter.VolumeFactor = FezMath.Saturate(num4 / ((double) num1 == 0.0 ? 1f : num1));
        soundEmitter.Update();
      }
    }

    public void FadeFrequencies(bool lowPass)
    {
      this.FadeFrequencies(lowPass, 2f);
    }

    public void FadeFrequencies(bool toLowPass, float fadeDuration)
    {
      if (!this.IsLowPass && toLowPass)
      {
        float originalStep = this.FrequencyStep;
        IWaiter thisWaiter = (IWaiter) null;
        this.freqTransitionWaiter = thisWaiter = Waiters.Interpolate((double) fadeDuration * (1.0 - (double) originalStep), (Action<float>) (s =>
        {
          if (this.freqTransitionWaiter != thisWaiter)
            return;
          this.FrequencyStep = originalStep + s * (1f - originalStep);
          if (!OggStreamer.HasInstance)
            return;
          OggStreamer.Instance.LowPassHFGain = MathHelper.Lerp(1f, 0.025f, Easing.EaseOut((double) this.FrequencyStep, EasingType.Cubic));
        }), (Action) (() =>
        {
          if (this.freqTransitionWaiter != thisWaiter)
            return;
          this.FrequencyStep = 1f;
          if (!OggStreamer.HasInstance)
            return;
          OggStreamer.Instance.LowPassHFGain = 0.025f;
        }));
      }
      else if (this.IsLowPass && !toLowPass)
      {
        float originalStep = this.FrequencyStep;
        IWaiter thisWaiter = (IWaiter) null;
        this.freqTransitionWaiter = thisWaiter = Waiters.Interpolate((double) fadeDuration * (double) originalStep, (Action<float>) (s =>
        {
          if (this.freqTransitionWaiter != thisWaiter)
            return;
          this.FrequencyStep = originalStep * (1f - s);
          if (!OggStreamer.HasInstance)
            return;
          OggStreamer.Instance.LowPassHFGain = MathHelper.Lerp(1f, 0.025f, Easing.EaseIn((double) this.FrequencyStep, EasingType.Cubic));
        }), (Action) (() =>
        {
          if (this.freqTransitionWaiter != thisWaiter)
            return;
          this.FrequencyStep = 0.0f;
          if (!OggStreamer.HasInstance)
            return;
          OggStreamer.Instance.LowPassHFGain = 1f;
        }));
      }
      this.IsLowPass = toLowPass;
    }

    public void FadeVolume(float fromVolume, float toVolume, float overSeconds)
    {
      IWaiter thisWaiter = (IWaiter) null;
      this.volTransitionWaiter = thisWaiter = Waiters.Interpolate((double) overSeconds, (Action<float>) (step =>
      {
        if (this.volTransitionWaiter != thisWaiter || this.EngineState.DotLoading)
          return;
        this.MusicVolumeFactor = MathHelper.Lerp(fromVolume, toVolume, step);
      }), (Action) (() =>
      {
        if (this.volTransitionWaiter != thisWaiter)
          return;
        if (!this.EngineState.DotLoading)
          this.MusicVolumeFactor = toVolume;
        this.volTransitionWaiter = (IWaiter) null;
      }));
    }

    public void Pause()
    {
      foreach (SoundEmitter soundEmitter in this.Emitters)
      {
        if (!soundEmitter.Dead && soundEmitter.Cue.State == SoundState.Playing)
        {
          soundEmitter.Cue.Pause();
          soundEmitter.WasPlaying = true;
        }
      }
      if (this.ActiveSong != null)
        this.ActiveSong.Pause();
      if (this.ActiveAmbience == null)
        return;
      this.ActiveAmbience.Pause();
    }

    public void Resume()
    {
      foreach (SoundEmitter soundEmitter in this.Emitters)
      {
        if (!soundEmitter.Dead && soundEmitter.WasPlaying && soundEmitter.Cue.State == SoundState.Paused)
          soundEmitter.Cue.Resume();
      }
      if (this.ActiveSong != null)
        this.ActiveSong.Resume();
      if (this.ActiveAmbience == null)
        return;
      this.ActiveAmbience.Resume();
    }

    public void KillSounds()
    {
      foreach (SoundEmitter soundEmitter in this.Emitters.ToArray())
      {
        if (!soundEmitter.Persistent)
        {
          if (!soundEmitter.Dead)
            soundEmitter.Dispose();
          this.Emitters.Remove(soundEmitter);
        }
      }
    }

    public void KillSounds(float fadeDuration)
    {
      foreach (SoundEmitter soundEmitter in this.Emitters.ToArray())
      {
        if (!soundEmitter.Persistent)
        {
          soundEmitter.FadeOutAndDie(fadeDuration, false);
          this.Emitters.Remove(soundEmitter);
        }
      }
    }

    public OggStream GetCue(string name, bool asyncPrecache = false)
    {
      OggStream oggStream = (OggStream) null;
      try
      {
        string str = name.Replace(" ^ ", "\\");
        bool flag = name.Contains("Ambience");
        oggStream = new OggStream(this.MusicAliases[str.ToLower(CultureInfo.InvariantCulture)], 6)
        {
          Category = flag ? "Ambience" : "Music",
          IsLooped = flag
        };
        oggStream.RealName = name;
        oggStream.Prepare(asyncPrecache);
        if (name.Contains("Gomez"))
          oggStream.LowPass = false;
      }
      catch (Exception ex)
      {
        Logger.Log("SoundManager", LogSeverity.Error, ex.Message);
      }
      return oggStream;
    }

    public void UpdateSongActiveTracks()
    {
      if (this.ActiveSong == null)
        return;
      this.ActiveSong.SetMutedLoops(this.LevelManager.MutedLoops, 6f);
    }

    public void UpdateSongActiveTracks(float fadeDuration)
    {
      if (this.ActiveSong == null)
        return;
      this.ActiveSong.SetMutedLoops(this.LevelManager.MutedLoops, fadeDuration);
    }

    public void PlayNewSong()
    {
      if (SoundManager.NoMusic)
        return;
      TrackedSong currentlyPlayingSong = this.CurrentlyPlayingSong;
      if (this.ActiveSong != null)
        this.ActiveSong.FadeOutAndRemoveComponent();
      if (this.LevelManager.Song == null)
        this.ActiveSong = (ActiveTrackedSong) null;
      else
        ServiceHelper.AddComponent((IGameComponent) (this.ActiveSong = new ActiveTrackedSong(this.Game)));
      if (currentlyPlayingSong == this.CurrentlyPlayingSong)
        return;
      this.SongChanged();
    }

    public void PlayNewSong(float fadeDuration)
    {
      if (SoundManager.NoMusic)
        return;
      TrackedSong currentlyPlayingSong = this.CurrentlyPlayingSong;
      if (this.ActiveSong != null)
        this.ActiveSong.FadeOutAndRemoveComponent(fadeDuration);
      if (this.LevelManager.Song == null)
        this.ActiveSong = (ActiveTrackedSong) null;
      else
        ServiceHelper.AddComponent((IGameComponent) (this.ActiveSong = new ActiveTrackedSong(this.Game)));
      if (currentlyPlayingSong == this.CurrentlyPlayingSong)
        return;
      this.SongChanged();
    }

    public void PlayNewSong(string name)
    {
      if (SoundManager.NoMusic)
        return;
      this.PlayNewSong(name, true);
    }

    public void PlayNewSong(string name, bool interrupt)
    {
      if (SoundManager.NoMusic)
        return;
      TrackedSong currentlyPlayingSong = this.CurrentlyPlayingSong;
      if (!interrupt)
        this.ShelvedSong = this.ActiveSong;
      else if (this.ActiveSong != null)
        this.ActiveSong.FadeOutAndRemoveComponent();
      if (string.IsNullOrEmpty(name))
      {
        this.ActiveSong = (ActiveTrackedSong) null;
      }
      else
      {
        TrackedSong song = this.CMProvider.CurrentLevel.Load<TrackedSong>("Music/" + name);
        song.Initialize();
        ServiceHelper.AddComponent((IGameComponent) (this.ActiveSong = new ActiveTrackedSong(this.Game, song, this.LevelManager.MutedLoops)));
      }
      if (currentlyPlayingSong == this.CurrentlyPlayingSong)
        return;
      this.SongChanged();
    }

    public void PlayNewSong(string name, float fadeDuration)
    {
      if (SoundManager.NoMusic)
        return;
      this.PlayNewSong(name, fadeDuration, true);
    }

    public void PlayNewSong(string name, float fadeDuration, bool interrupt)
    {
      if (SoundManager.NoMusic)
        return;
      TrackedSong currentlyPlayingSong = this.CurrentlyPlayingSong;
      if (!interrupt)
        this.ShelvedSong = this.ActiveSong;
      else if (this.ActiveSong != null)
        this.ActiveSong.FadeOutAndRemoveComponent(fadeDuration);
      if (string.IsNullOrEmpty(name))
      {
        this.ActiveSong = (ActiveTrackedSong) null;
      }
      else
      {
        TrackedSong song = this.CMProvider.CurrentLevel.Load<TrackedSong>("Music/" + name);
        song.Initialize();
        ServiceHelper.AddComponent((IGameComponent) (this.ActiveSong = new ActiveTrackedSong(this.Game, song, this.LevelManager.MutedLoops)));
      }
      if (currentlyPlayingSong == this.CurrentlyPlayingSong)
        return;
      this.SongChanged();
    }

    public void PlayNewAmbience()
    {
      if (this.ActiveAmbience == null)
        ServiceHelper.AddComponent((IGameComponent) (this.ActiveAmbience = new ActiveAmbience(this.Game, (IEnumerable<AmbienceTrack>) this.LevelManager.AmbienceTracks)));
      else
        this.ActiveAmbience.ChangeTracks((IEnumerable<AmbienceTrack>) this.LevelManager.AmbienceTracks);
    }

    public void UnmuteAmbienceTracks()
    {
      if (this.ActiveAmbience == null)
        return;
      this.ActiveAmbience.UnmuteTracks(false);
    }

    public void UnmuteAmbienceTracks(bool apply)
    {
      if (this.ActiveAmbience == null)
        return;
      this.ActiveAmbience.UnmuteTracks(apply);
    }

    public void MuteAmbienceTracks()
    {
      if (this.ActiveAmbience == null)
        return;
      this.ActiveAmbience.MuteTracks();
    }

    public void MuteAmbience(string trackName, float fadeDuration)
    {
      if (this.ActiveAmbience != null)
        this.ActiveAmbience.MuteTrack(trackName, fadeDuration);
      else
        Waiters.Wait((Func<bool>) (() => this.ActiveAmbience != null), (Action) (() => this.ActiveAmbience.MuteTrack(trackName, fadeDuration)));
    }

    public void UnmuteAmbience(string trackName, float fadeDuration)
    {
      if (this.ActiveAmbience != null)
        this.ActiveAmbience.UnmuteTrack(trackName, fadeDuration);
      else
        Waiters.Wait((Func<bool>) (() => this.ActiveAmbience != null), (Action) (() => this.ActiveAmbience.UnmuteTrack(trackName, fadeDuration)));
    }

    public SoundEmitter AddEmitter(SoundEmitter emitter)
    {
      if (!SoundManager.NoMoreSounds)
        this.Emitters.Add(emitter);
      return emitter;
    }

    public void UnshelfSong()
    {
      if (this.ShelvedSong == null)
        return;
      this.ActiveSong = this.ShelvedSong;
      this.ShelvedSong = (ActiveTrackedSong) null;
    }

    public void Stop()
    {
      if (this.ActiveSong != null)
        ServiceHelper.RemoveComponent<ActiveTrackedSong>(this.ActiveSong);
      if (this.ActiveAmbience != null)
        ServiceHelper.RemoveComponent<ActiveAmbience>(this.ActiveAmbience);
      this.ActiveSong = (ActiveTrackedSong) null;
      this.ActiveAmbience = (ActiveAmbience) null;
      this.MusicVolumeFactor = 1f;
    }

    public void ReloadVolumeLevels()
    {
      string cleanPath = SharedContentManager.GetCleanPath(this.CMProvider.Global.RootDirectory + "\\Sounds" + "\\SoundLevels.sdl");
      FileStream fileStream;
      try
      {
        fileStream = new FileStream(cleanPath, FileMode.Open, FileAccess.Read);
      }
      catch (Exception ex)
      {
        if (ex is FileNotFoundException || ex is DirectoryNotFoundException)
        {
          Logger.Log("Sound Levels", LogSeverity.Warning, "Could not find levels file, ignoring...");
          fileStream = (FileStream) null;
        }
        else
        {
          Logger.Log("Sound Levels", LogSeverity.Warning, ex.Message);
          return;
        }
      }
      if (fileStream == null)
        this.VolumeLevels = new VolumeLevels();
      else
        this.VolumeLevels = SdlSerializer.Deserialize<VolumeLevels>(new StreamReader((Stream) fileStream));
    }

    public float GetVolumeLevelFor(string name)
    {
      if (this.VolumeLevels == null)
        return 1f;
      lock (this.VolumeLevels)
      {
        if (name.Contains("Gomez") && this.LevelManager.Name == "PYRAMID")
          return 0.0f;
        VolumeLevel local_0;
        if (this.VolumeLevels.Sounds.TryGetValue(name, out local_0))
          return local_0.Level;
        else
          return 1f;
      }
    }
  }
}
