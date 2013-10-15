// Type: Microsoft.Xna.Framework.Graphics.RenderTargetBinding
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public struct RenderTargetBinding
  {
    internal Texture _renderTarget;
    internal bool isTargetCube;

    public Texture RenderTarget
    {
      get
      {
        return this._renderTarget;
      }
    }

    public RenderTargetBinding(RenderTarget2D renderTarget)
    {
      if (renderTarget == null)
        throw new ArgumentNullException("renderTarget");
      this._renderTarget = (Texture) renderTarget;
      this.isTargetCube = false;
    }

    public static implicit operator RenderTargetBinding(RenderTarget2D renderTarget)
    {
      return new RenderTargetBinding(renderTarget);
    }
  }
}
