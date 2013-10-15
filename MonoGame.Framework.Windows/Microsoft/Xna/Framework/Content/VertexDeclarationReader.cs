// Type: Microsoft.Xna.Framework.Content.VertexDeclarationReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  public class VertexDeclarationReader : ContentTypeReader<VertexDeclaration>
  {
    protected internal override VertexDeclaration Read(ContentReader reader, VertexDeclaration existingInstance)
    {
      int vertexStride = reader.ReadInt32();
      int length = reader.ReadInt32();
      VertexElement[] vertexElementArray = new VertexElement[length];
      for (int index = 0; index < length; ++index)
      {
        int offset = reader.ReadInt32();
        VertexElementFormat elementFormat = (VertexElementFormat) reader.ReadInt32();
        VertexElementUsage elementUsage = (VertexElementUsage) reader.ReadInt32();
        int usageIndex = reader.ReadInt32();
        vertexElementArray[index] = new VertexElement(offset, elementFormat, elementUsage, usageIndex);
      }
      return new VertexDeclaration(vertexStride, vertexElementArray);
    }
  }
}
