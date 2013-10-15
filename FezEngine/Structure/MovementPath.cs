// Type: FezEngine.Structure.MovementPath
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class MovementPath
  {
    [Serialization(Ignore = true)]
    public int Id { get; set; }

    [Serialization(Optional = true)]
    public bool IsSpline { get; set; }

    [Serialization(Optional = true)]
    public float OffsetSeconds { get; set; }

    [Serialization(CollectionItemName = "segment")]
    public List<PathSegment> Segments { get; set; }

    public PathEndBehavior EndBehavior { get; set; }

    public bool NeedsTrigger { get; set; }

    [Serialization(Optional = true)]
    public string SoundName { get; set; }

    [Serialization(Ignore = true)]
    public bool RunOnce { get; set; }

    [Serialization(Ignore = true)]
    public bool RunSingleSegment { get; set; }

    [Serialization(Ignore = true)]
    public bool Backwards { get; set; }

    [Serialization(Ignore = true)]
    public bool InTransition { get; set; }

    [Serialization(Ignore = true)]
    public bool OutTransition { get; set; }

    [Serialization(Optional = true)]
    public bool SaveTrigger { get; set; }

    public MovementPath()
    {
      this.Segments = new List<PathSegment>();
    }

    public MovementPath Clone()
    {
      List<PathSegment> list = new List<PathSegment>(this.Segments.Count);
      foreach (PathSegment pathSegment in this.Segments)
        list.Add(pathSegment.Clone());
      return new MovementPath()
      {
        IsSpline = this.IsSpline,
        OffsetSeconds = this.OffsetSeconds,
        Segments = list,
        NeedsTrigger = this.NeedsTrigger,
        SoundName = this.SoundName,
        RunOnce = this.RunOnce,
        RunSingleSegment = this.RunSingleSegment,
        Backwards = this.Backwards,
        InTransition = this.InTransition,
        OutTransition = this.OutTransition,
        SaveTrigger = this.SaveTrigger
      };
    }
  }
}
