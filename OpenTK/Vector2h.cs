// Type: OpenTK.Vector2h
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.IO;
using System.Runtime.Serialization;

namespace OpenTK
{
  [Serializable]
  public struct Vector2h : ISerializable, IEquatable<Vector2h>
  {
    public static readonly int SizeInBytes = 4;
    public Half X;
    public Half Y;

    static Vector2h()
    {
    }

    public Vector2h(Half value)
    {
      this.X = value;
      this.Y = value;
    }

    public Vector2h(float value)
    {
      this.X = new Half(value);
      this.Y = new Half(value);
    }

    public Vector2h(Half x, Half y)
    {
      this.X = x;
      this.Y = y;
    }

    public Vector2h(float x, float y)
    {
      this.X = new Half(x);
      this.Y = new Half(y);
    }

    public Vector2h(float x, float y, bool throwOnError)
    {
      this.X = new Half(x, throwOnError);
      this.Y = new Half(y, throwOnError);
    }

    [CLSCompliant(false)]
    public Vector2h(Vector2 v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
    }

    [CLSCompliant(false)]
    public Vector2h(Vector2 v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
    }

    public Vector2h(ref Vector2 v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
    }

    public Vector2h(ref Vector2 v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
    }

    public Vector2h(Vector2d v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
    }

    public Vector2h(Vector2d v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
    }

    [CLSCompliant(false)]
    public Vector2h(ref Vector2d v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
    }

    [CLSCompliant(false)]
    public Vector2h(ref Vector2d v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
    }

    public Vector2h(SerializationInfo info, StreamingContext context)
    {
      this.X = (Half) info.GetValue("X", typeof (Half));
      this.Y = (Half) info.GetValue("Y", typeof (Half));
    }

    public static explicit operator Vector2h(Vector2 v)
    {
      return new Vector2h(v);
    }

    public static explicit operator Vector2h(Vector2d v)
    {
      return new Vector2h(v);
    }

    public static explicit operator Vector2(Vector2h h)
    {
      return new Vector2((float) h.X, (float) h.Y);
    }

    public static explicit operator Vector2d(Vector2h h)
    {
      return new Vector2d((double) h.X, (double) h.Y);
    }

    public Vector2 ToVector2()
    {
      return new Vector2((float) this.X, (float) this.Y);
    }

    public Vector2d ToVector2d()
    {
      return new Vector2d((double) this.X, (double) this.Y);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("X", (float) this.X);
      info.AddValue("Y", (float) this.Y);
    }

    public void FromBinaryStream(BinaryReader bin)
    {
      this.X.FromBinaryStream(bin);
      this.Y.FromBinaryStream(bin);
    }

    public void ToBinaryStream(BinaryWriter bin)
    {
      this.X.ToBinaryStream(bin);
      this.Y.ToBinaryStream(bin);
    }

    public bool Equals(Vector2h other)
    {
      if (this.X.Equals(other.X))
        return this.Y.Equals(other.Y);
      else
        return false;
    }

    public override string ToString()
    {
      return string.Format("({0}, {1})", (object) this.X.ToString(), (object) this.Y.ToString());
    }

    public static byte[] GetBytes(Vector2h h)
    {
      byte[] numArray = new byte[Vector2h.SizeInBytes];
      byte[] bytes1 = Half.GetBytes(h.X);
      numArray[0] = bytes1[0];
      numArray[1] = bytes1[1];
      byte[] bytes2 = Half.GetBytes(h.Y);
      numArray[2] = bytes2[0];
      numArray[3] = bytes2[1];
      return numArray;
    }

    public static Vector2h FromBytes(byte[] value, int startIndex)
    {
      return new Vector2h()
      {
        X = Half.FromBytes(value, startIndex),
        Y = Half.FromBytes(value, startIndex + 2)
      };
    }
  }
}
