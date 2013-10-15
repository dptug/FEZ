// Type: CommunityExpressNS.Achievement
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Achievement
  {
    private IntPtr _stats;
    private Achievements _achievements;
    private string _achievementName;
    private bool _isAchieved;
    private string _displayName;
    private string _displayDescription;
    private Image _image;

    public string AchievementName
    {
      get
      {
        return this._achievementName;
      }
      private set
      {
        this._achievementName = value;
      }
    }

    public bool IsAchieved
    {
      get
      {
        return this._isAchieved;
      }
      internal set
      {
        this._isAchieved = true;
      }
    }

    public string DisplayName
    {
      get
      {
        if (this._displayName == null && this._stats != IntPtr.Zero)
          this._displayName = Marshal.PtrToStringAnsi(Achievement.SteamUnityAPI_SteamUserStats_GetAchievementDisplayAttribute(this._stats, this._achievementName, "name"));
        return this._displayName;
      }
    }

    public string DisplayDescription
    {
      get
      {
        if (this._displayDescription == null && this._stats != IntPtr.Zero)
          this._displayDescription = Marshal.PtrToStringAnsi(Achievement.SteamUnityAPI_SteamUserStats_GetAchievementDisplayAttribute(this._stats, this._achievementName, "desc"));
        return this._displayDescription;
      }
    }

    public Image Icon
    {
      get
      {
        if (this._image == null && this._stats != IntPtr.Zero)
          this._image = new Image(Achievement.SteamUnityAPI_SteamUserStats_GetAchievementIcon(this._stats, this._achievementName));
        return this._image;
      }
    }

    public byte[] IconData
    {
      get
      {
        Image icon = this.Icon;
        if (icon != null)
          return icon.AsBytes();
        else
          return (byte[]) null;
      }
    }

    public uint IconWidth
    {
      get
      {
        Image icon = this.Icon;
        if (icon != null)
          return icon.Width;
        else
          return 0U;
      }
    }

    public uint IconHeight
    {
      get
      {
        Image icon = this.Icon;
        if (icon != null)
          return icon.Height;
        else
          return 0U;
      }
    }

    public Achievement(Achievements achievements, IntPtr stats)
    {
      this._stats = stats;
      this._achievements = achievements;
    }

    public Achievement(Achievements achievements, IntPtr stats, string achievementName, bool isAchieved)
    {
      this._stats = stats;
      this._achievements = achievements;
      this._achievementName = achievementName;
      this._isAchieved = isAchieved;
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamUserStats_GetAchievementDisplayAttribute(IntPtr stats, [MarshalAs(UnmanagedType.LPStr)] string pchName, [MarshalAs(UnmanagedType.LPStr)] string pchAttribute);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamUserStats_GetAchievementIcon(IntPtr stats, [MarshalAs(UnmanagedType.LPStr)] string pchName);
  }
}
