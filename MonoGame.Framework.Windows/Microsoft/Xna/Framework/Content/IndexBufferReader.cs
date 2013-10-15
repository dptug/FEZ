// Type: Microsoft.Xna.Framework.Content.IndexBufferReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
