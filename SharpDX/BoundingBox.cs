// Type: SharpDX.BoundingBox
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
  public struct BoundingBox : IEquatable<BoundingBox>, IFormattable, IDataSerializable
  {
    public Vector3 Minimum;
    public Vector3 Maximum;

    public BoundingBox(Vector3 minimum, Vector3 maximum)
    {
      this.Minimum = minimum;
      this.Maximum = maximum;
    }

    public static bool operator ==(BoundingBox left, BoundingBox right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(BoundingBox left, BoundingBox right)
    {
      return !left.Equals(right);
    }

    public Vector3[] GetCorners()
    {
      return new Vector3[8]
      {
        new Vector3(this.Minimum.X, this.Maximum.Y, this.Maximum.Z),
        new Vector3(this.Maximum.X, this.Maximum.Y, this.Maximum.Z),
        new Vector3(this.Maximum.X, this.Minimum.Y, this.Maximum.Z),
        new Vector3(this.Minimum.X, this.Minimum.Y, this.Maximum.Z),
        new Vector3(this.Minimum.X, this.Maximum.Y, this.Minimum.Z),
        new Vector3(this.Maximum.X, this.Maximum.Y, this.Minimum.Z),
        new Vector3(this.Maximum.X, this.Minimum.Y, this.Minimum.Z),
        new Vector3(this.Minimum.X, this.Minimum.Y, this.Minimum.Z)
      };
    }

    public bool Intersects(ref Ray ray)
    {
      float distance;
      return Collision.RayIntersectsBox(ref ray, ref this, out distance);
    }

    public bool Intersects(ref Ray ray, out float distance)
    {
      return Collision.RayIntersectsBox(ref ray, ref this, out distance);
    }

    public bool Intersects(ref Ray ray, out Vector3 point)
    {
      return Collision.RayIntersectsBox(ref ray, ref this, out point);
    }

    public PlaneIntersectionType Intersects(ref Plane plane)
    {
      return Collision.PlaneIntersectsBox(ref plane, ref this);
    }

    public bool Intersects(ref BoundingBox box)
    {
      return Collision.BoxIntersectsBox(ref this, ref box);
    }

    public bool Intersects(ref BoundingSphere sphere)
    {
      return Collision.BoxIntersectsSphere(ref this, ref sphere);
    }

    public ContainmentType Contains(ref Vector3 point)
    {
      return Collision.BoxContainsPoint(ref this, ref point);
    }

    public ContainmentType Contains(ref BoundingBox box)
    {
      return Collision.BoxContainsBox(ref this, ref box);
    }

    public ContainmentType Contains(ref BoundingSphere sphere)
    {
      return Collision.BoxContainsSphere(ref this, ref sphere);
    }

    public static void FromPoints(Vector3[] points, out BoundingBox result)
    {
      if (points == null)
        throw new ArgumentNullException("points");
      Vector3 result1 = new Vector3(float.MaxValue);
      Vector3 result2 = new Vector3(float.MinValue);
      for (int index = 0; index < points.Length; ++index)
      {
        Vector3.Min(ref result1, ref points[index], out result1);
        Vector3.Max(ref result2, ref points[index], out result2);
      }
      result = new BoundingBox(result1, result2);
    }

    public static BoundingBox FromPoints(Vector3[] points)
    {
      if (points == null)
        throw new ArgumentNullException("points");
      Vector3 result1 = new Vector3(float.MaxValue);
      Vector3 result2 = new Vector3(float.MinValue);
      for (int index = 0; index < points.Length; ++index)
      {
        Vector3.Min(ref result1, ref points[index], out result1);
        Vector3.Max(ref result2, ref points[index], out result2);
      }
      return new BoundingBox(result1, result2);
    }

    public static void FromSphere(ref BoundingSphere sphere, out BoundingBox result)
    {
      result.Minimum = new Vector3(sphere.Center.X - sphere.Radius, sphere.Center.Y - sphere.Radius, sphere.Center.Z - sphere.Radius);
      result.Maximum = new Vector3(sphere.Center.X + sphere.Radius, sphere.Center.Y + sphere.Radius, sphere.Center.Z + sphere.Radius);
    }

    public static BoundingBox FromSphere(BoundingSphere sphere)
    {
      BoundingBox boundingBox;
      boundingBox.Minimum = new Vector3(sphere.Center.X - sphere.Radius, sphere.Center.Y - sphere.Radius, sphere.Center.Z - sphere.Radius);
      boundingBox.Maximum = new Vector3(sphere.Center.X + sphere.Radius, sphere.Center.Y + sphere.Radius, sphere.Center.Z + sphere.Radius);
      return boundingBox;
    }

    public static void Merge(ref BoundingBox value1, ref BoundingBox value2, out BoundingBox result)
    {
      Vector3.Min(ref value1.Minimum, ref value2.Minimum, out result.Minimum);
      Vector3.Max(ref value1.Maximum, ref value2.Maximum, out result.Maximum);
    }

    public static BoundingBox Merge(BoundingBox value1, BoundingBox value2)
    {
      BoundingBox boundingBox;
      Vector3.Min(ref value1.Minimum, ref value2.Minimum, out boundingBox.Minimum);
      Vector3.Max(ref value1.Maximum, ref value2.Maximum, out boundingBox.Maximum);
      return boundingBox;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Minimum:{0} Maximum:{1}", new object[2]
      {
        (object) this.Minimum.ToString(),
        (object) this.Maximum.ToString()
      });
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Minimum:{0} Maximum:{1}", new object[2]
      {
        (object) this.Minimum.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture),
        (object) this.Maximum.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture)
      });
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "Minimum:{0} Maximum:{1}", new object[2]
      {
        (object) this.Minimum.ToString(),
        (object) this.Maximum.ToString()
      });
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        return this.ToString(formatProvider);
      return string.Format(formatProvider, "Minimum:{0} Maximum:{1}", new object[2]
      {
        (object) this.Minimum.ToString(format, formatProvider),
        (object) this.Maximum.ToString(format, formatProvider)
      });
    }

    public override int GetHashCode()
    {
      return this.Minimum.GetHashCode() + this.Maximum.GetHashCode();
    }

    public bool Equals(BoundingBox value)
    {
      if (this.Minimum == value.Minimum)
        return this.Maximum == value.Maximum;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (BoundingBox)))
        return false;
      else
        return this.Equals((BoundingBox) value);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      serializer.Serialize<Vector3>(ref this.Minimum, SerializeFlags.Normal);
      serializer.Serialize<Vector3>(ref this.Maximum, SerializeFlags.Normal);
    }
  }
}
