// Type: Microsoft.Xna.Framework.Graphics.Texture3D
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  public class Texture3D : Texture
  {
    private int width;
    private int height;
    private int depth;
    private PixelInternalFormat glInternalFormat;
    private PixelFormat glFormat;
    private PixelType glType;

    public int Width
    {
      get
      {
        return this.width;
      }
    }

    public int Height
    {
      get
      {
        return this.height;
      }
    }

    public int Depth
    {
      get
      {
        return this.depth;
      }
    }

    public Texture3D(GraphicsDevice graphicsDevice, int width, int height, int depth, bool mipMap, SurfaceFormat format)
    {
      if (graphicsDevice == null)
        throw new ArgumentNullException("graphicsDevice");
      this.GraphicsDevice = graphicsDevice;
      this.width = width;
      this.height = height;
      this.depth = depth;
      this.levelCount = 1;
      this.glTarget = TextureTarget.Texture3D;
      GL.GenTextures(1, out this.glTexture);
      GL.BindTexture(this.glTarget, this.glTexture);
      GraphicsExtensions.GetGLFormat(format, out this.glInternalFormat, out this.glFormat, out this.glType);
      GL.TexImage3D(this.glTarget, 0, this.glInternalFormat, width, height, depth, 0, this.glFormat, this.glType, IntPtr.Zero);
      if (mipMap)
        throw new NotImplementedException("Texture3D does not yet support mipmaps.");
    }

    public void SetData<T>(T[] data) where T : struct
    {
      this.SetData<T>(data, 0, data.Length);
    }

    public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
    {
      this.SetData<T>(0, 0, 0, this.Width, this.Height, 0, this.Depth, data, startIndex, elementCount);
    }

    public void SetData<T>(int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data");
      int num = Marshal.SizeOf(typeof (T));
      GCHandle gcHandle = GCHandle.Alloc((object) data, GCHandleType.Pinned);
      IntPtr pixels = (IntPtr) (gcHandle.AddrOfPinnedObject().ToInt64() + (long) (startIndex * num));
      int width = right - left;
      int height = bottom - top;
      int depth = back - front;
      GL.BindTexture(this.glTarget, this.glTexture);
      GL.TexSubImage3D(this.glTarget, level, left, top, front, width, height, depth, this.glFormat, this.glType, pixels);
      gcHandle.Free();
    }
  }
}
