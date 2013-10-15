// Type: SharpDX.BoundingFrustum
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct BoundingFrustum : IEquatable<BoundingFrustum>, IDataSerializable
  {
    private Matrix pMatrix;
    private Plane pNear;
    private Plane pFar;
    private Plane pLeft;
    private Plane pRight;
    private Plane pTop;
    private Plane pBottom;

    public Matrix Matrix
    {
      get
      {
        return this.pMatrix;
      }
      set
      {
        this.pMatrix = value;
        BoundingFrustum.GetPlanesFromMatrix(ref this.pMatrix, out this.pNear, out this.pFar, out this.pLeft, out this.pRight, out this.pTop, out this.pBottom);
      }
    }

    public Plane Near
    {
      get
      {
        return this.pNear;
      }
    }

    public Plane Far
    {
      get
      {
        return this.pFar;
      }
    }

    public Plane Left
    {
      get
      {
        return this.pLeft;
      }
    }

    public Plane Right
    {
      get
      {
        return this.pRight;
      }
    }

    public Plane Top
    {
      get
      {
        return this.pTop;
      }
    }

    public Plane Bottom
    {
      get
      {
        return this.pBottom;
      }
    }

    public bool IsOrthographic
    {
      get
      {
        if (this.pLeft.Normal == -this.pRight.Normal)
          return this.pTop.Normal == -this.pBottom.Normal;
        else
          return false;
      }
    }

    public BoundingFrustum(Matrix matrix)
    {
      this.pMatrix = matrix;
      BoundingFrustum.GetPlanesFromMatrix(ref this.pMatrix, out this.pNear, out this.pFar, out this.pLeft, out this.pRight, out this.pTop, out this.pBottom);
    }

    public static bool operator ==(BoundingFrustum left, BoundingFrustum right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(BoundingFrustum left, BoundingFrustum right)
    {
      return !left.Equals(right);
    }

    public override int GetHashCode()
    {
      return this.pMatrix.GetHashCode();
    }

    public bool Equals(BoundingFrustum other)
    {
      return this.pMatrix == other.pMatrix;
    }

    public override bool Equals(object obj)
    {
      if (obj != null && obj is BoundingFrustum)
        return this.Equals((BoundingFrustum) obj);
      else
        return false;
    }

    public Plane GetPlane(int index)
    {
      switch (index)
      {
        case 0:
          return this.pLeft;
        case 1:
          return this.pRight;
        case 2:
          return this.pTop;
        case 3:
          return this.pBottom;
        case 4:
          return this.pNear;
        case 5:
          return this.pFar;
        default:
          return new Plane();
      }
    }

    private static void GetPlanesFromMatrix(ref Matrix matrix, out Plane near, out Plane far, out Plane left, out Plane right, out Plane top, out Plane bottom)
    {
      left.Normal.X = matrix.M14 + matrix.M11;
      left.Normal.Y = matrix.M24 + matrix.M21;
      left.Normal.Z = matrix.M34 + matrix.M31;
      left.D = matrix.M44 + matrix.M41;
      left.Normalize();
      right.Normal.X = matrix.M14 - matrix.M11;
      right.Normal.Y = matrix.M24 - matrix.M21;
      right.Normal.Z = matrix.M34 - matrix.M31;
      right.D = matrix.M44 - matrix.M41;
      right.Normalize();
      top.Normal.X = matrix.M14 - matrix.M12;
      top.Normal.Y = matrix.M24 - matrix.M22;
      top.Normal.Z = matrix.M34 - matrix.M32;
      top.D = matrix.M44 - matrix.M42;
      top.Normalize();
      bottom.Normal.X = matrix.M14 + matrix.M12;
      bottom.Normal.Y = matrix.M24 + matrix.M22;
      bottom.Normal.Z = matrix.M34 + matrix.M32;
      bottom.D = matrix.M44 + matrix.M42;
      bottom.Normalize();
      near.Normal.X = matrix.M13;
      near.Normal.Y = matrix.M23;
      near.Normal.Z = matrix.M33;
      near.D = matrix.M43;
      near.Normalize();
      far.Normal.X = matrix.M14 - matrix.M13;
      far.Normal.Y = matrix.M24 - matrix.M23;
      far.Normal.Z = matrix.M34 - matrix.M33;
      far.D = matrix.M44 - matrix.M43;
      far.Normalize();
    }

    private static Vector3 Get3PlanesInterPoint(ref Plane p1, ref Plane p2, ref Plane p3)
    {
      return -p1.D * Vector3.Cross(p2.Normal, p3.Normal) / Vector3.Dot(p1.Normal, Vector3.Cross(p2.Normal, p3.Normal)) - p2.D * Vector3.Cross(p3.Normal, p1.Normal) / Vector3.Dot(p2.Normal, Vector3.Cross(p3.Normal, p1.Normal)) - p3.D * Vector3.Cross(p1.Normal, p2.Normal) / Vector3.Dot(p3.Normal, Vector3.Cross(p1.Normal, p2.Normal));
    }

    public static BoundingFrustum FromCamera(Vector3 cameraPos, Vector3 lookDir, Vector3 upDir, float fov, float znear, float zfar, float aspect)
    {
      lookDir = Vector3.Normalize(lookDir);
      upDir = Vector3.Normalize(upDir);
      Vector3 vector3_1 = cameraPos + lookDir * znear;
      Vector3 vector3_2 = cameraPos + lookDir * zfar;
      float num1 = znear * (float) Math.Tan((double) fov / 2.0);
      float num2 = zfar * (float) Math.Tan((double) fov / 2.0);
      float num3 = num1 * aspect;
      float num4 = num2 * aspect;
      Vector3 vector3_3 = Vector3.Normalize(Vector3.Cross(upDir, lookDir));
      Vector3 vector3_4 = vector3_1 - num1 * upDir + num3 * vector3_3;
      Vector3 vector3_5 = vector3_1 + num1 * upDir + num3 * vector3_3;
      Vector3 vector3_6 = vector3_1 + num1 * upDir - num3 * vector3_3;
      Vector3 point1_1 = vector3_1 - num1 * upDir - num3 * vector3_3;
      Vector3 vector3_7 = vector3_2 - num2 * upDir + num4 * vector3_3;
      Vector3 point2 = vector3_2 + num2 * upDir + num4 * vector3_3;
      Vector3 vector3_8 = vector3_2 + num2 * upDir - num4 * vector3_3;
      Vector3 point1_2 = vector3_2 - num2 * upDir - num4 * vector3_3;
      BoundingFrustum boundingFrustum = new BoundingFrustum();
      boundingFrustum.pNear = new Plane(vector3_4, vector3_5, vector3_6);
      boundingFrustum.pFar = new Plane(vector3_8, point2, vector3_7);
      boundingFrustum.pLeft = new Plane(point1_1, vector3_6, vector3_8);
      boundingFrustum.pRight = new Plane(vector3_7, point2, vector3_5);
      boundingFrustum.pTop = new Plane(vector3_5, point2, vector3_8);
      boundingFrustum.pBottom = new Plane(point1_2, vector3_7, vector3_4);
      boundingFrustum.pNear.Normalize();
      boundingFrustum.pFar.Normalize();
      boundingFrustum.pLeft.Normalize();
      boundingFrustum.pRight.Normalize();
      boundingFrustum.pTop.Normalize();
      boundingFrustum.pBottom.Normalize();
      boundingFrustum.pMatrix = Matrix.LookAtLH(cameraPos, cameraPos + lookDir * 10f, upDir) * Matrix.PerspectiveFovLH(fov, aspect, znear, zfar);
      return boundingFrustum;
    }

    public static BoundingFrustum FromCamera(FrustumCameraParams cameraParams)
    {
      return BoundingFrustum.FromCamera(cameraParams.Position, cameraParams.LookAtDir, cameraParams.UpDir, cameraParams.FOV, cameraParams.ZNear, cameraParams.ZFar, cameraParams.AspectRatio);
    }

    public Vector3[] GetCorners()
    {
      return new Vector3[8]
      {
        BoundingFrustum.Get3PlanesInterPoint(ref this.pNear, ref this.pBottom, ref this.pRight),
        BoundingFrustum.Get3PlanesInterPoint(ref this.pNear, ref this.pTop, ref this.pRight),
        BoundingFrustum.Get3PlanesInterPoint(ref this.pNear, ref this.pTop, ref this.pLeft),
        BoundingFrustum.Get3PlanesInterPoint(ref this.pNear, ref this.pBottom, ref this.pLeft),
        BoundingFrustum.Get3PlanesInterPoint(ref this.pFar, ref this.pBottom, ref this.pRight),
        BoundingFrustum.Get3PlanesInterPoint(ref this.pFar, ref this.pTop, ref this.pRight),
        BoundingFrustum.Get3PlanesInterPoint(ref this.pFar, ref this.pTop, ref this.pLeft),
        BoundingFrustum.Get3PlanesInterPoint(ref this.pFar, ref this.pBottom, ref this.pLeft)
      };
    }

    public FrustumCameraParams GetCameraParams()
    {
      Vector3[] corners = this.GetCorners();
      FrustumCameraParams frustumCameraParams = new FrustumCameraParams();
      frustumCameraParams.Position = BoundingFrustum.Get3PlanesInterPoint(ref this.pRight, ref this.pTop, ref this.pLeft);
      frustumCameraParams.LookAtDir = this.pNear.Normal;
      frustumCameraParams.UpDir = Vector3.Normalize(Vector3.Cross(this.pRight.Normal, this.pNear.Normal));
      frustumCameraParams.FOV = (float) ((Math.PI / 2.0 - Math.Acos((double) Vector3.Dot(this.pNear.Normal, this.pTop.Normal))) * 2.0);
      frustumCameraParams.AspectRatio = (corners[6] - corners[5]).Length() / (corners[4] - corners[5]).Length();
      frustumCameraParams.ZNear = (frustumCameraParams.Position + this.pNear.Normal * this.pNear.D).Length();
      frustumCameraParams.ZFar = (frustumCameraParams.Position + this.pFar.Normal * this.pFar.D).Length();
      return frustumCameraParams;
    }

    public ContainmentType Contains(ref Vector3 point)
    {
      PlaneIntersectionType intersectionType1 = PlaneIntersectionType.Front;
      PlaneIntersectionType intersectionType2 = PlaneIntersectionType.Front;
      for (int index = 0; index < 6; ++index)
      {
        switch (index)
        {
          case 0:
            intersectionType2 = this.pNear.Intersects(ref point);
            break;
          case 1:
            intersectionType2 = this.pFar.Intersects(ref point);
            break;
          case 2:
            intersectionType2 = this.pLeft.Intersects(ref point);
            break;
          case 3:
            intersectionType2 = this.pRight.Intersects(ref point);
            break;
          case 4:
            intersectionType2 = this.pTop.Intersects(ref point);
            break;
          case 5:
            intersectionType2 = this.pBottom.Intersects(ref point);
            break;
        }
        switch (intersectionType2)
        {
          case PlaneIntersectionType.Back:
            return ContainmentType.Disjoint;
          case PlaneIntersectionType.Intersecting:
            intersectionType1 = PlaneIntersectionType.Intersecting;
            break;
        }
      }
      return intersectionType1 == PlaneIntersectionType.Intersecting ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector3 point)
    {
      return this.Contains(ref point);
    }

    public ContainmentType Contains(Vector3[] points)
    {
      bool flag1 = false;
      bool flag2 = true;
      for (int index = 0; index < points.Length; ++index)
      {
        switch (this.Contains(ref points[index]))
        {
          case ContainmentType.Disjoint:
            flag2 = false;
            break;
          case ContainmentType.Contains:
          case ContainmentType.Intersects:
            flag1 = true;
            break;
        }
      }
      if (!flag1)
        return ContainmentType.Disjoint;
      return flag2 ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(Vector3[] points, out ContainmentType result)
    {
      result = this.Contains(points);
    }

    private void GetBoxToPlanePVertexNVertex(ref BoundingBox box, ref Vector3 planeNormal, out Vector3 p, out Vector3 n)
    {
      p = box.Minimum;
      if ((double) planeNormal.X >= 0.0)
        p.X = box.Maximum.X;
      if ((double) planeNormal.Y >= 0.0)
        p.Y = box.Maximum.Y;
      if ((double) planeNormal.Z >= 0.0)
        p.Z = box.Maximum.Z;
      n = box.Maximum;
      if ((double) planeNormal.X >= 0.0)
        n.X = box.Minimum.X;
      if ((double) planeNormal.Y >= 0.0)
        n.Y = box.Minimum.Y;
      if ((double) planeNormal.Z < 0.0)
        return;
      n.Z = box.Minimum.Z;
    }

    public ContainmentType Contains(ref BoundingBox box)
    {
      ContainmentType containmentType = ContainmentType.Contains;
      for (int index = 0; index < 6; ++index)
      {
        Plane plane = this.GetPlane(index);
        Vector3 p;
        Vector3 n;
        this.GetBoxToPlanePVertexNVertex(ref box, ref plane.Normal, out p, out n);
        if (Collision.PlaneIntersectsPoint(ref plane, ref p) == PlaneIntersectionType.Back)
          return ContainmentType.Disjoint;
        if (Collision.PlaneIntersectsPoint(ref plane, ref n) == PlaneIntersectionType.Back)
          containmentType = ContainmentType.Intersects;
      }
      return containmentType;
    }

    public void Contains(ref BoundingBox box, out ContainmentType result)
    {
      result = this.Contains(box.GetCorners());
    }

    public ContainmentType Contains(ref BoundingSphere sphere)
    {
      PlaneIntersectionType intersectionType1 = PlaneIntersectionType.Front;
      PlaneIntersectionType intersectionType2 = PlaneIntersectionType.Front;
      for (int index = 0; index < 6; ++index)
      {
        switch (index)
        {
          case 0:
            intersectionType2 = this.pNear.Intersects(ref sphere);
            break;
          case 1:
            intersectionType2 = this.pFar.Intersects(ref sphere);
            break;
          case 2:
            intersectionType2 = this.pLeft.Intersects(ref sphere);
            break;
          case 3:
            intersectionType2 = this.pRight.Intersects(ref sphere);
            break;
          case 4:
            intersectionType2 = this.pTop.Intersects(ref sphere);
            break;
          case 5:
            intersectionType2 = this.pBottom.Intersects(ref sphere);
            break;
        }
        switch (intersectionType2)
        {
          case PlaneIntersectionType.Back:
            return ContainmentType.Disjoint;
          case PlaneIntersectionType.Intersecting:
            intersectionType1 = PlaneIntersectionType.Intersecting;
            break;
        }
      }
      return intersectionType1 == PlaneIntersectionType.Intersecting ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public void Contains(ref BoundingSphere sphere, out ContainmentType result)
    {
      result = this.Contains(ref sphere);
    }

    public bool Contains(ref BoundingFrustum frustum)
    {
      return this.Contains(frustum.GetCorners()) != ContainmentType.Disjoint;
    }

    public void Contains(ref BoundingFrustum frustum, out bool result)
    {
      result = this.Contains(frustum.GetCorners()) != ContainmentType.Disjoint;
    }

    public bool Intersects(ref BoundingSphere sphere)
    {
      return this.Contains(ref sphere) != ContainmentType.Disjoint;
    }

    public void Intersects(ref BoundingSphere sphere, out bool result)
    {
      result = this.Contains(ref sphere) != ContainmentType.Disjoint;
    }

    public bool Intersects(ref BoundingBox box)
    {
      return this.Contains(ref box) != ContainmentType.Disjoint;
    }

    public void Intersects(ref BoundingBox box, out bool result)
    {
      result = this.Contains(ref box) != ContainmentType.Disjoint;
    }

    private PlaneIntersectionType PlaneIntersectsPoints(ref Plane plane, Vector3[] points)
    {
      PlaneIntersectionType intersectionType = Collision.PlaneIntersectsPoint(ref plane, ref points[0]);
      for (int index = 1; index < points.Length; ++index)
      {
        if (Collision.PlaneIntersectsPoint(ref plane, ref points[index]) != intersectionType)
          return PlaneIntersectionType.Intersecting;
      }
      return intersectionType;
    }

    public PlaneIntersectionType Intersects(ref Plane plane)
    {
      return this.PlaneIntersectsPoints(ref plane, this.GetCorners());
    }

    public void Intersects(ref Plane plane, out PlaneIntersectionType result)
    {
      result = this.PlaneIntersectsPoints(ref plane, this.GetCorners());
    }

    public float GetWidthAtDepth(float depth)
    {
      return (float) (Math.Tan(Math.PI / 2.0 - Math.Acos((double) Vector3.Dot(this.pNear.Normal, this.pLeft.Normal))) * (double) depth * 2.0);
    }

    public float GetHeightAtDepth(float depth)
    {
      return (float) (Math.Tan(Math.PI / 2.0 - Math.Acos((double) Vector3.Dot(this.pNear.Normal, this.pTop.Normal))) * (double) depth * 2.0);
    }

    private BoundingFrustum GetInsideOutClone()
    {
      BoundingFrustum boundingFrustum = this;
      boundingFrustum.pNear.Normal = -boundingFrustum.pNear.Normal;
      boundingFrustum.pFar.Normal = -boundingFrustum.pFar.Normal;
      boundingFrustum.pLeft.Normal = -boundingFrustum.pLeft.Normal;
      boundingFrustum.pRight.Normal = -boundingFrustum.pRight.Normal;
      boundingFrustum.pTop.Normal = -boundingFrustum.pTop.Normal;
      boundingFrustum.pBottom.Normal = -boundingFrustum.pBottom.Normal;
      return boundingFrustum;
    }

    public bool Intersects(ref Ray ray)
    {
      float? inDistance;
      float? outDistance;
      return this.Intersects(ref ray, out inDistance, out outDistance);
    }

    public bool Intersects(ref Ray ray, out float? inDistance, out float? outDistance)
    {
      if (this.Contains(ray.Position) != ContainmentType.Disjoint)
      {
        float num = float.MaxValue;
        for (int index = 0; index < 6; ++index)
        {
          Plane plane = this.GetPlane(index);
          float distance;
          if (Collision.RayIntersectsPlane(ref ray, ref plane, out distance) && (double) distance < (double) num)
            num = distance;
        }
        inDistance = new float?(num);
        outDistance = new float?();
        return true;
      }
      else
      {
        float val1_1 = float.MaxValue;
        float val1_2 = float.MinValue;
        for (int index = 0; index < 6; ++index)
        {
          Plane plane = this.GetPlane(index);
          float distance;
          if (Collision.RayIntersectsPlane(ref ray, ref plane, out distance))
          {
            val1_1 = Math.Min(val1_1, distance);
            val1_2 = Math.Max(val1_2, distance);
          }
        }
        Vector3 point = (ray.Position + ray.Direction * val1_1 + ray.Position + ray.Direction * val1_2) / 2f;
        if (this.Contains(ref point) != ContainmentType.Disjoint)
        {
          inDistance = new float?(val1_1);
          outDistance = new float?(val1_2);
          return true;
        }
        else
        {
          inDistance = new float?();
          outDistance = new float?();
          return false;
        }
      }
    }

    public float GetZoomToExtentsShiftDistance(Vector3[] points)
    {
      float num1 = (float) Math.Sin(Math.PI / 2.0 - Math.Acos((double) Vector3.Dot(this.pNear.Normal, this.pTop.Normal)));
      float num2 = (float) Math.Sin(Math.PI / 2.0 - Math.Acos((double) Vector3.Dot(this.pNear.Normal, this.pLeft.Normal)));
      float num3 = num1 / num2;
      BoundingFrustum insideOutClone = this.GetInsideOutClone();
      float val1 = float.MinValue;
      for (int index = 0; index < points.Length; ++index)
      {
        float val2 = Math.Max(Math.Max(Math.Max(Collision.DistancePlanePoint(ref insideOutClone.pTop, ref points[index]), Collision.DistancePlanePoint(ref insideOutClone.pBottom, ref points[index])), Collision.DistancePlanePoint(ref insideOutClone.pLeft, ref points[index]) * num3), Collision.DistancePlanePoint(ref insideOutClone.pRight, ref points[index]) * num3);
        val1 = Math.Max(val1, val2);
      }
      return -val1 / num1;
    }

    public float GetZoomToExtentsShiftDistance(ref BoundingBox boundingBox)
    {
      return this.GetZoomToExtentsShiftDistance(boundingBox.GetCorners());
    }

    public Vector3 GetZoomToExtentsShiftVector(Vector3[] points)
    {
      return this.GetZoomToExtentsShiftDistance(points) * this.pNear.Normal;
    }

    public Vector3 GetZoomToExtentsShiftVector(ref BoundingBox boundingBox)
    {
      return this.GetZoomToExtentsShiftDistance(boundingBox.GetCorners()) * this.pNear.Normal;
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      serializer.Serialize<Matrix>(ref this.pMatrix, SerializeFlags.Normal);
      serializer.Serialize<Plane>(ref this.pNear, SerializeFlags.Normal);
      serializer.Serialize<Plane>(ref this.pFar, SerializeFlags.Normal);
      serializer.Serialize<Plane>(ref this.pLeft, SerializeFlags.Normal);
      serializer.Serialize<Plane>(ref this.pRight, SerializeFlags.Normal);
      serializer.Serialize<Plane>(ref this.pTop, SerializeFlags.Normal);
      serializer.Serialize<Plane>(ref this.pBottom, SerializeFlags.Normal);
    }
  }
}
