// Type: FezEngine.Structure.VolumeActorSettings
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine.Structure.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class VolumeActorSettings
  {
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public List<DotDialogueLine> DotDialogue = new List<DotDialogueLine>();
    [Serialization(Ignore = true)]
    public int NextLine = -1;
    [Serialization(Optional = true)]
    public Vector2 FarawayPlaneOffset;
    [Serialization(Ignore = true)]
    public float WaterOffset;
    [Serialization(Ignore = true)]
    public string DestinationSong;
    [Serialization(Ignore = true)]
    public float DestinationPixelsPerTrixel;
    [Serialization(Ignore = true)]
    public float DestinationRadius;
    [Serialization(Ignore = true)]
    public Vector2 DestinationOffset;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool IsPointOfInterest;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool WaterLocked;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool IsSecretPassage;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public CodeInput[] CodePattern;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool IsBlackHole;
    [Serialization(Ignore = true)]
    public bool Sucking;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool NeedsTrigger;
    [Serialization(Ignore = true)]
    public bool PreventHey;
  }
}
