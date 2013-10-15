// Type: FezEngine.Structure.Geometry.VertexPositionTextureInstance
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
  public struct VertexPositionTextureInstance : IEquatable<VertexPositionTextureInstance>, IShaderInstantiatableVertex, ITexturedVertex, IVertex, IVertexType
  {
    private static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[3]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
      new VertexElement(20, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
    });

    public Vector3 Position { get; set; }

    public Vector2 TextureCoordinate { get; set; }

    public float InstanceIndex { get; set; }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return VertexPositionTextureInstance.vertexDeclaration;
      }
    }

    static VertexPositionTextureInstance()
    {
    }

    public VertexPositionTextureInstance(Vector3 position, Vector2 textureCoordinate)
    {
      this = new VertexPositionTextureInstance();
      this.Position = position;
      this.TextureCoordinate = textureCoordinate;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public bool Equals(VertexPositionTextureInstance other)
    {
      if (other.Position == this.Position && other.TextureCoordinate == this.TextureCoordinate)
        return (double) other.InstanceIndex == (double) this.InstanceIndex;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Position.GetHashCode() ^ this.TextureCoordinate.GetHashCode() ^ this.InstanceIndex.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return this.Equals((VertexPositionTextureInstance) obj);
      else
        return false;
    }
  }
}
