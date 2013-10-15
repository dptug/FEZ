// Type: Microsoft.Xna.Framework.Graphics.VertexElementColor
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  [Serializable]
  public struct VertexElementColor
  {
    private byte R;
    private byte G;
    private byte B;
    private byte A;

    public Color Color
    {
      get
      {
        return new Color((int) this.R, (int) this.G, (int) this.B, (int) this.A);
      }
      set
      {
        this.R = value.R;
        this.G = value.G;
        this.B = value.B;
        this.A = value.A;
      }
    }

    public uint PackedValue
    {
      get
      {
        return (uint) ((((int) ((uint) (0 & -256) | (uint) this.R) & -65281 | (int) this.G << 8) & -16711681 | (int) this.B << 16) & 16777215 | (int) this.A << 24);
      }
      set
      {
        this.R = (byte) value;
        this.G = (byte) (value >> 8);
        this.B = (byte) (value >> 16);
        this.A = (byte) (value >> 24);
      }
    }

    public VertexElementColor(Color color)
    {
      this.R = color.R;
      this.G = color.G;
      this.B = color.B;
      this.A = color.A;
    }

    public static implicit operator Color(VertexElementColor typ)
    {
      return new Color()
      {
        R = typ.R,
        G = typ.G,
        B = typ.B,
        A = typ.A
      };
    }

    public static implicit operator VertexElementColor(Color typ)
    {
      return new VertexElementColor(typ);
    }

    public static bool operator ==(VertexElementColor left, Color right)
    {
      if ((int) left.R == (int) right.R && (int) left.G == (int) right.G && (int) left.B == (int) right.B)
        return (int) left.A == (int) right.A;
      else
        return false;
    }

    public static bool operator !=(VertexElementColor left, Color right)
    {
      return !(left == right);
    }

    public override string ToString()
    {
      return string.Format("[Color: R={0}, G={1}, B={2}, A={3}]", (object) this.R, (object) this.G, (object) this.B, (object) this.A);
    }

    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != this.GetType())
        return false;
      else
        return this == (Color) ((VertexElementColor) obj);
    }

    public override int GetHashCode()
    {
      return (int) this.PackedValue;
    }
  }
}
