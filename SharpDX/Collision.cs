// Type: SharpDX.Collision
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public static class Collision
  {
    public static void ClosestPointPointTriangle(ref Vector3 point, ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3, out Vector3 result)
    {
      Vector3 left1 = vertex2 - vertex1;
      Vector3 left2 = vertex3 - vertex1;
      Vector3 right1 = point - vertex1;
      float num1 = Vector3.Dot(left1, right1);
      float num2 = Vector3.Dot(left2, right1);
      if ((double) num1 <= 0.0 && (double) num2 <= 0.0)
        result = vertex1;
      Vector3 right2 = point - vertex2;
      float num3 = Vector3.Dot(left1, right2);
      float num4 = Vector3.Dot(left2, right2);
      if ((double) num3 >= 0.0 && (double) num4 <= (double) num3)
        result = vertex2;
      float num5 = (float) ((double) num1 * (double) num4 - (double) num3 * (double) num2);
      if ((double) num5 <= 0.0 && (double) num1 >= 0.0 && (double) num3 <= 0.0)
      {
        float num6 = num1 / (num1 - num3);
        result = vertex1 + num6 * left1;
      }
      Vector3 right3 = point - vertex3;
      float num7 = Vector3.Dot(left1, right3);
      float num8 = Vector3.Dot(left2, right3);
      if ((double) num8 >= 0.0 && (double) num7 <= (double) num8)
        result = vertex3;
      float num9 = (float) ((double) num7 * (double) num2 - (double) num1 * (double) num8);
      if ((double) num9 <= 0.0 && (double) num2 >= 0.0 && (double) num8 <= 0.0)
      {
        float num6 = num2 / (num2 - num8);
        result = vertex1 + num6 * left2;
      }
      float num10 = (float) ((double) num3 * (double) num8 - (double) num7 * (double) num4);
      if ((double) num10 <= 0.0 && (double) num4 - (double) num3 >= 0.0 && (double) num7 - (double) num8 >= 0.0)
      {
        float num6 = (float) (((double) num4 - (double) num3) / ((double) num4 - (double) num3 + ((double) num7 - (double) num8)));
        result = vertex2 + num6 * (vertex3 - vertex2);
      }
      float num11 = (float) (1.0 / ((double) num10 + (double) num9 + (double) num5));
      float num12 = num9 * num11;
      float num13 = num5 * num11;
      result = vertex1 + left1 * num12 + left2 * num13;
    }

    public static void ClosestPointPlanePoint(ref Plane plane, ref Vector3 point, out Vector3 result)
    {
      float result1;
      Vector3.Dot(ref plane.Normal, ref point, out result1);
      float num = result1 - plane.D;
      result = point - num * plane.Normal;
    }

    public static void ClosestPointBoxPoint(ref BoundingBox box, ref Vector3 point, out Vector3 result)
    {
      Vector3 result1;
      Vector3.Max(ref point, ref box.Minimum, out result1);
      Vector3.Min(ref result1, ref box.Maximum, out result);
    }

    public static void ClosestPointSpherePoint(ref BoundingSphere sphere, ref Vector3 point, out Vector3 result)
    {
      Vector3.Subtract(ref point, ref sphere.Center, out result);
      result.Normalize();
      result *= sphere.Radius;
      result += sphere.Center;
    }

    public static void ClosestPointSphereSphere(ref BoundingSphere sphere1, ref BoundingSphere sphere2, out Vector3 result)
    {
      Vector3.Subtract(ref sphere2.Center, ref sphere1.Center, out result);
      result.Normalize();
      result *= sphere1.Radius;
      result += sphere1.Center;
    }

    public static float DistancePlanePoint(ref Plane plane, ref Vector3 point)
    {
      float result;
      Vector3.Dot(ref plane.Normal, ref point, out result);
      return result - plane.D;
    }

    public static float DistanceBoxPoint(ref BoundingBox box, ref Vector3 point)
    {
      float num = 0.0f;
      if ((double) point.X < (double) box.Minimum.X)
        num += (float) (((double) box.Minimum.X - (double) point.X) * ((double) box.Minimum.X - (double) point.X));
      if ((double) point.X > (double) box.Maximum.X)
        num += (float) (((double) point.X - (double) box.Maximum.X) * ((double) point.X - (double) box.Maximum.X));
      if ((double) point.Y < (double) box.Minimum.Y)
        num += (float) (((double) box.Minimum.Y - (double) point.Y) * ((double) box.Minimum.Y - (double) point.Y));
      if ((double) point.Y > (double) box.Maximum.Y)
        num += (float) (((double) point.Y - (double) box.Maximum.Y) * ((double) point.Y - (double) box.Maximum.Y));
      if ((double) point.Z < (double) box.Minimum.Z)
        num += (float) (((double) box.Minimum.Z - (double) point.Z) * ((double) box.Minimum.Z - (double) point.Z));
      if ((double) point.Z > (double) box.Maximum.Z)
        num += (float) (((double) point.Z - (double) box.Maximum.Z) * ((double) point.Z - (double) box.Maximum.Z));
      return (float) Math.Sqrt((double) num);
    }

    public static float DistanceBoxBox(ref BoundingBox box1, ref BoundingBox box2)
    {
      float num1 = 0.0f;
      if ((double) box1.Minimum.X > (double) box2.Maximum.X)
      {
        float num2 = box2.Maximum.X - box1.Minimum.X;
        num1 += num2 * num2;
      }
      else if ((double) box2.Minimum.X > (double) box1.Maximum.X)
      {
        float num2 = box1.Maximum.X - box2.Minimum.X;
        num1 += num2 * num2;
      }
      if ((double) box1.Minimum.Y > (double) box2.Maximum.Y)
      {
        float num2 = box2.Maximum.Y - box1.Minimum.Y;
        num1 += num2 * num2;
      }
      else if ((double) box2.Minimum.Y > (double) box1.Maximum.Y)
      {
        float num2 = box1.Maximum.Y - box2.Minimum.Y;
        num1 += num2 * num2;
      }
      if ((double) box1.Minimum.Z > (double) box2.Maximum.Z)
      {
        float num2 = box2.Maximum.Z - box1.Minimum.Z;
        num1 += num2 * num2;
      }
      else if ((double) box2.Minimum.Z > (double) box1.Maximum.Z)
      {
        float num2 = box1.Maximum.Z - box2.Minimum.Z;
        num1 += num2 * num2;
      }
      return (float) Math.Sqrt((double) num1);
    }

    public static float DistanceSpherePoint(ref BoundingSphere sphere, ref Vector3 point)
    {
      float result;
      Vector3.Distance(ref sphere.Center, ref point, out result);
      return Math.Max(result - sphere.Radius, 0.0f);
    }

    public static float DistanceSphereSphere(ref BoundingSphere sphere1, ref BoundingSphere sphere2)
    {
      float result;
      Vector3.Distance(ref sphere1.Center, ref sphere2.Center, out result);
      return Math.Max(result - (sphere1.Radius + sphere2.Radius), 0.0f);
    }

    public static bool RayIntersectsPoint(ref Ray ray, ref Vector3 point)
    {
      Vector3 result;
      Vector3.Subtract(ref ray.Position, ref point, out result);
      float num1 = Vector3.Dot(result, ray.Direction);
      float num2 = Vector3.Dot(result, result) - 1E-06f;
      return ((double) num2 <= 0.0 || (double) num1 <= 0.0) && (double) (num1 * num1 - num2) >= 0.0;
    }

    public static bool RayIntersectsRay(ref Ray ray1, ref Ray ray2, out Vector3 point)
    {
      Vector3 result;
      Vector3.Cross(ref ray1.Direction, ref ray2.Direction, out result);
      float num1 = result.Length();
      if ((double) Math.Abs(num1) < 9.99999997475243E-07 && (double) Math.Abs(ray2.Position.X - ray1.Position.X) < 9.99999997475243E-07 && ((double) Math.Abs(ray2.Position.Y - ray1.Position.Y) < 9.99999997475243E-07 && (double) Math.Abs(ray2.Position.Z - ray1.Position.Z) < 9.99999997475243E-07))
      {
        point = Vector3.Zero;
        return true;
      }
      else
      {
        float num2 = num1 * num1;
        float num3 = ray2.Position.X - ray1.Position.X;
        float num4 = ray2.Position.Y - ray1.Position.Y;
        float num5 = ray2.Position.Z - ray1.Position.Z;
        float num6 = ray2.Direction.X;
        float num7 = ray2.Direction.Y;
        float num8 = ray2.Direction.Z;
        float num9 = result.X;
        float num10 = result.Y;
        float num11 = result.Z;
        float num12 = (float) ((double) num3 * (double) num7 * (double) num11 + (double) num4 * (double) num8 * (double) num9 + (double) num5 * (double) num6 * (double) num10 - (double) num3 * (double) num8 * (double) num10 - (double) num4 * (double) num6 * (double) num11 - (double) num5 * (double) num7 * (double) num9);
        float num13 = ray1.Direction.X;
        float num14 = ray1.Direction.Y;
        float num15 = ray1.Direction.Z;
        float num16 = (float) ((double) num3 * (double) num14 * (double) num11 + (double) num4 * (double) num15 * (double) num9 + (double) num5 * (double) num13 * (double) num10 - (double) num3 * (double) num15 * (double) num10 - (double) num4 * (double) num13 * (double) num11 - (double) num5 * (double) num14 * (double) num9);
        float num17 = num12 / num2;
        float num18 = num16 / num2;
        Vector3 vector3_1 = ray1.Position + num17 * ray1.Direction;
        Vector3 vector3_2 = ray2.Position + num18 * ray2.Direction;
        if ((double) Math.Abs(vector3_2.X - vector3_1.X) > 9.99999997475243E-07 || (double) Math.Abs(vector3_2.Y - vector3_1.Y) > 9.99999997475243E-07 || (double) Math.Abs(vector3_2.Z - vector3_1.Z) > 9.99999997475243E-07)
        {
          point = Vector3.Zero;
          return false;
        }
        else
        {
          point = vector3_1;
          return true;
        }
      }
    }

    public static bool RayIntersectsPlane(ref Ray ray, ref Plane plane, out float distance)
    {
      float result1;
      Vector3.Dot(ref plane.Normal, ref ray.Direction, out result1);
      if ((double) Math.Abs(result1) < 9.99999997475243E-07)
      {
        distance = 0.0f;
        return false;
      }
      else
      {
        float result2;
        Vector3.Dot(ref plane.Normal, ref ray.Position, out result2);
        distance = (-plane.D - result2) / result1;
        if ((double) distance < 0.0)
        {
          if ((double) distance < -9.99999997475243E-07)
          {
            distance = 0.0f;
            return false;
          }
          else
            distance = 0.0f;
        }
        return true;
      }
    }

    public static bool RayIntersectsPlane(ref Ray ray, ref Plane plane, out Vector3 point)
    {
      float distance;
      if (!Collision.RayIntersectsPlane(ref ray, ref plane, out distance))
      {
        point = Vector3.Zero;
        return false;
      }
      else
      {
        point = ray.Position + ray.Direction * distance;
        return true;
      }
    }

    public static bool RayIntersectsTriangle(ref Ray ray, ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3, out float distance)
    {
      Vector3 vector3_1;
      vector3_1.X = vertex2.X - vertex1.X;
      vector3_1.Y = vertex2.Y - vertex1.Y;
      vector3_1.Z = vertex2.Z - vertex1.Z;
      Vector3 vector3_2;
      vector3_2.X = vertex3.X - vertex1.X;
      vector3_2.Y = vertex3.Y - vertex1.Y;
      vector3_2.Z = vertex3.Z - vertex1.Z;
      Vector3 vector3_3;
      vector3_3.X = (float) ((double) ray.Direction.Y * (double) vector3_2.Z - (double) ray.Direction.Z * (double) vector3_2.Y);
      vector3_3.Y = (float) ((double) ray.Direction.Z * (double) vector3_2.X - (double) ray.Direction.X * (double) vector3_2.Z);
      vector3_3.Z = (float) ((double) ray.Direction.X * (double) vector3_2.Y - (double) ray.Direction.Y * (double) vector3_2.X);
      float num1 = (float) ((double) vector3_1.X * (double) vector3_3.X + (double) vector3_1.Y * (double) vector3_3.Y + (double) vector3_1.Z * (double) vector3_3.Z);
      if ((double) num1 > -9.99999997475243E-07 && (double) num1 < 9.99999997475243E-07)
      {
        distance = 0.0f;
        return false;
      }
      else
      {
        float num2 = 1f / num1;
        Vector3 vector3_4;
        vector3_4.X = ray.Position.X - vertex1.X;
        vector3_4.Y = ray.Position.Y - vertex1.Y;
        vector3_4.Z = ray.Position.Z - vertex1.Z;
        float num3 = (float) ((double) vector3_4.X * (double) vector3_3.X + (double) vector3_4.Y * (double) vector3_3.Y + (double) vector3_4.Z * (double) vector3_3.Z) * num2;
        if ((double) num3 < 0.0 || (double) num3 > 1.0)
        {
          distance = 0.0f;
          return false;
        }
        else
        {
          Vector3 vector3_5;
          vector3_5.X = (float) ((double) vector3_4.Y * (double) vector3_1.Z - (double) vector3_4.Z * (double) vector3_1.Y);
          vector3_5.Y = (float) ((double) vector3_4.Z * (double) vector3_1.X - (double) vector3_4.X * (double) vector3_1.Z);
          vector3_5.Z = (float) ((double) vector3_4.X * (double) vector3_1.Y - (double) vector3_4.Y * (double) vector3_1.X);
          float num4 = (float) ((double) ray.Direction.X * (double) vector3_5.X + (double) ray.Direction.Y * (double) vector3_5.Y + (double) ray.Direction.Z * (double) vector3_5.Z) * num2;
          if ((double) num4 < 0.0 || (double) num3 + (double) num4 > 1.0)
          {
            distance = 0.0f;
            return false;
          }
          else
          {
            float num5 = (float) ((double) vector3_2.X * (double) vector3_5.X + (double) vector3_2.Y * (double) vector3_5.Y + (double) vector3_2.Z * (double) vector3_5.Z) * num2;
            if ((double) num5 < 0.0)
            {
              distance = 0.0f;
              return false;
            }
            else
            {
              distance = num5;
              return true;
            }
          }
        }
      }
    }

    public static bool RayIntersectsTriangle(ref Ray ray, ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3, out Vector3 point)
    {
      float distance;
      if (!Collision.RayIntersectsTriangle(ref ray, ref vertex1, ref vertex2, ref vertex3, out distance))
      {
        point = Vector3.Zero;
        return false;
      }
      else
      {
        point = ray.Position + ray.Direction * distance;
        return true;
      }
    }

    public static bool RayIntersectsBox(ref Ray ray, ref BoundingBox box, out float distance)
    {
      distance = 0.0f;
      float val2 = float.MaxValue;
      if ((double) Math.Abs(ray.Direction.X) < 9.99999997475243E-07)
      {
        if ((double) ray.Position.X < (double) box.Minimum.X || (double) ray.Position.X > (double) box.Maximum.X)
        {
          distance = 0.0f;
          return false;
        }
      }
      else
      {
        float num1 = 1f / ray.Direction.X;
        float val1_1 = (box.Minimum.X - ray.Position.X) * num1;
        float val1_2 = (box.Maximum.X - ray.Position.X) * num1;
        if ((double) val1_1 > (double) val1_2)
        {
          float num2 = val1_1;
          val1_1 = val1_2;
          val1_2 = num2;
        }
        distance = Math.Max(val1_1, distance);
        val2 = Math.Min(val1_2, val2);
        if ((double) distance > (double) val2)
        {
          distance = 0.0f;
          return false;
        }
      }
      if ((double) Math.Abs(ray.Direction.Y) < 9.99999997475243E-07)
      {
        if ((double) ray.Position.Y < (double) box.Minimum.Y || (double) ray.Position.Y > (double) box.Maximum.Y)
        {
          distance = 0.0f;
          return false;
        }
      }
      else
      {
        float num1 = 1f / ray.Direction.Y;
        float val1_1 = (box.Minimum.Y - ray.Position.Y) * num1;
        float val1_2 = (box.Maximum.Y - ray.Position.Y) * num1;
        if ((double) val1_1 > (double) val1_2)
        {
          float num2 = val1_1;
          val1_1 = val1_2;
          val1_2 = num2;
        }
        distance = Math.Max(val1_1, distance);
        val2 = Math.Min(val1_2, val2);
        if ((double) distance > (double) val2)
        {
          distance = 0.0f;
          return false;
        }
      }
      if ((double) Math.Abs(ray.Direction.Z) < 9.99999997475243E-07)
      {
        if ((double) ray.Position.Z < (double) box.Minimum.Z || (double) ray.Position.Z > (double) box.Maximum.Z)
        {
          distance = 0.0f;
          return false;
        }
      }
      else
      {
        float num1 = 1f / ray.Direction.Z;
        float val1_1 = (box.Minimum.Z - ray.Position.Z) * num1;
        float val1_2 = (box.Maximum.Z - ray.Position.Z) * num1;
        if ((double) val1_1 > (double) val1_2)
        {
          float num2 = val1_1;
          val1_1 = val1_2;
          val1_2 = num2;
        }
        distance = Math.Max(val1_1, distance);
        float num3 = Math.Min(val1_2, val2);
        if ((double) distance > (double) num3)
        {
          distance = 0.0f;
          return false;
        }
      }
      return true;
    }

    public static bool RayIntersectsBox(ref Ray ray, ref BoundingBox box, out Vector3 point)
    {
      float distance;
      if (!Collision.RayIntersectsBox(ref ray, ref box, out distance))
      {
        point = Vector3.Zero;
        return false;
      }
      else
      {
        point = ray.Position + ray.Direction * distance;
        return true;
      }
    }

    public static bool RayIntersectsSphere(ref Ray ray, ref BoundingSphere sphere, out float distance)
    {
      Vector3 result;
      Vector3.Subtract(ref ray.Position, ref sphere.Center, out result);
      float num1 = Vector3.Dot(result, ray.Direction);
      float num2 = Vector3.Dot(result, result) - sphere.Radius * sphere.Radius;
      if ((double) num2 > 0.0 && (double) num1 > 0.0)
      {
        distance = 0.0f;
        return false;
      }
      else
      {
        float num3 = num1 * num1 - num2;
        if ((double) num3 < 0.0)
        {
          distance = 0.0f;
          return false;
        }
        else
        {
          distance = -num1 - (float) Math.Sqrt((double) num3);
          if ((double) distance < 0.0)
            distance = 0.0f;
          return true;
        }
      }
    }

    public static bool RayIntersectsSphere(ref Ray ray, ref BoundingSphere sphere, out Vector3 point)
    {
      float distance;
      if (!Collision.RayIntersectsSphere(ref ray, ref sphere, out distance))
      {
        point = Vector3.Zero;
        return false;
      }
      else
      {
        point = ray.Position + ray.Direction * distance;
        return true;
      }
    }

    public static PlaneIntersectionType PlaneIntersectsPoint(ref Plane plane, ref Vector3 point)
    {
      float result;
      Vector3.Dot(ref plane.Normal, ref point, out result);
      float num = result + plane.D;
      if ((double) num > 0.0)
        return PlaneIntersectionType.Front;
      return (double) num < 0.0 ? PlaneIntersectionType.Back : PlaneIntersectionType.Intersecting;
    }

    public static bool PlaneIntersectsPlane(ref Plane plane1, ref Plane plane2)
    {
      Vector3 result1;
      Vector3.Cross(ref plane1.Normal, ref plane2.Normal, out result1);
      float result2;
      Vector3.Dot(ref result1, ref result1, out result2);
      return (double) Math.Abs(result2) >= 9.99999997475243E-07;
    }

    public static bool PlaneIntersectsPlane(ref Plane plane1, ref Plane plane2, out Ray line)
    {
      Vector3 result1;
      Vector3.Cross(ref plane1.Normal, ref plane2.Normal, out result1);
      float result2;
      Vector3.Dot(ref result1, ref result1, out result2);
      if ((double) Math.Abs(result2) < 9.99999997475243E-07)
      {
        line = new Ray();
        return false;
      }
      else
      {
        Vector3 left = plane1.D * plane2.Normal - plane2.D * plane1.Normal;
        Vector3 result3;
        Vector3.Cross(ref left, ref result1, out result3);
        line.Position = result3;
        line.Direction = result1;
        line.Direction.Normalize();
        return true;
      }
    }

    public static PlaneIntersectionType PlaneIntersectsTriangle(ref Plane plane, ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
    {
      PlaneIntersectionType intersectionType1 = Collision.PlaneIntersectsPoint(ref plane, ref vertex1);
      PlaneIntersectionType intersectionType2 = Collision.PlaneIntersectsPoint(ref plane, ref vertex2);
      PlaneIntersectionType intersectionType3 = Collision.PlaneIntersectsPoint(ref plane, ref vertex3);
      if (intersectionType1 == PlaneIntersectionType.Front && intersectionType2 == PlaneIntersectionType.Front && intersectionType3 == PlaneIntersectionType.Front)
        return PlaneIntersectionType.Front;
      return intersectionType1 == PlaneIntersectionType.Back && intersectionType2 == PlaneIntersectionType.Back && intersectionType3 == PlaneIntersectionType.Back ? PlaneIntersectionType.Back : PlaneIntersectionType.Intersecting;
    }

    public static PlaneIntersectionType PlaneIntersectsBox(ref Plane plane, ref BoundingBox box)
    {
      Vector3 right1;
      right1.X = (double) plane.Normal.X >= 0.0 ? box.Minimum.X : box.Maximum.X;
      right1.Y = (double) plane.Normal.Y >= 0.0 ? box.Minimum.Y : box.Maximum.Y;
      right1.Z = (double) plane.Normal.Z >= 0.0 ? box.Minimum.Z : box.Maximum.Z;
      Vector3 right2;
      right2.X = (double) plane.Normal.X >= 0.0 ? box.Maximum.X : box.Minimum.X;
      right2.Y = (double) plane.Normal.Y >= 0.0 ? box.Maximum.Y : box.Minimum.Y;
      right2.Z = (double) plane.Normal.Z >= 0.0 ? box.Maximum.Z : box.Minimum.Z;
      float result;
      Vector3.Dot(ref plane.Normal, ref right1, out result);
      if ((double) result + (double) plane.D > 0.0)
        return PlaneIntersectionType.Front;
      return (double) Vector3.Dot(plane.Normal, right2) + (double) plane.D < 0.0 ? PlaneIntersectionType.Back : PlaneIntersectionType.Intersecting;
    }

    public static PlaneIntersectionType PlaneIntersectsSphere(ref Plane plane, ref BoundingSphere sphere)
    {
      float result;
      Vector3.Dot(ref plane.Normal, ref sphere.Center, out result);
      float num = result + plane.D;
      if ((double) num > (double) sphere.Radius)
        return PlaneIntersectionType.Front;
      return (double) num < -(double) sphere.Radius ? PlaneIntersectionType.Back : PlaneIntersectionType.Intersecting;
    }

    public static bool BoxIntersectsBox(ref BoundingBox box1, ref BoundingBox box2)
    {
      return (double) box1.Minimum.X <= (double) box2.Maximum.X && (double) box2.Minimum.X <= (double) box1.Maximum.X && ((double) box1.Minimum.Y <= (double) box2.Maximum.Y && (double) box2.Minimum.Y <= (double) box1.Maximum.Y) && ((double) box1.Minimum.Z <= (double) box2.Maximum.Z && (double) box2.Minimum.Z <= (double) box1.Maximum.Z);
    }

    public static bool BoxIntersectsSphere(ref BoundingBox box, ref BoundingSphere sphere)
    {
      Vector3 result;
      Vector3.Clamp(ref sphere.Center, ref box.Minimum, ref box.Maximum, out result);
      return (double) Vector3.DistanceSquared(sphere.Center, result) <= (double) sphere.Radius * (double) sphere.Radius;
    }

    public static bool SphereIntersectsTriangle(ref BoundingSphere sphere, ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
    {
      Vector3 result1;
      Collision.ClosestPointPointTriangle(ref sphere.Center, ref vertex1, ref vertex2, ref vertex3, out result1);
      Vector3 vector3 = result1 - sphere.Center;
      float result2;
      Vector3.Dot(ref vector3, ref vector3, out result2);
      return (double) result2 <= (double) sphere.Radius * (double) sphere.Radius;
    }

    public static bool SphereIntersectsSphere(ref BoundingSphere sphere1, ref BoundingSphere sphere2)
    {
      float num = sphere1.Radius + sphere2.Radius;
      return (double) Vector3.DistanceSquared(sphere1.Center, sphere2.Center) <= (double) num * (double) num;
    }

    public static ContainmentType BoxContainsPoint(ref BoundingBox box, ref Vector3 point)
    {
      return (double) box.Minimum.X <= (double) point.X && (double) box.Maximum.X >= (double) point.X && ((double) box.Minimum.Y <= (double) point.Y && (double) box.Maximum.Y >= (double) point.Y) && ((double) box.Minimum.Z <= (double) point.Z && (double) box.Maximum.Z >= (double) point.Z) ? ContainmentType.Contains : ContainmentType.Disjoint;
    }

    public static ContainmentType BoxContainsBox(ref BoundingBox box1, ref BoundingBox box2)
    {
      if ((double) box1.Maximum.X < (double) box2.Minimum.X || (double) box1.Minimum.X > (double) box2.Maximum.X || ((double) box1.Maximum.Y < (double) box2.Minimum.Y || (double) box1.Minimum.Y > (double) box2.Maximum.Y) || ((double) box1.Maximum.Z < (double) box2.Minimum.Z || (double) box1.Minimum.Z > (double) box2.Maximum.Z))
        return ContainmentType.Disjoint;
      return (double) box1.Minimum.X <= (double) box2.Minimum.X && (double) box2.Maximum.X <= (double) box1.Maximum.X && ((double) box1.Minimum.Y <= (double) box2.Minimum.Y && (double) box2.Maximum.Y <= (double) box1.Maximum.Y) && ((double) box1.Minimum.Z <= (double) box2.Minimum.Z && (double) box2.Maximum.Z <= (double) box1.Maximum.Z) ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public static ContainmentType BoxContainsSphere(ref BoundingBox box, ref BoundingSphere sphere)
    {
      Vector3 result;
      Vector3.Clamp(ref sphere.Center, ref box.Minimum, ref box.Maximum, out result);
      if ((double) Vector3.DistanceSquared(sphere.Center, result) > (double) sphere.Radius * (double) sphere.Radius)
        return ContainmentType.Disjoint;
      return (double) box.Minimum.X + (double) sphere.Radius <= (double) sphere.Center.X && (double) sphere.Center.X <= (double) box.Maximum.X - (double) sphere.Radius && ((double) box.Maximum.X - (double) box.Minimum.X > (double) sphere.Radius && (double) box.Minimum.Y + (double) sphere.Radius <= (double) sphere.Center.Y) && ((double) sphere.Center.Y <= (double) box.Maximum.Y - (double) sphere.Radius && (double) box.Maximum.Y - (double) box.Minimum.Y > (double) sphere.Radius && ((double) box.Minimum.Z + (double) sphere.Radius <= (double) sphere.Center.Z && (double) sphere.Center.Z <= (double) box.Maximum.Z - (double) sphere.Radius)) && (double) box.Maximum.X - (double) box.Minimum.X > (double) sphere.Radius ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public static ContainmentType SphereContainsPoint(ref BoundingSphere sphere, ref Vector3 point)
    {
      return (double) Vector3.DistanceSquared(point, sphere.Center) <= (double) sphere.Radius * (double) sphere.Radius ? ContainmentType.Contains : ContainmentType.Disjoint;
    }

    public static ContainmentType SphereContainsTriangle(ref BoundingSphere sphere, ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
    {
      if (Collision.SphereContainsPoint(ref sphere, ref vertex1) == ContainmentType.Contains && Collision.SphereContainsPoint(ref sphere, ref vertex2) == ContainmentType.Contains && Collision.SphereContainsPoint(ref sphere, ref vertex3) == ContainmentType.Contains)
        return ContainmentType.Contains;
      return Collision.SphereIntersectsTriangle(ref sphere, ref vertex1, ref vertex2, ref vertex3) ? ContainmentType.Intersects : ContainmentType.Disjoint;
    }

    public static ContainmentType SphereContainsBox(ref BoundingSphere sphere, ref BoundingBox box)
    {
      if (!Collision.BoxIntersectsSphere(ref box, ref sphere))
        return ContainmentType.Disjoint;
      float num = sphere.Radius * sphere.Radius;
      Vector3 vector3;
      vector3.X = sphere.Center.X - box.Minimum.X;
      vector3.Y = sphere.Center.Y - box.Maximum.Y;
      vector3.Z = sphere.Center.Z - box.Maximum.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = sphere.Center.X - box.Maximum.X;
      vector3.Y = sphere.Center.Y - box.Maximum.Y;
      vector3.Z = sphere.Center.Z - box.Maximum.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = sphere.Center.X - box.Maximum.X;
      vector3.Y = sphere.Center.Y - box.Minimum.Y;
      vector3.Z = sphere.Center.Z - box.Maximum.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = sphere.Center.X - box.Minimum.X;
      vector3.Y = sphere.Center.Y - box.Minimum.Y;
      vector3.Z = sphere.Center.Z - box.Maximum.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = sphere.Center.X - box.Minimum.X;
      vector3.Y = sphere.Center.Y - box.Maximum.Y;
      vector3.Z = sphere.Center.Z - box.Minimum.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = sphere.Center.X - box.Maximum.X;
      vector3.Y = sphere.Center.Y - box.Maximum.Y;
      vector3.Z = sphere.Center.Z - box.Minimum.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = sphere.Center.X - box.Maximum.X;
      vector3.Y = sphere.Center.Y - box.Minimum.Y;
      vector3.Z = sphere.Center.Z - box.Minimum.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = sphere.Center.X - box.Minimum.X;
      vector3.Y = sphere.Center.Y - box.Minimum.Y;
      vector3.Z = sphere.Center.Z - box.Minimum.Z;
      return (double) vector3.LengthSquared() > (double) num ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public static ContainmentType SphereContainsSphere(ref BoundingSphere sphere1, ref BoundingSphere sphere2)
    {
      float num = Vector3.Distance(sphere1.Center, sphere2.Center);
      if ((double) sphere1.Radius + (double) sphere2.Radius < (double) num)
        return ContainmentType.Disjoint;
      return (double) sphere1.Radius - (double) sphere2.Radius < (double) num ? ContainmentType.Intersects : ContainmentType.Contains;
    }
  }
}
