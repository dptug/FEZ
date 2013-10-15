// Type: FezEngine.Structure.NpcActionContent
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using Microsoft.Xna.Framework.Audio;

namespace FezEngine.Structure
{
  public class NpcActionContent
  {
    [Serialization(Optional = true)]
    public string AnimationName { get; set; }

    [Serialization(Optional = true)]
    public string SoundName { get; set; }

    [Serialization(Ignore = true)]
    public AnimatedTexture Animation { get; set; }

    [Serialization(Ignore = true)]
    public SoundEffect Sound { get; set; }

    public NpcActionContent Clone()
    {
      return new NpcActionContent()
      {
        AnimationName = this.AnimationName,
        SoundName = this.SoundName
      };
    }
  }
}
