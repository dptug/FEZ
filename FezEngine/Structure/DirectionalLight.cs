// Type: FezEngine.Structure.DirectionalLight
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
  public struct DirectionalLight
  {
    public Vector3 Direction { get; set; }

    public Vector3 Diffuse { get; set; }

    public Vector3 Specular { get; set; }

    public void Initialize()
    {
      this.Diffuse = this.Specular = Vector3.One;
    }
  }
}
