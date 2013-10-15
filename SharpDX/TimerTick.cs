// Type: SharpDX.TimerTick
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Diagnostics;

namespace SharpDX
{
  public class TimerTick
  {
    private long startRawTime;
    private long lastRawTime;
    private int pauseCount;
    private long pauseStartTime;
    private long timePaused;

    public TimeSpan TotalTime { get; private set; }

    public TimeSpan ElapsedAdjustedTime { get; private set; }

    public TimeSpan ElapsedTime { get; private set; }

    public bool IsPaused
    {
      get
      {
        return this.pauseCount > 0;
      }
    }

    public TimerTick()
    {
      this.Reset();
    }

    public void Reset()
    {
      this.TotalTime = TimeSpan.Zero;
      this.startRawTime = Stopwatch.GetTimestamp();
      this.lastRawTime = this.startRawTime;
    }

    public void Resume()
    {
      --this.pauseCount;
      if (this.pauseCount > 0)
        return;
      this.timePaused += Stopwatch.GetTimestamp() - this.pauseStartTime;
      this.pauseStartTime = 0L;
    }

    public void Tick()
    {
      if (this.IsPaused)
        return;
      long timestamp = Stopwatch.GetTimestamp();
      this.TotalTime = TimerTick.ConvertRawToTimestamp(timestamp - this.startRawTime);
      this.ElapsedTime = TimerTick.ConvertRawToTimestamp(timestamp - this.lastRawTime);
      this.ElapsedAdjustedTime = TimerTick.ConvertRawToTimestamp(timestamp - (this.lastRawTime + this.timePaused));
      if (this.ElapsedAdjustedTime < TimeSpan.Zero)
        this.ElapsedAdjustedTime = TimeSpan.Zero;
      this.timePaused = 0L;
      this.lastRawTime = timestamp;
    }

    public void Pause()
    {
      ++this.pauseCount;
      if (this.pauseCount != 1)
        return;
      this.pauseStartTime = Stopwatch.GetTimestamp();
    }

    private static TimeSpan ConvertRawToTimestamp(long delta)
    {
      return TimeSpan.FromTicks(delta * 10000000L / Stopwatch.Frequency);
    }
  }
}
