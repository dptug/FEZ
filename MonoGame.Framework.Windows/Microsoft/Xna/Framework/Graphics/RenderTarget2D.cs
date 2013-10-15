// Type: Microsoft.Xna.Framework.Graphics.RenderTarget2D
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class RenderTarget2D : Texture2D
  {
    private const RenderbufferTarget GLRenderbuffer = RenderbufferTarget.Renderbuffer;
    private const RenderbufferStorage GLDepthComponent16 = RenderbufferStorage.DepthComponent16;
    private const RenderbufferStorage GLDepthComponent24 = RenderbufferStorage.DepthComponent24;
    private const RenderbufferStorage GLDepth24Stencil8 = RenderbufferStorage.Depth24Stencil8;
    internal uint glDepthStencilBuffer;
    internal uint glFramebuffer;
    private bool disposed;

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

    public virtual event EventHandler<EventArgs> ContentLost;

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
        GL.GenRenderbuffers(1, out renderTarget2D.glDepthStencilBuffer);
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderTarget2D.glDepthStencilBuffer);
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
        if (renderTarget2D.MultiSampleCount == 0)
          GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, local_0, renderTarget2D.width, renderTarget2D.height);
        else
          GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, renderTarget2D.MultiSampleCount, local_0, renderTarget2D.width, renderTarget2D.height);
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
      if (!this.IsDisposed && (this.GraphicsDevice != null && !this.GraphicsDevice.IsDisposed))
        this.GraphicsDevice.AddDisposeAction((Action) (() =>
        {
          GL.DeleteRenderbuffers(1, ref this.glDepthStencilBuffer);
          if (this.glFramebuffer <= 0U)
            return;
          GL.DeleteFramebuffers(1, ref this.glFramebuffer);
        }));
      base.Dispose(disposing);
    }
  }
}
