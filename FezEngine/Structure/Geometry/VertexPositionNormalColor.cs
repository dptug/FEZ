// Type: FezEngine.Structure.Geometry.VertexPositionNormalColor
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
  public struct VertexPositionNormalColor : IEquatable<VertexPositionNormalColor>, ILitVertex, IVertex, IVertexType
  {
    public static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[3]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
      new VertexElement(24, VertexElementFormat.Color, VertexElementUsage.Color, 0)
    });

    public Vector3 Position { get; set; }

    public Vector3 Normal { get; set; }

    public Color Color { get; set; }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return VertexPositionNormalColor.vertexDeclaration;
      }
    }

    static VertexPositionNormalColor()
    {
    }

    public VertexPositionNormalColor(Vector3 position, Vector3 normal, Color color)
    {
      this = new VertexPositionNormalColor();
      this.Position = position;
      this.Normal = normal;
      this.Color = color;
    }

    public override string ToString()
    {
      return string.Format("{{Position:{0} Normal:{1} Color:{2}}}", (object) this.Position, (object) this.Normal, (object) this.Color);
    }

    public bool Equals(VertexPositionNormalColor other)
    {
      if (other.Position.Equals(this.Position) && other.Normal.Equals(this.Normal))
        return other.Color.Equals(this.Color);
      else
        return false;
    }
  }
}
