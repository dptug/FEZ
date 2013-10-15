// Type: Microsoft.Xna.Framework.Graphics.PackedVector.Short2
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
  public struct Short2 : IPackedVector<uint>, IPackedVector, IEquatable<Short2>
  {
    private uint _short2Packed;

    public uint PackedValue
    {
      get
      {
        return this._short2Packed;
      }
      set
      {
        this._short2Packed = value;
      }
    }

    public Short2(Vector2 vector)
    {
      this._short2Packed = Short2.PackInTwo(vector.X, vector.Y);
    }

    public Short2(float x, float y)
    {
      this._short2Packed = Short2.PackInTwo(x, y);
    }

    public static bool operator !=(Short2 a, Short2 b)
    {
      return (int) a.PackedValue != (int) b.PackedValue;
    }

    public static bool operator ==(Short2 a, Short2 b)
    {
      return (int) a.PackedValue == (int) b.PackedValue;
    }

    public override bool Equals(object obj)
    {
      if (obj is Short2)
        return this == (Short2) obj;
      else
        return false;
    }

    public bool Equals(Short2 other)
    {
      return this == other;
    }

    public override int GetHashCode()
    {
      return this._short2Packed.GetHashCode();
    }

    public override string ToString()
    {
      return this._short2Packed.ToString("x8");
    }

    public Vector2 ToVector2()
    {
      return new Vector2()
      {
        X = (float) (short) ((int) this._short2Packed & (int) ushort.MaxValue),
        Y = (float) (short) (this._short2Packed >> 16)
      };
    }

    private static uint PackInTwo(float vectorX, float vectorY)
    {
      return (uint) (int) Math.Max(Math.Min(vectorX, (float) short.MaxValue), (float) short.MinValue) & (uint) ushort.MaxValue | (uint) (((int) Math.Max(Math.Min(vectorY, (float) short.MaxValue), (float) short.MinValue) & (int) ushort.MaxValue) << 16);
    }

    void IPackedVector.PackFromVector4(Vector4 vector)
    {
      this._short2Packed = Short2.PackInTwo(vector.X, vector.Y);
    }

    Vector4 IPackedVector.ToVector4()
    {
      return new Vector4(0.0f, 0.0f, 0.0f, 1f)
      {
        X = (float) (short) ((int) this._short2Packed & (int) ushort.MaxValue),
        Y = (float) (short) (this._short2Packed >> 16)
      };
    }
  }
}
