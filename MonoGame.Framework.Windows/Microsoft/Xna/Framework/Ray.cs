// Type: Microsoft.Xna.Framework.Ray
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework
{
  [Serializable]
  public struct Ray : IEquatable<Ray>
  {
    public Vector3 Direction;
    public Vector3 Position;

    public Ray(Vector3 position, Vector3 direction)
    {
      this.Position = position;
      this.Direction = direction;
    }

    public static bool operator !=(Ray a, Ray b)
    {
      return !a.Equals(b);
    }

    public static bool operator ==(Ray a, Ray b)
    {
      return a.Equals(b);
    }

    public override bool Equals(object obj)
    {
      return obj is Ray && this.Equals((Ray) obj);
    }

    public bool Equals(Ray other)
    {
      return this.Position.Equals(other.Position) && this.Direction.Equals(other.Direction);
    }

    public override int GetHashCode()
    {
      return this.Position.GetHashCode() ^ this.Direction.GetHashCode();
    }

    public float? Intersects(BoundingBox box)
    {
      if ((double) this.Position.X >= (double) box.Min.X && (double) this.Position.X <= (double) box.Max.X && ((double) this.Position.Y >= (double) box.Min.Y && (double) this.Position.Y <= (double) box.Max.Y) && (double) this.Position.Z >= (double) box.Min.Z && (double) this.Position.Z <= (double) box.Max.Z)
        return new float?(0.0f);
      Vector3 vector3 = new Vector3(-1f);
      if ((double) this.Position.X < (double) box.Min.X && (double) this.Direction.X != 0.0)
        vector3.X = (box.Min.X - this.Position.X) / this.Direction.X;
      else if ((double) this.Position.X > (double) box.Max.X && (double) this.Direction.X != 0.0)
        vector3.X = (box.Max.X - this.Position.X) / this.Direction.X;
      if ((double) this.Position.Y < (double) box.Min.Y && (double) this.Direction.Y != 0.0)
        vector3.Y = (box.Min.Y - this.Position.Y) / this.Direction.Y;
      else if ((double) this.Position.Y > (double) box.Max.Y && (double) this.Direction.Y != 0.0)
        vector3.Y = (box.Max.Y - this.Position.Y) / this.Direction.Y;
      if ((double) this.Position.Z < (double) box.Min.Z && (double) this.Direction.Z != 0.0)
        vector3.Z = (box.Min.Z - this.Position.Z) / this.Direction.Z;
      else if ((double) this.Position.Z > (double) box.Max.Z && (double) this.Direction.Z != 0.0)
        vector3.Z = (box.Max.Z - this.Position.Z) / this.Direction.Z;
      if ((double) vector3.X >= (double) vector3.Y && (double) vector3.X >= (double) vector3.Z)
      {
        if ((double) vector3.X < 0.0)
          return new float?();
        float num1 = this.Position.Z + vector3.X * this.Direction.Z;
        if ((double) num1 < (double) box.Min.Z || (double) num1 > (double) box.Max.Z)
          return new float?();
        float num2 = this.Position.Y + vector3.X * this.Direction.Y;
        if ((double) num2 < (double) box.Min.Y || (double) num2 > (double) box.Max.Y)
          return new float?();
        else
          return new float?(vector3.X);
      }
      else if ((double) vector3.Y >= (double) vector3.X && (double) vector3.Y >= (double) vector3.Z)
      {
        if ((double) vector3.Y < 0.0)
          return new float?();
        float num1 = this.Position.Z + vector3.Y * this.Direction.Z;
        if ((double) num1 < (double) box.Min.Z || (double) num1 > (double) box.Max.Z)
          return new float?();
        float num2 = this.Position.X + vector3.Y * this.Direction.X;
        if ((double) num2 < (double) box.Min.X || (double) num2 > (double) box.Max.X)
          return new float?();
        else
          return new float?(vector3.Y);
      }
      else
      {
        if ((double) vector3.Z < 0.0)
          return new float?();
        float num1 = this.Position.X + vector3.Z * this.Direction.X;
        if ((double) num1 < (double) box.Min.X || (double) num1 > (double) box.Max.X)
          return new float?();
        float num2 = this.Position.Y + vector3.Z * this.Direction.Y;
        if ((double) num2 < (double) box.Min.Y || (double) num2 > (double) box.Max.Y)
          return new float?();
        else
          return new float?(vector3.Z);
      }
    }

    public void Intersects(ref BoundingBox box, out float? result)
    {
      result = this.Intersects(box);
    }

    public float? Intersects(BoundingFrustum frustum)
    {
      if (frustum == (BoundingFrustum) null)
        throw new ArgumentNullException("frustum");
      else
        return frustum.Intersects(this);
    }

    public float? Intersects(BoundingSphere sphere)
    {
      float? result;
      this.Intersects(ref sphere, out result);
      return result;
    }

    public float? Intersects(Plane plane)
    {
      float? result;
      this.Intersects(ref plane, out result);
      return result;
    }

    public void Intersects(ref Plane plane, out float? result)
    {
      float num = Vector3.Dot(this.Direction, plane.Normal);
      if ((double) Math.Abs(num) < 9.99999974737875E-06)
      {
        result = new float?();
      }
      else
      {
        result = new float?((-plane.D - Vector3.Dot(plane.Normal, this.Position)) / num);
        float? nullable = result;
        if (((double) nullable.GetValueOrDefault() >= 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
          return;
        nullable = result;
        if (((double) nullable.GetValueOrDefault() >= -9.99999974737875E-06 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
          result = new float?();
        else
          result = new float?(0.0f);
      }
    }

    public void Intersects(ref BoundingSphere sphere, out float? result)
    {
      Vector3 vector2 = sphere.Center - this.Position;
      float num1 = vector2.LengthSquared();
      float num2 = sphere.Radius * sphere.Radius;
      if ((double) num1 < (double) num2)
      {
        result = new float?(0.0f);
      }
      else
      {
        float result1;
        Vector3.Dot(ref this.Direction, ref vector2, out result1);
        if ((double) result1 < 0.0)
        {
          result = new float?();
        }
        else
        {
          float num3 = num2 + result1 * result1 - num1;
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          float?& local = @result;
          float? nullable1;
          if ((double) num3 >= 0.0)
          {
            float num4 = result1;
            float? nullable2 = new float?((float) Math.Sqrt((double) num3));
            nullable1 = nullable2.HasValue ? new float?(num4 - nullable2.GetValueOrDefault()) : new float?();
          }
          else
            nullable1 = new float?();
          // ISSUE: explicit reference operation
          ^local = nullable1;
        }
      }
    }

    public override string ToString()
    {
      return string.Format("{{Position:{0} Direction:{1}}}", (object) this.Position.ToString(), (object) this.Direction.ToString());
    }
  }
}
