// Type: Microsoft.Xna.Framework.Content.VertexBufferReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
