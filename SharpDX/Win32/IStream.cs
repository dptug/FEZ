// Type: SharpDX.Win32.IStream
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
  [Shadow(typeof (ComStreamShadow))]
  public interface IStream : IStreamBase, ICallbackable, IDisposable
  {
    long Seek(long offset, SeekOrigin origin);

    void SetSize(long newSize);

    long CopyTo(IStream streamDest, long numberOfBytesToCopy, out long bytesWritten);

    void Commit(CommitFlags commitFlags);

    void Revert();

    void LockRegion(long offset, long numberOfBytesToLock, LockType dwLockType);

    void UnlockRegion(long offset, long numberOfBytesToLock, LockType dwLockType);

    StorageStatistics GetStatistics(StorageStatisticsFlags storageStatisticsFlags);

    IStream Clone();
  }
}
