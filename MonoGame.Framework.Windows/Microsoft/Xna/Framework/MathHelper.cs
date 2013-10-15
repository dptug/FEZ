// Type: Microsoft.Xna.Framework.MathHelper
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework
{
  public static class MathHelper
  {
    public const float E = 2.718282f;
    public const float Log10E = 0.4342945f;
    public const float Log2E = 1.442695f;
    public const float Pi = 3.141593f;
    public const float PiOver2 = 1.570796f;
    public const float PiOver4 = 0.7853982f;
    public const float TwoPi = 6.283185f;

    public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
    {
      return (float) ((double) value1 + ((double) value2 - (double) value1) * (double) amount1 + ((double) value3 - (double) value1) * (double) amount2);
    }

    public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
    {
      double num1 = (double) amount * (double) amount;
      double num2 = num1 * (double) amount;
      return (float) (0.5 * (2.0 * (double) value2 + ((double) value3 - (double) value1) * (double) amount + (2.0 * (double) value1 - 5.0 * (double) value2 + 4.0 * (double) value3 - (double) value4) * num1 + (3.0 * (double) value2 - (double) value1 - 3.0 * (double) value3 + (double) value4) * num2));
    }

    public static float Clamp(float value, float min, float max)
    {
      value = (double) value > (double) max ? max : value;
      value = (double) value < (double) min ? min : value;
      return value;
    }

    public static float Distance(float value1, float value2)
    {
      return Math.Abs(value1 - value2);
    }

    public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
    {
      double num1 = (double) value1;
      double num2 = (double) value2;
      double num3 = (double) tangent1;
      double num4 = (double) tangent2;
      double num5 = (double) amount;
      double num6 = num5 * num5 * num5;
      double num7 = num5 * num5;
      return (double) amount != 0.0 ? ((double) amount != 1.0 ? (float) ((2.0 * num1 - 2.0 * num2 + num4 + num3) * num6 + (3.0 * num2 - 3.0 * num1 - 2.0 * num3 - num4) * num7 + num3 * num5 + num1) : value2) : value1;
    }

    public static float Lerp(float value1, float value2, float amount)
    {
      return value1 + (value2 - value1) * amount;
    }

    public static float Max(float value1, float value2)
    {
      return Math.Max(value1, value2);
    }

    public static float Min(float value1, float value2)
    {
      return Math.Min(value1, value2);
    }

    public static float SmoothStep(float value1, float value2, float amount)
    {
      float amount1 = MathHelper.Clamp(amount, 0.0f, 1f);
      return MathHelper.Hermite(value1, 0.0f, value2, 0.0f, amount1);
    }

    public static float ToDegrees(float radians)
    {
      return radians * 57.29578f;
    }

    public static float ToRadians(float degrees)
    {
      return degrees * (Math.PI / 180.0);
    }

    public static float WrapAngle(float angle)
    {
      angle = (float) Math.IEEERemainder((double) angle, 6.28318548202515);
      if ((double) angle <= -3.14159274101257)
        angle += 6.283185f;
      else if ((double) angle > 3.14159274101257)
        angle -= 6.283185f;
      return angle;
    }

    public static bool IsPowerOfTwo(int value)
    {
      return value > 0 && (value & value - 1) == 0;
    }
  }
}
