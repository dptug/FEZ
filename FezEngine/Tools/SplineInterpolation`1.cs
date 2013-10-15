// Type: FezEngine.Tools.SplineInterpolation`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Tools
{
  public abstract class SplineInterpolation<T>
  {
    public static EasingType EaseInType = EasingType.None;
    public static EasingType EaseOutType = EasingType.Quadratic;
    private static readonly GameTime EmptyGameTime = new GameTime();
    public static bool LongScreenshot;
    private TimeSpan totalElapsed;
    private TimeSpan duration;

    public T[] Points { get; private set; }

    public T Current { get; protected set; }

    public bool Reached { get; private set; }

    public bool Paused { get; private set; }

    public float TotalStep { get; private set; }

    static SplineInterpolation()
    {
    }

    protected SplineInterpolation(TimeSpan duration, params T[] points)
    {
      this.Paused = true;
      this.duration = duration;
      this.Points = points;
    }

    public void Start()
    {
      this.Paused = false;
      this.Update(SplineInterpolation<T>.EmptyGameTime);
    }

    public void Update(GameTime gameTime)
    {
      if (this.Reached || this.Paused)
        return;
      this.totalElapsed += gameTime.ElapsedGameTime;
      int index1 = this.Points.Length - 1;
      if (this.totalElapsed >= this.duration)
      {
        this.TotalStep = 1f;
        this.Current = this.Points[index1];
        this.totalElapsed = this.duration;
        this.Reached = true;
        this.Paused = true;
      }
      else
      {
        float num1 = SplineInterpolation<T>.EaseInType != EasingType.None ? (SplineInterpolation<T>.EaseOutType != EasingType.None ? (SplineInterpolation<T>.EaseInType != SplineInterpolation<T>.EaseOutType ? Easing.EaseInOut((double) this.totalElapsed.Ticks / (double) this.duration.Ticks, SplineInterpolation<T>.EaseInType, SplineInterpolation<T>.EaseOutType) : Easing.EaseInOut((double) this.totalElapsed.Ticks / (double) this.duration.Ticks, SplineInterpolation<T>.EaseInType)) : Easing.EaseIn((double) this.totalElapsed.Ticks / (double) this.duration.Ticks, SplineInterpolation<T>.EaseInType)) : Easing.EaseOut((double) this.totalElapsed.Ticks / (double) this.duration.Ticks, SplineInterpolation<T>.EaseOutType);
        int index2 = (int) MathHelper.Clamp((float) ((double) index1 * (double) num1 - 1.0), 0.0f, (float) index1);
        int index3 = (int) MathHelper.Clamp((float) index1 * num1, 0.0f, (float) index1);
        int index4 = (int) MathHelper.Clamp((float) ((double) index1 * (double) num1 + 1.0), 0.0f, (float) index1);
        int index5 = (int) MathHelper.Clamp((float) ((double) index1 * (double) num1 + 2.0), 0.0f, (float) index1);
        double num2 = (double) index3 / (double) index1;
        double num3 = (double) index4 / (double) index1 - num2;
        float t = (float) FezMath.Saturate(((double) num1 - num2) / (num3 == 0.0 ? 1.0 : num3));
        this.TotalStep = num1;
        this.Interpolate(this.Points[index2], this.Points[index3], this.Points[index4], this.Points[index5], t);
      }
    }

    protected abstract void Interpolate(T p0, T p1, T p2, T p3, float t);
  }
}
