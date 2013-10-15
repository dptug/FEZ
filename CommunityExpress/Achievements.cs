// Type: CommunityExpressNS.Achievements
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Achievements : ICollection<Achievement>, IEnumerable<Achievement>, IEnumerable
  {
    private IntPtr _stats = IntPtr.Zero;
    private IntPtr _gameserverStats = IntPtr.Zero;
    private List<Achievement> _achievementList = new List<Achievement>();
    private SteamID _id;
    private IEnumerable<string> _requestedAchievements;
    private OnUserStatsReceivedFromSteam _internalOnUserStatsReceived;
    private OnUserStatsReceived _onUserStatsReceived;

    public SteamID SteamID
    {
      get
      {
        return this._id;
      }
    }

    public IList<Achievement> AchievementList
    {
      get
      {
        return (IList<Achievement>) this._achievementList;
      }
    }

    public int Count
    {
      get
      {
        return this._achievementList.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamUserStats();

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_RequestCurrentStats(IntPtr stats, IntPtr OnUserStatsReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_GetAchievement(IntPtr stats, [MarshalAs(UnmanagedType.LPStr)] string achievementName, out byte isAchieved);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_GetUserAchievement(IntPtr stats, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string achievementName, out byte isAchieved);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_SetAchievement(IntPtr stats, [MarshalAs(UnmanagedType.LPStr)] string achievementName);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_StoreStats(IntPtr stats);

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamGameServerStats();

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamGameServerStats_RequestUserStats(IntPtr gameserverStats, ulong steamID);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServerStats_GetUserAchievement(IntPtr gameserverStats, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string achievementName, out byte isAchieved);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServerStats_SetUserAchievement(IntPtr stats, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string achievementName);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServerStats_StoreUserStats(IntPtr gameserverStats, ulong steamID);

    internal void Init(SteamID steamID = null, bool isGameServer = false)
    {
      this._id = steamID;
      if (isGameServer)
        this._gameserverStats = Achievements.SteamUnityAPI_SteamGameServerStats();
      else
        this._stats = Achievements.SteamUnityAPI_SteamUserStats();
    }

    public void RequestCurrentAchievements(OnUserStatsReceived onUserStatsReceived, IEnumerable<string> requestedAchievements)
    {
      this._requestedAchievements = requestedAchievements;
      this._onUserStatsReceived = onUserStatsReceived;
      if (this._internalOnUserStatsReceived == null)
        this._internalOnUserStatsReceived = new OnUserStatsReceivedFromSteam(this.OnUserStatsReceivedCallback);
      if (this._gameserverStats != IntPtr.Zero)
        CommunityExpress.Instance.AddGameServerUserStatsReceivedCallback(Achievements.SteamUnityAPI_SteamGameServerStats_RequestUserStats(this._gameserverStats, this._id.ToUInt64()), new OnUserStatsReceivedFromSteam(this.OnUserStatsReceivedCallback));
      else
        Achievements.SteamUnityAPI_SteamUserStats_RequestCurrentStats(this._stats, Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnUserStatsReceived));
    }

    private void OnUserStatsReceivedCallback(ref UserStatsReceived_t CallbackData)
    {
      this._id = new SteamID(CallbackData.m_steamIDUser);
      this.InitializeAchievementList(this._requestedAchievements);
      if (this._onUserStatsReceived == null)
        return;
      this._onUserStatsReceived((Stats) null, this);
    }

    public void InitializeAchievementList(IEnumerable<string> requestedAchievements)
    {
      this.Clear();
      this._requestedAchievements = requestedAchievements;
      if (this._gameserverStats != IntPtr.Zero)
      {
        foreach (string achievementName in this._requestedAchievements)
        {
          byte isAchieved;
          if (Achievements.SteamUnityAPI_SteamGameServerStats_GetUserAchievement(this._gameserverStats, this._id.ToUInt64(), achievementName, out isAchieved))
            this.Add(new Achievement(this, this._stats, achievementName, (int) isAchieved != 0));
        }
      }
      else if (this._id != (SteamID) null)
      {
        foreach (string achievementName in this._requestedAchievements)
        {
          byte isAchieved;
          if (Achievements.SteamUnityAPI_SteamUserStats_GetUserAchievement(this._stats, this._id.ToUInt64(), achievementName, out isAchieved))
            this.Add(new Achievement(this, this._stats, achievementName, (int) isAchieved != 0));
        }
      }
      else
      {
        foreach (string achievementName in this._requestedAchievements)
        {
          byte isAchieved;
          if (Achievements.SteamUnityAPI_SteamUserStats_GetAchievement(this._stats, achievementName, out isAchieved))
            this.Add(new Achievement(this, this._stats, achievementName, (int) isAchieved != 0));
        }
      }
    }

    public void UnlockAchievement(string achievementName, bool storeStats = false)
    {
      foreach (Achievement achievement in this._achievementList)
      {
        if (achievement.AchievementName == achievementName)
        {
          if (achievement.IsAchieved)
            break;
          if (this._gameserverStats != IntPtr.Zero)
            Achievements.SteamUnityAPI_SteamGameServerStats_SetUserAchievement(this._gameserverStats, this._id.ToUInt64(), achievement.AchievementName);
          else
            Achievements.SteamUnityAPI_SteamUserStats_SetAchievement(this._stats, achievement.AchievementName);
          achievement.IsAchieved = true;
          if (!storeStats)
            break;
          this.WriteStats();
          break;
        }
      }
    }

    public void UnlockAchievement(Achievement achievement, bool storeStats)
    {
      if (achievement.IsAchieved)
        return;
      if (this._gameserverStats != IntPtr.Zero)
        Achievements.SteamUnityAPI_SteamGameServerStats_SetUserAchievement(this._gameserverStats, this._id.ToUInt64(), achievement.AchievementName);
      else
        Achievements.SteamUnityAPI_SteamUserStats_SetAchievement(this._stats, achievement.AchievementName);
      achievement.IsAchieved = true;
      if (!storeStats)
        return;
      this.WriteStats();
    }

    public void WriteStats()
    {
      if (this._gameserverStats != IntPtr.Zero)
        Achievements.SteamUnityAPI_SteamGameServerStats_StoreUserStats(this._gameserverStats, this._id.ToUInt64());
      else
        Achievements.SteamUnityAPI_SteamUserStats_StoreStats(this._stats);
    }

    public void Add(Achievement item)
    {
      this._achievementList.Add(item);
    }

    public void Clear()
    {
      this._achievementList.Clear();
    }

    public bool Contains(Achievement item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Achievement[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(Achievement item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<Achievement> GetEnumerator()
    {
      return (IEnumerator<Achievement>) new ListEnumerator<Achievement>((IList<Achievement>) this._achievementList);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
