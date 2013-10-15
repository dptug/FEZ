// Type: Microsoft.Xna.Framework.Graphics.Viewport
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  [Serializable]
  public struct Viewport
  {
    private int x;
    private int y;
    private int width;
    private int height;
    private float minDepth;
    private float maxDepth;

    public int Height
    {
      get
      {
        return this.height;
      }
      set
      {
        this.height = value;
      }
    }

    public float MaxDepth
    {
      get
      {
        return this.maxDepth;
      }
      set
      {
        this.maxDepth = value;
      }
    }

    public float MinDepth
    {
      get
      {
        return this.minDepth;
      }
      set
      {
        this.minDepth = value;
      }
    }

    public int Width
    {
      get
      {
        return this.width;
      }
      set
      {
        this.width = value;
      }
    }

    public int Y
    {
      get
      {
        return this.y;
      }
      set
      {
        this.y = value;
      }
    }

    public int X
    {
      get
      {
        return this.x;
      }
      set
      {
        this.x = value;
      }
    }

    public float AspectRatio
    {
      get
      {
        if (this.height != 0 && this.width != 0)
          return (float) this.width / (float) this.height;
        else
          return 0.0f;
      }
    }

    public Rectangle Bounds
    {
      get
      {
        Rectangle rectangle;
        rectangle.X = this.x;
        rectangle.Y = this.y;
        rectangle.Width = this.width;
        rectangle.Height = this.height;
        return rectangle;
      }
      set
      {
        this.x = value.X;
        this.y = value.Y;
        this.width = value.Width;
        this.height = value.Height;
      }
    }

    public Rectangle TitleSafeArea
    {
      get
      {
        return new Rectangle(this.x, this.y, this.width, this.height);
      }
    }

    public Viewport(int x, int y, int width, int height)
    {
      this.x = x;
      this.y = y;
      this.width = width;
      this.height = height;
      this.minDepth = 0.0f;
      this.maxDepth = 1f;
    }

    public Viewport(Rectangle bounds)
    {
      this = new Viewport(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }

    public Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
    {
      Matrix matrix = Matrix.Multiply(Matrix.Multiply(world, view), projection);
      Vector3 vector3 = Vector3.Transform(source, matrix);
      float a = (float) ((double) source.X * (double) matrix.M14 + (double) source.Y * (double) matrix.M24 + (double) source.Z * (double) matrix.M34) + matrix.M44;
      if (!Viewport.WithinEpsilon(a, 1f))
        vector3 /= a;
      vector3.X = (float) (((double) vector3.X + 1.0) * 0.5) * (float) this.Width + (float) this.X;
      vector3.Y = (float) ((-(double) vector3.Y + 1.0) * 0.5) * (float) this.Height + (float) this.Y;
      vector3.Z = vector3.Z * (this.MaxDepth - this.MinDepth) + this.MinDepth;
      return vector3;
    }

    public Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
    {
      Matrix matrix = Matrix.Invert(Matrix.Multiply(Matrix.Multiply(world, view), projection));
      source.X = (float) (((double) source.X - (double) this.X) / (double) this.Width * 2.0 - 1.0);
      source.Y = (float) -(((double) source.Y - (double) this.Y) / (double) this.Height * 2.0 - 1.0);
      source.Z = (float) (((double) source.Z - (double) this.MinDepth) / ((double) this.MaxDepth - (double) this.MinDepth));
      Vector3 vector3 = Vector3.Transform(source, matrix);
      float a = (float) ((double) source.X * (double) matrix.M14 + (double) source.Y * (double) matrix.M24 + (double) source.Z * (double) matrix.M34) + matrix.M44;
      if (!Viewport.WithinEpsilon(a, 1f))
        vector3 /= a;
      return vector3;
    }

    private static bool WithinEpsilon(float a, float b)
    {
      float num = a - b;
      if (-1.40129846432482E-45 <= (double) num)
        return (double) num <= 1.40129846432482E-45;
      else
        return false;
    }

    public override string ToString()
    {
      return string.Format("[Viewport: X={0} Y={1} Width={2} Height={3}]", (object) this.X, (object) this.Y, (object) this.Width, (object) this.Height);
    }
  }
}
