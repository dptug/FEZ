// Type: FezEngine.Components.IFontManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Components
{
  public interface IFontManager
  {
    SpriteFont Big { get; }

    SpriteFont Small { get; }

    float SmallFactor { get; }

    float BigFactor { get; }

    float TopSpacing { get; }

    float SideSpacing { get; }
  }
}
