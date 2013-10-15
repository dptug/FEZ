// Type: FezEngine.Content.AnimatedTextureContent
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Content
{
  public class AnimatedTextureContent
  {
    public readonly List<FrameContent> Frames = new List<FrameContent>();
    public int FrameWidth;
    public int FrameHeight;
    public int Width;
    public int Height;
    public byte[] PackedImage;
  }
}
