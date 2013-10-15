// Type: Microsoft.Xna.Framework.Net.PacketReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System.IO;

namespace Microsoft.Xna.Framework.Net
{
  public class PacketReader : BinaryReader
  {
    internal byte[] Data
    {
      get
      {
        return ((MemoryStream) this.BaseStream).GetBuffer();
      }
      set
      {
        this.BaseStream.Write(value, 0, value.Length);
      }
    }

    public int Length
    {
      get
      {
        return (int) this.BaseStream.Length;
      }
    }

    public int Position
    {
      get
      {
        return (int) this.BaseStream.Position;
      }
      set
      {
        if (this.BaseStream.Position == (long) value)
          return;
        this.BaseStream.Position = (long) value;
      }
    }

    public PacketReader()
      : this(0)
    {
    }

    public PacketReader(int capacity)
      : base((Stream) new MemoryStream(0))
    {
    }

    public Color ReadColor()
    {
      Color transparent = Color.Transparent;
      transparent.PackedValue = this.ReadUInt32();
      return transparent;
    }

    public override double ReadDouble()
    {
      return this.ReadDouble();
    }

    public Matrix ReadMatrix()
    {
      return new Matrix()
      {
        M11 = this.ReadSingle(),
        M12 = this.ReadSingle(),
        M13 = this.ReadSingle(),
        M14 = this.ReadSingle(),
        M21 = this.ReadSingle(),
        M22 = this.ReadSingle(),
        M23 = this.ReadSingle(),
        M24 = this.ReadSingle(),
        M31 = this.ReadSingle(),
        M32 = this.ReadSingle(),
        M33 = this.ReadSingle(),
        M34 = this.ReadSingle(),
        M41 = this.ReadSingle(),
        M42 = this.ReadSingle(),
        M43 = this.ReadSingle(),
        M44 = this.ReadSingle()
      };
    }

    public Quaternion ReadQuaternion()
    {
      return new Quaternion()
      {
        X = this.ReadSingle(),
        Y = this.ReadSingle(),
        Z = this.ReadSingle(),
        W = this.ReadSingle()
      };
    }

    public Vector2 ReadVector2()
    {
      return new Vector2()
      {
        X = this.ReadSingle(),
        Y = this.ReadSingle()
      };
    }

    public Vector3 ReadVector3()
    {
      return new Vector3()
      {
        X = this.ReadSingle(),
        Y = this.ReadSingle(),
        Z = this.ReadSingle()
      };
    }

    public Vector4 ReadVector4()
    {
      return new Vector4()
      {
        X = this.ReadSingle(),
        Y = this.ReadSingle(),
        Z = this.ReadSingle(),
        W = this.ReadSingle()
      };
    }

    internal void Reset(int size)
    {
      MemoryStream memoryStream = (MemoryStream) this.BaseStream;
      memoryStream.SetLength((long) size);
      memoryStream.Position = 0L;
    }
  }
}
