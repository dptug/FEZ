// Type: FezEngine.Structure.Geometry.FezVertexPositionTexture
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;

namespace FezEngine.Structure.Geometry
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct FezVertexPositionTexture : IEquatable<FezVertexPositionTexture>, ITexturedVertex, IVertex, IVertexType
  {
    public static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[2]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
    });

    public Vector3 Position { get; set; }

    public Vector2 TextureCoordinate { get; set; }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return FezVertexPositionTexture.vertexDeclaration;
      }
    }

    static FezVertexPositionTexture()
    {
    }

    public FezVertexPositionTexture(Vector3 position, Vector2 textureCoordinate)
    {
      this = new FezVertexPositionTexture();
      this.Position = position;
      this.TextureCoordinate = textureCoordinate;
    }

    public override string ToString()
    {
      return string.Format("{{Position:{0} TextureCoordinate:{1}}}", (object) this.Position, (object) this.TextureCoordinate);
    }

    public bool Equals(FezVertexPositionTexture other)
    {
      if (other.Position.Equals(this.Position))
        return other.TextureCoordinate.Equals(this.TextureCoordinate);
      else
        return false;
    }
  }
}
