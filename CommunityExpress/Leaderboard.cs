// Type: CommunityExpressNS.Leaderboard
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Leaderboard
  {
    private IntPtr _stats;
    private Leaderboards _leaderboards;
    private ulong _leaderboard;
    private string _leaderboardName;
    private int _entryCount;
    private ELeaderboardSortMethod _sortMethod;
    private ELeaderboardDisplayType _displayType;
    private int _maxDetails;
    private OnLeaderboardRetrievedFromSteam _internalOnLeaderboardRetrieved;
    private OnLeaderboardRetrieved _onLeaderboardRetrieved;
    private OnLeaderboardEntriesRetrievedFromSteam _internalOnLeaderboardEntriesRetrieved;
    private OnLeaderboardEntriesRetrieved _onLeaderboardEntriesRetrieved;

    public string LeaderboardName
    {
      get
      {
        return this._leaderboardName;
      }
    }

    public int EntryCount
    {
      get
      {
        return this._entryCount;
      }
    }

    public ELeaderboardSortMethod SortMethod
    {
      get
      {
        return this._sortMethod;
      }
    }

    public ELeaderboardDisplayType DisplayType
    {
      get
      {
        return this._displayType;
      }
    }

    internal Leaderboard(Leaderboards leaderboards, ulong leaderboard, string leaderboardName, int entryCount, ELeaderboardSortMethod sortMethod, ELeaderboardDisplayType displayType)
    {
      this._stats = Leaderboard.SteamUnityAPI_SteamUserStats();
      this._leaderboards = leaderboards;
      this._leaderboard = leaderboard;
      this._leaderboardName = leaderboardName;
      this._entryCount = entryCount;
      this._sortMethod = sortMethod;
      this._displayType = displayType;
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamUserStats();

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_FindLeaderboard(IntPtr stats, [MarshalAs(UnmanagedType.LPStr)] string leaderboardName, IntPtr OnLeaderboardRetrievedCallback);

    [DllImport("CommunityExpressSW")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static string SteamUnityAPI_SteamUserStats_GetLeaderboardName(IntPtr stats, ulong leaderboard);

    [DllImport("CommunityExpressSW")]
    private static ELeaderboardSortMethod SteamUnityAPI_SteamUserStats_GetLeaderboardSortMethod(IntPtr stats, ulong leaderboard);

    [DllImport("CommunityExpressSW")]
    private static ELeaderboardDisplayType SteamUnityAPI_SteamUserStats_GetLeaderboardDisplayType(IntPtr stats, ulong leaderboard);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_UploadLeaderboardScore(IntPtr stats, ulong leaderboard, ELeaderboardUploadScoreMethod uploadScoreMethod, int score, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] scoreDetails, int scoreDetailCount);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_RequestLeaderboardEntries(IntPtr stats, ulong leaderboard, ELeaderboardDataRequest requestType, int startIndex, int endIndex, IntPtr OnLeaderboardEntriesRetrievedCallback);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUserStats_GetDownloadedLeaderboardEntry(IntPtr stats, ulong leaderboardEntries, int index, ref LeaderboardEntry_t leaderboardEntry, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] scoreDetails, int maxScoreDetailCount);

    public void UploadLeaderboardScore(ELeaderboardUploadScoreMethod uploadScoreMethod, int score, List<int> scoreDetails)
    {
      if (scoreDetails != null)
        Leaderboard.SteamUnityAPI_SteamUserStats_UploadLeaderboardScore(this._leaderboards.Stats, this._leaderboard, uploadScoreMethod, score, scoreDetails.ToArray(), scoreDetails.Count);
      else
        Leaderboard.SteamUnityAPI_SteamUserStats_UploadLeaderboardScore(this._leaderboards.Stats, this._leaderboard, uploadScoreMethod, score, (int[]) null, 0);
    }

    public void RequestLeaderboardEntries(int startIndex, int endIndex, int maxExpectedDetails, OnLeaderboardEntriesRetrieved onLeaderboardEntriesRetrieved)
    {
      this._onLeaderboardEntriesRetrieved = onLeaderboardEntriesRetrieved;
      this._maxDetails = maxExpectedDetails;
      if (this._internalOnLeaderboardEntriesRetrieved == null)
        this._internalOnLeaderboardEntriesRetrieved = new OnLeaderboardEntriesRetrievedFromSteam(this.OnLeaderboardEntriesRetrievedCallback);
      if (Leaderboard.SteamUnityAPI_SteamUserStats_RequestLeaderboardEntries(this._leaderboards.Stats, this._leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, startIndex, endIndex, Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnLeaderboardEntriesRetrieved)))
        return;
      this._onLeaderboardEntriesRetrieved((LeaderboardEntries) null);
    }

    public void RequestLeaderboardEntriesAroundCurrentUser(int rowsBefore, int rowsAfter, int maxExpectedDetails, OnLeaderboardEntriesRetrieved onLeaderboardEntriesRetrieved)
    {
      this._onLeaderboardEntriesRetrieved = onLeaderboardEntriesRetrieved;
      this._maxDetails = maxExpectedDetails;
      if (this._internalOnLeaderboardEntriesRetrieved == null)
        this._internalOnLeaderboardEntriesRetrieved = new OnLeaderboardEntriesRetrievedFromSteam(this.OnLeaderboardEntriesRetrievedCallback);
      if (Leaderboard.SteamUnityAPI_SteamUserStats_RequestLeaderboardEntries(this._leaderboards.Stats, this._leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -1 * rowsBefore, rowsAfter, Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnLeaderboardEntriesRetrieved)))
        return;
      this._onLeaderboardEntriesRetrieved((LeaderboardEntries) null);
    }

    public void RequestFriendLeaderboardEntries(int maxExpectedDetails, OnLeaderboardEntriesRetrieved onLeaderboardEntriesRetrieved)
    {
      this._onLeaderboardEntriesRetrieved = onLeaderboardEntriesRetrieved;
      this._maxDetails = maxExpectedDetails;
      if (this._internalOnLeaderboardEntriesRetrieved == null)
        this._internalOnLeaderboardEntriesRetrieved = new OnLeaderboardEntriesRetrievedFromSteam(this.OnLeaderboardEntriesRetrievedCallback);
      if (Leaderboard.SteamUnityAPI_SteamUserStats_RequestLeaderboardEntries(this._leaderboards.Stats, this._leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, 0, 2147483646, Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnLeaderboardEntriesRetrieved)))
        return;
      this._onLeaderboardEntriesRetrieved((LeaderboardEntries) null);
    }

    private void OnLeaderboardEntriesRetrievedCallback(ref LeaderboardScoresDownloaded_t callbackData)
    {
      if (callbackData.m_cEntryCount > 0)
      {
        int num = callbackData.m_cEntryCount;
        LeaderboardEntry_t leaderboardEntry = new LeaderboardEntry_t();
        LeaderboardEntries leaderboardEntries = new LeaderboardEntries(this);
        int[] scoreDetails1 = new int[this._maxDetails];
        for (int index = 0; index < num; ++index)
        {
          if (Leaderboard.SteamUnityAPI_SteamUserStats_GetDownloadedLeaderboardEntry(this._leaderboards.Stats, callbackData.m_hSteamLeaderboardEntries, index, ref leaderboardEntry, scoreDetails1, this._maxDetails))
          {
            List<int> scoreDetails2 = (List<int>) null;
            if (scoreDetails1 != null)
              scoreDetails2 = new List<int>((IEnumerable<int>) scoreDetails1);
            leaderboardEntries.Add(new LeaderboardEntry(leaderboardEntry.m_steamIDUser, leaderboardEntry.m_nGlobalRank, leaderboardEntry.m_nScore, scoreDetails2));
          }
        }
        this._onLeaderboardEntriesRetrieved(leaderboardEntries);
      }
      else
        this._onLeaderboardEntriesRetrieved((LeaderboardEntries) null);
    }

    public void Refresh(OnLeaderboardRetrieved onLeaderboardRefreshComplete)
    {
      this._onLeaderboardRetrieved = onLeaderboardRefreshComplete;
      if (this._internalOnLeaderboardRetrieved == null)
        this._internalOnLeaderboardRetrieved = new OnLeaderboardRetrievedFromSteam(this.OnLeaderboardRetrievedCallback);
      if (Leaderboard.SteamUnityAPI_SteamUserStats_FindLeaderboard(this._stats, this._leaderboardName, Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnLeaderboardRetrieved)))
        return;
      this._onLeaderboardRetrieved(this);
    }

    private void OnLeaderboardRetrievedCallback(ref LeaderboardFindResult_t findLearderboardResult)
    {
      if ((int) findLearderboardResult.m_bLeaderboardFound != 0)
      {
        this._leaderboard = findLearderboardResult.m_hSteamLeaderboard;
        this._leaderboardName = Leaderboard.SteamUnityAPI_SteamUserStats_GetLeaderboardName(this._stats, findLearderboardResult.m_hSteamLeaderboard);
        this._sortMethod = Leaderboard.SteamUnityAPI_SteamUserStats_GetLeaderboardSortMethod(this._stats, findLearderboardResult.m_hSteamLeaderboard);
        this._displayType = Leaderboard.SteamUnityAPI_SteamUserStats_GetLeaderboardDisplayType(this._stats, findLearderboardResult.m_hSteamLeaderboard);
      }
      this._onLeaderboardRetrieved(this);
    }
  }
}
