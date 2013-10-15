// Type: SharpDX.Win32.StorageStatistics
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
  public struct StorageStatistics
  {
    public string PwcsName;
    public int Type;
    public long CbSize;
    public long Mtime;
    public long Ctime;
    public long Atime;
    public int GrfMode;
    public int GrfLocksSupported;
    public Guid Clsid;
    public int GrfStateBits;
    public int Reserved;

    internal void __MarshalFree(ref StorageStatistics.__Native @ref)
    {
      @ref.__MarshalFree();
    }

    internal void __MarshalFrom(ref StorageStatistics.__Native @ref)
    {
      this.PwcsName = @ref.PwcsName == IntPtr.Zero ? (string) null : Marshal.PtrToStringUni(@ref.PwcsName);
      this.Type = @ref.Type;
      this.CbSize = @ref.CbSize;
      this.Mtime = @ref.Mtime;
      this.Ctime = @ref.Ctime;
      this.Atime = @ref.Atime;
      this.GrfMode = @ref.GrfMode;
      this.GrfLocksSupported = @ref.GrfLocksSupported;
      this.Clsid = @ref.Clsid;
      this.GrfStateBits = @ref.GrfStateBits;
      this.Reserved = @ref.Reserved;
    }

    internal void __MarshalTo(ref StorageStatistics.__Native @ref)
    {
      @ref.PwcsName = this.PwcsName == null ? IntPtr.Zero : Utilities.StringToHGlobalUni(this.PwcsName);
      @ref.Type = this.Type;
      @ref.CbSize = this.CbSize;
      @ref.Mtime = this.Mtime;
      @ref.Ctime = this.Ctime;
      @ref.Atime = this.Atime;
      @ref.GrfMode = this.GrfMode;
      @ref.GrfLocksSupported = this.GrfLocksSupported;
      @ref.Clsid = this.Clsid;
      @ref.GrfStateBits = this.GrfStateBits;
      @ref.Reserved = this.Reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct __Native
    {
      public IntPtr PwcsName;
      public int Type;
      public long CbSize;
      public long Mtime;
      public long Ctime;
      public long Atime;
      public int GrfMode;
      public int GrfLocksSupported;
      public Guid Clsid;
      public int GrfStateBits;
      public int Reserved;

      internal void __MarshalFree()
      {
        if (!(this.PwcsName != IntPtr.Zero))
          return;
        Marshal.FreeHGlobal(this.PwcsName);
      }
    }
  }
}
