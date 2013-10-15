// Type: SharpDX.DataBuffer
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Direct3D;
using System;
using System.Runtime.InteropServices;

namespace SharpDX
{
  public class DataBuffer : Component
  {
    private unsafe sbyte* _buffer;
    private GCHandle _gCHandle;
    private Blob _blob;
    private readonly bool _ownsBuffer;
    private readonly int _size;

    public unsafe IntPtr DataPointer
    {
      get
      {
        return new IntPtr((void*) this._buffer);
      }
    }

    public int Size
    {
      get
      {
        return this._size;
      }
    }

    public unsafe DataBuffer(int sizeInBytes)
    {
      this._buffer = (sbyte*) (void*) Utilities.AllocateMemory(sizeInBytes, 16);
      this._size = sizeInBytes;
      this._ownsBuffer = true;
    }

    public unsafe DataBuffer(IntPtr userBuffer, int sizeInBytes)
      : this((void*) userBuffer, sizeInBytes, false)
    {
    }

    internal unsafe DataBuffer(void* buffer, int sizeInBytes, GCHandle handle)
    {
      this._buffer = (sbyte*) buffer;
      this._size = sizeInBytes;
      this._gCHandle = handle;
      this._ownsBuffer = false;
    }

    internal unsafe DataBuffer(void* buffer, int sizeInBytes, bool makeCopy)
    {
      if (makeCopy)
      {
        this._buffer = (sbyte*) (void*) Utilities.AllocateMemory(sizeInBytes, 16);
        Utilities.CopyMemory((IntPtr) ((void*) this._buffer), (IntPtr) buffer, sizeInBytes);
      }
      else
        this._buffer = (sbyte*) buffer;
      this._size = sizeInBytes;
      this._ownsBuffer = makeCopy;
    }

    internal unsafe DataBuffer(Blob buffer)
    {
      this._buffer = (sbyte*) (void*) buffer.GetBufferPointer();
      this._size = (int) buffer.GetBufferSize();
      this._blob = buffer;
    }

    public static implicit operator DataPointer(DataBuffer from)
    {
      return new DataPointer(from.DataPointer, from.Size);
    }

    public static unsafe DataBuffer Create<T>(T[] userBuffer, int index = 0, bool pinBuffer = true) where T : struct
    {
      if (userBuffer == null)
        throw new ArgumentNullException("userBuffer");
      if (index < 0 || index > userBuffer.Length)
        throw new ArgumentException("Index is out of range [0, userBuffer.Length-1]", "index");
      int sizeInBytes = Utilities.SizeOf<T>(userBuffer);
      DataBuffer dataBuffer;
      if (pinBuffer)
      {
        GCHandle handle = GCHandle.Alloc((object) userBuffer, GCHandleType.Pinned);
        int num = index * Utilities.SizeOf<T>();
        dataBuffer = new DataBuffer((void*) (num + (IntPtr) (void*) handle.AddrOfPinnedObject()), sizeInBytes - num, handle);
      }
      else
      {
        fixed (T* objPtr = &userBuffer[0])
          dataBuffer = new DataBuffer((void*) objPtr, sizeInBytes, true);
      }
      return dataBuffer;
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
      if (this._ownsBuffer && (IntPtr) this._buffer != IntPtr.Zero)
      {
        Utilities.FreeMemory((IntPtr) ((void*) this._buffer));
        this._buffer = (sbyte*) null;
      }
      base.Dispose(disposing);
    }

    public unsafe void Clear(byte value = (byte) 0)
    {
      Utilities.ClearMemory((IntPtr) ((void*) this._buffer), value, this.Size);
    }

    public unsafe T Get<T>(int positionInBytes) where T : struct
    {
      T data = default (T);
      Utilities.Read<T>((IntPtr) ((void*) (this._buffer + positionInBytes)), ref data);
      return data;
    }

    public unsafe float GetFloat(int positionInBytes)
    {
      return *(float*) (this._buffer + positionInBytes);
    }

    public unsafe int GetInt(int positionInBytes)
    {
      return *(int*) (this._buffer + positionInBytes);
    }

    public unsafe short GetShort(int positionInBytes)
    {
      return *(short*) (this._buffer + positionInBytes);
    }

    public unsafe bool GetBoolean(int positionInBytes)
    {
      return *(int*) (this._buffer + positionInBytes) != 0;
    }

    public unsafe Vector2 GetVector2(int positionInBytes)
    {
      return *(Vector2*) (this._buffer + positionInBytes);
    }

    public unsafe Vector3 GetVector3(int positionInBytes)
    {
      return *(Vector3*) (this._buffer + positionInBytes);
    }

    public unsafe Vector4 GetVector4(int positionInBytes)
    {
      return *(Vector4*) (this._buffer + positionInBytes);
    }

    public unsafe Color3 GetColor3(int positionInBytes)
    {
      return *(Color3*) (this._buffer + positionInBytes);
    }

    public unsafe Color4 GetColor4(int positionInBytes)
    {
      return *(Color4*) (this._buffer + positionInBytes);
    }

    public unsafe Half GetHalf(int positionInBytes)
    {
      return *(Half*) (this._buffer + positionInBytes);
    }

    public unsafe Half2 GetHalf2(int positionInBytes)
    {
      return *(Half2*) (this._buffer + positionInBytes);
    }

    public unsafe Half3 GetHalf3(int positionInBytes)
    {
      return *(Half3*) (this._buffer + positionInBytes);
    }

    public unsafe Half4 GetHalf4(int positionInBytes)
    {
      return *(Half4*) (this._buffer + positionInBytes);
    }

    public unsafe Matrix GetMatrix(int positionInBytes)
    {
      return *(Matrix*) (this._buffer + positionInBytes);
    }

    public unsafe Quaternion GetQuaternion(int positionInBytes)
    {
      return *(Quaternion*) (this._buffer + positionInBytes);
    }

    public unsafe T[] GetRange<T>(int positionInBytes, int count) where T : struct
    {
      T[] data = new T[count];
      Utilities.Read<T>((IntPtr) ((void*) (this._buffer + positionInBytes)), data, 0, count);
      return data;
    }

    public unsafe void GetRange<T>(int positionInBytes, T[] buffer, int offset, int count) where T : struct
    {
      Utilities.Read<T>((IntPtr) ((void*) (this._buffer + positionInBytes)), buffer, offset, count);
    }

    public unsafe void Set<T>(int positionInBytes, ref T value) where T : struct
    {
      // ISSUE: explicit reference operation
      *(T*) (this._buffer + positionInBytes) = *(T*) @value;
    }

    public unsafe void Set<T>(int positionInBytes, T value) where T : struct
    {
      *(T*) (this._buffer + positionInBytes) = *&value;
    }

    public unsafe void Set(int positionInBytes, float value)
    {
      *(float*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, int value)
    {
      *(int*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, short value)
    {
      *(short*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, bool value)
    {
      *(int*) (this._buffer + positionInBytes) = value ? 1 : 0;
    }

    public unsafe void Set(int positionInBytes, Vector2 value)
    {
      *(Vector2*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Vector3 value)
    {
      *(Vector3*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Vector4 value)
    {
      *(Vector4*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Color3 value)
    {
      *(Color3*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Color4 value)
    {
      *(Color4*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Half value)
    {
      *(Half*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Half2 value)
    {
      *(Half2*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Half3 value)
    {
      *(Half3*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Half4 value)
    {
      *(Half4*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Matrix value)
    {
      *(Matrix*) (this._buffer + positionInBytes) = value;
    }

    public unsafe void Set(int positionInBytes, Quaternion value)
    {
      *(Quaternion*) (this._buffer + positionInBytes) = value;
    }

    public void Set<T>(int positionInBytes, T[] data) where T : struct
    {
      this.Set<T>(positionInBytes, data, 0, data.Length);
    }

    public unsafe void Set(int positionInBytes, IntPtr source, long count)
    {
      Utilities.CopyMemory((IntPtr) ((void*) (this._buffer + positionInBytes)), source, (int) count);
    }

    public unsafe void Set<T>(int positionInBytes, T[] data, int offset, int count) where T : struct
    {
      Utilities.Write<T>((IntPtr) ((void*) (this._buffer + positionInBytes)), data, offset, count);
    }
  }
}
