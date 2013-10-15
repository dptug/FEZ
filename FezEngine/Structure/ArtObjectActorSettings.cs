// Type: FezEngine.Structure.ArtObjectActorSettings
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine;
using FezEngine.Structure.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class ArtObjectActorSettings : IEquatable<ArtObjectActorSettings>
  {
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Inactive { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public ActorType ContainedTrile { get; set; }

    [Serialization(Optional = true)]
    public int? AttachedGroup { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float SpinOffset { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float SpinEvery { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public Viewpoint SpinView { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool OffCenter { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public Vector3 RotationCenter { get; set; }

    [Serialization(Optional = true)]
    public VibrationMotor[] VibrationPattern { get; set; }

    [Serialization(Optional = true)]
    public CodeInput[] CodePattern { get; set; }

    [Serialization(Optional = true)]
    public PathSegment Segment { get; set; }

    [Serialization(Optional = true)]
    public int? NextNode { get; set; }

    [Serialization(Optional = true)]
    public string DestinationLevel { get; set; }

    [Serialization(Optional = true)]
    public string TreasureMapName { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float TimeswitchWindBackSpeed { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public HashSet<FaceOrientation> InvisibleSides { get; set; }

    [Serialization(Ignore = true)]
    public ArtObjectInstance NextNodeAo { get; set; }

    [Serialization(Ignore = true)]
    public ArtObjectInstance PrecedingNodeAo { get; set; }

    [Serialization(Ignore = true)]
    public bool ShouldMoveToEnd { get; set; }

    [Serialization(Ignore = true)]
    public float? ShouldMoveToHeight { get; set; }

    public ArtObjectActorSettings()
    {
      this.InvisibleSides = new HashSet<FaceOrientation>((IEqualityComparer<FaceOrientation>) FaceOrientationComparer.Default);
    }

    public static bool operator ==(ArtObjectActorSettings lhs, ArtObjectActorSettings rhs)
    {
      if (lhs != null)
        return lhs.Equals(rhs);
      else
        return rhs == null;
    }

    public static bool operator !=(ArtObjectActorSettings lhs, ArtObjectActorSettings rhs)
    {
      return !(lhs == rhs);
    }

    public bool Equals(ArtObjectActorSettings other)
    {
      if (other != null && other.ContainedTrile == this.ContainedTrile && other.Inactive == this.Inactive)
      {
        int? attachedGroup1 = other.AttachedGroup;
        int? attachedGroup2 = this.AttachedGroup;
        if ((attachedGroup1.GetValueOrDefault() != attachedGroup2.GetValueOrDefault() ? 0 : (attachedGroup1.HasValue == attachedGroup2.HasValue ? 1 : 0)) != 0 && (double) other.SpinOffset == (double) this.SpinOffset && ((double) other.SpinEvery == (double) this.SpinEvery && other.SpinView == this.SpinView) && (other.OffCenter == this.OffCenter && other.RotationCenter == this.RotationCenter && (other.VibrationPattern == this.VibrationPattern && other.CodePattern == this.CodePattern)) && other.Segment == this.Segment)
        {
          int? nextNode1 = other.NextNode;
          int? nextNode2 = this.NextNode;
          if ((nextNode1.GetValueOrDefault() != nextNode2.GetValueOrDefault() ? 0 : (nextNode1.HasValue == nextNode2.HasValue ? 1 : 0)) != 0 && other.DestinationLevel == this.DestinationLevel && (other.TreasureMapName == this.TreasureMapName && (double) other.TimeswitchWindBackSpeed == (double) this.TimeswitchWindBackSpeed))
            return other.InvisibleSides.Equals((object) this.InvisibleSides);
        }
      }
      return false;
    }

    public override bool Equals(object obj)
    {
      if (obj is ArtObjectActorSettings)
        return this.Equals(obj as ArtObjectActorSettings);
      else
        return false;
    }
  }
}
