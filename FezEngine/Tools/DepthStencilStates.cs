// Type: FezEngine.Tools.DepthStencilStates
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Tools
{
  public static class DepthStencilStates
  {
    public static readonly DepthStencilState DefaultWithStencil = new DepthStencilState()
    {
      StencilEnable = true
    };

    static DepthStencilStates()
    {
    }
  }
}
