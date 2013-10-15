// Type: Microsoft.Xna.Framework.Graphics.DynamicIndexBuffer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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

    public DynamicIndexBuffer(GraphicsDevice graphicsDevice, IndexElementSize indexElementSize, int indexCount, BufferUsage bufferUsage)
      : base(graphicsDevice, indexElementSize, indexCount, bufferUsage, true)
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
