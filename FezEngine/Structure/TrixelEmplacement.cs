// Type: FezEngine.Structure.TrixelEmplacement
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Structure
{
  [TypeSerialization(FlattenToList = true)]
  public struct TrixelEmplacement : IEquatable<TrixelEmplacement>, IComparable<TrixelEmplacement>
  {
    public int X;
    public int Y;
    public int Z;

    [Serialization(Ignore = true)]
    public Vector3 Position
    {
      get
      {
        return new Vector3((float) this.X, (float) this.Y, (float) this.Z);
      }
      set
      {
        this.X = FezMath.Round((double) value.X);
        this.Y = FezMath.Round((double) value.Y);
        this.Z = FezMath.Round((double) value.Z);
      }
    }

    public TrixelEmplacement(TrixelEmplacement other)
    {
      this.X = other.X;
      this.Y = other.Y;
      this.Z = other.Z;
    }

    public TrixelEmplacement(Vector3 position)
    {
      this.X = FezMath.Round((double) position.X);
      this.Y = FezMath.Round((double) position.Y);
      this.Z = FezMath.Round((double) position.Z);
    }

    public TrixelEmplacement(int x, int y, int z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public static bool operator ==(TrixelEmplacement lhs, TrixelEmplacement rhs)
    {
      return lhs.Equals(rhs);
    }

    public static bool operator !=(TrixelEmplacement lhs, TrixelEmplacement rhs)
    {
      return !(lhs == rhs);
    }

    public static TrixelEmplacement operator +(TrixelEmplacement lhs, TrixelEmplacement rhs)
    {
      return new TrixelEmplacement(lhs.Position + rhs.Position);
    }

    public static TrixelEmplacement operator -(TrixelEmplacement lhs, TrixelEmplacement rhs)
    {
      return new TrixelEmplacement(lhs.Position - rhs.Position);
    }

    public static TrixelEmplacement operator +(TrixelEmplacement lhs, Vector3 rhs)
    {
      return new TrixelEmplacement(lhs.Position + rhs);
    }

    public static TrixelEmplacement operator -(TrixelEmplacement lhs, Vector3 rhs)
    {
      return new TrixelEmplacement(lhs.Position - rhs);
    }

    public static TrixelEmplacement operator /(TrixelEmplacement lhs, float rhs)
    {
      return new TrixelEmplacement(lhs.Position / rhs);
    }

    public static bool operator <(TrixelEmplacement lhs, TrixelEmplacement rhs)
    {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >(TrixelEmplacement lhs, TrixelEmplacement rhs)
    {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator <(TrixelEmplacement lhs, Vector3 rhs)
    {
      return lhs.CompareTo(new TrixelEmplacement(rhs)) < 0;
    }

    public static bool operator >(TrixelEmplacement lhs, Vector3 rhs)
    {
      return lhs.CompareTo(new TrixelEmplacement(rhs)) > 0;
    }

    public void Offset(int offsetX, int offsetY, int offsetZ)
    {
      this.X += offsetX;
      this.Y += offsetY;
      this.Z += offsetZ;
    }

    public override bool Equals(object obj)
    {
      if (obj is TrixelEmplacement)
        return this.Equals((TrixelEmplacement) obj);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.X ^ this.Y << 10 ^ this.Z << 20;
    }

    public override string ToString()
    {
      return string.Format("{{X:{0}, Y:{1}, Z:{2}}}", (object) this.X, (object) this.Y, (object) this.Z);
    }

    public TrixelEmplacement GetTraversal(FaceOrientation face)
    {
      return new TrixelEmplacement(this.Position + FezMath.AsVector(face));
    }

    public void TraverseInto(FaceOrientation face)
    {
      this.Position += FezMath.AsVector(face);
    }

    public bool IsNeighbor(TrixelEmplacement other)
    {
      if (Math.Abs(this.X - other.X) != 1 && Math.Abs(this.Y - other.Y) != 1)
        return Math.Abs(this.Z - other.Z) == 1;
      else
        return true;
    }

    public bool Equals(TrixelEmplacement other)
    {
      if ((ValueType) other != null && other.X == this.X && other.Y == this.Y)
        return other.Z == this.Z;
      else
        return false;
    }

    public int CompareTo(TrixelEmplacement other)
    {
      int num = this.X.CompareTo(other.X);
      if (num == 0)
      {
        num = this.Y.CompareTo(other.Y);
        if (num == 0)
          num = this.Z.CompareTo(other.Z);
      }
      return num;
    }
  }
}
