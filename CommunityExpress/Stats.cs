// Type: CommunityExpressNS.Stats
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Stats : ICollection<Stat>, IEnumerable<Stat>, IEnumerable
  {
    private IntPtr _stats = IntPtr.Zero;
    private IntPtr _gameserverStats = IntPtr.Zero;
    private List<Stat> _statList = new List<Stat>();
    private SteamID _id;
    private IEnumerable<string> _requestedStats;
    private OnUserStatsReceivedFromSteam _internalOnUserStatsReceived;
    private OnUserStatsReceived _onUserStatsReceived;

    public SteamID SteamID
    {
      get
      {
        return this._id;
      }
    }

    public IList<Stat> StatsList
    {
      get
      {
        return (IList<Stat>) this._statList;
      }
    }

    public int Count
    {
      get
      {
        return this._statList.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    internal Stats()
    {
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamUserStats();

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_RequestCurrentStats(IntPtr stats, IntPtr OnUserStatsReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_GetUserStatInt(IntPtr stats, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string statName, out int value);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_GetUserStatFloat(IntPtr stats, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string statName, out float value);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_SetStatInt(IntPtr stats, [MarshalAs(UnmanagedType.LPStr)] string statName, int value);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_SetStatFloat(IntPtr stats, [MarshalAs(UnmanagedType.LPStr)] string statName, float value);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_StoreStats(IntPtr stats);

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamGameServerStats();

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamGameServerStats_RequestUserStats(IntPtr gameserverStats, ulong steamID);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServerStats_GetUserStatInt(IntPtr gameserverStats, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string statName, out int value);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServerStats_GetUserStatFloat(IntPtr gameserverStats, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string statName, out float value);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServerStats_SetUserStatInt(IntPtr gameserverStats, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string statName, int value);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServerStats_SetUserStatFloat(IntPtr gameserverStats, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string statName, float value);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServerStats_StoreUserStats(IntPtr gameserverStats, ulong steamID);

    internal void Init(SteamID steamID = null, bool isGameServer = false)
    {
      this._id = steamID;
      if (isGameServer)
        this._gameserverStats = Stats.SteamUnityAPI_SteamGameServerStats();
      else
        this._stats = Stats.SteamUnityAPI_SteamUserStats();
    }

    public void RequestCurrentStats(OnUserStatsReceived onUserStatsReceived, IEnumerable<string> requestedStats)
    {
      this._requestedStats = requestedStats;
      this._onUserStatsReceived = onUserStatsReceived;
      if (this._internalOnUserStatsReceived == null)
        this._internalOnUserStatsReceived = new OnUserStatsReceivedFromSteam(this.OnUserStatsReceivedCallback);
      if (this._gameserverStats != IntPtr.Zero)
        CommunityExpress.Instance.AddGameServerUserStatsReceivedCallback(Stats.SteamUnityAPI_SteamGameServerStats_RequestUserStats(this._gameserverStats, this._id.ToUInt64()), new OnUserStatsReceivedFromSteam(this.OnUserStatsReceivedCallback));
      else
        Stats.SteamUnityAPI_SteamUserStats_RequestCurrentStats(this._stats, Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnUserStatsReceived));
    }

    private void OnUserStatsReceivedCallback(ref UserStatsReceived_t CallbackData)
    {
      this.Clear();
      this._id = new SteamID(CallbackData.m_steamIDUser);
      if (this._gameserverStats != IntPtr.Zero)
      {
        foreach (string statName in this._requestedStats)
        {
          int num1;
          if (Stats.SteamUnityAPI_SteamGameServerStats_GetUserStatInt(this._gameserverStats, this._id.ToUInt64(), statName, out num1))
          {
            this.Add(new Stat(this, statName, (object) num1));
          }
          else
          {
            float num2;
            if (Stats.SteamUnityAPI_SteamGameServerStats_GetUserStatFloat(this._gameserverStats, this._id.ToUInt64(), statName, out num2))
              this.Add(new Stat(this, statName, (object) num2));
          }
        }
      }
      else
      {
        foreach (string statName in this._requestedStats)
        {
          int num1;
          if (Stats.SteamUnityAPI_SteamUserStats_GetUserStatInt(this._stats, this._id.ToUInt64(), statName, out num1))
          {
            this.Add(new Stat(this, statName, (object) num1));
          }
          else
          {
            float num2;
            if (Stats.SteamUnityAPI_SteamUserStats_GetUserStatFloat(this._stats, this._id.ToUInt64(), statName, out num2))
              this.Add(new Stat(this, statName, (object) num2));
          }
        }
      }
      this._onUserStatsReceived(this, (Achievements) null);
    }

    public void WriteStats()
    {
      if (this._gameserverStats != IntPtr.Zero)
      {
        foreach (Stat stat in this._statList)
        {
          if (stat.HasChanged)
          {
            if (stat.StatValue is int)
              Stats.SteamUnityAPI_SteamGameServerStats_SetUserStatInt(this._gameserverStats, this._id.ToUInt64(), stat.StatName, (int) stat.StatValue);
            else
              Stats.SteamUnityAPI_SteamGameServerStats_SetUserStatFloat(this._gameserverStats, this._id.ToUInt64(), stat.StatName, (float) stat.StatValue);
            stat.HasChanged = false;
          }
        }
        Stats.SteamUnityAPI_SteamGameServerStats_StoreUserStats(this._gameserverStats, this._id.ToUInt64());
      }
      else
      {
        foreach (Stat stat in this._statList)
        {
          if (stat.HasChanged)
          {
            if (stat.StatValue is int)
              Stats.SteamUnityAPI_SteamUserStats_SetStatInt(this._stats, stat.StatName, (int) stat.StatValue);
            else
              Stats.SteamUnityAPI_SteamUserStats_SetStatFloat(this._stats, stat.StatName, (float) stat.StatValue);
            stat.HasChanged = false;
          }
        }
        Stats.SteamUnityAPI_SteamUserStats_StoreStats(this._stats);
      }
    }

    public void Add(Stat item)
    {
      this._statList.Add(item);
    }

    public void Clear()
    {
      this._statList.Clear();
    }

    public bool Contains(Stat item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Stat[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(Stat item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<Stat> GetEnumerator()
    {
      return (IEnumerator<Stat>) new ListEnumerator<Stat>((IList<Stat>) this._statList);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
