// Type: SharpDX.IO.NativeFileOptions
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX.IO
{
  [Flags]
  public enum NativeFileOptions : uint
  {
    None = 0U,
    Readonly = 1U,
    Hidden = 2U,
    System = 4U,
    Directory = 16U,
    Archive = 32U,
    Device = 64U,
    Normal = 128U,
    Temporary = 256U,
    SparseFile = 512U,
    ReparsePoint = 1024U,
    Compressed = 2048U,
    Offline = 4096U,
    NotContentIndexed = 8192U,
    Encrypted = 16384U,
    Write_Through = 2147483648U,
    Overlapped = 1073741824U,
    NoBuffering = 536870912U,
    RandomAccess = 268435456U,
    SequentialScan = 134217728U,
    DeleteOnClose = 67108864U,
    BackupSemantics = 33554432U,
    PosixSemantics = 16777216U,
    OpenReparsePoint = 2097152U,
    OpenNoRecall = 1048576U,
    FirstPipeInstance = 524288U,
  }
}
