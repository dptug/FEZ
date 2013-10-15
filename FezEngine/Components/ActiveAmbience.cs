// Type: FezEngine.Components.ActiveAmbience
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Components
{
  internal class ActiveAmbience : GameComponent
  {
    private readonly List<ActiveAmbienceTrack> ActiveTracks = new List<ActiveAmbienceTrack>();
    private List<AmbienceTrack> Tracks;
    private bool cancelPause;
    private bool resumeRequested;

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    public ActiveAmbience(Game game, IEnumerable<AmbienceTrack> tracks)
      : base(game)
    {
      this.Tracks = new List<AmbienceTrack>(tracks);
    }

    public override void Initialize()
    {
      this.Enabled = false;
      foreach (AmbienceTrack track in this.Tracks)
      {
        bool activeForDayPhase = ((((((!this.TimeManager.IsDayPhaseForMusic(DayPhase.Day) ? (false ? 1 : 0) : (track.Day ? 1 : 0)) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Night) ? 0 : (track.Night ? 1 : 0))) != 0 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Dawn) ? 0 : (track.Dawn ? 1 : 0))) != 0 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Dusk) ? 0 : (track.Dusk ? 1 : 0))) != 0;
        this.ActiveTracks.Add(new ActiveAmbienceTrack(track, activeForDayPhase));
      }
      this.Tracks.Clear();
      this.Tracks = (List<AmbienceTrack>) null;
      this.Enabled = true;
    }

    public override void Update(GameTime gameTime)
    {
      bool flag1 = this.TimeManager.IsDayPhaseForMusic(DayPhase.Day);
      bool flag2 = this.TimeManager.IsDayPhaseForMusic(DayPhase.Dawn);
      bool flag3 = this.TimeManager.IsDayPhaseForMusic(DayPhase.Dusk);
      bool flag4 = this.TimeManager.IsDayPhaseForMusic(DayPhase.Night);
      foreach (ActiveAmbienceTrack activeAmbienceTrack1 in this.ActiveTracks)
      {
        bool activeForDayPhase = activeAmbienceTrack1.ActiveForDayPhase;
        activeAmbienceTrack1.ActiveForDayPhase = false;
        ActiveAmbienceTrack activeAmbienceTrack2 = activeAmbienceTrack1;
        int num1 = (activeAmbienceTrack2.ActiveForDayPhase ? 1 : 0) | (!flag1 ? 0 : (activeAmbienceTrack1.Track.Day ? 1 : 0));
        activeAmbienceTrack2.ActiveForDayPhase = num1 != 0;
        ActiveAmbienceTrack activeAmbienceTrack3 = activeAmbienceTrack1;
        int num2 = (activeAmbienceTrack3.ActiveForDayPhase ? 1 : 0) | (!flag4 ? 0 : (activeAmbienceTrack1.Track.Night ? 1 : 0));
        activeAmbienceTrack3.ActiveForDayPhase = num2 != 0;
        ActiveAmbienceTrack activeAmbienceTrack4 = activeAmbienceTrack1;
        int num3 = (activeAmbienceTrack4.ActiveForDayPhase ? 1 : 0) | (!flag2 ? 0 : (activeAmbienceTrack1.Track.Dawn ? 1 : 0));
        activeAmbienceTrack4.ActiveForDayPhase = num3 != 0;
        ActiveAmbienceTrack activeAmbienceTrack5 = activeAmbienceTrack1;
        int num4 = (activeAmbienceTrack5.ActiveForDayPhase ? 1 : 0) | (!flag3 ? 0 : (activeAmbienceTrack1.Track.Dusk ? 1 : 0));
        activeAmbienceTrack5.ActiveForDayPhase = num4 != 0;
        if (activeForDayPhase != activeAmbienceTrack1.ActiveForDayPhase && !activeAmbienceTrack1.ForceMuted)
          activeAmbienceTrack1.OnMuteStateChanged(16f);
      }
    }

    public void Pause()
    {
      if (!this.Enabled)
        return;
      Waiters.Interpolate(0.25, (Action<float>) (step =>
      {
        ActiveAmbience temp_10 = this;
        int temp_15 = (temp_10.cancelPause ? 1 : 0) | (this.resumeRequested ? 1 : (!this.Enabled ? 1 : 0));
        temp_10.cancelPause = temp_15 != 0;
      }), (Action) (() =>
      {
        if (!this.cancelPause && !this.resumeRequested)
        {
          foreach (ActiveAmbienceTrack item_0 in this.ActiveTracks)
            item_0.Pause();
          this.Enabled = false;
        }
        this.cancelPause = this.resumeRequested = false;
      }));
    }

    public void Resume()
    {
      if (this.Enabled)
        return;
      foreach (ActiveAmbienceTrack activeAmbienceTrack in this.ActiveTracks)
        activeAmbienceTrack.Resume();
      this.Enabled = true;
      this.resumeRequested = true;
      Waiters.Interpolate(0.125, (Action<float>) (step =>
      {
        if (!this.Enabled)
          return;
        this.resumeRequested = false;
      }));
    }

    public void ChangeTracks(IEnumerable<AmbienceTrack> tracks)
    {
      foreach (ActiveAmbienceTrack activeAmbienceTrack in this.ActiveTracks)
      {
        foreach (AmbienceTrack ambienceTrack in tracks)
        {
          if (ambienceTrack.Name == activeAmbienceTrack.Track.Name)
          {
            activeAmbienceTrack.Track = ambienceTrack;
            bool flag1 = activeAmbienceTrack.ActiveForDayPhase && (!activeAmbienceTrack.ForceMuted && !activeAmbienceTrack.WasMuted);
            activeAmbienceTrack.WasMuted = false;
            bool flag2 = ((((((!this.TimeManager.IsDayPhaseForMusic(DayPhase.Day) ? (false ? 1 : 0) : (ambienceTrack.Day ? 1 : 0)) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Night) ? 0 : (ambienceTrack.Night ? 1 : 0))) != 0 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Dawn) ? 0 : (ambienceTrack.Dawn ? 1 : 0))) != 0 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Dusk) ? 0 : (ambienceTrack.Dusk ? 1 : 0))) != 0;
            activeAmbienceTrack.ActiveForDayPhase = flag2;
            if (flag1 != activeAmbienceTrack.ActiveForDayPhase)
            {
              if (!activeAmbienceTrack.ForceMuted)
              {
                activeAmbienceTrack.OnMuteStateChanged(2f);
                break;
              }
              else
                break;
            }
            else
              break;
          }
        }
      }
      foreach (ActiveAmbienceTrack activeAmbienceTrack in Enumerable.Where<ActiveAmbienceTrack>((IEnumerable<ActiveAmbienceTrack>) this.ActiveTracks, (Func<ActiveAmbienceTrack, bool>) (x => !Enumerable.Any<AmbienceTrack>(tracks, (Func<AmbienceTrack, bool>) (y => y.Name == x.Track.Name)))))
      {
        activeAmbienceTrack.ForceMuted = true;
        activeAmbienceTrack.OnMuteStateChanged(2f);
        ActiveAmbienceTrack t1 = activeAmbienceTrack;
        Waiters.Wait(2.0, (Action) (() =>
        {
          t1.Dispose();
          this.ActiveTracks.Remove(t1);
        }));
      }
      foreach (AmbienceTrack track in Enumerable.Where<AmbienceTrack>(tracks, (Func<AmbienceTrack, bool>) (x => !Enumerable.Any<ActiveAmbienceTrack>((IEnumerable<ActiveAmbienceTrack>) this.ActiveTracks, (Func<ActiveAmbienceTrack, bool>) (y => y.Track.Name == x.Name)))))
      {
        bool activeForDayPhase = ((((((!this.TimeManager.IsDayPhaseForMusic(DayPhase.Day) ? (false ? 1 : 0) : (track.Day ? 1 : 0)) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Night) ? 0 : (track.Night ? 1 : 0))) != 0 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Dawn) ? 0 : (track.Dawn ? 1 : 0))) != 0 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Dusk) ? 0 : (track.Dusk ? 1 : 0))) != 0;
        this.ActiveTracks.Add(new ActiveAmbienceTrack(track, activeForDayPhase));
      }
    }

    public void MuteTrack(string name, float fadeDuration)
    {
      foreach (ActiveAmbienceTrack activeAmbienceTrack in this.ActiveTracks)
      {
        if (activeAmbienceTrack.Track.Name == name)
        {
          activeAmbienceTrack.ForceMuted = true;
          activeAmbienceTrack.OnMuteStateChanged(fadeDuration);
          break;
        }
      }
    }

    public void UnmuteTrack(string name, float fadeDuration)
    {
      foreach (ActiveAmbienceTrack activeAmbienceTrack in this.ActiveTracks)
      {
        if (activeAmbienceTrack.Track.Name == name)
        {
          activeAmbienceTrack.ForceMuted = false;
          activeAmbienceTrack.OnMuteStateChanged(fadeDuration);
          break;
        }
      }
    }

    public void UnmuteTracks(bool apply)
    {
      foreach (ActiveAmbienceTrack activeAmbienceTrack in this.ActiveTracks)
      {
        if (activeAmbienceTrack.ForceMuted)
          activeAmbienceTrack.WasMuted = true;
        activeAmbienceTrack.ForceMuted = false;
        if (apply)
          activeAmbienceTrack.OnMuteStateChanged();
      }
    }

    public void MuteTracks()
    {
      foreach (ActiveAmbienceTrack activeAmbienceTrack in this.ActiveTracks)
      {
        activeAmbienceTrack.ForceMuted = true;
        activeAmbienceTrack.OnMuteStateChanged();
      }
    }

    protected override void Dispose(bool disposing)
    {
      foreach (ActiveAmbienceTrack activeAmbienceTrack in this.ActiveTracks)
        activeAmbienceTrack.Dispose();
      this.ActiveTracks.Clear();
      this.Enabled = false;
    }
  }
}
