// Type: SharpDX.IO.NativeFileStream
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.IO
{
  public class NativeFileStream : Stream
  {
    private bool canRead;
    private bool canWrite;
    private bool canSeek;
    private IntPtr handle;
    private long position;

    public override bool CanRead
    {
      get
      {
        return this.canRead;
      }
    }

    public override bool CanSeek
    {
      get
      {
        return this.canSeek;
      }
    }

    public override bool CanWrite
    {
      get
      {
        return this.canWrite;
      }
    }

    public override long Length
    {
      get
      {
        long fileSize;
        if (!NativeFile.GetFileSizeEx(this.handle, out fileSize))
          throw new IOException("Unable to get file length", NativeFileStream.MarshalGetLastWin32Error());
        else
          return fileSize;
      }
    }

    public override long Position
    {
      get
      {
        return this.position;
      }
      set
      {
        this.Seek(value, SeekOrigin.Begin);
        this.position = value;
      }
    }

    public NativeFileStream(string fileName, NativeFileMode fileMode, NativeFileAccess access, NativeFileShare share = NativeFileShare.Read)
    {
      this.handle = NativeFile.Create(fileName, access, share, IntPtr.Zero, fileMode, NativeFileOptions.None, IntPtr.Zero);
      if (this.handle == new IntPtr(-1))
      {
        int lastWin32Error = NativeFileStream.MarshalGetLastWin32Error();
        if (lastWin32Error == 2)
          throw new FileNotFoundException("Unable to find file", fileName);
        Result resultFromWin32Error = Result.GetResultFromWin32Error(lastWin32Error);
        throw new IOException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Unable to open file {0}", new object[1]
        {
          (object) fileName
        }), resultFromWin32Error.Code);
      }
      else
      {
        this.canRead = (NativeFileAccess) 0 != (access & NativeFileAccess.Read);
        this.canWrite = (NativeFileAccess) 0 != (access & NativeFileAccess.Write);
        this.canSeek = true;
      }
    }

    private static int MarshalGetLastWin32Error()
    {
      return Marshal.GetLastWin32Error();
    }

    public override void Flush()
    {
      if (!NativeFile.FlushFileBuffers(this.handle))
        throw new IOException("Unable to flush stream", NativeFileStream.MarshalGetLastWin32Error());
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      long distanceToMoveHigh;
      if (!NativeFile.SetFilePointerEx(this.handle, offset, out distanceToMoveHigh, origin))
        throw new IOException("Unable to seek to this position", NativeFileStream.MarshalGetLastWin32Error());
      this.position = distanceToMoveHigh;
      return this.position;
    }

    public override void SetLength(long value)
    {
      long distanceToMoveHigh;
      if (!NativeFile.SetFilePointerEx(this.handle, value, out distanceToMoveHigh, SeekOrigin.Begin))
        throw new IOException("Unable to seek to this position", NativeFileStream.MarshalGetLastWin32Error());
      if (!NativeFile.SetEndOfFile(this.handle))
        throw new IOException("Unable to set the new length", NativeFileStream.MarshalGetLastWin32Error());
      if (this.position < value)
        this.Seek(this.position, SeekOrigin.Begin);
      else
        this.Seek(0L, SeekOrigin.End);
    }

    public override unsafe int Read(byte[] buffer, int offset, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      fixed (byte* numPtr = buffer)
        return this.Read((IntPtr) ((void*) numPtr), offset, count);
    }

    public unsafe int Read(IntPtr buffer, int offset, int count)
    {
      if (buffer == IntPtr.Zero)
        throw new ArgumentNullException("buffer");
      int numberOfBytesRead;
      if (!NativeFile.ReadFile(this.handle, (IntPtr) ((void*) ((IntPtr) (void*) buffer + offset)), count, out numberOfBytesRead, IntPtr.Zero))
        throw new IOException("Unable to read from file", NativeFileStream.MarshalGetLastWin32Error());
      this.position += (long) numberOfBytesRead;
      return numberOfBytesRead;
    }

    public override unsafe void Write(byte[] buffer, int offset, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      fixed (byte* numPtr = buffer)
        this.Write((IntPtr) ((void*) numPtr), offset, count);
    }

    public unsafe void Write(IntPtr buffer, int offset, int count)
    {
      if (buffer == IntPtr.Zero)
        throw new ArgumentNullException("buffer");
      int numberOfBytesRead;
      if (!NativeFile.WriteFile(this.handle, (IntPtr) ((void*) ((IntPtr) (void*) buffer + offset)), count, out numberOfBytesRead, IntPtr.Zero))
        throw new IOException("Unable to write to file", NativeFileStream.MarshalGetLastWin32Error());
      this.position += (long) numberOfBytesRead;
    }

    protected override void Dispose(bool disposing)
    {
      Utilities.CloseHandle(this.handle);
      this.handle = IntPtr.Zero;
      base.Dispose(disposing);
    }
  }
}
