// Type: FezEngine.Tools.Easing
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Tools
{
  public static class Easing
  {
    public static float Ease(double linearStep, float acceleration, EasingType type)
    {
      float num = (double) acceleration > 0.0 ? Easing.EaseIn(linearStep, type) : ((double) acceleration < 0.0 ? Easing.EaseOut(linearStep, type) : (float) linearStep);
      return MathHelper.Lerp((float) linearStep, num, Math.Abs(Math.Min(acceleration, 1f)));
    }

    public static float EaseIn(double linearStep, EasingType type)
    {
      switch (type)
      {
        case EasingType.None:
          return 1f;
        case EasingType.Linear:
          return (float) linearStep;
        case EasingType.Sine:
          return Easing.Sine.EaseIn(linearStep);
        case EasingType.Quadratic:
          return Easing.Power.EaseIn(linearStep, 2);
        case EasingType.Cubic:
          return Easing.Power.EaseIn(linearStep, 3);
        case EasingType.Quartic:
          return Easing.Power.EaseIn(linearStep, 4);
        case EasingType.Quintic:
          return Easing.Power.EaseIn(linearStep, 5);
        case EasingType.Sextic:
          return Easing.Power.EaseIn(linearStep, 6);
        case EasingType.Septic:
          return Easing.Power.EaseIn(linearStep, 7);
        case EasingType.Octic:
          return Easing.Power.EaseIn(linearStep, 8);
        case EasingType.Nonic:
          return Easing.Power.EaseIn(linearStep, 9);
        case EasingType.Decic:
          return Easing.Power.EaseIn(linearStep, 10);
        case EasingType.Circular:
          return Easing.Circular.EaseIn(linearStep);
        default:
          throw new NotImplementedException();
      }
    }

    public static float EaseOut(double linearStep, EasingType type)
    {
      switch (type)
      {
        case EasingType.None:
          return 1f;
        case EasingType.Linear:
          return (float) linearStep;
        case EasingType.Sine:
          return Easing.Sine.EaseOut(linearStep);
        case EasingType.Quadratic:
          return Easing.Power.EaseOut(linearStep, 2);
        case EasingType.Cubic:
          return Easing.Power.EaseOut(linearStep, 3);
        case EasingType.Quartic:
          return Easing.Power.EaseOut(linearStep, 4);
        case EasingType.Quintic:
          return Easing.Power.EaseOut(linearStep, 5);
        case EasingType.Sextic:
          return Easing.Power.EaseOut(linearStep, 6);
        case EasingType.Septic:
          return Easing.Power.EaseOut(linearStep, 7);
        case EasingType.Octic:
          return Easing.Power.EaseOut(linearStep, 8);
        case EasingType.Nonic:
          return Easing.Power.EaseOut(linearStep, 9);
        case EasingType.Decic:
          return Easing.Power.EaseOut(linearStep, 10);
        case EasingType.Circular:
          return Easing.Circular.EaseOut(linearStep);
        default:
          throw new NotImplementedException();
      }
    }

    public static float EaseInOut(double linearStep, EasingType easeInType, float acceleration, EasingType easeOutType, float deceleration)
    {
      if (linearStep >= 0.5)
        return MathHelper.Lerp((float) linearStep, Easing.EaseInOut(linearStep, easeOutType), deceleration);
      else
        return MathHelper.Lerp((float) linearStep, Easing.EaseInOut(linearStep, easeInType), acceleration);
    }

    public static float EaseInOut(double linearStep, EasingType easeInType, EasingType easeOutType)
    {
      if (linearStep >= 0.5)
        return Easing.EaseInOut(linearStep, easeOutType);
      else
        return Easing.EaseInOut(linearStep, easeInType);
    }

    public static float EaseInOut(double linearStep, EasingType type)
    {
      switch (type)
      {
        case EasingType.None:
          return 1f;
        case EasingType.Linear:
          return (float) linearStep;
        case EasingType.Sine:
          return Easing.Sine.EaseInOut(linearStep);
        case EasingType.Quadratic:
          return Easing.Power.EaseInOut(linearStep, 2);
        case EasingType.Cubic:
          return Easing.Power.EaseInOut(linearStep, 3);
        case EasingType.Quartic:
          return Easing.Power.EaseInOut(linearStep, 4);
        case EasingType.Quintic:
          return Easing.Power.EaseInOut(linearStep, 5);
        case EasingType.Sextic:
          return Easing.Power.EaseInOut(linearStep, 6);
        case EasingType.Septic:
          return Easing.Power.EaseInOut(linearStep, 7);
        case EasingType.Octic:
          return Easing.Power.EaseInOut(linearStep, 8);
        case EasingType.Nonic:
          return Easing.Power.EaseInOut(linearStep, 9);
        case EasingType.Decic:
          return Easing.Power.EaseInOut(linearStep, 10);
        case EasingType.Circular:
          return Easing.Circular.EaseInOut(linearStep);
        default:
          throw new NotImplementedException();
      }
    }

    private static class Sine
    {
      public static float EaseIn(double s)
      {
        return (float) Math.Sin(s * 1.57079637050629 - 1.57079637050629) + 1f;
      }

      public static float EaseOut(double s)
      {
        return (float) Math.Sin(s * 1.57079637050629);
      }

      public static float EaseInOut(double s)
      {
        return (float) (Math.Sin(s * 3.14159274101257 - 1.57079637050629) + 1.0) / 2f;
      }
    }

    private static class Power
    {
      public static float EaseIn(double s, int power)
      {
        return (float) Math.Pow(s, (double) power);
      }

      public static float EaseOut(double s, int power)
      {
        int num = power % 2 == 0 ? -1 : 1;
        return (float) num * ((float) Math.Pow(s - 1.0, (double) power) + (float) num);
      }

      public static float EaseInOut(double s, int power)
      {
        s *= 2.0;
        if (s < 1.0)
          return Easing.Power.EaseIn(s, power) / 2f;
        int num = power % 2 == 0 ? -1 : 1;
        return (float) ((double) num / 2.0 * (Math.Pow(s - 2.0, (double) power) + (double) (num * 2)));
      }
    }

    private static class Circular
    {
      public static float EaseIn(double s)
      {
        return (float) -(Math.Sqrt(1.0 - s * s) - 1.0);
      }

      public static float EaseOut(double s)
      {
        return (float) Math.Sqrt(1.0 - (s - 1.0) * s);
      }

      public static float EaseInOut(double s)
      {
        s *= 2.0;
        if (s < 1.0)
          return Easing.Circular.EaseIn(s) / 2f;
        else
          return (float) (Math.Sqrt(1.0 - (s - 2.0) * s) + 1.0) / 2f;
      }
    }
  }
}
