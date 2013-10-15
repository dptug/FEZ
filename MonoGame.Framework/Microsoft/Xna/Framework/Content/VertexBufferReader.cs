// Type: Microsoft.Xna.Framework.Content.VertexBufferReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  internal class VertexBufferReader : ContentTypeReader<VertexBuffer>
  {
    protected internal override VertexBuffer Read(ContentReader input, VertexBuffer existingInstance)
    {
      VertexDeclaration vertexDeclaration = input.ReadRawObject<VertexDeclaration>();
      int vertexCount = (int) input.ReadUInt32();
      byte[] data = input.ReadBytes(vertexCount * vertexDeclaration.VertexStride);
      VertexBuffer vertexBuffer = new VertexBuffer(input.GraphicsDevice, vertexDeclaration, vertexCount, BufferUsage.None);
      vertexBuffer.SetData<byte>(data);
      return vertexBuffer;
    }
  }
}
