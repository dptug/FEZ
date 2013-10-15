// Type: OpenTK.BlittableValueType
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  public static class BlittableValueType
  {
    public static bool Check<T>(T type)
    {
      return BlittableValueType<T>.Check();
    }

    public static bool Check<T>(T[] type)
    {
      return BlittableValueType<T>.Check();
    }

    public static bool Check<T>(T[,] type)
    {
      return BlittableValueType<T>.Check();
    }

    public static bool Check<T>(T[,,] type)
    {
      return BlittableValueType<T>.Check();
    }

    [CLSCompliant(false)]
    public static bool Check<T>(T[][] type)
    {
      return BlittableValueType<T>.Check();
    }

    public static int StrideOf<T>(T type)
    {
      if (!BlittableValueType.Check<T>(type))
        throw new ArgumentException("type");
      else
        return BlittableValueType<T>.Stride;
    }

    public static int StrideOf<T>(T[] type)
    {
      if (!BlittableValueType.Check<T>(type))
        throw new ArgumentException("type");
      else
        return BlittableValueType<T>.Stride;
    }

    public static int StrideOf<T>(T[,] type)
    {
      if (!BlittableValueType.Check<T>(type))
        throw new ArgumentException("type");
      else
        return BlittableValueType<T>.Stride;
    }

    public static int StrideOf<T>(T[,,] type)
    {
      if (!BlittableValueType.Check<T>(type))
        throw new ArgumentException("type");
      else
        return BlittableValueType<T>.Stride;
    }
  }
}
