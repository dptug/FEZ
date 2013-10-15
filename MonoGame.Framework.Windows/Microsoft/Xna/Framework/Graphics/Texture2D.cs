// Type: Microsoft.Xna.Framework.Graphics.Texture2D
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  public class Texture2D : Texture
  {
    protected int width;
    protected int height;
    private PixelInternalFormat glInternalFormat;
    private OpenTK.Graphics.OpenGL.PixelFormat glFormat;
    private PixelType glType;

    public Microsoft.Xna.Framework.Rectangle Bounds
    {
      get
      {
        return new Microsoft.Xna.Framework.Rectangle(0, 0, this.width, this.height);
      }
    }

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

    public Texture2D(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format)
      : this(graphicsDevice, width, height, mipmap, format, false)
    {
    }

    internal Texture2D(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format, bool renderTarget)
    {
      Texture2D texture2D = this;
      if (graphicsDevice == null)
        throw new ArgumentNullException("Graphics Device Cannot Be Null");
      this.GraphicsDevice = graphicsDevice;
      this.width = width;
      this.height = height;
      this.format = format;
      this.levelCount = 1;
      if (mipmap)
      {
        int num1 = Math.Max(this.width, this.height);
        while (num1 > 1)
        {
          num1 /= 2;
          Texture2D texture2D1 = this;
          int num2 = texture2D1.levelCount + 1;
          texture2D1.levelCount = num2;
        }
      }
      this.glTarget = TextureTarget.Texture2D;
      Threading.BlockOnUIThread((Action) (() =>
      {
        int local_0 = GraphicsExtensions.GetBoundTexture2D();
        texture2D.GenerateGLTextureIfRequired();
        GraphicsExtensions.GetGLFormat(format, out texture2D.glInternalFormat, out texture2D.glFormat, out texture2D.glType);
        if (texture2D.glFormat == (OpenTK.Graphics.OpenGL.PixelFormat) 34467)
        {
          int local_1_1;
          switch (format)
          {
            case SurfaceFormat.Dxt1:
              local_1_1 = (texture2D.width + 3) / 4 * ((texture2D.height + 3) / 4) * 8;
              break;
            case SurfaceFormat.Dxt3:
            case SurfaceFormat.Dxt5:
              local_1_1 = (texture2D.width + 3) / 4 * ((texture2D.height + 3) / 4) * 16;
              break;
            case SurfaceFormat.RgbPvrtc2Bpp:
            case SurfaceFormat.RgbaPvrtc2Bpp:
              local_1_1 = (Math.Max(texture2D.width, 8) * Math.Max(texture2D.height, 8) * 2 + 7) / 8;
              break;
            case SurfaceFormat.RgbPvrtc4Bpp:
            case SurfaceFormat.RgbaPvrtc4Bpp:
              local_1_1 = (Math.Max(texture2D.width, 16) * Math.Max(texture2D.height, 8) * 4 + 7) / 8;
              break;
            default:
              throw new NotImplementedException();
          }
          GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, texture2D.glInternalFormat, texture2D.width, texture2D.height, 0, local_1_1, IntPtr.Zero);
        }
        else
          GL.TexImage2D(TextureTarget.Texture2D, 0, texture2D.glInternalFormat, texture2D.width, texture2D.height, 0, texture2D.glFormat, texture2D.glType, IntPtr.Zero);
        GL.BindTexture(TextureTarget.Texture2D, local_0);
      }));
    }

    public Texture2D(GraphicsDevice graphicsDevice, int width, int height)
      : this(graphicsDevice, width, height, false, SurfaceFormat.Color, false)
    {
    }

    public void SetData<T>(int level, Microsoft.Xna.Framework.Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data");
      Threading.BlockOnUIThread((Action) (() =>
      {
        int local_0 = Marshal.SizeOf(typeof (T));
        GCHandle local_1 = GCHandle.Alloc((object) data, GCHandleType.Pinned);
        int local_2 = startIndex * local_0;
        IntPtr local_3 = (IntPtr) (local_1.AddrOfPinnedObject().ToInt64() + (long) local_2);
        int local_4;
        int local_5;
        int local_6;
        int local_7;
        if (rect.HasValue)
        {
          local_4 = rect.Value.X;
          local_5 = rect.Value.Y;
          local_6 = rect.Value.Width;
          local_7 = rect.Value.Height;
        }
        else
        {
          local_4 = 0;
          local_5 = 0;
          local_6 = Math.Max(this.width >> level, 1);
          local_7 = Math.Max(this.height >> level, 1);
        }
        int local_8 = GraphicsExtensions.GetBoundTexture2D();
        this.GenerateGLTextureIfRequired();
        GL.BindTexture(TextureTarget.Texture2D, this.glTexture);
        if (this.glFormat == (OpenTK.Graphics.OpenGL.PixelFormat) 34467)
        {
          if (rect.HasValue)
            GL.CompressedTexSubImage2D(TextureTarget.Texture2D, level, local_4, local_5, local_6, local_7, this.glFormat, data.Length - local_2, local_3);
          else
            GL.CompressedTexImage2D(TextureTarget.Texture2D, level, this.glInternalFormat, local_6, local_7, 0, data.Length - local_2, local_3);
        }
        else if (rect.HasValue)
          GL.TexSubImage2D(TextureTarget.Texture2D, level, local_4, local_5, local_6, local_7, this.glFormat, this.glType, local_3);
        else
          GL.TexImage2D(TextureTarget.Texture2D, level, this.glInternalFormat, local_6, local_7, 0, this.glFormat, this.glType, local_3);
        GL.Finish();
        GL.BindTexture(TextureTarget.Texture2D, local_8);
        local_1.Free();
        GL.Finish();
      }));
    }

    public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
    {
      this.SetData<T>(0, new Microsoft.Xna.Framework.Rectangle?(), data, startIndex, elementCount);
    }

    public void SetData<T>(T[] data) where T : struct
    {
      this.SetData<T>(0, new Microsoft.Xna.Framework.Rectangle?(), data, 0, data.Length);
    }

    public void GetData<T>(int level, Microsoft.Xna.Framework.Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
    {
      Threading.BlockOnUIThread((Action) (() =>
      {
        GL.BindTexture(TextureTarget.Texture2D, this.glTexture);
        if (rect.HasValue)
          throw new NotImplementedException();
        if (this.glFormat == (OpenTK.Graphics.OpenGL.PixelFormat) 34467)
          throw new NotImplementedException();
        GL.GetTexImage<T>(TextureTarget.Texture2D, level, this.glFormat, this.glType, data);
      }));
    }

    public void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct
    {
      this.GetData<T>(0, new Microsoft.Xna.Framework.Rectangle?(), data, startIndex, elementCount);
    }

    public void GetData<T>(T[] data) where T : struct
    {
      this.GetData<T>(0, new Microsoft.Xna.Framework.Rectangle?(), data, 0, data.Length);
    }

    public static Texture2D FromStream(GraphicsDevice graphicsDevice, Stream stream)
    {
      using (System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap) Image.FromStream(stream))
      {
        ImageEx.RGBToBGR((Image) bitmap);
        byte[] numArray = new byte[bitmap.Width * bitmap.Height * 4];
        BitmapData bitmapdata = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        if (bitmapdata.Stride != bitmap.Width * 4)
          throw new NotImplementedException();
        Marshal.Copy(bitmapdata.Scan0, numArray, 0, numArray.Length);
        bitmap.UnlockBits(bitmapdata);
        Texture2D texture2D = new Texture2D(graphicsDevice, bitmap.Width, bitmap.Height);
        texture2D.SetData<byte>(numArray);
        return texture2D;
      }
    }

    private void FillTextureFromStream(Stream stream)
    {
    }

    public void SaveAsJpeg(Stream stream, int width, int height)
    {
      this.SaveAsImage(ImageFormat.Jpeg, stream, width, height);
    }

    public void SaveAsPng(Stream stream)
    {
      this.SaveAsPng(stream, this.width, this.height);
    }

    public void SaveAsPng(Stream stream, int width, int height)
    {
      this.SaveAsImage(ImageFormat.Png, stream, width, height);
    }

    private void SaveAsImage(ImageFormat outputFormat, Stream stream, int width, int height)
    {
      byte[] numArray = new byte[width * height * GraphicsExtensions.Size(this.Format)];
      this.GetData<byte>(numArray);
      using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
      {
        BitmapData bitmapdata = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
        Marshal.Copy(numArray, 0, bitmapdata.Scan0, numArray.Length);
        bitmap.UnlockBits(bitmapdata);
        ImageEx.RGBToBGR((Image) bitmap);
        bitmap.Save(stream, outputFormat);
      }
    }

    internal void Reload(Stream textureStream)
    {
      this.GenerateGLTextureIfRequired();
      this.FillTextureFromStream(textureStream);
    }

    private void GenerateGLTextureIfRequired()
    {
      if (this.glTexture >= 0)
        return;
      GL.GenTextures(1, out this.glTexture);
      TextureWrapMode textureWrapMode = TextureWrapMode.Repeat;
      if ((this.width & this.width - 1) != 0 || (this.height & this.height - 1) != 0)
        textureWrapMode = TextureWrapMode.ClampToEdge;
      GL.BindTexture(TextureTarget.Texture2D, this.glTexture);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, this.levelCount > 1 ? 9987 : 9729);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) textureWrapMode);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) textureWrapMode);
    }
  }
}
