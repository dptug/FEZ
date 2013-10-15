// Type: Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct VertexPositionNormalTexture : IVertexType
  {
    public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[3]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
      new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
    });
    public Vector3 Position;
    public Vector3 Normal;
    public Vector2 TextureCoordinate;

    VertexDeclaration IVertexType.VertexDeclaration
    {
      get
      {
        return VertexPositionNormalTexture.VertexDeclaration;
      }
    }

    static VertexPositionNormalTexture()
    {
    }

    public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
    {
      this.Position = position;
      this.Normal = normal;
      this.TextureCoordinate = textureCoordinate;
    }

    public static bool operator ==(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
    {
      return left.Position == right.Position && left.Normal == right.Normal && left.TextureCoordinate == right.TextureCoordinate;
    }

    public static bool operator !=(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
    {
      return !(left == right);
    }

    public override int GetHashCode()
    {
      return 0;
    }

    public override string ToString()
    {
      return string.Format("{{Position:{0} Normal:{1} TextureCoordinate:{2}}}", new object[3]
      {
        (object) this.Position,
        (object) this.Normal,
        (object) this.TextureCoordinate
      });
    }

    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != this.GetType())
        return false;
      else
        return this == (VertexPositionNormalTexture) obj;
    }
  }
}
