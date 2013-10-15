// Type: FezEngine.Structure.TrileEmplacement
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
  public struct TrileEmplacement : IEquatable<TrileEmplacement>, IComparable<TrileEmplacement>
  {
    public int X;
    public int Y;
    public int Z;

    [Serialization(Ignore = true)]
    public Vector3 AsVector
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

    public TrileEmplacement(Vector3 position)
    {
      this.X = FezMath.Round((double) position.X);
      this.Y = FezMath.Round((double) position.Y);
      this.Z = FezMath.Round((double) position.Z);
    }

    public TrileEmplacement(int x, int y, int z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public static bool operator ==(TrileEmplacement lhs, TrileEmplacement rhs)
    {
      if ((ValueType) lhs != null)
        return lhs.Equals(rhs);
      else
        return (ValueType) rhs == null;
    }

    public static bool operator !=(TrileEmplacement lhs, TrileEmplacement rhs)
    {
      return !(lhs == rhs);
    }

    public static TrileEmplacement operator +(TrileEmplacement lhs, TrileEmplacement rhs)
    {
      return new TrileEmplacement(lhs.AsVector + rhs.AsVector);
    }

    public static TrileEmplacement operator -(TrileEmplacement lhs, TrileEmplacement rhs)
    {
      return new TrileEmplacement(lhs.AsVector - rhs.AsVector);
    }

    public static TrileEmplacement operator +(TrileEmplacement lhs, Vector3 rhs)
    {
      return new TrileEmplacement(lhs.AsVector + rhs);
    }

    public static TrileEmplacement operator -(TrileEmplacement lhs, Vector3 rhs)
    {
      return new TrileEmplacement(lhs.AsVector - rhs);
    }

    public static TrileEmplacement operator /(TrileEmplacement lhs, float rhs)
    {
      return new TrileEmplacement(lhs.AsVector / rhs);
    }

    public static TrileEmplacement operator *(TrileEmplacement lhs, Vector3 rhs)
    {
      return new TrileEmplacement(lhs.AsVector * rhs);
    }

    public TrileEmplacement GetOffset(Vector3 vector)
    {
      return new TrileEmplacement(this.X + (int) vector.X, this.Y + (int) vector.Y, this.Z + (int) vector.Z);
    }

    public TrileEmplacement GetOffset(int offsetX, int offsetY, int offsetZ)
    {
      return new TrileEmplacement(this.X + offsetX, this.Y + offsetY, this.Z + offsetZ);
    }

    public override bool Equals(object obj)
    {
      if (obj is TrileEmplacement)
        return this.Equals((TrileEmplacement) obj);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.X ^ this.Y << 10 ^ this.Z << 20;
    }

    public override string ToString()
    {
      return string.Format("({0}, {1}, {2})", (object) this.X, (object) this.Y, (object) this.Z);
    }

    public void TraverseInto(FaceOrientation face)
    {
      switch (face)
      {
        case FaceOrientation.Left:
          --this.X;
          break;
        case FaceOrientation.Down:
          --this.Y;
          break;
        case FaceOrientation.Back:
          --this.Z;
          break;
        case FaceOrientation.Top:
          ++this.Y;
          break;
        case FaceOrientation.Front:
          ++this.Z;
          break;
        default:
          ++this.X;
          break;
      }
    }

    public TrileEmplacement GetTraversal(ref FaceOrientation face)
    {
      switch (face)
      {
        case FaceOrientation.Left:
          return new TrileEmplacement(this.X - 1, this.Y, this.Z);
        case FaceOrientation.Down:
          return new TrileEmplacement(this.X, this.Y - 1, this.Z);
        case FaceOrientation.Back:
          return new TrileEmplacement(this.X, this.Y, this.Z - 1);
        case FaceOrientation.Top:
          return new TrileEmplacement(this.X, this.Y + 1, this.Z);
        case FaceOrientation.Front:
          return new TrileEmplacement(this.X, this.Y, this.Z + 1);
        default:
          return new TrileEmplacement(this.X + 1, this.Y, this.Z);
      }
    }

    public bool Equals(TrileEmplacement other)
    {
      if (other.X == this.X && other.Y == this.Y)
        return other.Z == this.Z;
      else
        return false;
    }

    public int CompareTo(TrileEmplacement other)
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
