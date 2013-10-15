// Type: SharpDX.Win32.ComStreamProxy
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
  internal class ComStreamProxy : CallbackBase, IStream, IStreamBase, ICallbackable, IDisposable
  {
    private byte[] tempBuffer = new byte[4096];
    private Stream sourceStream;

    public ComStreamProxy(Stream sourceStream)
    {
      this.sourceStream = sourceStream;
    }

    public unsafe int Read(IntPtr buffer, int numberOfBytesToRead)
    {
      int num = 0;
      while (numberOfBytesToRead > 0)
      {
        int count = this.sourceStream.Read(this.tempBuffer, 0, Math.Min(numberOfBytesToRead, this.tempBuffer.Length));
        if (count == 0)
          return num;
        Utilities.Write<byte>(new IntPtr((void*) (num + (IntPtr) (void*) buffer)), this.tempBuffer, 0, count);
        numberOfBytesToRead -= count;
        num += count;
      }
      return num;
    }

    public unsafe int Write(IntPtr buffer, int numberOfBytesToWrite)
    {
      int num = 0;
      while (numberOfBytesToWrite > 0)
      {
        int count = Math.Min(numberOfBytesToWrite, this.tempBuffer.Length);
        Utilities.Read<byte>(new IntPtr((void*) (num + (IntPtr) (void*) buffer)), this.tempBuffer, 0, count);
        this.sourceStream.Write(this.tempBuffer, 0, count);
        numberOfBytesToWrite -= count;
        num += count;
      }
      return num;
    }

    public long Seek(long offset, SeekOrigin origin)
    {
      return this.sourceStream.Seek(offset, origin);
    }

    public void SetSize(long newSize)
    {
    }

    public unsafe long CopyTo(IStream streamDest, long numberOfBytesToCopy, out long bytesWritten)
    {
      bytesWritten = 0L;
      fixed (byte* numPtr = this.tempBuffer)
      {
        while (numberOfBytesToCopy > 0L)
        {
          int numberOfBytesToRead = this.sourceStream.Read(this.tempBuffer, 0, (int) Math.Min(numberOfBytesToCopy, (long) this.tempBuffer.Length));
          if (numberOfBytesToRead != 0)
          {
            streamDest.Write((IntPtr) ((void*) numPtr), numberOfBytesToRead);
            numberOfBytesToCopy -= (long) numberOfBytesToRead;
            bytesWritten += (long) numberOfBytesToRead;
          }
          else
            break;
        }
      }
      return bytesWritten;
    }

    public void Commit(CommitFlags commitFlags)
    {
      this.sourceStream.Flush();
    }

    public void Revert()
    {
      throw new NotImplementedException();
    }

    public void LockRegion(long offset, long numberOfBytesToLock, LockType dwLockType)
    {
      throw new NotImplementedException();
    }

    public void UnlockRegion(long offset, long numberOfBytesToLock, LockType dwLockType)
    {
      throw new NotImplementedException();
    }

    public StorageStatistics GetStatistics(StorageStatisticsFlags storageStatisticsFlags)
    {
      long num = this.sourceStream.Length;
      if (num == 0L)
        num = (long) int.MaxValue;
      return new StorageStatistics()
      {
        Type = 2,
        CbSize = num,
        GrfLocksSupported = 2,
        GrfMode = 2
      };
    }

    public IStream Clone()
    {
      return (IStream) new ComStreamProxy(this.sourceStream);
    }

    protected override void Dispose(bool disposing)
    {
      this.sourceStream = (Stream) null;
      base.Dispose(disposing);
    }
  }
}
