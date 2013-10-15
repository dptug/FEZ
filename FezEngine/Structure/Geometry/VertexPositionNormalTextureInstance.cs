// Type: FezEngine.Structure.Geometry.VertexPositionNormalTextureInstance
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
  public struct VertexPositionNormalTextureInstance : IEquatable<VertexPositionNormalTextureInstance>, IShaderInstantiatableVertex, ILitVertex, ITexturedVertex, IVertex, IVertexType
  {
    public static readonly Vector3[] ByteToNormal = new Vector3[6]
    {
      Vector3.Left,
      Vector3.Down,
      Vector3.Forward,
      Vector3.Right,
      Vector3.Up,
      Vector3.Backward
    };
    private static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[4]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
      new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
      new VertexElement(32, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
    });
    private Vector3 position;
    private Vector3 normal;
    private Vector2 textureCoordinate;
    private float instanceIndex;

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

    public Vector3 Normal
    {
      get
      {
        return this.normal;
      }
      set
      {
        this.normal = value;
      }
    }

    public Vector2 TextureCoordinate
    {
      get
      {
        return this.textureCoordinate;
      }
      set
      {
        this.textureCoordinate = value;
      }
    }

    public float InstanceIndex
    {
      get
      {
        return this.instanceIndex;
      }
      set
      {
        this.instanceIndex = value;
      }
    }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return VertexPositionNormalTextureInstance.vertexDeclaration;
      }
    }

    static VertexPositionNormalTextureInstance()
    {
    }

    public VertexPositionNormalTextureInstance(Vector3 position, Vector3 normal)
    {
      this = new VertexPositionNormalTextureInstance(position, normal, -1f);
    }

    public VertexPositionNormalTextureInstance(Vector3 position, byte normal, Vector2 textureCoordinate)
    {
      this = new VertexPositionNormalTextureInstance();
      this.position = position;
      this.normal = VertexPositionNormalTextureInstance.ByteToNormal[(int) normal];
      this.textureCoordinate = textureCoordinate;
      this.instanceIndex = -1f;
    }

    public VertexPositionNormalTextureInstance(Vector3 position, Vector3 normal, float instanceIndex)
    {
      this = new VertexPositionNormalTextureInstance();
      this.Position = position;
      this.Normal = normal;
      this.InstanceIndex = instanceIndex;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public bool Equals(VertexPositionNormalTextureInstance other)
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
        return this.Equals((VertexPositionNormalTextureInstance) obj);
      else
        return false;
    }
  }
}
