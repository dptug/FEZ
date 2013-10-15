// Type: Microsoft.Xna.Framework.Graphics.IEffectFog
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
  public interface IEffectFog
  {
    Vector3 FogColor { get; set; }

    bool FogEnabled { get; set; }

    float FogEnd { get; set; }

    float FogStart { get; set; }
  }
}
