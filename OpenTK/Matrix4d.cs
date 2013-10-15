// Type: OpenTK.Matrix4d
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  [Serializable]
  public struct Matrix4d : IEquatable<Matrix4d>
  {
    public static Matrix4d Identity = new Matrix4d(Vector4d.UnitX, Vector4d.UnitY, Vector4d.UnitZ, Vector4d.UnitW);
    public Vector4d Row0;
    public Vector4d Row1;
    public Vector4d Row2;
    public Vector4d Row3;

    public double Determinant
    {
      get
      {
        return this.Row0.X * this.Row1.Y * this.Row2.Z * this.Row3.W - this.Row0.X * this.Row1.Y * this.Row2.W * this.Row3.Z + this.Row0.X * this.Row1.Z * this.Row2.W * this.Row3.Y - this.Row0.X * this.Row1.Z * this.Row2.Y * this.Row3.W + this.Row0.X * this.Row1.W * this.Row2.Y * this.Row3.Z - this.Row0.X * this.Row1.W * this.Row2.Z * this.Row3.Y - this.Row0.Y * this.Row1.Z * this.Row2.W * this.Row3.X + this.Row0.Y * this.Row1.Z * this.Row2.X * this.Row3.W - this.Row0.Y * this.Row1.W * this.Row2.X * this.Row3.Z + this.Row0.Y * this.Row1.W * this.Row2.Z * this.Row3.X - this.Row0.Y * this.Row1.X * this.Row2.Z * this.Row3.W + this.Row0.Y * this.Row1.X * this.Row2.W * this.Row3.Z + this.Row0.Z * this.Row1.W * this.Row2.X * this.Row3.Y - this.Row0.Z * this.Row1.W * this.Row2.Y * this.Row3.X + this.Row0.Z * this.Row1.X * this.Row2.Y * this.Row3.W - this.Row0.Z * this.Row1.X * this.Row2.W * this.Row3.Y + this.Row0.Z * this.Row1.Y * this.Row2.W * this.Row3.X - this.Row0.Z * this.Row1.Y * this.Row2.X * this.Row3.W - this.Row0.W * this.Row1.X * this.Row2.Y * this.Row3.Z + this.Row0.W * this.Row1.X * this.Row2.Z * this.Row3.Y - this.Row0.W * this.Row1.Y * this.Row2.Z * this.Row3.X + this.Row0.W * this.Row1.Y * this.Row2.X * this.Row3.Z - this.Row0.W * this.Row1.Z * this.Row2.X * this.Row3.Y + this.Row0.W * this.Row1.Z * this.Row2.Y * this.Row3.X;
      }
    }

    public Vector4d Column0
    {
      get
      {
        return new Vector4d(this.Row0.X, this.Row1.X, this.Row2.X, this.Row3.X);
      }
    }

    public Vector4d Column1
    {
      get
      {
        return new Vector4d(this.Row0.Y, this.Row1.Y, this.Row2.Y, this.Row3.Y);
      }
    }

    public Vector4d Column2
    {
      get
      {
        return new Vector4d(this.Row0.Z, this.Row1.Z, this.Row2.Z, this.Row3.Z);
      }
    }

    public Vector4d Column3
    {
      get
      {
        return new Vector4d(this.Row0.W, this.Row1.W, this.Row2.W, this.Row3.W);
      }
    }

    public double M11
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

    public double M12
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

    public double M13
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

    public double M14
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

    public double M21
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

    public double M22
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

    public double M23
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

    public double M24
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

    public double M31
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

    public double M32
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

    public double M33
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

    public double M34
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

    public double M41
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

    public double M42
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

    public double M43
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

    public double M44
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

    static Matrix4d()
    {
    }

    public Matrix4d(Vector4d row0, Vector4d row1, Vector4d row2, Vector4d row3)
    {
      this.Row0 = row0;
      this.Row1 = row1;
      this.Row2 = row2;
      this.Row3 = row3;
    }

    public Matrix4d(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
    {
      this.Row0 = new Vector4d(m00, m01, m02, m03);
      this.Row1 = new Vector4d(m10, m11, m12, m13);
      this.Row2 = new Vector4d(m20, m21, m22, m23);
      this.Row3 = new Vector4d(m30, m31, m32, m33);
    }

    public static Matrix4d operator *(Matrix4d left, Matrix4d right)
    {
      return Matrix4d.Mult(left, right);
    }

    public static bool operator ==(Matrix4d left, Matrix4d right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Matrix4d left, Matrix4d right)
    {
      return !left.Equals(right);
    }

    public void Invert()
    {
      this = Matrix4d.Invert(this);
    }

    public void Transpose()
    {
      this = Matrix4d.Transpose(this);
    }

    public static void CreateFromAxisAngle(Vector3d axis, double angle, out Matrix4d result)
    {
      double num1 = Math.Cos(-angle);
      double num2 = Math.Sin(-angle);
      double num3 = 1.0 - num1;
      axis.Normalize();
      result = new Matrix4d(num3 * axis.X * axis.X + num1, num3 * axis.X * axis.Y - num2 * axis.Z, num3 * axis.X * axis.Z + num2 * axis.Y, 0.0, num3 * axis.X * axis.Y + num2 * axis.Z, num3 * axis.Y * axis.Y + num1, num3 * axis.Y * axis.Z - num2 * axis.X, 0.0, num3 * axis.X * axis.Z - num2 * axis.Y, num3 * axis.Y * axis.Z + num2 * axis.X, num3 * axis.Z * axis.Z + num1, 0.0, 0.0, 0.0, 0.0, 1.0);
    }

    public static Matrix4d CreateFromAxisAngle(Vector3d axis, double angle)
    {
      Matrix4d result;
      Matrix4d.CreateFromAxisAngle(axis, angle, out result);
      return result;
    }

    public static void CreateRotationX(double angle, out Matrix4d result)
    {
      double num = Math.Cos(angle);
      double z = Math.Sin(angle);
      result.Row0 = Vector4d.UnitX;
      result.Row1 = new Vector4d(0.0, num, z, 0.0);
      result.Row2 = new Vector4d(0.0, -z, num, 0.0);
      result.Row3 = Vector4d.UnitW;
    }

    public static Matrix4d CreateRotationX(double angle)
    {
      Matrix4d result;
      Matrix4d.CreateRotationX(angle, out result);
      return result;
    }

    public static void CreateRotationY(double angle, out Matrix4d result)
    {
      double num = Math.Cos(angle);
      double x = Math.Sin(angle);
      result.Row0 = new Vector4d(num, 0.0, -x, 0.0);
      result.Row1 = Vector4d.UnitY;
      result.Row2 = new Vector4d(x, 0.0, num, 0.0);
      result.Row3 = Vector4d.UnitW;
    }

    public static Matrix4d CreateRotationY(double angle)
    {
      Matrix4d result;
      Matrix4d.CreateRotationY(angle, out result);
      return result;
    }

    public static void CreateRotationZ(double angle, out Matrix4d result)
    {
      double num = Math.Cos(angle);
      double y = Math.Sin(angle);
      result.Row0 = new Vector4d(num, y, 0.0, 0.0);
      result.Row1 = new Vector4d(-y, num, 0.0, 0.0);
      result.Row2 = Vector4d.UnitZ;
      result.Row3 = Vector4d.UnitW;
    }

    public static Matrix4d CreateRotationZ(double angle)
    {
      Matrix4d result;
      Matrix4d.CreateRotationZ(angle, out result);
      return result;
    }

    public static void CreateTranslation(double x, double y, double z, out Matrix4d result)
    {
      result = Matrix4d.Identity;
      result.Row3 = new Vector4d(x, y, z, 1.0);
    }

    public static void CreateTranslation(ref Vector3d vector, out Matrix4d result)
    {
      result = Matrix4d.Identity;
      result.Row3 = new Vector4d(vector.X, vector.Y, vector.Z, 1.0);
    }

    public static Matrix4d CreateTranslation(double x, double y, double z)
    {
      Matrix4d result;
      Matrix4d.CreateTranslation(x, y, z, out result);
      return result;
    }

    public static Matrix4d CreateTranslation(Vector3d vector)
    {
      Matrix4d result;
      Matrix4d.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
      return result;
    }

    public static void CreateOrthographic(double width, double height, double zNear, double zFar, out Matrix4d result)
    {
      Matrix4d.CreateOrthographicOffCenter(-width / 2.0, width / 2.0, -height / 2.0, height / 2.0, zNear, zFar, out result);
    }

    public static Matrix4d CreateOrthographic(double width, double height, double zNear, double zFar)
    {
      Matrix4d result;
      Matrix4d.CreateOrthographicOffCenter(-width / 2.0, width / 2.0, -height / 2.0, height / 2.0, zNear, zFar, out result);
      return result;
    }

    public static void CreateOrthographicOffCenter(double left, double right, double bottom, double top, double zNear, double zFar, out Matrix4d result)
    {
      result = new Matrix4d();
      double num1 = 1.0 / (right - left);
      double num2 = 1.0 / (top - bottom);
      double num3 = 1.0 / (zFar - zNear);
      result.M11 = 2.0 * num1;
      result.M22 = 2.0 * num2;
      result.M33 = -2.0 * num3;
      result.M41 = -(right + left) * num1;
      result.M42 = -(top + bottom) * num2;
      result.M43 = -(zFar + zNear) * num3;
      result.M44 = 1.0;
    }

    public static Matrix4d CreateOrthographicOffCenter(double left, double right, double bottom, double top, double zNear, double zFar)
    {
      Matrix4d result;
      Matrix4d.CreateOrthographicOffCenter(left, right, bottom, top, zNear, zFar, out result);
      return result;
    }

    public static void CreatePerspectiveFieldOfView(double fovy, double aspect, double zNear, double zFar, out Matrix4d result)
    {
      if (fovy <= 0.0 || fovy > Math.PI)
        throw new ArgumentOutOfRangeException("fovy");
      if (aspect <= 0.0)
        throw new ArgumentOutOfRangeException("aspect");
      if (zNear <= 0.0)
        throw new ArgumentOutOfRangeException("zNear");
      if (zFar <= 0.0)
        throw new ArgumentOutOfRangeException("zFar");
      double top = zNear * Math.Tan(0.5 * fovy);
      double bottom = -top;
      Matrix4d.CreatePerspectiveOffCenter(bottom * aspect, top * aspect, bottom, top, zNear, zFar, out result);
    }

    public static Matrix4d CreatePerspectiveFieldOfView(double fovy, double aspect, double zNear, double zFar)
    {
      Matrix4d result;
      Matrix4d.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar, out result);
      return result;
    }

    public static void CreatePerspectiveOffCenter(double left, double right, double bottom, double top, double zNear, double zFar, out Matrix4d result)
    {
      if (zNear <= 0.0)
        throw new ArgumentOutOfRangeException("zNear");
      if (zFar <= 0.0)
        throw new ArgumentOutOfRangeException("zFar");
      if (zNear >= zFar)
        throw new ArgumentOutOfRangeException("zNear");
      double m00 = 2.0 * zNear / (right - left);
      double m11 = 2.0 * zNear / (top - bottom);
      double m20 = (right + left) / (right - left);
      double m21 = (top + bottom) / (top - bottom);
      double m22 = -(zFar + zNear) / (zFar - zNear);
      double m32 = -(2.0 * zFar * zNear) / (zFar - zNear);
      result = new Matrix4d(m00, 0.0, 0.0, 0.0, 0.0, m11, 0.0, 0.0, m20, m21, m22, -1.0, 0.0, 0.0, m32, 0.0);
    }

    public static Matrix4d CreatePerspectiveOffCenter(double left, double right, double bottom, double top, double zNear, double zFar)
    {
      Matrix4d result;
      Matrix4d.CreatePerspectiveOffCenter(left, right, bottom, top, zNear, zFar, out result);
      return result;
    }

    [Obsolete("Use CreateTranslation instead.")]
    public static Matrix4d Translation(Vector3d trans)
    {
      return Matrix4d.Translation(trans.X, trans.Y, trans.Z);
    }

    [Obsolete("Use CreateTranslation instead.")]
    public static Matrix4d Translation(double x, double y, double z)
    {
      Matrix4d matrix4d = Matrix4d.Identity;
      matrix4d.Row3 = new Vector4d(x, y, z, 1.0);
      return matrix4d;
    }

    public static Matrix4d Scale(double scale)
    {
      return Matrix4d.Scale(scale, scale, scale);
    }

    public static Matrix4d Scale(Vector3d scale)
    {
      return Matrix4d.Scale(scale.X, scale.Y, scale.Z);
    }

    public static Matrix4d Scale(double x, double y, double z)
    {
      Matrix4d matrix4d;
      matrix4d.Row0 = Vector4d.UnitX * x;
      matrix4d.Row1 = Vector4d.UnitY * y;
      matrix4d.Row2 = Vector4d.UnitZ * z;
      matrix4d.Row3 = Vector4d.UnitW;
      return matrix4d;
    }

    public static Matrix4d RotateX(double angle)
    {
      double num = Math.Cos(angle);
      double z = Math.Sin(angle);
      Matrix4d matrix4d;
      matrix4d.Row0 = Vector4d.UnitX;
      matrix4d.Row1 = new Vector4d(0.0, num, z, 0.0);
      matrix4d.Row2 = new Vector4d(0.0, -z, num, 0.0);
      matrix4d.Row3 = Vector4d.UnitW;
      return matrix4d;
    }

    public static Matrix4d RotateY(double angle)
    {
      double num = Math.Cos(angle);
      double x = Math.Sin(angle);
      Matrix4d matrix4d;
      matrix4d.Row0 = new Vector4d(num, 0.0, -x, 0.0);
      matrix4d.Row1 = Vector4d.UnitY;
      matrix4d.Row2 = new Vector4d(x, 0.0, num, 0.0);
      matrix4d.Row3 = Vector4d.UnitW;
      return matrix4d;
    }

    public static Matrix4d RotateZ(double angle)
    {
      double num = Math.Cos(angle);
      double y = Math.Sin(angle);
      Matrix4d matrix4d;
      matrix4d.Row0 = new Vector4d(num, y, 0.0, 0.0);
      matrix4d.Row1 = new Vector4d(-y, num, 0.0, 0.0);
      matrix4d.Row2 = Vector4d.UnitZ;
      matrix4d.Row3 = Vector4d.UnitW;
      return matrix4d;
    }

    public static Matrix4d Rotate(Vector3d axis, double angle)
    {
      double num1 = Math.Cos(-angle);
      double num2 = Math.Sin(-angle);
      double num3 = 1.0 - num1;
      axis.Normalize();
      Matrix4d matrix4d;
      matrix4d.Row0 = new Vector4d(num3 * axis.X * axis.X + num1, num3 * axis.X * axis.Y - num2 * axis.Z, num3 * axis.X * axis.Z + num2 * axis.Y, 0.0);
      matrix4d.Row1 = new Vector4d(num3 * axis.X * axis.Y + num2 * axis.Z, num3 * axis.Y * axis.Y + num1, num3 * axis.Y * axis.Z - num2 * axis.X, 0.0);
      matrix4d.Row2 = new Vector4d(num3 * axis.X * axis.Z - num2 * axis.Y, num3 * axis.Y * axis.Z + num2 * axis.X, num3 * axis.Z * axis.Z + num1, 0.0);
      matrix4d.Row3 = Vector4d.UnitW;
      return matrix4d;
    }

    public static Matrix4d Rotate(Quaterniond q)
    {
      Vector3d axis;
      double angle;
      q.ToAxisAngle(out axis, out angle);
      return Matrix4d.Rotate(axis, angle);
    }

    public static Matrix4d LookAt(Vector3d eye, Vector3d target, Vector3d up)
    {
      Vector3d vector3d1 = Vector3d.Normalize(eye - target);
      Vector3d right = Vector3d.Normalize(Vector3d.Cross(up, vector3d1));
      Vector3d vector3d2 = Vector3d.Normalize(Vector3d.Cross(vector3d1, right));
      Matrix4d matrix4d = new Matrix4d(new Vector4d(right.X, vector3d2.X, vector3d1.X, 0.0), new Vector4d(right.Y, vector3d2.Y, vector3d1.Y, 0.0), new Vector4d(right.Z, vector3d2.Z, vector3d1.Z, 0.0), Vector4d.UnitW);
      return Matrix4d.CreateTranslation(-eye) * matrix4d;
    }

    public static Matrix4d LookAt(double eyeX, double eyeY, double eyeZ, double targetX, double targetY, double targetZ, double upX, double upY, double upZ)
    {
      return Matrix4d.LookAt(new Vector3d(eyeX, eyeY, eyeZ), new Vector3d(targetX, targetY, targetZ), new Vector3d(upX, upY, upZ));
    }

    public static Matrix4d Frustum(double left, double right, double bottom, double top, double near, double far)
    {
      double num1 = 1.0 / (right - left);
      double num2 = 1.0 / (top - bottom);
      double num3 = 1.0 / (far - near);
      return new Matrix4d(new Vector4d(2.0 * near * num1, 0.0, 0.0, 0.0), new Vector4d(0.0, 2.0 * near * num2, 0.0, 0.0), new Vector4d((right + left) * num1, (top + bottom) * num2, -(far + near) * num3, -1.0), new Vector4d(0.0, 0.0, -2.0 * far * near * num3, 0.0));
    }

    public static Matrix4d Perspective(double fovy, double aspect, double near, double far)
    {
      double top = near * Math.Tan(0.5 * fovy);
      double bottom = -top;
      return Matrix4d.Frustum(bottom * aspect, top * aspect, bottom, top, near, far);
    }

    public static Matrix4d Mult(Matrix4d left, Matrix4d right)
    {
      Matrix4d result;
      Matrix4d.Mult(ref left, ref right, out result);
      return result;
    }

    public static void Mult(ref Matrix4d left, ref Matrix4d right, out Matrix4d result)
    {
      double num1 = left.Row0.X;
      double num2 = left.Row0.Y;
      double num3 = left.Row0.Z;
      double num4 = left.Row0.W;
      double num5 = left.Row1.X;
      double num6 = left.Row1.Y;
      double num7 = left.Row1.Z;
      double num8 = left.Row1.W;
      double num9 = left.Row2.X;
      double num10 = left.Row2.Y;
      double num11 = left.Row2.Z;
      double num12 = left.Row2.W;
      double num13 = left.Row3.X;
      double num14 = left.Row3.Y;
      double num15 = left.Row3.Z;
      double num16 = left.Row3.W;
      double num17 = right.Row0.X;
      double num18 = right.Row0.Y;
      double num19 = right.Row0.Z;
      double num20 = right.Row0.W;
      double num21 = right.Row1.X;
      double num22 = right.Row1.Y;
      double num23 = right.Row1.Z;
      double num24 = right.Row1.W;
      double num25 = right.Row2.X;
      double num26 = right.Row2.Y;
      double num27 = right.Row2.Z;
      double num28 = right.Row2.W;
      double num29 = right.Row3.X;
      double num30 = right.Row3.Y;
      double num31 = right.Row3.Z;
      double num32 = right.Row3.W;
      result.Row0.X = num1 * num17 + num2 * num21 + num3 * num25 + num4 * num29;
      result.Row0.Y = num1 * num18 + num2 * num22 + num3 * num26 + num4 * num30;
      result.Row0.Z = num1 * num19 + num2 * num23 + num3 * num27 + num4 * num31;
      result.Row0.W = num1 * num20 + num2 * num24 + num3 * num28 + num4 * num32;
      result.Row1.X = num5 * num17 + num6 * num21 + num7 * num25 + num8 * num29;
      result.Row1.Y = num5 * num18 + num6 * num22 + num7 * num26 + num8 * num30;
      result.Row1.Z = num5 * num19 + num6 * num23 + num7 * num27 + num8 * num31;
      result.Row1.W = num5 * num20 + num6 * num24 + num7 * num28 + num8 * num32;
      result.Row2.X = num9 * num17 + num10 * num21 + num11 * num25 + num12 * num29;
      result.Row2.Y = num9 * num18 + num10 * num22 + num11 * num26 + num12 * num30;
      result.Row2.Z = num9 * num19 + num10 * num23 + num11 * num27 + num12 * num31;
      result.Row2.W = num9 * num20 + num10 * num24 + num11 * num28 + num12 * num32;
      result.Row3.X = num13 * num17 + num14 * num21 + num15 * num25 + num16 * num29;
      result.Row3.Y = num13 * num18 + num14 * num22 + num15 * num26 + num16 * num30;
      result.Row3.Z = num13 * num19 + num14 * num23 + num15 * num27 + num16 * num31;
      result.Row3.W = num13 * num20 + num14 * num24 + num15 * num28 + num16 * num32;
    }

    public static Matrix4d Invert(Matrix4d mat)
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
      double[,] numArray4 = new double[4, 4]
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
        double num1 = 0.0;
        for (int index4 = 0; index4 < 4; ++index4)
        {
          if (numArray3[index4] != 0)
          {
            for (int index5 = 0; index5 < 4; ++index5)
            {
              if (numArray3[index5] == -1)
              {
                double num2 = Math.Abs(numArray4[index4, index5]);
                if (num2 > num1)
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
            double num2 = numArray4[index2, index4];
            numArray4[index2, index4] = numArray4[index1, index4];
            numArray4[index1, index4] = num2;
          }
        }
        numArray2[index3] = index2;
        numArray1[index3] = index1;
        double num3 = numArray4[index1, index1];
        if (num3 == 0.0)
          throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
        double num4 = 1.0 / num3;
        numArray4[index1, index1] = 1.0;
        for (int index4 = 0; index4 < 4; ++index4)
          numArray4[index1, index4] *= num4;
        for (int index4 = 0; index4 < 4; ++index4)
        {
          if (index1 != index4)
          {
            double num2 = numArray4[index4, index1];
            numArray4[index4, index1] = 0.0;
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
          double num = numArray4[index6, index4];
          numArray4[index6, index4] = numArray4[index6, index5];
          numArray4[index6, index5] = num;
        }
      }
      mat.Row0 = new Vector4d(numArray4[0, 0], numArray4[0, 1], numArray4[0, 2], numArray4[0, 3]);
      mat.Row1 = new Vector4d(numArray4[1, 0], numArray4[1, 1], numArray4[1, 2], numArray4[1, 3]);
      mat.Row2 = new Vector4d(numArray4[2, 0], numArray4[2, 1], numArray4[2, 2], numArray4[2, 3]);
      mat.Row3 = new Vector4d(numArray4[3, 0], numArray4[3, 1], numArray4[3, 2], numArray4[3, 3]);
      return mat;
    }

    public static Matrix4d Transpose(Matrix4d mat)
    {
      return new Matrix4d(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
    }

    public static void Transpose(ref Matrix4d mat, out Matrix4d result)
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
      if (!(obj is Matrix4d))
        return false;
      else
        return this.Equals((Matrix4d) obj);
    }

    public bool Equals(Matrix4d other)
    {
      if (this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2)
        return this.Row3 == other.Row3;
      else
        return false;
    }
  }
}
