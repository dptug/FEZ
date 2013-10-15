// Type: FezEngine.Structure.InstanceFace
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using System;

namespace FezEngine.Structure
{
  public class InstanceFace : IEquatable<InstanceFace>
  {
    public TrileInstance Instance { get; set; }

    public FaceOrientation Face { get; set; }

    public InstanceFace()
    {
    }

    public InstanceFace(TrileInstance instance, FaceOrientation face)
    {
      this.Instance = instance;
      this.Face = face;
    }

    public static bool operator ==(InstanceFace lhs, InstanceFace rhs)
    {
      if (lhs != null)
        return lhs.Equals(rhs);
      else
        return rhs == null;
    }

    public static bool operator !=(InstanceFace lhs, InstanceFace rhs)
    {
      return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
      return this.Instance.GetHashCode() ^ this.Face.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is TrileFace)
        return base.Equals((object) (obj as TrileFace));
      else
        return false;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public bool Equals(InstanceFace other)
    {
      if (other != null && other.Instance == this.Instance)
        return other.Face == this.Face;
      else
        return false;
    }
  }
}
