// Type: Microsoft.Xna.Framework.Graphics.DynamicIndexBuffer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class DynamicIndexBuffer : IndexBuffer
  {
    internal int UserOffset;

    public bool IsContentLost
    {
      get
      {
        return false;
      }
    }

    public DynamicIndexBuffer(GraphicsDevice graphicsDevice, IndexElementSize indexElementSize, int indexCount, BufferUsage usage)
      : base(graphicsDevice, indexElementSize, indexCount, usage, true)
    {
    }

    public DynamicIndexBuffer(GraphicsDevice graphicsDevice, Type indexType, int indexCount, BufferUsage usage)
      : base(graphicsDevice, indexType, indexCount, usage, true)
    {
    }

    public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct
    {
      this.SetDataInternal<T>(offsetInBytes, data, startIndex, elementCount, options);
    }

    public void SetData<T>(T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct
    {
      this.SetDataInternal<T>(0, data, startIndex, elementCount, options);
    }
  }
}
