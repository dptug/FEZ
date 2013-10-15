// Type: FezEngine.Services.ISoundManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace FezEngine.Services
{
  public interface ISoundManager
  {
    bool IsLowPass { get; }

    List<SoundEmitter> Emitters { get; }

    Vector2 LimitDistance { get; }

    float MusicVolume { get; set; }

    float SoundEffectVolume { get; set; }

    bool ScriptChangedSong { get; set; }

    TimeSpan PlayPosition { get; }

    TrackedSong CurrentlyPlayingSong { get; }

    float MusicVolumeFactor { get; set; }

    float GlobalVolumeFactor { get; set; }

    event Action SongChanged;

    SoundEmitter AddEmitter(SoundEmitter emitter);

    void FadeFrequencies(bool lowPass);

    void FadeFrequencies(bool interior, float forSeconds);

    void FadeVolume(float fromVolume, float toVolume, float overSeconds);

    void PlayNewSong();

    void PlayNewSong(string name);

    void PlayNewSong(string name, bool interrupt);

    void PlayNewSong(float fadeDuration);

    void PlayNewSong(string name, float fadeDuration);

    void PlayNewSong(string name, float fadeDuration, bool interrupt);

    void PlayNewAmbience();

    void Pause();

    void Resume();

    void KillSounds();

    void KillSounds(float fadeDuration);

    OggStream GetCue(string name, bool asyncPrecache = false);

    void UpdateSongActiveTracks();

    void UpdateSongActiveTracks(float fadeDuration);

    void MuteAmbience(string trackName, float fadeDuration);

    void UnmuteAmbience(string trackName, float fadeDuration);

    void MuteAmbienceTracks();

    void UnmuteAmbienceTracks();

    void UnmuteAmbienceTracks(bool apply);

    void FactorizeVolume();

    void Stop();

    void UnshelfSong();

    void InitializeLibrary();

    void ReloadVolumeLevels();

    float GetVolumeLevelFor(string name);
  }
}
