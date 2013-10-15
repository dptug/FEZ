// Type: Microsoft.Xna.Framework.Graphics.Texture
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Graphics.OpenGL;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public abstract class Texture : GraphicsResource
  {
    internal int glTexture = -1;
    internal TextureUnit glTextureUnit = TextureUnit.Texture0;
    internal SamplerState glLastSamplerState = (SamplerState) null;
    protected SurfaceFormat format;
    protected int levelCount;
    internal TextureTarget glTarget;

    public SurfaceFormat Format
    {
      get
      {
        return this.format;
      }
    }

    public int LevelCount
    {
      get
      {
        return this.levelCount;
      }
    }

    internal int GetPitch(int width)
    {
      int num;
      switch (this.format)
      {
        case SurfaceFormat.Color:
        case SurfaceFormat.NormalizedByte4:
        case SurfaceFormat.Rgba1010102:
        case SurfaceFormat.Rg32:
        case SurfaceFormat.Single:
        case SurfaceFormat.HalfVector2:
          num = width * 4;
          break;
        case SurfaceFormat.Bgr565:
        case SurfaceFormat.Bgra5551:
        case SurfaceFormat.Bgra4444:
        case SurfaceFormat.NormalizedByte2:
        case SurfaceFormat.HalfSingle:
          num = width * 2;
          break;
        case SurfaceFormat.Dxt1:
        case SurfaceFormat.RgbPvrtc2Bpp:
        case SurfaceFormat.RgbaPvrtc2Bpp:
        case SurfaceFormat.RgbEtc1:
          num = (width + 3) / 4 * 8;
          break;
        case SurfaceFormat.Dxt3:
        case SurfaceFormat.Dxt5:
        case SurfaceFormat.RgbPvrtc4Bpp:
        case SurfaceFormat.RgbaPvrtc4Bpp:
          num = (width + 3) / 4 * 16;
          break;
        case SurfaceFormat.Rgba64:
        case SurfaceFormat.Vector2:
        case SurfaceFormat.HalfVector4:
          num = width * 8;
          break;
        case SurfaceFormat.Alpha8:
          num = width;
          break;
        case SurfaceFormat.Vector4:
          num = width * 16;
          break;
        default:
          throw new NotImplementedException("Unexpected format!");
      }
      return num;
    }

    protected internal new virtual void GraphicsDeviceResetting()
    {
      this.glTexture = -1;
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed)
      {
        if (this.GraphicsDevice != null && !this.GraphicsDevice.IsDisposed)
          this.GraphicsDevice.AddDisposeAction((Action) (() => GL.DeleteTextures(1, ref this.glTexture)));
        this.glLastSamplerState = (SamplerState) null;
      }
      base.Dispose(disposing);
    }
  }
}
