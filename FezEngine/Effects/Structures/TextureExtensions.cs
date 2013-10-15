// Type: FezEngine.Effects.Structures.TextureExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FezEngine.Effects.Structures
{
  public static class TextureExtensions
  {
    public static void Unhook(this Texture texture)
    {
      if (texture.Tag == null)
        return;
      foreach (SemanticMappedParameter<Texture> semanticMappedParameter in texture.Tag as HashSet<SemanticMappedTexture>)
        semanticMappedParameter.Set((Texture) null);
      texture.Tag = (object) null;
    }
  }
}
