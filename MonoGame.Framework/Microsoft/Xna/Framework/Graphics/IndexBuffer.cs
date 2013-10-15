// Type: Microsoft.Xna.Framework.Graphics.IndexBuffer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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

    protected IndexBuffer(GraphicsDevice graphicsDevice, Type indexType, int indexCount, BufferUsage usage, bool dynamic)
      : this(graphicsDevice, IndexBuffer.SizeForType(graphicsDevice, indexType), indexCount, usage, dynamic)
    {
    }

    protected IndexBuffer(GraphicsDevice graphicsDevice, IndexElementSize indexElementSize, int indexCount, BufferUsage usage, bool dynamic)
    {
      if (graphicsDevice == null)
        throw new ArgumentNullException("GraphicsDevice is null");
      this.GraphicsDevice = graphicsDevice;
      this.IndexElementSize = indexElementSize;
      this.IndexCount = indexCount;
      this.BufferUsage = usage;
      int num = (int) this.IndexElementSize;
      this._isDynamic = dynamic;
      Threading.BlockOnUIThread(new Action(this.GenerateIfRequired));
    }

    public IndexBuffer(GraphicsDevice graphicsDevice, IndexElementSize indexElementSize, int indexCount, BufferUsage bufferUsage)
      : this(graphicsDevice, indexElementSize, indexCount, bufferUsage, false)
    {
    }

    public IndexBuffer(GraphicsDevice graphicsDevice, Type indexType, int indexCount, BufferUsage usage)
      : this(graphicsDevice, IndexBuffer.SizeForType(graphicsDevice, indexType), indexCount, usage, false)
    {
    }

    private static IndexElementSize SizeForType(GraphicsDevice graphicsDevice, Type type)
    {
      switch (Marshal.SizeOf(type))
      {
        case 2:
          return IndexElementSize.SixteenBits;
        case 4:
          if (graphicsDevice.GraphicsProfile == GraphicsProfile.Reach)
            throw new NotSupportedException("The profile does not support an elementSize of IndexElementSize.ThirtyTwoBits; use IndexElementSize.SixteenBits or a type that has a size of two bytes.");
          else
            return IndexElementSize.ThirtyTwoBits;
        default:
          throw new ArgumentOutOfRangeException("Index buffers can only be created for types that are sixteen or thirty two bits in length");
      }
    }

    protected internal override void GraphicsDeviceResetting()
    {
      this.ibo = 0U;
    }

    private void GenerateIfRequired()
    {
      if ((int) this.ibo != 0)
        return;
      int num = this.IndexCount * (this.IndexElementSize == IndexElementSize.SixteenBits ? 2 : 4);
      GL.GenBuffers(1, out this.ibo);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ibo);
      GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) num, IntPtr.Zero, this._isDynamic ? BufferUsageHint.StreamDraw : BufferUsageHint.StaticDraw);
    }

    public void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data is null");
      if (data.Length < startIndex + elementCount)
        throw new InvalidOperationException("The array specified in the data parameter is not the correct size for the amount of data requested.");
      if (this.BufferUsage == BufferUsage.WriteOnly)
        throw new NotSupportedException("This IndexBuffer was created with a usage type of BufferUsage.WriteOnly. Calling GetData on a resource that was created with BufferUsage.WriteOnly is not supported.");
      if (Threading.IsOnUIThread())
        this.GetBufferData<T>(offsetInBytes, data, startIndex, elementCount);
      else
        Threading.BlockOnUIThread((Action) (() => this.GetBufferData<T>(offsetInBytes, data, startIndex, elementCount)));
    }

    private void GetBufferData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : struct
    {
      GL.BindBuffer(BufferTarget.ArrayBuffer, this.ibo);
      int num = Marshal.SizeOf(typeof (T));
      IntPtr source = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.ReadOnly);
      source = new IntPtr(source.ToInt64() + (long) offsetInBytes);
      if (data is byte[])
      {
        byte[] destination = data as byte[];
        Marshal.Copy(source, destination, 0, destination.Length);
      }
      else
      {
        byte[] destination = new byte[elementCount * num];
        Marshal.Copy(source, destination, 0, destination.Length);
        Buffer.BlockCopy((Array) destination, 0, (Array) data, startIndex * num, elementCount * num);
      }
      GL.UnmapBuffer(BufferTarget.ArrayBuffer);
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
      this.SetDataInternal<T>(offsetInBytes, data, startIndex, elementCount, SetDataOptions.None);
    }

    public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
    {
      this.SetDataInternal<T>(0, data, startIndex, elementCount, SetDataOptions.None);
    }

    public void SetData<T>(T[] data) where T : struct
    {
      this.SetDataInternal<T>(0, data, 0, data.Length, SetDataOptions.None);
    }

    protected void SetDataInternal<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data is null");
      if (data.Length < startIndex + elementCount)
        throw new InvalidOperationException("The array specified in the data parameter is not the correct size for the amount of data requested.");
      if (Threading.IsOnUIThread())
        this.BufferData<T>(offsetInBytes, data, startIndex, elementCount, options);
      else
        Threading.BlockOnUIThread((Action) (() => this.BufferData<T>(offsetInBytes, data, startIndex, elementCount, options)));
    }

    private void BufferData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct
    {
      this.GenerateIfRequired();
      int num1 = Marshal.SizeOf(typeof (T));
      int num2 = num1 * elementCount;
      GCHandle gcHandle = GCHandle.Alloc((object) data, GCHandleType.Pinned);
      IntPtr data1 = (IntPtr) (gcHandle.AddrOfPinnedObject().ToInt64() + (long) (startIndex * num1));
      int num3 = this.IndexCount * (this.IndexElementSize == IndexElementSize.SixteenBits ? 2 : 4);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ibo);
      if (options == SetDataOptions.Discard)
        GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) num3, IntPtr.Zero, this._isDynamic ? BufferUsageHint.StreamDraw : BufferUsageHint.StaticDraw);
      GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr) offsetInBytes, (IntPtr) num2, data1);
      gcHandle.Free();
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed)
        GraphicsDevice.AddDisposeAction((Action) (() => GL.DeleteBuffers(1, ref this.ibo)));
      base.Dispose(disposing);
    }
  }
}
