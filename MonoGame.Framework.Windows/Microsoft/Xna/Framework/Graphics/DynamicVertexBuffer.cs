// Type: Microsoft.Xna.Framework.Graphics.DynamicVertexBuffer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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

    public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct
    {
      base.SetData<T>(offsetInBytes, data, startIndex, elementCount, this.VertexDeclaration.VertexStride, options);
    }

    public void SetData<T>(T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct
    {
      base.SetData<T>(0, data, startIndex, elementCount, this.VertexDeclaration.VertexStride, options);
    }
  }
}
