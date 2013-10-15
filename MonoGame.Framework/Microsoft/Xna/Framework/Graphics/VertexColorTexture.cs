// Type: Microsoft.Xna.Framework.Graphics.VertexColorTexture
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  internal struct VertexColorTexture
  {
    public Vector2 Vertex;
    public uint Color;
    public Vector2 TexCoord;

    public VertexColorTexture(Vector2 vertex, Color color, Vector2 texCoord)
    {
      this.Vertex = vertex;
      this.Color = color.PackedValue;
      this.TexCoord = texCoord;
    }
  }
}
