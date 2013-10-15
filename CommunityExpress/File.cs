// Type: CommunityExpressNS.File
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CommunityExpressNS
{
  public class File
  {
    private string _fileName;
    private int _fileSize;

    public string FileName
    {
      get
      {
        return this._fileName;
      }
    }

    public int FileSize
    {
      get
      {
        return this._fileSize;
      }
    }

    public bool Exists
    {
      get
      {
        return File.SteamUnityAPI_SteamRemoteStorage_FileExists(CommunityExpress.Instance.RemoteStorage.SteamPointer, this._fileName);
      }
    }

    public bool Persisted
    {
      get
      {
        return File.SteamUnityAPI_SteamRemoteStorage_FilePersisted(CommunityExpress.Instance.RemoteStorage.SteamPointer, this._fileName);
      }
    }

    internal File(string fileName, int fileSize)
    {
      this._fileName = fileName;
      this._fileSize = fileSize;
    }

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamRemoteStorage_FileRead(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName, [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder sb, int nSize);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamRemoteStorage_FileRead(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] data, int nSize);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_ForgetFile(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_DeleteFile(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_FileExists(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamRemoteStorage_FilePersisted(IntPtr remoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

    public string ReadFile()
    {
      StringBuilder sb = new StringBuilder(this._fileSize);
      File.SteamUnityAPI_SteamRemoteStorage_FileRead(CommunityExpress.Instance.RemoteStorage.SteamPointer, this._fileName, sb, this._fileSize);
      return ((object) sb).ToString();
    }

    public byte[] ReadBytes()
    {
      byte[] data = new byte[this._fileSize];
      File.SteamUnityAPI_SteamRemoteStorage_FileRead(CommunityExpress.Instance.RemoteStorage.SteamPointer, this._fileName, data, this._fileSize);
      return data;
    }

    public bool Forget()
    {
      return File.SteamUnityAPI_SteamRemoteStorage_ForgetFile(CommunityExpress.Instance.RemoteStorage.SteamPointer, this._fileName);
    }

    public bool Delete()
    {
      return File.SteamUnityAPI_SteamRemoteStorage_DeleteFile(CommunityExpress.Instance.RemoteStorage.SteamPointer, this._fileName);
    }
  }
}
