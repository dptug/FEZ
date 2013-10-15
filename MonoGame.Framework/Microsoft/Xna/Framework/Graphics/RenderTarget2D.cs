// Type: Microsoft.Xna.Framework.Graphics.RenderTarget2D
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class RenderTarget2D : Texture2D, IRenderTarget
  {
    private static readonly RenderbufferTarget GLRenderbuffer = GraphicsExtensions.UseArbFramebuffer ? RenderbufferTarget.Renderbuffer : RenderbufferTarget.Renderbuffer;
    private const RenderbufferStorage GLDepthComponent16 = RenderbufferStorage.DepthComponent16;
    private const RenderbufferStorage GLDepthComponent24 = RenderbufferStorage.DepthComponent24;
    private const RenderbufferStorage GLDepth24Stencil8 = RenderbufferStorage.Depth24Stencil8;
    internal uint glDepthStencilBuffer;

    public DepthFormat DepthStencilFormat { get; private set; }

    public int MultiSampleCount { get; private set; }

    public RenderTargetUsage RenderTargetUsage { get; private set; }

    public bool IsContentLost
    {
      get
      {
        return false;
      }
    }

    public event EventHandler<EventArgs> ContentLost;

    static RenderTarget2D()
    {
    }

    public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
      : base(graphicsDevice, width, height, mipMap, preferredFormat, true)
    {
      RenderTarget2D renderTarget2D = this;
      this.DepthStencilFormat = preferredDepthFormat;
      this.MultiSampleCount = preferredMultiSampleCount;
      this.RenderTargetUsage = usage;
      if (preferredDepthFormat == DepthFormat.None)
        return;
      Threading.BlockOnUIThread((Action) (() =>
      {
        GraphicsExtensions.GenRenderbuffers(1, out renderTarget2D.glDepthStencilBuffer);
        GraphicsExtensions.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderTarget2D.glDepthStencilBuffer);
        RenderbufferStorage local_0 = RenderbufferStorage.DepthComponent16;
        switch (preferredDepthFormat)
        {
          case DepthFormat.Depth24Stencil8:
            local_0 = RenderbufferStorage.Depth24Stencil8;
            break;
          case DepthFormat.Depth24:
            local_0 = RenderbufferStorage.DepthComponent24;
            break;
          case DepthFormat.Depth16:
            local_0 = RenderbufferStorage.DepthComponent16;
            break;
        }
        GraphicsExtensions.RenderbufferStorage(RenderTarget2D.GLRenderbuffer, local_0, renderTarget2D.width, renderTarget2D.height);
      }));
    }

    public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat)
      : this(graphicsDevice, width, height, mipMap, preferredFormat, preferredDepthFormat, 0, RenderTargetUsage.DiscardContents)
    {
    }

    public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height)
      : this(graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed && this.GraphicsDevice != null && !this.GraphicsDevice.IsDisposed)
        GraphicsDevice.AddDisposeAction((Action) (() => GraphicsExtensions.DeleteRenderbuffers(1, ref this.glDepthStencilBuffer)));
      base.Dispose(disposing);
    }
  }
}
