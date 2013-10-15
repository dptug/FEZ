// Type: Microsoft.Xna.Framework.Graphics.TextureCube
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  public class TextureCube : Texture
  {
    protected int size;
    private PixelInternalFormat glInternalFormat;
    private PixelFormat glFormat;
    private PixelType glType;

    public int Size
    {
      get
      {
        return this.size;
      }
    }

    public TextureCube(GraphicsDevice graphicsDevice, int size, bool mipMap, SurfaceFormat format)
      : this(graphicsDevice, size, mipMap, format, false)
    {
    }

    internal TextureCube(GraphicsDevice graphicsDevice, int size, bool mipMap, SurfaceFormat format, bool renderTarget)
    {
      if (graphicsDevice == null)
        throw new ArgumentNullException("graphicsDevice");
      this.GraphicsDevice = graphicsDevice;
      this.size = size;
      this.format = format;
      this.levelCount = mipMap ? Texture.CalculateMipLevels(size, 0, 0) : 1;
      this.glTarget = TextureTarget.TextureCubeMap;
      GL.GenTextures(1, out this.glTexture);
      GL.BindTexture(TextureTarget.TextureCubeMap, this.glTexture);
      GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, mipMap ? 9987 : 9729);
      GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, 9729);
      GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, 33071);
      GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, 33071);
      GraphicsExtensions.GetGLFormat(format, out this.glInternalFormat, out this.glFormat, out this.glType);
      for (int index = 0; index < 6; ++index)
      {
        TextureTarget glCubeFace = this.GetGLCubeFace((CubeMapFace) index);
        if (this.glFormat == (PixelFormat) 34467)
          throw new NotImplementedException();
        GL.TexImage2D(glCubeFace, 0, this.glInternalFormat, size, size, 0, this.glFormat, this.glType, IntPtr.Zero);
      }
      if (!mipMap)
        return;
      GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.GenerateMipmap, 1);
    }

    public void GetData<T>(CubeMapFace cubeMapFace, T[] data) where T : struct
    {
      throw new NotImplementedException();
    }

    public void SetData<T>(CubeMapFace face, T[] data) where T : struct
    {
      this.SetData<T>(face, 0, new Rectangle?(), data, 0, data.Length);
    }

    public void SetData<T>(CubeMapFace face, T[] data, int startIndex, int elementCount) where T : struct
    {
      this.SetData<T>(face, 0, new Rectangle?(), data, startIndex, elementCount);
    }

    public void SetData<T>(CubeMapFace face, int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data");
      int num = Marshal.SizeOf(typeof (T));
      GCHandle gcHandle = GCHandle.Alloc((object) data, GCHandleType.Pinned);
      IntPtr pixels = (IntPtr) (gcHandle.AddrOfPinnedObject().ToInt64() + (long) (startIndex * num));
      int xoffset;
      int yoffset;
      int width;
      int height;
      if (rect.HasValue)
      {
        xoffset = rect.Value.X;
        yoffset = rect.Value.Y;
        width = rect.Value.Width;
        height = rect.Value.Height;
      }
      else
      {
        xoffset = 0;
        yoffset = 0;
        width = Math.Max(1, this.size >> level);
        height = Math.Max(1, this.size >> level);
      }
      GL.BindTexture(TextureTarget.TextureCubeMap, this.glTexture);
      TextureTarget glCubeFace = this.GetGLCubeFace(face);
      if (this.glFormat == (PixelFormat) 34467)
        throw new NotImplementedException();
      GL.TexSubImage2D(glCubeFace, level, xoffset, yoffset, width, height, this.glFormat, this.glType, pixels);
      gcHandle.Free();
    }

    private TextureTarget GetGLCubeFace(CubeMapFace face)
    {
      switch (face)
      {
        case CubeMapFace.PositiveX:
          return TextureTarget.TextureCubeMapPositiveX;
        case CubeMapFace.NegativeX:
          return TextureTarget.TextureCubeMapNegativeX;
        case CubeMapFace.PositiveY:
          return TextureTarget.TextureCubeMapPositiveY;
        case CubeMapFace.NegativeY:
          return TextureTarget.TextureCubeMapNegativeY;
        case CubeMapFace.PositiveZ:
          return TextureTarget.TextureCubeMapPositiveZ;
        case CubeMapFace.NegativeZ:
          return TextureTarget.TextureCubeMapNegativeZ;
        default:
          throw new ArgumentException();
      }
    }
  }
}
