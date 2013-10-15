// Type: Microsoft.Xna.Framework.BoundingSphere
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Xna.Framework
{
  public struct BoundingSphere : IEquatable<BoundingSphere>
  {
    public Vector3 Center;
    public float Radius;

    public BoundingSphere(Vector3 center, float radius)
    {
      this.Center = center;
      this.Radius = radius;
    }

    public static bool operator ==(BoundingSphere a, BoundingSphere b)
    {
      return a.Equals(b);
    }

    public static bool operator !=(BoundingSphere a, BoundingSphere b)
    {
      return !a.Equals(b);
    }

    public BoundingSphere Transform(Matrix matrix)
    {
      return new BoundingSphere()
      {
        Center = Vector3.Transform(this.Center, matrix),
        Radius = this.Radius * (float) Math.Sqrt((double) Math.Max((float) ((double) matrix.M11 * (double) matrix.M11 + (double) matrix.M12 * (double) matrix.M12 + (double) matrix.M13 * (double) matrix.M13), Math.Max((float) ((double) matrix.M21 * (double) matrix.M21 + (double) matrix.M22 * (double) matrix.M22 + (double) matrix.M23 * (double) matrix.M23), (float) ((double) matrix.M31 * (double) matrix.M31 + (double) matrix.M32 * (double) matrix.M32 + (double) matrix.M33 * (double) matrix.M33))))
      };
    }

    public void Transform(ref Matrix matrix, out BoundingSphere result)
    {
      result.Center = Vector3.Transform(this.Center, matrix);
      result.Radius = this.Radius * (float) Math.Sqrt((double) Math.Max((float) ((double) matrix.M11 * (double) matrix.M11 + (double) matrix.M12 * (double) matrix.M12 + (double) matrix.M13 * (double) matrix.M13), Math.Max((float) ((double) matrix.M21 * (double) matrix.M21 + (double) matrix.M22 * (double) matrix.M22 + (double) matrix.M23 * (double) matrix.M23), (float) ((double) matrix.M31 * (double) matrix.M31 + (double) matrix.M32 * (double) matrix.M32 + (double) matrix.M33 * (double) matrix.M33))));
    }

    public ContainmentType Contains(BoundingBox box)
    {
      bool flag = true;
      foreach (Vector3 point in box.GetCorners())
      {
        if (this.Contains(point) == ContainmentType.Disjoint)
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return ContainmentType.Contains;
      double num = 0.0;
      if ((double) this.Center.X < (double) box.Min.X)
        num += ((double) this.Center.X - (double) box.Min.X) * ((double) this.Center.X - (double) box.Min.X);
      else if ((double) this.Center.X > (double) box.Max.X)
        num += ((double) this.Center.X - (double) box.Max.X) * ((double) this.Center.X - (double) box.Max.X);
      if ((double) this.Center.Y < (double) box.Min.Y)
        num += ((double) this.Center.Y - (double) box.Min.Y) * ((double) this.Center.Y - (double) box.Min.Y);
      else if ((double) this.Center.Y > (double) box.Max.Y)
        num += ((double) this.Center.Y - (double) box.Max.Y) * ((double) this.Center.Y - (double) box.Max.Y);
      if ((double) this.Center.Z < (double) box.Min.Z)
        num += ((double) this.Center.Z - (double) box.Min.Z) * ((double) this.Center.Z - (double) box.Min.Z);
      else if ((double) this.Center.Z > (double) box.Max.Z)
        num += ((double) this.Center.Z - (double) box.Max.Z) * ((double) this.Center.Z - (double) box.Max.Z);
      return num <= (double) this.Radius * (double) this.Radius ? ContainmentType.Intersects : ContainmentType.Disjoint;
    }

    public void Contains(ref BoundingBox box, out ContainmentType result)
    {
      result = this.Contains(box);
    }

    public ContainmentType Contains(BoundingFrustum frustum)
    {
      bool flag = true;
      foreach (Vector3 point in frustum.GetCorners())
      {
        if (this.Contains(point) == ContainmentType.Disjoint)
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return ContainmentType.Contains;
      return 0.0 <= (double) this.Radius * (double) this.Radius ? ContainmentType.Intersects : ContainmentType.Disjoint;
    }

    public ContainmentType Contains(BoundingSphere sphere)
    {
      float num = Vector3.Distance(sphere.Center, this.Center);
      if ((double) num > (double) sphere.Radius + (double) this.Radius)
        return ContainmentType.Disjoint;
      return (double) num <= (double) this.Radius - (double) sphere.Radius ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingSphere sphere, out ContainmentType result)
    {
      result = this.Contains(sphere);
    }

    public ContainmentType Contains(Vector3 point)
    {
      float num = Vector3.Distance(point, this.Center);
      if ((double) num > (double) this.Radius)
        return ContainmentType.Disjoint;
      return (double) num < (double) this.Radius ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref Vector3 point, out ContainmentType result)
    {
      result = this.Contains(point);
    }

    public static BoundingSphere CreateFromBoundingBox(BoundingBox box)
    {
      Vector3 vector3 = new Vector3((float) (((double) box.Min.X + (double) box.Max.X) / 2.0), (float) (((double) box.Min.Y + (double) box.Max.Y) / 2.0), (float) (((double) box.Min.Z + (double) box.Max.Z) / 2.0));
      float radius = Vector3.Distance(vector3, box.Max);
      return new BoundingSphere(vector3, radius);
    }

    public static void CreateFromBoundingBox(ref BoundingBox box, out BoundingSphere result)
    {
      result = BoundingSphere.CreateFromBoundingBox(box);
    }

    public static BoundingSphere CreateFromFrustum(BoundingFrustum frustum)
    {
      return BoundingSphere.CreateFromPoints((IEnumerable<Vector3>) frustum.GetCorners());
    }

    public static BoundingSphere CreateFromPoints(IEnumerable<Vector3> points)
    {
      if (points == null)
        throw new ArgumentNullException("points");
      float radius = 0.0f;
      Vector3 vector3_1 = new Vector3();
      int num1 = 0;
      foreach (Vector3 vector3_2 in points)
      {
        vector3_1 += vector3_2;
        ++num1;
      }
      Vector3 center = vector3_1 / (float) num1;
      foreach (Vector3 vector3_2 in points)
      {
        float num2 = (vector3_2 - center).Length();
        if ((double) num2 > (double) radius)
          radius = num2;
      }
      return new BoundingSphere(center, radius);
    }

    public static BoundingSphere CreateMerged(BoundingSphere original, BoundingSphere additional)
    {
      Vector3 vector3_1 = Vector3.Subtract(additional.Center, original.Center);
      float num1 = vector3_1.Length();
      if ((double) num1 <= (double) original.Radius + (double) additional.Radius)
      {
        if ((double) num1 <= (double) original.Radius - (double) additional.Radius)
          return original;
        if ((double) num1 <= (double) additional.Radius - (double) original.Radius)
          return additional;
      }
      float num2 = Math.Max(original.Radius - num1, additional.Radius);
      float num3 = Math.Max(original.Radius + num1, additional.Radius);
      Vector3 vector3_2 = vector3_1 + (float) (((double) num2 - (double) num3) / (2.0 * (double) vector3_1.Length())) * vector3_1;
      return new BoundingSphere()
      {
        Center = original.Center + vector3_2,
        Radius = (float) (((double) num2 + (double) num3) / 2.0)
      };
    }

    public static void CreateMerged(ref BoundingSphere original, ref BoundingSphere additional, out BoundingSphere result)
    {
      result = BoundingSphere.CreateMerged(original, additional);
    }

    public bool Equals(BoundingSphere other)
    {
      if (this.Center == other.Center)
        return (double) this.Radius == (double) other.Radius;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (obj is BoundingSphere)
        return this.Equals((BoundingSphere) obj);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Center.GetHashCode() + this.Radius.GetHashCode();
    }

    public bool Intersects(BoundingBox box)
    {
      return box.Intersects(this);
    }

    public void Intersects(ref BoundingBox box, out bool result)
    {
      result = this.Intersects(box);
    }

    public bool Intersects(BoundingFrustum frustum)
    {
      if (frustum == (BoundingFrustum) null)
        throw new NullReferenceException();
      else
        throw new NotImplementedException();
    }

    public bool Intersects(BoundingSphere sphere)
    {
      return (double) Vector3.Distance(sphere.Center, this.Center) <= (double) sphere.Radius + (double) this.Radius;
    }

    public void Intersects(ref BoundingSphere sphere, out bool result)
    {
      result = this.Intersects(sphere);
    }

    public PlaneIntersectionType Intersects(Plane plane)
    {
      PlaneIntersectionType result = PlaneIntersectionType.Front;
      this.Intersects(ref plane, out result);
      return result;
    }

    public void Intersects(ref Plane plane, out PlaneIntersectionType result)
    {
      float result1 = 0.0f;
      Vector3.Dot(ref plane.Normal, ref this.Center, out result1);
      float num = result1 + plane.D;
      if ((double) num > (double) this.Radius)
        result = PlaneIntersectionType.Front;
      else if ((double) num < -(double) this.Radius)
        result = PlaneIntersectionType.Back;
      else
        result = PlaneIntersectionType.Intersecting;
    }

    public float? Intersects(Ray ray)
    {
      return ray.Intersects(this);
    }

    public void Intersects(ref Ray ray, out float? result)
    {
      result = this.Intersects(ray);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{Center:{0} Radius:{1}}}", new object[2]
      {
        (object) this.Center.ToString(),
        (object) this.Radius.ToString()
      });
    }
  }
}
