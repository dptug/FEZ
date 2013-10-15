// Type: Microsoft.Xna.Framework.Graphics.VertexPositionColor
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct VertexPositionColor : IVertexType
  {
    public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[2]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
    });
    public Vector3 Position;
    public VertexElementColor Color;

    VertexDeclaration IVertexType.VertexDeclaration
    {
      get
      {
        return VertexPositionColor.VertexDeclaration;
      }
    }

    static VertexPositionColor()
    {
    }

    public VertexPositionColor(Vector3 position, Color color)
    {
      this.Position = position;
      this.Color = (VertexElementColor) color;
    }

    public static bool operator ==(VertexPositionColor left, VertexPositionColor right)
    {
      return left.Color == (Color) right.Color && left.Position == right.Position;
    }

    public static bool operator !=(VertexPositionColor left, VertexPositionColor right)
    {
      return !(left == right);
    }

    public override int GetHashCode()
    {
      return 0;
    }

    public override string ToString()
    {
      return string.Format("{{Position:{0} Color:{1}}}", new object[2]
      {
        (object) this.Position,
        (object) this.Color
      });
    }

    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != this.GetType())
        return false;
      else
        return this == (VertexPositionColor) obj;
    }
  }
}
