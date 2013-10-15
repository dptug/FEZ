// Type: Microsoft.Xna.Framework.Graphics.VertexPositionTexture
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct VertexPositionTexture : IVertexType
  {
    public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[2]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
    });
    public Vector3 Position;
    public Vector2 TextureCoordinate;

    VertexDeclaration IVertexType.VertexDeclaration
    {
      get
      {
        return VertexPositionTexture.VertexDeclaration;
      }
    }

    static VertexPositionTexture()
    {
    }

    public VertexPositionTexture(Vector3 position, Vector2 textureCoordinate)
    {
      this.Position = position;
      this.TextureCoordinate = textureCoordinate;
    }

    public static bool operator ==(VertexPositionTexture left, VertexPositionTexture right)
    {
      return left.Position == right.Position && left.TextureCoordinate == right.TextureCoordinate;
    }

    public static bool operator !=(VertexPositionTexture left, VertexPositionTexture right)
    {
      return !(left == right);
    }

    public override int GetHashCode()
    {
      return 0;
    }

    public override string ToString()
    {
      return string.Format("{{Position:{0} TextureCoordinate:{1}}}", new object[2]
      {
        (object) this.Position,
        (object) this.TextureCoordinate
      });
    }

    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != this.GetType())
        return false;
      else
        return this == (VertexPositionTexture) obj;
    }
  }
}
