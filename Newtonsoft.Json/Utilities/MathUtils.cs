// Type: Newtonsoft.Json.Utilities.MathUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Utilities
{
  internal class MathUtils
  {
    public static int IntLength(int i)
    {
      if (i < 0)
        throw new ArgumentOutOfRangeException();
      if (i == 0)
        return 1;
      else
        return (int) Math.Floor(Math.Log10((double) i)) + 1;
    }

    public static char IntToHex(int n)
    {
      if (n <= 9)
        return (char) (n + 48);
      else
        return (char) (n - 10 + 97);
    }

    public static int? Min(int? val1, int? val2)
    {
      if (!val1.HasValue)
        return val2;
      if (!val2.HasValue)
        return val1;
      else
        return new int?(Math.Min(val1.Value, val2.Value));
    }

    public static int? Max(int? val1, int? val2)
    {
      if (!val1.HasValue)
        return val2;
      if (!val2.HasValue)
        return val1;
      else
        return new int?(Math.Max(val1.Value, val2.Value));
    }

    public static double? Max(double? val1, double? val2)
    {
      if (!val1.HasValue)
        return val2;
      if (!val2.HasValue)
        return val1;
      else
        return new double?(Math.Max(val1.Value, val2.Value));
    }

    public static bool ApproxEquals(double d1, double d2)
    {
      if (d1 == d2)
        return true;
      double num1 = (Math.Abs(d1) + Math.Abs(d2) + 10.0) * 2.22044604925031E-16;
      double num2 = d1 - d2;
      if (-num1 < num2)
        return num1 > num2;
      else
        return false;
    }
  }
}
