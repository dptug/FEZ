// Type: Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
  public struct HalfSingle : IPackedVector<ushort>, IEquatable<HalfSingle>, IPackedVector
  {
    private ushort packedValue;

    public ushort PackedValue
    {
      get
      {
        return this.packedValue;
      }
      set
      {
        this.packedValue = value;
      }
    }

    public HalfSingle(float single)
    {
      this.packedValue = HalfTypeHelper.convert(single);
    }

    public static bool operator ==(HalfSingle lhs, HalfSingle rhs)
    {
      return (int) lhs.packedValue == (int) rhs.packedValue;
    }

    public static bool operator !=(HalfSingle lhs, HalfSingle rhs)
    {
      return (int) lhs.packedValue != (int) rhs.packedValue;
    }

    public float ToSingle()
    {
      return HalfTypeHelper.convert(this.packedValue);
    }

    void IPackedVector.PackFromVector4(Vector4 vector)
    {
      this.packedValue = HalfTypeHelper.convert(vector.X);
    }

    Vector4 IPackedVector.ToVector4()
    {
      return new Vector4(this.ToSingle(), 0.0f, 0.0f, 1f);
    }

    public override bool Equals(object obj)
    {
      if (obj != null && obj.GetType() == this.GetType())
        return this == (HalfSingle) obj;
      else
        return false;
    }

    public bool Equals(HalfSingle other)
    {
      return (int) this.packedValue == (int) other.packedValue;
    }

    public override string ToString()
    {
      return this.ToSingle().ToString();
    }

    public override int GetHashCode()
    {
      return this.packedValue.GetHashCode();
    }
  }
}
