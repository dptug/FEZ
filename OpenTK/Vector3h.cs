// Type: OpenTK.Vector3h
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
  public struct Vector3h : ISerializable, IEquatable<Vector3h>
  {
    public static readonly int SizeInBytes = 6;
    public Half X;
    public Half Y;
    public Half Z;

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

    static Vector3h()
    {
    }

    public Vector3h(Half value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
    }

    public Vector3h(float value)
    {
      this.X = new Half(value);
      this.Y = new Half(value);
      this.Z = new Half(value);
    }

    public Vector3h(Half x, Half y, Half z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Vector3h(float x, float y, float z)
    {
      this.X = new Half(x);
      this.Y = new Half(y);
      this.Z = new Half(z);
    }

    public Vector3h(float x, float y, float z, bool throwOnError)
    {
      this.X = new Half(x, throwOnError);
      this.Y = new Half(y, throwOnError);
      this.Z = new Half(z, throwOnError);
    }

    [CLSCompliant(false)]
    public Vector3h(Vector3 v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
      this.Z = new Half(v.Z);
    }

    [CLSCompliant(false)]
    public Vector3h(Vector3 v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
      this.Z = new Half(v.Z, throwOnError);
    }

    public Vector3h(ref Vector3 v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
      this.Z = new Half(v.Z);
    }

    public Vector3h(ref Vector3 v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
      this.Z = new Half(v.Z, throwOnError);
    }

    public Vector3h(Vector3d v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
      this.Z = new Half(v.Z);
    }

    public Vector3h(Vector3d v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
      this.Z = new Half(v.Z, throwOnError);
    }

    [CLSCompliant(false)]
    public Vector3h(ref Vector3d v)
    {
      this.X = new Half(v.X);
      this.Y = new Half(v.Y);
      this.Z = new Half(v.Z);
    }

    [CLSCompliant(false)]
    public Vector3h(ref Vector3d v, bool throwOnError)
    {
      this.X = new Half(v.X, throwOnError);
      this.Y = new Half(v.Y, throwOnError);
      this.Z = new Half(v.Z, throwOnError);
    }

    public Vector3h(SerializationInfo info, StreamingContext context)
    {
      this.X = (Half) info.GetValue("X", typeof (Half));
      this.Y = (Half) info.GetValue("Y", typeof (Half));
      this.Z = (Half) info.GetValue("Z", typeof (Half));
    }

    public static explicit operator Vector3h(Vector3 v3f)
    {
      return new Vector3h(v3f);
    }

    public static explicit operator Vector3h(Vector3d v3d)
    {
      return new Vector3h(v3d);
    }

    public static explicit operator Vector3(Vector3h h3)
    {
      return new Vector3()
      {
        X = h3.X.ToSingle(),
        Y = h3.Y.ToSingle(),
        Z = h3.Z.ToSingle()
      };
    }

    public static explicit operator Vector3d(Vector3h h3)
    {
      return new Vector3d()
      {
        X = (double) h3.X.ToSingle(),
        Y = (double) h3.Y.ToSingle(),
        Z = (double) h3.Z.ToSingle()
      };
    }

    public Vector3 ToVector3()
    {
      return new Vector3((float) this.X, (float) this.Y, (float) this.Z);
    }

    public Vector3d ToVector3d()
    {
      return new Vector3d((double) this.X, (double) this.Y, (double) this.Z);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("X", (float) this.X);
      info.AddValue("Y", (float) this.Y);
      info.AddValue("Z", (float) this.Z);
    }

    public void FromBinaryStream(BinaryReader bin)
    {
      this.X.FromBinaryStream(bin);
      this.Y.FromBinaryStream(bin);
      this.Z.FromBinaryStream(bin);
    }

    public void ToBinaryStream(BinaryWriter bin)
    {
      this.X.ToBinaryStream(bin);
      this.Y.ToBinaryStream(bin);
      this.Z.ToBinaryStream(bin);
    }

    public bool Equals(Vector3h other)
    {
      if (this.X.Equals(other.X) && this.Y.Equals(other.Y))
        return this.Z.Equals(other.Z);
      else
        return false;
    }

    public override string ToString()
    {
      return string.Format("({0}, {1}, {2})", (object) this.X.ToString(), (object) this.Y.ToString(), (object) this.Z.ToString());
    }

    public static byte[] GetBytes(Vector3h h)
    {
      byte[] numArray = new byte[Vector3h.SizeInBytes];
      byte[] bytes1 = Half.GetBytes(h.X);
      numArray[0] = bytes1[0];
      numArray[1] = bytes1[1];
      byte[] bytes2 = Half.GetBytes(h.Y);
      numArray[2] = bytes2[0];
      numArray[3] = bytes2[1];
      byte[] bytes3 = Half.GetBytes(h.Z);
      numArray[4] = bytes3[0];
      numArray[5] = bytes3[1];
      return numArray;
    }

    public static Vector3h FromBytes(byte[] value, int startIndex)
    {
      return new Vector3h()
      {
        X = Half.FromBytes(value, startIndex),
        Y = Half.FromBytes(value, startIndex + 2),
        Z = Half.FromBytes(value, startIndex + 4)
      };
    }
  }
}
