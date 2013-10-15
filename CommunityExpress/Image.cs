// Type: CommunityExpressNS.Image
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Image
  {
    private int _steamImage;
    private uint _iconWidth;
    private uint _iconHeight;

    public uint Width
    {
      get
      {
        return this._iconWidth;
      }
    }

    public uint Height
    {
      get
      {
        return this._iconHeight;
      }
    }

    internal Image(int steamImage)
    {
      this._steamImage = steamImage;
      Image.SteamUnityAPI_SteamUtils_GetImageSize(this._steamImage, out this._iconWidth, out this._iconHeight);
    }

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamUtils_GetImageSize(int iconIndex, out uint iconWidth, out uint iconHeight);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamUtils_GetImageRGBA(int iconIndex, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] data, int dataSize);

    [DllImport("CommunityExpressSW", EntryPoint = "SteamUnityAPI_SteamUtils_GetImageRGBA")]
    private static void SteamUnityAPI_SteamUtils_GetImageRGBA_Ptr(int iconIndex, IntPtr data, int dataSize);

    public void GetPixels(IntPtr data, int dataSize)
    {
      if (this._iconWidth <= 0U || this._iconHeight <= 0U)
        return;
      Image.SteamUnityAPI_SteamUtils_GetImageRGBA_Ptr(this._steamImage, data, dataSize);
    }

    public byte[] AsBytes()
    {
      if (this._iconWidth <= 0U || this._iconHeight <= 0U)
        return (byte[]) null;
      byte[] data = new byte[(IntPtr) (uint) ((int) this._iconWidth * (int) this._iconHeight * 4)];
      Image.SteamUnityAPI_SteamUtils_GetImageRGBA(this._steamImage, data, (int) this._iconWidth * (int) this._iconHeight * 4);
      return data;
    }
  }
}
