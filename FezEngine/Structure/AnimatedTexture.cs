// Type: FezEngine.Structure.AnimatedTexture
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine.Effects.Structures;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezEngine.Structure
{
  public class AnimatedTexture : IDisposable
  {
    public Texture2D Texture { get; set; }

    public Rectangle[] Offsets { get; set; }

    public AnimationTiming Timing { get; set; }

    public int FrameWidth { get; set; }

    public int FrameHeight { get; set; }

    public Vector2 PotOffset { get; set; }

    [Serialization(Ignore = true)]
    public bool NoHat { get; set; }

    public void Dispose()
    {
      if (this.Texture != null)
      {
        TextureExtensions.Unhook((Texture) this.Texture);
        this.Texture.Dispose();
      }
      this.Texture = (Texture2D) null;
      this.Timing = (AnimationTiming) null;
    }
  }
}
