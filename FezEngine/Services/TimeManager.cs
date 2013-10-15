// Type: FezEngine.Services.TimeManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public class TimeManager : ITimeManager
  {
    public static DateTime InitialTime = DateTime.Today.AddHours(12.0);
    private const float TransitionDivider = 3f;

    public DateTime CurrentTime { get; set; }

    public float DefaultTimeFactor
    {
      get
      {
        return 260f;
      }
    }

    public float DayFraction
    {
      get
      {
        return (float) this.CurrentTime.TimeOfDay.TotalDays;
      }
    }

    public float TimeFactor { get; set; }

    public float NightContribution { get; private set; }

    public float DawnContribution { get; private set; }

    public float DuskContribution { get; private set; }

    public float CurrentAmbientFactor { get; set; }

    public Color CurrentFogColor { get; set; }

    public event Action Tick = new Action(Util.NullAction);

    static TimeManager()
    {
    }

    public TimeManager()
    {
      this.TimeFactor = this.DefaultTimeFactor;
      this.CurrentTime = TimeManager.InitialTime;
      this.Tick += (Action) (() =>
      {
        this.DawnContribution = TimeManager.Ease(this.DayFraction, DayPhaseExtensions.StartTime(DayPhase.Dawn), DayPhaseExtensions.Duration(DayPhase.Dawn));
        this.DuskContribution = TimeManager.Ease(this.DayFraction, DayPhaseExtensions.StartTime(DayPhase.Dusk), DayPhaseExtensions.Duration(DayPhase.Dusk));
        this.NightContribution = TimeManager.Ease(this.DayFraction, DayPhaseExtensions.StartTime(DayPhase.Night), DayPhaseExtensions.Duration(DayPhase.Night));
        this.NightContribution = Math.Max(this.NightContribution, TimeManager.Ease(this.DayFraction, DayPhaseExtensions.StartTime(DayPhase.Night) - 1f, DayPhaseExtensions.Duration(DayPhase.Night)));
      });
    }

    private static float Ease(float value, float start, float duration)
    {
      float num1 = value - start;
      float num2 = duration / 3f;
      if ((double) num1 < (double) num2)
        return FezMath.Saturate(num1 / num2);
      if ((double) num1 > 2.0 * (double) num2)
        return 1f - FezMath.Saturate((num1 - 2f * num2) / num2);
      return (double) num1 < 0.0 || (double) num1 > (double) duration ? 0.0f : 1f;
    }

    public void OnTick()
    {
      this.Tick();
    }

    public bool IsDayPhase(DayPhase phase)
    {
      float dayFraction = this.DayFraction;
      float num1 = DayPhaseExtensions.StartTime(phase);
      float num2 = DayPhaseExtensions.EndTime(phase);
      if ((double) num1 < (double) num2)
      {
        if ((double) dayFraction >= (double) num1)
          return (double) dayFraction <= (double) num2;
        else
          return false;
      }
      else if ((double) dayFraction < (double) num1)
        return (double) dayFraction <= (double) num2;
      else
        return true;
    }

    public bool IsDayPhaseForMusic(DayPhase phase)
    {
      float dayFraction = this.DayFraction;
      float num1 = DayPhaseExtensions.MusicStartTime(phase);
      float num2 = DayPhaseExtensions.MusicEndTime(phase);
      if ((double) num1 < (double) num2)
      {
        if ((double) dayFraction >= (double) num1)
          return (double) dayFraction <= (double) num2;
        else
          return false;
      }
      else if ((double) dayFraction < (double) num1)
        return (double) dayFraction <= (double) num2;
      else
        return true;
    }

    public float DayPhaseFraction(DayPhase phase)
    {
      float num = this.DayFraction - DayPhaseExtensions.StartTime(phase);
      if ((double) num < 1.0)
        ++num;
      return num / DayPhaseExtensions.Duration(phase);
    }
  }
}
