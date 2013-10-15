// Type: SharpDX.Viewport
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Viewport : IEquatable<Viewport>, IDataSerializable
  {
    public int X;
    public int Y;
    public int Width;
    public int Height;
    public float MinDepth;
    public float MaxDepth;

    public DrawingRectangle Bounds
    {
      get
      {
        return new DrawingRectangle(this.X, this.Y, this.Width, this.Height);
      }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
        this.Width = value.Width;
        this.Height = value.Height;
      }
    }

    public float AspectRatio
    {
      get
      {
        if (this.Height != 0)
          return (float) this.Width / (float) this.Height;
        else
          return 0.0f;
      }
    }

    public Viewport(int x, int y, int width, int height)
    {
      this.X = x;
      this.Y = y;
      this.Width = width;
      this.Height = height;
      this.MinDepth = 0.0f;
      this.MaxDepth = 1f;
    }

    public Viewport(int x, int y, int width, int height, float minDepth, float maxDepth)
    {
      this.X = x;
      this.Y = y;
      this.Width = width;
      this.Height = height;
      this.MinDepth = minDepth;
      this.MaxDepth = maxDepth;
    }

    public Viewport(DrawingRectangle bounds)
    {
      this.X = bounds.X;
      this.Y = bounds.Y;
      this.Width = bounds.Width;
      this.Height = bounds.Height;
      this.MinDepth = 0.0f;
      this.MaxDepth = 1f;
    }

    public static bool operator ==(Viewport left, Viewport right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Viewport left, Viewport right)
    {
      return !left.Equals(right);
    }

    public bool Equals(Viewport other)
    {
      if (this.X == other.X && this.Y == other.Y && (this.Width == other.Width && this.Height == other.Height) && MathUtil.WithinEpsilon(this.MinDepth, other.MinDepth))
        return MathUtil.WithinEpsilon(this.MaxDepth, other.MaxDepth);
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || !(obj is Viewport))
        return false;
      else
        return this.Equals((Viewport) obj);
    }

    public override int GetHashCode()
    {
      return ((((this.X * 397 ^ this.Y) * 397 ^ this.Width) * 397 ^ this.Height) * 397 ^ this.MinDepth.GetHashCode()) * 397 ^ this.MaxDepth.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{X:{0} Y:{1} Width:{2} Height:{3} MinDepth:{4} MaxDepth:{5}}}", (object) this.X, (object) this.Y, (object) this.Width, (object) this.Height, (object) this.MinDepth, (object) this.MaxDepth);
    }

    public Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
    {
      Matrix transform = Matrix.Multiply(Matrix.Multiply(world, view), projection);
      Vector3 vector3 = (Vector3) Vector3.Transform(source, transform);
      float a = (float) ((double) source.X * (double) transform.M14 + (double) source.Y * (double) transform.M24 + (double) source.Z * (double) transform.M34) + transform.M44;
      if (!MathUtil.WithinEpsilon(a, 1f))
        vector3 /= a;
      vector3.X = (float) (((double) vector3.X + 1.0) * 0.5) * (float) this.Width + (float) this.X;
      vector3.Y = (float) ((-(double) vector3.Y + 1.0) * 0.5) * (float) this.Height + (float) this.Y;
      vector3.Z = vector3.Z * (this.MaxDepth - this.MinDepth) + this.MinDepth;
      return vector3;
    }

    public Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
    {
      Matrix transform = Matrix.Invert(Matrix.Multiply(Matrix.Multiply(world, view), projection));
      source.X = (float) (((double) source.X - (double) this.X) / (double) this.Width * 2.0 - 1.0);
      source.Y = (float) -(((double) source.Y - (double) this.Y) / (double) this.Height * 2.0 - 1.0);
      source.Z = (float) (((double) source.Z - (double) this.MinDepth) / ((double) this.MaxDepth - (double) this.MinDepth));
      Vector3 vector3 = (Vector3) Vector3.Transform(source, transform);
      float a = (float) ((double) source.X * (double) transform.M14 + (double) source.Y * (double) transform.M24 + (double) source.Z * (double) transform.M34) + transform.M44;
      if (!MathUtil.WithinEpsilon(a, 1f))
        vector3 /= a;
      return vector3;
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.X);
        serializer.Writer.Write(this.Y);
        serializer.Writer.Write(this.Width);
        serializer.Writer.Write(this.Height);
        serializer.Writer.Write(this.MinDepth);
        serializer.Writer.Write(this.MaxDepth);
      }
      else
      {
        this.X = serializer.Reader.ReadInt32();
        this.Y = serializer.Reader.ReadInt32();
        this.Width = serializer.Reader.ReadInt32();
        this.Height = serializer.Reader.ReadInt32();
        this.MinDepth = serializer.Reader.ReadSingle();
        this.MaxDepth = serializer.Reader.ReadSingle();
      }
    }
  }
}
