// Type: FezEngine.Structure.PointLight
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
  public struct PointLight
  {
    public Vector3 Position { get; set; }

    public float LinearAttenuation { get; set; }

    public float QuadraticAttenuation { get; set; }

    public bool Spot { get; set; }

    public float Theta { get; set; }

    public float Phi { get; set; }

    public float Falloff { get; set; }

    public Vector3 Diffuse { get; set; }

    public Vector3 Specular { get; set; }

    public void Initialize()
    {
      this.Diffuse = this.Specular = Vector3.One;
    }
  }
}
