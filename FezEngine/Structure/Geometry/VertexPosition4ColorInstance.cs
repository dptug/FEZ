// Type: FezEngine.Structure.Geometry.VertexPosition4ColorInstance
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
  public struct VertexPosition4ColorInstance : IEquatable<VertexPosition4ColorInstance>, IShaderInstantiatableVertex, IVertexType
  {
    private static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[3]
    {
      new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),
      new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color, 0),
      new VertexElement(20, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0)
    });

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return VertexPosition4ColorInstance.vertexDeclaration;
      }
    }

    public Vector4 Position { get; set; }

    public Color Color { get; set; }

    public float InstanceIndex { get; set; }

    public static int SizeInBytes
    {
      get
      {
        return 24;
      }
    }

    static VertexPosition4ColorInstance()
    {
    }

    public VertexPosition4ColorInstance(Vector4 position, Color color)
    {
      this = new VertexPosition4ColorInstance();
      this.Position = position;
      this.Color = color;
    }

    public override string ToString()
    {
      return string.Format("{{Position:{0} Color:{1}}}", (object) this.Position, (object) this.Color);
    }

    public bool Equals(VertexPosition4ColorInstance other)
    {
      if (other.Position == this.Position)
        return other.Color == this.Color;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Position.GetHashCode() ^ this.Color.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return this.Equals((VertexPosition4ColorInstance) obj);
      else
        return false;
    }
  }
}
