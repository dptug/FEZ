// Type: FezEngine.Structure.Material
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
  public class Material
  {
    public Vector3 Diffuse;
    public float Opacity;

    public Material()
    {
      this.Diffuse = Vector3.One;
      this.Opacity = 1f;
    }

    public Material Clone()
    {
      return new Material()
      {
        Diffuse = this.Diffuse,
        Opacity = this.Opacity
      };
    }
  }
}
