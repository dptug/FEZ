// Type: FezEngine.Structure.Geometry.FezVertexPositionNormalTexture
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;

namespace FezEngine.Structure.Geometry
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct FezVertexPositionNormalTexture : IEquatable<FezVertexPositionNormalTexture>, ILitVertex, ITexturedVertex, IVertex, IVertexType
  {
    private static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[3]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
      new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
    });

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return FezVertexPositionNormalTexture.vertexDeclaration;
      }
    }

    public Vector3 Position { get; set; }

    public Vector3 Normal { get; set; }

    public Vector2 TextureCoordinate { get; set; }

    static FezVertexPositionNormalTexture()
    {
    }

    public FezVertexPositionNormalTexture(Vector3 position, Vector3 normal)
    {
      this = new FezVertexPositionNormalTexture();
      this.Position = position;
      this.Normal = normal;
    }

    public FezVertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 texCoord)
    {
      this = new FezVertexPositionNormalTexture(position, normal);
      this.TextureCoordinate = texCoord;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public bool Equals(FezVertexPositionNormalTexture other)
    {
      if (other.Position == this.Position)
        return other.Normal == this.Normal;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Position.GetHashCode() ^ this.Normal.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return this.Equals((FezVertexPositionNormalTexture) obj);
      else
        return false;
    }
  }
}
