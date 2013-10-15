// Type: FezEngine.Services.Scripting.ISoundService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Static = true)]
  public interface ISoundService : IScriptingBase
  {
    [Description("Plays a sound by its filename")]
    void Play(string soundName);

    [Description("Plays a sound by its prefix and an auto-incremented index (starts at 1)")]
    void PlayNext(string soundPrefix);

    [Description("Changes the volume of BGM")]
    void SetMusicVolume(float volume);

    [Description("Resets the indices for a sound")]
    void ResetIndices(string soundPrefix);

    [Description("Changes the music track and plays it")]
    void ChangeMusic(string newMusic);

    [Description("Enables a track by its name")]
    void UnmuteTrack(string trackName, float fadeDuration);

    [Description("Disables a track by its name")]
    void MuteTrack(string trackName, float fadeDuration);

    void UnmuteAmbience(string trackName, float fadeDuration);

    void MuteAmbience(string trackName, float fadeDuration);

    [Description("Fades the music out")]
    void FadeMusicOut(float overSeconds);

    void FadeMusicTo(float to, float overSeconds);

    void ChangePhases(string trackName, bool dawn, bool day, bool dusk, bool night);
  }
}
