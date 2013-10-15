// Type: Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
  public struct NormalizedShort4 : IPackedVector<ulong>, IPackedVector, IEquatable<NormalizedShort4>
  {
    private ulong short4Packed;

    public ulong PackedValue
    {
      get
      {
        return this.short4Packed;
      }
      set
      {
        this.short4Packed = value;
      }
    }

    public NormalizedShort4(Vector4 vector)
    {
      this.short4Packed = NormalizedShort4.PackInFour(vector.X, vector.Y, vector.Z, vector.W);
    }

    public NormalizedShort4(float x, float y, float z, float w)
    {
      this.short4Packed = NormalizedShort4.PackInFour(x, y, z, w);
    }

    public static bool operator !=(NormalizedShort4 a, NormalizedShort4 b)
    {
      return !a.Equals(b);
    }

    public static bool operator ==(NormalizedShort4 a, NormalizedShort4 b)
    {
      return a.Equals(b);
    }

    public override bool Equals(object obj)
    {
      return obj is NormalizedShort4 && this.Equals((NormalizedShort4) obj);
    }

    public bool Equals(NormalizedShort4 other)
    {
      return this.short4Packed.Equals(other.short4Packed);
    }

    public override int GetHashCode()
    {
      return this.short4Packed.GetHashCode();
    }

    public override string ToString()
    {
      return this.short4Packed.ToString("X");
    }

    private static ulong PackInFour(float vectorX, float vectorY, float vectorZ, float vectorW)
    {
      return (ulong) ((long) ((ulong) (int) MathHelper.Clamp((float) Math.Round((double) vectorX * (double) short.MaxValue), (float) short.MinValue, (float) short.MaxValue) & (ulong) uint.MaxValue) | ((long) (int) MathHelper.Clamp((float) Math.Round((double) vectorY * (double) short.MaxValue), (float) short.MinValue, (float) short.MaxValue) & (long) uint.MaxValue) << 16 | ((long) (int) MathHelper.Clamp((float) Math.Round((double) vectorZ * (double) short.MaxValue), (float) short.MinValue, (float) short.MaxValue) & (long) uint.MaxValue) << 32) | (ulong) (((long) (int) MathHelper.Clamp((float) Math.Round((double) vectorW * (double) short.MaxValue), (float) short.MinValue, (float) short.MaxValue) & (long) uint.MaxValue) << 48);
    }

    void IPackedVector.PackFromVector4(Vector4 vector)
    {
      this.short4Packed = NormalizedShort4.PackInFour(vector.X, vector.Y, vector.Z, vector.W);
    }

    public Vector4 ToVector4()
    {
      return new Vector4()
      {
        X = (float) (short) ((long) this.short4Packed & (long) uint.MaxValue) / (float) short.MaxValue,
        Y = (float) ((long) (short) (this.short4Packed >> 16) & (long) uint.MaxValue) / (float) short.MaxValue,
        Z = (float) ((long) (short) (this.short4Packed >> 32) & (long) uint.MaxValue) / (float) short.MaxValue,
        W = (float) (short) (this.short4Packed >> 48) / (float) short.MaxValue
      };
    }
  }
}
