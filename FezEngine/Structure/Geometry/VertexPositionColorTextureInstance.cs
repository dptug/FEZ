// Type: FezEngine.Structure.Geometry.VertexPositionColorTextureInstance
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
  public struct VertexPositionColorTextureInstance : IEquatable<VertexPositionColorTextureInstance>, IShaderInstantiatableVertex, ITexturedVertex, IColoredVertex, IVertex, IVertexType
  {
    public static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[4]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
      new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
      new VertexElement(24, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
    });

    public Vector3 Position { get; set; }

    public Color Color { get; set; }

    public Vector2 TextureCoordinate { get; set; }

    public float InstanceIndex { get; set; }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return VertexPositionColorTextureInstance.vertexDeclaration;
      }
    }

    static VertexPositionColorTextureInstance()
    {
    }

    public VertexPositionColorTextureInstance(Vector3 position, Color color, Vector2 textureCoordinate)
    {
      this = new VertexPositionColorTextureInstance();
      this.Position = position;
      this.Color = color;
      this.TextureCoordinate = textureCoordinate;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public bool Equals(VertexPositionColorTextureInstance other)
    {
      if (other.Position == this.Position && other.Color == this.Color && other.TextureCoordinate == this.TextureCoordinate)
        return (double) other.InstanceIndex == (double) this.InstanceIndex;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Position.GetHashCode() ^ this.Color.GetHashCode() ^ this.TextureCoordinate.GetHashCode() ^ this.InstanceIndex.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return this.Equals((VertexPositionColorTextureInstance) obj);
      else
        return false;
    }
  }
}
