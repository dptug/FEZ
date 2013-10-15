// Type: FezEngine.Structure.Geometry.VertexFakePointSprite
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
  public struct VertexFakePointSprite : IEquatable<VertexFakePointSprite>, IColoredVertex, ITexturedVertex, IVertex, IVertexType
  {
    private static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[4]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
      new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
      new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1)
    });
    private Vector3 _position;
    private Color _color;
    private Vector2 _textureCoordinate;
    private Vector2 _offset;

    public Vector3 Position
    {
      get
      {
        return this._position;
      }
      set
      {
        this._position = value;
      }
    }

    public Color Color
    {
      get
      {
        return this._color;
      }
      set
      {
        this._color = value;
      }
    }

    public Vector2 TextureCoordinate
    {
      get
      {
        return this._textureCoordinate;
      }
      set
      {
        this._textureCoordinate = value;
      }
    }

    public Vector2 Offset
    {
      get
      {
        return this._offset;
      }
      set
      {
        this._offset = value;
      }
    }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return VertexFakePointSprite.vertexDeclaration;
      }
    }

    static VertexFakePointSprite()
    {
    }

    public VertexFakePointSprite(Vector3 centerPosition, Color color, Vector2 texCoord, Vector2 offset)
    {
      this = new VertexFakePointSprite();
      this._position = centerPosition;
      this._color = color;
      this._textureCoordinate = texCoord;
      this._offset = offset;
    }

    public bool Equals(VertexFakePointSprite other)
    {
      if (other.Position == this.Position && other.Color == this.Color && other.TextureCoordinate == this.TextureCoordinate)
        return other.Offset == this.Offset;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Position.GetHashCode() ^ this.Color.GetHashCode() ^ this.TextureCoordinate.GetHashCode() ^ this.Offset.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return this.Equals((VertexFakePointSprite) obj);
      else
        return false;
    }
  }
}
