// Type: FezEngine.Structure.RectangularTrixelSurfacePart
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using ContentSerialization.Attributes;
using FezEngine;
using System;

namespace FezEngine.Structure
{
  public class RectangularTrixelSurfacePart : IEquatable<RectangularTrixelSurfacePart>
  {
    public TrixelEmplacement Start { get; set; }

    [Serialization(Name = "tSize")]
    public int TangentSize { get; set; }

    [Serialization(Name = "bSize")]
    public int BitangentSize { get; set; }

    [Serialization(Ignore = true)]
    public FaceOrientation Orientation { get; set; }

    public override int GetHashCode()
    {
      return Util.CombineHashCodes(this.Start.GetHashCode(), this.TangentSize.GetHashCode(), this.BitangentSize.GetHashCode(), this.Orientation.GetHashCode());
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return this.Equals(obj as RectangularTrixelSurfacePart);
      else
        return false;
    }

    public bool Equals(RectangularTrixelSurfacePart other)
    {
      if (other != null && other.Orientation == this.Orientation && (other.Start.Equals(this.Start) && other.TangentSize == this.TangentSize))
        return other.BitangentSize == this.BitangentSize;
      else
        return false;
    }
  }
}
