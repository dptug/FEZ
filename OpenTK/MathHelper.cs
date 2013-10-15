// Type: OpenTK.MathHelper
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  public static class MathHelper
  {
    public const float Pi = 3.141593f;
    public const float PiOver2 = 1.570796f;
    public const float PiOver3 = 1.047198f;
    public const float PiOver4 = 0.7853982f;
    public const float PiOver6 = 0.5235988f;
    public const float TwoPi = 6.283185f;
    public const float ThreePiOver2 = 4.712389f;
    public const float E = 2.718282f;
    public const float Log10E = 0.4342945f;
    public const float Log2E = 1.442695f;

    public static long NextPowerOfTwo(long n)
    {
      if (n < 0L)
        throw new ArgumentOutOfRangeException("n", "Must be positive.");
      else
        return (long) Math.Pow(2.0, Math.Ceiling(Math.Log((double) n, 2.0)));
    }

    public static int NextPowerOfTwo(int n)
    {
      if (n < 0)
        throw new ArgumentOutOfRangeException("n", "Must be positive.");
      else
        return (int) Math.Pow(2.0, Math.Ceiling(Math.Log((double) n, 2.0)));
    }

    public static float NextPowerOfTwo(float n)
    {
      if ((double) n < 0.0)
        throw new ArgumentOutOfRangeException("n", "Must be positive.");
      else
        return (float) Math.Pow(2.0, Math.Ceiling(Math.Log((double) n, 2.0)));
    }

    public static double NextPowerOfTwo(double n)
    {
      if (n < 0.0)
        throw new ArgumentOutOfRangeException("n", "Must be positive.");
      else
        return Math.Pow(2.0, Math.Ceiling(Math.Log(n, 2.0)));
    }

    public static long Factorial(int n)
    {
      long num = 1L;
      for (; n > 1; --n)
        num *= (long) n;
      return num;
    }

    public static long BinomialCoefficient(int n, int k)
    {
      return MathHelper.Factorial(n) / (MathHelper.Factorial(k) * MathHelper.Factorial(n - k));
    }

    public static unsafe float InverseSqrtFast(float x)
    {
      float num = 0.5f * x;
      x = *(float*) &(1597463174 - (*(int*) &x >> 1));
      x *= (float) (1.5 - (double) num * (double) x * (double) x);
      return x;
    }

    public static double InverseSqrtFast(double x)
    {
      return (double) MathHelper.InverseSqrtFast((float) x);
    }

    public static float DegreesToRadians(float degrees)
    {
      // ISSUE: unable to decompile the method.
    }

    public static float RadiansToDegrees(float radians)
    {
      return radians * 57.29578f;
    }

    public static double DegreesToRadians(double degrees)
    {
      return degrees * (Math.PI / 180.0);
    }

    public static double RadiansToDegrees(double radians)
    {
      return radians * 57.2957795130823;
    }

    public static void Swap(ref double a, ref double b)
    {
      double num = a;
      a = b;
      b = num;
    }

    public static void Swap(ref float a, ref float b)
    {
      float num = a;
      a = b;
      b = num;
    }
  }
}
