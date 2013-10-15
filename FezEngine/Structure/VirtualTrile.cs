// Type: FezEngine.Structure.VirtualTrile
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
  public struct VirtualTrile
  {
    public int VerticalOffset;

    public TrileInstance Instance { get; set; }

    public Vector3 Position
    {
      get
      {
        return this.Instance.Position + new Vector3(0.0f, (float) this.VerticalOffset, 0.0f);
      }
    }
  }
}
