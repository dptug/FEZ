// Type: SharpDX.BoundingSphere
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
  public struct BoundingSphere : IEquatable<BoundingSphere>, IFormattable, IDataSerializable
  {
    public Vector3 Center;
    public float Radius;

    public BoundingSphere(Vector3 center, float radius)
    {
      this.Center = center;
      this.Radius = radius;
    }

    public static bool operator ==(BoundingSphere left, BoundingSphere right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(BoundingSphere left, BoundingSphere right)
    {
      return !left.Equals(right);
    }

    public bool Intersects(ref Ray ray)
    {
      float distance;
      return Collision.RayIntersectsSphere(ref ray, ref this, out distance);
    }

    public bool Intersects(ref Ray ray, out float distance)
    {
      return Collision.RayIntersectsSphere(ref ray, ref this, out distance);
    }

    public bool Intersects(ref Ray ray, out Vector3 point)
    {
      return Collision.RayIntersectsSphere(ref ray, ref this, out point);
    }

    public PlaneIntersectionType Intersects(ref Plane plane)
    {
      return Collision.PlaneIntersectsSphere(ref plane, ref this);
    }

    public bool Intersects(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
    {
      return Collision.SphereIntersectsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3);
    }

    public bool Intersects(ref BoundingBox box)
    {
      return Collision.BoxIntersectsSphere(ref box, ref this);
    }

    public bool Intersects(ref BoundingSphere sphere)
    {
      return Collision.SphereIntersectsSphere(ref this, ref sphere);
    }

    public ContainmentType Contains(ref Vector3 point)
    {
      return Collision.SphereContainsPoint(ref this, ref point);
    }

    public ContainmentType Contains(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
    {
      return Collision.SphereContainsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3);
    }

    public ContainmentType Contains(ref BoundingBox box)
    {
      return Collision.SphereContainsBox(ref this, ref box);
    }

    public ContainmentType Contains(ref BoundingSphere sphere)
    {
      return Collision.SphereContainsSphere(ref this, ref sphere);
    }

    public static void FromPoints(Vector3[] points, out BoundingSphere result)
    {
      Vector3 result1 = Vector3.Zero;
      for (int index = 0; index < points.Length; ++index)
        Vector3.Add(ref points[index], ref result1, out result1);
      result1 /= (float) points.Length;
      float num1 = 0.0f;
      for (int index = 0; index < points.Length; ++index)
      {
        float result2;
        Vector3.DistanceSquared(ref result1, ref points[index], out result2);
        if ((double) result2 > (double) num1)
          num1 = result2;
      }
      float num2 = (float) Math.Sqrt((double) num1);
      result.Center = result1;
      result.Radius = num2;
    }

    public static BoundingSphere FromPoints(Vector3[] points)
    {
      BoundingSphere result;
      BoundingSphere.FromPoints(points, out result);
      return result;
    }

    public static void FromBox(ref BoundingBox box, out BoundingSphere result)
    {
      Vector3.Lerp(ref box.Minimum, ref box.Maximum, 0.5f, out result.Center);
      float num1 = box.Minimum.X - box.Maximum.X;
      float num2 = box.Minimum.Y - box.Maximum.Y;
      float num3 = box.Minimum.Z - box.Maximum.Z;
      float num4 = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
      result.Radius = num4 * 0.5f;
    }

    public static BoundingSphere FromBox(BoundingBox box)
    {
      BoundingSphere result;
      BoundingSphere.FromBox(ref box, out result);
      return result;
    }

    public static void Merge(ref BoundingSphere value1, ref BoundingSphere value2, out BoundingSphere result)
    {
      Vector3 vector3_1 = value2.Center - value1.Center;
      float num1 = vector3_1.Length();
      float val1 = value1.Radius;
      float num2 = value2.Radius;
      if ((double) val1 + (double) num2 >= (double) num1)
      {
        if ((double) val1 - (double) num2 >= (double) num1)
        {
          result = value1;
          return;
        }
        else if ((double) num2 - (double) val1 >= (double) num1)
        {
          result = value2;
          return;
        }
      }
      Vector3 vector3_2 = vector3_1 * (1f / num1);
      float num3 = Math.Min(-val1, num1 - num2);
      float num4 = (float) (((double) Math.Max(val1, num1 + num2) - (double) num3) * 0.5);
      result.Center = value1.Center + vector3_2 * (num4 + num3);
      result.Radius = num4;
    }

    public static BoundingSphere Merge(BoundingSphere value1, BoundingSphere value2)
    {
      BoundingSphere result;
      BoundingSphere.Merge(ref value1, ref value2, out result);
      return result;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Center:{0} Radius:{1}", new object[2]
      {
        (object) this.Center.ToString(),
        (object) this.Radius.ToString()
      });
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Center:{0} Radius:{1}", new object[2]
      {
        (object) this.Center.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture),
        (object) this.Radius.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture)
      });
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "Center:{0} Radius:{1}", new object[2]
      {
        (object) this.Center.ToString(),
        (object) this.Radius.ToString()
      });
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        return this.ToString(formatProvider);
      return string.Format(formatProvider, "Center:{0} Radius:{1}", new object[2]
      {
        (object) this.Center.ToString(format, formatProvider),
        (object) this.Radius.ToString(format, formatProvider)
      });
    }

    public override int GetHashCode()
    {
      return this.Center.GetHashCode() + this.Radius.GetHashCode();
    }

    public bool Equals(BoundingSphere value)
    {
      if (this.Center == value.Center)
        return (double) this.Radius == (double) value.Radius;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (BoundingSphere)))
        return false;
      else
        return this.Equals((BoundingSphere) value);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      serializer.Serialize<Vector3>(ref this.Center, SerializeFlags.Normal);
      serializer.Serialize(ref this.Radius);
    }
  }
}
