// Type: Microsoft.Xna.Framework.Graphics.VertexBuffer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
      VertexBuffer vertexBuffer = this;
      if (graphicsDevice == null)
        throw new ArgumentNullException("Graphics Device Cannot Be null");
      this.GraphicsDevice = graphicsDevice;
      this.VertexDeclaration = vertexDeclaration;
      this.VertexCount = vertexCount;
      this.BufferUsage = bufferUsage;
      if (vertexDeclaration.GraphicsDevice != graphicsDevice)
        vertexDeclaration.GraphicsDevice = graphicsDevice;
      this._isDynamic = dynamic;
      Threading.BlockOnUIThread((Action) (() =>
      {
        GL.GenBuffers(1, out vertexBuffer.vbo);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexDeclaration.VertexStride * vertexCount), IntPtr.Zero, dynamic ? BufferUsageHint.StreamDraw : BufferUsageHint.StaticDraw);
      }));
    }

    public VertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage bufferUsage)
      : this(graphicsDevice, vertexDeclaration, vertexCount, bufferUsage, false)
    {
    }

    public VertexBuffer(GraphicsDevice graphicsDevice, Type type, int vertexCount, BufferUsage bufferUsage)
      : this(graphicsDevice, VertexDeclaration.FromType(type), vertexCount, bufferUsage, false)
    {
    }

    public void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data is null");
      if (data.Length < startIndex + elementCount)
        throw new InvalidOperationException("The array specified in the data parameter is not the correct size for the amount of data requested.");
      if (this.BufferUsage == BufferUsage.WriteOnly)
        throw new NotSupportedException("This VertexBuffer was created with a usage type of BufferUsage.WriteOnly. Calling GetData on a resource that was created with BufferUsage.WriteOnly is not supported.");
      if (vertexStride > this.VertexCount * this.VertexDeclaration.VertexStride || vertexStride < this.VertexDeclaration.VertexStride)
        throw new ArgumentOutOfRangeException("One of the following conditions is true:\nThe vertex stride is larger than the vertex buffer.\nThe vertex stride is too small for the type of data requested.");
      Threading.BlockOnUIThread((Action) (() =>
      {
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
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
          byte[] local_2_1 = new byte[elementCount * vertexStride];
          Marshal.Copy(local_1, local_2_1, 0, local_2_1.Length);
          GCHandle local_3 = GCHandle.Alloc((object) data, GCHandleType.Pinned);
          IntPtr local_4 = (IntPtr) (local_3.AddrOfPinnedObject().ToInt64() + (long) (startIndex * local_0));
          Marshal.Copy(local_2_1, 0, local_4, local_2_1.Length);
          local_3.Free();
        }
        GL.UnmapBuffer(BufferTarget.ArrayBuffer);
      }));
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
      this.SetData<T>(0, data, startIndex, elementCount, this.VertexDeclaration.VertexStride, SetDataOptions.Discard);
    }

    public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
    {
      this.SetData<T>(0, data, startIndex, elementCount, this.VertexDeclaration.VertexStride, SetDataOptions.Discard);
    }

    public void SetData<T>(T[] data) where T : struct
    {
      this.SetData<T>(0, data, 0, data.Length, this.VertexDeclaration.VertexStride, SetDataOptions.Discard);
    }

    protected void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride, SetDataOptions options) where T : struct
    {
      if (data == null)
        throw new ArgumentNullException("data is null");
      if (data.Length < startIndex + elementCount)
        throw new InvalidOperationException("The array specified in the data parameter is not the correct size for the amount of data requested.");
      int bufferSize = this.VertexCount * this.VertexDeclaration.VertexStride;
      if (vertexStride > bufferSize || vertexStride < this.VertexDeclaration.VertexStride)
        throw new ArgumentOutOfRangeException("One of the following conditions is true:\nThe vertex stride is larger than the vertex buffer.\nThe vertex stride is too small for the type of data requested.");
      int elementSizeInBytes = Marshal.SizeOf(typeof (T));
      Threading.BlockOnUIThread((Action) (() =>
      {
        int local_0 = elementSizeInBytes * elementCount;
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
        if (options == SetDataOptions.Discard)
          GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) bufferSize, IntPtr.Zero, this._isDynamic ? BufferUsageHint.StreamDraw : BufferUsageHint.StaticDraw);
        GL.BufferSubData<T>(BufferTarget.ArrayBuffer, (IntPtr) offsetInBytes, (IntPtr) local_0, data);
      }));
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed && (this.GraphicsDevice != null && !this.GraphicsDevice.IsDisposed))
        this.GraphicsDevice.AddDisposeAction((Action) (() => GL.DeleteBuffers(1, ref this.vbo)));
      base.Dispose(disposing);
    }
  }
}
