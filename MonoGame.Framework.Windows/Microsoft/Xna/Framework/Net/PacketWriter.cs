// Type: Microsoft.Xna.Framework.Net.PacketWriter
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System.IO;

namespace Microsoft.Xna.Framework.Net
{
  public class PacketWriter : BinaryWriter
  {
    internal byte[] Data
    {
      get
      {
        return ((MemoryStream) this.BaseStream).GetBuffer();
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

    public PacketWriter()
      : this(0)
    {
    }

    public PacketWriter(int capacity)
      : base((Stream) new MemoryStream(capacity))
    {
    }

    public void Write(Color Value)
    {
      base.Write(Value.PackedValue);
    }

    public override void Write(double Value)
    {
      base.Write(Value);
    }

    public void Write(Matrix Value)
    {
      base.Write(Value.M11);
      base.Write(Value.M12);
      base.Write(Value.M13);
      base.Write(Value.M14);
      base.Write(Value.M21);
      base.Write(Value.M22);
      base.Write(Value.M23);
      base.Write(Value.M24);
      base.Write(Value.M31);
      base.Write(Value.M32);
      base.Write(Value.M33);
      base.Write(Value.M34);
      base.Write(Value.M41);
      base.Write(Value.M42);
      base.Write(Value.M43);
      base.Write(Value.M44);
    }

    public void Write(Quaternion Value)
    {
      base.Write(Value.X);
      base.Write(Value.Y);
      base.Write(Value.Z);
      base.Write(Value.W);
    }

    public override void Write(float Value)
    {
      base.Write(Value);
    }

    public void Write(Vector2 Value)
    {
      base.Write(Value.X);
      base.Write(Value.Y);
    }

    public void Write(Vector3 Value)
    {
      base.Write(Value.X);
      base.Write(Value.Y);
      base.Write(Value.Z);
    }

    public void Write(Vector4 Value)
    {
      base.Write(Value.X);
      base.Write(Value.Y);
      base.Write(Value.Z);
      base.Write(Value.W);
    }

    internal void Reset()
    {
      MemoryStream memoryStream = (MemoryStream) this.BaseStream;
      memoryStream.SetLength(0L);
      memoryStream.Position = 0L;
    }
  }
}
