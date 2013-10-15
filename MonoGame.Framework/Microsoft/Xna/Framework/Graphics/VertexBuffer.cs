// Type: Microsoft.Xna.Framework.Graphics.VertexBuffer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  public class VertexBuffer : GraphicsResource
  {
    protected bool _isDynamic;
    internal uint vbo;

    public int VertexCount { get; private set; }

    public VertexDeclaration VertexDeclaration { get; private set; }

    public BufferUsage BufferUsage { get; private set; }

    protected VertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage bufferUsage, bool dynamic)
    {
      if (graphicsDevice == null)
        throw new ArgumentNullException("Graphics Device Cannot Be null");
      this.GraphicsDevice = graphicsDevice;
      this.VertexDeclaration = vertexDeclaration;
      this.VertexCount = vertexCount;
      this.BufferUsage = bufferUsage;
      if (vertexDeclaration.GraphicsDevice != graphicsDevice)
        vertexDeclaration.GraphicsDevice = graphicsDevice;
      this._isDynamic = dynamic;
      Threading.BlockOnUIThread(new Action(this.GenerateIfRequired));
    }

    public VertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage bufferUsage)
      : this(graphicsDevice, vertexDeclaration, vertexCount, bufferUsage, false)
    {
    }

    public VertexBuffer(GraphicsDevice graphicsDevice, Type type, int vertexCount, BufferUsage bufferUsage)
      : this(graphicsDevice, VertexDeclaration.FromType(type), vertexCount, bufferUsage, false)
    {
    }

    protected internal override void GraphicsDeviceResetting()
    {
      this.vbo = 0U;
    }

    private void GenerateIfRequired()
    {
      if ((int) this.vbo != 0)
        return;
      GL.GenBuffers(1, out this.vbo);
      GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
      GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(this.VertexDeclaration.VertexStride * this.VertexCount), IntPtr.Zero, this._isDynamic ? BufferUsageHint.StreamDraw : BufferUsageHint.StaticDraw);
    }

    public void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data is null");
      if (data.Length < startIndex + elementCount)
        throw new InvalidOperationException("The array specified in the data parameter is not the correct size for the amount of data requested.");
      if (this.BufferUsage == BufferUsage.WriteOnly)
        throw new NotSupportedException("This VertexBuffer was created with a usage type of BufferUsage.WriteOnly. Calling GetData on a resource that was created with BufferUsage.WriteOnly is not supported.");
      if (elementCount * vertexStride > this.VertexCount * this.VertexDeclaration.VertexStride)
        throw new ArgumentOutOfRangeException("The vertex stride is larger than the vertex buffer.");
      if (Threading.IsOnUIThread())
        this.GetBufferData<T>(offsetInBytes, data, startIndex, elementCount, vertexStride);
      else
        Threading.BlockOnUIThread((Action) (() => this.GetBufferData<T>(offsetInBytes, data, startIndex, elementCount, vertexStride)));
    }

    private void GetBufferData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct
    {
      GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
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
        byte[] numArray = new byte[elementCount * vertexStride - offsetInBytes];
        Marshal.Copy(source, numArray, 0, numArray.Length);
        GCHandle gcHandle = GCHandle.Alloc((object) data, GCHandleType.Pinned);
        IntPtr destination = (IntPtr) (gcHandle.AddrOfPinnedObject().ToInt64() + (long) (startIndex * num));
        int length = Marshal.SizeOf(typeof (T));
        if (length == vertexStride)
        {
          Marshal.Copy(numArray, 0, destination, numArray.Length);
        }
        else
        {
          for (int index = 0; index < elementCount; ++index)
          {
            Marshal.Copy(numArray, index * vertexStride, destination, length);
            destination = (IntPtr) (destination.ToInt64() + (long) length);
          }
        }
        gcHandle.Free();
      }
      GL.UnmapBuffer(BufferTarget.ArrayBuffer);
    }

    public void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct
    {
      int vertexStride = Marshal.SizeOf(typeof (T));
      this.GetData<T>(0, data, startIndex, elementCount, vertexStride);
    }

    public void GetData<T>(T[] data) where T : struct
    {
      int vertexStride = Marshal.SizeOf(typeof (T));
      this.GetData<T>(0, data, 0, Enumerable.Count<T>((IEnumerable<T>) data), vertexStride);
    }

    public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct
    {
      this.SetDataInternal<T>(offsetInBytes, data, startIndex, elementCount, this.VertexDeclaration.VertexStride, SetDataOptions.None);
    }

    public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
    {
      this.SetDataInternal<T>(0, data, startIndex, elementCount, this.VertexDeclaration.VertexStride, SetDataOptions.None);
    }

    public void SetData<T>(T[] data) where T : struct
    {
      this.SetDataInternal<T>(0, data, 0, data.Length, this.VertexDeclaration.VertexStride, SetDataOptions.None);
    }

    protected void SetDataInternal<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride, SetDataOptions options) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data is null");
      if (data.Length < startIndex + elementCount)
        throw new InvalidOperationException("The array specified in the data parameter is not the correct size for the amount of data requested.");
      int bufferSize = this.VertexCount * this.VertexDeclaration.VertexStride;
      if (vertexStride > bufferSize || vertexStride < this.VertexDeclaration.VertexStride)
        throw new ArgumentOutOfRangeException("One of the following conditions is true:\nThe vertex stride is larger than the vertex buffer.\nThe vertex stride is too small for the type of data requested.");
      int elementSizeInBytes = Marshal.SizeOf(typeof (T));
      if (Threading.IsOnUIThread())
        this.SetBufferData<T>(bufferSize, elementSizeInBytes, offsetInBytes, data, startIndex, elementCount, vertexStride, options);
      else
        Threading.BlockOnUIThread((Action) (() => this.SetBufferData<T>(bufferSize, elementSizeInBytes, offsetInBytes, data, startIndex, elementCount, vertexStride, options)));
    }

    private void SetBufferData<T>(int bufferSize, int elementSizeInBytes, int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride, SetDataOptions options) where T : struct
    {
      this.GenerateIfRequired();
      int num = elementSizeInBytes * elementCount;
      GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
      if (options == SetDataOptions.Discard)
        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) bufferSize, IntPtr.Zero, this._isDynamic ? BufferUsageHint.StreamDraw : BufferUsageHint.StaticDraw);
      GCHandle gcHandle = GCHandle.Alloc((object) data, GCHandleType.Pinned);
      IntPtr data1 = (IntPtr) (gcHandle.AddrOfPinnedObject().ToInt64() + (long) (startIndex * elementSizeInBytes));
      GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr) offsetInBytes, (IntPtr) num, data1);
      gcHandle.Free();
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed)
        GraphicsDevice.AddDisposeAction((Action) (() => GL.DeleteBuffers(1, ref this.vbo)));
      base.Dispose(disposing);
    }
  }
}
