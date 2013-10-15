// Type: Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort2
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
  public struct NormalizedShort2 : IPackedVector<uint>, IPackedVector, IEquatable<NormalizedShort2>
  {
    private uint short2Packed;

    public uint PackedValue
    {
      get
      {
        return this.short2Packed;
      }
      set
      {
        this.short2Packed = value;
      }
    }

    public NormalizedShort2(Vector2 vector)
    {
      this.short2Packed = NormalizedShort2.PackInTwo(vector.X, vector.Y);
    }

    public NormalizedShort2(float x, float y)
    {
      this.short2Packed = NormalizedShort2.PackInTwo(x, y);
    }

    public static bool operator !=(NormalizedShort2 a, NormalizedShort2 b)
    {
      return !a.Equals(b);
    }

    public static bool operator ==(NormalizedShort2 a, NormalizedShort2 b)
    {
      return a.Equals(b);
    }

    public override bool Equals(object obj)
    {
      return obj is NormalizedShort2 && this.Equals((NormalizedShort2) obj);
    }

    public bool Equals(NormalizedShort2 other)
    {
      return this.short2Packed.Equals(other.short2Packed);
    }

    public override int GetHashCode()
    {
      return this.short2Packed.GetHashCode();
    }

    public override string ToString()
    {
      return this.short2Packed.ToString("X");
    }

    public Vector2 ToVector2()
    {
      return new Vector2()
      {
        X = (float) (short) ((int) this.short2Packed & (int) ushort.MaxValue) / (float) short.MaxValue,
        Y = (float) (short) (this.short2Packed >> 16) / (float) short.MaxValue
      };
    }

    private static uint PackInTwo(float vectorX, float vectorY)
    {
      return (uint) (int) MathHelper.Clamp((float) Math.Round((double) vectorX * (double) short.MaxValue), (float) short.MinValue, (float) short.MaxValue) & (uint) ushort.MaxValue | (uint) (((int) MathHelper.Clamp((float) Math.Round((double) vectorY * (double) short.MaxValue), (float) short.MinValue, (float) short.MaxValue) & (int) ushort.MaxValue) << 16);
    }

    void IPackedVector.PackFromVector4(Vector4 vector)
    {
      this.short2Packed = NormalizedShort2.PackInTwo(vector.X, vector.Y);
    }

    Vector4 IPackedVector.ToVector4()
    {
      return new Vector4(0.0f, 0.0f, 0.0f, 1f)
      {
        X = (float) (short) ((int) this.short2Packed & (int) ushort.MaxValue),
        Y = (float) (short) (this.short2Packed >> 16)
      };
    }
  }
}
