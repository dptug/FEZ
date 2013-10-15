// Type: FezEngine.Structure.TrileGroup
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class TrileGroup
  {
    [Serialization(Ignore = true)]
    public int Id { get; set; }

    public string Name { get; set; }

    [Serialization(CollectionItemName = "trile")]
    public List<TrileInstance> Triles { get; set; }

    [Serialization(Optional = true)]
    public MovementPath Path { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Heavy { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public ActorType ActorType { get; set; }

    [Serialization(Ignore = true)]
    public bool InMidAir { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float GeyserOffset { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float GeyserPauseFor { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float GeyserLiftFor { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float GeyserApexHeight { get; set; }

    [Serialization(Ignore = true)]
    public bool MoveToEnd { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Spin180Degrees { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool SpinClockwise { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float SpinFrequency { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool SpinNeedsTriggering { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public Vector3 SpinCenter { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool FallOnRotate { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float SpinOffset { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public string AssociatedSound { get; set; }

    [Serialization(Ignore = true)]
    public bool PhysicsInitialized { get; set; }

    public TrileGroup()
    {
      this.Name = "Unnamed";
      this.Triles = new List<TrileInstance>();
    }

    public TrileGroup(TrileGroup group)
      : this()
    {
      this.Name = group.Name;
      this.Triles = new List<TrileInstance>(Enumerable.Select<TrileInstance, TrileInstance>((IEnumerable<TrileInstance>) group.Triles, (Func<TrileInstance, TrileInstance>) (x => x.Clone())));
      this.Heavy = group.Heavy;
      this.ActorType = group.ActorType;
      this.Path = group.Path == null ? (MovementPath) null : group.Path.Clone();
      this.GeyserPauseFor = group.GeyserPauseFor;
      this.GeyserLiftFor = group.GeyserLiftFor;
      this.GeyserApexHeight = group.GeyserApexHeight;
      this.GeyserOffset = group.GeyserOffset;
      this.SpinClockwise = group.SpinClockwise;
      this.SpinFrequency = group.SpinFrequency;
      this.SpinNeedsTriggering = group.SpinNeedsTriggering;
      this.SpinCenter = group.SpinCenter;
      this.Spin180Degrees = group.Spin180Degrees;
      this.FallOnRotate = group.FallOnRotate;
      this.SpinOffset = group.SpinOffset;
      this.AssociatedSound = group.AssociatedSound;
    }
  }
}
