// Type: SharpDX.Win32.ComStream
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
  [Guid("0000000c-0000-0000-C000-000000000046")]
  public class ComStream : ComStreamBase, IStream, IStreamBase, ICallbackable, IDisposable
  {
    public ComStream(IntPtr nativePtr)
      : base(nativePtr)
    {
    }

    public static explicit operator ComStream(IntPtr nativePointer)
    {
      if (!(nativePointer == IntPtr.Zero))
        return new ComStream(nativePointer);
      else
        return (ComStream) null;
    }

    public unsafe long Seek(long dlibMove, SeekOrigin dwOrigin)
    {
      long num;
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      (Result) (__calli((__FnPtr<int (void*, long, int, void*)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(5) * sizeof (void*)))(this._nativePointer, (int) dlibMove, (long) dwOrigin, (void*) &num)).CheckError();
      return num;
    }

    public unsafe void SetSize(long libNewSize)
    {
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      (Result) (__calli((__FnPtr<int (void*, long)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(6) * sizeof (void*)))((long) this._nativePointer, (void*) libNewSize)).CheckError();
    }

    internal unsafe long CopyTo_(IntPtr stmRef, long cb, out long cbWrittenRef)
    {
      long num;
      Result result;
      fixed (long* numPtr = &cbWrittenRef)
      {
        // ISSUE: cast to a function pointer type
        // ISSUE: function pointer call
        result = (Result) (__calli((__FnPtr<int (void*, void*, long, void*, void*)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(7) * sizeof (void*)))(this._nativePointer, (void*) stmRef, cb, (void*) &num, (void*) numPtr));
      }
      result.CheckError();
      return num;
    }

    public unsafe void Commit(CommitFlags grfCommitFlags)
    {
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      (Result) (__calli((__FnPtr<int (void*, int)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(8) * sizeof (void*)))((int) this._nativePointer, (void*) grfCommitFlags)).CheckError();
    }

    public unsafe void Revert()
    {
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      (Result) (__calli((__FnPtr<int (void*)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(9) * sizeof (void*)))(this._nativePointer)).CheckError();
    }

    public unsafe void LockRegion(long libOffset, long cb, LockType dwLockType)
    {
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      (Result) (__calli((__FnPtr<int (void*, long, long, int)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(10) * sizeof (void*)))((int) this._nativePointer, libOffset, cb, (void*) dwLockType)).CheckError();
    }

    public unsafe void UnlockRegion(long libOffset, long cb, LockType dwLockType)
    {
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      (Result) (__calli((__FnPtr<int (void*, long, long, int)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(11) * sizeof (void*)))((int) this._nativePointer, libOffset, cb, (void*) dwLockType)).CheckError();
    }

    public unsafe StorageStatistics GetStatistics(StorageStatisticsFlags grfStatFlag)
    {
      StorageStatistics.__Native @ref = new StorageStatistics.__Native();
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      Result result = (Result) (__calli((__FnPtr<int (void*, void*, int)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(12) * sizeof (void*)))((int) this._nativePointer, (void*) &@ref, (void*) grfStatFlag));
      StorageStatistics storageStatistics = new StorageStatistics();
      storageStatistics.__MarshalFrom(ref @ref);
      result.CheckError();
      return storageStatistics;
    }

    public unsafe IStream Clone()
    {
      IntPtr nativePtr = IntPtr.Zero;
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      Result result = (Result) (__calli((__FnPtr<int (void*, void*)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(13) * sizeof (void*)))(this._nativePointer, (void*) &nativePtr));
      IStream stream = nativePtr == IntPtr.Zero ? (IStream) null : (IStream) new ComStream(nativePtr);
      result.CheckError();
      return stream;
    }

    public long CopyTo(IStream streamDest, long numberOfBytesToCopy, out long bytesWritten)
    {
      this.CopyTo_(ComStream.ToIntPtr(streamDest), numberOfBytesToCopy, out bytesWritten);
      return bytesWritten;
    }

    public static IntPtr ToIntPtr(IStream stream)
    {
      return ComStreamShadow.ToIntPtr(stream);
    }
  }
}
