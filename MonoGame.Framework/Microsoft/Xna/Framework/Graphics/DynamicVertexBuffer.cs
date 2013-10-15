// Type: Microsoft.Xna.Framework.Graphics.DynamicVertexBuffer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class DynamicVertexBuffer : VertexBuffer
  {
    internal int UserOffset;

    public bool IsContentLost
    {
      get
      {
        return false;
      }
    }

    public DynamicVertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage bufferUsage)
      : base(graphicsDevice, vertexDeclaration, vertexCount, bufferUsage, true)
    {
    }

    public DynamicVertexBuffer(GraphicsDevice graphicsDevice, Type type, int vertexCount, BufferUsage bufferUsage)
      : base(graphicsDevice, VertexDeclaration.FromType(type), vertexCount, bufferUsage, true)
    {
    }

    public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride, SetDataOptions options) where T : struct
    {
      this.SetDataInternal<T>(offsetInBytes, data, startIndex, elementCount, vertexStride, options);
    }

    public void SetData<T>(T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct
    {
      this.SetDataInternal<T>(0, data, startIndex, elementCount, this.VertexDeclaration.VertexStride, options);
    }
  }
}
