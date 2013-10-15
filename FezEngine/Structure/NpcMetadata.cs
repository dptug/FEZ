// Type: FezEngine.Structure.NpcMetadata
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class NpcMetadata
  {
    [Serialization(Optional = true)]
    public float WalkSpeed = 1.5f;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public List<NpcAction> SoundActions = new List<NpcAction>();
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool AvoidsGomez;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public ActorType ActorType;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public string SoundPath;
  }
}
