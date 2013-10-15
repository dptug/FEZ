// Type: Microsoft.Xna.Framework.Ray
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
      if (!(obj is Ray))
        return false;
      else
        return this.Equals((Ray) obj);
    }

    public bool Equals(Ray other)
    {
      if (this.Position.Equals(other.Position))
        return this.Direction.Equals(other.Direction);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Position.GetHashCode() ^ this.Direction.GetHashCode();
    }

    public float? Intersects(BoundingBox box)
    {
      float? nullable1 = new float?();
      float? nullable2 = new float?();
      if ((double) Math.Abs(this.Direction.X) < 9.99999997475243E-07)
      {
        if ((double) this.Position.X < (double) box.Min.X || (double) this.Position.X > (double) box.Max.X)
          return new float?();
      }
      else
      {
        nullable1 = new float?((box.Min.X - this.Position.X) / this.Direction.X);
        nullable2 = new float?((box.Max.X - this.Position.X) / this.Direction.X);
        float? nullable3 = nullable1;
        float? nullable4 = nullable2;
        if (((double) nullable3.GetValueOrDefault() <= (double) nullable4.GetValueOrDefault() ? 0 : (nullable3.HasValue & nullable4.HasValue ? 1 : 0)) != 0)
        {
          float? nullable5 = nullable1;
          nullable1 = nullable2;
          nullable2 = nullable5;
        }
      }
      if ((double) Math.Abs(this.Direction.Y) < 9.99999997475243E-07)
      {
        if ((double) this.Position.Y < (double) box.Min.Y || (double) this.Position.Y > (double) box.Max.Y)
          return new float?();
      }
      else
      {
        float num1 = (box.Min.Y - this.Position.Y) / this.Direction.Y;
        float num2 = (box.Max.Y - this.Position.Y) / this.Direction.Y;
        if ((double) num1 > (double) num2)
        {
          float num3 = num1;
          num1 = num2;
          num2 = num3;
        }
        if (nullable1.HasValue)
        {
          float? nullable3 = nullable1;
          float num3 = num2;
          if (((double) nullable3.GetValueOrDefault() <= (double) num3 ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
            goto label_14;
        }
        if (nullable2.HasValue)
        {
          float num3 = num1;
          float? nullable3 = nullable2;
          if (((double) num3 <= (double) nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
            goto label_14;
        }
        if (nullable1.HasValue)
        {
          float num3 = num1;
          float? nullable3 = nullable1;
          if (((double) num3 <= (double) nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) == 0)
            goto label_18;
        }
        nullable1 = new float?(num1);
label_18:
        if (nullable2.HasValue)
        {
          float num3 = num2;
          float? nullable3 = nullable2;
          if (((double) num3 >= (double) nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) == 0)
            goto label_21;
        }
        nullable2 = new float?(num2);
        goto label_21;
label_14:
        return new float?();
      }
label_21:
      if ((double) Math.Abs(this.Direction.Z) < 9.99999997475243E-07)
      {
        if ((double) this.Position.Z < (double) box.Min.Z || (double) this.Position.Z > (double) box.Max.Z)
          return new float?();
      }
      else
      {
        float num1 = (box.Min.Z - this.Position.Z) / this.Direction.Z;
        float num2 = (box.Max.Z - this.Position.Z) / this.Direction.Z;
        if ((double) num1 > (double) num2)
        {
          float num3 = num1;
          num1 = num2;
          num2 = num3;
        }
        if (nullable1.HasValue)
        {
          float? nullable3 = nullable1;
          float num3 = num2;
          if (((double) nullable3.GetValueOrDefault() <= (double) num3 ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
            goto label_30;
        }
        if (nullable2.HasValue)
        {
          float num3 = num1;
          float? nullable3 = nullable2;
          if (((double) num3 <= (double) nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
            goto label_30;
        }
        if (nullable1.HasValue)
        {
          float num3 = num1;
          float? nullable3 = nullable1;
          if (((double) num3 <= (double) nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) == 0)
            goto label_34;
        }
        nullable1 = new float?(num1);
label_34:
        if (nullable2.HasValue)
        {
          float num3 = num2;
          float? nullable3 = nullable2;
          if (((double) num3 >= (double) nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) == 0)
            goto label_37;
        }
        nullable2 = new float?(num2);
        goto label_37;
label_30:
        return new float?();
      }
label_37:
      if (nullable1.HasValue)
      {
        float? nullable3 = nullable1;
        if (((double) nullable3.GetValueOrDefault() >= 0.0 ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
        {
          float? nullable4 = nullable2;
          if (((double) nullable4.GetValueOrDefault() <= 0.0 ? 0 : (nullable4.HasValue ? 1 : 0)) != 0)
            return new float?(0.0f);
        }
      }
      float? nullable6 = nullable1;
      if (((double) nullable6.GetValueOrDefault() >= 0.0 ? 0 : (nullable6.HasValue ? 1 : 0)) != 0)
        return new float?();
      else
        return nullable1;
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
        float? nullable1 = result;
        if (((double) nullable1.GetValueOrDefault() >= 0.0 ? 0 : (nullable1.HasValue ? 1 : 0)) == 0)
          return;
        float? nullable2 = result;
        if (((double) nullable2.GetValueOrDefault() >= -9.99999974737875E-06 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
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
