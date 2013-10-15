// Type: Microsoft.Xna.Framework.Matrix
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework
{
  [Serializable]
  public struct Matrix : IEquatable<Matrix>
  {
    private static Matrix identity = new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
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

    public Vector3 Backward
    {
      get
      {
        return new Vector3(this.M31, this.M32, this.M33);
      }
      set
      {
        this.M31 = value.X;
        this.M32 = value.Y;
        this.M33 = value.Z;
      }
    }

    public Vector3 Down
    {
      get
      {
        return new Vector3(-this.M21, -this.M22, -this.M23);
      }
      set
      {
        this.M21 = -value.X;
        this.M22 = -value.Y;
        this.M23 = -value.Z;
      }
    }

    public Vector3 Forward
    {
      get
      {
        return new Vector3(-this.M31, -this.M32, -this.M33);
      }
      set
      {
        this.M31 = -value.X;
        this.M32 = -value.Y;
        this.M33 = -value.Z;
      }
    }

    public static Matrix Identity
    {
      get
      {
        return Matrix.identity;
      }
    }

    public Vector3 Left
    {
      get
      {
        return new Vector3(-this.M11, -this.M12, -this.M13);
      }
      set
      {
        this.M11 = -value.X;
        this.M12 = -value.Y;
        this.M13 = -value.Z;
      }
    }

    public Vector3 Right
    {
      get
      {
        return new Vector3(this.M11, this.M12, this.M13);
      }
      set
      {
        this.M11 = value.X;
        this.M12 = value.Y;
        this.M13 = value.Z;
      }
    }

    public Vector3 Translation
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

    public Vector3 Up
    {
      get
      {
        return new Vector3(this.M21, this.M22, this.M23);
      }
      set
      {
        this.M21 = value.X;
        this.M22 = value.Y;
        this.M23 = value.Z;
      }
    }

    static Matrix()
    {
    }

    public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
    {
      this.M11 = m11;
      this.M12 = m12;
      this.M13 = m13;
      this.M14 = m14;
      this.M21 = m21;
      this.M22 = m22;
      this.M23 = m23;
      this.M24 = m24;
      this.M31 = m31;
      this.M32 = m32;
      this.M33 = m33;
      this.M34 = m34;
      this.M41 = m41;
      this.M42 = m42;
      this.M43 = m43;
      this.M44 = m44;
    }

    public static Matrix operator +(Matrix matrix1, Matrix matrix2)
    {
      Matrix.Add(ref matrix1, ref matrix2, out matrix1);
      return matrix1;
    }

    public static Matrix operator /(Matrix matrix1, Matrix matrix2)
    {
      matrix1.M11 = matrix1.M11 / matrix2.M11;
      matrix1.M12 = matrix1.M12 / matrix2.M12;
      matrix1.M13 = matrix1.M13 / matrix2.M13;
      matrix1.M14 = matrix1.M14 / matrix2.M14;
      matrix1.M21 = matrix1.M21 / matrix2.M21;
      matrix1.M22 = matrix1.M22 / matrix2.M22;
      matrix1.M23 = matrix1.M23 / matrix2.M23;
      matrix1.M24 = matrix1.M24 / matrix2.M24;
      matrix1.M31 = matrix1.M31 / matrix2.M31;
      matrix1.M32 = matrix1.M32 / matrix2.M32;
      matrix1.M33 = matrix1.M33 / matrix2.M33;
      matrix1.M34 = matrix1.M34 / matrix2.M34;
      matrix1.M41 = matrix1.M41 / matrix2.M41;
      matrix1.M42 = matrix1.M42 / matrix2.M42;
      matrix1.M43 = matrix1.M43 / matrix2.M43;
      matrix1.M44 = matrix1.M44 / matrix2.M44;
      return matrix1;
    }

    public static Matrix operator /(Matrix matrix, float divider)
    {
      float num = 1f / divider;
      matrix.M11 = matrix.M11 * num;
      matrix.M12 = matrix.M12 * num;
      matrix.M13 = matrix.M13 * num;
      matrix.M14 = matrix.M14 * num;
      matrix.M21 = matrix.M21 * num;
      matrix.M22 = matrix.M22 * num;
      matrix.M23 = matrix.M23 * num;
      matrix.M24 = matrix.M24 * num;
      matrix.M31 = matrix.M31 * num;
      matrix.M32 = matrix.M32 * num;
      matrix.M33 = matrix.M33 * num;
      matrix.M34 = matrix.M34 * num;
      matrix.M41 = matrix.M41 * num;
      matrix.M42 = matrix.M42 * num;
      matrix.M43 = matrix.M43 * num;
      matrix.M44 = matrix.M44 * num;
      return matrix;
    }

    public static bool operator ==(Matrix matrix1, Matrix matrix2)
    {
      if ((double) matrix1.M11 == (double) matrix2.M11 && (double) matrix1.M12 == (double) matrix2.M12 && ((double) matrix1.M13 == (double) matrix2.M13 && (double) matrix1.M14 == (double) matrix2.M14) && ((double) matrix1.M21 == (double) matrix2.M21 && (double) matrix1.M22 == (double) matrix2.M22 && ((double) matrix1.M23 == (double) matrix2.M23 && (double) matrix1.M24 == (double) matrix2.M24)) && ((double) matrix1.M31 == (double) matrix2.M31 && (double) matrix1.M32 == (double) matrix2.M32 && ((double) matrix1.M33 == (double) matrix2.M33 && (double) matrix1.M34 == (double) matrix2.M34) && ((double) matrix1.M41 == (double) matrix2.M41 && (double) matrix1.M42 == (double) matrix2.M42 && (double) matrix1.M43 == (double) matrix2.M43)))
        return (double) matrix1.M44 == (double) matrix2.M44;
      else
        return false;
    }

    public static bool operator !=(Matrix matrix1, Matrix matrix2)
    {
      if ((double) matrix1.M11 == (double) matrix2.M11 && (double) matrix1.M12 == (double) matrix2.M12 && ((double) matrix1.M13 == (double) matrix2.M13 && (double) matrix1.M14 == (double) matrix2.M14) && ((double) matrix1.M21 == (double) matrix2.M21 && (double) matrix1.M22 == (double) matrix2.M22 && ((double) matrix1.M23 == (double) matrix2.M23 && (double) matrix1.M24 == (double) matrix2.M24)) && ((double) matrix1.M31 == (double) matrix2.M31 && (double) matrix1.M32 == (double) matrix2.M32 && ((double) matrix1.M33 == (double) matrix2.M33 && (double) matrix1.M34 == (double) matrix2.M34) && ((double) matrix1.M41 == (double) matrix2.M41 && (double) matrix1.M42 == (double) matrix2.M42 && (double) matrix1.M43 == (double) matrix2.M43)))
        return (double) matrix1.M44 != (double) matrix2.M44;
      else
        return true;
    }

    public static Matrix operator *(Matrix matrix1, Matrix matrix2)
    {
      float num1 = (float) ((double) matrix1.M11 * (double) matrix2.M11 + (double) matrix1.M12 * (double) matrix2.M21 + (double) matrix1.M13 * (double) matrix2.M31 + (double) matrix1.M14 * (double) matrix2.M41);
      float num2 = (float) ((double) matrix1.M11 * (double) matrix2.M12 + (double) matrix1.M12 * (double) matrix2.M22 + (double) matrix1.M13 * (double) matrix2.M32 + (double) matrix1.M14 * (double) matrix2.M42);
      float num3 = (float) ((double) matrix1.M11 * (double) matrix2.M13 + (double) matrix1.M12 * (double) matrix2.M23 + (double) matrix1.M13 * (double) matrix2.M33 + (double) matrix1.M14 * (double) matrix2.M43);
      float num4 = (float) ((double) matrix1.M11 * (double) matrix2.M14 + (double) matrix1.M12 * (double) matrix2.M24 + (double) matrix1.M13 * (double) matrix2.M34 + (double) matrix1.M14 * (double) matrix2.M44);
      float num5 = (float) ((double) matrix1.M21 * (double) matrix2.M11 + (double) matrix1.M22 * (double) matrix2.M21 + (double) matrix1.M23 * (double) matrix2.M31 + (double) matrix1.M24 * (double) matrix2.M41);
      float num6 = (float) ((double) matrix1.M21 * (double) matrix2.M12 + (double) matrix1.M22 * (double) matrix2.M22 + (double) matrix1.M23 * (double) matrix2.M32 + (double) matrix1.M24 * (double) matrix2.M42);
      float num7 = (float) ((double) matrix1.M21 * (double) matrix2.M13 + (double) matrix1.M22 * (double) matrix2.M23 + (double) matrix1.M23 * (double) matrix2.M33 + (double) matrix1.M24 * (double) matrix2.M43);
      float num8 = (float) ((double) matrix1.M21 * (double) matrix2.M14 + (double) matrix1.M22 * (double) matrix2.M24 + (double) matrix1.M23 * (double) matrix2.M34 + (double) matrix1.M24 * (double) matrix2.M44);
      float num9 = (float) ((double) matrix1.M31 * (double) matrix2.M11 + (double) matrix1.M32 * (double) matrix2.M21 + (double) matrix1.M33 * (double) matrix2.M31 + (double) matrix1.M34 * (double) matrix2.M41);
      float num10 = (float) ((double) matrix1.M31 * (double) matrix2.M12 + (double) matrix1.M32 * (double) matrix2.M22 + (double) matrix1.M33 * (double) matrix2.M32 + (double) matrix1.M34 * (double) matrix2.M42);
      float num11 = (float) ((double) matrix1.M31 * (double) matrix2.M13 + (double) matrix1.M32 * (double) matrix2.M23 + (double) matrix1.M33 * (double) matrix2.M33 + (double) matrix1.M34 * (double) matrix2.M43);
      float num12 = (float) ((double) matrix1.M31 * (double) matrix2.M14 + (double) matrix1.M32 * (double) matrix2.M24 + (double) matrix1.M33 * (double) matrix2.M34 + (double) matrix1.M34 * (double) matrix2.M44);
      float num13 = (float) ((double) matrix1.M41 * (double) matrix2.M11 + (double) matrix1.M42 * (double) matrix2.M21 + (double) matrix1.M43 * (double) matrix2.M31 + (double) matrix1.M44 * (double) matrix2.M41);
      float num14 = (float) ((double) matrix1.M41 * (double) matrix2.M12 + (double) matrix1.M42 * (double) matrix2.M22 + (double) matrix1.M43 * (double) matrix2.M32 + (double) matrix1.M44 * (double) matrix2.M42);
      float num15 = (float) ((double) matrix1.M41 * (double) matrix2.M13 + (double) matrix1.M42 * (double) matrix2.M23 + (double) matrix1.M43 * (double) matrix2.M33 + (double) matrix1.M44 * (double) matrix2.M43);
      float num16 = (float) ((double) matrix1.M41 * (double) matrix2.M14 + (double) matrix1.M42 * (double) matrix2.M24 + (double) matrix1.M43 * (double) matrix2.M34 + (double) matrix1.M44 * (double) matrix2.M44);
      matrix1.M11 = num1;
      matrix1.M12 = num2;
      matrix1.M13 = num3;
      matrix1.M14 = num4;
      matrix1.M21 = num5;
      matrix1.M22 = num6;
      matrix1.M23 = num7;
      matrix1.M24 = num8;
      matrix1.M31 = num9;
      matrix1.M32 = num10;
      matrix1.M33 = num11;
      matrix1.M34 = num12;
      matrix1.M41 = num13;
      matrix1.M42 = num14;
      matrix1.M43 = num15;
      matrix1.M44 = num16;
      return matrix1;
    }

    public static Matrix operator *(Matrix matrix, float scaleFactor)
    {
      matrix.M11 = matrix.M11 * scaleFactor;
      matrix.M12 = matrix.M12 * scaleFactor;
      matrix.M13 = matrix.M13 * scaleFactor;
      matrix.M14 = matrix.M14 * scaleFactor;
      matrix.M21 = matrix.M21 * scaleFactor;
      matrix.M22 = matrix.M22 * scaleFactor;
      matrix.M23 = matrix.M23 * scaleFactor;
      matrix.M24 = matrix.M24 * scaleFactor;
      matrix.M31 = matrix.M31 * scaleFactor;
      matrix.M32 = matrix.M32 * scaleFactor;
      matrix.M33 = matrix.M33 * scaleFactor;
      matrix.M34 = matrix.M34 * scaleFactor;
      matrix.M41 = matrix.M41 * scaleFactor;
      matrix.M42 = matrix.M42 * scaleFactor;
      matrix.M43 = matrix.M43 * scaleFactor;
      matrix.M44 = matrix.M44 * scaleFactor;
      return matrix;
    }

    public static Matrix operator -(Matrix matrix1, Matrix matrix2)
    {
      matrix1.M11 = matrix1.M11 - matrix2.M11;
      matrix1.M12 = matrix1.M12 - matrix2.M12;
      matrix1.M13 = matrix1.M13 - matrix2.M13;
      matrix1.M14 = matrix1.M14 - matrix2.M14;
      matrix1.M21 = matrix1.M21 - matrix2.M21;
      matrix1.M22 = matrix1.M22 - matrix2.M22;
      matrix1.M23 = matrix1.M23 - matrix2.M23;
      matrix1.M24 = matrix1.M24 - matrix2.M24;
      matrix1.M31 = matrix1.M31 - matrix2.M31;
      matrix1.M32 = matrix1.M32 - matrix2.M32;
      matrix1.M33 = matrix1.M33 - matrix2.M33;
      matrix1.M34 = matrix1.M34 - matrix2.M34;
      matrix1.M41 = matrix1.M41 - matrix2.M41;
      matrix1.M42 = matrix1.M42 - matrix2.M42;
      matrix1.M43 = matrix1.M43 - matrix2.M43;
      matrix1.M44 = matrix1.M44 - matrix2.M44;
      return matrix1;
    }

    public static Matrix operator -(Matrix matrix)
    {
      matrix.M11 = -matrix.M11;
      matrix.M12 = -matrix.M12;
      matrix.M13 = -matrix.M13;
      matrix.M14 = -matrix.M14;
      matrix.M21 = -matrix.M21;
      matrix.M22 = -matrix.M22;
      matrix.M23 = -matrix.M23;
      matrix.M24 = -matrix.M24;
      matrix.M31 = -matrix.M31;
      matrix.M32 = -matrix.M32;
      matrix.M33 = -matrix.M33;
      matrix.M34 = -matrix.M34;
      matrix.M41 = -matrix.M41;
      matrix.M42 = -matrix.M42;
      matrix.M43 = -matrix.M43;
      matrix.M44 = -matrix.M44;
      return matrix;
    }

    public static float[] ToFloatArray(Matrix mat)
    {
      return new float[16]
      {
        mat.M11,
        mat.M12,
        mat.M13,
        mat.M14,
        mat.M21,
        mat.M22,
        mat.M23,
        mat.M24,
        mat.M31,
        mat.M32,
        mat.M33,
        mat.M34,
        mat.M41,
        mat.M42,
        mat.M43,
        mat.M44
      };
    }

    public static Matrix Add(Matrix matrix1, Matrix matrix2)
    {
      matrix1.M11 += matrix2.M11;
      matrix1.M12 += matrix2.M12;
      matrix1.M13 += matrix2.M13;
      matrix1.M14 += matrix2.M14;
      matrix1.M21 += matrix2.M21;
      matrix1.M22 += matrix2.M22;
      matrix1.M23 += matrix2.M23;
      matrix1.M24 += matrix2.M24;
      matrix1.M31 += matrix2.M31;
      matrix1.M32 += matrix2.M32;
      matrix1.M33 += matrix2.M33;
      matrix1.M34 += matrix2.M34;
      matrix1.M41 += matrix2.M41;
      matrix1.M42 += matrix2.M42;
      matrix1.M43 += matrix2.M43;
      matrix1.M44 += matrix2.M44;
      return matrix1;
    }

    public static void Add(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
      result.M11 = matrix1.M11 + matrix2.M11;
      result.M12 = matrix1.M12 + matrix2.M12;
      result.M13 = matrix1.M13 + matrix2.M13;
      result.M14 = matrix1.M14 + matrix2.M14;
      result.M21 = matrix1.M21 + matrix2.M21;
      result.M22 = matrix1.M22 + matrix2.M22;
      result.M23 = matrix1.M23 + matrix2.M23;
      result.M24 = matrix1.M24 + matrix2.M24;
      result.M31 = matrix1.M31 + matrix2.M31;
      result.M32 = matrix1.M32 + matrix2.M32;
      result.M33 = matrix1.M33 + matrix2.M33;
      result.M34 = matrix1.M34 + matrix2.M34;
      result.M41 = matrix1.M41 + matrix2.M41;
      result.M42 = matrix1.M42 + matrix2.M42;
      result.M43 = matrix1.M43 + matrix2.M43;
      result.M44 = matrix1.M44 + matrix2.M44;
    }

    public static Matrix CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3? cameraForwardVector)
    {
      Vector3 vector1 = cameraPosition - objectPosition;
      Matrix identity = Matrix.Identity;
      vector1.Normalize();
      identity.Forward = vector1;
      identity.Left = Vector3.Cross(vector1, cameraUpVector);
      identity.Up = cameraUpVector;
      identity.Translation = objectPosition;
      return identity;
    }

    public static void CreateBillboard(ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 cameraUpVector, Vector3? cameraForwardVector, out Matrix result)
    {
      Vector3 result1;
      result1.X = objectPosition.X - cameraPosition.X;
      result1.Y = objectPosition.Y - cameraPosition.Y;
      result1.Z = objectPosition.Z - cameraPosition.Z;
      float num = result1.LengthSquared();
      if ((double) num < 9.99999974737875E-05)
        result1 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
      else
        Vector3.Multiply(ref result1, 1f / (float) Math.Sqrt((double) num), out result1);
      Vector3 result2;
      Vector3.Cross(ref cameraUpVector, ref result1, out result2);
      result2.Normalize();
      Vector3 result3;
      Vector3.Cross(ref result1, ref result2, out result3);
      result.M11 = result2.X;
      result.M12 = result2.Y;
      result.M13 = result2.Z;
      result.M14 = 0.0f;
      result.M21 = result3.X;
      result.M22 = result3.Y;
      result.M23 = result3.Z;
      result.M24 = 0.0f;
      result.M31 = result1.X;
      result.M32 = result1.Y;
      result.M33 = result1.Z;
      result.M34 = 0.0f;
      result.M41 = objectPosition.X;
      result.M42 = objectPosition.Y;
      result.M43 = objectPosition.Z;
      result.M44 = 1f;
    }

    public static Matrix CreateConstrainedBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector)
    {
      Vector3 result1;
      result1.X = objectPosition.X - cameraPosition.X;
      result1.Y = objectPosition.Y - cameraPosition.Y;
      result1.Z = objectPosition.Z - cameraPosition.Z;
      float num = result1.LengthSquared();
      if ((double) num < 9.99999974737875E-05)
        result1 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
      else
        Vector3.Multiply(ref result1, 1f / (float) Math.Sqrt((double) num), out result1);
      Vector3 vector2 = rotateAxis;
      float result2;
      Vector3.Dot(ref rotateAxis, ref result1, out result2);
      Vector3 result3;
      Vector3 result4;
      if ((double) Math.Abs(result2) > 0.998254716396332)
      {
        if (objectForwardVector.HasValue)
        {
          result3 = objectForwardVector.Value;
          Vector3.Dot(ref rotateAxis, ref result3, out result2);
          if ((double) Math.Abs(result2) > 0.998254716396332)
            result3 = (double) Math.Abs((float) ((double) rotateAxis.X * (double) Vector3.Forward.X + (double) rotateAxis.Y * (double) Vector3.Forward.Y + (double) rotateAxis.Z * (double) Vector3.Forward.Z)) > 0.998254716396332 ? Vector3.Right : Vector3.Forward;
        }
        else
          result3 = (double) Math.Abs((float) ((double) rotateAxis.X * (double) Vector3.Forward.X + (double) rotateAxis.Y * (double) Vector3.Forward.Y + (double) rotateAxis.Z * (double) Vector3.Forward.Z)) > 0.998254716396332 ? Vector3.Right : Vector3.Forward;
        Vector3.Cross(ref rotateAxis, ref result3, out result4);
        result4.Normalize();
        Vector3.Cross(ref result4, ref rotateAxis, out result3);
        result3.Normalize();
      }
      else
      {
        Vector3.Cross(ref rotateAxis, ref result1, out result4);
        result4.Normalize();
        Vector3.Cross(ref result4, ref vector2, out result3);
        result3.Normalize();
      }
      Matrix matrix;
      matrix.M11 = result4.X;
      matrix.M12 = result4.Y;
      matrix.M13 = result4.Z;
      matrix.M14 = 0.0f;
      matrix.M21 = vector2.X;
      matrix.M22 = vector2.Y;
      matrix.M23 = vector2.Z;
      matrix.M24 = 0.0f;
      matrix.M31 = result3.X;
      matrix.M32 = result3.Y;
      matrix.M33 = result3.Z;
      matrix.M34 = 0.0f;
      matrix.M41 = objectPosition.X;
      matrix.M42 = objectPosition.Y;
      matrix.M43 = objectPosition.Z;
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateConstrainedBillboard(ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector, out Matrix result)
    {
      Vector3 result1;
      result1.X = objectPosition.X - cameraPosition.X;
      result1.Y = objectPosition.Y - cameraPosition.Y;
      result1.Z = objectPosition.Z - cameraPosition.Z;
      float num = result1.LengthSquared();
      if ((double) num < 9.99999974737875E-05)
        result1 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
      else
        Vector3.Multiply(ref result1, 1f / (float) Math.Sqrt((double) num), out result1);
      Vector3 vector2 = rotateAxis;
      float result2;
      Vector3.Dot(ref rotateAxis, ref result1, out result2);
      Vector3 result3;
      Vector3 result4;
      if ((double) Math.Abs(result2) > 0.998254716396332)
      {
        if (objectForwardVector.HasValue)
        {
          result3 = objectForwardVector.Value;
          Vector3.Dot(ref rotateAxis, ref result3, out result2);
          if ((double) Math.Abs(result2) > 0.998254716396332)
            result3 = (double) Math.Abs((float) ((double) rotateAxis.X * (double) Vector3.Forward.X + (double) rotateAxis.Y * (double) Vector3.Forward.Y + (double) rotateAxis.Z * (double) Vector3.Forward.Z)) > 0.998254716396332 ? Vector3.Right : Vector3.Forward;
        }
        else
          result3 = (double) Math.Abs((float) ((double) rotateAxis.X * (double) Vector3.Forward.X + (double) rotateAxis.Y * (double) Vector3.Forward.Y + (double) rotateAxis.Z * (double) Vector3.Forward.Z)) > 0.998254716396332 ? Vector3.Right : Vector3.Forward;
        Vector3.Cross(ref rotateAxis, ref result3, out result4);
        result4.Normalize();
        Vector3.Cross(ref result4, ref rotateAxis, out result3);
        result3.Normalize();
      }
      else
      {
        Vector3.Cross(ref rotateAxis, ref result1, out result4);
        result4.Normalize();
        Vector3.Cross(ref result4, ref vector2, out result3);
        result3.Normalize();
      }
      result.M11 = result4.X;
      result.M12 = result4.Y;
      result.M13 = result4.Z;
      result.M14 = 0.0f;
      result.M21 = vector2.X;
      result.M22 = vector2.Y;
      result.M23 = vector2.Z;
      result.M24 = 0.0f;
      result.M31 = result3.X;
      result.M32 = result3.Y;
      result.M33 = result3.Z;
      result.M34 = 0.0f;
      result.M41 = objectPosition.X;
      result.M42 = objectPosition.Y;
      result.M43 = objectPosition.Z;
      result.M44 = 1f;
    }

    public static Matrix CreateFromAxisAngle(Vector3 axis, float angle)
    {
      float num1 = axis.X;
      float num2 = axis.Y;
      float num3 = axis.Z;
      float num4 = (float) Math.Sin((double) angle);
      float num5 = (float) Math.Cos((double) angle);
      float num6 = num1 * num1;
      float num7 = num2 * num2;
      float num8 = num3 * num3;
      float num9 = num1 * num2;
      float num10 = num1 * num3;
      float num11 = num2 * num3;
      Matrix matrix;
      matrix.M11 = num6 + num5 * (1f - num6);
      matrix.M12 = (float) ((double) num9 - (double) num5 * (double) num9 + (double) num4 * (double) num3);
      matrix.M13 = (float) ((double) num10 - (double) num5 * (double) num10 - (double) num4 * (double) num2);
      matrix.M14 = 0.0f;
      matrix.M21 = (float) ((double) num9 - (double) num5 * (double) num9 - (double) num4 * (double) num3);
      matrix.M22 = num7 + num5 * (1f - num7);
      matrix.M23 = (float) ((double) num11 - (double) num5 * (double) num11 + (double) num4 * (double) num1);
      matrix.M24 = 0.0f;
      matrix.M31 = (float) ((double) num10 - (double) num5 * (double) num10 + (double) num4 * (double) num2);
      matrix.M32 = (float) ((double) num11 - (double) num5 * (double) num11 - (double) num4 * (double) num1);
      matrix.M33 = num8 + num5 * (1f - num8);
      matrix.M34 = 0.0f;
      matrix.M41 = 0.0f;
      matrix.M42 = 0.0f;
      matrix.M43 = 0.0f;
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Matrix result)
    {
      float num1 = axis.X;
      float num2 = axis.Y;
      float num3 = axis.Z;
      float num4 = (float) Math.Sin((double) angle);
      float num5 = (float) Math.Cos((double) angle);
      float num6 = num1 * num1;
      float num7 = num2 * num2;
      float num8 = num3 * num3;
      float num9 = num1 * num2;
      float num10 = num1 * num3;
      float num11 = num2 * num3;
      result.M11 = num6 + num5 * (1f - num6);
      result.M12 = (float) ((double) num9 - (double) num5 * (double) num9 + (double) num4 * (double) num3);
      result.M13 = (float) ((double) num10 - (double) num5 * (double) num10 - (double) num4 * (double) num2);
      result.M14 = 0.0f;
      result.M21 = (float) ((double) num9 - (double) num5 * (double) num9 - (double) num4 * (double) num3);
      result.M22 = num7 + num5 * (1f - num7);
      result.M23 = (float) ((double) num11 - (double) num5 * (double) num11 + (double) num4 * (double) num1);
      result.M24 = 0.0f;
      result.M31 = (float) ((double) num10 - (double) num5 * (double) num10 + (double) num4 * (double) num2);
      result.M32 = (float) ((double) num11 - (double) num5 * (double) num11 - (double) num4 * (double) num1);
      result.M33 = num8 + num5 * (1f - num8);
      result.M34 = 0.0f;
      result.M41 = 0.0f;
      result.M42 = 0.0f;
      result.M43 = 0.0f;
      result.M44 = 1f;
    }

    public static Matrix CreateFromQuaternion(Quaternion quaternion)
    {
      float num1 = quaternion.X * quaternion.X;
      float num2 = quaternion.Y * quaternion.Y;
      float num3 = quaternion.Z * quaternion.Z;
      float num4 = quaternion.X * quaternion.Y;
      float num5 = quaternion.Z * quaternion.W;
      float num6 = quaternion.Z * quaternion.X;
      float num7 = quaternion.Y * quaternion.W;
      float num8 = quaternion.Y * quaternion.Z;
      float num9 = quaternion.X * quaternion.W;
      Matrix matrix;
      matrix.M11 = (float) (1.0 - 2.0 * ((double) num2 + (double) num3));
      matrix.M12 = (float) (2.0 * ((double) num4 + (double) num5));
      matrix.M13 = (float) (2.0 * ((double) num6 - (double) num7));
      matrix.M14 = 0.0f;
      matrix.M21 = (float) (2.0 * ((double) num4 - (double) num5));
      matrix.M22 = (float) (1.0 - 2.0 * ((double) num3 + (double) num1));
      matrix.M23 = (float) (2.0 * ((double) num8 + (double) num9));
      matrix.M24 = 0.0f;
      matrix.M31 = (float) (2.0 * ((double) num6 + (double) num7));
      matrix.M32 = (float) (2.0 * ((double) num8 - (double) num9));
      matrix.M33 = (float) (1.0 - 2.0 * ((double) num2 + (double) num1));
      matrix.M34 = 0.0f;
      matrix.M41 = 0.0f;
      matrix.M42 = 0.0f;
      matrix.M43 = 0.0f;
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix result)
    {
      float num1 = quaternion.X * quaternion.X;
      float num2 = quaternion.Y * quaternion.Y;
      float num3 = quaternion.Z * quaternion.Z;
      float num4 = quaternion.X * quaternion.Y;
      float num5 = quaternion.Z * quaternion.W;
      float num6 = quaternion.Z * quaternion.X;
      float num7 = quaternion.Y * quaternion.W;
      float num8 = quaternion.Y * quaternion.Z;
      float num9 = quaternion.X * quaternion.W;
      result.M11 = (float) (1.0 - 2.0 * ((double) num2 + (double) num3));
      result.M12 = (float) (2.0 * ((double) num4 + (double) num5));
      result.M13 = (float) (2.0 * ((double) num6 - (double) num7));
      result.M14 = 0.0f;
      result.M21 = (float) (2.0 * ((double) num4 - (double) num5));
      result.M22 = (float) (1.0 - 2.0 * ((double) num3 + (double) num1));
      result.M23 = (float) (2.0 * ((double) num8 + (double) num9));
      result.M24 = 0.0f;
      result.M31 = (float) (2.0 * ((double) num6 + (double) num7));
      result.M32 = (float) (2.0 * ((double) num8 - (double) num9));
      result.M33 = (float) (1.0 - 2.0 * ((double) num2 + (double) num1));
      result.M34 = 0.0f;
      result.M41 = 0.0f;
      result.M42 = 0.0f;
      result.M43 = 0.0f;
      result.M44 = 1f;
    }

    public static Matrix CreateFromYawPitchRoll(float yaw, float pitch, float roll)
    {
      Quaternion result1;
      Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out result1);
      Matrix result2;
      Matrix.CreateFromQuaternion(ref result1, out result2);
      return result2;
    }

    public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Matrix result)
    {
      Quaternion result1;
      Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out result1);
      Matrix.CreateFromQuaternion(ref result1, out result);
    }

    public static Matrix CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
    {
      Vector3 vector3_1 = Vector3.Normalize(cameraPosition - cameraTarget);
      Vector3 vector3_2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector3_1));
      Vector3 vector1 = Vector3.Cross(vector3_1, vector3_2);
      Matrix matrix;
      matrix.M11 = vector3_2.X;
      matrix.M12 = vector1.X;
      matrix.M13 = vector3_1.X;
      matrix.M14 = 0.0f;
      matrix.M21 = vector3_2.Y;
      matrix.M22 = vector1.Y;
      matrix.M23 = vector3_1.Y;
      matrix.M24 = 0.0f;
      matrix.M31 = vector3_2.Z;
      matrix.M32 = vector1.Z;
      matrix.M33 = vector3_1.Z;
      matrix.M34 = 0.0f;
      matrix.M41 = -Vector3.Dot(vector3_2, cameraPosition);
      matrix.M42 = -Vector3.Dot(vector1, cameraPosition);
      matrix.M43 = -Vector3.Dot(vector3_1, cameraPosition);
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateLookAt(ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector, out Matrix result)
    {
      Vector3 vector3_1 = Vector3.Normalize(cameraPosition - cameraTarget);
      Vector3 vector3_2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector3_1));
      Vector3 vector1 = Vector3.Cross(vector3_1, vector3_2);
      result.M11 = vector3_2.X;
      result.M12 = vector1.X;
      result.M13 = vector3_1.X;
      result.M14 = 0.0f;
      result.M21 = vector3_2.Y;
      result.M22 = vector1.Y;
      result.M23 = vector3_1.Y;
      result.M24 = 0.0f;
      result.M31 = vector3_2.Z;
      result.M32 = vector1.Z;
      result.M33 = vector3_1.Z;
      result.M34 = 0.0f;
      result.M41 = -Vector3.Dot(vector3_2, cameraPosition);
      result.M42 = -Vector3.Dot(vector1, cameraPosition);
      result.M43 = -Vector3.Dot(vector3_1, cameraPosition);
      result.M44 = 1f;
    }

    public static Matrix CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
    {
      Matrix matrix;
      matrix.M11 = 2f / width;
      matrix.M12 = matrix.M13 = matrix.M14 = 0.0f;
      matrix.M22 = 2f / height;
      matrix.M21 = matrix.M23 = matrix.M24 = 0.0f;
      matrix.M33 = (float) (1.0 / ((double) zNearPlane - (double) zFarPlane));
      matrix.M31 = matrix.M32 = matrix.M34 = 0.0f;
      matrix.M41 = matrix.M42 = 0.0f;
      matrix.M43 = zNearPlane / (zNearPlane - zFarPlane);
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane, out Matrix result)
    {
      result.M11 = 2f / width;
      result.M12 = result.M13 = result.M14 = 0.0f;
      result.M22 = 2f / height;
      result.M21 = result.M23 = result.M24 = 0.0f;
      result.M33 = (float) (1.0 / ((double) zNearPlane - (double) zFarPlane));
      result.M31 = result.M32 = result.M34 = 0.0f;
      result.M41 = result.M42 = 0.0f;
      result.M43 = zNearPlane / (zNearPlane - zFarPlane);
      result.M44 = 1f;
    }

    public static Matrix CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
    {
      Matrix matrix;
      matrix.M11 = (float) (2.0 / ((double) right - (double) left));
      matrix.M12 = 0.0f;
      matrix.M13 = 0.0f;
      matrix.M14 = 0.0f;
      matrix.M21 = 0.0f;
      matrix.M22 = (float) (2.0 / ((double) top - (double) bottom));
      matrix.M23 = 0.0f;
      matrix.M24 = 0.0f;
      matrix.M31 = 0.0f;
      matrix.M32 = 0.0f;
      matrix.M33 = (float) (1.0 / ((double) zNearPlane - (double) zFarPlane));
      matrix.M34 = 0.0f;
      matrix.M41 = (float) (((double) left + (double) right) / ((double) left - (double) right));
      matrix.M42 = (float) (((double) top + (double) bottom) / ((double) bottom - (double) top));
      matrix.M43 = zNearPlane / (zNearPlane - zFarPlane);
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane, out Matrix result)
    {
      result.M11 = (float) (2.0 / ((double) right - (double) left));
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M14 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = (float) (2.0 / ((double) top - (double) bottom));
      result.M23 = 0.0f;
      result.M24 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = (float) (1.0 / ((double) zNearPlane - (double) zFarPlane));
      result.M34 = 0.0f;
      result.M41 = (float) (((double) left + (double) right) / ((double) left - (double) right));
      result.M42 = (float) (((double) top + (double) bottom) / ((double) bottom - (double) top));
      result.M43 = zNearPlane / (zNearPlane - zFarPlane);
      result.M44 = 1f;
    }

    public static Matrix CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
    {
      if ((double) nearPlaneDistance <= 0.0)
        throw new ArgumentException("nearPlaneDistance <= 0");
      if ((double) farPlaneDistance <= 0.0)
        throw new ArgumentException("farPlaneDistance <= 0");
      if ((double) nearPlaneDistance >= (double) farPlaneDistance)
        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
      Matrix matrix;
      matrix.M11 = 2f * nearPlaneDistance / width;
      matrix.M12 = matrix.M13 = matrix.M14 = 0.0f;
      matrix.M22 = 2f * nearPlaneDistance / height;
      matrix.M21 = matrix.M23 = matrix.M24 = 0.0f;
      matrix.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      matrix.M31 = matrix.M32 = 0.0f;
      matrix.M34 = -1f;
      matrix.M41 = matrix.M42 = matrix.M44 = 0.0f;
      matrix.M43 = (float) ((double) nearPlaneDistance * (double) farPlaneDistance / ((double) nearPlaneDistance - (double) farPlaneDistance));
      return matrix;
    }

    public static void CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
    {
      if ((double) nearPlaneDistance <= 0.0)
        throw new ArgumentException("nearPlaneDistance <= 0");
      if ((double) farPlaneDistance <= 0.0)
        throw new ArgumentException("farPlaneDistance <= 0");
      if ((double) nearPlaneDistance >= (double) farPlaneDistance)
        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
      result.M11 = 2f * nearPlaneDistance / width;
      result.M12 = result.M13 = result.M14 = 0.0f;
      result.M22 = 2f * nearPlaneDistance / height;
      result.M21 = result.M23 = result.M24 = 0.0f;
      result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      result.M31 = result.M32 = 0.0f;
      result.M34 = -1f;
      result.M41 = result.M42 = result.M44 = 0.0f;
      result.M43 = (float) ((double) nearPlaneDistance * (double) farPlaneDistance / ((double) nearPlaneDistance - (double) farPlaneDistance));
    }

    public static Matrix CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
    {
      if ((double) fieldOfView <= 0.0 || (double) fieldOfView >= 3.14159297943115)
        throw new ArgumentException("fieldOfView <= 0 O >= PI");
      if ((double) nearPlaneDistance <= 0.0)
        throw new ArgumentException("nearPlaneDistance <= 0");
      if ((double) farPlaneDistance <= 0.0)
        throw new ArgumentException("farPlaneDistance <= 0");
      if ((double) nearPlaneDistance >= (double) farPlaneDistance)
        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
      float num1 = 1f / (float) Math.Tan((double) fieldOfView * 0.5);
      float num2 = num1 / aspectRatio;
      Matrix matrix;
      matrix.M11 = num2;
      matrix.M12 = matrix.M13 = matrix.M14 = 0.0f;
      matrix.M22 = num1;
      matrix.M21 = matrix.M23 = matrix.M24 = 0.0f;
      matrix.M31 = matrix.M32 = 0.0f;
      matrix.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      matrix.M34 = -1f;
      matrix.M41 = matrix.M42 = matrix.M44 = 0.0f;
      matrix.M43 = (float) ((double) nearPlaneDistance * (double) farPlaneDistance / ((double) nearPlaneDistance - (double) farPlaneDistance));
      return matrix;
    }

    public static void CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
    {
      if ((double) fieldOfView <= 0.0 || (double) fieldOfView >= 3.14159297943115)
        throw new ArgumentException("fieldOfView <= 0 or >= PI");
      if ((double) nearPlaneDistance <= 0.0)
        throw new ArgumentException("nearPlaneDistance <= 0");
      if ((double) farPlaneDistance <= 0.0)
        throw new ArgumentException("farPlaneDistance <= 0");
      if ((double) nearPlaneDistance >= (double) farPlaneDistance)
        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
      float num1 = 1f / (float) Math.Tan((double) fieldOfView * 0.5);
      float num2 = num1 / aspectRatio;
      result.M11 = num2;
      result.M12 = result.M13 = result.M14 = 0.0f;
      result.M22 = num1;
      result.M21 = result.M23 = result.M24 = 0.0f;
      result.M31 = result.M32 = 0.0f;
      result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      result.M34 = -1f;
      result.M41 = result.M42 = result.M44 = 0.0f;
      result.M43 = (float) ((double) nearPlaneDistance * (double) farPlaneDistance / ((double) nearPlaneDistance - (double) farPlaneDistance));
    }

    public static Matrix CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
    {
      if ((double) nearPlaneDistance <= 0.0)
        throw new ArgumentException("nearPlaneDistance <= 0");
      if ((double) farPlaneDistance <= 0.0)
        throw new ArgumentException("farPlaneDistance <= 0");
      if ((double) nearPlaneDistance >= (double) farPlaneDistance)
        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
      Matrix matrix;
      matrix.M11 = (float) (2.0 * (double) nearPlaneDistance / ((double) right - (double) left));
      matrix.M12 = matrix.M13 = matrix.M14 = 0.0f;
      matrix.M22 = (float) (2.0 * (double) nearPlaneDistance / ((double) top - (double) bottom));
      matrix.M21 = matrix.M23 = matrix.M24 = 0.0f;
      matrix.M31 = (float) (((double) left + (double) right) / ((double) right - (double) left));
      matrix.M32 = (float) (((double) top + (double) bottom) / ((double) top - (double) bottom));
      matrix.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      matrix.M34 = -1f;
      matrix.M43 = (float) ((double) nearPlaneDistance * (double) farPlaneDistance / ((double) nearPlaneDistance - (double) farPlaneDistance));
      matrix.M41 = matrix.M42 = matrix.M44 = 0.0f;
      return matrix;
    }

    public static void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
    {
      if ((double) nearPlaneDistance <= 0.0)
        throw new ArgumentException("nearPlaneDistance <= 0");
      if ((double) farPlaneDistance <= 0.0)
        throw new ArgumentException("farPlaneDistance <= 0");
      if ((double) nearPlaneDistance >= (double) farPlaneDistance)
        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
      result.M11 = (float) (2.0 * (double) nearPlaneDistance / ((double) right - (double) left));
      result.M12 = result.M13 = result.M14 = 0.0f;
      result.M22 = (float) (2.0 * (double) nearPlaneDistance / ((double) top - (double) bottom));
      result.M21 = result.M23 = result.M24 = 0.0f;
      result.M31 = (float) (((double) left + (double) right) / ((double) right - (double) left));
      result.M32 = (float) (((double) top + (double) bottom) / ((double) top - (double) bottom));
      result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      result.M34 = -1f;
      result.M43 = (float) ((double) nearPlaneDistance * (double) farPlaneDistance / ((double) nearPlaneDistance - (double) farPlaneDistance));
      result.M41 = result.M42 = result.M44 = 0.0f;
    }

    public static Matrix CreateRotationX(float radians)
    {
      Matrix identity = Matrix.Identity;
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      identity.M22 = num1;
      identity.M23 = num2;
      identity.M32 = -num2;
      identity.M33 = num1;
      return identity;
    }

    public static void CreateRotationX(float radians, out Matrix result)
    {
      result = Matrix.Identity;
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      result.M22 = num1;
      result.M23 = num2;
      result.M32 = -num2;
      result.M33 = num1;
    }

    public static Matrix CreateRotationY(float radians)
    {
      Matrix identity = Matrix.Identity;
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      identity.M11 = num1;
      identity.M13 = -num2;
      identity.M31 = num2;
      identity.M33 = num1;
      return identity;
    }

    public static void CreateRotationY(float radians, out Matrix result)
    {
      result = Matrix.Identity;
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      result.M11 = num1;
      result.M13 = -num2;
      result.M31 = num2;
      result.M33 = num1;
    }

    public static Matrix CreateRotationZ(float radians)
    {
      Matrix identity = Matrix.Identity;
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      identity.M11 = num1;
      identity.M12 = num2;
      identity.M21 = -num2;
      identity.M22 = num1;
      return identity;
    }

    public static void CreateRotationZ(float radians, out Matrix result)
    {
      result = Matrix.Identity;
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      result.M11 = num1;
      result.M12 = num2;
      result.M21 = -num2;
      result.M22 = num1;
    }

    public static Matrix CreateScale(float scale)
    {
      Matrix matrix;
      matrix.M11 = scale;
      matrix.M12 = 0.0f;
      matrix.M13 = 0.0f;
      matrix.M14 = 0.0f;
      matrix.M21 = 0.0f;
      matrix.M22 = scale;
      matrix.M23 = 0.0f;
      matrix.M24 = 0.0f;
      matrix.M31 = 0.0f;
      matrix.M32 = 0.0f;
      matrix.M33 = scale;
      matrix.M34 = 0.0f;
      matrix.M41 = 0.0f;
      matrix.M42 = 0.0f;
      matrix.M43 = 0.0f;
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateScale(float scale, out Matrix result)
    {
      result.M11 = scale;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M14 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = scale;
      result.M23 = 0.0f;
      result.M24 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = scale;
      result.M34 = 0.0f;
      result.M41 = 0.0f;
      result.M42 = 0.0f;
      result.M43 = 0.0f;
      result.M44 = 1f;
    }

    public static Matrix CreateScale(float xScale, float yScale, float zScale)
    {
      Matrix matrix;
      matrix.M11 = xScale;
      matrix.M12 = 0.0f;
      matrix.M13 = 0.0f;
      matrix.M14 = 0.0f;
      matrix.M21 = 0.0f;
      matrix.M22 = yScale;
      matrix.M23 = 0.0f;
      matrix.M24 = 0.0f;
      matrix.M31 = 0.0f;
      matrix.M32 = 0.0f;
      matrix.M33 = zScale;
      matrix.M34 = 0.0f;
      matrix.M41 = 0.0f;
      matrix.M42 = 0.0f;
      matrix.M43 = 0.0f;
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateScale(float xScale, float yScale, float zScale, out Matrix result)
    {
      result.M11 = xScale;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M14 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = yScale;
      result.M23 = 0.0f;
      result.M24 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = zScale;
      result.M34 = 0.0f;
      result.M41 = 0.0f;
      result.M42 = 0.0f;
      result.M43 = 0.0f;
      result.M44 = 1f;
    }

    public static Matrix CreateScale(Vector3 scales)
    {
      Matrix matrix;
      matrix.M11 = scales.X;
      matrix.M12 = 0.0f;
      matrix.M13 = 0.0f;
      matrix.M14 = 0.0f;
      matrix.M21 = 0.0f;
      matrix.M22 = scales.Y;
      matrix.M23 = 0.0f;
      matrix.M24 = 0.0f;
      matrix.M31 = 0.0f;
      matrix.M32 = 0.0f;
      matrix.M33 = scales.Z;
      matrix.M34 = 0.0f;
      matrix.M41 = 0.0f;
      matrix.M42 = 0.0f;
      matrix.M43 = 0.0f;
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateScale(ref Vector3 scales, out Matrix result)
    {
      result.M11 = scales.X;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M14 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = scales.Y;
      result.M23 = 0.0f;
      result.M24 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = scales.Z;
      result.M34 = 0.0f;
      result.M41 = 0.0f;
      result.M42 = 0.0f;
      result.M43 = 0.0f;
      result.M44 = 1f;
    }

    public static Matrix CreateTranslation(float xPosition, float yPosition, float zPosition)
    {
      Matrix matrix;
      matrix.M11 = 1f;
      matrix.M12 = 0.0f;
      matrix.M13 = 0.0f;
      matrix.M14 = 0.0f;
      matrix.M21 = 0.0f;
      matrix.M22 = 1f;
      matrix.M23 = 0.0f;
      matrix.M24 = 0.0f;
      matrix.M31 = 0.0f;
      matrix.M32 = 0.0f;
      matrix.M33 = 1f;
      matrix.M34 = 0.0f;
      matrix.M41 = xPosition;
      matrix.M42 = yPosition;
      matrix.M43 = zPosition;
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateTranslation(ref Vector3 position, out Matrix result)
    {
      result.M11 = 1f;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M14 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = 1f;
      result.M23 = 0.0f;
      result.M24 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = 1f;
      result.M34 = 0.0f;
      result.M41 = position.X;
      result.M42 = position.Y;
      result.M43 = position.Z;
      result.M44 = 1f;
    }

    public static Matrix CreateTranslation(Vector3 position)
    {
      Matrix matrix;
      matrix.M11 = 1f;
      matrix.M12 = 0.0f;
      matrix.M13 = 0.0f;
      matrix.M14 = 0.0f;
      matrix.M21 = 0.0f;
      matrix.M22 = 1f;
      matrix.M23 = 0.0f;
      matrix.M24 = 0.0f;
      matrix.M31 = 0.0f;
      matrix.M32 = 0.0f;
      matrix.M33 = 1f;
      matrix.M34 = 0.0f;
      matrix.M41 = position.X;
      matrix.M42 = position.Y;
      matrix.M43 = position.Z;
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateTranslation(float xPosition, float yPosition, float zPosition, out Matrix result)
    {
      result.M11 = 1f;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M14 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = 1f;
      result.M23 = 0.0f;
      result.M24 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = 1f;
      result.M34 = 0.0f;
      result.M41 = xPosition;
      result.M42 = yPosition;
      result.M43 = zPosition;
      result.M44 = 1f;
    }

    public static Matrix CreateReflection(Plane value)
    {
      value.Normalize();
      float num1 = value.Normal.X;
      float num2 = value.Normal.Y;
      float num3 = value.Normal.Z;
      float num4 = -2f * num1;
      float num5 = -2f * num2;
      float num6 = -2f * num3;
      Matrix matrix;
      matrix.M11 = (float) ((double) num4 * (double) num1 + 1.0);
      matrix.M12 = num5 * num1;
      matrix.M13 = num6 * num1;
      matrix.M14 = 0.0f;
      matrix.M21 = num4 * num2;
      matrix.M22 = (float) ((double) num5 * (double) num2 + 1.0);
      matrix.M23 = num6 * num2;
      matrix.M24 = 0.0f;
      matrix.M31 = num4 * num3;
      matrix.M32 = num5 * num3;
      matrix.M33 = (float) ((double) num6 * (double) num3 + 1.0);
      matrix.M34 = 0.0f;
      matrix.M41 = num4 * value.D;
      matrix.M42 = num5 * value.D;
      matrix.M43 = num6 * value.D;
      matrix.M44 = 1f;
      return matrix;
    }

    public static void CreateReflection(ref Plane value, out Matrix result)
    {
      Plane result1;
      Plane.Normalize(ref value, out result1);
      value.Normalize();
      float num1 = result1.Normal.X;
      float num2 = result1.Normal.Y;
      float num3 = result1.Normal.Z;
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
      result.M41 = num4 * result1.D;
      result.M42 = num5 * result1.D;
      result.M43 = num6 * result1.D;
      result.M44 = 1f;
    }

    public static Matrix CreateWorld(Vector3 position, Vector3 forward, Vector3 up)
    {
      Matrix result;
      Matrix.CreateWorld(ref position, ref forward, ref up, out result);
      return result;
    }

    public static void CreateWorld(ref Vector3 position, ref Vector3 forward, ref Vector3 up, out Matrix result)
    {
      Vector3 result1;
      Vector3.Normalize(ref forward, out result1);
      Vector3 result2;
      Vector3.Cross(ref forward, ref up, out result2);
      Vector3 result3;
      Vector3.Cross(ref result2, ref forward, out result3);
      result2.Normalize();
      result3.Normalize();
      result = new Matrix();
      result.Right = result2;
      result.Up = result3;
      result.Forward = result1;
      result.Translation = position;
      result.M44 = 1f;
    }

    public float Determinant()
    {
      float num1 = this.M11;
      float num2 = this.M12;
      float num3 = this.M13;
      float num4 = this.M14;
      float num5 = this.M21;
      float num6 = this.M22;
      float num7 = this.M23;
      float num8 = this.M24;
      float num9 = this.M31;
      float num10 = this.M32;
      float num11 = this.M33;
      float num12 = this.M34;
      float num13 = this.M41;
      float num14 = this.M42;
      float num15 = this.M43;
      float num16 = this.M44;
      float num17 = (float) ((double) num11 * (double) num16 - (double) num12 * (double) num15);
      float num18 = (float) ((double) num10 * (double) num16 - (double) num12 * (double) num14);
      float num19 = (float) ((double) num10 * (double) num15 - (double) num11 * (double) num14);
      float num20 = (float) ((double) num9 * (double) num16 - (double) num12 * (double) num13);
      float num21 = (float) ((double) num9 * (double) num15 - (double) num11 * (double) num13);
      float num22 = (float) ((double) num9 * (double) num14 - (double) num10 * (double) num13);
      return (float) ((double) num1 * ((double) num6 * (double) num17 - (double) num7 * (double) num18 + (double) num8 * (double) num19) - (double) num2 * ((double) num5 * (double) num17 - (double) num7 * (double) num20 + (double) num8 * (double) num21) + (double) num3 * ((double) num5 * (double) num18 - (double) num6 * (double) num20 + (double) num8 * (double) num22) - (double) num4 * ((double) num5 * (double) num19 - (double) num6 * (double) num21 + (double) num7 * (double) num22));
    }

    public static Matrix Divide(Matrix matrix1, Matrix matrix2)
    {
      matrix1.M11 = matrix1.M11 / matrix2.M11;
      matrix1.M12 = matrix1.M12 / matrix2.M12;
      matrix1.M13 = matrix1.M13 / matrix2.M13;
      matrix1.M14 = matrix1.M14 / matrix2.M14;
      matrix1.M21 = matrix1.M21 / matrix2.M21;
      matrix1.M22 = matrix1.M22 / matrix2.M22;
      matrix1.M23 = matrix1.M23 / matrix2.M23;
      matrix1.M24 = matrix1.M24 / matrix2.M24;
      matrix1.M31 = matrix1.M31 / matrix2.M31;
      matrix1.M32 = matrix1.M32 / matrix2.M32;
      matrix1.M33 = matrix1.M33 / matrix2.M33;
      matrix1.M34 = matrix1.M34 / matrix2.M34;
      matrix1.M41 = matrix1.M41 / matrix2.M41;
      matrix1.M42 = matrix1.M42 / matrix2.M42;
      matrix1.M43 = matrix1.M43 / matrix2.M43;
      matrix1.M44 = matrix1.M44 / matrix2.M44;
      return matrix1;
    }

    public static void Divide(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
      result.M11 = matrix1.M11 / matrix2.M11;
      result.M12 = matrix1.M12 / matrix2.M12;
      result.M13 = matrix1.M13 / matrix2.M13;
      result.M14 = matrix1.M14 / matrix2.M14;
      result.M21 = matrix1.M21 / matrix2.M21;
      result.M22 = matrix1.M22 / matrix2.M22;
      result.M23 = matrix1.M23 / matrix2.M23;
      result.M24 = matrix1.M24 / matrix2.M24;
      result.M31 = matrix1.M31 / matrix2.M31;
      result.M32 = matrix1.M32 / matrix2.M32;
      result.M33 = matrix1.M33 / matrix2.M33;
      result.M34 = matrix1.M34 / matrix2.M34;
      result.M41 = matrix1.M41 / matrix2.M41;
      result.M42 = matrix1.M42 / matrix2.M42;
      result.M43 = matrix1.M43 / matrix2.M43;
      result.M44 = matrix1.M44 / matrix2.M44;
    }

    public static Matrix Divide(Matrix matrix1, float divider)
    {
      float num = 1f / divider;
      matrix1.M11 = matrix1.M11 * num;
      matrix1.M12 = matrix1.M12 * num;
      matrix1.M13 = matrix1.M13 * num;
      matrix1.M14 = matrix1.M14 * num;
      matrix1.M21 = matrix1.M21 * num;
      matrix1.M22 = matrix1.M22 * num;
      matrix1.M23 = matrix1.M23 * num;
      matrix1.M24 = matrix1.M24 * num;
      matrix1.M31 = matrix1.M31 * num;
      matrix1.M32 = matrix1.M32 * num;
      matrix1.M33 = matrix1.M33 * num;
      matrix1.M34 = matrix1.M34 * num;
      matrix1.M41 = matrix1.M41 * num;
      matrix1.M42 = matrix1.M42 * num;
      matrix1.M43 = matrix1.M43 * num;
      matrix1.M44 = matrix1.M44 * num;
      return matrix1;
    }

    public static void Divide(ref Matrix matrix1, float divider, out Matrix result)
    {
      float num = 1f / divider;
      result.M11 = matrix1.M11 * num;
      result.M12 = matrix1.M12 * num;
      result.M13 = matrix1.M13 * num;
      result.M14 = matrix1.M14 * num;
      result.M21 = matrix1.M21 * num;
      result.M22 = matrix1.M22 * num;
      result.M23 = matrix1.M23 * num;
      result.M24 = matrix1.M24 * num;
      result.M31 = matrix1.M31 * num;
      result.M32 = matrix1.M32 * num;
      result.M33 = matrix1.M33 * num;
      result.M34 = matrix1.M34 * num;
      result.M41 = matrix1.M41 * num;
      result.M42 = matrix1.M42 * num;
      result.M43 = matrix1.M43 * num;
      result.M44 = matrix1.M44 * num;
    }

    public bool Equals(Matrix other)
    {
      if ((double) this.M11 == (double) other.M11 && (double) this.M22 == (double) other.M22 && ((double) this.M33 == (double) other.M33 && (double) this.M44 == (double) other.M44) && ((double) this.M12 == (double) other.M12 && (double) this.M13 == (double) other.M13 && ((double) this.M14 == (double) other.M14 && (double) this.M21 == (double) other.M21)) && ((double) this.M23 == (double) other.M23 && (double) this.M24 == (double) other.M24 && ((double) this.M31 == (double) other.M31 && (double) this.M32 == (double) other.M32) && ((double) this.M34 == (double) other.M34 && (double) this.M41 == (double) other.M41 && (double) this.M42 == (double) other.M42)))
        return (double) this.M43 == (double) other.M43;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Matrix)
        flag = this.Equals((Matrix) obj);
      return flag;
    }

    public override int GetHashCode()
    {
      return this.M11.GetHashCode() + this.M12.GetHashCode() + this.M13.GetHashCode() + this.M14.GetHashCode() + this.M21.GetHashCode() + this.M22.GetHashCode() + this.M23.GetHashCode() + this.M24.GetHashCode() + this.M31.GetHashCode() + this.M32.GetHashCode() + this.M33.GetHashCode() + this.M34.GetHashCode() + this.M41.GetHashCode() + this.M42.GetHashCode() + this.M43.GetHashCode() + this.M44.GetHashCode();
    }

    public static Matrix Invert(Matrix matrix)
    {
      Matrix.Invert(ref matrix, out matrix);
      return matrix;
    }

    public static void Invert(ref Matrix matrix, out Matrix result)
    {
      float num1 = matrix.M11;
      float num2 = matrix.M12;
      float num3 = matrix.M13;
      float num4 = matrix.M14;
      float num5 = matrix.M21;
      float num6 = matrix.M22;
      float num7 = matrix.M23;
      float num8 = matrix.M24;
      float num9 = matrix.M31;
      float num10 = matrix.M32;
      float num11 = matrix.M33;
      float num12 = matrix.M34;
      float num13 = matrix.M41;
      float num14 = matrix.M42;
      float num15 = matrix.M43;
      float num16 = matrix.M44;
      float num17 = (float) ((double) num11 * (double) num16 - (double) num12 * (double) num15);
      float num18 = (float) ((double) num10 * (double) num16 - (double) num12 * (double) num14);
      float num19 = (float) ((double) num10 * (double) num15 - (double) num11 * (double) num14);
      float num20 = (float) ((double) num9 * (double) num16 - (double) num12 * (double) num13);
      float num21 = (float) ((double) num9 * (double) num15 - (double) num11 * (double) num13);
      float num22 = (float) ((double) num9 * (double) num14 - (double) num10 * (double) num13);
      float num23 = (float) ((double) num6 * (double) num17 - (double) num7 * (double) num18 + (double) num8 * (double) num19);
      float num24 = (float) -((double) num5 * (double) num17 - (double) num7 * (double) num20 + (double) num8 * (double) num21);
      float num25 = (float) ((double) num5 * (double) num18 - (double) num6 * (double) num20 + (double) num8 * (double) num22);
      float num26 = (float) -((double) num5 * (double) num19 - (double) num6 * (double) num21 + (double) num7 * (double) num22);
      float num27 = (float) (1.0 / ((double) num1 * (double) num23 + (double) num2 * (double) num24 + (double) num3 * (double) num25 + (double) num4 * (double) num26));
      result.M11 = num23 * num27;
      result.M21 = num24 * num27;
      result.M31 = num25 * num27;
      result.M41 = num26 * num27;
      result.M12 = (float) -((double) num2 * (double) num17 - (double) num3 * (double) num18 + (double) num4 * (double) num19) * num27;
      result.M22 = (float) ((double) num1 * (double) num17 - (double) num3 * (double) num20 + (double) num4 * (double) num21) * num27;
      result.M32 = (float) -((double) num1 * (double) num18 - (double) num2 * (double) num20 + (double) num4 * (double) num22) * num27;
      result.M42 = (float) ((double) num1 * (double) num19 - (double) num2 * (double) num21 + (double) num3 * (double) num22) * num27;
      float num28 = (float) ((double) num7 * (double) num16 - (double) num8 * (double) num15);
      float num29 = (float) ((double) num6 * (double) num16 - (double) num8 * (double) num14);
      float num30 = (float) ((double) num6 * (double) num15 - (double) num7 * (double) num14);
      float num31 = (float) ((double) num5 * (double) num16 - (double) num8 * (double) num13);
      float num32 = (float) ((double) num5 * (double) num15 - (double) num7 * (double) num13);
      float num33 = (float) ((double) num5 * (double) num14 - (double) num6 * (double) num13);
      result.M13 = (float) ((double) num2 * (double) num28 - (double) num3 * (double) num29 + (double) num4 * (double) num30) * num27;
      result.M23 = (float) -((double) num1 * (double) num28 - (double) num3 * (double) num31 + (double) num4 * (double) num32) * num27;
      result.M33 = (float) ((double) num1 * (double) num29 - (double) num2 * (double) num31 + (double) num4 * (double) num33) * num27;
      result.M43 = (float) -((double) num1 * (double) num30 - (double) num2 * (double) num32 + (double) num3 * (double) num33) * num27;
      float num34 = (float) ((double) num7 * (double) num12 - (double) num8 * (double) num11);
      float num35 = (float) ((double) num6 * (double) num12 - (double) num8 * (double) num10);
      float num36 = (float) ((double) num6 * (double) num11 - (double) num7 * (double) num10);
      float num37 = (float) ((double) num5 * (double) num12 - (double) num8 * (double) num9);
      float num38 = (float) ((double) num5 * (double) num11 - (double) num7 * (double) num9);
      float num39 = (float) ((double) num5 * (double) num10 - (double) num6 * (double) num9);
      result.M14 = (float) -((double) num2 * (double) num34 - (double) num3 * (double) num35 + (double) num4 * (double) num36) * num27;
      result.M24 = (float) ((double) num1 * (double) num34 - (double) num3 * (double) num37 + (double) num4 * (double) num38) * num27;
      result.M34 = (float) -((double) num1 * (double) num35 - (double) num2 * (double) num37 + (double) num4 * (double) num39) * num27;
      result.M44 = (float) ((double) num1 * (double) num36 - (double) num2 * (double) num38 + (double) num3 * (double) num39) * num27;
    }

    public static Matrix Lerp(Matrix matrix1, Matrix matrix2, float amount)
    {
      matrix1.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
      matrix1.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
      matrix1.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
      matrix1.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;
      matrix1.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
      matrix1.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
      matrix1.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
      matrix1.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;
      matrix1.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
      matrix1.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
      matrix1.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
      matrix1.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;
      matrix1.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
      matrix1.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
      matrix1.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
      matrix1.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;
      return matrix1;
    }

    public static void Lerp(ref Matrix matrix1, ref Matrix matrix2, float amount, out Matrix result)
    {
      result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
      result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
      result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
      result.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;
      result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
      result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
      result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
      result.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;
      result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
      result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
      result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
      result.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;
      result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
      result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
      result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
      result.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;
    }

    public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
    {
      float num1 = (float) ((double) matrix1.M11 * (double) matrix2.M11 + (double) matrix1.M12 * (double) matrix2.M21 + (double) matrix1.M13 * (double) matrix2.M31 + (double) matrix1.M14 * (double) matrix2.M41);
      float num2 = (float) ((double) matrix1.M11 * (double) matrix2.M12 + (double) matrix1.M12 * (double) matrix2.M22 + (double) matrix1.M13 * (double) matrix2.M32 + (double) matrix1.M14 * (double) matrix2.M42);
      float num3 = (float) ((double) matrix1.M11 * (double) matrix2.M13 + (double) matrix1.M12 * (double) matrix2.M23 + (double) matrix1.M13 * (double) matrix2.M33 + (double) matrix1.M14 * (double) matrix2.M43);
      float num4 = (float) ((double) matrix1.M11 * (double) matrix2.M14 + (double) matrix1.M12 * (double) matrix2.M24 + (double) matrix1.M13 * (double) matrix2.M34 + (double) matrix1.M14 * (double) matrix2.M44);
      float num5 = (float) ((double) matrix1.M21 * (double) matrix2.M11 + (double) matrix1.M22 * (double) matrix2.M21 + (double) matrix1.M23 * (double) matrix2.M31 + (double) matrix1.M24 * (double) matrix2.M41);
      float num6 = (float) ((double) matrix1.M21 * (double) matrix2.M12 + (double) matrix1.M22 * (double) matrix2.M22 + (double) matrix1.M23 * (double) matrix2.M32 + (double) matrix1.M24 * (double) matrix2.M42);
      float num7 = (float) ((double) matrix1.M21 * (double) matrix2.M13 + (double) matrix1.M22 * (double) matrix2.M23 + (double) matrix1.M23 * (double) matrix2.M33 + (double) matrix1.M24 * (double) matrix2.M43);
      float num8 = (float) ((double) matrix1.M21 * (double) matrix2.M14 + (double) matrix1.M22 * (double) matrix2.M24 + (double) matrix1.M23 * (double) matrix2.M34 + (double) matrix1.M24 * (double) matrix2.M44);
      float num9 = (float) ((double) matrix1.M31 * (double) matrix2.M11 + (double) matrix1.M32 * (double) matrix2.M21 + (double) matrix1.M33 * (double) matrix2.M31 + (double) matrix1.M34 * (double) matrix2.M41);
      float num10 = (float) ((double) matrix1.M31 * (double) matrix2.M12 + (double) matrix1.M32 * (double) matrix2.M22 + (double) matrix1.M33 * (double) matrix2.M32 + (double) matrix1.M34 * (double) matrix2.M42);
      float num11 = (float) ((double) matrix1.M31 * (double) matrix2.M13 + (double) matrix1.M32 * (double) matrix2.M23 + (double) matrix1.M33 * (double) matrix2.M33 + (double) matrix1.M34 * (double) matrix2.M43);
      float num12 = (float) ((double) matrix1.M31 * (double) matrix2.M14 + (double) matrix1.M32 * (double) matrix2.M24 + (double) matrix1.M33 * (double) matrix2.M34 + (double) matrix1.M34 * (double) matrix2.M44);
      float num13 = (float) ((double) matrix1.M41 * (double) matrix2.M11 + (double) matrix1.M42 * (double) matrix2.M21 + (double) matrix1.M43 * (double) matrix2.M31 + (double) matrix1.M44 * (double) matrix2.M41);
      float num14 = (float) ((double) matrix1.M41 * (double) matrix2.M12 + (double) matrix1.M42 * (double) matrix2.M22 + (double) matrix1.M43 * (double) matrix2.M32 + (double) matrix1.M44 * (double) matrix2.M42);
      float num15 = (float) ((double) matrix1.M41 * (double) matrix2.M13 + (double) matrix1.M42 * (double) matrix2.M23 + (double) matrix1.M43 * (double) matrix2.M33 + (double) matrix1.M44 * (double) matrix2.M43);
      float num16 = (float) ((double) matrix1.M41 * (double) matrix2.M14 + (double) matrix1.M42 * (double) matrix2.M24 + (double) matrix1.M43 * (double) matrix2.M34 + (double) matrix1.M44 * (double) matrix2.M44);
      matrix1.M11 = num1;
      matrix1.M12 = num2;
      matrix1.M13 = num3;
      matrix1.M14 = num4;
      matrix1.M21 = num5;
      matrix1.M22 = num6;
      matrix1.M23 = num7;
      matrix1.M24 = num8;
      matrix1.M31 = num9;
      matrix1.M32 = num10;
      matrix1.M33 = num11;
      matrix1.M34 = num12;
      matrix1.M41 = num13;
      matrix1.M42 = num14;
      matrix1.M43 = num15;
      matrix1.M44 = num16;
      return matrix1;
    }

    public static void Multiply(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
      float num1 = (float) ((double) matrix1.M11 * (double) matrix2.M11 + (double) matrix1.M12 * (double) matrix2.M21 + (double) matrix1.M13 * (double) matrix2.M31 + (double) matrix1.M14 * (double) matrix2.M41);
      float num2 = (float) ((double) matrix1.M11 * (double) matrix2.M12 + (double) matrix1.M12 * (double) matrix2.M22 + (double) matrix1.M13 * (double) matrix2.M32 + (double) matrix1.M14 * (double) matrix2.M42);
      float num3 = (float) ((double) matrix1.M11 * (double) matrix2.M13 + (double) matrix1.M12 * (double) matrix2.M23 + (double) matrix1.M13 * (double) matrix2.M33 + (double) matrix1.M14 * (double) matrix2.M43);
      float num4 = (float) ((double) matrix1.M11 * (double) matrix2.M14 + (double) matrix1.M12 * (double) matrix2.M24 + (double) matrix1.M13 * (double) matrix2.M34 + (double) matrix1.M14 * (double) matrix2.M44);
      float num5 = (float) ((double) matrix1.M21 * (double) matrix2.M11 + (double) matrix1.M22 * (double) matrix2.M21 + (double) matrix1.M23 * (double) matrix2.M31 + (double) matrix1.M24 * (double) matrix2.M41);
      float num6 = (float) ((double) matrix1.M21 * (double) matrix2.M12 + (double) matrix1.M22 * (double) matrix2.M22 + (double) matrix1.M23 * (double) matrix2.M32 + (double) matrix1.M24 * (double) matrix2.M42);
      float num7 = (float) ((double) matrix1.M21 * (double) matrix2.M13 + (double) matrix1.M22 * (double) matrix2.M23 + (double) matrix1.M23 * (double) matrix2.M33 + (double) matrix1.M24 * (double) matrix2.M43);
      float num8 = (float) ((double) matrix1.M21 * (double) matrix2.M14 + (double) matrix1.M22 * (double) matrix2.M24 + (double) matrix1.M23 * (double) matrix2.M34 + (double) matrix1.M24 * (double) matrix2.M44);
      float num9 = (float) ((double) matrix1.M31 * (double) matrix2.M11 + (double) matrix1.M32 * (double) matrix2.M21 + (double) matrix1.M33 * (double) matrix2.M31 + (double) matrix1.M34 * (double) matrix2.M41);
      float num10 = (float) ((double) matrix1.M31 * (double) matrix2.M12 + (double) matrix1.M32 * (double) matrix2.M22 + (double) matrix1.M33 * (double) matrix2.M32 + (double) matrix1.M34 * (double) matrix2.M42);
      float num11 = (float) ((double) matrix1.M31 * (double) matrix2.M13 + (double) matrix1.M32 * (double) matrix2.M23 + (double) matrix1.M33 * (double) matrix2.M33 + (double) matrix1.M34 * (double) matrix2.M43);
      float num12 = (float) ((double) matrix1.M31 * (double) matrix2.M14 + (double) matrix1.M32 * (double) matrix2.M24 + (double) matrix1.M33 * (double) matrix2.M34 + (double) matrix1.M34 * (double) matrix2.M44);
      float num13 = (float) ((double) matrix1.M41 * (double) matrix2.M11 + (double) matrix1.M42 * (double) matrix2.M21 + (double) matrix1.M43 * (double) matrix2.M31 + (double) matrix1.M44 * (double) matrix2.M41);
      float num14 = (float) ((double) matrix1.M41 * (double) matrix2.M12 + (double) matrix1.M42 * (double) matrix2.M22 + (double) matrix1.M43 * (double) matrix2.M32 + (double) matrix1.M44 * (double) matrix2.M42);
      float num15 = (float) ((double) matrix1.M41 * (double) matrix2.M13 + (double) matrix1.M42 * (double) matrix2.M23 + (double) matrix1.M43 * (double) matrix2.M33 + (double) matrix1.M44 * (double) matrix2.M43);
      float num16 = (float) ((double) matrix1.M41 * (double) matrix2.M14 + (double) matrix1.M42 * (double) matrix2.M24 + (double) matrix1.M43 * (double) matrix2.M34 + (double) matrix1.M44 * (double) matrix2.M44);
      result.M11 = num1;
      result.M12 = num2;
      result.M13 = num3;
      result.M14 = num4;
      result.M21 = num5;
      result.M22 = num6;
      result.M23 = num7;
      result.M24 = num8;
      result.M31 = num9;
      result.M32 = num10;
      result.M33 = num11;
      result.M34 = num12;
      result.M41 = num13;
      result.M42 = num14;
      result.M43 = num15;
      result.M44 = num16;
    }

    public static Matrix Multiply(Matrix matrix1, float factor)
    {
      matrix1.M11 *= factor;
      matrix1.M12 *= factor;
      matrix1.M13 *= factor;
      matrix1.M14 *= factor;
      matrix1.M21 *= factor;
      matrix1.M22 *= factor;
      matrix1.M23 *= factor;
      matrix1.M24 *= factor;
      matrix1.M31 *= factor;
      matrix1.M32 *= factor;
      matrix1.M33 *= factor;
      matrix1.M34 *= factor;
      matrix1.M41 *= factor;
      matrix1.M42 *= factor;
      matrix1.M43 *= factor;
      matrix1.M44 *= factor;
      return matrix1;
    }

    public static void Multiply(ref Matrix matrix1, float factor, out Matrix result)
    {
      result.M11 = matrix1.M11 * factor;
      result.M12 = matrix1.M12 * factor;
      result.M13 = matrix1.M13 * factor;
      result.M14 = matrix1.M14 * factor;
      result.M21 = matrix1.M21 * factor;
      result.M22 = matrix1.M22 * factor;
      result.M23 = matrix1.M23 * factor;
      result.M24 = matrix1.M24 * factor;
      result.M31 = matrix1.M31 * factor;
      result.M32 = matrix1.M32 * factor;
      result.M33 = matrix1.M33 * factor;
      result.M34 = matrix1.M34 * factor;
      result.M41 = matrix1.M41 * factor;
      result.M42 = matrix1.M42 * factor;
      result.M43 = matrix1.M43 * factor;
      result.M44 = matrix1.M44 * factor;
    }

    public static Matrix Negate(Matrix matrix)
    {
      matrix.M11 = -matrix.M11;
      matrix.M12 = -matrix.M12;
      matrix.M13 = -matrix.M13;
      matrix.M14 = -matrix.M14;
      matrix.M21 = -matrix.M21;
      matrix.M22 = -matrix.M22;
      matrix.M23 = -matrix.M23;
      matrix.M24 = -matrix.M24;
      matrix.M31 = -matrix.M31;
      matrix.M32 = -matrix.M32;
      matrix.M33 = -matrix.M33;
      matrix.M34 = -matrix.M34;
      matrix.M41 = -matrix.M41;
      matrix.M42 = -matrix.M42;
      matrix.M43 = -matrix.M43;
      matrix.M44 = -matrix.M44;
      return matrix;
    }

    public static void Negate(ref Matrix matrix, out Matrix result)
    {
      result.M11 = -matrix.M11;
      result.M12 = -matrix.M12;
      result.M13 = -matrix.M13;
      result.M14 = -matrix.M14;
      result.M21 = -matrix.M21;
      result.M22 = -matrix.M22;
      result.M23 = -matrix.M23;
      result.M24 = -matrix.M24;
      result.M31 = -matrix.M31;
      result.M32 = -matrix.M32;
      result.M33 = -matrix.M33;
      result.M34 = -matrix.M34;
      result.M41 = -matrix.M41;
      result.M42 = -matrix.M42;
      result.M43 = -matrix.M43;
      result.M44 = -matrix.M44;
    }

    public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
    {
      matrix1.M11 = matrix1.M11 - matrix2.M11;
      matrix1.M12 = matrix1.M12 - matrix2.M12;
      matrix1.M13 = matrix1.M13 - matrix2.M13;
      matrix1.M14 = matrix1.M14 - matrix2.M14;
      matrix1.M21 = matrix1.M21 - matrix2.M21;
      matrix1.M22 = matrix1.M22 - matrix2.M22;
      matrix1.M23 = matrix1.M23 - matrix2.M23;
      matrix1.M24 = matrix1.M24 - matrix2.M24;
      matrix1.M31 = matrix1.M31 - matrix2.M31;
      matrix1.M32 = matrix1.M32 - matrix2.M32;
      matrix1.M33 = matrix1.M33 - matrix2.M33;
      matrix1.M34 = matrix1.M34 - matrix2.M34;
      matrix1.M41 = matrix1.M41 - matrix2.M41;
      matrix1.M42 = matrix1.M42 - matrix2.M42;
      matrix1.M43 = matrix1.M43 - matrix2.M43;
      matrix1.M44 = matrix1.M44 - matrix2.M44;
      return matrix1;
    }

    public static void Subtract(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
      result.M11 = matrix1.M11 - matrix2.M11;
      result.M12 = matrix1.M12 - matrix2.M12;
      result.M13 = matrix1.M13 - matrix2.M13;
      result.M14 = matrix1.M14 - matrix2.M14;
      result.M21 = matrix1.M21 - matrix2.M21;
      result.M22 = matrix1.M22 - matrix2.M22;
      result.M23 = matrix1.M23 - matrix2.M23;
      result.M24 = matrix1.M24 - matrix2.M24;
      result.M31 = matrix1.M31 - matrix2.M31;
      result.M32 = matrix1.M32 - matrix2.M32;
      result.M33 = matrix1.M33 - matrix2.M33;
      result.M34 = matrix1.M34 - matrix2.M34;
      result.M41 = matrix1.M41 - matrix2.M41;
      result.M42 = matrix1.M42 - matrix2.M42;
      result.M43 = matrix1.M43 - matrix2.M43;
      result.M44 = matrix1.M44 - matrix2.M44;
    }

    public override string ToString()
    {
      return "{" + string.Format("M11:{0} M12:{1} M13:{2} M14:{3}", (object) this.M11, (object) this.M12, (object) this.M13, (object) this.M14) + "} {" + string.Format("M21:{0} M22:{1} M23:{2} M24:{3}", (object) this.M21, (object) this.M22, (object) this.M23, (object) this.M24) + "} {" + string.Format("M31:{0} M32:{1} M33:{2} M34:{3}", (object) this.M31, (object) this.M32, (object) this.M33, (object) this.M34) + "} {" + string.Format("M41:{0} M42:{1} M43:{2} M44:{3}", (object) this.M41, (object) this.M42, (object) this.M43, (object) this.M44) + "}";
    }

    public static Matrix Transpose(Matrix matrix)
    {
      Matrix result;
      Matrix.Transpose(ref matrix, out result);
      return result;
    }

    public static void Transpose(ref Matrix matrix, out Matrix result)
    {
      Matrix matrix1;
      matrix1.M11 = matrix.M11;
      matrix1.M12 = matrix.M21;
      matrix1.M13 = matrix.M31;
      matrix1.M14 = matrix.M41;
      matrix1.M21 = matrix.M12;
      matrix1.M22 = matrix.M22;
      matrix1.M23 = matrix.M32;
      matrix1.M24 = matrix.M42;
      matrix1.M31 = matrix.M13;
      matrix1.M32 = matrix.M23;
      matrix1.M33 = matrix.M33;
      matrix1.M34 = matrix.M43;
      matrix1.M41 = matrix.M14;
      matrix1.M42 = matrix.M24;
      matrix1.M43 = matrix.M34;
      matrix1.M44 = matrix.M44;
      result = matrix1;
    }

    private static void findDeterminants(ref Matrix matrix, out float major, out float minor1, out float minor2, out float minor3, out float minor4, out float minor5, out float minor6, out float minor7, out float minor8, out float minor9, out float minor10, out float minor11, out float minor12)
    {
      double num1 = (double) matrix.M11 * (double) matrix.M22 - (double) matrix.M12 * (double) matrix.M21;
      double num2 = (double) matrix.M11 * (double) matrix.M23 - (double) matrix.M13 * (double) matrix.M21;
      double num3 = (double) matrix.M11 * (double) matrix.M24 - (double) matrix.M14 * (double) matrix.M21;
      double num4 = (double) matrix.M12 * (double) matrix.M23 - (double) matrix.M13 * (double) matrix.M22;
      double num5 = (double) matrix.M12 * (double) matrix.M24 - (double) matrix.M14 * (double) matrix.M22;
      double num6 = (double) matrix.M13 * (double) matrix.M24 - (double) matrix.M14 * (double) matrix.M23;
      double num7 = (double) matrix.M31 * (double) matrix.M42 - (double) matrix.M32 * (double) matrix.M41;
      double num8 = (double) matrix.M31 * (double) matrix.M43 - (double) matrix.M33 * (double) matrix.M41;
      double num9 = (double) matrix.M31 * (double) matrix.M44 - (double) matrix.M34 * (double) matrix.M41;
      double num10 = (double) matrix.M32 * (double) matrix.M43 - (double) matrix.M33 * (double) matrix.M42;
      double num11 = (double) matrix.M32 * (double) matrix.M44 - (double) matrix.M34 * (double) matrix.M42;
      double num12 = (double) matrix.M33 * (double) matrix.M44 - (double) matrix.M34 * (double) matrix.M43;
      major = (float) (num1 * num12 - num2 * num11 + num3 * num10 + num4 * num9 - num5 * num8 + num6 * num7);
      minor1 = (float) num1;
      minor2 = (float) num2;
      minor3 = (float) num3;
      minor4 = (float) num4;
      minor5 = (float) num5;
      minor6 = (float) num6;
      minor7 = (float) num7;
      minor8 = (float) num8;
      minor9 = (float) num9;
      minor10 = (float) num10;
      minor11 = (float) num11;
      minor12 = (float) num12;
    }

    public bool Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation)
    {
      translation.X = this.M41;
      translation.Y = this.M42;
      translation.Z = this.M43;
      float num1 = Math.Sign(this.M11 * this.M12 * this.M13 * this.M14) < 0 ? -1f : 1f;
      float num2 = Math.Sign(this.M21 * this.M22 * this.M23 * this.M24) < 0 ? -1f : 1f;
      float num3 = Math.Sign(this.M31 * this.M32 * this.M33 * this.M34) < 0 ? -1f : 1f;
      scale.X = num1 * (float) Math.Sqrt((double) this.M11 * (double) this.M11 + (double) this.M12 * (double) this.M12 + (double) this.M13 * (double) this.M13);
      scale.Y = num2 * (float) Math.Sqrt((double) this.M21 * (double) this.M21 + (double) this.M22 * (double) this.M22 + (double) this.M23 * (double) this.M23);
      scale.Z = num3 * (float) Math.Sqrt((double) this.M31 * (double) this.M31 + (double) this.M32 * (double) this.M32 + (double) this.M33 * (double) this.M33);
      if ((double) scale.X == 0.0 || (double) scale.Y == 0.0 || (double) scale.Z == 0.0)
      {
        rotation = Quaternion.Identity;
        return false;
      }
      else
      {
        Matrix matrix = new Matrix(this.M11 / scale.X, this.M12 / scale.X, this.M13 / scale.X, 0.0f, this.M21 / scale.Y, this.M22 / scale.Y, this.M23 / scale.Y, 0.0f, this.M31 / scale.Z, this.M32 / scale.Z, this.M33 / scale.Z, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
        rotation = Quaternion.CreateFromRotationMatrix(matrix);
        return true;
      }
    }
  }
}
