// Type: Microsoft.Xna.Framework.Graphics.TextureCube
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
    {
      this.size = size;
      this.levelCount = 1;
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
      int num1 = this.size;
      while (num1 > 1)
      {
        num1 /= 2;
        TextureCube textureCube = this;
        int num2 = textureCube.levelCount + 1;
        textureCube.levelCount = num2;
      }
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
      int xoffset = 0;
      int yoffset = 0;
      int width = Math.Max(1, this.size >> level);
      int height = Math.Max(1, this.size >> level);
      if (rect.HasValue)
      {
        xoffset = rect.Value.X;
        yoffset = rect.Value.Y;
        width = rect.Value.Width;
        height = rect.Value.Height;
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
