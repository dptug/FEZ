// Type: OpenTK.Matrix4
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  [Serializable]
  public struct Matrix4 : IEquatable<Matrix4>
  {
    public static Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);
    public Vector4 Row0;
    public Vector4 Row1;
    public Vector4 Row2;
    public Vector4 Row3;

    public float Determinant
    {
      get
      {
        return (float) ((double) this.Row0.X * (double) this.Row1.Y * (double) this.Row2.Z * (double) this.Row3.W - (double) this.Row0.X * (double) this.Row1.Y * (double) this.Row2.W * (double) this.Row3.Z + (double) this.Row0.X * (double) this.Row1.Z * (double) this.Row2.W * (double) this.Row3.Y - (double) this.Row0.X * (double) this.Row1.Z * (double) this.Row2.Y * (double) this.Row3.W + (double) this.Row0.X * (double) this.Row1.W * (double) this.Row2.Y * (double) this.Row3.Z - (double) this.Row0.X * (double) this.Row1.W * (double) this.Row2.Z * (double) this.Row3.Y - (double) this.Row0.Y * (double) this.Row1.Z * (double) this.Row2.W * (double) this.Row3.X + (double) this.Row0.Y * (double) this.Row1.Z * (double) this.Row2.X * (double) this.Row3.W - (double) this.Row0.Y * (double) this.Row1.W * (double) this.Row2.X * (double) this.Row3.Z + (double) this.Row0.Y * (double) this.Row1.W * (double) this.Row2.Z * (double) this.Row3.X - (double) this.Row0.Y * (double) this.Row1.X * (double) this.Row2.Z * (double) this.Row3.W + (double) this.Row0.Y * (double) this.Row1.X * (double) this.Row2.W * (double) this.Row3.Z + (double) this.Row0.Z * (double) this.Row1.W * (double) this.Row2.X * (double) this.Row3.Y - (double) this.Row0.Z * (double) this.Row1.W * (double) this.Row2.Y * (double) this.Row3.X + (double) this.Row0.Z * (double) this.Row1.X * (double) this.Row2.Y * (double) this.Row3.W - (double) this.Row0.Z * (double) this.Row1.X * (double) this.Row2.W * (double) this.Row3.Y + (double) this.Row0.Z * (double) this.Row1.Y * (double) this.Row2.W * (double) this.Row3.X - (double) this.Row0.Z * (double) this.Row1.Y * (double) this.Row2.X * (double) this.Row3.W - (double) this.Row0.W * (double) this.Row1.X * (double) this.Row2.Y * (double) this.Row3.Z + (double) this.Row0.W * (double) this.Row1.X * (double) this.Row2.Z * (double) this.Row3.Y - (double) this.Row0.W * (double) this.Row1.Y * (double) this.Row2.Z * (double) this.Row3.X + (double) this.Row0.W * (double) this.Row1.Y * (double) this.Row2.X * (double) this.Row3.Z - (double) this.Row0.W * (double) this.Row1.Z * (double) this.Row2.X * (double) this.Row3.Y + (double) this.Row0.W * (double) this.Row1.Z * (double) this.Row2.Y * (double) this.Row3.X);
      }
    }

    public Vector4 Column0
    {
      get
      {
        return new Vector4(this.Row0.X, this.Row1.X, this.Row2.X, this.Row3.X);
      }
    }

    public Vector4 Column1
    {
      get
      {
        return new Vector4(this.Row0.Y, this.Row1.Y, this.Row2.Y, this.Row3.Y);
      }
    }

    public Vector4 Column2
    {
      get
      {
        return new Vector4(this.Row0.Z, this.Row1.Z, this.Row2.Z, this.Row3.Z);
      }
    }

    public Vector4 Column3
    {
      get
      {
        return new Vector4(this.Row0.W, this.Row1.W, this.Row2.W, this.Row3.W);
      }
    }

    public float M11
    {
      get
      {
        return this.Row0.X;
      }
      set
      {
        this.Row0.X = value;
      }
    }

    public float M12
    {
      get
      {
        return this.Row0.Y;
      }
      set
      {
        this.Row0.Y = value;
      }
    }

    public float M13
    {
      get
      {
        return this.Row0.Z;
      }
      set
      {
        this.Row0.Z = value;
      }
    }

    public float M14
    {
      get
      {
        return this.Row0.W;
      }
      set
      {
        this.Row0.W = value;
      }
    }

    public float M21
    {
      get
      {
        return this.Row1.X;
      }
      set
      {
        this.Row1.X = value;
      }
    }

    public float M22
    {
      get
      {
        return this.Row1.Y;
      }
      set
      {
        this.Row1.Y = value;
      }
    }

    public float M23
    {
      get
      {
        return this.Row1.Z;
      }
      set
      {
        this.Row1.Z = value;
      }
    }

    public float M24
    {
      get
      {
        return this.Row1.W;
      }
      set
      {
        this.Row1.W = value;
      }
    }

    public float M31
    {
      get
      {
        return this.Row2.X;
      }
      set
      {
        this.Row2.X = value;
      }
    }

    public float M32
    {
      get
      {
        return this.Row2.Y;
      }
      set
      {
        this.Row2.Y = value;
      }
    }

    public float M33
    {
      get
      {
        return this.Row2.Z;
      }
      set
      {
        this.Row2.Z = value;
      }
    }

    public float M34
    {
      get
      {
        return this.Row2.W;
      }
      set
      {
        this.Row2.W = value;
      }
    }

    public float M41
    {
      get
      {
        return this.Row3.X;
      }
      set
      {
        this.Row3.X = value;
      }
    }

    public float M42
    {
      get
      {
        return this.Row3.Y;
      }
      set
      {
        this.Row3.Y = value;
      }
    }

    public float M43
    {
      get
      {
        return this.Row3.Z;
      }
      set
      {
        this.Row3.Z = value;
      }
    }

    public float M44
    {
      get
      {
        return this.Row3.W;
      }
      set
      {
        this.Row3.W = value;
      }
    }

    static Matrix4()
    {
    }

    public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
    {
      this.Row0 = row0;
      this.Row1 = row1;
      this.Row2 = row2;
      this.Row3 = row3;
    }

    public Matrix4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
    {
      this.Row0 = new Vector4(m00, m01, m02, m03);
      this.Row1 = new Vector4(m10, m11, m12, m13);
      this.Row2 = new Vector4(m20, m21, m22, m23);
      this.Row3 = new Vector4(m30, m31, m32, m33);
    }

    public static Matrix4 operator *(Matrix4 left, Matrix4 right)
    {
      return Matrix4.Mult(left, right);
    }

    public static bool operator ==(Matrix4 left, Matrix4 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Matrix4 left, Matrix4 right)
    {
      return !left.Equals(right);
    }

    public void Invert()
    {
      this = Matrix4.Invert(this);
    }

    public void Transpose()
    {
      this = Matrix4.Transpose(this);
    }

    public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix4 result)
    {
      float num1 = (float) Math.Cos(-(double) angle);
      float num2 = (float) Math.Sin(-(double) angle);
      float num3 = 1f - num1;
      axis.Normalize();
      result = new Matrix4(num3 * axis.X * axis.X + num1, (float) ((double) num3 * (double) axis.X * (double) axis.Y - (double) num2 * (double) axis.Z), (float) ((double) num3 * (double) axis.X * (double) axis.Z + (double) num2 * (double) axis.Y), 0.0f, (float) ((double) num3 * (double) axis.X * (double) axis.Y + (double) num2 * (double) axis.Z), num3 * axis.Y * axis.Y + num1, (float) ((double) num3 * (double) axis.Y * (double) axis.Z - (double) num2 * (double) axis.X), 0.0f, (float) ((double) num3 * (double) axis.X * (double) axis.Z - (double) num2 * (double) axis.Y), (float) ((double) num3 * (double) axis.Y * (double) axis.Z + (double) num2 * (double) axis.X), num3 * axis.Z * axis.Z + num1, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
    }

    public static Matrix4 CreateFromAxisAngle(Vector3 axis, float angle)
    {
      Matrix4 result;
      Matrix4.CreateFromAxisAngle(axis, angle, out result);
      return result;
    }

    public static void CreateRotationX(float angle, out Matrix4 result)
    {
      float num = (float) Math.Cos((double) angle);
      float z = (float) Math.Sin((double) angle);
      result.Row0 = Vector4.UnitX;
      result.Row1 = new Vector4(0.0f, num, z, 0.0f);
      result.Row2 = new Vector4(0.0f, -z, num, 0.0f);
      result.Row3 = Vector4.UnitW;
    }

    public static Matrix4 CreateRotationX(float angle)
    {
      Matrix4 result;
      Matrix4.CreateRotationX(angle, out result);
      return result;
    }

    public static void CreateRotationY(float angle, out Matrix4 result)
    {
      float num = (float) Math.Cos((double) angle);
      float x = (float) Math.Sin((double) angle);
      result.Row0 = new Vector4(num, 0.0f, -x, 0.0f);
      result.Row1 = Vector4.UnitY;
      result.Row2 = new Vector4(x, 0.0f, num, 0.0f);
      result.Row3 = Vector4.UnitW;
    }

    public static Matrix4 CreateRotationY(float angle)
    {
      Matrix4 result;
      Matrix4.CreateRotationY(angle, out result);
      return result;
    }

    public static void CreateRotationZ(float angle, out Matrix4 result)
    {
      float num = (float) Math.Cos((double) angle);
      float y = (float) Math.Sin((double) angle);
      result.Row0 = new Vector4(num, y, 0.0f, 0.0f);
      result.Row1 = new Vector4(-y, num, 0.0f, 0.0f);
      result.Row2 = Vector4.UnitZ;
      result.Row3 = Vector4.UnitW;
    }

    public static Matrix4 CreateRotationZ(float angle)
    {
      Matrix4 result;
      Matrix4.CreateRotationZ(angle, out result);
      return result;
    }

    public static void CreateTranslation(float x, float y, float z, out Matrix4 result)
    {
      result = Matrix4.Identity;
      result.Row3 = new Vector4(x, y, z, 1f);
    }

    public static void CreateTranslation(ref Vector3 vector, out Matrix4 result)
    {
      result = Matrix4.Identity;
      result.Row3 = new Vector4(vector.X, vector.Y, vector.Z, 1f);
    }

    public static Matrix4 CreateTranslation(float x, float y, float z)
    {
      Matrix4 result;
      Matrix4.CreateTranslation(x, y, z, out result);
      return result;
    }

    public static Matrix4 CreateTranslation(Vector3 vector)
    {
      Matrix4 result;
      Matrix4.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
      return result;
    }

    public static void CreateOrthographic(float width, float height, float zNear, float zFar, out Matrix4 result)
    {
      Matrix4.CreateOrthographicOffCenter((float) (-(double) width / 2.0), width / 2f, (float) (-(double) height / 2.0), height / 2f, zNear, zFar, out result);
    }

    public static Matrix4 CreateOrthographic(float width, float height, float zNear, float zFar)
    {
      Matrix4 result;
      Matrix4.CreateOrthographicOffCenter((float) (-(double) width / 2.0), width / 2f, (float) (-(double) height / 2.0), height / 2f, zNear, zFar, out result);
      return result;
    }

    public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4 result)
    {
      result = new Matrix4();
      float num1 = (float) (1.0 / ((double) right - (double) left));
      float num2 = (float) (1.0 / ((double) top - (double) bottom));
      float num3 = (float) (1.0 / ((double) zFar - (double) zNear));
      result.M11 = 2f * num1;
      result.M22 = 2f * num2;
      result.M33 = -2f * num3;
      result.M41 = (float) -((double) right + (double) left) * num1;
      result.M42 = (float) -((double) top + (double) bottom) * num2;
      result.M43 = (float) -((double) zFar + (double) zNear) * num3;
      result.M44 = 1f;
    }

    public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
    {
      Matrix4 result;
      Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, zNear, zFar, out result);
      return result;
    }

    public static void CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar, out Matrix4 result)
    {
      if ((double) fovy <= 0.0 || (double) fovy > Math.PI)
        throw new ArgumentOutOfRangeException("fovy");
      if ((double) aspect <= 0.0)
        throw new ArgumentOutOfRangeException("aspect");
      if ((double) zNear <= 0.0)
        throw new ArgumentOutOfRangeException("zNear");
      if ((double) zFar <= 0.0)
        throw new ArgumentOutOfRangeException("zFar");
      float top = zNear * (float) Math.Tan(0.5 * (double) fovy);
      float bottom = -top;
      Matrix4.CreatePerspectiveOffCenter(bottom * aspect, top * aspect, bottom, top, zNear, zFar, out result);
    }

    public static Matrix4 CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar)
    {
      Matrix4 result;
      Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar, out result);
      return result;
    }

    public static void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4 result)
    {
      if ((double) zNear <= 0.0)
        throw new ArgumentOutOfRangeException("zNear");
      if ((double) zFar <= 0.0)
        throw new ArgumentOutOfRangeException("zFar");
      if ((double) zNear >= (double) zFar)
        throw new ArgumentOutOfRangeException("zNear");
      float m00 = (float) (2.0 * (double) zNear / ((double) right - (double) left));
      float m11 = (float) (2.0 * (double) zNear / ((double) top - (double) bottom));
      float m20 = (float) (((double) right + (double) left) / ((double) right - (double) left));
      float m21 = (float) (((double) top + (double) bottom) / ((double) top - (double) bottom));
      float m22 = (float) (-((double) zFar + (double) zNear) / ((double) zFar - (double) zNear));
      float m32 = (float) (-(2.0 * (double) zFar * (double) zNear) / ((double) zFar - (double) zNear));
      result = new Matrix4(m00, 0.0f, 0.0f, 0.0f, 0.0f, m11, 0.0f, 0.0f, m20, m21, m22, -1f, 0.0f, 0.0f, m32, 0.0f);
    }

    public static Matrix4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
    {
      Matrix4 result;
      Matrix4.CreatePerspectiveOffCenter(left, right, bottom, top, zNear, zFar, out result);
      return result;
    }

    [Obsolete("Use CreateTranslation instead.")]
    public static Matrix4 Translation(Vector3 trans)
    {
      return Matrix4.Translation(trans.X, trans.Y, trans.Z);
    }

    [Obsolete("Use CreateTranslation instead.")]
    public static Matrix4 Translation(float x, float y, float z)
    {
      Matrix4 matrix4 = Matrix4.Identity;
      matrix4.Row3 = new Vector4(x, y, z, 1f);
      return matrix4;
    }

    public static Matrix4 Scale(float scale)
    {
      return Matrix4.Scale(scale, scale, scale);
    }

    public static Matrix4 Scale(Vector3 scale)
    {
      return Matrix4.Scale(scale.X, scale.Y, scale.Z);
    }

    public static Matrix4 Scale(float x, float y, float z)
    {
      Matrix4 matrix4;
      matrix4.Row0 = Vector4.UnitX * x;
      matrix4.Row1 = Vector4.UnitY * y;
      matrix4.Row2 = Vector4.UnitZ * z;
      matrix4.Row3 = Vector4.UnitW;
      return matrix4;
    }

    [Obsolete("Use CreateRotationX instead.")]
    public static Matrix4 RotateX(float angle)
    {
      float num = (float) Math.Cos((double) angle);
      float z = (float) Math.Sin((double) angle);
      Matrix4 matrix4;
      matrix4.Row0 = Vector4.UnitX;
      matrix4.Row1 = new Vector4(0.0f, num, z, 0.0f);
      matrix4.Row2 = new Vector4(0.0f, -z, num, 0.0f);
      matrix4.Row3 = Vector4.UnitW;
      return matrix4;
    }

    [Obsolete("Use CreateRotationY instead.")]
    public static Matrix4 RotateY(float angle)
    {
      float num = (float) Math.Cos((double) angle);
      float x = (float) Math.Sin((double) angle);
      Matrix4 matrix4;
      matrix4.Row0 = new Vector4(num, 0.0f, -x, 0.0f);
      matrix4.Row1 = Vector4.UnitY;
      matrix4.Row2 = new Vector4(x, 0.0f, num, 0.0f);
      matrix4.Row3 = Vector4.UnitW;
      return matrix4;
    }

    [Obsolete("Use CreateRotationZ instead.")]
    public static Matrix4 RotateZ(float angle)
    {
      float num = (float) Math.Cos((double) angle);
      float y = (float) Math.Sin((double) angle);
      Matrix4 matrix4;
      matrix4.Row0 = new Vector4(num, y, 0.0f, 0.0f);
      matrix4.Row1 = new Vector4(-y, num, 0.0f, 0.0f);
      matrix4.Row2 = Vector4.UnitZ;
      matrix4.Row3 = Vector4.UnitW;
      return matrix4;
    }

    [Obsolete("Use CreateFromAxisAngle instead.")]
    public static Matrix4 Rotate(Vector3 axis, float angle)
    {
      float num1 = (float) Math.Cos(-(double) angle);
      float num2 = (float) Math.Sin(-(double) angle);
      float num3 = 1f - num1;
      axis.Normalize();
      Matrix4 matrix4;
      matrix4.Row0 = new Vector4(num3 * axis.X * axis.X + num1, (float) ((double) num3 * (double) axis.X * (double) axis.Y - (double) num2 * (double) axis.Z), (float) ((double) num3 * (double) axis.X * (double) axis.Z + (double) num2 * (double) axis.Y), 0.0f);
      matrix4.Row1 = new Vector4((float) ((double) num3 * (double) axis.X * (double) axis.Y + (double) num2 * (double) axis.Z), num3 * axis.Y * axis.Y + num1, (float) ((double) num3 * (double) axis.Y * (double) axis.Z - (double) num2 * (double) axis.X), 0.0f);
      matrix4.Row2 = new Vector4((float) ((double) num3 * (double) axis.X * (double) axis.Z - (double) num2 * (double) axis.Y), (float) ((double) num3 * (double) axis.Y * (double) axis.Z + (double) num2 * (double) axis.X), num3 * axis.Z * axis.Z + num1, 0.0f);
      matrix4.Row3 = Vector4.UnitW;
      return matrix4;
    }

    public static Matrix4 Rotate(Quaternion q)
    {
      Vector3 axis;
      float angle;
      q.ToAxisAngle(out axis, out angle);
      return Matrix4.CreateFromAxisAngle(axis, angle);
    }

    public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
    {
      Vector3 vector3_1 = Vector3.Normalize(eye - target);
      Vector3 right = Vector3.Normalize(Vector3.Cross(up, vector3_1));
      Vector3 vector3_2 = Vector3.Normalize(Vector3.Cross(vector3_1, right));
      Matrix4 matrix4 = new Matrix4(new Vector4(right.X, vector3_2.X, vector3_1.X, 0.0f), new Vector4(right.Y, vector3_2.Y, vector3_1.Y, 0.0f), new Vector4(right.Z, vector3_2.Z, vector3_1.Z, 0.0f), Vector4.UnitW);
      return Matrix4.CreateTranslation(-eye) * matrix4;
    }

    public static Matrix4 LookAt(float eyeX, float eyeY, float eyeZ, float targetX, float targetY, float targetZ, float upX, float upY, float upZ)
    {
      return Matrix4.LookAt(new Vector3(eyeX, eyeY, eyeZ), new Vector3(targetX, targetY, targetZ), new Vector3(upX, upY, upZ));
    }

    [Obsolete("Use CreatePerspectiveOffCenter instead.")]
    public static Matrix4 Frustum(float left, float right, float bottom, float top, float near, float far)
    {
      float num1 = (float) (1.0 / ((double) right - (double) left));
      float num2 = (float) (1.0 / ((double) top - (double) bottom));
      float num3 = (float) (1.0 / ((double) far - (double) near));
      return new Matrix4(new Vector4(2f * near * num1, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 2f * near * num2, 0.0f, 0.0f), new Vector4((right + left) * num1, (top + bottom) * num2, (float) -((double) far + (double) near) * num3, -1f), new Vector4(0.0f, 0.0f, -2f * far * near * num3, 0.0f));
    }

    [Obsolete("Use CreatePerspectiveFieldOfView instead.")]
    public static Matrix4 Perspective(float fovy, float aspect, float near, float far)
    {
      float top = near * (float) Math.Tan(0.5 * (double) fovy);
      float bottom = -top;
      return Matrix4.Frustum(bottom * aspect, top * aspect, bottom, top, near, far);
    }

    public static Matrix4 Mult(Matrix4 left, Matrix4 right)
    {
      Matrix4 result;
      Matrix4.Mult(ref left, ref right, out result);
      return result;
    }

    public static void Mult(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
    {
      float num1 = left.Row0.X;
      float num2 = left.Row0.Y;
      float num3 = left.Row0.Z;
      float num4 = left.Row0.W;
      float num5 = left.Row1.X;
      float num6 = left.Row1.Y;
      float num7 = left.Row1.Z;
      float num8 = left.Row1.W;
      float num9 = left.Row2.X;
      float num10 = left.Row2.Y;
      float num11 = left.Row2.Z;
      float num12 = left.Row2.W;
      float num13 = left.Row3.X;
      float num14 = left.Row3.Y;
      float num15 = left.Row3.Z;
      float num16 = left.Row3.W;
      float num17 = right.Row0.X;
      float num18 = right.Row0.Y;
      float num19 = right.Row0.Z;
      float num20 = right.Row0.W;
      float num21 = right.Row1.X;
      float num22 = right.Row1.Y;
      float num23 = right.Row1.Z;
      float num24 = right.Row1.W;
      float num25 = right.Row2.X;
      float num26 = right.Row2.Y;
      float num27 = right.Row2.Z;
      float num28 = right.Row2.W;
      float num29 = right.Row3.X;
      float num30 = right.Row3.Y;
      float num31 = right.Row3.Z;
      float num32 = right.Row3.W;
      result.Row0.X = (float) ((double) num1 * (double) num17 + (double) num2 * (double) num21 + (double) num3 * (double) num25 + (double) num4 * (double) num29);
      result.Row0.Y = (float) ((double) num1 * (double) num18 + (double) num2 * (double) num22 + (double) num3 * (double) num26 + (double) num4 * (double) num30);
      result.Row0.Z = (float) ((double) num1 * (double) num19 + (double) num2 * (double) num23 + (double) num3 * (double) num27 + (double) num4 * (double) num31);
      result.Row0.W = (float) ((double) num1 * (double) num20 + (double) num2 * (double) num24 + (double) num3 * (double) num28 + (double) num4 * (double) num32);
      result.Row1.X = (float) ((double) num5 * (double) num17 + (double) num6 * (double) num21 + (double) num7 * (double) num25 + (double) num8 * (double) num29);
      result.Row1.Y = (float) ((double) num5 * (double) num18 + (double) num6 * (double) num22 + (double) num7 * (double) num26 + (double) num8 * (double) num30);
      result.Row1.Z = (float) ((double) num5 * (double) num19 + (double) num6 * (double) num23 + (double) num7 * (double) num27 + (double) num8 * (double) num31);
      result.Row1.W = (float) ((double) num5 * (double) num20 + (double) num6 * (double) num24 + (double) num7 * (double) num28 + (double) num8 * (double) num32);
      result.Row2.X = (float) ((double) num9 * (double) num17 + (double) num10 * (double) num21 + (double) num11 * (double) num25 + (double) num12 * (double) num29);
      result.Row2.Y = (float) ((double) num9 * (double) num18 + (double) num10 * (double) num22 + (double) num11 * (double) num26 + (double) num12 * (double) num30);
      result.Row2.Z = (float) ((double) num9 * (double) num19 + (double) num10 * (double) num23 + (double) num11 * (double) num27 + (double) num12 * (double) num31);
      result.Row2.W = (float) ((double) num9 * (double) num20 + (double) num10 * (double) num24 + (double) num11 * (double) num28 + (double) num12 * (double) num32);
      result.Row3.X = (float) ((double) num13 * (double) num17 + (double) num14 * (double) num21 + (double) num15 * (double) num25 + (double) num16 * (double) num29);
      result.Row3.Y = (float) ((double) num13 * (double) num18 + (double) num14 * (double) num22 + (double) num15 * (double) num26 + (double) num16 * (double) num30);
      result.Row3.Z = (float) ((double) num13 * (double) num19 + (double) num14 * (double) num23 + (double) num15 * (double) num27 + (double) num16 * (double) num31);
      result.Row3.W = (float) ((double) num13 * (double) num20 + (double) num14 * (double) num24 + (double) num15 * (double) num28 + (double) num16 * (double) num32);
    }

    public static Matrix4 Invert(Matrix4 mat)
    {
      int[] numArray1 = new int[4];
      int[] numArray2 = new int[4];
      int[] numArray3 = new int[4]
      {
        -1,
        -1,
        -1,
        -1
      };
      float[,] numArray4 = new float[4, 4]
      {
        {
          mat.Row0.X,
          mat.Row0.Y,
          mat.Row0.Z,
          mat.Row0.W
        },
        {
          mat.Row1.X,
          mat.Row1.Y,
          mat.Row1.Z,
          mat.Row1.W
        },
        {
          mat.Row2.X,
          mat.Row2.Y,
          mat.Row2.Z,
          mat.Row2.W
        },
        {
          mat.Row3.X,
          mat.Row3.Y,
          mat.Row3.Z,
          mat.Row3.W
        }
      };
      int index1 = 0;
      int index2 = 0;
      for (int index3 = 0; index3 < 4; ++index3)
      {
        float num1 = 0.0f;
        for (int index4 = 0; index4 < 4; ++index4)
        {
          if (numArray3[index4] != 0)
          {
            for (int index5 = 0; index5 < 4; ++index5)
            {
              if (numArray3[index5] == -1)
              {
                float num2 = Math.Abs(numArray4[index4, index5]);
                if ((double) num2 > (double) num1)
                {
                  num1 = num2;
                  index2 = index4;
                  index1 = index5;
                }
              }
              else if (numArray3[index5] > 0)
                return mat;
            }
          }
        }
        ++numArray3[index1];
        if (index2 != index1)
        {
          for (int index4 = 0; index4 < 4; ++index4)
          {
            float num2 = numArray4[index2, index4];
            numArray4[index2, index4] = numArray4[index1, index4];
            numArray4[index1, index4] = num2;
          }
        }
        numArray2[index3] = index2;
        numArray1[index3] = index1;
        float num3 = numArray4[index1, index1];
        if ((double) num3 == 0.0)
          throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
        float num4 = 1f / num3;
        numArray4[index1, index1] = 1f;
        for (int index4 = 0; index4 < 4; ++index4)
          numArray4[index1, index4] *= num4;
        for (int index4 = 0; index4 < 4; ++index4)
        {
          if (index1 != index4)
          {
            float num2 = numArray4[index4, index1];
            numArray4[index4, index1] = 0.0f;
            for (int index5 = 0; index5 < 4; ++index5)
              numArray4[index4, index5] -= numArray4[index1, index5] * num2;
          }
        }
      }
      for (int index3 = 3; index3 >= 0; --index3)
      {
        int index4 = numArray2[index3];
        int index5 = numArray1[index3];
        for (int index6 = 0; index6 < 4; ++index6)
        {
          float num = numArray4[index6, index4];
          numArray4[index6, index4] = numArray4[index6, index5];
          numArray4[index6, index5] = num;
        }
      }
      mat.Row0 = new Vector4(numArray4[0, 0], numArray4[0, 1], numArray4[0, 2], numArray4[0, 3]);
      mat.Row1 = new Vector4(numArray4[1, 0], numArray4[1, 1], numArray4[1, 2], numArray4[1, 3]);
      mat.Row2 = new Vector4(numArray4[2, 0], numArray4[2, 1], numArray4[2, 2], numArray4[2, 3]);
      mat.Row3 = new Vector4(numArray4[3, 0], numArray4[3, 1], numArray4[3, 2], numArray4[3, 3]);
      return mat;
    }

    public static Matrix4 Transpose(Matrix4 mat)
    {
      return new Matrix4(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
    }

    public static void Transpose(ref Matrix4 mat, out Matrix4 result)
    {
      result.Row0 = mat.Column0;
      result.Row1 = mat.Column1;
      result.Row2 = mat.Column2;
      result.Row3 = mat.Column3;
    }

    public override string ToString()
    {
      return string.Format("{0}\n{1}\n{2}\n{3}", (object) this.Row0, (object) this.Row1, (object) this.Row2, (object) this.Row3);
    }

    public override int GetHashCode()
    {
      return this.Row0.GetHashCode() ^ this.Row1.GetHashCode() ^ this.Row2.GetHashCode() ^ this.Row3.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Matrix4))
        return false;
      else
        return this.Equals((Matrix4) obj);
    }

    public bool Equals(Matrix4 other)
    {
      if (this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2)
        return this.Row3 == other.Row3;
      else
        return false;
    }
  }
}
