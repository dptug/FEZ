// Type: Microsoft.Xna.Framework.Graphics.Texture
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK.Graphics.OpenGL;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public abstract class Texture : GraphicsResource
  {
    internal int glTexture = -1;
    internal TextureUnit glTextureUnit = TextureUnit.Texture0;
    protected SurfaceFormat format;
    protected int levelCount;
    internal TextureTarget glTarget;
    internal SamplerState glLastSamplerState;

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

    internal static int CalculateMipLevels(int width, int height = 0, int depth = 0)
    {
      int num1 = 1;
      int num2 = Math.Max(Math.Max(width, height), depth);
      while (num2 > 1)
      {
        num2 /= 2;
        ++num1;
      }
      return num1;
    }

    internal int GetPitch(int width)
    {
      switch (this.format)
      {
        case SurfaceFormat.Color:
        case SurfaceFormat.NormalizedByte4:
        case SurfaceFormat.Rgba1010102:
        case SurfaceFormat.Rg32:
        case SurfaceFormat.Single:
        case SurfaceFormat.HalfVector2:
          return width * 4;
        case SurfaceFormat.Bgr565:
        case SurfaceFormat.Bgra5551:
        case SurfaceFormat.Bgra4444:
        case SurfaceFormat.NormalizedByte2:
        case SurfaceFormat.HalfSingle:
          return width * 2;
        case SurfaceFormat.Dxt1:
        case SurfaceFormat.RgbPvrtc2Bpp:
        case SurfaceFormat.RgbaPvrtc2Bpp:
        case SurfaceFormat.RgbEtc1:
          return (width + 3) / 4 * 8;
        case SurfaceFormat.Dxt3:
        case SurfaceFormat.Dxt5:
        case SurfaceFormat.RgbPvrtc4Bpp:
        case SurfaceFormat.RgbaPvrtc4Bpp:
          return (width + 3) / 4 * 16;
        case SurfaceFormat.Rgba64:
        case SurfaceFormat.Vector2:
        case SurfaceFormat.HalfVector4:
          return width * 8;
        case SurfaceFormat.Alpha8:
          return width;
        case SurfaceFormat.Vector4:
          return width * 16;
        default:
          throw new NotImplementedException("Unexpected format!");
      }
    }

    protected internal override void GraphicsDeviceResetting()
    {
      this.glTexture = -1;
      this.glLastSamplerState = (SamplerState) null;
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed)
      {
        GraphicsDevice.AddDisposeAction((Action) (() =>
        {
          GL.DeleteTextures(1, ref this.glTexture);
          this.glTexture = -1;
        }));
        this.glLastSamplerState = (SamplerState) null;
      }
      base.Dispose(disposing);
    }
  }
}
