// Type: FezEngine.Structure.CameraNodeData
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using System;

namespace FezEngine.Structure
{
  public class CameraNodeData : ICloneable
  {
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Perspective { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int PixelsPerTrixel { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public string SoundName { get; set; }

    public object Clone()
    {
      return (object) new CameraNodeData()
      {
        Perspective = this.Perspective,
        PixelsPerTrixel = this.PixelsPerTrixel,
        SoundName = this.SoundName
      };
    }
  }
}
