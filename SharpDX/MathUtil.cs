// Type: SharpDX.MathUtil
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public static class MathUtil
  {
    public const float ZeroTolerance = 1E-06f;
    public const float Pi = 3.141593f;
    public const float TwoPi = 6.283185f;
    public const float PiOverTwo = 1.570796f;
    public const float PiOverFour = 0.7853982f;

    public static bool WithinEpsilon(float a, float b)
    {
      float num = a - b;
      if (-1.40129846432482E-45 <= (double) num)
        return (double) num <= 1.40129846432482E-45;
      else
        return false;
    }

    public static T[] Array<T>(T value, int count)
    {
      T[] objArray = new T[count];
      for (int index = 0; index < count; ++index)
        objArray[index] = value;
      return objArray;
    }

    public static float RevolutionsToDegrees(float revolution)
    {
      return revolution * 360f;
    }

    public static float RevolutionsToRadians(float revolution)
    {
      return revolution * 6.283185f;
    }

    public static float RevolutionsToGradians(float revolution)
    {
      return revolution * 400f;
    }

    public static float DegreesToRevolutions(float degree)
    {
      return degree / 360f;
    }

    public static float DegreesToRadians(float degree)
    {
      // ISSUE: unable to decompile the method.
    }

    public static float RadiansToRevolutions(float radian)
    {
      return radian / 6.283185f;
    }

    public static float RadiansToGradians(float radian)
    {
      return radian * 63.66198f;
    }

    public static float GradiansToRevolutions(float gradian)
    {
      return gradian / 400f;
    }

    public static float GradiansToDegrees(float gradian)
    {
      return gradian * 0.9f;
    }

    public static float GradiansToRadians(float gradian)
    {
      return gradian * 0.01570796f;
    }

    public static float RadiansToDegrees(float radian)
    {
      return radian * 57.29578f;
    }

    public static float Clamp(float value, float min, float max)
    {
      if ((double) value < (double) min)
        return min;
      if ((double) value <= (double) max)
        return value;
      else
        return max;
    }

    public static int Clamp(int value, int min, int max)
    {
      if (value < min)
        return min;
      if (value <= max)
        return value;
      else
        return max;
    }

    public static float Mod(float value, float modulo)
    {
      if ((double) modulo == 0.0)
        return value;
      else
        return value - modulo * (float) Math.Floor((double) value / (double) modulo);
    }

    public static float Mod2PI(float value)
    {
      return MathUtil.Mod(value, 6.283185f);
    }

    public static int Wrap(int value, int min, int max)
    {
      if (min > max)
      {
        int num = min;
        min = max;
        max = num;
      }
      value -= min;
      int num1 = max - min;
      if (num1 == 0)
        return max;
      else
        return value - num1 * (value / num1) + min;
    }

    public static float Wrap(float value, float min, float max)
    {
      if ((double) min > (double) max)
      {
        float num = min;
        min = max;
        max = num;
      }
      value -= min;
      float num1 = max - min;
      if ((double) num1 == 0.0)
        return max;
      else
        return value - num1 * (float) Math.Floor((double) value / (double) num1) + min;
    }

    public static float Gauss(float amplitude, float x, float y, float radX, float radY, float sigmaX, float sigmaY)
    {
      return amplitude * 2.718282f - (float) (Math.Pow((double) x - (double) radX / 2.0, 2.0) / (2.0 * Math.Pow((double) sigmaX, 2.0)) + Math.Pow((double) y - (double) radY / 2.0, 2.0) / (2.0 * Math.Pow((double) sigmaY, 2.0)));
    }

    public static double Gauss(double amplitude, double x, double y, double radX, double radY, double sigmaX, double sigmaY)
    {
      return amplitude * 2.718281828 - (Math.Pow(x - radX / 2.0, 2.0) / (2.0 * Math.Pow(sigmaX, 2.0)) + Math.Pow(y - radY / 2.0, 2.0) / (2.0 * Math.Pow(sigmaY, 2.0)));
    }

    public static float NextFloat(this Random random, float min, float max)
    {
      return min + (float) (random.NextDouble() * ((double) max - (double) min));
    }

    public static double NextDouble(this Random random, double min, double max)
    {
      return min + random.NextDouble() * (max - min);
    }

    public static long NextLong(this Random random)
    {
      byte[] buffer = new byte[8];
      random.NextBytes(buffer);
      return BitConverter.ToInt64(buffer, 0);
    }

    public static long NextLong(this Random random, long min, long max)
    {
      byte[] buffer = new byte[8];
      random.NextBytes(buffer);
      return Math.Abs(BitConverter.ToInt64(buffer, 0) % (max - min)) + min;
    }

    public static Vector2 NextVector2(this Random random, Vector2 min, Vector2 max)
    {
      return new Vector2(MathUtil.NextFloat(random, min.X, max.X), MathUtil.NextFloat(random, min.Y, max.Y));
    }

    public static Vector3 NextVector3(this Random random, Vector3 min, Vector3 max)
    {
      return new Vector3(MathUtil.NextFloat(random, min.X, max.X), MathUtil.NextFloat(random, min.Y, max.Y), MathUtil.NextFloat(random, min.Z, max.Z));
    }

    public static Vector4 NextVector4(this Random random, Vector4 min, Vector4 max)
    {
      return new Vector4(MathUtil.NextFloat(random, min.X, max.X), MathUtil.NextFloat(random, min.Y, max.Y), MathUtil.NextFloat(random, min.Z, max.Z), MathUtil.NextFloat(random, min.W, max.W));
    }

    public static Color NextColor(this Random random)
    {
      return new Color(MathUtil.NextFloat(random, 0.0f, 1f), MathUtil.NextFloat(random, 0.0f, 1f), MathUtil.NextFloat(random, 0.0f, 1f), 1f);
    }

    public static Color NextColor(this Random random, float minBrightness, float maxBrightness)
    {
      return new Color(MathUtil.NextFloat(random, minBrightness, maxBrightness), MathUtil.NextFloat(random, minBrightness, maxBrightness), MathUtil.NextFloat(random, minBrightness, maxBrightness), 1f);
    }

    public static Color NextColor(this Random random, float minBrightness, float maxBrightness, float alpha)
    {
      return new Color(MathUtil.NextFloat(random, minBrightness, maxBrightness), MathUtil.NextFloat(random, minBrightness, maxBrightness), MathUtil.NextFloat(random, minBrightness, maxBrightness), alpha);
    }

    public static Color NextColor(this Random random, float minBrightness, float maxBrightness, float minAlpha, float maxAlpha)
    {
      return new Color(MathUtil.NextFloat(random, minBrightness, maxBrightness), MathUtil.NextFloat(random, minBrightness, maxBrightness), MathUtil.NextFloat(random, minBrightness, maxBrightness), MathUtil.NextFloat(random, minAlpha, maxAlpha));
    }

    public static DrawingPoint NextDPoint(this Random random, DrawingPoint min, DrawingPoint max)
    {
      return new DrawingPoint(random.Next(min.X, max.X), random.Next(min.Y, max.Y));
    }

    public static DrawingPointF NextDPointF(this Random random, DrawingPointF min, DrawingPointF max)
    {
      return new DrawingPointF(MathUtil.NextFloat(random, min.X, max.X), MathUtil.NextFloat(random, min.Y, max.Y));
    }

    public static TimeSpan NextTimespan(this Random random, TimeSpan min, TimeSpan max)
    {
      return TimeSpan.FromTicks(MathUtil.NextLong(random, min.Ticks, max.Ticks));
    }
  }
}
