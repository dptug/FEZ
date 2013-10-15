// Type: Microsoft.Xna.Framework.Graphics.RenderTargetCube
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class RenderTargetCube : TextureCube, IRenderTarget
  {
    public DepthFormat DepthStencilFormat { get; private set; }

    public int MultiSampleCount { get; private set; }

    public RenderTargetUsage RenderTargetUsage { get; private set; }

    int IRenderTarget.Width
    {
      get
      {
        return this.size;
      }
    }

    int IRenderTarget.Height
    {
      get
      {
        return this.size;
      }
    }

    public RenderTargetCube(GraphicsDevice graphicsDevice, int size, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat)
      : this(graphicsDevice, size, mipMap, preferredFormat, preferredDepthFormat, 0, RenderTargetUsage.DiscardContents)
    {
    }

    public RenderTargetCube(GraphicsDevice graphicsDevice, int size, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
      : base(graphicsDevice, size, mipMap, preferredFormat, true)
    {
      this.DepthStencilFormat = preferredDepthFormat;
      this.MultiSampleCount = preferredMultiSampleCount;
      this.RenderTargetUsage = usage;
      throw new NotImplementedException();
    }

    protected override void Dispose(bool disposing)
    {
      if (this.IsDisposed)
        return;
      base.Dispose(disposing);
    }
  }
}
