// Type: CommunityExpressNS.Leaderboards
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Leaderboards : ICollection<Leaderboard>, IEnumerable<Leaderboard>, IEnumerable
  {
    private List<Leaderboard> _leaderboardList = new List<Leaderboard>();
    private IntPtr _stats;
    private OnLeaderboardRetrievedFromSteam _internalOnLeaderboardRetrieved;
    private OnLeaderboardRetrieved _onLeaderboardRetrieved;

    public IntPtr Stats
    {
      get
      {
        return this._stats;
      }
    }

    public int Count
    {
      get
      {
        return this._leaderboardList.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    internal Leaderboards()
    {
      this._stats = Leaderboards.SteamUnityAPI_SteamUserStats();
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamUserStats();

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_FindLeaderboard(IntPtr stats, [MarshalAs(UnmanagedType.LPStr)] string leaderboardName, IntPtr OnLeaderboardRetrievedCallback);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_FindOrCreateLeaderboard(IntPtr stats, [MarshalAs(UnmanagedType.LPStr)] string leaderboardName, ELeaderboardSortMethod sortMethod, ELeaderboardDisplayType displayType, IntPtr OnLeaderboardRetrievedCallback);

    [DllImport("CommunityExpressSW")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static string SteamUnityAPI_SteamUserStats_GetLeaderboardName(IntPtr stats, ulong leaderboard);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamUserStats_GetLeaderboardEntryCount(IntPtr stats, ulong leaderboard);

    [DllImport("CommunityExpressSW")]
    private static ELeaderboardSortMethod SteamUnityAPI_SteamUserStats_GetLeaderboardSortMethod(IntPtr stats, ulong leaderboard);

    [DllImport("CommunityExpressSW")]
    private static ELeaderboardDisplayType SteamUnityAPI_SteamUserStats_GetLeaderboardDisplayType(IntPtr stats, ulong leaderboard);

    public void FindLeaderboard(OnLeaderboardRetrieved onLeaderboardRetrieved, string leaderboardName)
    {
      Leaderboard leaderboard1 = (Leaderboard) null;
      foreach (Leaderboard leaderboard2 in this._leaderboardList)
      {
        if (leaderboard2.LeaderboardName == leaderboardName)
        {
          leaderboard1 = leaderboard2;
          break;
        }
      }
      if (leaderboard1 != null)
      {
        onLeaderboardRetrieved(leaderboard1);
      }
      else
      {
        this._onLeaderboardRetrieved = onLeaderboardRetrieved;
        if (this._internalOnLeaderboardRetrieved == null)
          this._internalOnLeaderboardRetrieved = new OnLeaderboardRetrievedFromSteam(this.OnLeaderboardRetrievedCallback);
        if (Leaderboards.SteamUnityAPI_SteamUserStats_FindLeaderboard(this._stats, leaderboardName, Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnLeaderboardRetrieved)))
          return;
        this._onLeaderboardRetrieved((Leaderboard) null);
      }
    }

    public void FindOrCreateLeaderboard(OnLeaderboardRetrieved onLeaderboardRetrieved, string leaderboardName, ELeaderboardSortMethod sortMethod, ELeaderboardDisplayType displayType)
    {
      Leaderboard leaderboard1 = (Leaderboard) null;
      foreach (Leaderboard leaderboard2 in this._leaderboardList)
      {
        if (leaderboard2.LeaderboardName == leaderboardName)
        {
          leaderboard1 = leaderboard2;
          break;
        }
      }
      if (leaderboard1 != null)
      {
        onLeaderboardRetrieved(leaderboard1);
      }
      else
      {
        this._onLeaderboardRetrieved = onLeaderboardRetrieved;
        if (this._internalOnLeaderboardRetrieved == null)
          this._internalOnLeaderboardRetrieved = new OnLeaderboardRetrievedFromSteam(this.OnLeaderboardRetrievedCallback);
        if (Leaderboards.SteamUnityAPI_SteamUserStats_FindOrCreateLeaderboard(this._stats, leaderboardName, sortMethod, displayType, Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnLeaderboardRetrieved)))
          return;
        this._onLeaderboardRetrieved((Leaderboard) null);
      }
    }

    private void OnLeaderboardRetrievedCallback(ref LeaderboardFindResult_t findLearderboardResult)
    {
      Leaderboard leaderboard = (Leaderboard) null;
      if ((int) findLearderboardResult.m_bLeaderboardFound != 0)
      {
        leaderboard = new Leaderboard(this, findLearderboardResult.m_hSteamLeaderboard, Leaderboards.SteamUnityAPI_SteamUserStats_GetLeaderboardName(this._stats, findLearderboardResult.m_hSteamLeaderboard), Leaderboards.SteamUnityAPI_SteamUserStats_GetLeaderboardEntryCount(this._stats, findLearderboardResult.m_hSteamLeaderboard), Leaderboards.SteamUnityAPI_SteamUserStats_GetLeaderboardSortMethod(this._stats, findLearderboardResult.m_hSteamLeaderboard), Leaderboards.SteamUnityAPI_SteamUserStats_GetLeaderboardDisplayType(this._stats, findLearderboardResult.m_hSteamLeaderboard));
        this.Add(leaderboard);
      }
      this._onLeaderboardRetrieved(leaderboard);
    }

    public void Add(Leaderboard item)
    {
      this._leaderboardList.Add(item);
    }

    public void Clear()
    {
      this._leaderboardList.Clear();
    }

    public bool Contains(Leaderboard item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Leaderboard[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(Leaderboard item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<Leaderboard> GetEnumerator()
    {
      return (IEnumerator<Leaderboard>) new ListEnumerator<Leaderboard>((IList<Leaderboard>) this._leaderboardList);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
