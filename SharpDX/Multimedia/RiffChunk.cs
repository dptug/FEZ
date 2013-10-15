// Type: SharpDX.Multimedia.RiffChunk
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Globalization;
using System.IO;

namespace SharpDX.Multimedia
{
  public class RiffChunk
  {
    public Stream Stream { get; private set; }

    public FourCC Type { get; private set; }

    public uint Size { get; private set; }

    public uint DataPosition { get; private set; }

    public bool IsList { get; private set; }

    public bool IsHeader { get; private set; }

    public RiffChunk(Stream stream, FourCC type, uint size, uint dataPosition, bool isList = false, bool isHeader = false)
    {
      this.Stream = stream;
      this.Type = type;
      this.Size = size;
      this.DataPosition = dataPosition;
      this.IsList = isList;
      this.IsHeader = isHeader;
    }

    public byte[] GetData()
    {
      byte[] buffer = new byte[(IntPtr) this.Size];
      this.Stream.Position = (long) this.DataPosition;
      this.Stream.Read(buffer, 0, (int) this.Size);
      return buffer;
    }

    public unsafe T GetDataAs<T>() where T : struct
    {
      T data = default (T);
      fixed (byte* numPtr = this.GetData())
        Utilities.Read<T>((IntPtr) ((void*) numPtr), ref data);
      return data;
    }

    public unsafe T[] GetDataAsArray<T>() where T : struct
    {
      int num = Utilities.SizeOf<T>();
      if ((long) this.Size % (long) num != 0L)
        throw new ArgumentException("Size of T is incompatible with size of chunk");
      T[] data = new T[(long) this.Size / (long) num];
      fixed (byte* numPtr = this.GetData())
        Utilities.Read<T>((IntPtr) ((void*) numPtr), data, 0, data.Length);
      return data;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Type: {0}, Size: {1}, Position: {2}, IsList: {3}, IsHeader: {4}", (object) this.Type, (object) this.Size, (object) this.DataPosition, (object) (bool) (this.IsList ? 1 : 0), (object) (bool) (this.IsHeader ? 1 : 0));
    }
  }
}
