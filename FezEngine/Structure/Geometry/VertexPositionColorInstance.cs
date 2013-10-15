// Type: FezEngine.Structure.Geometry.VertexPositionColorInstance
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
  public struct VertexPositionColorInstance : IEquatable<VertexPositionColorInstance>, IShaderInstantiatableVertex, IColoredVertex, IVertex, IVertexType
  {
    private static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[3]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
      new VertexElement(16, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
    });

    public Vector3 Position { get; set; }

    public Color Color { get; set; }

    public float InstanceIndex { get; set; }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return VertexPositionColorInstance.vertexDeclaration;
      }
    }

    static VertexPositionColorInstance()
    {
    }

    public VertexPositionColorInstance(Vector3 position, Color color)
    {
      this = new VertexPositionColorInstance();
      this.Position = position;
      this.Color = color;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public bool Equals(VertexPositionColorInstance other)
    {
      if (other.Position == this.Position && other.Color == this.Color)
        return (double) other.InstanceIndex == (double) this.InstanceIndex;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Position.GetHashCode() ^ this.Color.GetHashCode() ^ this.InstanceIndex.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return this.Equals((VertexPositionColorInstance) obj);
      else
        return false;
    }
  }
}
