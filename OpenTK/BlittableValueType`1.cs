// Type: OpenTK.BlittableValueType`1
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenTK
{
  public static class BlittableValueType<T>
  {
    private static readonly Type Type = typeof (T);
    private static readonly int stride;

    public static int Stride
    {
      get
      {
        return BlittableValueType<T>.stride;
      }
    }

    static BlittableValueType()
    {
      if (!BlittableValueType<T>.Type.IsValueType || BlittableValueType<T>.Type.IsGenericType)
        return;
      BlittableValueType<T>.stride = Marshal.SizeOf(typeof (T));
    }

    public static bool Check()
    {
      return BlittableValueType<T>.Check(BlittableValueType<T>.Type);
    }

    public static bool Check(Type type)
    {
      BlittableValueType<T>.CheckStructLayoutAttribute(type);
      return BlittableValueType<T>.CheckType(type);
    }

    private static bool CheckType(Type type)
    {
      if (type.IsPrimitive)
        return true;
      if (!type.IsValueType)
        return false;
      foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!BlittableValueType<T>.CheckType(fieldInfo.FieldType))
          return false;
      }
      return BlittableValueType<T>.Stride != 0;
    }

    private static bool CheckStructLayoutAttribute(Type type)
    {
      StructLayoutAttribute[] structLayoutAttributeArray = (StructLayoutAttribute[]) type.GetCustomAttributes(typeof (StructLayoutAttribute), true);
      return structLayoutAttributeArray != null && (structLayoutAttributeArray == null || structLayoutAttributeArray.Length <= 0 || (structLayoutAttributeArray[0].Value == LayoutKind.Explicit || structLayoutAttributeArray[0].Pack == 1));
    }
  }
}
