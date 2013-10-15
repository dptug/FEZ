// Type: SharpDX.Win32.ComStreamShadow
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
  internal class ComStreamShadow : ComStreamBaseShadow
  {
    private static readonly ComStreamShadow.ComStreamVtbl Vtbl = new ComStreamShadow.ComStreamVtbl();

    protected override CppObjectVtbl GetVtbl
    {
      get
      {
        return (CppObjectVtbl) ComStreamShadow.Vtbl;
      }
    }

    static ComStreamShadow()
    {
    }

    public static IntPtr ToIntPtr(IStream stream)
    {
      return CppObjectShadow.ToIntPtr<IStream>((ICallbackable) stream);
    }

    private class ComStreamVtbl : ComStreamBaseShadow.ComStreamBaseVtbl
    {
      public ComStreamVtbl()
        : base(9)
      {
        this.AddMethod((Delegate) new ComStreamShadow.ComStreamVtbl.SeekDelegate(ComStreamShadow.ComStreamVtbl.SeekImpl));
        this.AddMethod((Delegate) new ComStreamShadow.ComStreamVtbl.SetSizeDelegate(ComStreamShadow.ComStreamVtbl.SetSizeImpl));
        this.AddMethod((Delegate) new ComStreamShadow.ComStreamVtbl.CopyToDelegate(ComStreamShadow.ComStreamVtbl.CopyToImpl));
        this.AddMethod((Delegate) new ComStreamShadow.ComStreamVtbl.CommitDelegate(ComStreamShadow.ComStreamVtbl.CommitImpl));
        this.AddMethod((Delegate) new ComStreamShadow.ComStreamVtbl.RevertDelegate(ComStreamShadow.ComStreamVtbl.RevertImpl));
        this.AddMethod((Delegate) new ComStreamShadow.ComStreamVtbl.LockRegionDelegate(ComStreamShadow.ComStreamVtbl.LockRegionImpl));
        this.AddMethod((Delegate) new ComStreamShadow.ComStreamVtbl.UnlockRegionDelegate(ComStreamShadow.ComStreamVtbl.UnlockRegionImpl));
        this.AddMethod((Delegate) new ComStreamShadow.ComStreamVtbl.StatDelegate(ComStreamShadow.ComStreamVtbl.StatImpl));
        this.AddMethod((Delegate) new ComStreamShadow.ComStreamVtbl.CloneDelegate(ComStreamShadow.ComStreamVtbl.CloneImpl));
      }

      private static unsafe int SeekImpl(IntPtr thisPtr, long offset, SeekOrigin origin, IntPtr newPosition)
      {
        try
        {
          long num = ((IStream) CppObjectShadow.ToShadow<ComStreamShadow>(thisPtr).Callback).Seek(offset, origin);
          if (newPosition != IntPtr.Zero)
            *(long*) (void*) newPosition = num;
        }
        catch (Exception ex)
        {
          return (int) Result.GetResultFromException(ex);
        }
        return Result.Ok.Code;
      }

      private static Result SetSizeImpl(IntPtr thisPtr, long newSize)
      {
        Result result = Result.Ok;
        try
        {
          ((IStream) CppObjectShadow.ToShadow<ComStreamShadow>(thisPtr).Callback).SetSize(newSize);
        }
        catch (SharpDXException ex)
        {
          result = ex.ResultCode;
        }
        catch (Exception ex)
        {
          result = (Result) Result.Fail.Code;
        }
        return result;
      }

      private static int CopyToImpl(IntPtr thisPtr, IntPtr streamPointer, long numberOfBytes, out long numberOfBytesRead, out long numberOfBytesWritten)
      {
        numberOfBytesRead = 0L;
        numberOfBytesWritten = 0L;
        try
        {
          IStream stream = (IStream) CppObjectShadow.ToShadow<ComStreamShadow>(thisPtr).Callback;
          numberOfBytesRead = stream.CopyTo((IStream) new ComStream(streamPointer), numberOfBytes, out numberOfBytesWritten);
        }
        catch (Exception ex)
        {
          return (int) Result.GetResultFromException(ex);
        }
        return Result.Ok.Code;
      }

      private static Result CommitImpl(IntPtr thisPtr, CommitFlags flags)
      {
        Result result = Result.Ok;
        try
        {
          ((IStream) CppObjectShadow.ToShadow<ComStreamShadow>(thisPtr).Callback).Commit(flags);
        }
        catch (SharpDXException ex)
        {
          result = ex.ResultCode;
        }
        catch (Exception ex)
        {
          result = (Result) Result.Fail.Code;
        }
        return result;
      }

      private static Result RevertImpl(IntPtr thisPtr)
      {
        Result result = Result.Ok;
        try
        {
          ((IStream) CppObjectShadow.ToShadow<ComStreamShadow>(thisPtr).Callback).Revert();
        }
        catch (SharpDXException ex)
        {
          result = ex.ResultCode;
        }
        catch (Exception ex)
        {
          result = (Result) Result.Fail.Code;
        }
        return result;
      }

      private static Result LockRegionImpl(IntPtr thisPtr, long offset, long numberOfBytes, LockType lockType)
      {
        Result result = Result.Ok;
        try
        {
          ((IStream) CppObjectShadow.ToShadow<ComStreamShadow>(thisPtr).Callback).LockRegion(offset, numberOfBytes, lockType);
        }
        catch (SharpDXException ex)
        {
          result = ex.ResultCode;
        }
        catch (Exception ex)
        {
          result = (Result) Result.Fail.Code;
        }
        return result;
      }

      private static Result UnlockRegionImpl(IntPtr thisPtr, long offset, long numberOfBytes, LockType lockType)
      {
        Result result = Result.Ok;
        try
        {
          ((IStream) CppObjectShadow.ToShadow<ComStreamShadow>(thisPtr).Callback).UnlockRegion(offset, numberOfBytes, lockType);
        }
        catch (SharpDXException ex)
        {
          result = ex.ResultCode;
        }
        catch (Exception ex)
        {
          result = (Result) Result.Fail.Code;
        }
        return result;
      }

      private static Result StatImpl(IntPtr thisPtr, ref StorageStatistics.__Native statisticsPtr, StorageStatisticsFlags flags)
      {
        try
        {
          ((IStream) CppObjectShadow.ToShadow<ComStreamShadow>(thisPtr).Callback).GetStatistics(flags).__MarshalTo(ref statisticsPtr);
        }
        catch (SharpDXException ex)
        {
          return ex.ResultCode;
        }
        catch (Exception ex)
        {
          return (Result) Result.Fail.Code;
        }
        return Result.Ok;
      }

      private static Result CloneImpl(IntPtr thisPtr, out IntPtr streamPointer)
      {
        streamPointer = IntPtr.Zero;
        Result result = Result.Ok;
        try
        {
          IStream stream = ((IStream) CppObjectShadow.ToShadow<ComStreamShadow>(thisPtr).Callback).Clone();
          streamPointer = ComStream.ToIntPtr(stream);
        }
        catch (SharpDXException ex)
        {
          result = ex.ResultCode;
        }
        catch (Exception ex)
        {
          result = (Result) Result.Fail.Code;
        }
        return result;
      }

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate int SeekDelegate(IntPtr thisPtr, long offset, SeekOrigin origin, IntPtr newPosition);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate Result SetSizeDelegate(IntPtr thisPtr, long newSize);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate int CopyToDelegate(IntPtr thisPtr, IntPtr streamPointer, long numberOfBytes, out long numberOfBytesRead, out long numberOfBytesWritten);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate Result CommitDelegate(IntPtr thisPtr, CommitFlags flags);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate Result RevertDelegate(IntPtr thisPtr);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate Result LockRegionDelegate(IntPtr thisPtr, long offset, long numberOfBytes, LockType lockType);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate Result UnlockRegionDelegate(IntPtr thisPtr, long offset, long numberOfBytes, LockType lockType);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate Result StatDelegate(IntPtr thisPtr, ref StorageStatistics.__Native statisticsPtr, StorageStatisticsFlags flags);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate Result CloneDelegate(IntPtr thisPtr, out IntPtr streamPointer);
    }
  }
}
