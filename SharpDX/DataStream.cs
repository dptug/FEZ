// Type: SharpDX.DataStream
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Direct3D;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX
{
  public class DataStream : Stream
  {
    private unsafe sbyte* _buffer;
    private readonly bool _canRead;
    private readonly bool _canWrite;
    private GCHandle _gCHandle;
    private Blob _blob;
    private readonly bool _ownsBuffer;
    private long _position;
    private readonly long _size;

    public override bool CanRead
    {
      get
      {
        return this._canRead;
      }
    }

    public override bool CanSeek
    {
      get
      {
        return true;
      }
    }

    public override bool CanWrite
    {
      get
      {
        return this._canWrite;
      }
    }

    public unsafe IntPtr DataPointer
    {
      get
      {
        return new IntPtr((void*) this._buffer);
      }
    }

    public override long Length
    {
      get
      {
        return this._size;
      }
    }

    public override long Position
    {
      get
      {
        return this._position;
      }
      set
      {
        this.Seek(value, SeekOrigin.Begin);
      }
    }

    public unsafe IntPtr PositionPointer
    {
      get
      {
        return (IntPtr) ((void*) (this._buffer + this._position));
      }
    }

    public long RemainingLength
    {
      get
      {
        return this._size - this._position;
      }
    }

    public unsafe DataStream(Blob buffer)
    {
      this._buffer = (sbyte*) (void*) buffer.GetBufferPointer();
      this._size = (long) buffer.GetBufferSize();
      this._canRead = true;
      this._canWrite = true;
      this._blob = buffer;
    }

    public unsafe DataStream(int sizeInBytes, bool canRead, bool canWrite)
    {
      this._buffer = (sbyte*) (void*) Utilities.AllocateMemory(sizeInBytes, 16);
      this._size = (long) sizeInBytes;
      this._ownsBuffer = true;
      this._canRead = canRead;
      this._canWrite = canWrite;
    }

    public unsafe DataStream(IntPtr userBuffer, long sizeInBytes, bool canRead, bool canWrite)
    {
      this._buffer = (sbyte*) userBuffer.ToPointer();
      this._size = sizeInBytes;
      this._canRead = canRead;
      this._canWrite = canWrite;
    }

    internal unsafe DataStream(void* dataPointer, int sizeInBytes, bool canRead, bool canWrite, GCHandle handle)
    {
      this._gCHandle = handle;
      this._buffer = (sbyte*) dataPointer;
      this._size = (long) sizeInBytes;
      this._canRead = canRead;
      this._canWrite = canWrite;
      this._ownsBuffer = false;
    }

    internal unsafe DataStream(void* buffer, int sizeInBytes, bool canRead, bool canWrite, bool makeCopy)
    {
      if (makeCopy)
      {
        this._buffer = (sbyte*) (void*) Utilities.AllocateMemory(sizeInBytes, 16);
        Utilities.CopyMemory((IntPtr) ((void*) this._buffer), (IntPtr) buffer, sizeInBytes);
      }
      else
        this._buffer = (sbyte*) buffer;
      this._size = (long) sizeInBytes;
      this._canRead = canRead;
      this._canWrite = canWrite;
      this._ownsBuffer = makeCopy;
    }

    public static implicit operator DataPointer(DataStream from)
    {
      return new DataPointer(from.PositionPointer, (int) from.RemainingLength);
    }

    public static unsafe DataStream Create<T>(T[] userBuffer, bool canRead, bool canWrite, int index = 0, bool pinBuffer = true) where T : struct
    {
      if (userBuffer == null)
        throw new ArgumentNullException("userBuffer");
      if (index < 0 || index > userBuffer.Length)
        throw new ArgumentException("Index is out of range [0, userBuffer.Length-1]", "index");
      int sizeInBytes = Utilities.SizeOf<T>(userBuffer);
      DataStream dataStream;
      if (pinBuffer)
      {
        GCHandle handle = GCHandle.Alloc((object) userBuffer, GCHandleType.Pinned);
        int num = index * Utilities.SizeOf<T>();
        dataStream = new DataStream((void*) (num + (IntPtr) (void*) handle.AddrOfPinnedObject()), sizeInBytes - num, canRead, canWrite, handle);
      }
      else
      {
        fixed (T* objPtr = &userBuffer[0])
          dataStream = new DataStream((void*) objPtr, sizeInBytes, canRead, canWrite, true);
      }
      return dataStream;
    }

    protected override unsafe void Dispose(bool disposing)
    {
      if (disposing && this._blob != null)
      {
        this._blob.Dispose();
        this._blob = (Blob) null;
      }
      if (this._gCHandle.IsAllocated)
        this._gCHandle.Free();
      if (!this._ownsBuffer || (IntPtr) this._buffer == IntPtr.Zero)
        return;
      Utilities.FreeMemory((IntPtr) ((void*) this._buffer));
      this._buffer = (sbyte*) null;
    }

    public override void Flush()
    {
      throw new NotSupportedException("DataStream objects cannot be flushed.");
    }

    public unsafe T Read<T>() where T : struct
    {
      if (!this._canRead)
        throw new NotSupportedException();
      sbyte* numPtr = this._buffer + this._position;
      T data = default (T);
      this._position = (sbyte*) (void*) Utilities.ReadAndPosition<T>((IntPtr) ((void*) numPtr), ref data) - this._buffer;
      return data;
    }

    public unsafe float ReadFloat()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      float num = *(float*) (this._buffer + this._position);
      this._position += 4L;
      return num;
    }

    public unsafe int ReadInt()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      int num = *(int*) (this._buffer + this._position);
      this._position += 4L;
      return num;
    }

    public unsafe short ReadShort()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      short num = *(short*) (this._buffer + this._position);
      this._position += 2L;
      return num;
    }

    public unsafe bool ReadBoolean()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      bool flag = *(int*) (this._buffer + this._position) != 0;
      this._position += 4L;
      return flag;
    }

    public unsafe Vector2 ReadVector2()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Vector2 vector2 = *(Vector2*) (this._buffer + this._position);
      this._position += 8L;
      return vector2;
    }

    public unsafe Vector3 ReadVector3()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Vector3 vector3 = *(Vector3*) (this._buffer + this._position);
      this._position += 12L;
      return vector3;
    }

    public unsafe Vector4 ReadVector4()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Vector4 vector4 = *(Vector4*) (this._buffer + this._position);
      this._position += 16L;
      return vector4;
    }

    public unsafe Color3 ReadColor3()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Color3 color3 = *(Color3*) (this._buffer + this._position);
      this._position += 12L;
      return color3;
    }

    public unsafe Color4 ReadColor4()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Color4 color4 = *(Color4*) (this._buffer + this._position);
      this._position += 16L;
      return color4;
    }

    public unsafe Half ReadHalf()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Half half = *(Half*) (this._buffer + this._position);
      this._position += 2L;
      return half;
    }

    public unsafe Half2 ReadHalf2()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Half2 half2 = *(Half2*) (this._buffer + this._position);
      this._position += 4L;
      return half2;
    }

    public unsafe Half3 ReadHalf3()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Half3 half3 = *(Half3*) (this._buffer + this._position);
      this._position += 6L;
      return half3;
    }

    public unsafe Half4 ReadHalf4()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Half4 half4 = *(Half4*) (this._buffer + this._position);
      this._position += 8L;
      return half4;
    }

    public unsafe Matrix ReadMatrix()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Matrix matrix = *(Matrix*) (this._buffer + this._position);
      this._position += 64L;
      return matrix;
    }

    public unsafe Quaternion ReadQuaternion()
    {
      if (!this._canRead)
        throw new NotSupportedException();
      Quaternion quaternion = *(Quaternion*) (this._buffer + this._position);
      this._position += 16L;
      return quaternion;
    }

    public override unsafe int ReadByte()
    {
      if (this._position >= this.Length)
        return -1;
      else
        return (int) this._buffer[((IntPtr) this._position++).ToInt64()];
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int count1 = (int) Math.Min(this.RemainingLength, (long) count);
      return this.ReadRange<byte>(buffer, offset, count1);
    }

    public unsafe void Read(IntPtr buffer, int offset, int count)
    {
      Utilities.CopyMemory(new IntPtr((void*) ((IntPtr) (void*) buffer + offset)), (IntPtr) ((void*) (this._buffer + this._position)), count);
      this._position += (long) count;
    }

    public unsafe T[] ReadRange<T>(int count) where T : struct
    {
      if (!this._canRead)
        throw new NotSupportedException();
      sbyte* numPtr = this._buffer + this._position;
      T[] data = new T[count];
      this._position = (sbyte*) (void*) Utilities.Read<T>((IntPtr) ((void*) numPtr), data, 0, count) - this._buffer;
      return data;
    }

    public unsafe int ReadRange<T>(T[] buffer, int offset, int count) where T : struct
    {
      if (!this._canRead)
        throw new NotSupportedException();
      long num = this._position;
      this._position = (sbyte*) (void*) Utilities.Read<T>((IntPtr) ((void*) (this._buffer + this._position)), buffer, offset, count) - this._buffer;
      return (int) (this._position - num);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      long num = 0L;
      switch (origin)
      {
        case SeekOrigin.Begin:
          num = offset;
          break;
        case SeekOrigin.Current:
          num = this._position + offset;
          break;
        case SeekOrigin.End:
          num = this._size - offset;
          break;
      }
      if (num < 0L)
        throw new InvalidOperationException("Cannot seek beyond the beginning of the stream.");
      if (num > this._size)
        throw new InvalidOperationException("Cannot seek beyond the end of the stream.");
      this._position = num;
      return this._position;
    }

    public override void SetLength(long value)
    {
      throw new NotSupportedException("DataStream objects cannot be resized.");
    }

    public unsafe void Write<T>(T value) where T : struct
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      this._position = (sbyte*) (void*) Utilities.WriteAndPosition<T>((IntPtr) ((void*) (this._buffer + this._position)), ref value) - this._buffer;
    }

    public unsafe void Write(float value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(float*) (this._buffer + this._position) = value;
      this._position += 4L;
    }

    public unsafe void Write(int value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(int*) (this._buffer + this._position) = value;
      this._position += 4L;
    }

    public unsafe void Write(short value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(short*) (this._buffer + this._position) = value;
      this._position += 2L;
    }

    public unsafe void Write(bool value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(int*) (this._buffer + this._position) = value ? 1 : 0;
      this._position += 4L;
    }

    public unsafe void Write(Vector2 value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Vector2*) (this._buffer + this._position) = value;
      this._position += 8L;
    }

    public unsafe void Write(Vector3 value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Vector3*) (this._buffer + this._position) = value;
      this._position += 12L;
    }

    public unsafe void Write(Vector4 value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Vector4*) (this._buffer + this._position) = value;
      this._position += 16L;
    }

    public unsafe void Write(Color3 value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Color3*) (this._buffer + this._position) = value;
      this._position += 12L;
    }

    public unsafe void Write(Color4 value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Color4*) (this._buffer + this._position) = value;
      this._position += 16L;
    }

    public unsafe void Write(Half value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Half*) (this._buffer + this._position) = value;
      this._position += 2L;
    }

    public unsafe void Write(Half2 value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Half2*) (this._buffer + this._position) = value;
      this._position += 4L;
    }

    public unsafe void Write(Half3 value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Half3*) (this._buffer + this._position) = value;
      this._position += 6L;
    }

    public unsafe void Write(Half4 value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Half4*) (this._buffer + this._position) = value;
      this._position += 8L;
    }

    public unsafe void Write(Matrix value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Matrix*) (this._buffer + this._position) = value;
      this._position += 64L;
    }

    public unsafe void Write(Quaternion value)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      *(Quaternion*) (this._buffer + this._position) = value;
      this._position += 16L;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.WriteRange<byte>(buffer, offset, count);
    }

    public unsafe void Write(IntPtr buffer, int offset, int count)
    {
      Utilities.CopyMemory((IntPtr) ((void*) (this._buffer + this._position)), new IntPtr((void*) ((IntPtr) (void*) buffer + offset)), count);
      this._position += (long) count;
    }

    public void WriteRange<T>(T[] data) where T : struct
    {
      this.WriteRange<T>(data, 0, data.Length);
    }

    public unsafe void WriteRange(IntPtr source, long count)
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      Utilities.CopyMemory((IntPtr) ((void*) (this._buffer + this._position)), source, (int) count);
      this._position += count;
    }

    public unsafe void WriteRange<T>(T[] data, int offset, int count) where T : struct
    {
      if (!this._canWrite)
        throw new NotSupportedException();
      this._position = (sbyte*) (void*) Utilities.Write<T>((IntPtr) ((void*) (this._buffer + this._position)), data, offset, count) - this._buffer;
    }
  }
}
