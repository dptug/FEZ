// Type: Microsoft.Xna.Framework.BoundingBox
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
  [Serializable]
  public struct BoundingBox : IEquatable<BoundingBox>
  {
    public const int CornerCount = 8;
    public Vector3 Min;
    public Vector3 Max;

    public BoundingBox(Vector3 min, Vector3 max)
    {
      this.Min = min;
      this.Max = max;
    }

    public static bool operator ==(BoundingBox a, BoundingBox b)
    {
      return a.Equals(b);
    }

    public static bool operator !=(BoundingBox a, BoundingBox b)
    {
      return !a.Equals(b);
    }

    public ContainmentType Contains(BoundingBox box)
    {
      if ((double) box.Max.X < (double) this.Min.X || (double) box.Min.X > (double) this.Max.X || ((double) box.Max.Y < (double) this.Min.Y || (double) box.Min.Y > (double) this.Max.Y) || (double) box.Max.Z < (double) this.Min.Z || (double) box.Min.Z > (double) this.Max.Z)
        return ContainmentType.Disjoint;
      return (double) box.Min.X >= (double) this.Min.X && (double) box.Max.X <= (double) this.Max.X && ((double) box.Min.Y >= (double) this.Min.Y && (double) box.Max.Y <= (double) this.Max.Y) && (double) box.Min.Z >= (double) this.Min.Z && (double) box.Max.Z <= (double) this.Max.Z ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingBox box, out ContainmentType result)
    {
      result = this.Contains(box);
    }

    public ContainmentType Contains(BoundingFrustum frustum)
    {
      Vector3[] corners = frustum.GetCorners();
      int index1;
      ContainmentType result;
      for (index1 = 0; index1 < corners.Length; ++index1)
      {
        this.Contains(ref corners[index1], out result);
        if (result == ContainmentType.Disjoint)
          break;
      }
      if (index1 == corners.Length)
        return ContainmentType.Contains;
      if (index1 != 0)
        return ContainmentType.Intersects;
      for (int index2 = index1 + 1; index2 < corners.Length; ++index2)
      {
        this.Contains(ref corners[index2], out result);
        if (result != ContainmentType.Contains)
          return ContainmentType.Intersects;
      }
      return ContainmentType.Contains;
    }

    public ContainmentType Contains(BoundingSphere sphere)
    {
      if ((double) sphere.Center.X - (double) this.Min.X > (double) sphere.Radius && (double) sphere.Center.Y - (double) this.Min.Y > (double) sphere.Radius && ((double) sphere.Center.Z - (double) this.Min.Z > (double) sphere.Radius && (double) this.Max.X - (double) sphere.Center.X > (double) sphere.Radius) && (double) this.Max.Y - (double) sphere.Center.Y > (double) sphere.Radius && (double) this.Max.Z - (double) sphere.Center.Z > (double) sphere.Radius)
        return ContainmentType.Contains;
      double num = 0.0;
      if ((double) sphere.Center.X - (double) this.Min.X <= (double) sphere.Radius)
        num += ((double) sphere.Center.X - (double) this.Min.X) * ((double) sphere.Center.X - (double) this.Min.X);
      else if ((double) this.Max.X - (double) sphere.Center.X <= (double) sphere.Radius)
        num += ((double) sphere.Center.X - (double) this.Max.X) * ((double) sphere.Center.X - (double) this.Max.X);
      if ((double) sphere.Center.Y - (double) this.Min.Y <= (double) sphere.Radius)
        num += ((double) sphere.Center.Y - (double) this.Min.Y) * ((double) sphere.Center.Y - (double) this.Min.Y);
      else if ((double) this.Max.Y - (double) sphere.Center.Y <= (double) sphere.Radius)
        num += ((double) sphere.Center.Y - (double) this.Max.Y) * ((double) sphere.Center.Y - (double) this.Max.Y);
      if ((double) sphere.Center.Z - (double) this.Min.Z <= (double) sphere.Radius)
        num += ((double) sphere.Center.Z - (double) this.Min.Z) * ((double) sphere.Center.Z - (double) this.Min.Z);
      else if ((double) this.Max.Z - (double) sphere.Center.Z <= (double) sphere.Radius)
        num += ((double) sphere.Center.Z - (double) this.Max.Z) * ((double) sphere.Center.Z - (double) this.Max.Z);
      return num <= (double) sphere.Radius * (double) sphere.Radius ? ContainmentType.Intersects : ContainmentType.Disjoint;
    }

    public void Contains(ref BoundingSphere sphere, out ContainmentType result)
    {
      result = this.Contains(sphere);
    }

    public ContainmentType Contains(Vector3 point)
    {
      ContainmentType result;
      this.Contains(ref point, out result);
      return result;
    }

    public void Contains(ref Vector3 point, out ContainmentType result)
    {
      if ((double) point.X < (double) this.Min.X || (double) point.X > (double) this.Max.X || ((double) point.Y < (double) this.Min.Y || (double) point.Y > (double) this.Max.Y) || (double) point.Z < (double) this.Min.Z || (double) point.Z > (double) this.Max.Z)
        result = ContainmentType.Disjoint;
      else if ((double) point.X == (double) this.Min.X || (double) point.X == (double) this.Max.X || ((double) point.Y == (double) this.Min.Y || (double) point.Y == (double) this.Max.Y) || (double) point.Z == (double) this.Min.Z || (double) point.Z == (double) this.Max.Z)
        result = ContainmentType.Intersects;
      else
        result = ContainmentType.Contains;
    }

    public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
    {
      if (points == null)
        throw new ArgumentNullException();
      bool flag = true;
      Vector3 min = new Vector3(float.MaxValue);
      Vector3 max = new Vector3(float.MinValue);
      foreach (Vector3 vector3 in points)
      {
        min = Vector3.Min(min, vector3);
        max = Vector3.Max(max, vector3);
        flag = false;
      }
      if (flag)
        throw new ArgumentException();
      else
        return new BoundingBox(min, max);
    }

    public static BoundingBox CreateFromSphere(BoundingSphere sphere)
    {
      Vector3 vector3 = new Vector3(sphere.Radius);
      return new BoundingBox(sphere.Center - vector3, sphere.Center + vector3);
    }

    public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
    {
      result = BoundingBox.CreateFromSphere(sphere);
    }

    public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
    {
      return new BoundingBox(Vector3.Min(original.Min, additional.Min), Vector3.Max(original.Max, additional.Max));
    }

    public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
    {
      result = BoundingBox.CreateMerged(original, additional);
    }

    public bool Equals(BoundingBox other)
    {
      return this.Min == other.Min && this.Max == other.Max;
    }

    public override bool Equals(object obj)
    {
      return obj is BoundingBox && this.Equals((BoundingBox) obj);
    }

    public Vector3[] GetCorners()
    {
      return new Vector3[8]
      {
        new Vector3(this.Min.X, this.Max.Y, this.Max.Z),
        new Vector3(this.Max.X, this.Max.Y, this.Max.Z),
        new Vector3(this.Max.X, this.Min.Y, this.Max.Z),
        new Vector3(this.Min.X, this.Min.Y, this.Max.Z),
        new Vector3(this.Min.X, this.Max.Y, this.Min.Z),
        new Vector3(this.Max.X, this.Max.Y, this.Min.Z),
        new Vector3(this.Max.X, this.Min.Y, this.Min.Z),
        new Vector3(this.Min.X, this.Min.Y, this.Min.Z)
      };
    }

    public void GetCorners(Vector3[] corners)
    {
      if (corners == null)
        throw new ArgumentNullException("corners");
      if (corners.Length < 8)
        throw new ArgumentOutOfRangeException("corners", "Not Enought Corners");
      corners[0].X = this.Min.X;
      corners[0].Y = this.Max.Y;
      corners[0].Z = this.Max.Z;
      corners[1].X = this.Max.X;
      corners[1].Y = this.Max.Y;
      corners[1].Z = this.Max.Z;
      corners[2].X = this.Max.X;
      corners[2].Y = this.Min.Y;
      corners[2].Z = this.Max.Z;
      corners[3].X = this.Min.X;
      corners[3].Y = this.Min.Y;
      corners[3].Z = this.Max.Z;
      corners[4].X = this.Min.X;
      corners[4].Y = this.Max.Y;
      corners[4].Z = this.Min.Z;
      corners[5].X = this.Max.X;
      corners[5].Y = this.Max.Y;
      corners[5].Z = this.Min.Z;
      corners[6].X = this.Max.X;
      corners[6].Y = this.Min.Y;
      corners[6].Z = this.Min.Z;
      corners[7].X = this.Min.X;
      corners[7].Y = this.Min.Y;
      corners[7].Z = this.Min.Z;
    }

    public override int GetHashCode()
    {
      return this.Min.GetHashCode() + this.Max.GetHashCode();
    }

    public bool Intersects(BoundingBox box)
    {
      bool result;
      this.Intersects(ref box, out result);
      return result;
    }

    public void Intersects(ref BoundingBox box, out bool result)
    {
      if ((double) this.Max.X >= (double) box.Min.X && (double) this.Min.X <= (double) box.Max.X)
      {
        if ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y)
          result = false;
        else
          result = (double) this.Max.Z >= (double) box.Min.Z && (double) this.Min.Z <= (double) box.Max.Z;
      }
      else
        result = false;
    }

    public bool Intersects(BoundingFrustum frustum)
    {
      return frustum.Intersects(this);
    }

    public bool Intersects(BoundingSphere sphere)
    {
      if ((double) sphere.Center.X - (double) this.Min.X > (double) sphere.Radius && (double) sphere.Center.Y - (double) this.Min.Y > (double) sphere.Radius && ((double) sphere.Center.Z - (double) this.Min.Z > (double) sphere.Radius && (double) this.Max.X - (double) sphere.Center.X > (double) sphere.Radius) && (double) this.Max.Y - (double) sphere.Center.Y > (double) sphere.Radius && (double) this.Max.Z - (double) sphere.Center.Z > (double) sphere.Radius)
        return true;
      double num = 0.0;
      if ((double) sphere.Center.X - (double) this.Min.X <= (double) sphere.Radius)
        num += ((double) sphere.Center.X - (double) this.Min.X) * ((double) sphere.Center.X - (double) this.Min.X);
      else if ((double) this.Max.X - (double) sphere.Center.X <= (double) sphere.Radius)
        num += ((double) sphere.Center.X - (double) this.Max.X) * ((double) sphere.Center.X - (double) this.Max.X);
      if ((double) sphere.Center.Y - (double) this.Min.Y <= (double) sphere.Radius)
        num += ((double) sphere.Center.Y - (double) this.Min.Y) * ((double) sphere.Center.Y - (double) this.Min.Y);
      else if ((double) this.Max.Y - (double) sphere.Center.Y <= (double) sphere.Radius)
        num += ((double) sphere.Center.Y - (double) this.Max.Y) * ((double) sphere.Center.Y - (double) this.Max.Y);
      if ((double) sphere.Center.Z - (double) this.Min.Z <= (double) sphere.Radius)
        num += ((double) sphere.Center.Z - (double) this.Min.Z) * ((double) sphere.Center.Z - (double) this.Min.Z);
      else if ((double) this.Max.Z - (double) sphere.Center.Z <= (double) sphere.Radius)
        num += ((double) sphere.Center.Z - (double) this.Max.Z) * ((double) sphere.Center.Z - (double) this.Max.Z);
      return num <= (double) sphere.Radius * (double) sphere.Radius;
    }

    public void Intersects(ref BoundingSphere sphere, out bool result)
    {
      result = this.Intersects(sphere);
    }

    public PlaneIntersectionType Intersects(Plane plane)
    {
      PlaneIntersectionType result;
      this.Intersects(ref plane, out result);
      return result;
    }

    public void Intersects(ref Plane plane, out PlaneIntersectionType result)
    {
      Vector3 vector2_1;
      Vector3 vector2_2;
      if ((double) plane.Normal.X >= 0.0)
      {
        vector2_1.X = this.Max.X;
        vector2_2.X = this.Min.X;
      }
      else
      {
        vector2_1.X = this.Min.X;
        vector2_2.X = this.Max.X;
      }
      if ((double) plane.Normal.Y >= 0.0)
      {
        vector2_1.Y = this.Max.Y;
        vector2_2.Y = this.Min.Y;
      }
      else
      {
        vector2_1.Y = this.Min.Y;
        vector2_2.Y = this.Max.Y;
      }
      if ((double) plane.Normal.Z >= 0.0)
      {
        vector2_1.Z = this.Max.Z;
        vector2_2.Z = this.Min.Z;
      }
      else
      {
        vector2_1.Z = this.Min.Z;
        vector2_2.Z = this.Max.Z;
      }
      if ((double) (Vector3.Dot(plane.Normal, vector2_2) + plane.D) > 0.0)
        result = PlaneIntersectionType.Front;
      else if ((double) (Vector3.Dot(plane.Normal, vector2_1) + plane.D) < 0.0)
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
      return string.Format("{{Min:{0} Max:{1}}}", (object) this.Min.ToString(), (object) this.Max.ToString());
    }
  }
}
