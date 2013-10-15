// Type: SharpDX.Matrix3x2
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Matrix3x2 : IDataSerializable
  {
    public static readonly Matrix3x2 Identity = new Matrix3x2(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    public float M11;
    public float M12;
    public float M21;
    public float M22;
    public float M31;
    public float M32;

    public Vector2 Row1
    {
      get
      {
        return new Vector2(this.M11, this.M12);
      }
      set
      {
        this.M11 = value.X;
        this.M12 = value.Y;
      }
    }

    public Vector2 Row2
    {
      get
      {
        return new Vector2(this.M21, this.M22);
      }
      set
      {
        this.M21 = value.X;
        this.M22 = value.Y;
      }
    }

    public Vector2 Row3
    {
      get
      {
        return new Vector2(this.M31, this.M32);
      }
      set
      {
        this.M31 = value.X;
        this.M32 = value.Y;
      }
    }

    public Vector3 Column1
    {
      get
      {
        return new Vector3(this.M11, this.M21, this.M31);
      }
      set
      {
        this.M11 = value.X;
        this.M21 = value.Y;
        this.M31 = value.Z;
      }
    }

    public Vector3 Column2
    {
      get
      {
        return new Vector3(this.M12, this.M22, this.M32);
      }
      set
      {
        this.M12 = value.X;
        this.M22 = value.Y;
        this.M32 = value.Z;
      }
    }

    public Vector2 TranslationVector
    {
      get
      {
        return new Vector2(this.M31, this.M32);
      }
      set
      {
        this.M31 = value.X;
        this.M32 = value.Y;
      }
    }

    public Vector2 ScaleVector
    {
      get
      {
        return new Vector2(this.M11, this.M22);
      }
      set
      {
        this.M11 = value.X;
        this.M22 = value.Y;
      }
    }

    public bool IsIdentity
    {
      get
      {
        return this.Equals(Matrix3x2.Identity);
      }
    }

    public float this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.M11;
          case 1:
            return this.M12;
          case 2:
            return this.M21;
          case 3:
            return this.M22;
          case 4:
            return this.M31;
          case 5:
            return this.M32;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Matrix3x2 run from 0 to 5, inclusive.");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.M11 = value;
            break;
          case 1:
            this.M12 = value;
            break;
          case 2:
            this.M21 = value;
            break;
          case 3:
            this.M22 = value;
            break;
          case 4:
            this.M31 = value;
            break;
          case 5:
            this.M32 = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Matrix3x2 run from 0 to 5, inclusive.");
        }
      }
    }

    public float this[int row, int column]
    {
      get
      {
        if (row < 0 || row > 2)
          throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 2, inclusive.");
        if (column < 0 || column > 1)
          throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 1, inclusive.");
        else
          return this[row * 2 + column];
      }
      set
      {
        if (row < 0 || row > 2)
          throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 2, inclusive.");
        if (column < 0 || column > 1)
          throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 1, inclusive.");
        this[row * 2 + column] = value;
      }
    }

    static Matrix3x2()
    {
    }

    public Matrix3x2(float value)
    {
      this.M11 = this.M12 = this.M21 = this.M22 = this.M31 = this.M32 = value;
    }

    public Matrix3x2(float M11, float M12, float M21, float M22, float M31, float M32)
    {
      this.M11 = M11;
      this.M12 = M12;
      this.M21 = M21;
      this.M22 = M22;
      this.M31 = M31;
      this.M32 = M32;
    }

    public Matrix3x2(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 6)
        throw new ArgumentOutOfRangeException("values", "There must be only input values for Matrix3x2.");
      this.M11 = values[0];
      this.M12 = values[1];
      this.M21 = values[2];
      this.M22 = values[3];
      this.M31 = values[4];
      this.M32 = values[5];
    }

    public static implicit operator Matrix3x2(Matrix matrix)
    {
      return new Matrix3x2()
      {
        M11 = matrix.M11,
        M12 = matrix.M12,
        M21 = matrix.M21,
        M22 = matrix.M22,
        M31 = matrix.M41,
        M32 = matrix.M42
      };
    }

    public static Matrix3x2 operator +(Matrix3x2 left, Matrix3x2 right)
    {
      Matrix3x2 result;
      Matrix3x2.Add(ref left, ref right, out result);
      return result;
    }

    public static Matrix3x2 operator +(Matrix3x2 value)
    {
      return value;
    }

    public static Matrix3x2 operator -(Matrix3x2 left, Matrix3x2 right)
    {
      Matrix3x2 result;
      Matrix3x2.Subtract(ref left, ref right, out result);
      return result;
    }

    public static Matrix3x2 operator -(Matrix3x2 value)
    {
      Matrix3x2 result;
      Matrix3x2.Negate(ref value, out result);
      return result;
    }

    public static Matrix3x2 operator *(float left, Matrix3x2 right)
    {
      Matrix3x2 result;
      Matrix3x2.Multiply(ref right, left, out result);
      return result;
    }

    public static Matrix3x2 operator *(Matrix3x2 left, float right)
    {
      Matrix3x2 result;
      Matrix3x2.Multiply(ref left, right, out result);
      return result;
    }

    public static Matrix3x2 operator *(Matrix3x2 left, Matrix3x2 right)
    {
      Matrix3x2 result;
      Matrix3x2.Multiply(ref left, ref right, out result);
      return result;
    }

    public static Matrix3x2 operator /(Matrix3x2 left, float right)
    {
      Matrix3x2 result;
      Matrix3x2.Divide(ref left, right, out result);
      return result;
    }

    public static Matrix3x2 operator /(Matrix3x2 left, Matrix3x2 right)
    {
      Matrix3x2 result;
      Matrix3x2.Divide(ref left, ref right, out result);
      return result;
    }

    public static bool operator ==(Matrix3x2 left, Matrix3x2 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Matrix3x2 left, Matrix3x2 right)
    {
      return !left.Equals(right);
    }

    public float[] ToArray()
    {
      return new float[6]
      {
        this.M11,
        this.M12,
        this.M21,
        this.M22,
        this.M31,
        this.M32
      };
    }

    public static void Add(ref Matrix3x2 left, ref Matrix3x2 right, out Matrix3x2 result)
    {
      result.M11 = left.M11 + right.M11;
      result.M12 = left.M12 + right.M12;
      result.M21 = left.M21 + right.M21;
      result.M22 = left.M22 + right.M22;
      result.M31 = left.M31 + right.M31;
      result.M32 = left.M32 + right.M32;
    }

    public static Matrix3x2 Add(Matrix3x2 left, Matrix3x2 right)
    {
      Matrix3x2 result;
      Matrix3x2.Add(ref left, ref right, out result);
      return result;
    }

    public static void Subtract(ref Matrix3x2 left, ref Matrix3x2 right, out Matrix3x2 result)
    {
      result.M11 = left.M11 - right.M11;
      result.M12 = left.M12 - right.M12;
      result.M21 = left.M21 - right.M21;
      result.M22 = left.M22 - right.M22;
      result.M31 = left.M31 - right.M31;
      result.M32 = left.M32 - right.M32;
    }

    public static Matrix3x2 Subtract(Matrix3x2 left, Matrix3x2 right)
    {
      Matrix3x2 result;
      Matrix3x2.Subtract(ref left, ref right, out result);
      return result;
    }

    public static void Multiply(ref Matrix3x2 left, float right, out Matrix3x2 result)
    {
      result.M11 = left.M11 * right;
      result.M12 = left.M12 * right;
      result.M21 = left.M21 * right;
      result.M22 = left.M22 * right;
      result.M31 = left.M31 * right;
      result.M32 = left.M32 * right;
    }

    public static Matrix3x2 Multiply(Matrix3x2 left, float right)
    {
      Matrix3x2 result;
      Matrix3x2.Multiply(ref left, right, out result);
      return result;
    }

    public static void Multiply(ref Matrix3x2 left, ref Matrix3x2 right, out Matrix3x2 result)
    {
      result = new Matrix3x2();
      result.M11 = (float) ((double) left.M11 * (double) right.M11 + (double) left.M12 * (double) right.M21);
      result.M12 = (float) ((double) left.M11 * (double) right.M12 + (double) left.M12 * (double) right.M22);
      result.M21 = (float) ((double) left.M21 * (double) right.M11 + (double) left.M22 * (double) right.M21);
      result.M22 = (float) ((double) left.M21 * (double) right.M12 + (double) left.M22 * (double) right.M22);
      result.M31 = (float) ((double) left.M31 * (double) right.M11 + (double) left.M32 * (double) right.M21) + right.M31;
      result.M32 = (float) ((double) left.M31 * (double) right.M12 + (double) left.M32 * (double) right.M22) + right.M32;
    }

    public static Matrix3x2 Multiply(Matrix3x2 left, Matrix3x2 right)
    {
      Matrix3x2 result;
      Matrix3x2.Multiply(ref left, ref right, out result);
      return result;
    }

    public static void Divide(ref Matrix3x2 left, float right, out Matrix3x2 result)
    {
      float num = 1f / right;
      result.M11 = left.M11 * num;
      result.M12 = left.M12 * num;
      result.M21 = left.M21 * num;
      result.M22 = left.M22 * num;
      result.M31 = left.M31 * num;
      result.M32 = left.M32 * num;
    }

    public static void Divide(ref Matrix3x2 left, ref Matrix3x2 right, out Matrix3x2 result)
    {
      result.M11 = left.M11 / right.M11;
      result.M12 = left.M12 / right.M12;
      result.M21 = left.M21 / right.M21;
      result.M22 = left.M22 / right.M22;
      result.M31 = left.M31 / right.M31;
      result.M32 = left.M32 / right.M32;
    }

    public static void Negate(ref Matrix3x2 value, out Matrix3x2 result)
    {
      result.M11 = -value.M11;
      result.M12 = -value.M12;
      result.M21 = -value.M21;
      result.M22 = -value.M22;
      result.M31 = -value.M31;
      result.M32 = -value.M32;
    }

    public static Matrix3x2 Negate(Matrix3x2 value)
    {
      Matrix3x2 result;
      Matrix3x2.Negate(ref value, out result);
      return result;
    }

    public static void Scaling(ref Vector2 scale, out Matrix3x2 result)
    {
      Matrix3x2.Scaling(scale.X, scale.Y, out result);
    }

    public static Matrix3x2 Scaling(Vector2 scale)
    {
      Matrix3x2 result;
      Matrix3x2.Scaling(ref scale, out result);
      return result;
    }

    public static void Scaling(float x, float y, out Matrix3x2 result)
    {
      result = Matrix3x2.Identity;
      result.M11 = x;
      result.M22 = y;
    }

    public static Matrix3x2 Scaling(float x, float y)
    {
      Matrix3x2 result;
      Matrix3x2.Scaling(x, y, out result);
      return result;
    }

    public static void Scaling(float scale, out Matrix3x2 result)
    {
      result = Matrix3x2.Identity;
      result.M11 = result.M22 = scale;
    }

    public static Matrix3x2 Scaling(float scale)
    {
      Matrix3x2 result;
      Matrix3x2.Scaling(scale, out result);
      return result;
    }

    public static Matrix3x2 Scaling(float x, float y, Vector2 center)
    {
      Matrix3x2 matrix3x2;
      matrix3x2.M11 = x;
      matrix3x2.M12 = 0.0f;
      matrix3x2.M21 = 0.0f;
      matrix3x2.M22 = y;
      matrix3x2.M31 = center.X - x * center.X;
      matrix3x2.M32 = center.Y - y * center.Y;
      return matrix3x2;
    }

    public static void Scaling(float x, float y, ref Vector2 center, out Matrix3x2 result)
    {
      Matrix3x2 matrix3x2;
      matrix3x2.M11 = x;
      matrix3x2.M12 = 0.0f;
      matrix3x2.M21 = 0.0f;
      matrix3x2.M22 = y;
      matrix3x2.M31 = center.X - x * center.X;
      matrix3x2.M32 = center.Y - y * center.Y;
      result = matrix3x2;
    }

    public float Determinant()
    {
      return (float) ((double) this.M11 * (double) this.M22 - (double) this.M12 * (double) this.M21);
    }

    public static void Rotation(float angle, out Matrix3x2 result)
    {
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      result = Matrix3x2.Identity;
      result.M11 = num1;
      result.M12 = num2;
      result.M21 = -num2;
      result.M22 = num1;
    }

    public static Matrix3x2 Rotation(float angle)
    {
      Matrix3x2 result;
      Matrix3x2.Rotation(angle, out result);
      return result;
    }

    public static void Translation(ref Vector2 value, out Matrix3x2 result)
    {
      Matrix3x2.Translation(value.X, value.Y, out result);
    }

    public static Matrix3x2 Translation(Vector2 value)
    {
      Matrix3x2 result;
      Matrix3x2.Translation(ref value, out result);
      return result;
    }

    public static void Translation(float x, float y, out Matrix3x2 result)
    {
      result = Matrix3x2.Identity;
      result.M31 = x;
      result.M32 = y;
    }

    public static Matrix3x2 Translation(float x, float y)
    {
      Matrix3x2 result;
      Matrix3x2.Translation(x, y, out result);
      return result;
    }

    public static Vector2 TransformPoint(Matrix3x2 matrix, Vector2 point)
    {
      Vector2 vector2;
      vector2.X = (float) ((double) point.X * (double) matrix.M11 + (double) point.Y * (double) matrix.M21) + matrix.M31;
      vector2.Y = (float) ((double) point.X * (double) matrix.M12 + (double) point.Y * (double) matrix.M22) + matrix.M32;
      return vector2;
    }

    public static void TransformPoint(ref Matrix3x2 matrix, ref Vector2 point, out Vector2 result)
    {
      Vector2 vector2;
      vector2.X = (float) ((double) point.X * (double) matrix.M11 + (double) point.Y * (double) matrix.M21) + matrix.M31;
      vector2.Y = (float) ((double) point.X * (double) matrix.M12 + (double) point.Y * (double) matrix.M22) + matrix.M32;
      result = vector2;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]", (object) this.M11, (object) this.M12, (object) this.M21, (object) this.M22, (object) this.M31, (object) this.M32);
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format(format, (object) CultureInfo.CurrentCulture, (object) "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]", (object) this.M11.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M12.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M21.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M22.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M31.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M32.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture));
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]", (object) this.M11.ToString(formatProvider), (object) this.M12.ToString(formatProvider), (object) this.M21.ToString(formatProvider), (object) this.M22.ToString(formatProvider), (object) this.M31.ToString(formatProvider), (object) this.M32.ToString(formatProvider));
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        return this.ToString(formatProvider);
      return string.Format(format, (object) formatProvider, (object) "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]", (object) this.M11.ToString(format, formatProvider), (object) this.M12.ToString(format, formatProvider), (object) this.M21.ToString(format, formatProvider), (object) this.M22.ToString(format, formatProvider), (object) this.M31.ToString(format, formatProvider), (object) this.M32.ToString(format, formatProvider));
    }

    public override int GetHashCode()
    {
      return this.M11.GetHashCode() + this.M12.GetHashCode() + this.M21.GetHashCode() + this.M22.GetHashCode() + this.M31.GetHashCode() + this.M32.GetHashCode();
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.M11);
        serializer.Writer.Write(this.M12);
        serializer.Writer.Write(this.M21);
        serializer.Writer.Write(this.M22);
        serializer.Writer.Write(this.M31);
        serializer.Writer.Write(this.M32);
      }
      else
      {
        this.M11 = serializer.Reader.ReadSingle();
        this.M12 = serializer.Reader.ReadSingle();
        this.M21 = serializer.Reader.ReadSingle();
        this.M22 = serializer.Reader.ReadSingle();
        this.M31 = serializer.Reader.ReadSingle();
        this.M32 = serializer.Reader.ReadSingle();
      }
    }

    public bool Equals(Matrix3x2 other)
    {
      if ((double) Math.Abs(other.M11 - this.M11) < 9.99999997475243E-07 && (double) Math.Abs(other.M12 - this.M12) < 9.99999997475243E-07 && ((double) Math.Abs(other.M21 - this.M21) < 9.99999997475243E-07 && (double) Math.Abs(other.M22 - this.M22) < 9.99999997475243E-07) && (double) Math.Abs(other.M31 - this.M31) < 9.99999997475243E-07)
        return (double) Math.Abs(other.M32 - this.M32) < 9.99999997475243E-07;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Matrix3x2)))
        return false;
      else
        return this.Equals((Matrix3x2) value);
    }
  }
}
