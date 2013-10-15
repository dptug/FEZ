// Type: Microsoft.Xna.Framework.BoundingFrustum
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Text;

namespace Microsoft.Xna.Framework
{
  public class BoundingFrustum : IEquatable<BoundingFrustum>
  {
    public const int CornerCount = 8;
    private Matrix matrix;
    private Plane bottom;
    private Plane far;
    private Plane left;
    private Plane right;
    private Plane near;
    private Plane top;
    private Vector3[] corners;

    public Plane Bottom
    {
      get
      {
        return this.bottom;
      }
    }

    public Plane Far
    {
      get
      {
        return this.far;
      }
    }

    public Plane Left
    {
      get
      {
        return this.left;
      }
    }

    public Matrix Matrix
    {
      get
      {
        return this.matrix;
      }
      set
      {
        this.matrix = value;
        this.CreatePlanes();
        this.CreateCorners();
      }
    }

    public Plane Near
    {
      get
      {
        return this.near;
      }
    }

    public Plane Right
    {
      get
      {
        return this.right;
      }
    }

    public Plane Top
    {
      get
      {
        return this.top;
      }
    }

    public BoundingFrustum(Matrix value)
    {
      this.matrix = value;
      this.CreatePlanes();
      this.CreateCorners();
    }

    public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
    {
      if (object.Equals((object) a, (object) null))
        return object.Equals((object) b, (object) null);
      if (object.Equals((object) b, (object) null))
        return object.Equals((object) a, (object) null);
      else
        return a.matrix == b.matrix;
    }

    public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
    {
      return !(a == b);
    }

    public ContainmentType Contains(BoundingBox box)
    {
      ContainmentType result;
      this.Contains(ref box, out result);
      return result;
    }

    public void Contains(ref BoundingBox box, out ContainmentType result)
    {
      bool flag = false;
      PlaneIntersectionType result1;
      box.Intersects(ref this.near, out result1);
      if (result1 == PlaneIntersectionType.Front)
      {
        result = ContainmentType.Disjoint;
      }
      else
      {
        if (result1 == PlaneIntersectionType.Intersecting)
          flag = true;
        box.Intersects(ref this.left, out result1);
        if (result1 == PlaneIntersectionType.Front)
        {
          result = ContainmentType.Disjoint;
        }
        else
        {
          if (result1 == PlaneIntersectionType.Intersecting)
            flag = true;
          box.Intersects(ref this.right, out result1);
          if (result1 == PlaneIntersectionType.Front)
          {
            result = ContainmentType.Disjoint;
          }
          else
          {
            if (result1 == PlaneIntersectionType.Intersecting)
              flag = true;
            box.Intersects(ref this.top, out result1);
            if (result1 == PlaneIntersectionType.Front)
            {
              result = ContainmentType.Disjoint;
            }
            else
            {
              if (result1 == PlaneIntersectionType.Intersecting)
                flag = true;
              box.Intersects(ref this.bottom, out result1);
              if (result1 == PlaneIntersectionType.Front)
              {
                result = ContainmentType.Disjoint;
              }
              else
              {
                if (result1 == PlaneIntersectionType.Intersecting)
                  flag = true;
                box.Intersects(ref this.far, out result1);
                if (result1 == PlaneIntersectionType.Front)
                {
                  result = ContainmentType.Disjoint;
                }
                else
                {
                  if (result1 == PlaneIntersectionType.Intersecting)
                    flag = true;
                  result = flag ? ContainmentType.Intersects : ContainmentType.Contains;
                }
              }
            }
          }
        }
      }
    }

    public ContainmentType Contains(BoundingFrustum frustum)
    {
      if (this == frustum)
        return ContainmentType.Contains;
      else
        throw new NotImplementedException();
    }

    public ContainmentType Contains(BoundingSphere sphere)
    {
      ContainmentType result;
      this.Contains(ref sphere, out result);
      return result;
    }

    public void Contains(ref BoundingSphere sphere, out ContainmentType result)
    {
      result = ContainmentType.Contains;
      float result1;
      Vector3.Dot(ref this.bottom.Normal, ref sphere.Center, out result1);
      float result2 = result1 + this.bottom.D;
      if ((double) result2 > (double) sphere.Radius)
      {
        result = ContainmentType.Disjoint;
      }
      else
      {
        if ((double) Math.Abs(result2) < (double) sphere.Radius)
          result = ContainmentType.Intersects;
        Vector3.Dot(ref this.top.Normal, ref sphere.Center, out result2);
        float result3 = result2 + this.top.D;
        if ((double) result3 > (double) sphere.Radius)
        {
          result = ContainmentType.Disjoint;
        }
        else
        {
          if ((double) Math.Abs(result3) < (double) sphere.Radius)
            result = ContainmentType.Intersects;
          Vector3.Dot(ref this.near.Normal, ref sphere.Center, out result3);
          float result4 = result3 + this.near.D;
          if ((double) result4 > (double) sphere.Radius)
          {
            result = ContainmentType.Disjoint;
          }
          else
          {
            if ((double) Math.Abs(result4) < (double) sphere.Radius)
              result = ContainmentType.Intersects;
            Vector3.Dot(ref this.far.Normal, ref sphere.Center, out result4);
            float result5 = result4 + this.far.D;
            if ((double) result5 > (double) sphere.Radius)
            {
              result = ContainmentType.Disjoint;
            }
            else
            {
              if ((double) Math.Abs(result5) < (double) sphere.Radius)
                result = ContainmentType.Intersects;
              Vector3.Dot(ref this.left.Normal, ref sphere.Center, out result5);
              float result6 = result5 + this.left.D;
              if ((double) result6 > (double) sphere.Radius)
              {
                result = ContainmentType.Disjoint;
              }
              else
              {
                if ((double) Math.Abs(result6) < (double) sphere.Radius)
                  result = ContainmentType.Intersects;
                Vector3.Dot(ref this.right.Normal, ref sphere.Center, out result6);
                float num = result6 + this.right.D;
                if ((double) num > (double) sphere.Radius)
                {
                  result = ContainmentType.Disjoint;
                }
                else
                {
                  if ((double) Math.Abs(num) >= (double) sphere.Radius)
                    return;
                  result = ContainmentType.Intersects;
                }
              }
            }
          }
        }
      }
    }

    public ContainmentType Contains(Vector3 point)
    {
      ContainmentType result;
      this.Contains(ref point, out result);
      return result;
    }

    public void Contains(ref Vector3 point, out ContainmentType result)
    {
      if ((double) PlaneHelper.ClassifyPoint(ref point, ref this.top) > 0.0)
        result = ContainmentType.Disjoint;
      else if ((double) PlaneHelper.ClassifyPoint(ref point, ref this.bottom) > 0.0)
        result = ContainmentType.Disjoint;
      else if ((double) PlaneHelper.ClassifyPoint(ref point, ref this.left) > 0.0)
        result = ContainmentType.Disjoint;
      else if ((double) PlaneHelper.ClassifyPoint(ref point, ref this.right) > 0.0)
        result = ContainmentType.Disjoint;
      else if ((double) PlaneHelper.ClassifyPoint(ref point, ref this.near) > 0.0)
        result = ContainmentType.Disjoint;
      else if ((double) PlaneHelper.ClassifyPoint(ref point, ref this.far) > 0.0)
        result = ContainmentType.Disjoint;
      else
        result = ContainmentType.Contains;
    }

    public bool Equals(BoundingFrustum other)
    {
      return this == other;
    }

    public override bool Equals(object obj)
    {
      BoundingFrustum boundingFrustum = obj as BoundingFrustum;
      return !object.Equals((object) boundingFrustum, (object) null) && this == boundingFrustum;
    }

    public Vector3[] GetCorners()
    {
      return (Vector3[]) this.corners.Clone();
    }

    public void GetCorners(Vector3[] corners)
    {
      if (corners == null)
        throw new ArgumentNullException("corners");
      if (corners.Length < 8)
        throw new ArgumentOutOfRangeException("corners");
      this.corners.CopyTo((Array) corners, 0);
    }

    public override int GetHashCode()
    {
      return this.matrix.GetHashCode();
    }

    public bool Intersects(BoundingBox box)
    {
      bool result = false;
      this.Intersects(ref box, out result);
      return result;
    }

    public void Intersects(ref BoundingBox box, out bool result)
    {
      ContainmentType result1 = ContainmentType.Disjoint;
      this.Contains(ref box, out result1);
      result = result1 != ContainmentType.Disjoint;
    }

    public bool Intersects(BoundingFrustum frustum)
    {
      throw new NotImplementedException();
    }

    public bool Intersects(BoundingSphere sphere)
    {
      throw new NotImplementedException();
    }

    public void Intersects(ref BoundingSphere sphere, out bool result)
    {
      throw new NotImplementedException();
    }

    public PlaneIntersectionType Intersects(Plane plane)
    {
      throw new NotImplementedException();
    }

    public void Intersects(ref Plane plane, out PlaneIntersectionType result)
    {
      throw new NotImplementedException();
    }

    public float? Intersects(Ray ray)
    {
      throw new NotImplementedException();
    }

    public void Intersects(ref Ray ray, out float? result)
    {
      throw new NotImplementedException();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(256);
      stringBuilder.Append("{Near:");
      stringBuilder.Append(this.near.ToString());
      stringBuilder.Append(" Far:");
      stringBuilder.Append(this.far.ToString());
      stringBuilder.Append(" Left:");
      stringBuilder.Append(this.left.ToString());
      stringBuilder.Append(" Right:");
      stringBuilder.Append(this.right.ToString());
      stringBuilder.Append(" Top:");
      stringBuilder.Append(this.top.ToString());
      stringBuilder.Append(" Bottom:");
      stringBuilder.Append(this.bottom.ToString());
      stringBuilder.Append("}");
      return ((object) stringBuilder).ToString();
    }

    private void CreateCorners()
    {
      this.corners = new Vector3[8];
      this.corners[0] = BoundingFrustum.IntersectionPoint(ref this.near, ref this.left, ref this.top);
      this.corners[1] = BoundingFrustum.IntersectionPoint(ref this.near, ref this.right, ref this.top);
      this.corners[2] = BoundingFrustum.IntersectionPoint(ref this.near, ref this.right, ref this.bottom);
      this.corners[3] = BoundingFrustum.IntersectionPoint(ref this.near, ref this.left, ref this.bottom);
      this.corners[4] = BoundingFrustum.IntersectionPoint(ref this.far, ref this.left, ref this.top);
      this.corners[5] = BoundingFrustum.IntersectionPoint(ref this.far, ref this.right, ref this.top);
      this.corners[6] = BoundingFrustum.IntersectionPoint(ref this.far, ref this.right, ref this.bottom);
      this.corners[7] = BoundingFrustum.IntersectionPoint(ref this.far, ref this.left, ref this.bottom);
    }

    private void CreatePlanes()
    {
      this.left = new Plane(-this.matrix.M14 - this.matrix.M11, -this.matrix.M24 - this.matrix.M21, -this.matrix.M34 - this.matrix.M31, -this.matrix.M44 - this.matrix.M41);
      this.right = new Plane(this.matrix.M11 - this.matrix.M14, this.matrix.M21 - this.matrix.M24, this.matrix.M31 - this.matrix.M34, this.matrix.M41 - this.matrix.M44);
      this.top = new Plane(this.matrix.M12 - this.matrix.M14, this.matrix.M22 - this.matrix.M24, this.matrix.M32 - this.matrix.M34, this.matrix.M42 - this.matrix.M44);
      this.bottom = new Plane(-this.matrix.M14 - this.matrix.M12, -this.matrix.M24 - this.matrix.M22, -this.matrix.M34 - this.matrix.M32, -this.matrix.M44 - this.matrix.M42);
      this.near = new Plane(-this.matrix.M13, -this.matrix.M23, -this.matrix.M33, -this.matrix.M43);
      this.far = new Plane(this.matrix.M13 - this.matrix.M14, this.matrix.M23 - this.matrix.M24, this.matrix.M33 - this.matrix.M34, this.matrix.M43 - this.matrix.M44);
      this.NormalizePlane(ref this.left);
      this.NormalizePlane(ref this.right);
      this.NormalizePlane(ref this.top);
      this.NormalizePlane(ref this.bottom);
      this.NormalizePlane(ref this.near);
      this.NormalizePlane(ref this.far);
    }

    private static Vector3 IntersectionPoint(ref Plane a, ref Plane b, ref Plane c)
    {
      float num = -Vector3.Dot(a.Normal, Vector3.Cross(b.Normal, c.Normal));
      Vector3 vector3_1 = a.D * Vector3.Cross(b.Normal, c.Normal);
      Vector3 vector3_2 = b.D * Vector3.Cross(c.Normal, a.Normal);
      Vector3 vector3_3 = c.D * Vector3.Cross(a.Normal, b.Normal);
      return new Vector3(vector3_1.X + vector3_2.X + vector3_3.X, vector3_1.Y + vector3_2.Y + vector3_3.Y, vector3_1.Z + vector3_2.Z + vector3_3.Z) / num;
    }

    private void NormalizePlane(ref Plane p)
    {
      float num = 1f / p.Normal.Length();
      p.Normal.X *= num;
      p.Normal.Y *= num;
      p.Normal.Z *= num;
      p.D *= num;
    }
  }
}
