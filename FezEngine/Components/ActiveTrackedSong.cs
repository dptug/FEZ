// Type: FezEngine.Components.ActiveTrackedSong
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FezEngine.Components
{
  public class ActiveTrackedSong : GameComponent
  {
    private readonly Stopwatch Watch = new Stopwatch();
    private readonly List<ActiveLoop> ActiveLoops = new List<ActiveLoop>();
    private int OoaTIndex = -1;
    public const float DefaultFadeDuration = 2f;
    private IList<string> mutedLoops;
    public TrackedSong Song;
    private long BeatsCounted;
    private long BarsCounted;
    private double LastTotalMinutes;
    private ActiveLoop[] AllOaaTs;
    private ActiveLoop CurrentOaaT;
    private ActiveLoop NextOaaT;
    private bool cancelPause;
    private bool resumeRequested;

    public bool IgnoreDayPhase { get; set; }

    public int CurrentBeat
    {
      get
      {
        return (int) (this.BeatsCounted % (long) this.Song.TimeSignature);
      }
    }

    public int CurrentBar
    {
      get
      {
        return (int) this.BarsCounted;
      }
    }

    public TimeSpan PlayPosition
    {
      get
      {
        return this.Watch.Elapsed;
      }
    }

    public IList<string> MutedLoops
    {
      get
      {
        return this.mutedLoops;
      }
      set
      {
        this.mutedLoops = value;
        foreach (ActiveLoop activeLoop in this.ActiveLoops)
        {
          if (!activeLoop.Muted && this.mutedLoops.Contains(activeLoop.Loop.Name))
          {
            activeLoop.Muted = true;
            activeLoop.OnMuteStateChanged();
          }
        }
        foreach (ActiveLoop activeLoop in this.ActiveLoops)
        {
          if (activeLoop.Muted && !this.mutedLoops.Contains(activeLoop.Loop.Name))
          {
            activeLoop.Muted = false;
            activeLoop.OnMuteStateChanged();
          }
        }
      }
    }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    public event Action Beat = new Action(Util.NullAction);

    public event Action Bar = new Action(Util.NullAction);

    public ActiveTrackedSong(Game game)
      : base(game)
    {
    }

    public ActiveTrackedSong(Game game, TrackedSong song, IList<string> mutedLoops)
      : this(game)
    {
      this.Song = song;
      this.MutedLoops = mutedLoops;
    }

    public override void Initialize()
    {
      if (this.Song == null)
        this.Song = this.LevelManager.Song;
      if (this.MutedLoops == null)
        this.MutedLoops = this.LevelManager.MutedLoops;
      if (this.Song == null)
      {
        this.Enabled = false;
        ServiceHelper.RemoveComponent<ActiveTrackedSong>(this);
      }
      else
      {
        this.BarsCounted = this.BeatsCounted = 0L;
        this.Enabled = false;
        Waiters.Wait(0.1, (Action) (() =>
        {
          foreach (Loop item_0 in this.Song.Loops)
          {
            bool local_1 = this.IgnoreDayPhase;
            if (!this.IgnoreDayPhase)
              local_1 = ((((((((local_1 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Day) ? 0 : (item_0.Day ? 1 : 0))) != 0 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Night) ? 0 : (item_0.Night ? 1 : 0))) != 0 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Dawn) ? 0 : (item_0.Dawn ? 1 : 0))) != 0 ? 1 : 0) | (!this.TimeManager.IsDayPhaseForMusic(DayPhase.Dusk) ? 0 : (item_0.Dusk ? 1 : 0))) != 0;
            ActiveLoop local_2 = new ActiveLoop(item_0, this.MutedLoops.Contains(item_0.Name), local_1, item_0.OneAtATime);
            if (item_0.OneAtATime)
              local_2.CycleLink = new Action(this.CycleOaaTs);
            this.ActiveLoops.Add(local_2);
          }
          this.AllOaaTs = Enumerable.ToArray<ActiveLoop>(Enumerable.Where<ActiveLoop>((IEnumerable<ActiveLoop>) this.ActiveLoops, (Func<ActiveLoop, bool>) (x => x.Loop.OneAtATime)));
          if (this.Song.Loops.Count > 0 && this.AllOaaTs.Length > 0)
            this.CycleOaaTs();
          this.Enabled = true;
          this.Watch.Start();
        }));
      }
    }

    private void CycleOaaTs()
    {
      if (this.CurrentOaaT != null && this.CurrentOaaT.Loop.CutOffTail && !this.CurrentOaaT.WaitedForDelay)
        this.CurrentOaaT.CutOff();
      this.CurrentOaaT = this.NextOaaT;
      if (this.CurrentOaaT != null)
        this.CurrentOaaT.ActiveForOoaTs = false;
      int index;
      if (this.Song.RandomOrdering)
      {
        index = RandomHelper.Random.Next(0, this.AllOaaTs.Length);
        if (this.CurrentOaaT != null && index == this.ActiveLoops.IndexOf(this.CurrentOaaT))
          index = RandomHelper.Random.Next(0, this.AllOaaTs.Length);
      }
      else
        index = this.Song.CustomOrdering == null || this.Song.CustomOrdering.Length == 0 ? (this.ActiveLoops.IndexOf(this.CurrentOaaT) + 1) % this.ActiveLoops.Count : this.Song.CustomOrdering[this.OoaTIndex = (this.OoaTIndex + 1) % this.Song.CustomOrdering.Length] - 1;
      this.NextOaaT = this.AllOaaTs[index];
      this.NextOaaT.ActiveForOoaTs = true;
      this.NextOaaT.SchedulePlay();
      if (this.CurrentOaaT != null)
        return;
      this.NextOaaT.ForcePlay();
      if (this.NextOaaT.Loop.Delay != 0)
        return;
      this.CycleOaaTs();
    }

    public override void Update(GameTime gameTime)
    {
      double totalMinutes = this.Watch.Elapsed.TotalMinutes;
      foreach (ActiveLoop activeLoop in this.ActiveLoops)
      {
        if (activeLoop.Loop.FractionalTime)
        {
          float totalBars = (float) (totalMinutes - this.LastTotalMinutes) * (float) this.Song.Tempo / (float) this.Song.TimeSignature;
          activeLoop.UpdateFractional(totalBars);
        }
        activeLoop.UpdatePrecache();
      }
      this.LastTotalMinutes = totalMinutes;
      double num1 = Math.Floor(totalMinutes * (double) this.Song.Tempo);
      if (num1 > (double) this.BeatsCounted)
      {
        this.BeatsCounted = (long) (int) num1;
        this.OnBeat();
        long num2 = this.BeatsCounted / (long) this.Song.TimeSignature;
        if (num2 > this.BarsCounted)
        {
          this.BarsCounted = num2;
          this.OnBar();
        }
      }
      if (this.IgnoreDayPhase)
        return;
      bool flag1 = this.TimeManager.IsDayPhaseForMusic(DayPhase.Day);
      bool flag2 = this.TimeManager.IsDayPhaseForMusic(DayPhase.Dawn);
      bool flag3 = this.TimeManager.IsDayPhaseForMusic(DayPhase.Dusk);
      bool flag4 = this.TimeManager.IsDayPhaseForMusic(DayPhase.Night);
      foreach (ActiveLoop activeLoop1 in this.ActiveLoops)
      {
        bool activeForDayPhase = activeLoop1.ActiveForDayPhase;
        activeLoop1.ActiveForDayPhase = false;
        ActiveLoop activeLoop2 = activeLoop1;
        int num2 = (activeLoop2.ActiveForDayPhase ? 1 : 0) | (!flag1 ? 0 : (activeLoop1.Loop.Day ? 1 : 0));
        activeLoop2.ActiveForDayPhase = num2 != 0;
        ActiveLoop activeLoop3 = activeLoop1;
        int num3 = (activeLoop3.ActiveForDayPhase ? 1 : 0) | (!flag4 ? 0 : (activeLoop1.Loop.Night ? 1 : 0));
        activeLoop3.ActiveForDayPhase = num3 != 0;
        ActiveLoop activeLoop4 = activeLoop1;
        int num4 = (activeLoop4.ActiveForDayPhase ? 1 : 0) | (!flag2 ? 0 : (activeLoop1.Loop.Dawn ? 1 : 0));
        activeLoop4.ActiveForDayPhase = num4 != 0;
        ActiveLoop activeLoop5 = activeLoop1;
        int num5 = (activeLoop5.ActiveForDayPhase ? 1 : 0) | (!flag3 ? 0 : (activeLoop1.Loop.Dusk ? 1 : 0));
        activeLoop5.ActiveForDayPhase = num5 != 0;
        if (activeForDayPhase != activeLoop1.ActiveForDayPhase)
          activeLoop1.OnMuteStateChanged(16f);
      }
    }

    public void Pause()
    {
      if (!this.Enabled)
        return;
      Waiters.Interpolate(0.25, (Action<float>) (step =>
      {
        ActiveTrackedSong temp_10 = this;
        int temp_15 = (temp_10.cancelPause ? 1 : 0) | (this.resumeRequested ? 1 : (!this.Enabled ? 1 : 0));
        temp_10.cancelPause = temp_15 != 0;
        if (this.cancelPause)
          return;
        this.SoundManager.MusicVolumeFactor = FezMath.Saturate(1f - Easing.EaseOut((double) step, EasingType.Sine));
      }), (Action) (() =>
      {
        if (!this.cancelPause && !this.resumeRequested)
        {
          this.Watch.Stop();
          foreach (ActiveLoop item_0 in this.ActiveLoops)
            item_0.Pause();
          this.Enabled = false;
          this.SoundManager.MusicVolumeFactor = 1f;
        }
        this.cancelPause = this.resumeRequested = false;
      }));
    }

    public void Resume()
    {
      if (this.Enabled || this.Watch == null)
        return;
      this.Watch.Start();
      foreach (ActiveLoop activeLoop in this.ActiveLoops)
        activeLoop.Resume();
      this.Enabled = true;
      this.resumeRequested = true;
      Waiters.Interpolate(0.125, (Action<float>) (step =>
      {
        if (!this.Enabled)
          return;
        this.SoundManager.MusicVolumeFactor = FezMath.Saturate(Easing.EaseOut((double) step, EasingType.Sine));
        this.resumeRequested = false;
      }), (Action) (() => this.SoundManager.MusicVolumeFactor = 1f));
    }

    private void OnBeat()
    {
      this.Beat();
    }

    private void OnBar()
    {
      foreach (ActiveLoop activeLoop in this.ActiveLoops)
      {
        if (!activeLoop.Loop.FractionalTime)
          activeLoop.OnBar();
      }
      this.Bar();
    }

    public void SetMutedLoops(IList<string> loops, float fadeDuration)
    {
      this.mutedLoops = loops;
      foreach (ActiveLoop activeLoop in this.ActiveLoops)
      {
        if (!activeLoop.Muted && this.mutedLoops.Contains(activeLoop.Loop.Name))
        {
          activeLoop.Muted = true;
          activeLoop.OnMuteStateChanged(fadeDuration);
        }
      }
      foreach (ActiveLoop activeLoop in this.ActiveLoops)
      {
        if (activeLoop.Muted && !this.mutedLoops.Contains(activeLoop.Loop.Name))
        {
          activeLoop.Muted = false;
          activeLoop.OnMuteStateChanged(fadeDuration);
        }
      }
    }

    public void ReInitialize(IList<string> newMutedLoops)
    {
      this.mutedLoops = newMutedLoops;
      this.Dispose(false);
      this.Initialize();
    }

    protected override void Dispose(bool disposing)
    {
      foreach (ActiveLoop activeLoop in this.ActiveLoops)
        activeLoop.Dispose();
      this.ActiveLoops.Clear();
      this.Enabled = false;
      if (!disposing)
        return;
      this.Beat = (Action) null;
      this.Bar = (Action) null;
    }

    public void FadeOutAndRemoveComponent()
    {
      this.FadeOutAndRemoveComponent(2f);
    }

    public void FadeOutAndRemoveComponent(float fadeDuration)
    {
      if (!this.Enabled)
        return;
      foreach (ActiveLoop activeLoop in this.ActiveLoops)
      {
        activeLoop.Muted = true;
        activeLoop.OnMuteStateChanged(fadeDuration);
      }
      this.Enabled = false;
      Waiters.Wait((double) fadeDuration, (Action) (() => ServiceHelper.RemoveComponent<ActiveTrackedSong>(this)));
    }
  }
}
