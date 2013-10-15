// Type: FezEngine.Structure.TrileFace
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Structure
{
  public class TrileFace : IEquatable<TrileFace>
  {
    public TrileEmplacement Id;

    public FaceOrientation Face { get; set; }

    public TrileFace()
    {
      this.Id = new TrileEmplacement();
    }

    public static bool operator ==(TrileFace lhs, TrileFace rhs)
    {
      if (lhs != null)
        return lhs.Equals(rhs);
      else
        return rhs == null;
    }

    public static bool operator !=(TrileFace lhs, TrileFace rhs)
    {
      return !(lhs == rhs);
    }

    public static TrileFace operator +(TrileFace lhs, Vector3 rhs)
    {
      return new TrileFace()
      {
        Id = lhs.Id + rhs,
        Face = lhs.Face
      };
    }

    public static TrileFace operator -(TrileFace lhs, Vector3 rhs)
    {
      return new TrileFace()
      {
        Id = lhs.Id - rhs,
        Face = lhs.Face
      };
    }

    public override int GetHashCode()
    {
      return this.Id.GetHashCode() ^ this.Face.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is TrileFace)
        return this.Equals(obj as TrileFace);
      else
        return false;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public bool Equals(TrileFace other)
    {
      if (other != null && other.Id == this.Id)
        return other.Face == this.Face;
      else
        return false;
    }
  }
}
