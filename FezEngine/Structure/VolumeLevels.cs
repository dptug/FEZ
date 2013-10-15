// Type: FezEngine.Structure.VolumeLevels
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using System;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class VolumeLevels
  {
    [Serialization(CollectionItemName = "Sound")]
    public Dictionary<string, VolumeLevel> Sounds = new Dictionary<string, VolumeLevel>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  }
}
