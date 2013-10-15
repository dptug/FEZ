// Type: FezEngine.Structure.Geometry.VertexPositionInstance
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
  public struct VertexPositionInstance : IEquatable<VertexPositionInstance>, IVertex, IVertexType, IShaderInstantiatableVertex
  {
    private static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[2]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0)
    });

    public Vector3 Position { get; set; }

    public float InstanceIndex { get; set; }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return VertexPositionInstance.vertexDeclaration;
      }
    }

    static VertexPositionInstance()
    {
    }

    public VertexPositionInstance(Vector3 position)
    {
      this = new VertexPositionInstance();
      this.Position = position;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public bool Equals(VertexPositionInstance other)
    {
      if (other.Position == this.Position)
        return (double) other.InstanceIndex == (double) this.InstanceIndex;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Position.GetHashCode() ^ this.InstanceIndex.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return this.Equals((VertexPositionInstance) obj);
      else
        return false;
    }
  }
}
