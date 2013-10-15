// Type: FezEngine.Structure.TrileCustomData
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

namespace FezEngine.Structure
{
  public class TrileCustomData
  {
    public bool Unstable;
    public bool Shiny;
    public bool TiltTwoAxis;
    public bool IsCustom;

    public void DetermineCustom()
    {
      this.IsCustom = this.Unstable || this.Shiny || this.TiltTwoAxis;
    }
  }
}
