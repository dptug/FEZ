// Type: SharpDX.Matrix
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Design;
using SharpDX.Serialization;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [DynamicSerializer("TKMX")]
  [TypeConverter(typeof (MatrixConverter))]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Matrix : IEquatable<Matrix>, IFormattable, IDataSerializable
  {
    public static readonly int SizeInBytes = Marshal.SizeOf(typeof (Matrix));
    public static readonly Matrix Zero = new Matrix();
    public static readonly Matrix Identity = new Matrix()
    {
      M11 = 1f,
      M22 = 1f,
      M33 = 1f,
      M44 = 1f
    };
    public float M11;
    public float M12;
    public float M13;
    public float M14;
    public float M21;
    public float M22;
    public float M23;
    public float M24;
    public float M31;
    public float M32;
    public float M33;
    public float M34;
    public float M41;
    public float M42;
    public float M43;
    public float M44;

    public Vector3 Up
    {
      get
      {
        Vector3 vector3;
        vector3.X = this.M21;
        vector3.Y = this.M22;
        vector3.Z = this.M23;
        return vector3;
      }
      set
      {
        this.M21 = value.X;
        this.M22 = value.Y;
        this.M23 = value.Z;
      }
    }

    public Vector3 Down
    {
      get
      {
        Vector3 vector3;
        vector3.X = -this.M21;
        vector3.Y = -this.M22;
        vector3.Z = -this.M23;
        return vector3;
      }
      set
      {
        this.M21 = -value.X;
        this.M22 = -value.Y;
        this.M23 = -value.Z;
      }
    }

    public Vector3 Right
    {
      get
      {
        Vector3 vector3;
        vector3.X = this.M11;
        vector3.Y = this.M12;
        vector3.Z = this.M13;
        return vector3;
      }
      set
      {
        this.M11 = value.X;
        this.M12 = value.Y;
        this.M13 = value.Z;
      }
    }

    public Vector3 Left
    {
      get
      {
        Vector3 vector3;
        vector3.X = -this.M11;
        vector3.Y = -this.M12;
        vector3.Z = -this.M13;
        return vector3;
      }
      set
      {
        this.M11 = -value.X;
        this.M12 = -value.Y;
        this.M13 = -value.Z;
      }
    }

    public Vector3 Forward
    {
      get
      {
        Vector3 vector3;
        vector3.X = -this.M31;
        vector3.Y = -this.M32;
        vector3.Z = -this.M33;
        return vector3;
      }
      set
      {
        this.M31 = -value.X;
        this.M32 = -value.Y;
        this.M33 = -value.Z;
      }
    }

    public Vector3 Backward
    {
      get
      {
        Vector3 vector3;
        vector3.X = this.M31;
        vector3.Y = this.M32;
        vector3.Z = this.M33;
        return vector3;
      }
      set
      {
        this.M31 = value.X;
        this.M32 = value.Y;
        this.M33 = value.Z;
      }
    }

    public Vector4 Row1
    {
      get
      {
        return new Vector4(this.M11, this.M12, this.M13, this.M14);
      }
      set
      {
        this.M11 = value.X;
        this.M12 = value.Y;
        this.M13 = value.Z;
        this.M14 = value.W;
      }
    }

    public Vector4 Row2
    {
      get
      {
        return new Vector4(this.M21, this.M22, this.M23, this.M24);
      }
      set
      {
        this.M21 = value.X;
        this.M22 = value.Y;
        this.M23 = value.Z;
        this.M24 = value.W;
      }
    }

    public Vector4 Row3
    {
      get
      {
        return new Vector4(this.M31, this.M32, this.M33, this.M34);
      }
      set
      {
        this.M31 = value.X;
        this.M32 = value.Y;
        this.M33 = value.Z;
        this.M34 = value.W;
      }
    }

    public Vector4 Row4
    {
      get
      {
        return new Vector4(this.M41, this.M42, this.M43, this.M44);
      }
      set
      {
        this.M41 = value.X;
        this.M42 = value.Y;
        this.M43 = value.Z;
        this.M44 = value.W;
      }
    }

    public Vector4 Column1
    {
      get
      {
        return new Vector4(this.M11, this.M21, this.M31, this.M41);
      }
      set
      {
        this.M11 = value.X;
        this.M21 = value.Y;
        this.M31 = value.Z;
        this.M41 = value.W;
      }
    }

    public Vector4 Column2
    {
      get
      {
        return new Vector4(this.M12, this.M22, this.M32, this.M42);
      }
      set
      {
        this.M12 = value.X;
        this.M22 = value.Y;
        this.M32 = value.Z;
        this.M42 = value.W;
      }
    }

    public Vector4 Column3
    {
      get
      {
        return new Vector4(this.M13, this.M23, this.M33, this.M43);
      }
      set
      {
        this.M13 = value.X;
        this.M23 = value.Y;
        this.M33 = value.Z;
        this.M43 = value.W;
      }
    }

    public Vector4 Column4
    {
      get
      {
        return new Vector4(this.M14, this.M24, this.M34, this.M44);
      }
      set
      {
        this.M14 = value.X;
        this.M24 = value.Y;
        this.M34 = value.Z;
        this.M44 = value.W;
      }
    }

    public Vector3 TranslationVector
    {
      get
      {
        return new Vector3(this.M41, this.M42, this.M43);
      }
      set
      {
        this.M41 = value.X;
        this.M42 = value.Y;
        this.M43 = value.Z;
      }
    }

    public Vector3 ScaleVector
    {
      get
      {
        return new Vector3(this.M11, this.M22, this.M33);
      }
      set
      {
        this.M11 = value.X;
        this.M22 = value.Y;
        this.M33 = value.Z;
      }
    }

    public bool IsIdentity
    {
      get
      {
        return this.Equals(Matrix.Identity);
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
            return this.M13;
          case 3:
            return this.M14;
          case 4:
            return this.M21;
          case 5:
            return this.M22;
          case 6:
            return this.M23;
          case 7:
            return this.M24;
          case 8:
            return this.M31;
          case 9:
            return this.M32;
          case 10:
            return this.M33;
          case 11:
            return this.M34;
          case 12:
            return this.M41;
          case 13:
            return this.M42;
          case 14:
            return this.M43;
          case 15:
            return this.M44;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Matrix run from 0 to 15, inclusive.");
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
            this.M13 = value;
            break;
          case 3:
            this.M14 = value;
            break;
          case 4:
            this.M21 = value;
            break;
          case 5:
            this.M22 = value;
            break;
          case 6:
            this.M23 = value;
            break;
          case 7:
            this.M24 = value;
            break;
          case 8:
            this.M31 = value;
            break;
          case 9:
            this.M32 = value;
            break;
          case 10:
            this.M33 = value;
            break;
          case 11:
            this.M34 = value;
            break;
          case 12:
            this.M41 = value;
            break;
          case 13:
            this.M42 = value;
            break;
          case 14:
            this.M43 = value;
            break;
          case 15:
            this.M44 = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Matrix run from 0 to 15, inclusive.");
        }
      }
    }

    public float this[int row, int column]
    {
      get
      {
        if (row < 0 || row > 3)
          throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 3, inclusive.");
        if (column < 0 || column > 3)
          throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 3, inclusive.");
        else
          return this[row * 4 + column];
      }
      set
      {
        if (row < 0 || row > 3)
          throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 3, inclusive.");
        if (column < 0 || column > 3)
          throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 3, inclusive.");
        this[row * 4 + column] = value;
      }
    }

    static Matrix()
    {
    }

    public Matrix(float value)
    {
      this.M11 = this.M12 = this.M13 = this.M14 = this.M21 = this.M22 = this.M23 = this.M24 = this.M31 = this.M32 = this.M33 = this.M34 = this.M41 = this.M42 = this.M43 = this.M44 = value;
    }

    public Matrix(float M11, float M12, float M13, float M14, float M21, float M22, float M23, float M24, float M31, float M32, float M33, float M34, float M41, float M42, float M43, float M44)
    {
      this.M11 = M11;
      this.M12 = M12;
      this.M13 = M13;
      this.M14 = M14;
      this.M21 = M21;
      this.M22 = M22;
      this.M23 = M23;
      this.M24 = M24;
      this.M31 = M31;
      this.M32 = M32;
      this.M33 = M33;
      this.M34 = M34;
      this.M41 = M41;
      this.M42 = M42;
      this.M43 = M43;
      this.M44 = M44;
    }

    public Matrix(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 16)
        throw new ArgumentOutOfRangeException("values", "There must be sixteen and only sixteen input values for Matrix.");
      this.M11 = values[0];
      this.M12 = values[1];
      this.M13 = values[2];
      this.M14 = values[3];
      this.M21 = values[4];
      this.M22 = values[5];
      this.M23 = values[6];
      this.M24 = values[7];
      this.M31 = values[8];
      this.M32 = values[9];
      this.M33 = values[10];
      this.M34 = values[11];
      this.M41 = values[12];
      this.M42 = values[13];
      this.M43 = values[14];
      this.M44 = values[15];
    }

    public static Matrix operator +(Matrix left, Matrix right)
    {
      Matrix result;
      Matrix.Add(ref left, ref right, out result);
      return result;
    }

    public static Matrix operator +(Matrix value)
    {
      return value;
    }

    public static Matrix operator -(Matrix left, Matrix right)
    {
      Matrix result;
      Matrix.Subtract(ref left, ref right, out result);
      return result;
    }

    public static Matrix operator -(Matrix value)
    {
      Matrix result;
      Matrix.Negate(ref value, out result);
      return result;
    }

    public static Matrix operator *(float left, Matrix right)
    {
      Matrix result;
      Matrix.Multiply(ref right, left, out result);
      return result;
    }

    public static Matrix operator *(Matrix left, float right)
    {
      Matrix result;
      Matrix.Multiply(ref left, right, out result);
      return result;
    }

    public static Matrix operator *(Matrix left, Matrix right)
    {
      Matrix result;
      Matrix.Multiply(ref left, ref right, out result);
      return result;
    }

    public static Matrix operator /(Matrix left, float right)
    {
      Matrix result;
      Matrix.Divide(ref left, right, out result);
      return result;
    }

    public static Matrix operator /(Matrix left, Matrix right)
    {
      Matrix result;
      Matrix.Divide(ref left, ref right, out result);
      return result;
    }

    public static bool operator ==(Matrix left, Matrix right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Matrix left, Matrix right)
    {
      return !left.Equals(right);
    }

    public float Determinant()
    {
      float num1 = (float) ((double) this.M33 * (double) this.M44 - (double) this.M34 * (double) this.M43);
      float num2 = (float) ((double) this.M32 * (double) this.M44 - (double) this.M34 * (double) this.M42);
      float num3 = (float) ((double) this.M32 * (double) this.M43 - (double) this.M33 * (double) this.M42);
      float num4 = (float) ((double) this.M31 * (double) this.M44 - (double) this.M34 * (double) this.M41);
      float num5 = (float) ((double) this.M31 * (double) this.M43 - (double) this.M33 * (double) this.M41);
      float num6 = (float) ((double) this.M31 * (double) this.M42 - (double) this.M32 * (double) this.M41);
      return (float) ((double) this.M11 * ((double) this.M22 * (double) num1 - (double) this.M23 * (double) num2 + (double) this.M24 * (double) num3) - (double) this.M12 * ((double) this.M21 * (double) num1 - (double) this.M23 * (double) num4 + (double) this.M24 * (double) num5) + (double) this.M13 * ((double) this.M21 * (double) num2 - (double) this.M22 * (double) num4 + (double) this.M24 * (double) num6) - (double) this.M14 * ((double) this.M21 * (double) num3 - (double) this.M22 * (double) num5 + (double) this.M23 * (double) num6));
    }

    public void Invert()
    {
      Matrix.Invert(ref this, out this);
    }

    public void Transpose()
    {
      Matrix.Transpose(ref this, out this);
    }

    public void Orthogonalize()
    {
      Matrix.Orthogonalize(ref this, out this);
    }

    public void Orthonormalize()
    {
      Matrix.Orthonormalize(ref this, out this);
    }

    public void DecomposeQR(out Matrix Q, out Matrix R)
    {
      Matrix matrix = this;
      matrix.Transpose();
      Matrix.Orthonormalize(ref matrix, out Q);
      Q.Transpose();
      R = new Matrix();
      R.M11 = Vector4.Dot(Q.Column1, this.Column1);
      R.M12 = Vector4.Dot(Q.Column1, this.Column2);
      R.M13 = Vector4.Dot(Q.Column1, this.Column3);
      R.M14 = Vector4.Dot(Q.Column1, this.Column4);
      R.M22 = Vector4.Dot(Q.Column2, this.Column2);
      R.M23 = Vector4.Dot(Q.Column2, this.Column3);
      R.M24 = Vector4.Dot(Q.Column2, this.Column4);
      R.M33 = Vector4.Dot(Q.Column3, this.Column3);
      R.M34 = Vector4.Dot(Q.Column3, this.Column4);
      R.M44 = Vector4.Dot(Q.Column4, this.Column4);
    }

    public void DecomposeLQ(out Matrix L, out Matrix Q)
    {
      Matrix.Orthonormalize(ref this, out Q);
      L = new Matrix();
      L.M11 = Vector4.Dot(Q.Row1, this.Row1);
      L.M21 = Vector4.Dot(Q.Row1, this.Row2);
      L.M22 = Vector4.Dot(Q.Row2, this.Row2);
      L.M31 = Vector4.Dot(Q.Row1, this.Row3);
      L.M32 = Vector4.Dot(Q.Row2, this.Row3);
      L.M33 = Vector4.Dot(Q.Row3, this.Row3);
      L.M41 = Vector4.Dot(Q.Row1, this.Row4);
      L.M42 = Vector4.Dot(Q.Row2, this.Row4);
      L.M43 = Vector4.Dot(Q.Row3, this.Row4);
      L.M44 = Vector4.Dot(Q.Row4, this.Row4);
    }

    public bool Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation)
    {
      translation.X = this.M41;
      translation.Y = this.M42;
      translation.Z = this.M43;
      scale.X = (float) Math.Sqrt((double) this.M11 * (double) this.M11 + (double) this.M12 * (double) this.M12 + (double) this.M13 * (double) this.M13);
      scale.Y = (float) Math.Sqrt((double) this.M21 * (double) this.M21 + (double) this.M22 * (double) this.M22 + (double) this.M23 * (double) this.M23);
      scale.Z = (float) Math.Sqrt((double) this.M31 * (double) this.M31 + (double) this.M32 * (double) this.M32 + (double) this.M33 * (double) this.M33);
      if ((double) Math.Abs(scale.X) < 9.99999997475243E-07 || (double) Math.Abs(scale.Y) < 9.99999997475243E-07 || (double) Math.Abs(scale.Z) < 9.99999997475243E-07)
      {
        rotation = Quaternion.Identity;
        return false;
      }
      else
      {
        Quaternion.RotationMatrix(ref new Matrix()
        {
          M11 = this.M11 / scale.X,
          M12 = this.M12 / scale.X,
          M13 = this.M13 / scale.X,
          M21 = this.M21 / scale.Y,
          M22 = this.M22 / scale.Y,
          M23 = this.M23 / scale.Y,
          M31 = this.M31 / scale.Z,
          M32 = this.M32 / scale.Z,
          M33 = this.M33 / scale.Z,
          M44 = 1f
        }, out rotation);
        return true;
      }
    }

    public void ExchangeRows(int firstRow, int secondRow)
    {
      if (firstRow < 0)
        throw new ArgumentOutOfRangeException("firstRow", "The parameter firstRow must be greater than or equal to zero.");
      if (firstRow > 3)
        throw new ArgumentOutOfRangeException("firstRow", "The parameter firstRow must be less than or equal to three.");
      if (secondRow < 0)
        throw new ArgumentOutOfRangeException("secondRow", "The parameter secondRow must be greater than or equal to zero.");
      if (secondRow > 3)
        throw new ArgumentOutOfRangeException("secondRow", "The parameter secondRow must be less than or equal to three.");
      if (firstRow == secondRow)
        return;
      float num1 = this[secondRow, 0];
      float num2 = this[secondRow, 1];
      float num3 = this[secondRow, 2];
      float num4 = this[secondRow, 3];
      this[secondRow, 0] = this[firstRow, 0];
      this[secondRow, 1] = this[firstRow, 1];
      this[secondRow, 2] = this[firstRow, 2];
      this[secondRow, 3] = this[firstRow, 3];
      this[firstRow, 0] = num1;
      this[firstRow, 1] = num2;
      this[firstRow, 2] = num3;
      this[firstRow, 3] = num4;
    }

    public void ExchangeColumns(int firstColumn, int secondColumn)
    {
      if (firstColumn < 0)
        throw new ArgumentOutOfRangeException("firstColumn", "The parameter firstColumn must be greater than or equal to zero.");
      if (firstColumn > 3)
        throw new ArgumentOutOfRangeException("firstColumn", "The parameter firstColumn must be less than or equal to three.");
      if (secondColumn < 0)
        throw new ArgumentOutOfRangeException("secondColumn", "The parameter secondColumn must be greater than or equal to zero.");
      if (secondColumn > 3)
        throw new ArgumentOutOfRangeException("secondColumn", "The parameter secondColumn must be less than or equal to three.");
      if (firstColumn == secondColumn)
        return;
      float num1 = this[0, secondColumn];
      float num2 = this[1, secondColumn];
      float num3 = this[2, secondColumn];
      float num4 = this[3, secondColumn];
      this[0, secondColumn] = this[0, firstColumn];
      this[1, secondColumn] = this[1, firstColumn];
      this[2, secondColumn] = this[2, firstColumn];
      this[3, secondColumn] = this[3, firstColumn];
      this[0, firstColumn] = num1;
      this[1, firstColumn] = num2;
      this[2, firstColumn] = num3;
      this[3, firstColumn] = num4;
    }

    public float[] ToArray()
    {
      return new float[16]
      {
        this.M11,
        this.M12,
        this.M13,
        this.M14,
        this.M21,
        this.M22,
        this.M23,
        this.M24,
        this.M31,
        this.M32,
        this.M33,
        this.M34,
        this.M41,
        this.M42,
        this.M43,
        this.M44
      };
    }

    public static void Add(ref Matrix left, ref Matrix right, out Matrix result)
    {
      result.M11 = left.M11 + right.M11;
      result.M12 = left.M12 + right.M12;
      result.M13 = left.M13 + right.M13;
      result.M14 = left.M14 + right.M14;
      result.M21 = left.M21 + right.M21;
      result.M22 = left.M22 + right.M22;
      result.M23 = left.M23 + right.M23;
      result.M24 = left.M24 + right.M24;
      result.M31 = left.M31 + right.M31;
      result.M32 = left.M32 + right.M32;
      result.M33 = left.M33 + right.M33;
      result.M34 = left.M34 + right.M34;
      result.M41 = left.M41 + right.M41;
      result.M42 = left.M42 + right.M42;
      result.M43 = left.M43 + right.M43;
      result.M44 = left.M44 + right.M44;
    }

    public static Matrix Add(Matrix left, Matrix right)
    {
      Matrix result;
      Matrix.Add(ref left, ref right, out result);
      return result;
    }

    public static void Subtract(ref Matrix left, ref Matrix right, out Matrix result)
    {
      result.M11 = left.M11 - right.M11;
      result.M12 = left.M12 - right.M12;
      result.M13 = left.M13 - right.M13;
      result.M14 = left.M14 - right.M14;
      result.M21 = left.M21 - right.M21;
      result.M22 = left.M22 - right.M22;
      result.M23 = left.M23 - right.M23;
      result.M24 = left.M24 - right.M24;
      result.M31 = left.M31 - right.M31;
      result.M32 = left.M32 - right.M32;
      result.M33 = left.M33 - right.M33;
      result.M34 = left.M34 - right.M34;
      result.M41 = left.M41 - right.M41;
      result.M42 = left.M42 - right.M42;
      result.M43 = left.M43 - right.M43;
      result.M44 = left.M44 - right.M44;
    }

    public static Matrix Subtract(Matrix left, Matrix right)
    {
      Matrix result;
      Matrix.Subtract(ref left, ref right, out result);
      return result;
    }

    public static void Multiply(ref Matrix left, float right, out Matrix result)
    {
      result.M11 = left.M11 * right;
      result.M12 = left.M12 * right;
      result.M13 = left.M13 * right;
      result.M14 = left.M14 * right;
      result.M21 = left.M21 * right;
      result.M22 = left.M22 * right;
      result.M23 = left.M23 * right;
      result.M24 = left.M24 * right;
      result.M31 = left.M31 * right;
      result.M32 = left.M32 * right;
      result.M33 = left.M33 * right;
      result.M34 = left.M34 * right;
      result.M41 = left.M41 * right;
      result.M42 = left.M42 * right;
      result.M43 = left.M43 * right;
      result.M44 = left.M44 * right;
    }

    public static Matrix Multiply(Matrix left, float right)
    {
      Matrix result;
      Matrix.Multiply(ref left, right, out result);
      return result;
    }

    public static void Multiply(ref Matrix left, ref Matrix right, out Matrix result)
    {
      result = new Matrix();
      result.M11 = (float) ((double) left.M11 * (double) right.M11 + (double) left.M12 * (double) right.M21 + (double) left.M13 * (double) right.M31 + (double) left.M14 * (double) right.M41);
      result.M12 = (float) ((double) left.M11 * (double) right.M12 + (double) left.M12 * (double) right.M22 + (double) left.M13 * (double) right.M32 + (double) left.M14 * (double) right.M42);
      result.M13 = (float) ((double) left.M11 * (double) right.M13 + (double) left.M12 * (double) right.M23 + (double) left.M13 * (double) right.M33 + (double) left.M14 * (double) right.M43);
      result.M14 = (float) ((double) left.M11 * (double) right.M14 + (double) left.M12 * (double) right.M24 + (double) left.M13 * (double) right.M34 + (double) left.M14 * (double) right.M44);
      result.M21 = (float) ((double) left.M21 * (double) right.M11 + (double) left.M22 * (double) right.M21 + (double) left.M23 * (double) right.M31 + (double) left.M24 * (double) right.M41);
      result.M22 = (float) ((double) left.M21 * (double) right.M12 + (double) left.M22 * (double) right.M22 + (double) left.M23 * (double) right.M32 + (double) left.M24 * (double) right.M42);
      result.M23 = (float) ((double) left.M21 * (double) right.M13 + (double) left.M22 * (double) right.M23 + (double) left.M23 * (double) right.M33 + (double) left.M24 * (double) right.M43);
      result.M24 = (float) ((double) left.M21 * (double) right.M14 + (double) left.M22 * (double) right.M24 + (double) left.M23 * (double) right.M34 + (double) left.M24 * (double) right.M44);
      result.M31 = (float) ((double) left.M31 * (double) right.M11 + (double) left.M32 * (double) right.M21 + (double) left.M33 * (double) right.M31 + (double) left.M34 * (double) right.M41);
      result.M32 = (float) ((double) left.M31 * (double) right.M12 + (double) left.M32 * (double) right.M22 + (double) left.M33 * (double) right.M32 + (double) left.M34 * (double) right.M42);
      result.M33 = (float) ((double) left.M31 * (double) right.M13 + (double) left.M32 * (double) right.M23 + (double) left.M33 * (double) right.M33 + (double) left.M34 * (double) right.M43);
      result.M34 = (float) ((double) left.M31 * (double) right.M14 + (double) left.M32 * (double) right.M24 + (double) left.M33 * (double) right.M34 + (double) left.M34 * (double) right.M44);
      result.M41 = (float) ((double) left.M41 * (double) right.M11 + (double) left.M42 * (double) right.M21 + (double) left.M43 * (double) right.M31 + (double) left.M44 * (double) right.M41);
      result.M42 = (float) ((double) left.M41 * (double) right.M12 + (double) left.M42 * (double) right.M22 + (double) left.M43 * (double) right.M32 + (double) left.M44 * (double) right.M42);
      result.M43 = (float) ((double) left.M41 * (double) right.M13 + (double) left.M42 * (double) right.M23 + (double) left.M43 * (double) right.M33 + (double) left.M44 * (double) right.M43);
      result.M44 = (float) ((double) left.M41 * (double) right.M14 + (double) left.M42 * (double) right.M24 + (double) left.M43 * (double) right.M34 + (double) left.M44 * (double) right.M44);
    }

    public static Matrix Multiply(Matrix left, Matrix right)
    {
      Matrix result;
      Matrix.Multiply(ref left, ref right, out result);
      return result;
    }

    public static void Divide(ref Matrix left, float right, out Matrix result)
    {
      float num = 1f / right;
      result.M11 = left.M11 * num;
      result.M12 = left.M12 * num;
      result.M13 = left.M13 * num;
      result.M14 = left.M14 * num;
      result.M21 = left.M21 * num;
      result.M22 = left.M22 * num;
      result.M23 = left.M23 * num;
      result.M24 = left.M24 * num;
      result.M31 = left.M31 * num;
      result.M32 = left.M32 * num;
      result.M33 = left.M33 * num;
      result.M34 = left.M34 * num;
      result.M41 = left.M41 * num;
      result.M42 = left.M42 * num;
      result.M43 = left.M43 * num;
      result.M44 = left.M44 * num;
    }

    public static Matrix Divide(Matrix left, float right)
    {
      Matrix result;
      Matrix.Divide(ref left, right, out result);
      return result;
    }

    public static void Divide(ref Matrix left, ref Matrix right, out Matrix result)
    {
      result.M11 = left.M11 / right.M11;
      result.M12 = left.M12 / right.M12;
      result.M13 = left.M13 / right.M13;
      result.M14 = left.M14 / right.M14;
      result.M21 = left.M21 / right.M21;
      result.M22 = left.M22 / right.M22;
      result.M23 = left.M23 / right.M23;
      result.M24 = left.M24 / right.M24;
      result.M31 = left.M31 / right.M31;
      result.M32 = left.M32 / right.M32;
      result.M33 = left.M33 / right.M33;
      result.M34 = left.M34 / right.M34;
      result.M41 = left.M41 / right.M41;
      result.M42 = left.M42 / right.M42;
      result.M43 = left.M43 / right.M43;
      result.M44 = left.M44 / right.M44;
    }

    public static Matrix Divide(Matrix left, Matrix right)
    {
      Matrix result;
      Matrix.Divide(ref left, ref right, out result);
      return result;
    }

    public static void Exponent(ref Matrix value, int exponent, out Matrix result)
    {
      if (exponent < 0)
        throw new ArgumentOutOfRangeException("exponent", "The exponent can not be negative.");
      if (exponent == 0)
        result = Matrix.Identity;
      else if (exponent == 1)
      {
        result = value;
      }
      else
      {
        Matrix matrix1 = Matrix.Identity;
        Matrix matrix2 = value;
        while (true)
        {
          if ((exponent & 1) != 0)
            matrix1 *= matrix2;
          exponent /= 2;
          if (exponent > 0)
            matrix2 *= matrix2;
          else
            break;
        }
        result = matrix1;
      }
    }

    public static Matrix Exponent(Matrix value, int exponent)
    {
      Matrix result;
      Matrix.Exponent(ref value, exponent, out result);
      return result;
    }

    public static void Negate(ref Matrix value, out Matrix result)
    {
      result.M11 = -value.M11;
      result.M12 = -value.M12;
      result.M13 = -value.M13;
      result.M14 = -value.M14;
      result.M21 = -value.M21;
      result.M22 = -value.M22;
      result.M23 = -value.M23;
      result.M24 = -value.M24;
      result.M31 = -value.M31;
      result.M32 = -value.M32;
      result.M33 = -value.M33;
      result.M34 = -value.M34;
      result.M41 = -value.M41;
      result.M42 = -value.M42;
      result.M43 = -value.M43;
      result.M44 = -value.M44;
    }

    public static Matrix Negate(Matrix value)
    {
      Matrix result;
      Matrix.Negate(ref value, out result);
      return result;
    }

    public static void Lerp(ref Matrix start, ref Matrix end, float amount, out Matrix result)
    {
      result.M11 = start.M11 + (end.M11 - start.M11) * amount;
      result.M12 = start.M12 + (end.M12 - start.M12) * amount;
      result.M13 = start.M13 + (end.M13 - start.M13) * amount;
      result.M14 = start.M14 + (end.M14 - start.M14) * amount;
      result.M21 = start.M21 + (end.M21 - start.M21) * amount;
      result.M22 = start.M22 + (end.M22 - start.M22) * amount;
      result.M23 = start.M23 + (end.M23 - start.M23) * amount;
      result.M24 = start.M24 + (end.M24 - start.M24) * amount;
      result.M31 = start.M31 + (end.M31 - start.M31) * amount;
      result.M32 = start.M32 + (end.M32 - start.M32) * amount;
      result.M33 = start.M33 + (end.M33 - start.M33) * amount;
      result.M34 = start.M34 + (end.M34 - start.M34) * amount;
      result.M41 = start.M41 + (end.M41 - start.M41) * amount;
      result.M42 = start.M42 + (end.M42 - start.M42) * amount;
      result.M43 = start.M43 + (end.M43 - start.M43) * amount;
      result.M44 = start.M44 + (end.M44 - start.M44) * amount;
    }

    public static Matrix Lerp(Matrix start, Matrix end, float amount)
    {
      Matrix result;
      Matrix.Lerp(ref start, ref end, amount, out result);
      return result;
    }

    public static void SmoothStep(ref Matrix start, ref Matrix end, float amount, out Matrix result)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      result.M11 = start.M11 + (end.M11 - start.M11) * amount;
      result.M12 = start.M12 + (end.M12 - start.M12) * amount;
      result.M13 = start.M13 + (end.M13 - start.M13) * amount;
      result.M14 = start.M14 + (end.M14 - start.M14) * amount;
      result.M21 = start.M21 + (end.M21 - start.M21) * amount;
      result.M22 = start.M22 + (end.M22 - start.M22) * amount;
      result.M23 = start.M23 + (end.M23 - start.M23) * amount;
      result.M24 = start.M24 + (end.M24 - start.M24) * amount;
      result.M31 = start.M31 + (end.M31 - start.M31) * amount;
      result.M32 = start.M32 + (end.M32 - start.M32) * amount;
      result.M33 = start.M33 + (end.M33 - start.M33) * amount;
      result.M34 = start.M34 + (end.M34 - start.M34) * amount;
      result.M41 = start.M41 + (end.M41 - start.M41) * amount;
      result.M42 = start.M42 + (end.M42 - start.M42) * amount;
      result.M43 = start.M43 + (end.M43 - start.M43) * amount;
      result.M44 = start.M44 + (end.M44 - start.M44) * amount;
    }

    public static Matrix SmoothStep(Matrix start, Matrix end, float amount)
    {
      Matrix result;
      Matrix.SmoothStep(ref start, ref end, amount, out result);
      return result;
    }

    public static void Transpose(ref Matrix value, out Matrix result)
    {
      result = new Matrix()
      {
        M11 = value.M11,
        M12 = value.M21,
        M13 = value.M31,
        M14 = value.M41,
        M21 = value.M12,
        M22 = value.M22,
        M23 = value.M32,
        M24 = value.M42,
        M31 = value.M13,
        M32 = value.M23,
        M33 = value.M33,
        M34 = value.M43,
        M41 = value.M14,
        M42 = value.M24,
        M43 = value.M34,
        M44 = value.M44
      };
    }

    public static void TransposeByRef(ref Matrix value, ref Matrix result)
    {
      result.M11 = value.M11;
      result.M12 = value.M21;
      result.M13 = value.M31;
      result.M14 = value.M41;
      result.M21 = value.M12;
      result.M22 = value.M22;
      result.M23 = value.M32;
      result.M24 = value.M42;
      result.M31 = value.M13;
      result.M32 = value.M23;
      result.M33 = value.M33;
      result.M34 = value.M43;
      result.M41 = value.M14;
      result.M42 = value.M24;
      result.M43 = value.M34;
      result.M44 = value.M44;
    }

    public static Matrix Transpose(Matrix value)
    {
      Matrix result;
      Matrix.Transpose(ref value, out result);
      return result;
    }

    public static void Invert(ref Matrix value, out Matrix result)
    {
      float num1 = (float) ((double) value.M31 * (double) value.M42 - (double) value.M32 * (double) value.M41);
      float num2 = (float) ((double) value.M31 * (double) value.M43 - (double) value.M33 * (double) value.M41);
      float num3 = (float) ((double) value.M34 * (double) value.M41 - (double) value.M31 * (double) value.M44);
      float num4 = (float) ((double) value.M32 * (double) value.M43 - (double) value.M33 * (double) value.M42);
      float num5 = (float) ((double) value.M34 * (double) value.M42 - (double) value.M32 * (double) value.M44);
      float num6 = (float) ((double) value.M33 * (double) value.M44 - (double) value.M34 * (double) value.M43);
      float num7 = (float) ((double) value.M22 * (double) num6 + (double) value.M23 * (double) num5 + (double) value.M24 * (double) num4);
      float num8 = (float) ((double) value.M21 * (double) num6 + (double) value.M23 * (double) num3 + (double) value.M24 * (double) num2);
      float num9 = (float) ((double) value.M21 * -(double) num5 + (double) value.M22 * (double) num3 + (double) value.M24 * (double) num1);
      float num10 = (float) ((double) value.M21 * (double) num4 + (double) value.M22 * -(double) num2 + (double) value.M23 * (double) num1);
      float num11 = (float) ((double) value.M11 * (double) num7 - (double) value.M12 * (double) num8 + (double) value.M13 * (double) num9 - (double) value.M14 * (double) num10);
      if ((double) Math.Abs(num11) <= 9.99999997475243E-07)
      {
        result = Matrix.Zero;
      }
      else
      {
        float num12 = 1f / num11;
        float num13 = (float) ((double) value.M11 * (double) value.M22 - (double) value.M12 * (double) value.M21);
        float num14 = (float) ((double) value.M11 * (double) value.M23 - (double) value.M13 * (double) value.M21);
        float num15 = (float) ((double) value.M14 * (double) value.M21 - (double) value.M11 * (double) value.M24);
        float num16 = (float) ((double) value.M12 * (double) value.M23 - (double) value.M13 * (double) value.M22);
        float num17 = (float) ((double) value.M14 * (double) value.M22 - (double) value.M12 * (double) value.M24);
        float num18 = (float) ((double) value.M13 * (double) value.M24 - (double) value.M14 * (double) value.M23);
        float num19 = (float) ((double) value.M12 * (double) num6 + (double) value.M13 * (double) num5 + (double) value.M14 * (double) num4);
        float num20 = (float) ((double) value.M11 * (double) num6 + (double) value.M13 * (double) num3 + (double) value.M14 * (double) num2);
        float num21 = (float) ((double) value.M11 * -(double) num5 + (double) value.M12 * (double) num3 + (double) value.M14 * (double) num1);
        float num22 = (float) ((double) value.M11 * (double) num4 + (double) value.M12 * -(double) num2 + (double) value.M13 * (double) num1);
        float num23 = (float) ((double) value.M42 * (double) num18 + (double) value.M43 * (double) num17 + (double) value.M44 * (double) num16);
        float num24 = (float) ((double) value.M41 * (double) num18 + (double) value.M43 * (double) num15 + (double) value.M44 * (double) num14);
        float num25 = (float) ((double) value.M41 * -(double) num17 + (double) value.M42 * (double) num15 + (double) value.M44 * (double) num13);
        float num26 = (float) ((double) value.M41 * (double) num16 + (double) value.M42 * -(double) num14 + (double) value.M43 * (double) num13);
        float num27 = (float) ((double) value.M32 * (double) num18 + (double) value.M33 * (double) num17 + (double) value.M34 * (double) num16);
        float num28 = (float) ((double) value.M31 * (double) num18 + (double) value.M33 * (double) num15 + (double) value.M34 * (double) num14);
        float num29 = (float) ((double) value.M31 * -(double) num17 + (double) value.M32 * (double) num15 + (double) value.M34 * (double) num13);
        float num30 = (float) ((double) value.M31 * (double) num16 + (double) value.M32 * -(double) num14 + (double) value.M33 * (double) num13);
        result.M11 = num7 * num12;
        result.M12 = -num19 * num12;
        result.M13 = num23 * num12;
        result.M14 = -num27 * num12;
        result.M21 = -num8 * num12;
        result.M22 = num20 * num12;
        result.M23 = -num24 * num12;
        result.M24 = num28 * num12;
        result.M31 = num9 * num12;
        result.M32 = -num21 * num12;
        result.M33 = num25 * num12;
        result.M34 = -num29 * num12;
        result.M41 = -num10 * num12;
        result.M42 = num22 * num12;
        result.M43 = -num26 * num12;
        result.M44 = num30 * num12;
      }
    }

    public static Matrix Invert(Matrix value)
    {
      value.Invert();
      return value;
    }

    public static void Orthogonalize(ref Matrix value, out Matrix result)
    {
      result = value;
      result.Row2 = result.Row2 - Vector4.Dot(result.Row1, result.Row2) / Vector4.Dot(result.Row1, result.Row1) * result.Row1;
      result.Row3 = result.Row3 - Vector4.Dot(result.Row1, result.Row3) / Vector4.Dot(result.Row1, result.Row1) * result.Row1;
      result.Row3 = result.Row3 - Vector4.Dot(result.Row2, result.Row3) / Vector4.Dot(result.Row2, result.Row2) * result.Row2;
      result.Row4 = result.Row4 - Vector4.Dot(result.Row1, result.Row4) / Vector4.Dot(result.Row1, result.Row1) * result.Row1;
      result.Row4 = result.Row4 - Vector4.Dot(result.Row2, result.Row4) / Vector4.Dot(result.Row2, result.Row2) * result.Row2;
      result.Row4 = result.Row4 - Vector4.Dot(result.Row3, result.Row4) / Vector4.Dot(result.Row3, result.Row3) * result.Row3;
    }

    public static Matrix Orthogonalize(Matrix value)
    {
      Matrix result;
      Matrix.Orthogonalize(ref value, out result);
      return result;
    }

    public static void Orthonormalize(ref Matrix value, out Matrix result)
    {
      result = value;
      result.Row1 = Vector4.Normalize(result.Row1);
      result.Row2 = result.Row2 - Vector4.Dot(result.Row1, result.Row2) * result.Row1;
      result.Row2 = Vector4.Normalize(result.Row2);
      result.Row3 = result.Row3 - Vector4.Dot(result.Row1, result.Row3) * result.Row1;
      result.Row3 = result.Row3 - Vector4.Dot(result.Row2, result.Row3) * result.Row2;
      result.Row3 = Vector4.Normalize(result.Row3);
      result.Row4 = result.Row4 - Vector4.Dot(result.Row1, result.Row4) * result.Row1;
      result.Row4 = result.Row4 - Vector4.Dot(result.Row2, result.Row4) * result.Row2;
      result.Row4 = result.Row4 - Vector4.Dot(result.Row3, result.Row4) * result.Row3;
      result.Row4 = Vector4.Normalize(result.Row4);
    }

    public static Matrix Orthonormalize(Matrix value)
    {
      Matrix result;
      Matrix.Orthonormalize(ref value, out result);
      return result;
    }

    public static void UpperTriangularForm(ref Matrix value, out Matrix result)
    {
      result = value;
      int index1 = 0;
      int num1 = 4;
      int num2 = 4;
      for (int secondRow = 0; secondRow < num1 && num2 > index1; ++secondRow)
      {
        int firstRow = secondRow;
        while ((double) Math.Abs(result[firstRow, index1]) < 9.99999997475243E-07)
        {
          ++firstRow;
          if (firstRow == num1)
          {
            firstRow = secondRow;
            ++index1;
            if (index1 == num2)
              return;
          }
        }
        if (firstRow != secondRow)
          result.ExchangeRows(firstRow, secondRow);
        float num3 = 1f / result[secondRow, index1];
        for (; firstRow < num1; ++firstRow)
        {
          if (firstRow != secondRow)
          {
            // ISSUE: variable of a reference type
            Matrix& local1;
            int index2;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local1 = @result))[index2 = firstRow, 0] = (^local1)[index2, 0] - result[secondRow, 0] * num3 * result[firstRow, index1];
            // ISSUE: variable of a reference type
            Matrix& local2;
            int index3;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local2 = @result))[index3 = firstRow, 1] = (^local2)[index3, 1] - result[secondRow, 1] * num3 * result[firstRow, index1];
            // ISSUE: variable of a reference type
            Matrix& local3;
            int index4;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local3 = @result))[index4 = firstRow, 2] = (^local3)[index4, 2] - result[secondRow, 2] * num3 * result[firstRow, index1];
            // ISSUE: variable of a reference type
            Matrix& local4;
            int index5;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local4 = @result))[index5 = firstRow, 3] = (^local4)[index5, 3] - result[secondRow, 3] * num3 * result[firstRow, index1];
          }
        }
        ++index1;
      }
    }

    public static Matrix UpperTriangularForm(Matrix value)
    {
      Matrix result;
      Matrix.UpperTriangularForm(ref value, out result);
      return result;
    }

    public static void LowerTriangularForm(ref Matrix value, out Matrix result)
    {
      Matrix matrix = value;
      Matrix.Transpose(ref matrix, out result);
      int index1 = 0;
      int num1 = 4;
      int num2 = 4;
      for (int secondRow = 0; secondRow < num1; ++secondRow)
      {
        if (num2 <= index1)
          return;
        int firstRow = secondRow;
        while ((double) Math.Abs(result[firstRow, index1]) < 9.99999997475243E-07)
        {
          ++firstRow;
          if (firstRow == num1)
          {
            firstRow = secondRow;
            ++index1;
            if (index1 == num2)
              return;
          }
        }
        if (firstRow != secondRow)
          result.ExchangeRows(firstRow, secondRow);
        float num3 = 1f / result[secondRow, index1];
        for (; firstRow < num1; ++firstRow)
        {
          if (firstRow != secondRow)
          {
            // ISSUE: variable of a reference type
            Matrix& local1;
            int index2;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local1 = @result))[index2 = firstRow, 0] = (^local1)[index2, 0] - result[secondRow, 0] * num3 * result[firstRow, index1];
            // ISSUE: variable of a reference type
            Matrix& local2;
            int index3;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local2 = @result))[index3 = firstRow, 1] = (^local2)[index3, 1] - result[secondRow, 1] * num3 * result[firstRow, index1];
            // ISSUE: variable of a reference type
            Matrix& local3;
            int index4;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local3 = @result))[index4 = firstRow, 2] = (^local3)[index4, 2] - result[secondRow, 2] * num3 * result[firstRow, index1];
            // ISSUE: variable of a reference type
            Matrix& local4;
            int index5;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local4 = @result))[index5 = firstRow, 3] = (^local4)[index5, 3] - result[secondRow, 3] * num3 * result[firstRow, index1];
          }
        }
        ++index1;
      }
      Matrix.Transpose(ref result, out result);
    }

    public static Matrix LowerTriangularForm(Matrix value)
    {
      Matrix result;
      Matrix.LowerTriangularForm(ref value, out result);
      return result;
    }

    public static void RowEchelonForm(ref Matrix value, out Matrix result)
    {
      result = value;
      int index1 = 0;
      int num1 = 4;
      int num2 = 4;
      for (int secondRow = 0; secondRow < num1 && num2 > index1; ++secondRow)
      {
        int firstRow = secondRow;
        while ((double) Math.Abs(result[firstRow, index1]) < 9.99999997475243E-07)
        {
          ++firstRow;
          if (firstRow == num1)
          {
            firstRow = secondRow;
            ++index1;
            if (index1 == num2)
              return;
          }
        }
        if (firstRow != secondRow)
          result.ExchangeRows(firstRow, secondRow);
        float num3 = 1f / result[secondRow, index1];
        // ISSUE: variable of a reference type
        Matrix& local1;
        int index2;
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        (^(local1 = @result))[index2 = secondRow, 0] = (^local1)[index2, 0] * num3;
        // ISSUE: variable of a reference type
        Matrix& local2;
        int index3;
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        (^(local2 = @result))[index3 = secondRow, 1] = (^local2)[index3, 1] * num3;
        // ISSUE: variable of a reference type
        Matrix& local3;
        int index4;
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        (^(local3 = @result))[index4 = secondRow, 2] = (^local3)[index4, 2] * num3;
        // ISSUE: variable of a reference type
        Matrix& local4;
        int index5;
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        (^(local4 = @result))[index5 = secondRow, 3] = (^local4)[index5, 3] * num3;
        for (; firstRow < num1; ++firstRow)
        {
          if (firstRow != secondRow)
          {
            // ISSUE: variable of a reference type
            Matrix& local5;
            int index6;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local5 = @result))[index6 = firstRow, 0] = (^local5)[index6, 0] - result[secondRow, 0] * result[firstRow, index1];
            // ISSUE: variable of a reference type
            Matrix& local6;
            int index7;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local6 = @result))[index7 = firstRow, 1] = (^local6)[index7, 1] - result[secondRow, 1] * result[firstRow, index1];
            // ISSUE: variable of a reference type
            Matrix& local7;
            int index8;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local7 = @result))[index8 = firstRow, 2] = (^local7)[index8, 2] - result[secondRow, 2] * result[firstRow, index1];
            // ISSUE: variable of a reference type
            Matrix& local8;
            int index9;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            (^(local8 = @result))[index9 = firstRow, 3] = (^local8)[index9, 3] - result[secondRow, 3] * result[firstRow, index1];
          }
        }
        ++index1;
      }
    }

    public static Matrix RowEchelonForm(Matrix value)
    {
      Matrix result;
      Matrix.RowEchelonForm(ref value, out result);
      return result;
    }

    public static void ReducedRowEchelonForm(ref Matrix value, ref Vector4 augment, out Matrix result, out Vector4 augmentResult)
    {
      float[,] numArray = new float[4, 5]
      {
        {
          value[0, 0],
          value[0, 1],
          value[0, 2],
          value[0, 3],
          augment[0]
        },
        {
          value[1, 0],
          value[1, 1],
          value[1, 2],
          value[1, 3],
          augment[1]
        },
        {
          value[2, 0],
          value[2, 1],
          value[2, 2],
          value[2, 3],
          augment[2]
        },
        {
          value[3, 0],
          value[3, 1],
          value[3, 2],
          value[3, 3],
          augment[3]
        }
      };
      int index1 = 0;
      int num1 = 4;
      int num2 = 5;
      for (int index2 = 0; index2 < num1 && num2 > index1; ++index2)
      {
        int index3 = index2;
        while ((double) numArray[index3, index1] == 0.0)
        {
          ++index3;
          if (index3 == num1)
          {
            index3 = index2;
            ++index1;
            if (num2 == index1)
              break;
          }
        }
        for (int index4 = 0; index4 < num2; ++index4)
        {
          float num3 = numArray[index2, index4];
          numArray[index2, index4] = numArray[index3, index4];
          numArray[index3, index4] = num3;
        }
        float num4 = numArray[index2, index1];
        for (int index4 = 0; index4 < num2; ++index4)
          numArray[index2, index4] /= num4;
        for (int index4 = 0; index4 < num1; ++index4)
        {
          if (index4 != index2)
          {
            float num3 = numArray[index4, index1];
            for (int index5 = 0; index5 < num2; ++index5)
              numArray[index4, index5] -= num3 * numArray[index2, index5];
          }
        }
        ++index1;
      }
      result.M11 = numArray[0, 0];
      result.M12 = numArray[0, 1];
      result.M13 = numArray[0, 2];
      result.M14 = numArray[0, 3];
      result.M21 = numArray[1, 0];
      result.M22 = numArray[1, 1];
      result.M23 = numArray[1, 2];
      result.M24 = numArray[1, 3];
      result.M31 = numArray[2, 0];
      result.M32 = numArray[2, 1];
      result.M33 = numArray[2, 2];
      result.M34 = numArray[2, 3];
      result.M41 = numArray[3, 0];
      result.M42 = numArray[3, 1];
      result.M43 = numArray[3, 2];
      result.M44 = numArray[3, 3];
      augmentResult.X = numArray[0, 4];
      augmentResult.Y = numArray[1, 4];
      augmentResult.Z = numArray[2, 4];
      augmentResult.W = numArray[3, 4];
    }

    public static void Billboard(ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 cameraUpVector, ref Vector3 cameraForwardVector, out Matrix result)
    {
      Vector3 vector3_1 = objectPosition - cameraPosition;
      float num = vector3_1.LengthSquared();
      Vector3 vector3_2 = (double) num >= 9.99999997475243E-07 ? vector3_1 * (float) (1.0 / Math.Sqrt((double) num)) : -cameraForwardVector;
      Vector3 result1;
      Vector3.Cross(ref cameraUpVector, ref vector3_2, out result1);
      result1.Normalize();
      Vector3 result2;
      Vector3.Cross(ref vector3_2, ref result1, out result2);
      result.M11 = result1.X;
      result.M12 = result1.Y;
      result.M13 = result1.Z;
      result.M14 = 0.0f;
      result.M21 = result2.X;
      result.M22 = result2.Y;
      result.M23 = result2.Z;
      result.M24 = 0.0f;
      result.M31 = vector3_2.X;
      result.M32 = vector3_2.Y;
      result.M33 = vector3_2.Z;
      result.M34 = 0.0f;
      result.M41 = objectPosition.X;
      result.M42 = objectPosition.Y;
      result.M43 = objectPosition.Z;
      result.M44 = 1f;
    }

    public static Matrix Billboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3 cameraForwardVector)
    {
      Matrix result;
      Matrix.Billboard(ref objectPosition, ref cameraPosition, ref cameraUpVector, ref cameraForwardVector, out result);
      return result;
    }

    public static void LookAtLH(ref Vector3 eye, ref Vector3 target, ref Vector3 up, out Matrix result)
    {
      Vector3 result1;
      Vector3.Subtract(ref target, ref eye, out result1);
      result1.Normalize();
      Vector3 result2;
      Vector3.Cross(ref up, ref result1, out result2);
      result2.Normalize();
      Vector3 result3;
      Vector3.Cross(ref result1, ref result2, out result3);
      result = Matrix.Identity;
      result.M11 = result2.X;
      result.M21 = result2.Y;
      result.M31 = result2.Z;
      result.M12 = result3.X;
      result.M22 = result3.Y;
      result.M32 = result3.Z;
      result.M13 = result1.X;
      result.M23 = result1.Y;
      result.M33 = result1.Z;
      Vector3.Dot(ref result2, ref eye, out result.M41);
      Vector3.Dot(ref result3, ref eye, out result.M42);
      Vector3.Dot(ref result1, ref eye, out result.M43);
      result.M41 = -result.M41;
      result.M42 = -result.M42;
      result.M43 = -result.M43;
    }

    public static Matrix LookAtLH(Vector3 eye, Vector3 target, Vector3 up)
    {
      Matrix result;
      Matrix.LookAtLH(ref eye, ref target, ref up, out result);
      return result;
    }

    public static void LookAtRH(ref Vector3 eye, ref Vector3 target, ref Vector3 up, out Matrix result)
    {
      Vector3 result1;
      Vector3.Subtract(ref eye, ref target, out result1);
      result1.Normalize();
      Vector3 result2;
      Vector3.Cross(ref up, ref result1, out result2);
      result2.Normalize();
      Vector3 result3;
      Vector3.Cross(ref result1, ref result2, out result3);
      result = Matrix.Identity;
      result.M11 = result2.X;
      result.M21 = result2.Y;
      result.M31 = result2.Z;
      result.M12 = result3.X;
      result.M22 = result3.Y;
      result.M32 = result3.Z;
      result.M13 = result1.X;
      result.M23 = result1.Y;
      result.M33 = result1.Z;
      Vector3.Dot(ref result2, ref eye, out result.M41);
      Vector3.Dot(ref result3, ref eye, out result.M42);
      Vector3.Dot(ref result1, ref eye, out result.M43);
      result.M41 = -result.M41;
      result.M42 = -result.M42;
      result.M43 = -result.M43;
    }

    public static Matrix LookAtRH(Vector3 eye, Vector3 target, Vector3 up)
    {
      Matrix result;
      Matrix.LookAtRH(ref eye, ref target, ref up, out result);
      return result;
    }

    public static void OrthoLH(float width, float height, float znear, float zfar, out Matrix result)
    {
      float right = width * 0.5f;
      float top = height * 0.5f;
      Matrix.OrthoOffCenterLH(-right, right, -top, top, znear, zfar, out result);
    }

    public static Matrix OrthoLH(float width, float height, float znear, float zfar)
    {
      Matrix result;
      Matrix.OrthoLH(width, height, znear, zfar, out result);
      return result;
    }

    public static void OrthoRH(float width, float height, float znear, float zfar, out Matrix result)
    {
      float right = width * 0.5f;
      float top = height * 0.5f;
      Matrix.OrthoOffCenterRH(-right, right, -top, top, znear, zfar, out result);
    }

    public static Matrix OrthoRH(float width, float height, float znear, float zfar)
    {
      Matrix result;
      Matrix.OrthoRH(width, height, znear, zfar, out result);
      return result;
    }

    public static void OrthoOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
    {
      float num = (float) (1.0 / ((double) zfar - (double) znear));
      result = Matrix.Identity;
      result.M11 = (float) (2.0 / ((double) right - (double) left));
      result.M22 = (float) (2.0 / ((double) top - (double) bottom));
      result.M33 = num;
      result.M41 = (float) (((double) left + (double) right) / ((double) left - (double) right));
      result.M42 = (float) (((double) top + (double) bottom) / ((double) bottom - (double) top));
      result.M43 = -znear * num;
    }

    public static Matrix OrthoOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar)
    {
      Matrix result;
      Matrix.OrthoOffCenterLH(left, right, bottom, top, znear, zfar, out result);
      return result;
    }

    public static void OrthoOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
    {
      Matrix.OrthoOffCenterLH(left, right, bottom, top, znear, zfar, out result);
      result.M33 *= -1f;
    }

    public static Matrix OrthoOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar)
    {
      Matrix result;
      Matrix.OrthoOffCenterRH(left, right, bottom, top, znear, zfar, out result);
      return result;
    }

    public static void PerspectiveLH(float width, float height, float znear, float zfar, out Matrix result)
    {
      float right = width * 0.5f;
      float top = height * 0.5f;
      Matrix.PerspectiveOffCenterLH(-right, right, -top, top, znear, zfar, out result);
    }

    public static Matrix PerspectiveLH(float width, float height, float znear, float zfar)
    {
      Matrix result;
      Matrix.PerspectiveLH(width, height, znear, zfar, out result);
      return result;
    }

    public static void PerspectiveRH(float width, float height, float znear, float zfar, out Matrix result)
    {
      float right = width * 0.5f;
      float top = height * 0.5f;
      Matrix.PerspectiveOffCenterRH(-right, right, -top, top, znear, zfar, out result);
    }

    public static Matrix PerspectiveRH(float width, float height, float znear, float zfar)
    {
      Matrix result;
      Matrix.PerspectiveRH(width, height, znear, zfar, out result);
      return result;
    }

    public static void PerspectiveFovLH(float fov, float aspect, float znear, float zfar, out Matrix result)
    {
      float num1 = (float) (1.0 / Math.Tan((double) fov * 0.5));
      float num2 = num1 / aspect;
      float right = znear / num2;
      float top = znear / num1;
      Matrix.PerspectiveOffCenterLH(-right, right, -top, top, znear, zfar, out result);
    }

    public static Matrix PerspectiveFovLH(float fov, float aspect, float znear, float zfar)
    {
      Matrix result;
      Matrix.PerspectiveFovLH(fov, aspect, znear, zfar, out result);
      return result;
    }

    public static void PerspectiveFovRH(float fov, float aspect, float znear, float zfar, out Matrix result)
    {
      float num1 = (float) (1.0 / Math.Tan((double) fov * 0.5));
      float num2 = num1 / aspect;
      float right = znear / num2;
      float top = znear / num1;
      Matrix.PerspectiveOffCenterRH(-right, right, -top, top, znear, zfar, out result);
    }

    public static Matrix PerspectiveFovRH(float fov, float aspect, float znear, float zfar)
    {
      Matrix result;
      Matrix.PerspectiveFovRH(fov, aspect, znear, zfar, out result);
      return result;
    }

    public static void PerspectiveOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
    {
      float num = zfar / (zfar - znear);
      result = new Matrix();
      result.M11 = (float) (2.0 * (double) znear / ((double) right - (double) left));
      result.M22 = (float) (2.0 * (double) znear / ((double) top - (double) bottom));
      result.M31 = (float) (((double) left + (double) right) / ((double) left - (double) right));
      result.M32 = (float) (((double) top + (double) bottom) / ((double) bottom - (double) top));
      result.M33 = num;
      result.M34 = 1f;
      result.M43 = -znear * num;
    }

    public static Matrix PerspectiveOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar)
    {
      Matrix result;
      Matrix.PerspectiveOffCenterLH(left, right, bottom, top, znear, zfar, out result);
      return result;
    }

    public static void PerspectiveOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
    {
      Matrix.PerspectiveOffCenterLH(left, right, bottom, top, znear, zfar, out result);
      result.M31 *= -1f;
      result.M32 *= -1f;
      result.M33 *= -1f;
      result.M34 *= -1f;
    }

    public static Matrix PerspectiveOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar)
    {
      Matrix result;
      Matrix.PerspectiveOffCenterRH(left, right, bottom, top, znear, zfar, out result);
      return result;
    }

    public static void Reflection(ref Plane plane, out Matrix result)
    {
      float num1 = plane.Normal.X;
      float num2 = plane.Normal.Y;
      float num3 = plane.Normal.Z;
      float num4 = -2f * num1;
      float num5 = -2f * num2;
      float num6 = -2f * num3;
      result.M11 = (float) ((double) num4 * (double) num1 + 1.0);
      result.M12 = num5 * num1;
      result.M13 = num6 * num1;
      result.M14 = 0.0f;
      result.M21 = num4 * num2;
      result.M22 = (float) ((double) num5 * (double) num2 + 1.0);
      result.M23 = num6 * num2;
      result.M24 = 0.0f;
      result.M31 = num4 * num3;
      result.M32 = num5 * num3;
      result.M33 = (float) ((double) num6 * (double) num3 + 1.0);
      result.M34 = 0.0f;
      result.M41 = num4 * plane.D;
      result.M42 = num5 * plane.D;
      result.M43 = num6 * plane.D;
      result.M44 = 1f;
    }

    public static Matrix Reflection(Plane plane)
    {
      Matrix result;
      Matrix.Reflection(ref plane, out result);
      return result;
    }

    public static void Shadow(ref Vector4 light, ref Plane plane, out Matrix result)
    {
      float num1 = (float) ((double) plane.Normal.X * (double) light.X + (double) plane.Normal.Y * (double) light.Y + (double) plane.Normal.Z * (double) light.Z + (double) plane.D * (double) light.W);
      float num2 = -plane.Normal.X;
      float num3 = -plane.Normal.Y;
      float num4 = -plane.Normal.Z;
      float num5 = -plane.D;
      result.M11 = num2 * light.X + num1;
      result.M21 = num3 * light.X;
      result.M31 = num4 * light.X;
      result.M41 = num5 * light.X;
      result.M12 = num2 * light.Y;
      result.M22 = num3 * light.Y + num1;
      result.M32 = num4 * light.Y;
      result.M42 = num5 * light.Y;
      result.M13 = num2 * light.Z;
      result.M23 = num3 * light.Z;
      result.M33 = num4 * light.Z + num1;
      result.M43 = num5 * light.Z;
      result.M14 = num2 * light.W;
      result.M24 = num3 * light.W;
      result.M34 = num4 * light.W;
      result.M44 = num5 * light.W + num1;
    }

    public static Matrix Shadow(Vector4 light, Plane plane)
    {
      Matrix result;
      Matrix.Shadow(ref light, ref plane, out result);
      return result;
    }

    public static void Scaling(ref Vector3 scale, out Matrix result)
    {
      Matrix.Scaling(scale.X, scale.Y, scale.Z, out result);
    }

    public static Matrix Scaling(Vector3 scale)
    {
      Matrix result;
      Matrix.Scaling(ref scale, out result);
      return result;
    }

    public static void Scaling(float x, float y, float z, out Matrix result)
    {
      result = Matrix.Identity;
      result.M11 = x;
      result.M22 = y;
      result.M33 = z;
    }

    public static Matrix Scaling(float x, float y, float z)
    {
      Matrix result;
      Matrix.Scaling(x, y, z, out result);
      return result;
    }

    public static void Scaling(float scale, out Matrix result)
    {
      result = Matrix.Identity;
      result.M11 = result.M22 = result.M33 = scale;
    }

    public static Matrix Scaling(float scale)
    {
      Matrix result;
      Matrix.Scaling(scale, out result);
      return result;
    }

    public static void RotationX(float angle, out Matrix result)
    {
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      result = Matrix.Identity;
      result.M22 = num1;
      result.M23 = num2;
      result.M32 = -num2;
      result.M33 = num1;
    }

    public static Matrix RotationX(float angle)
    {
      Matrix result;
      Matrix.RotationX(angle, out result);
      return result;
    }

    public static void RotationY(float angle, out Matrix result)
    {
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      result = Matrix.Identity;
      result.M11 = num1;
      result.M13 = -num2;
      result.M31 = num2;
      result.M33 = num1;
    }

    public static Matrix RotationY(float angle)
    {
      Matrix result;
      Matrix.RotationY(angle, out result);
      return result;
    }

    public static void RotationZ(float angle, out Matrix result)
    {
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      result = Matrix.Identity;
      result.M11 = num1;
      result.M12 = num2;
      result.M21 = -num2;
      result.M22 = num1;
    }

    public static Matrix RotationZ(float angle)
    {
      Matrix result;
      Matrix.RotationZ(angle, out result);
      return result;
    }

    public static void RotationAxis(ref Vector3 axis, float angle, out Matrix result)
    {
      float num1 = axis.X;
      float num2 = axis.Y;
      float num3 = axis.Z;
      float num4 = (float) Math.Cos((double) angle);
      float num5 = (float) Math.Sin((double) angle);
      float num6 = num1 * num1;
      float num7 = num2 * num2;
      float num8 = num3 * num3;
      float num9 = num1 * num2;
      float num10 = num1 * num3;
      float num11 = num2 * num3;
      result = Matrix.Identity;
      result.M11 = num6 + num4 * (1f - num6);
      result.M12 = (float) ((double) num9 - (double) num4 * (double) num9 + (double) num5 * (double) num3);
      result.M13 = (float) ((double) num10 - (double) num4 * (double) num10 - (double) num5 * (double) num2);
      result.M21 = (float) ((double) num9 - (double) num4 * (double) num9 - (double) num5 * (double) num3);
      result.M22 = num7 + num4 * (1f - num7);
      result.M23 = (float) ((double) num11 - (double) num4 * (double) num11 + (double) num5 * (double) num1);
      result.M31 = (float) ((double) num10 - (double) num4 * (double) num10 + (double) num5 * (double) num2);
      result.M32 = (float) ((double) num11 - (double) num4 * (double) num11 - (double) num5 * (double) num1);
      result.M33 = num8 + num4 * (1f - num8);
    }

    public static Matrix RotationAxis(Vector3 axis, float angle)
    {
      Matrix result;
      Matrix.RotationAxis(ref axis, angle, out result);
      return result;
    }

    public static void RotationQuaternion(ref Quaternion rotation, out Matrix result)
    {
      float num1 = rotation.X * rotation.X;
      float num2 = rotation.Y * rotation.Y;
      float num3 = rotation.Z * rotation.Z;
      float num4 = rotation.X * rotation.Y;
      float num5 = rotation.Z * rotation.W;
      float num6 = rotation.Z * rotation.X;
      float num7 = rotation.Y * rotation.W;
      float num8 = rotation.Y * rotation.Z;
      float num9 = rotation.X * rotation.W;
      result = Matrix.Identity;
      result.M11 = (float) (1.0 - 2.0 * ((double) num2 + (double) num3));
      result.M12 = (float) (2.0 * ((double) num4 + (double) num5));
      result.M13 = (float) (2.0 * ((double) num6 - (double) num7));
      result.M21 = (float) (2.0 * ((double) num4 - (double) num5));
      result.M22 = (float) (1.0 - 2.0 * ((double) num3 + (double) num1));
      result.M23 = (float) (2.0 * ((double) num8 + (double) num9));
      result.M31 = (float) (2.0 * ((double) num6 + (double) num7));
      result.M32 = (float) (2.0 * ((double) num8 - (double) num9));
      result.M33 = (float) (1.0 - 2.0 * ((double) num2 + (double) num1));
    }

    public static Matrix RotationQuaternion(Quaternion rotation)
    {
      Matrix result;
      Matrix.RotationQuaternion(ref rotation, out result);
      return result;
    }

    public static void RotationYawPitchRoll(float yaw, float pitch, float roll, out Matrix result)
    {
      Quaternion result1 = new Quaternion();
      Quaternion.RotationYawPitchRoll(yaw, pitch, roll, out result1);
      Matrix.RotationQuaternion(ref result1, out result);
    }

    public static Matrix RotationYawPitchRoll(float yaw, float pitch, float roll)
    {
      Matrix result;
      Matrix.RotationYawPitchRoll(yaw, pitch, roll, out result);
      return result;
    }

    public static void Translation(ref Vector3 value, out Matrix result)
    {
      Matrix.Translation(value.X, value.Y, value.Z, out result);
    }

    public static Matrix Translation(Vector3 value)
    {
      Matrix result;
      Matrix.Translation(ref value, out result);
      return result;
    }

    public static void Translation(float x, float y, float z, out Matrix result)
    {
      result = Matrix.Identity;
      result.M41 = x;
      result.M42 = y;
      result.M43 = z;
    }

    public static Matrix Translation(float x, float y, float z)
    {
      Matrix result;
      Matrix.Translation(x, y, z, out result);
      return result;
    }

    public static void AffineTransformation(float scaling, ref Quaternion rotation, ref Vector3 translation, out Matrix result)
    {
      result = Matrix.Scaling(scaling) * Matrix.RotationQuaternion(rotation) * Matrix.Translation(translation);
    }

    public static Matrix AffineTransformation(float scaling, Quaternion rotation, Vector3 translation)
    {
      Matrix result;
      Matrix.AffineTransformation(scaling, ref rotation, ref translation, out result);
      return result;
    }

    public static void AffineTransformation(float scaling, ref Vector3 rotationCenter, ref Quaternion rotation, ref Vector3 translation, out Matrix result)
    {
      result = Matrix.Scaling(scaling) * Matrix.Translation(-rotationCenter) * Matrix.RotationQuaternion(rotation) * Matrix.Translation(rotationCenter) * Matrix.Translation(translation);
    }

    public static Matrix AffineTransformation(float scaling, Vector3 rotationCenter, Quaternion rotation, Vector3 translation)
    {
      Matrix result;
      Matrix.AffineTransformation(scaling, ref rotationCenter, ref rotation, ref translation, out result);
      return result;
    }

    public static void AffineTransformation2D(float scaling, float rotation, ref Vector2 translation, out Matrix result)
    {
      result = Matrix.Scaling(scaling, scaling, 1f) * Matrix.RotationZ(rotation) * Matrix.Translation((Vector3) translation);
    }

    public static Matrix AffineTransformation2D(float scaling, float rotation, Vector2 translation)
    {
      Matrix result;
      Matrix.AffineTransformation2D(scaling, rotation, ref translation, out result);
      return result;
    }

    public static void AffineTransformation2D(float scaling, ref Vector2 rotationCenter, float rotation, ref Vector2 translation, out Matrix result)
    {
      result = Matrix.Scaling(scaling, scaling, 1f) * Matrix.Translation((Vector3) (-rotationCenter)) * Matrix.RotationZ(rotation) * Matrix.Translation((Vector3) rotationCenter) * Matrix.Translation((Vector3) translation);
    }

    public static Matrix AffineTransformation2D(float scaling, Vector2 rotationCenter, float rotation, Vector2 translation)
    {
      Matrix result;
      Matrix.AffineTransformation2D(scaling, ref rotationCenter, rotation, ref translation, out result);
      return result;
    }

    public static void Transformation(ref Vector3 scalingCenter, ref Quaternion scalingRotation, ref Vector3 scaling, ref Vector3 rotationCenter, ref Quaternion rotation, ref Vector3 translation, out Matrix result)
    {
      Matrix matrix = Matrix.RotationQuaternion(scalingRotation);
      result = Matrix.Translation(-scalingCenter) * Matrix.Transpose(matrix) * Matrix.Scaling(scaling) * matrix * Matrix.Translation(scalingCenter) * Matrix.Translation(-rotationCenter) * Matrix.RotationQuaternion(rotation) * Matrix.Translation(rotationCenter) * Matrix.Translation(translation);
    }

    public static Matrix Transformation(Vector3 scalingCenter, Quaternion scalingRotation, Vector3 scaling, Vector3 rotationCenter, Quaternion rotation, Vector3 translation)
    {
      Matrix result;
      Matrix.Transformation(ref scalingCenter, ref scalingRotation, ref scaling, ref rotationCenter, ref rotation, ref translation, out result);
      return result;
    }

    public static void Transformation2D(ref Vector2 scalingCenter, float scalingRotation, ref Vector2 scaling, ref Vector2 rotationCenter, float rotation, ref Vector2 translation, out Matrix result)
    {
      result = Matrix.Translation((Vector3) (-scalingCenter)) * Matrix.RotationZ(-scalingRotation) * Matrix.Scaling((Vector3) scaling) * Matrix.RotationZ(scalingRotation) * Matrix.Translation((Vector3) scalingCenter) * Matrix.Translation((Vector3) (-rotationCenter)) * Matrix.RotationZ(rotation) * Matrix.Translation((Vector3) rotationCenter) * Matrix.Translation((Vector3) translation);
      result.M33 = 1f;
      result.M44 = 1f;
    }

    public static Matrix Transformation2D(Vector2 scalingCenter, float scalingRotation, Vector2 scaling, Vector2 rotationCenter, float rotation, Vector2 translation)
    {
      Matrix result;
      Matrix.Transformation2D(ref scalingCenter, scalingRotation, ref scaling, ref rotationCenter, rotation, ref translation, out result);
      return result;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M23:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}]", (object) this.M11, (object) this.M12, (object) this.M13, (object) this.M14, (object) this.M21, (object) this.M22, (object) this.M23, (object) this.M24, (object) this.M31, (object) this.M32, (object) this.M33, (object) this.M34, (object) this.M41, (object) this.M42, (object) this.M43, (object) this.M44);
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format(format, (object) CultureInfo.CurrentCulture, (object) "[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M23:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}]", (object) this.M11.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M12.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M13.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M14.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M21.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M22.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M23.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M24.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M31.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M32.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M33.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M34.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M41.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M42.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M43.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.M44.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture));
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M23:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}]", (object) this.M11.ToString(formatProvider), (object) this.M12.ToString(formatProvider), (object) this.M13.ToString(formatProvider), (object) this.M14.ToString(formatProvider), (object) this.M21.ToString(formatProvider), (object) this.M22.ToString(formatProvider), (object) this.M23.ToString(formatProvider), (object) this.M24.ToString(formatProvider), (object) this.M31.ToString(formatProvider), (object) this.M32.ToString(formatProvider), (object) this.M33.ToString(formatProvider), (object) this.M34.ToString(formatProvider), (object) this.M41.ToString(formatProvider), (object) this.M42.ToString(formatProvider), (object) this.M43.ToString(formatProvider), (object) this.M44.ToString(formatProvider));
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        return this.ToString(formatProvider);
      return string.Format(format, (object) formatProvider, (object) "[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M23:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}]", (object) this.M11.ToString(format, formatProvider), (object) this.M12.ToString(format, formatProvider), (object) this.M13.ToString(format, formatProvider), (object) this.M14.ToString(format, formatProvider), (object) this.M21.ToString(format, formatProvider), (object) this.M22.ToString(format, formatProvider), (object) this.M23.ToString(format, formatProvider), (object) this.M24.ToString(format, formatProvider), (object) this.M31.ToString(format, formatProvider), (object) this.M32.ToString(format, formatProvider), (object) this.M33.ToString(format, formatProvider), (object) this.M34.ToString(format, formatProvider), (object) this.M41.ToString(format, formatProvider), (object) this.M42.ToString(format, formatProvider), (object) this.M43.ToString(format, formatProvider), (object) this.M44.ToString(format, formatProvider));
    }

    public override int GetHashCode()
    {
      return this.M11.GetHashCode() + this.M12.GetHashCode() + this.M13.GetHashCode() + this.M14.GetHashCode() + this.M21.GetHashCode() + this.M22.GetHashCode() + this.M23.GetHashCode() + this.M24.GetHashCode() + this.M31.GetHashCode() + this.M32.GetHashCode() + this.M33.GetHashCode() + this.M34.GetHashCode() + this.M41.GetHashCode() + this.M42.GetHashCode() + this.M43.GetHashCode() + this.M44.GetHashCode();
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.M11);
        serializer.Writer.Write(this.M12);
        serializer.Writer.Write(this.M13);
        serializer.Writer.Write(this.M14);
        serializer.Writer.Write(this.M21);
        serializer.Writer.Write(this.M22);
        serializer.Writer.Write(this.M23);
        serializer.Writer.Write(this.M24);
        serializer.Writer.Write(this.M31);
        serializer.Writer.Write(this.M32);
        serializer.Writer.Write(this.M33);
        serializer.Writer.Write(this.M34);
        serializer.Writer.Write(this.M41);
        serializer.Writer.Write(this.M42);
        serializer.Writer.Write(this.M43);
        serializer.Writer.Write(this.M44);
      }
      else
      {
        this.M11 = serializer.Reader.ReadSingle();
        this.M12 = serializer.Reader.ReadSingle();
        this.M13 = serializer.Reader.ReadSingle();
        this.M14 = serializer.Reader.ReadSingle();
        this.M21 = serializer.Reader.ReadSingle();
        this.M22 = serializer.Reader.ReadSingle();
        this.M23 = serializer.Reader.ReadSingle();
        this.M24 = serializer.Reader.ReadSingle();
        this.M31 = serializer.Reader.ReadSingle();
        this.M32 = serializer.Reader.ReadSingle();
        this.M33 = serializer.Reader.ReadSingle();
        this.M34 = serializer.Reader.ReadSingle();
        this.M41 = serializer.Reader.ReadSingle();
        this.M42 = serializer.Reader.ReadSingle();
        this.M43 = serializer.Reader.ReadSingle();
        this.M44 = serializer.Reader.ReadSingle();
      }
    }

    public bool Equals(Matrix other)
    {
      if ((double) Math.Abs(other.M11 - this.M11) < 9.99999997475243E-07 && (double) Math.Abs(other.M12 - this.M12) < 9.99999997475243E-07 && ((double) Math.Abs(other.M13 - this.M13) < 9.99999997475243E-07 && (double) Math.Abs(other.M14 - this.M14) < 9.99999997475243E-07) && ((double) Math.Abs(other.M21 - this.M21) < 9.99999997475243E-07 && (double) Math.Abs(other.M22 - this.M22) < 9.99999997475243E-07 && ((double) Math.Abs(other.M23 - this.M23) < 9.99999997475243E-07 && (double) Math.Abs(other.M24 - this.M24) < 9.99999997475243E-07)) && ((double) Math.Abs(other.M31 - this.M31) < 9.99999997475243E-07 && (double) Math.Abs(other.M32 - this.M32) < 9.99999997475243E-07 && ((double) Math.Abs(other.M33 - this.M33) < 9.99999997475243E-07 && (double) Math.Abs(other.M34 - this.M34) < 9.99999997475243E-07) && ((double) Math.Abs(other.M41 - this.M41) < 9.99999997475243E-07 && (double) Math.Abs(other.M42 - this.M42) < 9.99999997475243E-07 && (double) Math.Abs(other.M43 - this.M43) < 9.99999997475243E-07)))
        return (double) Math.Abs(other.M44 - this.M44) < 9.99999997475243E-07;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Matrix)))
        return false;
      else
        return this.Equals((Matrix) value);
    }
  }
}
