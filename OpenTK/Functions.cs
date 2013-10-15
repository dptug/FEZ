// Type: OpenTK.Functions
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  [Obsolete("Use OpenTK.MathHelper instead.")]
  public static class Functions
  {
    public static readonly float PIF = 3.141593f;
    public static readonly float RTODF = 180f / Functions.PIF;
    public static readonly float DTORF = Functions.PIF / 180f;
    public static readonly double PI = Math.PI;
    public static readonly double RTOD = 180.0 / (double) Functions.PIF;
    public static readonly double DTOR = (double) Functions.PIF / 180.0;

    static Functions()
    {
    }

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
      return Functions.Factorial(n) / (Functions.Factorial(k) * Functions.Factorial(n - k));
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
      return (double) Functions.InverseSqrtFast((float) x);
    }

    public static float DegreesToRadians(float degrees)
    {
      // ISSUE: unable to decompile the method.
    }

    public static float RadiansToDegrees(float radians)
    {
      return radians * 57.29578f;
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
