// Type: Microsoft.Xna.Framework.Graphics.IEffectLights
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
  public interface IEffectLights
  {
    Vector3 AmbientLightColor { get; set; }

    DirectionalLight DirectionalLight0 { get; }

    DirectionalLight DirectionalLight1 { get; }

    DirectionalLight DirectionalLight2 { get; }

    bool LightingEnabled { get; set; }

    void EnableDefaultLighting();
  }
}
