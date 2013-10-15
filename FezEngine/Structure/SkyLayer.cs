// Type: FezEngine.Structure.SkyLayer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;

namespace FezEngine.Structure
{
  public class SkyLayer
  {
    [Serialization(Optional = true)]
    public string Name { get; set; }

    [Serialization(Optional = true)]
    public bool InFront { get; set; }

    [Serialization(Optional = true)]
    public float Opacity { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float FogTint { get; set; }

    public SkyLayer()
    {
      this.Opacity = 1f;
    }

    public SkyLayer ShallowCopy()
    {
      return new SkyLayer()
      {
        Name = this.Name,
        InFront = this.InFront,
        Opacity = this.Opacity,
        FogTint = this.FogTint
      };
    }

    public void UpdateFromCopy(SkyLayer copy)
    {
      this.Name = copy.Name;
      this.InFront = copy.InFront;
      this.Opacity = copy.Opacity;
      this.FogTint = copy.FogTint;
    }
  }
}
