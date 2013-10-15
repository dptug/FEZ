// Type: FezEngine.Structure.Volume
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class Volume
  {
    private Vector3 from;
    private Vector3 to;

    [Serialization(Ignore = true)]
    public int Id { get; set; }

    [Serialization(Ignore = true)]
    public bool Enabled { get; set; }

    [Serialization(Ignore = true)]
    public bool PlayerInside { get; set; }

    [Serialization(Ignore = true)]
    public bool? PlayerIsHigher { get; set; }

    public HashSet<FaceOrientation> Orientations { get; set; }

    [Serialization(Optional = true)]
    public VolumeActorSettings ActorSettings { get; set; }

    public Vector3 From
    {
      get
      {
        return this.from;
      }
      set
      {
        this.from = value;
        this.BoundingBox = new BoundingBox(value, this.BoundingBox.Max);
      }
    }

    public Vector3 To
    {
      get
      {
        return this.to;
      }
      set
      {
        this.to = value;
        this.BoundingBox = new BoundingBox(this.BoundingBox.Min, value);
      }
    }

    [Serialization(Ignore = true)]
    public BoundingBox BoundingBox { get; set; }

    public Volume()
    {
      this.Orientations = new HashSet<FaceOrientation>((IEqualityComparer<FaceOrientation>) FaceOrientationComparer.Default);
      this.Enabled = true;
    }
  }
}
