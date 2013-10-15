// Type: Microsoft.Xna.Framework.Graphics.IndexBuffer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  public class IndexBuffer : GraphicsResource
  {
    private bool _isDynamic;
    internal uint ibo;

    public BufferUsage BufferUsage { get; private set; }

    public int IndexCount { get; private set; }

    public IndexElementSize IndexElementSize { get; private set; }

    protected IndexBuffer(GraphicsDevice graphicsDevice, IndexElementSize indexElementSize, int indexCount, BufferUsage bufferUsage, bool dynamic)
    {
      IndexBuffer indexBuffer = this;
      if (graphicsDevice == null)
        throw new ArgumentNullException("Graphics Device Cannot Be Null");
      this.GraphicsDevice = graphicsDevice;
      this.IndexElementSize = indexElementSize;
      this.IndexCount = indexCount;
      this.BufferUsage = bufferUsage;
      int sizeInBytes = indexCount * (this.IndexElementSize == IndexElementSize.SixteenBits ? 2 : 4);
      this._isDynamic = dynamic;
      Threading.BlockOnUIThread((Action) (() =>
      {
        GL.GenBuffers(1, out indexBuffer.ibo);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.ibo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) sizeInBytes, IntPtr.Zero, dynamic ? BufferUsageHint.StreamDraw : BufferUsageHint.StaticDraw);
      }));
    }

    public IndexBuffer(GraphicsDevice graphicsDevice, IndexElementSize indexElementSize, int indexCount, BufferUsage bufferUsage)
      : this(graphicsDevice, indexElementSize, indexCount, bufferUsage, false)
    {
    }

    public IndexBuffer(GraphicsDevice graphicsDevice, Type indexType, int indexCount, BufferUsage usage)
      : this(graphicsDevice, indexType == typeof (short) || indexType == typeof (ushort) ? IndexElementSize.SixteenBits : IndexElementSize.ThirtyTwoBits, indexCount, usage)
    {
    }

    public void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data is null");
      if (data.Length < startIndex + elementCount)
        throw new InvalidOperationException("The array specified in the data parameter is not the correct size for the amount of data requested.");
      if (this.BufferUsage == BufferUsage.WriteOnly)
        throw new NotSupportedException("This IndexBuffer was created with a usage type of BufferUsage.WriteOnly. Calling GetData on a resource that was created with BufferUsage.WriteOnly is not supported.");
      Threading.BlockOnUIThread((Action) (() =>
      {
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.ibo);
        int local_0 = Marshal.SizeOf(typeof (T));
        IntPtr local_1 = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.ReadOnly);
        local_1 = new IntPtr(local_1.ToInt64() + (long) offsetInBytes);
        if (data is byte[])
        {
          byte[] local_2 = data as byte[];
          Marshal.Copy(local_1, local_2, 0, local_2.Length);
        }
        else
        {
          byte[] local_2_1 = new byte[elementCount * local_0];
          Marshal.Copy(local_1, local_2_1, 0, local_2_1.Length);
          Buffer.BlockCopy((Array) local_2_1, 0, (Array) data, startIndex * local_0, elementCount * local_0);
        }
        GL.UnmapBuffer(BufferTarget.ArrayBuffer);
      }));
    }

    public void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct
    {
      this.GetData<T>(0, data, startIndex, elementCount);
    }

    public void GetData<T>(T[] data) where T : struct
    {
      this.GetData<T>(0, data, 0, data.Length);
    }

    public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : struct
    {
      this.SetDataInternal<T>(0, data, startIndex, elementCount, SetDataOptions.Discard);
    }

    public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
    {
      this.SetDataInternal<T>(0, data, startIndex, elementCount, SetDataOptions.Discard);
    }

    public void SetData<T>(T[] data) where T : struct
    {
      this.SetDataInternal<T>(0, data, 0, data.Length, SetDataOptions.Discard);
    }

    protected void SetDataInternal<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data is null");
      if (data.Length < startIndex + elementCount)
        throw new InvalidOperationException("The array specified in the data parameter is not the correct size for the amount of data requested.");
      Threading.BlockOnUIThread((Action) (() =>
      {
        int local_0 = Marshal.SizeOf(typeof (T));
        int local_1 = local_0 * elementCount;
        GCHandle local_2 = GCHandle.Alloc((object) data, GCHandleType.Pinned);
        IntPtr local_3 = (IntPtr) (local_2.AddrOfPinnedObject().ToInt64() + (long) (startIndex * local_0));
        int local_4 = this.IndexCount * (this.IndexElementSize == IndexElementSize.SixteenBits ? 2 : 4);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ibo);
        if (options == SetDataOptions.Discard)
          GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) local_4, IntPtr.Zero, this._isDynamic ? BufferUsageHint.StreamDraw : BufferUsageHint.StaticDraw);
        GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr) offsetInBytes, (IntPtr) local_1, local_3);
        local_2.Free();
      }));
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed && (this.GraphicsDevice != null && !this.GraphicsDevice.IsDisposed))
        this.GraphicsDevice.AddDisposeAction((Action) (() => GL.DeleteBuffers(1, ref this.ibo)));
      base.Dispose(disposing);
    }
  }
}
