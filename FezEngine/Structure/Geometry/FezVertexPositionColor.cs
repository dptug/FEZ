// Type: FezEngine.Structure.Geometry.FezVertexPositionColor
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
  public struct FezVertexPositionColor : IEquatable<FezVertexPositionColor>, IColoredVertex, IVertex, IVertexType
  {
    public static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[2]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
    });
    private Vector3 position;
    private Color color;

    public Vector3 Position
    {
      get
      {
        return this.position;
      }
      set
      {
        this.position = value;
      }
    }

    public Color Color
    {
      get
      {
        return this.color;
      }
      set
      {
        this.color = value;
      }
    }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return FezVertexPositionColor.vertexDeclaration;
      }
    }

    static FezVertexPositionColor()
    {
    }

    public FezVertexPositionColor(Vector3 position, Color color)
    {
      this = new FezVertexPositionColor();
      this.position = position;
      this.color = color;
    }

    public override string ToString()
    {
      return string.Format("{{Position:{0} Color:{1}}}", (object) this.position, (object) this.color);
    }

    public bool Equals(FezVertexPositionColor other)
    {
      if (other.position == this.position)
        return other.color == this.color;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.position.GetHashCode() ^ this.color.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return this.Equals((FezVertexPositionColor) obj);
      else
        return false;
    }
  }
}
