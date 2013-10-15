// Type: CommunityExpressNS.RemoteStorage
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class RemoteStorage : ICollection<File>, IEnumerable<File>, IEnumerable
  {
    private IntPtr _remoteStorage;

    internal IntPtr SteamPointer
    {
      get
      {
        return this._remoteStorage;
      }
    }

    public int AvailableSpace
    {
      get
      {
        int totalSpace;
        int availableSpace;
        RemoteStorage.SteamUnityAPI_SteamRemoteStorage_GetQuota(this._remoteStorage, out totalSpace, out availableSpace);
        return availableSpace;
      }
    }

    public int Count
    {
      get
      {
        return RemoteStorage.SteamUnityAPI_SteamRemoteStorage_GetFileCount(this._remoteStorage);
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    internal RemoteStorage()
    {
      this._remoteStorage = RemoteStorage.SteamUnityAPI_SteamRemoteStorage();
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamRemoteStorage();

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamRemoteStorage_GetFileCount(IntPtr remoteStorage);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamRemoteStorage_GetFileSize(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamRemoteStorage_GetFileNameAndSize(IntPtr remoteStorage, int iFile, out int nFileSizeInBytes);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_WriteFile(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName, IntPtr fileContents, int fileContentsLength);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_ForgetFile(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_DeleteFile(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_FileExists(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_FilePersisted(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_GetQuota(IntPtr remoteStorage, out int totalSpace, out int availableSpace);

    public void WriteFile(string fileName, string fileContents)
    {
      RemoteStorage.SteamUnityAPI_SteamRemoteStorage_WriteFile(this._remoteStorage, fileName, Marshal.StringToHGlobalAnsi(fileContents), fileContents.Length);
    }

    public void WriteFile(string fileName, byte[] fileContents)
    {
      IntPtr num = Marshal.AllocHGlobal(fileContents.Length);
      Marshal.Copy(fileContents, 0, num, fileContents.Length);
      RemoteStorage.SteamUnityAPI_SteamRemoteStorage_WriteFile(this._remoteStorage, fileName, num, fileContents.Length);
      Marshal.FreeHGlobal(num);
    }

    public File GetFile(string fileName)
    {
      return new File(fileName, this.GetFileSize(fileName));
    }

    public int GetFileSize(string fileName)
    {
      return RemoteStorage.SteamUnityAPI_SteamRemoteStorage_GetFileSize(this._remoteStorage, fileName);
    }

    public void ForgetFile(string fileName)
    {
      RemoteStorage.SteamUnityAPI_SteamRemoteStorage_ForgetFile(this._remoteStorage, fileName);
    }

    public void DeleteFile(string fileName)
    {
      RemoteStorage.SteamUnityAPI_SteamRemoteStorage_DeleteFile(this._remoteStorage, fileName);
    }

    public bool FileExists(string fileName)
    {
      return this.GetFileSize(fileName) > 0;
    }

    public bool FilePersisted(string fileName)
    {
      return RemoteStorage.SteamUnityAPI_SteamRemoteStorage_FilePersisted(this._remoteStorage, fileName);
    }

    public void Add(File item)
    {
      throw new NotSupportedException();
    }

    public void Clear()
    {
      throw new NotSupportedException();
    }

    public bool Contains(File item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(File[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(File item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<File> GetEnumerator()
    {
      return (IEnumerator<File>) new RemoteStorage.FileEnumator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    private class FileEnumator : IEnumerator<File>, IDisposable, IEnumerator
    {
      private int _index;
      private RemoteStorage _remoteStorage;

      public File Current
      {
        get
        {
          int nFileSizeInBytes;
          return new File(Marshal.PtrToStringAnsi(RemoteStorage.SteamUnityAPI_SteamRemoteStorage_GetFileNameAndSize(this._remoteStorage._remoteStorage, this._index, out nFileSizeInBytes)), nFileSizeInBytes);
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.Current;
        }
      }

      public FileEnumator(RemoteStorage remoteStorage)
      {
        this._remoteStorage = remoteStorage;
        this._index = -1;
      }

      public bool MoveNext()
      {
        ++this._index;
        return this._index < this._remoteStorage.Count;
      }

      public void Reset()
      {
        this._index = -1;
      }

      public void Dispose()
      {
      }
    }
  }
}
