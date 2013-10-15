// Type: FezEngine.Structure.TrileActorSettings
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using ContentSerialization.Attributes;
using FezEngine;
using System;

namespace FezEngine.Structure
{
  public class TrileActorSettings : IEquatable<TrileActorSettings>
  {
    [Serialization(Optional = true)]
    public ActorType Type { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public FaceOrientation Face { get; set; }

    public TrileActorSettings()
    {
    }

    public TrileActorSettings(TrileActorSettings copy)
    {
      this.Type = copy.Type;
      this.Face = copy.Face;
    }

    public static bool operator ==(TrileActorSettings lhs, TrileActorSettings rhs)
    {
      if (lhs != null)
        return lhs.Equals(rhs);
      else
        return rhs == null;
    }

    public static bool operator !=(TrileActorSettings lhs, TrileActorSettings rhs)
    {
      return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
      return Util.CombineHashCodes(this.Type.GetHashCode(), this.Face.GetHashCode());
    }

    public bool Equals(TrileActorSettings other)
    {
      if (other != null && other.Type == this.Type)
        return other.Face == this.Face;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (obj is TrileActorSettings)
        return this.Equals(obj as TrileActorSettings);
      else
        return false;
    }
  }
}
