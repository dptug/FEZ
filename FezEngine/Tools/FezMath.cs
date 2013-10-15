// Type: FezEngine.Tools.FezMath
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public static class FezMath
  {
    public static readonly Vector3 HalfVector = new Vector3(0.5f);
    public static readonly Vector3 XZMask = new Vector3(1f, 0.0f, 1f);
    private static readonly double Log2 = Math.Log(2.0);
    private static readonly Vector3[] tempCorners = new Vector3[8];
    public const float Epsilon = 0.001f;

    static FezMath()
    {
    }

    public static float DoubleIter(float currentValue, float iterationTime, float timeUntilDoubles)
    {
      float num1 = timeUntilDoubles / iterationTime;
      double num2 = (double) timeUntilDoubles == 0.0 ? 0.0 : Math.Pow(2.0, 1.0 / (double) num1);
      return currentValue * (float) num2;
    }

    public static double Saturate(double value)
    {
      if (value < 0.0)
        return 0.0;
      if (value <= 1.0)
        return value;
      else
        return 1.0;
    }

    public static float Saturate(float value)
    {
      if ((double) value < 0.0)
        return 0.0f;
      if ((double) value <= 1.0)
        return value;
      else
        return 1f;
    }

    public static Vector3 Saturate(this Vector3 vector)
    {
      return new Vector3(FezMath.Saturate(vector.X), FezMath.Saturate(vector.Y), FezMath.Saturate(vector.Z));
    }

    public static Vector3 MaxClamp(this Vector3 vector)
    {
      Vector3 vector3 = FezMath.Abs(vector);
      if ((double) vector3.X >= (double) vector3.Y && (double) vector3.X >= (double) vector3.Z)
        return new Vector3((float) Math.Sign(vector.X), 0.0f, 0.0f);
      if ((double) vector3.Y >= (double) vector3.X && (double) vector3.Y >= (double) vector3.Z)
        return new Vector3(0.0f, (float) Math.Sign(vector.Y), 0.0f);
      if ((double) vector3.Z >= (double) vector3.X && (double) vector3.Z >= (double) vector3.Y)
        return new Vector3(0.0f, 0.0f, (float) Math.Sign(vector.Z));
      else
        return Vector3.Zero;
    }

    public static Vector3 MaxClampXZ(this Vector3 vector)
    {
      Vector3 vector3 = FezMath.Abs(vector);
      if ((double) vector3.X >= (double) vector3.Z)
        return new Vector3((float) Math.Sign(vector.X), 0.0f, 0.0f);
      else
        return new Vector3(0.0f, 0.0f, (float) Math.Sign(vector.Z));
    }

    public static Vector3 UpVector(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
          return Vector3.Up;
        case Viewpoint.Right:
          return Vector3.Up;
        case Viewpoint.Back:
          return Vector3.Up;
        case Viewpoint.Left:
          return Vector3.Up;
        case Viewpoint.Up:
          return Vector3.Backward;
        case Viewpoint.Down:
          return Vector3.Backward;
        default:
          return Vector3.Zero;
      }
    }

    public static Vector3 RightVector(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
          return Vector3.Right;
        case Viewpoint.Right:
          return Vector3.Forward;
        case Viewpoint.Back:
          return Vector3.Left;
        case Viewpoint.Left:
          return Vector3.Backward;
        case Viewpoint.Up:
          return Vector3.Right;
        case Viewpoint.Down:
          return Vector3.Right;
        default:
          return Vector3.Zero;
      }
    }

    public static Vector3 ScreenSpaceMask(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
          return new Vector3(1f, 1f, 0.0f);
        case Viewpoint.Right:
          return new Vector3(0.0f, 1f, 1f);
        case Viewpoint.Back:
          return new Vector3(1f, 1f, 0.0f);
        case Viewpoint.Left:
          return new Vector3(0.0f, 1f, 1f);
        case Viewpoint.Up:
          return new Vector3(1f, 1f, 0.0f);
        case Viewpoint.Down:
          return new Vector3(1f, 1f, 0.0f);
        default:
          return Vector3.Zero;
      }
    }

    public static Vector3 SideMask(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
          return Vector3.Right;
        case Viewpoint.Right:
          return Vector3.Backward;
        case Viewpoint.Back:
          return Vector3.Right;
        case Viewpoint.Left:
          return Vector3.Backward;
        case Viewpoint.Up:
          return Vector3.Right;
        case Viewpoint.Down:
          return Vector3.Right;
        default:
          return Vector3.Zero;
      }
    }

    public static Vector3 ForwardVector(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
          return Vector3.Forward;
        case Viewpoint.Right:
          return Vector3.Left;
        case Viewpoint.Back:
          return Vector3.Backward;
        case Viewpoint.Left:
          return Vector3.Right;
        case Viewpoint.Up:
          return Vector3.Up;
        case Viewpoint.Down:
          return Vector3.Down;
        default:
          return Vector3.Zero;
      }
    }

    public static Vector3 DepthMask(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
          return Vector3.UnitZ;
        case Viewpoint.Right:
          return Vector3.UnitX;
        case Viewpoint.Back:
          return Vector3.UnitZ;
        case Viewpoint.Left:
          return Vector3.UnitX;
        case Viewpoint.Up:
          return Vector3.UnitY;
        case Viewpoint.Down:
          return Vector3.UnitY;
        default:
          return Vector3.Zero;
      }
    }

    public static Vector2 ComputeTexCoord<T>(this T vertex) where T : ILitVertex, ITexturedVertex
    {
      return FezMath.ComputeTexCoord<T>(vertex, Vector3.One);
    }

    public static Vector2 ComputeTexCoord<T>(this T vertex, Vector3 trileSize) where T : ILitVertex, ITexturedVertex
    {
      FaceOrientation orientation = FezMath.OrientationFromDirection(vertex.Normal);
      Vector2 vector2;
      switch (orientation)
      {
        case FaceOrientation.Left:
          vector2 = new Vector2(0.375f, 0.0f);
          break;
        case FaceOrientation.Back:
          vector2 = new Vector2(0.375f, 0.0f);
          break;
        case FaceOrientation.Right:
          vector2 = new Vector2(0.25f, 0.0f);
          break;
        case FaceOrientation.Top:
          vector2 = new Vector2(0.5f, 0.0f);
          break;
        case FaceOrientation.Front:
          vector2 = Vector2.Zero;
          break;
        default:
          vector2 = new Vector2(0.625f, 0.0f);
          break;
      }
      Vector3 b = ((Vector3.One - FezMath.Abs(vertex.Normal)) * vertex.Position / trileSize * 2f + vertex.Normal) / 2f + new Vector3(0.5f);
      float num1 = FezMath.Dot(FezMath.RightVector(FezMath.AsViewpoint(orientation)), b);
      float num2 = FezMath.Dot(FezMath.UpVector(FezMath.AsViewpoint(orientation)), b);
      if (orientation != FaceOrientation.Top)
        num2 = 1f - num2;
      return new Vector2(vector2.X + num1 / 8f, vector2.Y + num2);
    }

    public static int NextPowerOfTwo(double value)
    {
      return (int) Math.Pow(2.0, Math.Ceiling(Math.Log(value) / FezMath.Log2));
    }

    public static float Log(double b, double r)
    {
      return (float) (Math.Log(b) / Math.Log(r));
    }

    public static bool In<T>(T value, T value1, T value2, IEqualityComparer<T> comparer)
    {
      if (!comparer.Equals(value, value1))
        return comparer.Equals(value, value2);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, IEqualityComparer<T> comparer)
    {
      if (!FezMath.In<T>(value, value1, value2, comparer))
        return comparer.Equals(value, value3);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4, IEqualityComparer<T> comparer)
    {
      if (!FezMath.In<T>(value, value1, value2, value3, comparer))
        return comparer.Equals(value, value4);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4, T value5, IEqualityComparer<T> comparer)
    {
      if (!FezMath.In<T>(value, value1, value2, value3, value4, comparer))
        return comparer.Equals(value, value5);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4, T value5, T value6, IEqualityComparer<T> comparer)
    {
      if (!FezMath.In<T>(value, value1, value2, value3, value4, value5, comparer))
        return comparer.Equals(value, value6);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4, T value5, T value6, T value7, IEqualityComparer<T> comparer)
    {
      if (!FezMath.In<T>(value, value1, value2, value3, value4, value5, value6, comparer))
        return comparer.Equals(value, value7);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4, T value5, T value6, T value7, T value8, IEqualityComparer<T> comparer)
    {
      if (!FezMath.In<T>(value, value1, value2, value3, value4, value5, value6, value7, comparer))
        return comparer.Equals(value, value8);
      else
        return true;
    }

    public static bool In<T>(T value, T[] values) where T : IEquatable<T>
    {
      foreach (T other in values)
      {
        if (value.Equals(other))
          return true;
      }
      return false;
    }

    public static bool In<T>(T value, T value1, T value2) where T : IEquatable<T>
    {
      if (!value.Equals(value1))
        return value.Equals(value2);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3) where T : IEquatable<T>
    {
      if (!FezMath.In<T>(value, value1, value2))
        return value.Equals(value3);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4) where T : IEquatable<T>
    {
      if (!FezMath.In<T>(value, value1, value2, value3))
        return value.Equals(value4);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4, T value5) where T : IEquatable<T>
    {
      if (!FezMath.In<T>(value, value1, value2, value3, value4))
        return value.Equals(value5);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4, T value5, T value6) where T : IEquatable<T>
    {
      if (!FezMath.In<T>(value, value1, value2, value3, value4, value5))
        return value.Equals(value6);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4, T value5, T value6, T value7) where T : IEquatable<T>
    {
      if (!FezMath.In<T>(value, value1, value2, value3, value4, value5, value6))
        return value.Equals(value7);
      else
        return true;
    }

    public static bool In<T>(T value, T value1, T value2, T value3, T value4, T value5, T value6, T value7, T value8) where T : IEquatable<T>
    {
      if (!FezMath.In<T>(value, value1, value2, value3, value4, value5, value6, value7))
        return value.Equals(value8);
      else
        return true;
    }

    public static Vector3 Max(Vector3 first, Vector3 second)
    {
      Vector3 vector3 = first;
      vector3 = new Vector3(Math.Max(vector3.X, second.X), Math.Max(vector3.Y, second.Y), Math.Max(vector3.Z, second.Z));
      return vector3;
    }

    public static T Max<T>(T first, T second) where T : IComparable<T>
    {
      T obj = first;
      return obj.CompareTo(second) > 0 ? obj : second;
    }

    public static T Max<T>(T first, T second, T third) where T : IComparable<T>
    {
      T obj1 = first;
      T obj2 = obj1.CompareTo(second) > 0 ? obj1 : second;
      obj2 = obj2.CompareTo(third) > 0 ? obj2 : third;
      return obj2;
    }

    public static Vector3 Min(Vector3 first, Vector3 second)
    {
      Vector3 vector3 = first;
      vector3 = new Vector3(Math.Min(vector3.X, second.X), Math.Min(vector3.Y, second.Y), Math.Min(vector3.Z, second.Z));
      return vector3;
    }

    public static T Min<T>(T first, T second) where T : IComparable<T>
    {
      T obj = first;
      return obj.CompareTo(second) < 0 ? obj : second;
    }

    public static T Min<T>(T first, T second, T third) where T : IComparable<T>
    {
      T obj1 = first;
      T obj2 = obj1.CompareTo(second) < 0 ? obj1 : second;
      obj2 = obj2.CompareTo(third) < 0 ? obj2 : third;
      return obj2;
    }

    public static T Coalesce<T>(T first, T second, IEqualityComparer<T> comparer) where T : struct
    {
      T x = default (T);
      if (!comparer.Equals(x, first))
        return first;
      if (!comparer.Equals(x, second))
        return second;
      else
        return x;
    }

    public static T Coalesce<T>(T first, T second, T third, IEqualityComparer<T> comparer) where T : struct
    {
      T x = default (T);
      if (!comparer.Equals(x, first))
        return first;
      if (!comparer.Equals(x, second))
        return second;
      if (!comparer.Equals(x, third))
        return third;
      else
        return x;
    }

    public static T Coalesce<T>(T first, T second) where T : struct, IEquatable<T>
    {
      T other = default (T);
      if (!first.Equals(other))
        return first;
      if (!second.Equals(other))
        return second;
      else
        return other;
    }

    public static T Coalesce<T>(T first, T second, T third) where T : struct, IEquatable<T>
    {
      T other = default (T);
      if (!first.Equals(other))
        return first;
      if (!second.Equals(other))
        return second;
      if (!third.Equals(other))
        return third;
      else
        return other;
    }

    public static int Round(double value)
    {
      if (value < 0.0)
        return (int) (value - 0.5);
      else
        return (int) (value + 0.5);
    }

    public static Vector2 Round(this Vector2 v)
    {
      return new Vector2((float) FezMath.Round((double) v.X), (float) FezMath.Round((double) v.Y));
    }

    public static Vector2 Round(this Vector2 v, int d)
    {
      return new Vector2((float) Math.Round((double) v.X, d), (float) Math.Round((double) v.Y, d));
    }

    public static Vector3 Round(this Vector3 v)
    {
      return new Vector3((float) FezMath.Round((double) v.X), (float) FezMath.Round((double) v.Y), (float) FezMath.Round((double) v.Z));
    }

    public static Vector3 Round(this Vector3 v, int d)
    {
      return new Vector3((float) Math.Round((double) v.X, d), (float) Math.Round((double) v.Y, d), (float) Math.Round((double) v.Z, d));
    }

    public static Vector3 Floor(this Vector3 v)
    {
      return new Vector3((float) Math.Floor((double) v.X), (float) Math.Floor((double) v.Y), (float) Math.Floor((double) v.Z));
    }

    public static Vector3 Ceiling(this Vector3 v)
    {
      return new Vector3((float) Math.Ceiling((double) v.X), (float) Math.Ceiling((double) v.Y), (float) Math.Ceiling((double) v.Z));
    }

    public static Vector3 AlmostClamp(Vector3 v, float epsilon)
    {
      v.X = FezMath.AlmostClamp(v.X, epsilon);
      v.Y = FezMath.AlmostClamp(v.Y, epsilon);
      v.Z = FezMath.AlmostClamp(v.Z, epsilon);
      return v;
    }

    public static Vector3 AlmostClamp(Vector3 v)
    {
      return FezMath.AlmostClamp(v, 1.0 / 1000.0);
    }

    public static float AlmostClamp(float x, float epsilon)
    {
      if (FezMath.AlmostEqual(x, 0.0f, epsilon))
        return 0.0f;
      if (FezMath.AlmostEqual(x, 1f, epsilon))
        return 1f;
      if (FezMath.AlmostEqual(x, -1f, epsilon))
        return -1f;
      else
        return x;
    }

    public static float AlmostClamp(float x)
    {
      return FezMath.AlmostClamp(x, 1.0 / 1000.0);
    }

    public static bool AlmostEqual(double a, double b, double epsilon)
    {
      return Math.Abs(a - b) <= epsilon;
    }

    public static bool AlmostEqual(double a, double b)
    {
      return Math.Abs(a - b) <= 1.0 / 1000.0;
    }

    public static bool AlmostEqual(float a, float b, float epsilon)
    {
      return (double) Math.Abs(a - b) <= (double) epsilon;
    }

    public static bool AlmostEqual(float a, float b)
    {
      return (double) Math.Abs(a - b) <= 1.0 / 1000.0;
    }

    public static bool AlmostEqual(Vector3 a, Vector3 b)
    {
      return FezMath.AlmostEqual(a, b, 1.0 / 1000.0);
    }

    public static bool AlmostEqual(Vector3 a, Vector3 b, float epsilon)
    {
      if (FezMath.AlmostEqual(a.X, b.X, epsilon) && FezMath.AlmostEqual(a.Y, b.Y, epsilon))
        return FezMath.AlmostEqual(a.Z, b.Z, epsilon);
      else
        return false;
    }

    public static bool AlmostEqual(Quaternion a, Quaternion b)
    {
      return FezMath.AlmostEqual(a, b, 1.0 / 1000.0);
    }

    public static bool AlmostEqual(Quaternion a, Quaternion b, float epsilon)
    {
      if (FezMath.AlmostEqual(a.X, b.X, epsilon) && FezMath.AlmostEqual(a.Y, b.Y, epsilon) && FezMath.AlmostEqual(a.Z, b.Z, epsilon))
        return FezMath.AlmostEqual(a.W, b.W, epsilon);
      else
        return false;
    }

    public static bool AlmostEqual(Vector2 a, Vector2 b)
    {
      return FezMath.AlmostEqual(a, b, 1.0 / 1000.0);
    }

    public static bool AlmostEqual(Vector2 a, Vector2 b, float epsilon)
    {
      if (FezMath.AlmostEqual(a.X, b.X, epsilon))
        return FezMath.AlmostEqual(a.Y, b.Y, epsilon);
      else
        return false;
    }

    public static bool AlmostWrapEqual(Vector2 a, Vector2 b)
    {
      return FezMath.AlmostWrapEqual(a, b, 1.0 / 1000.0);
    }

    public static bool AlmostWrapEqual(Vector2 a, Vector2 b, float epsilon)
    {
      if (FezMath.AlmostWrapEqual(a.X, b.X, epsilon))
        return FezMath.AlmostWrapEqual(a.Y, b.Y, epsilon);
      else
        return false;
    }

    public static bool AlmostWrapEqual(float a, float b)
    {
      return FezMath.AlmostWrapEqual(a, b, 1.0 / 1000.0);
    }

    public static bool AlmostWrapEqual(float a, float b, float epsilon)
    {
      return FezMath.AlmostEqual(Vector2.Dot(new Vector2((float) Math.Cos((double) a), (float) Math.Sin((double) a)), new Vector2((float) Math.Cos((double) b), (float) Math.Sin((double) b))), 1f, epsilon);
    }

    public static float CurveAngle(float from, float to, float step)
    {
      if (FezMath.AlmostEqual(step, 0.0f))
        return from;
      if (FezMath.AlmostEqual(from, to) || FezMath.AlmostEqual(step, 1f))
        return to;
      Vector2 vector2 = FezMath.Slerp(new Vector2((float) Math.Cos((double) from), (float) Math.Sin((double) from)), new Vector2((float) Math.Cos((double) to), (float) Math.Sin((double) to)), step);
      return (float) Math.Atan2((double) vector2.Y, (double) vector2.X);
    }

    public static bool IsSide(this FaceOrientation orientation)
    {
      if (orientation != FaceOrientation.Down)
        return orientation != FaceOrientation.Top;
      else
        return false;
    }

    public static float ToPhi(this FaceOrientation orientation)
    {
      switch (orientation)
      {
        case FaceOrientation.Left:
          return -1.570796f;
        case FaceOrientation.Back:
          return -3.141593f;
        case FaceOrientation.Right:
          return 1.570796f;
        case FaceOrientation.Front:
          return 0.0f;
        default:
          throw new InvalidOperationException("Side orientations only");
      }
    }

    public static float ToPhi(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
          return 0.0f;
        case Viewpoint.Right:
          return 1.570796f;
        case Viewpoint.Back:
          return -3.141593f;
        case Viewpoint.Left:
          return -1.570796f;
        default:
          throw new InvalidOperationException("Orthographic views only");
      }
    }

    public static FaceOrientation OrientationFromPhi(float phi)
    {
      phi = FezMath.WrapAngle(phi);
      if (FezMath.AlmostEqual(phi, 0.0f))
        return FaceOrientation.Front;
      if (FezMath.AlmostEqual(phi, 1.570796f))
        return FaceOrientation.Right;
      if (FezMath.AlmostEqual(phi, -3.141593f))
        return FaceOrientation.Back;
      if (FezMath.AlmostEqual(phi, -1.570796f))
        return FaceOrientation.Left;
      else
        return FezMath.OrientationFromPhi(FezMath.SnapPhi(phi));
    }

    public static Vector2 Slerp(Vector2 from, Vector2 to, float step)
    {
      if (FezMath.AlmostEqual(step, 0.0f))
        return from;
      if (FezMath.AlmostEqual(step, 1f))
        return to;
      double a = Math.Acos((double) MathHelper.Clamp(Vector2.Dot(from, to), -1f, 1f));
      if (FezMath.AlmostEqual(a, 0.0))
        return to;
      double num = Math.Sin(a);
      return (float) (Math.Sin((1.0 - (double) step) * a) / num) * from + (float) (Math.Sin((double) step * a) / num) * to;
    }

    public static Vector3 Slerp(Vector3 from, Vector3 to, float step)
    {
      if (FezMath.AlmostEqual(step, 0.0f))
        return from;
      if (FezMath.AlmostEqual(step, 1f))
        return to;
      float num1 = Vector3.Dot(from, to);
      if ((double) num1 == -1.0)
      {
        Vector3 vector3 = Vector3.Cross(Vector3.Normalize(to - from), Vector3.UnitY);
        if ((double) step < 0.5)
          return FezMath.Slerp(from, vector3, step * 2f);
        else
          return FezMath.Slerp(vector3, to, (float) (((double) step - 0.5) / 2.0));
      }
      else
      {
        double a = Math.Acos((double) MathHelper.Clamp(num1, -1f, 1f));
        if (FezMath.AlmostEqual(a, 0.0))
          return to;
        double num2 = Math.Sin(a);
        return (float) (Math.Sin((1.0 - (double) step) * a) / num2) * from + (float) (Math.Sin((double) step * a) / num2) * to;
      }
    }

    public static float AngleBetween(Vector3 a, Vector3 b)
    {
      return (float) Math.Acos((double) MathHelper.Clamp(Vector3.Dot(a, b), -1f, 1f));
    }

    public static FaceOrientation VisibleOrientation(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
          return FaceOrientation.Front;
        case Viewpoint.Back:
          return FaceOrientation.Back;
        case Viewpoint.Left:
          return FaceOrientation.Left;
        case Viewpoint.Down:
          return FaceOrientation.Top;
        default:
          return FaceOrientation.Right;
      }
    }

    public static Viewpoint AsViewpoint(this FaceOrientation orientation)
    {
      switch (orientation)
      {
        case FaceOrientation.Left:
          return Viewpoint.Left;
        case FaceOrientation.Down:
          return Viewpoint.Down;
        case FaceOrientation.Back:
          return Viewpoint.Back;
        case FaceOrientation.Top:
          return Viewpoint.Up;
        case FaceOrientation.Front:
          return Viewpoint.Front;
        default:
          return Viewpoint.Right;
      }
    }

    public static Axis AsAxis(this FaceOrientation face)
    {
      switch (face)
      {
        case FaceOrientation.Left:
        case FaceOrientation.Right:
          return Axis.X;
        case FaceOrientation.Back:
        case FaceOrientation.Front:
          return Axis.Z;
        default:
          return Axis.Y;
      }
    }

    public static Vector3 GetMask(this Axis axis)
    {
      switch (axis)
      {
        case Axis.X:
          return Vector3.UnitX;
        case Axis.Y:
          return Vector3.UnitY;
        default:
          return Vector3.UnitZ;
      }
    }

    public static Vector3 GetMask(this Vector3 vector)
    {
      return FezMath.Abs(FezMath.Sign(vector));
    }

    public static Axis VisibleAxis(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
        case Viewpoint.Back:
          return Axis.Z;
        case Viewpoint.Down:
          return Axis.Y;
        default:
          return Axis.X;
      }
    }

    public static float WrapAngle(float theta)
    {
      theta = (double) theta < -3.14159274101257 ? (float) (((double) theta - 3.14159274101257) % 6.28318548202515 + 3.14159274101257) : (float) (((double) theta + 3.14159274101257) % 6.28318548202515 - 3.14159274101257);
      return theta;
    }

    public static FaceOrientation OrientationFromDirection(Vector3 direction)
    {
      if (direction == Vector3.Forward)
        return FaceOrientation.Back;
      if (direction == Vector3.Backward)
        return FaceOrientation.Front;
      if (direction == Vector3.Up)
        return FaceOrientation.Top;
      if (direction == Vector3.Down)
        return FaceOrientation.Down;
      return direction == Vector3.Left ? FaceOrientation.Left : FaceOrientation.Right;
    }

    public static Vector3 AsVector(this FaceOrientation o)
    {
      switch (o)
      {
        case FaceOrientation.Left:
          return Vector3.Left;
        case FaceOrientation.Down:
          return Vector3.Down;
        case FaceOrientation.Back:
          return Vector3.Forward;
        case FaceOrientation.Top:
          return Vector3.Up;
        case FaceOrientation.Front:
          return Vector3.Backward;
        default:
          return Vector3.Right;
      }
    }

    public static FaceOrientation GetTangent(this FaceOrientation o)
    {
      switch (o)
      {
        case FaceOrientation.Left:
          return FaceOrientation.Front;
        case FaceOrientation.Down:
          return FaceOrientation.Right;
        case FaceOrientation.Back:
          return FaceOrientation.Top;
        case FaceOrientation.Top:
          return FaceOrientation.Right;
        case FaceOrientation.Front:
          return FaceOrientation.Top;
        default:
          return FaceOrientation.Front;
      }
    }

    public static FaceOrientation GetBitangent(this FaceOrientation o)
    {
      switch (o)
      {
        case FaceOrientation.Left:
          return FaceOrientation.Top;
        case FaceOrientation.Down:
          return FaceOrientation.Front;
        case FaceOrientation.Back:
          return FaceOrientation.Right;
        case FaceOrientation.Top:
          return FaceOrientation.Front;
        case FaceOrientation.Front:
          return FaceOrientation.Right;
        default:
          return FaceOrientation.Top;
      }
    }

    public static FaceOrientation GetOpposite(this FaceOrientation o)
    {
      return (FaceOrientation) ((int) (o + 3) % 6);
    }

    public static bool IsPositive(this Viewpoint view)
    {
      return view <= Viewpoint.Right;
    }

    public static int AsNumeric(this bool b)
    {
      return !b ? 0 : 1;
    }

    public static Vector3 Abs(this Vector3 vector)
    {
      return new Vector3(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z));
    }

    public static Vector2 Abs(this Vector2 vector)
    {
      return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
    }

    public static Vector3 Clamp(Vector3 vector, Vector3 minimum, Vector3 maximum)
    {
      return new Vector3()
      {
        X = MathHelper.Clamp(vector.X, minimum.X, maximum.X),
        Y = MathHelper.Clamp(vector.Y, minimum.Y, maximum.Y),
        Z = MathHelper.Clamp(vector.Z, minimum.Z, maximum.Z)
      };
    }

    public static Axis AxisFromPhi(float combinedPhi)
    {
      return (double) combinedPhi > 0.785398185253143 && (double) combinedPhi < 2.35619449615479 || (double) combinedPhi < -0.785398185253143 && (double) combinedPhi > -2.35619449615479 ? Axis.X : Axis.Z;
    }

    public static Viewpoint GetOpposite(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
          return Viewpoint.Back;
        case Viewpoint.Right:
          return Viewpoint.Left;
        case Viewpoint.Back:
          return Viewpoint.Front;
        case Viewpoint.Left:
          return Viewpoint.Right;
        default:
          throw new InvalidOperationException("Orthographic views only");
      }
    }

    public static HorizontalDirection DirectionFromMovement(float xMovement)
    {
      if ((double) xMovement > 0.0)
        return HorizontalDirection.Right;
      return (double) xMovement < 0.0 ? HorizontalDirection.Left : HorizontalDirection.None;
    }

    public static HorizontalDirection GetOpposite(this HorizontalDirection direction)
    {
      return direction != HorizontalDirection.Left ? HorizontalDirection.Left : HorizontalDirection.Right;
    }

    public static bool IsPositive(this FaceOrientation orientation)
    {
      return orientation > FaceOrientation.Back;
    }

    public static Quaternion QuaternionFromPhi(float phi)
    {
      double num = (double) phi / 2.0;
      return new Quaternion(0.0f, (float) Math.Sin(num), 0.0f, (float) Math.Cos(num));
    }

    public static Vector3 Sign(this Vector3 vector)
    {
      return new Vector3((float) Math.Sign(vector.X), (float) Math.Sign(vector.Y), (float) Math.Sign(vector.Z));
    }

    public static Vector3 XYZ(this Vector4 vector)
    {
      return new Vector3(vector.X, vector.Y, vector.Z);
    }

    public static Vector2 XY(this Vector3 vector)
    {
      return new Vector2(vector.X, vector.Y);
    }

    public static Vector2 YZ(this Vector3 vector)
    {
      return new Vector2(vector.Y, vector.Z);
    }

    public static Vector2 XZ(this Vector3 vector)
    {
      return new Vector2(vector.X, vector.Z);
    }

    public static Vector3 ZYX(this Vector3 vector)
    {
      return new Vector3(vector.Z, vector.Y, vector.X);
    }

    public static Vector2 ZY(this Vector3 vector)
    {
      return new Vector2(vector.Z, vector.Y);
    }

    public static Vector3 X0Y(this Vector2 vector)
    {
      return new Vector3(vector.X, 0.0f, vector.Y);
    }

    public static Vector3 XYX(this Vector2 vector)
    {
      return new Vector3(vector.X, vector.Y, vector.X);
    }

    public static int Sign(this HorizontalDirection direction)
    {
      return direction != HorizontalDirection.Right ? -1 : 1;
    }

    public static float SnapPhi(float phi)
    {
      return (float) FezMath.Round(0.636619754652023 * (double) phi) / 0.6366197f;
    }

    public static float Frac(float number)
    {
      return number - (float) (int) number;
    }

    public static double Frac(double number)
    {
      return number - (double) (int) number;
    }

    public static Vector3 Frac(this Vector3 vector)
    {
      return new Vector3(vector.X - (float) (int) vector.X, vector.Y - (float) (int) vector.Y, vector.Z - (float) (int) vector.Z);
    }

    public static int GetDistance(this Viewpoint fromView, Viewpoint toView)
    {
      int num = toView - fromView;
      if (Math.Abs(num) == 3)
        num = Math.Sign(num) * -1;
      return num;
    }

    public static Viewpoint GetRotatedView(this Viewpoint fromView, int distance)
    {
      int num = (int) (fromView + distance);
      while (num > 4)
        num -= 4;
      while (num < 1)
        num += 4;
      return (Viewpoint) num;
    }

    public static bool IsOrthographic(this Viewpoint view)
    {
      switch (view)
      {
        case Viewpoint.Front:
        case Viewpoint.Right:
        case Viewpoint.Back:
        case Viewpoint.Left:
          return true;
        default:
          return false;
      }
    }

    public static BoundingBox Enclose(Vector3 a, Vector3 b)
    {
      return new BoundingBox()
      {
        Min = new Vector3(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z)),
        Max = new Vector3(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z))
      };
    }

    public static float Dot(this Vector3 a, Vector3 b)
    {
      return Vector3.Dot(a, b);
    }

    public static Matrix CreateLookAt(Vector3 position, Vector3 lookAt, Vector3 upVector)
    {
      Vector3 vector3_1 = Vector3.Normalize(lookAt - position);
      Vector3 vector2 = Vector3.Normalize(Vector3.Cross(upVector, vector3_1));
      Vector3 vector3_2 = Vector3.Cross(vector3_1, vector2);
      return new Matrix(vector2.X, vector3_2.X, vector3_1.X, 0.0f, vector2.Y, vector3_2.Y, vector3_1.Y, 0.0f, vector2.Z, vector3_2.Z, vector3_1.Z, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
    }

    public static float SineTransition(float step)
    {
      return (float) Math.Sin((double) FezMath.Saturate(step) * Math.PI);
    }

    public static void RotateOnCenter(ref BoundingBox boundingBox, ref Quaternion quaternion)
    {
      Vector3 vector3_1 = (boundingBox.Max - boundingBox.Min) / 2f;
      Vector3 vector3_2 = boundingBox.Min + vector3_1;
      boundingBox.Min = -vector3_1;
      boundingBox.Max = vector3_1;
      boundingBox.GetCorners(FezMath.tempCorners);
      boundingBox.Min = new Vector3(float.MaxValue);
      boundingBox.Max = new Vector3(float.MinValue);
      for (int index = 0; index < 8; ++index)
      {
        Vector3 result;
        Vector3.Transform(ref FezMath.tempCorners[index], ref quaternion, out result);
        result += vector3_2;
        Vector3.Min(ref boundingBox.Min, ref result, out boundingBox.Min);
        Vector3.Max(ref boundingBox.Max, ref result, out boundingBox.Max);
      }
    }

    public static Vector3 GetCenter(this BoundingBox boundingBox)
    {
      return boundingBox.Min + (boundingBox.Max - boundingBox.Min) / 2f;
    }

    public static Quaternion CatmullRom(Quaternion q0, Quaternion q1, Quaternion q2, Quaternion q3, float t)
    {
      Vector4 vector4 = Vector4.CatmullRom(new Vector4(q0.X, q0.Y, q0.Z, q0.W), new Vector4(q1.X, q1.Y, q1.Z, q1.W), new Vector4(q2.X, q2.Y, q2.Z, q2.W), new Vector4(q3.X, q3.Y, q3.Z, q3.W), t);
      return Quaternion.Normalize(new Quaternion(vector4.X, vector4.Y, vector4.Z, vector4.W));
    }

    public static Vector2 TransformTexCoord(Vector2 texCoord, Matrix transform)
    {
      return new Vector2((float) ((double) texCoord.X * (double) transform.M11 + (double) texCoord.Y * (double) transform.M21) + transform.M31, (float) ((double) texCoord.X * (double) transform.M12 + (double) texCoord.Y * (double) transform.M22) + transform.M32);
    }
  }
}
