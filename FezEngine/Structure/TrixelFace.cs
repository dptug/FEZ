// Type: FezEngine.Structure.TrixelFace
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using System;

namespace FezEngine.Structure
{
  public class TrixelFace : IEquatable<TrixelFace>
  {
    public TrixelEmplacement Id;

    public FaceOrientation Face { get; set; }

    public TrixelFace()
      : this(new TrixelEmplacement(), FaceOrientation.Left)
    {
    }

    public TrixelFace(int x, int y, int z, FaceOrientation face)
      : this(new TrixelEmplacement(x, y, z), face)
    {
    }

    public TrixelFace(TrixelEmplacement id, FaceOrientation face)
    {
      this.Id = id;
      this.Face = face;
    }

    public override int GetHashCode()
    {
      return this.Id.GetHashCode() + this.Face.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is TrixelFace)
        return this.Equals(obj as TrixelFace);
      else
        return false;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public bool Equals(TrixelFace other)
    {
      if (other.Id.Equals(this.Id))
        return other.Face == this.Face;
      else
        return false;
    }
  }
}
