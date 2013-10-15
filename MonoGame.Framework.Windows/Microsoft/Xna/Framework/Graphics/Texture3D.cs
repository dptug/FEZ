// Type: Microsoft.Xna.Framework.Graphics.Texture3D
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  public class Texture3D : Texture
  {
    private PixelInternalFormat glInternalFormat;
    private PixelFormat glFormat;
    private PixelType glType;

    public int Width { get; private set; }

    public int Height { get; private set; }

    public int Depth { get; private set; }

    public Texture3D(GraphicsDevice graphicsDevice, int width, int height, int depth, bool mipMap, SurfaceFormat format)
    {
      this.GraphicsDevice = graphicsDevice;
      this.Width = width;
      this.Height = height;
      this.Depth = depth;
      this.glTarget = TextureTarget.Texture3D;
      GL.GenTextures(1, out this.glTexture);
      GL.BindTexture(this.glTarget, this.glTexture);
      GraphicsExtensions.GetGLFormat(format, out this.glInternalFormat, out this.glFormat, out this.glType);
      GL.TexImage3D(this.glTarget, 0, this.glInternalFormat, width, height, depth, 0, this.glFormat, this.glType, IntPtr.Zero);
      if (mipMap)
        throw new NotImplementedException();
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
      GL.BindTexture(this.glTarget, this.glTexture);
      GL.TexSubImage3D(this.glTarget, level, left, top, front, right - left, bottom - top, back - front, this.glFormat, this.glType, pixels);
      gcHandle.Free();
    }
  }
}
