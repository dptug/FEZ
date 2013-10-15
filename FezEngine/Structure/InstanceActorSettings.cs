// Type: FezEngine.Structure.InstanceActorSettings
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using ContentSerialization.Attributes;
using System;

namespace FezEngine.Structure
{
  public class InstanceActorSettings : IEquatable<InstanceActorSettings>
  {
    public const int Steps = 16;

    [Serialization(Ignore = true)]
    public bool Inactive { get; set; }

    [Serialization(Optional = true)]
    public int? ContainedTrile { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public string SignText { get; set; }

    [Serialization(Optional = true)]
    public bool[] Sequence { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public string SequenceSampleName { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public string SequenceAlternateSampleName { get; set; }

    [Serialization(Optional = true)]
    public int? HostVolume { get; set; }

    public InstanceActorSettings()
    {
    }

    public InstanceActorSettings(InstanceActorSettings copy)
    {
      this.ContainedTrile = copy.ContainedTrile;
      this.SignText = copy.SignText;
      if (copy.Sequence != null)
      {
        this.Sequence = new bool[16];
        Array.Copy((Array) copy.Sequence, (Array) this.Sequence, 16);
      }
      this.SequenceSampleName = copy.SequenceSampleName;
      this.SequenceAlternateSampleName = copy.SequenceAlternateSampleName;
      this.HostVolume = copy.HostVolume;
    }

    public static bool operator ==(InstanceActorSettings lhs, InstanceActorSettings rhs)
    {
      if (lhs != null)
        return lhs.Equals(rhs);
      else
        return rhs == null;
    }

    public static bool operator !=(InstanceActorSettings lhs, InstanceActorSettings rhs)
    {
      return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
      return Util.CombineHashCodes((object) this.ContainedTrile, (object) this.SignText, (object) this.Sequence, (object) this.SequenceSampleName, (object) this.SequenceAlternateSampleName, (object) this.HostVolume);
    }

    public bool Equals(InstanceActorSettings other)
    {
      if (other != null)
      {
        int? containedTrile1 = other.ContainedTrile;
        int? containedTrile2 = this.ContainedTrile;
        if ((containedTrile1.GetValueOrDefault() != containedTrile2.GetValueOrDefault() ? 0 : (containedTrile1.HasValue == containedTrile2.HasValue ? 1 : 0)) != 0 && other.SignText == this.SignText && (object.Equals((object) other.Sequence, (object) this.Sequence) && other.SequenceSampleName == this.SequenceSampleName) && other.SequenceAlternateSampleName == this.SequenceAlternateSampleName)
        {
          int? hostVolume1 = other.HostVolume;
          int? hostVolume2 = this.HostVolume;
          if (hostVolume1.GetValueOrDefault() == hostVolume2.GetValueOrDefault())
            return hostVolume1.HasValue == hostVolume2.HasValue;
          else
            return false;
        }
      }
      return false;
    }

    public override bool Equals(object obj)
    {
      if (obj is InstanceActorSettings)
        return this.Equals(obj as InstanceActorSettings);
      else
        return false;
    }
  }
}
