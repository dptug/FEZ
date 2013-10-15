// Type: Microsoft.Xna.Framework.Graphics.VertexColorTexture
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
