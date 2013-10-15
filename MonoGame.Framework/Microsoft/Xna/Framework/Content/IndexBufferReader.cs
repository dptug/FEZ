// Type: Microsoft.Xna.Framework.Content.IndexBufferReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  internal class IndexBufferReader : ContentTypeReader<IndexBuffer>
  {
    protected internal override IndexBuffer Read(ContentReader input, IndexBuffer existingInstance)
    {
      bool flag = input.ReadBoolean();
      int count = input.ReadInt32();
      byte[] data = input.ReadBytes(count);
      IndexBuffer indexBuffer = new IndexBuffer(input.GraphicsDevice, flag ? IndexElementSize.SixteenBits : IndexElementSize.ThirtyTwoBits, count / (flag ? 2 : 4), BufferUsage.None);
      indexBuffer.SetData<byte>(data);
      return indexBuffer;
    }
  }
}
