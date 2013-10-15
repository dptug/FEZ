// Type: FezGame.Services.Scripting.TimeService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Services.Scripting
{
  public class TimeService : ITimeService, IScriptingBase
  {
    public int Hour
    {
      get
      {
        return this.TimeManager.CurrentTime.Hour;
      }
    }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    public LongRunningAction SetHour(int hour, bool immediate)
    {
      DateTime dateTime = new DateTime(1985, 12, 23, hour, 0, 0);
      if (immediate)
      {
        this.TimeManager.CurrentTime = dateTime;
        return (LongRunningAction) null;
      }
      else
      {
        long ticks = this.TimeManager.CurrentTime.Ticks;
        long destinationTicks = dateTime.Ticks;
        while (ticks - destinationTicks > 432000000000L)
          destinationTicks += 864000000000L;
        while (destinationTicks - ticks > 432000000000L)
          ticks += 864000000000L;
        int direction = Math.Sign(destinationTicks - ticks);
        destinationTicks -= (long) direction * 36000000000L / 2L;
        return new LongRunningAction((Func<float, float, bool>) ((elapsedSeconds, totalSeconds) =>
        {
          bool local_0 = direction != Math.Sign(destinationTicks - this.TimeManager.CurrentTime.Ticks);
          if (local_0)
            this.TimeManager.TimeFactor = MathHelper.Lerp(this.TimeManager.TimeFactor, this.TimeManager.DefaultTimeFactor, elapsedSeconds);
          else if ((double) totalSeconds < 1.0)
            this.TimeManager.TimeFactor = (float) ((double) this.TimeManager.DefaultTimeFactor * (double) Easing.EaseIn((double) FezMath.Saturate(totalSeconds), EasingType.Quadratic) * 100.0) * (float) direction;
          if (local_0)
            return FezMath.AlmostEqual(this.TimeManager.TimeFactor, 360f);
          else
            return false;
        }), (Action) (() => this.TimeManager.TimeFactor = this.TimeManager.DefaultTimeFactor));
      }
    }

    public void SetTimeFactor(int factor)
    {
      this.TimeManager.TimeFactor = (float) factor;
    }

    public LongRunningAction IncrementTimeFactor(float secondsUntilDouble)
    {
      return new LongRunningAction((Func<float, float, bool>) ((elapsedSeconds, _) =>
      {
        this.TimeManager.TimeFactor = FezMath.DoubleIter(this.TimeManager.TimeFactor, elapsedSeconds, secondsUntilDouble);
        return false;
      }));
    }

    public void ResetEvents()
    {
    }
  }
}
