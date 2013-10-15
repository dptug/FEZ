// Type: SharpDX.Plane
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Plane : IEquatable<Plane>, IFormattable, IDataSerializable
  {
    public Vector3 Normal;
    public float D;

    public float this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.Normal.X;
          case 1:
            return this.Normal.Y;
          case 2:
            return this.Normal.Z;
          case 3:
            return this.D;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Plane run from 0 to 3, inclusive.");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.Normal.X = value;
            break;
          case 1:
            this.Normal.Y = value;
            break;
          case 2:
            this.Normal.Z = value;
            break;
          case 3:
            this.D = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Plane run from 0 to 3, inclusive.");
        }
      }
    }

    public Plane(float value)
    {
      this.Normal.X = this.Normal.Y = this.Normal.Z = this.D = value;
    }

    public Plane(float a, float b, float c, float d)
    {
      this.Normal.X = a;
      this.Normal.Y = b;
      this.Normal.Z = c;
      this.D = d;
    }

    public Plane(Vector3 point, Vector3 normal)
    {
      this.Normal = normal;
      this.D = -Vector3.Dot(normal, point);
    }

    public Plane(Vector3 value, float d)
    {
      this.Normal = value;
      this.D = d;
    }

    public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
    {
      float num1 = point2.X - point1.X;
      float num2 = point2.Y - point1.Y;
      float num3 = point2.Z - point1.Z;
      float num4 = point3.X - point1.X;
      float num5 = point3.Y - point1.Y;
      float num6 = point3.Z - point1.Z;
      float num7 = (float) ((double) num2 * (double) num6 - (double) num3 * (double) num5);
      float num8 = (float) ((double) num3 * (double) num4 - (double) num1 * (double) num6);
      float num9 = (float) ((double) num1 * (double) num5 - (double) num2 * (double) num4);
      float num10 = 1f / (float) Math.Sqrt((double) num7 * (double) num7 + (double) num8 * (double) num8 + (double) num9 * (double) num9);
      this.Normal.X = num7 * num10;
      this.Normal.Y = num8 * num10;
      this.Normal.Z = num9 * num10;
      this.D = (float) -((double) this.Normal.X * (double) point1.X + (double) this.Normal.Y * (double) point1.Y + (double) this.Normal.Z * (double) point1.Z);
    }

    public Plane(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Plane.");
      this.Normal.X = values[0];
      this.Normal.Y = values[1];
      this.Normal.Z = values[2];
      this.D = values[3];
    }

    public static Plane operator *(float scale, Plane plane)
    {
      return new Plane(plane.Normal.X * scale, plane.Normal.Y * scale, plane.Normal.Z * scale, plane.D * scale);
    }

    public static Plane operator *(Plane plane, float scale)
    {
      return new Plane(plane.Normal.X * scale, plane.Normal.Y * scale, plane.Normal.Z * scale, plane.D * scale);
    }

    public static bool operator ==(Plane left, Plane right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Plane left, Plane right)
    {
      return !left.Equals(right);
    }

    public void Normalize()
    {
      float num = 1f / (float) Math.Sqrt((double) this.Normal.X * (double) this.Normal.X + (double) this.Normal.Y * (double) this.Normal.Y + (double) this.Normal.Z * (double) this.Normal.Z);
      this.Normal.X *= num;
      this.Normal.Y *= num;
      this.Normal.Z *= num;
      this.D *= num;
    }

    public float[] ToArray()
    {
      return new float[4]
      {
        this.Normal.X,
        this.Normal.Y,
        this.Normal.Z,
        this.D
      };
    }

    public PlaneIntersectionType Intersects(ref Vector3 point)
    {
      return Collision.PlaneIntersectsPoint(ref this, ref point);
    }

    public bool Intersects(ref Ray ray)
    {
      float distance;
      return Collision.RayIntersectsPlane(ref ray, ref this, out distance);
    }

    public bool Intersects(ref Ray ray, out float distance)
    {
      return Collision.RayIntersectsPlane(ref ray, ref this, out distance);
    }

    public bool Intersects(ref Ray ray, out Vector3 point)
    {
      return Collision.RayIntersectsPlane(ref ray, ref this, out point);
    }

    public bool Intersects(ref Plane plane)
    {
      return Collision.PlaneIntersectsPlane(ref this, ref plane);
    }

    public bool Intersects(ref Plane plane, out Ray line)
    {
      return Collision.PlaneIntersectsPlane(ref this, ref plane, out line);
    }

    public PlaneIntersectionType Intersects(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
    {
      return Collision.PlaneIntersectsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3);
    }

    public PlaneIntersectionType Intersects(ref BoundingBox box)
    {
      return Collision.PlaneIntersectsBox(ref this, ref box);
    }

    public PlaneIntersectionType Intersects(ref BoundingSphere sphere)
    {
      return Collision.PlaneIntersectsSphere(ref this, ref sphere);
    }

    public static void Multiply(ref Plane value, float scale, out Plane result)
    {
      result.Normal.X = value.Normal.X * scale;
      result.Normal.Y = value.Normal.Y * scale;
      result.Normal.Z = value.Normal.Z * scale;
      result.D = value.D * scale;
    }

    public static Plane Multiply(Plane value, float scale)
    {
      return new Plane(value.Normal.X * scale, value.Normal.Y * scale, value.Normal.Z * scale, value.D * scale);
    }

    public static void Dot(ref Plane left, ref Vector4 right, out float result)
    {
      result = (float) ((double) left.Normal.X * (double) right.X + (double) left.Normal.Y * (double) right.Y + (double) left.Normal.Z * (double) right.Z + (double) left.D * (double) right.W);
    }

    public static float Dot(Plane left, Vector4 right)
    {
      return (float) ((double) left.Normal.X * (double) right.X + (double) left.Normal.Y * (double) right.Y + (double) left.Normal.Z * (double) right.Z + (double) left.D * (double) right.W);
    }

    public static void DotCoordinate(ref Plane left, ref Vector3 right, out float result)
    {
      result = (float) ((double) left.Normal.X * (double) right.X + (double) left.Normal.Y * (double) right.Y + (double) left.Normal.Z * (double) right.Z) + left.D;
    }

    public static float DotCoordinate(Plane left, Vector3 right)
    {
      return (float) ((double) left.Normal.X * (double) right.X + (double) left.Normal.Y * (double) right.Y + (double) left.Normal.Z * (double) right.Z) + left.D;
    }

    public static void DotNormal(ref Plane left, ref Vector3 right, out float result)
    {
      result = (float) ((double) left.Normal.X * (double) right.X + (double) left.Normal.Y * (double) right.Y + (double) left.Normal.Z * (double) right.Z);
    }

    public static float DotNormal(Plane left, Vector3 right)
    {
      return (float) ((double) left.Normal.X * (double) right.X + (double) left.Normal.Y * (double) right.Y + (double) left.Normal.Z * (double) right.Z);
    }

    public static void Normalize(ref Plane plane, out Plane result)
    {
      float num = 1f / (float) Math.Sqrt((double) plane.Normal.X * (double) plane.Normal.X + (double) plane.Normal.Y * (double) plane.Normal.Y + (double) plane.Normal.Z * (double) plane.Normal.Z);
      result.Normal.X = plane.Normal.X * num;
      result.Normal.Y = plane.Normal.Y * num;
      result.Normal.Z = plane.Normal.Z * num;
      result.D = plane.D * num;
    }

    public static Plane Normalize(Plane plane)
    {
      float num = 1f / (float) Math.Sqrt((double) plane.Normal.X * (double) plane.Normal.X + (double) plane.Normal.Y * (double) plane.Normal.Y + (double) plane.Normal.Z * (double) plane.Normal.Z);
      return new Plane(plane.Normal.X * num, plane.Normal.Y * num, plane.Normal.Z * num, plane.D * num);
    }

    public static void Transform(ref Plane plane, ref Quaternion rotation, out Plane result)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num1;
      float num5 = rotation.W * num2;
      float num6 = rotation.W * num3;
      float num7 = rotation.X * num1;
      float num8 = rotation.X * num2;
      float num9 = rotation.X * num3;
      float num10 = rotation.Y * num2;
      float num11 = rotation.Y * num3;
      float num12 = rotation.Z * num3;
      float num13 = plane.Normal.X;
      float num14 = plane.Normal.Y;
      float num15 = plane.Normal.Z;
      result.Normal.X = (float) ((double) num13 * (1.0 - (double) num10 - (double) num12) + (double) num14 * ((double) num8 - (double) num6) + (double) num15 * ((double) num9 + (double) num5));
      result.Normal.Y = (float) ((double) num13 * ((double) num8 + (double) num6) + (double) num14 * (1.0 - (double) num7 - (double) num12) + (double) num15 * ((double) num11 - (double) num4));
      result.Normal.Z = (float) ((double) num13 * ((double) num9 - (double) num5) + (double) num14 * ((double) num11 + (double) num4) + (double) num15 * (1.0 - (double) num7 - (double) num10));
      result.D = plane.D;
    }

    public static Plane Transform(Plane plane, Quaternion rotation)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num1;
      float num5 = rotation.W * num2;
      float num6 = rotation.W * num3;
      float num7 = rotation.X * num1;
      float num8 = rotation.X * num2;
      float num9 = rotation.X * num3;
      float num10 = rotation.Y * num2;
      float num11 = rotation.Y * num3;
      float num12 = rotation.Z * num3;
      float num13 = plane.Normal.X;
      float num14 = plane.Normal.Y;
      float num15 = plane.Normal.Z;
      Plane plane1;
      plane1.Normal.X = (float) ((double) num13 * (1.0 - (double) num10 - (double) num12) + (double) num14 * ((double) num8 - (double) num6) + (double) num15 * ((double) num9 + (double) num5));
      plane1.Normal.Y = (float) ((double) num13 * ((double) num8 + (double) num6) + (double) num14 * (1.0 - (double) num7 - (double) num12) + (double) num15 * ((double) num11 - (double) num4));
      plane1.Normal.Z = (float) ((double) num13 * ((double) num9 - (double) num5) + (double) num14 * ((double) num11 + (double) num4) + (double) num15 * (1.0 - (double) num7 - (double) num10));
      plane1.D = plane.D;
      return plane1;
    }

    public static void Transform(Plane[] planes, ref Quaternion rotation)
    {
      if (planes == null)
        throw new ArgumentNullException("planes");
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num1;
      float num5 = rotation.W * num2;
      float num6 = rotation.W * num3;
      float num7 = rotation.X * num1;
      float num8 = rotation.X * num2;
      float num9 = rotation.X * num3;
      float num10 = rotation.Y * num2;
      float num11 = rotation.Y * num3;
      float num12 = rotation.Z * num3;
      for (int index = 0; index < planes.Length; ++index)
      {
        float num13 = planes[index].Normal.X;
        float num14 = planes[index].Normal.Y;
        float num15 = planes[index].Normal.Z;
        planes[index].Normal.X = (float) ((double) num13 * (1.0 - (double) num10 - (double) num12) + (double) num14 * ((double) num8 - (double) num6) + (double) num15 * ((double) num9 + (double) num5));
        planes[index].Normal.Y = (float) ((double) num13 * ((double) num8 + (double) num6) + (double) num14 * (1.0 - (double) num7 - (double) num12) + (double) num15 * ((double) num11 - (double) num4));
        planes[index].Normal.Z = (float) ((double) num13 * ((double) num9 - (double) num5) + (double) num14 * ((double) num11 + (double) num4) + (double) num15 * (1.0 - (double) num7 - (double) num10));
      }
    }

    public static void Transform(ref Plane plane, ref Matrix transformation, out Plane result)
    {
      float num1 = plane.Normal.X;
      float num2 = plane.Normal.Y;
      float num3 = plane.Normal.Z;
      float num4 = plane.D;
      Matrix result1;
      Matrix.Invert(ref transformation, out result1);
      result.Normal.X = (float) ((double) num1 * (double) result1.M11 + (double) num2 * (double) result1.M12 + (double) num3 * (double) result1.M13 + (double) num4 * (double) result1.M14);
      result.Normal.Y = (float) ((double) num1 * (double) result1.M21 + (double) num2 * (double) result1.M22 + (double) num3 * (double) result1.M23 + (double) num4 * (double) result1.M24);
      result.Normal.Z = (float) ((double) num1 * (double) result1.M31 + (double) num2 * (double) result1.M32 + (double) num3 * (double) result1.M33 + (double) num4 * (double) result1.M34);
      result.D = (float) ((double) num1 * (double) result1.M41 + (double) num2 * (double) result1.M42 + (double) num3 * (double) result1.M43 + (double) num4 * (double) result1.M44);
    }

    public static Plane Transform(Plane plane, Matrix transformation)
    {
      float num1 = plane.Normal.X;
      float num2 = plane.Normal.Y;
      float num3 = plane.Normal.Z;
      float num4 = plane.D;
      transformation.Invert();
      Plane plane1;
      plane1.Normal.X = (float) ((double) num1 * (double) transformation.M11 + (double) num2 * (double) transformation.M12 + (double) num3 * (double) transformation.M13 + (double) num4 * (double) transformation.M14);
      plane1.Normal.Y = (float) ((double) num1 * (double) transformation.M21 + (double) num2 * (double) transformation.M22 + (double) num3 * (double) transformation.M23 + (double) num4 * (double) transformation.M24);
      plane1.Normal.Z = (float) ((double) num1 * (double) transformation.M31 + (double) num2 * (double) transformation.M32 + (double) num3 * (double) transformation.M33 + (double) num4 * (double) transformation.M34);
      plane1.D = (float) ((double) num1 * (double) transformation.M41 + (double) num2 * (double) transformation.M42 + (double) num3 * (double) transformation.M43 + (double) num4 * (double) transformation.M44);
      return plane1;
    }

    public static void Transform(Plane[] planes, ref Matrix transformation)
    {
      if (planes == null)
        throw new ArgumentNullException("planes");
      Matrix result;
      Matrix.Invert(ref transformation, out result);
      for (int index = 0; index < planes.Length; ++index)
        Plane.Transform(ref planes[index], ref transformation, out planes[index]);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "A:{0} B:{1} C:{2} D:{3}", (object) this.Normal.X, (object) this.Normal.Y, (object) this.Normal.Z, (object) this.D);
    }

    public string ToString(string format)
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "A:{0} B:{1} C:{2} D:{3}", (object) this.Normal.X.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Normal.Y.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Normal.Z.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.D.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture));
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "A:{0} B:{1} C:{2} D:{3}", (object) this.Normal.X, (object) this.Normal.Y, (object) this.Normal.Z, (object) this.D);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "A:{0} B:{1} C:{2} D:{3}", (object) this.Normal.X.ToString(format, formatProvider), (object) this.Normal.Y.ToString(format, formatProvider), (object) this.Normal.Z.ToString(format, formatProvider), (object) this.D.ToString(format, formatProvider));
    }

    public override int GetHashCode()
    {
      return this.Normal.GetHashCode() + this.D.GetHashCode();
    }

    public bool Equals(Plane value)
    {
      if (this.Normal == value.Normal)
        return (double) this.D == (double) value.D;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Plane)))
        return false;
      else
        return this.Equals((Plane) value);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      serializer.Serialize<Vector3>(ref this.Normal, SerializeFlags.Normal);
      serializer.Serialize(ref this.D);
    }
  }
}
