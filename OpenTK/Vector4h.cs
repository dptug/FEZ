// Type: OpenTK.Vector4h
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OpenTK
{
  [Serializable]
  public struct Vector4h : ISerializable, IEquatable<Vector4h>
  {
    public static readonly int SizeInBytes = 8;
    public Half X;
    public Half Y;
    public Half Z;
    public Half W;

    [XmlIgnore]
    public Vector2h Xy
    {
      get
      {
        return new Vector2h(this.X, this.Y);
      }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
      }
    }

    [XmlIgnore]
    public Vector3h Xyz
    {
      get
      {
        return new Vector3h(this.X, this.Y, this.Z);
      }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
        this.Z = value.Z;
      }
    }

    static Vector4h()
    {
    }

    public Vector4h(Half value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
      this.W = value;
    }

    public Vector4h(float value)
    {
      this.X = new Half(value);
      this.Y = new Half(value);
      this.Z = new Half(value);
      this.W = new Half(value);
    }

    public Vector4h(Half x, Half y, Half z, Half w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public Vector4h(float x, float y, float z, float w)
    {
      this.X = new Half(x);
      this.Y = new Half(y);
      this.Z = new Half(z);
      this.W = new Half(w);
    }

    public Vector4h(float x, float y, float z, float w, bool throwOnError)
    {
      this.X = new Half(x, throwOnError);
      this.Y = new Half(y, throwOnError);
      this.Z = new Half(z, throwOnError);
      this.W = new Half(w, throwOnError);
    }

    [CLSCompliant(false)]
    public Vector4h(Vector4 v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
      this.Z = new Half(v.Z);
      this.W = new Half(v.W);
    }

    [CLSCompliant(false)]
    public Vector4h(Vector4 v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
      this.Z = new Half(v.Z, throwOnError);
      this.W = new Half(v.W, throwOnError);
    }

    public Vector4h(ref Vector4 v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
      this.Z = new Half(v.Z);
      this.W = new Half(v.W);
    }

    public Vector4h(ref Vector4 v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
      this.Z = new Half(v.Z, throwOnError);
      this.W = new Half(v.W, throwOnError);
    }

    public Vector4h(Vector4d v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
      this.Z = new Half(v.Z);
      this.W = new Half(v.W);
    }

    public Vector4h(Vector4d v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
      this.Z = new Half(v.Z, throwOnError);
      this.W = new Half(v.W, throwOnError);
    }

    [CLSCompliant(false)]
    public Vector4h(ref Vector4d v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
      this.Z = new Half(v.Z);
      this.W = new Half(v.W);
    }

    [CLSCompliant(false)]
    public Vector4h(ref Vector4d v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
      this.Z = new Half(v.Z, throwOnError);
      this.W = new Half(v.W, throwOnError);
    }

    public Vector4h(SerializationInfo info, StreamingContext context)
    {
      this.X = (Half) info.GetValue("X", typeof (Half));
      this.Y = (Half) info.GetValue("Y", typeof (Half));
      this.Z = (Half) info.GetValue("Z", typeof (Half));
      this.W = (Half) info.GetValue("W", typeof (Half));
    }

    public static explicit operator Vector4h(Vector4 v4f)
    {
      return new Vector4h(v4f);
    }

    public static explicit operator Vector4h(Vector4d v4d)
    {
      return new Vector4h(v4d);
    }

    public static explicit operator Vector4(Vector4h h4)
    {
      return new Vector4()
      {
        X = h4.X.ToSingle(),
        Y = h4.Y.ToSingle(),
        Z = h4.Z.ToSingle(),
        W = h4.W.ToSingle()
      };
    }

    public static explicit operator Vector4d(Vector4h h4)
    {
      return new Vector4d()
      {
        X = (double) h4.X.ToSingle(),
        Y = (double) h4.Y.ToSingle(),
        Z = (double) h4.Z.ToSingle(),
        W = (double) h4.W.ToSingle()
      };
    }

    public Vector4 ToVector4()
    {
      return new Vector4((float) this.X, (float) this.Y, (float) this.Z, (float) this.W);
    }

    public Vector4d ToVector4d()
    {
      return new Vector4d((double) this.X, (double) this.Y, (double) this.Z, (double) this.W);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("X", (float) this.X);
      info.AddValue("Y", (float) this.Y);
      info.AddValue("Z", (float) this.Z);
      info.AddValue("W", (float) this.W);
    }

    public void FromBinaryStream(BinaryReader bin)
    {
      this.X.FromBinaryStream(bin);
      this.Y.FromBinaryStream(bin);
      this.Z.FromBinaryStream(bin);
      this.W.FromBinaryStream(bin);
    }

    public void ToBinaryStream(BinaryWriter bin)
    {
      this.X.ToBinaryStream(bin);
      this.Y.ToBinaryStream(bin);
      this.Z.ToBinaryStream(bin);
      this.W.ToBinaryStream(bin);
    }

    public bool Equals(Vector4h other)
    {
      if (this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z))
        return this.W.Equals(other.W);
      else
        return false;
    }

    public override string ToString()
    {
      return string.Format("({0}, {1}, {2}, {3})", (object) this.X.ToString(), (object) this.Y.ToString(), (object) this.Z.ToString(), (object) this.W.ToString());
    }

    public static byte[] GetBytes(Vector4h h)
    {
      byte[] numArray = new byte[Vector4h.SizeInBytes];
      byte[] bytes1 = Half.GetBytes(h.X);
      numArray[0] = bytes1[0];
      numArray[1] = bytes1[1];
      byte[] bytes2 = Half.GetBytes(h.Y);
      numArray[2] = bytes2[0];
      numArray[3] = bytes2[1];
      byte[] bytes3 = Half.GetBytes(h.Z);
      numArray[4] = bytes3[0];
      numArray[5] = bytes3[1];
      byte[] bytes4 = Half.GetBytes(h.W);
      numArray[6] = bytes4[0];
      numArray[7] = bytes4[1];
      return numArray;
    }

    public static Vector4h FromBytes(byte[] value, int startIndex)
    {
      return new Vector4h()
      {
        X = Half.FromBytes(value, startIndex),
        Y = Half.FromBytes(value, startIndex + 2),
        Z = Half.FromBytes(value, startIndex + 4),
        W = Half.FromBytes(value, startIndex + 6)
      };
    }
  }
}
