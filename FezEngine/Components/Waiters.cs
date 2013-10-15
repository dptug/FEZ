// Type: FezEngine.Components.Waiters
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Components
{
  public static class Waiters
  {
    public static IWaiter DoUntil(Func<bool> endCondition, Action<float> action)
    {
      return Waiters.DoUntil(endCondition, action, new Action(Util.NullAction));
    }

    public static IWaiter DoUntil(Func<bool> endCondition, Action<float> action, Action onComplete)
    {
      Waiter waiter;
      ServiceHelper.AddComponent((IGameComponent) (waiter = new Waiter(endCondition, (Action<TimeSpan>) (elapsed => action((float) elapsed.TotalSeconds)), onComplete)));
      return (IWaiter) waiter;
    }

    public static IWaiter Wait(Func<bool> endCondition, Action onValid)
    {
      Waiter waiter;
      ServiceHelper.AddComponent((IGameComponent) (waiter = new Waiter(endCondition, onValid)));
      return (IWaiter) waiter;
    }

    public static IWaiter Wait(double secondsToWait, Action onValid)
    {
      Func<Waiters.TimeKeeper, bool> condition = (Func<Waiters.TimeKeeper, bool>) (waited => waited.Elapsed.TotalSeconds > secondsToWait);
      Action<TimeSpan, Waiters.TimeKeeper> whileWaiting = (Action<TimeSpan, Waiters.TimeKeeper>) ((elapsed, waited) => waited.Elapsed += elapsed);
      Action onValid1 = onValid;
      Waiter<Waiters.TimeKeeper> waiter;
      ServiceHelper.AddComponent((IGameComponent) (waiter = new Waiter<Waiters.TimeKeeper>(condition, whileWaiting, onValid1)));
      return (IWaiter) waiter;
    }

    public static IWaiter Wait(double secondsToWait, Func<float, bool> earlyOutCondition, Action onValid)
    {
      Func<Waiters.TimeKeeper, bool> condition = (Func<Waiters.TimeKeeper, bool>) (waited =>
      {
        if (!earlyOutCondition((float) waited.Elapsed.TotalSeconds))
          return waited.Elapsed.TotalSeconds > secondsToWait;
        else
          return true;
      });
      Action<TimeSpan, Waiters.TimeKeeper> whileWaiting = (Action<TimeSpan, Waiters.TimeKeeper>) ((elapsed, waited) => waited.Elapsed += elapsed);
      Action onValid1 = onValid;
      Waiter<Waiters.TimeKeeper> waiter;
      ServiceHelper.AddComponent((IGameComponent) (waiter = new Waiter<Waiters.TimeKeeper>(condition, whileWaiting, onValid1)));
      return (IWaiter) waiter;
    }

    public static IWaiter Interpolate(double durationSeconds, Action<float> assignation)
    {
      return Waiters.Interpolate(durationSeconds, assignation, new Action(Util.NullAction));
    }

    public static IWaiter Interpolate(double durationSeconds, Action<float> assignation, Action onComplete)
    {
      if (durationSeconds == 0.0)
      {
        onComplete();
        return (IWaiter) null;
      }
      else
      {
        Waiter<Waiters.TimeKeeper> waiter;
        ServiceHelper.AddComponent((IGameComponent) (waiter = new Waiter<Waiters.TimeKeeper>((Func<Waiters.TimeKeeper, bool>) (waited => waited.Elapsed.TotalSeconds > durationSeconds), (Action<TimeSpan, Waiters.TimeKeeper>) ((elapsed, waited) =>
        {
          waited.Elapsed += elapsed;
          assignation((float) (waited.Elapsed.TotalSeconds / durationSeconds));
        }), onComplete)));
        return (IWaiter) waiter;
      }
    }

    private class TimeKeeper
    {
      public TimeSpan Elapsed;
    }
  }
}
