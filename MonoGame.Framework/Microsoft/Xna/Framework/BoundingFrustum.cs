// Type: Microsoft.Xna.Framework.BoundingFrustum
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Text;

namespace Microsoft.Xna.Framework
{
  public class BoundingFrustum : IEquatable<BoundingFrustum>
  {
    private const int PlaneCount = 6;
    public const int CornerCount = 8;
    private Matrix matrix;
    private Vector3[] corners;
    private Plane[] planes;

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
        return this.planes[0];
      }
    }

    public Plane Far
    {
      get
      {
        return this.planes[1];
      }
    }

    public Plane Left
    {
      get
      {
        return this.planes[2];
      }
    }

    public Plane Right
    {
      get
      {
        return this.planes[3];
      }
    }

    public Plane Top
    {
      get
      {
        return this.planes[4];
      }
    }

    public Plane Bottom
    {
      get
      {
        return this.planes[5];
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
      ContainmentType result = ContainmentType.Disjoint;
      this.Contains(ref box, out result);
      return result;
    }

    public void Contains(ref BoundingBox box, out ContainmentType result)
    {
      bool flag = false;
      for (int index = 0; index < 6; ++index)
      {
        PlaneIntersectionType result1 = PlaneIntersectionType.Front;
        box.Intersects(ref this.planes[index], out result1);
        switch (result1)
        {
          case PlaneIntersectionType.Front:
            result = ContainmentType.Disjoint;
            return;
          case PlaneIntersectionType.Intersecting:
            flag = true;
            break;
        }
      }
      result = flag ? ContainmentType.Intersects : ContainmentType.Contains;
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
      ContainmentType result = ContainmentType.Disjoint;
      this.Contains(ref sphere, out result);
      return result;
    }

    public void Contains(ref BoundingSphere sphere, out ContainmentType result)
    {
      bool flag = false;
      for (int index = 0; index < 6; ++index)
      {
        PlaneIntersectionType result1 = PlaneIntersectionType.Front;
        sphere.Intersects(ref this.planes[index], out result1);
        switch (result1)
        {
          case PlaneIntersectionType.Front:
            result = ContainmentType.Disjoint;
            return;
          case PlaneIntersectionType.Intersecting:
            flag = true;
            break;
        }
      }
      result = flag ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector3 point)
    {
      ContainmentType result = ContainmentType.Disjoint;
      this.Contains(ref point, out result);
      return result;
    }

    public void Contains(ref Vector3 point, out ContainmentType result)
    {
      for (int index = 0; index < 6; ++index)
      {
        if ((double) PlaneHelper.ClassifyPoint(ref point, ref this.planes[index]) > 0.0)
        {
          result = ContainmentType.Disjoint;
          return;
        }
      }
      result = ContainmentType.Contains;
    }

    public bool Equals(BoundingFrustum other)
    {
      return this == other;
    }

    public override bool Equals(object obj)
    {
      BoundingFrustum boundingFrustum = obj as BoundingFrustum;
      if (!object.Equals((object) boundingFrustum, (object) null))
        return this == boundingFrustum;
      else
        return false;
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
      bool result = false;
      this.Intersects(ref sphere, out result);
      return result;
    }

    public void Intersects(ref BoundingSphere sphere, out bool result)
    {
      ContainmentType result1 = ContainmentType.Disjoint;
      this.Contains(ref sphere, out result1);
      result = result1 != ContainmentType.Disjoint;
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
      stringBuilder.Append(this.planes[0].ToString());
      stringBuilder.Append(" Far:");
      stringBuilder.Append(this.planes[1].ToString());
      stringBuilder.Append(" Left:");
      stringBuilder.Append(this.planes[2].ToString());
      stringBuilder.Append(" Right:");
      stringBuilder.Append(this.planes[3].ToString());
      stringBuilder.Append(" Top:");
      stringBuilder.Append(this.planes[4].ToString());
      stringBuilder.Append(" Bottom:");
      stringBuilder.Append(this.planes[5].ToString());
      stringBuilder.Append("}");
      return ((object) stringBuilder).ToString();
    }

    private void CreateCorners()
    {
      this.corners = new Vector3[8];
      BoundingFrustum.IntersectionPoint(ref this.planes[0], ref this.planes[2], ref this.planes[4], out this.corners[0]);
      BoundingFrustum.IntersectionPoint(ref this.planes[0], ref this.planes[3], ref this.planes[4], out this.corners[1]);
      BoundingFrustum.IntersectionPoint(ref this.planes[0], ref this.planes[3], ref this.planes[5], out this.corners[2]);
      BoundingFrustum.IntersectionPoint(ref this.planes[0], ref this.planes[2], ref this.planes[5], out this.corners[3]);
      BoundingFrustum.IntersectionPoint(ref this.planes[1], ref this.planes[2], ref this.planes[4], out this.corners[4]);
      BoundingFrustum.IntersectionPoint(ref this.planes[1], ref this.planes[3], ref this.planes[4], out this.corners[5]);
      BoundingFrustum.IntersectionPoint(ref this.planes[1], ref this.planes[3], ref this.planes[5], out this.corners[6]);
      BoundingFrustum.IntersectionPoint(ref this.planes[1], ref this.planes[2], ref this.planes[5], out this.corners[7]);
    }

    private void CreatePlanes()
    {
      this.planes = new Plane[6];
      this.planes[0] = new Plane(-this.matrix.M13, -this.matrix.M23, -this.matrix.M33, -this.matrix.M43);
      this.planes[1] = new Plane(this.matrix.M13 - this.matrix.M14, this.matrix.M23 - this.matrix.M24, this.matrix.M33 - this.matrix.M34, this.matrix.M43 - this.matrix.M44);
      this.planes[2] = new Plane(-this.matrix.M14 - this.matrix.M11, -this.matrix.M24 - this.matrix.M21, -this.matrix.M34 - this.matrix.M31, -this.matrix.M44 - this.matrix.M41);
      this.planes[3] = new Plane(this.matrix.M11 - this.matrix.M14, this.matrix.M21 - this.matrix.M24, this.matrix.M31 - this.matrix.M34, this.matrix.M41 - this.matrix.M44);
      this.planes[4] = new Plane(this.matrix.M12 - this.matrix.M14, this.matrix.M22 - this.matrix.M24, this.matrix.M32 - this.matrix.M34, this.matrix.M42 - this.matrix.M44);
      this.planes[5] = new Plane(-this.matrix.M14 - this.matrix.M12, -this.matrix.M24 - this.matrix.M22, -this.matrix.M34 - this.matrix.M32, -this.matrix.M44 - this.matrix.M42);
      this.NormalizePlane(ref this.planes[0]);
      this.NormalizePlane(ref this.planes[1]);
      this.NormalizePlane(ref this.planes[2]);
      this.NormalizePlane(ref this.planes[3]);
      this.NormalizePlane(ref this.planes[4]);
      this.NormalizePlane(ref this.planes[5]);
    }

    private static void IntersectionPoint(ref Plane a, ref Plane b, ref Plane c, out Vector3 result)
    {
      Vector3 result1;
      Vector3.Cross(ref b.Normal, ref c.Normal, out result1);
      float result2;
      Vector3.Dot(ref a.Normal, ref result1, out result2);
      float num = result2 * -1f;
      Vector3.Cross(ref b.Normal, ref c.Normal, out result1);
      Vector3 result3;
      Vector3.Multiply(ref result1, a.D, out result3);
      Vector3.Cross(ref c.Normal, ref a.Normal, out result1);
      Vector3 result4;
      Vector3.Multiply(ref result1, b.D, out result4);
      Vector3.Cross(ref a.Normal, ref b.Normal, out result1);
      Vector3 result5;
      Vector3.Multiply(ref result1, c.D, out result5);
      result.X = (result3.X + result4.X + result5.X) / num;
      result.Y = (result3.Y + result4.Y + result5.Y) / num;
      result.Z = (result3.Z + result4.Z + result5.Z) / num;
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
