// Type: CommunityExpressNS.Matchmaking
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Matchmaking
  {
    private Lobbies _lobbyList = new Lobbies();
    public const uint HServerListRequest_Invalid = 0U;
    private IntPtr _matchmaking;
    private ulong _lobbyListRequest;
    private OnLobbyCreated _onLobbyCreated;
    private OnLobbyListReceived _onLobbyListReceived;
    private Lobby _lobbyJoined;
    private OnLobbyJoined _onLobbyJoined;
    private IntPtr _matchmakingServers;
    private uint _serverListRequest;
    private Servers _serverList;
    private Dictionary<string, string> _serverFilters;
    private OnServerReceived _onServerReceived;
    private OnServerListReceived _onServerListReceived;
    private OnMatchmakingServerReceivededFromSteam _onServerReceivedFromSteam;
    private OnMatchmakingServerListReceivededFromSteam _onServerListReceivedFromSteam;

    internal Matchmaking()
    {
      this._matchmaking = Matchmaking.SteamUnityAPI_SteamMatchmaking();
      this._matchmakingServers = Matchmaking.SteamUnityAPI_SteamMatchmakingServers();
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamMatchmaking();

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamMatchmakingServers();

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamUtils_GetAppID();

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamMatchmaking_CreateLobby(IntPtr matchmaking, ELobbyType lobbyType, int maxMembers);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListStringFilter(IntPtr matchmaking, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value, ELobbyComparison comparisonType);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListNumericalFilter(IntPtr matchmaking, [MarshalAs(UnmanagedType.LPStr)] string key, int value, ELobbyComparison comparisonType);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListNearValueFilter(IntPtr matchmaking, [MarshalAs(UnmanagedType.LPStr)] string key, int value);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(IntPtr matchmaking, int slotsAvailable);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListDistanceFilter(IntPtr matchmaking, ELobbyDistanceFilter lobbyDistanceFilter);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListResultCountFilter(IntPtr matchmaking, int maxResults);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(IntPtr matchmaking, ulong steamIDLobby);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamMatchmaking_RequestLobbyList(IntPtr matchmaking);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamMatchmaking_JoinLobby(IntPtr matchmaking, ulong steamIDLobby);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamMatchmaking_LeaveLobby(IntPtr matchmaking, ulong steamIDLobby);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamMatchmaking_GetLobbyByIndex(IntPtr matchmaking, int lobbyIndex);

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamMatchmakingServers_RequestInternetServerList(IntPtr matchmakingServers, uint appId, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] keys, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] values, uint keyvalueCount, IntPtr serverReceivedCallback, IntPtr serverListReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamMatchmakingServers_RequestLANServerList(IntPtr matchmakingServers, uint appId, IntPtr serverReceivedCallback, IntPtr serverListReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamMatchmakingServers_RequestSpectatorServerList(IntPtr matchmakingServers, uint appId, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] keys, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] values, uint keyvalueCount, IntPtr serverReceivedCallback, IntPtr serverListReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamMatchmakingServers_RequestHistoryServerList(IntPtr matchmakingServers, uint appId, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] keys, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] values, uint keyvalueCount, IntPtr serverReceivedCallback, IntPtr serverListReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamMatchmakingServers_RequestFavoriteServerList(IntPtr matchmakingServers, uint appId, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] keys, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] values, uint keyvalueCount, IntPtr serverReceivedCallback, IntPtr serverListReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamMatchmakingServers_RequestFriendServerList(IntPtr matchmakingServers, uint appId, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] keys, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] values, uint keyvalueCount, IntPtr serverReceivedCallback, IntPtr serverListReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamMatchmakingServers_ReleaseRequest(IntPtr matchmakingServers, uint request);

    public void CreateLobby(ELobbyType lobbyType, int maxMembers, OnLobbyCreated onLobbyCreated)
    {
      if (CommunityExpress.Instance.IsGameServerInitialized)
        throw new Exception("A Lobby cannot be created after a Steam Game Server has been initialized.");
      this._onLobbyCreated = onLobbyCreated;
      CommunityExpress.Instance.AddCreateLobbyCallback(Matchmaking.SteamUnityAPI_SteamMatchmaking_CreateLobby(this._matchmaking, lobbyType, maxMembers), new OnMatchmakingLobbyCreatedBySteam(this.OnLobbyCreatedCallback));
    }

    private void OnLobbyCreatedCallback(ref LobbyCreated_t callbackData)
    {
      this._onLobbyCreated(new Lobby((Lobbies) null, new SteamID(callbackData.m_ulSteamIDLobby)));
    }

    public void RequestLobbyList(ICollection<LobbyStringFilter> stringFilters, ICollection<LobbyIntFilter> intFilters, Dictionary<string, int> nearValueFilters, int requiredSlotsAvailable, ELobbyDistanceFilter lobbyDistance, int maxResults, ICollection<SteamID> compatibleSteamIDs, OnLobbyListReceived onLobbyListReceived)
    {
      if ((long) this._lobbyListRequest != 0L)
        this.CancelCurrentLobbyListRequest();
      this._onLobbyListReceived = onLobbyListReceived;
      if (stringFilters != null)
      {
        foreach (LobbyStringFilter lobbyStringFilter in (IEnumerable<LobbyStringFilter>) stringFilters)
          Matchmaking.SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListStringFilter(this._matchmaking, lobbyStringFilter.key, lobbyStringFilter.value, lobbyStringFilter.comparison);
      }
      if (intFilters != null)
      {
        foreach (LobbyIntFilter lobbyIntFilter in (IEnumerable<LobbyIntFilter>) intFilters)
          Matchmaking.SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListNumericalFilter(this._matchmaking, lobbyIntFilter.key, lobbyIntFilter.value, lobbyIntFilter.comparison);
      }
      if (nearValueFilters != null)
      {
        foreach (KeyValuePair<string, int> keyValuePair in nearValueFilters)
          Matchmaking.SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListNearValueFilter(this._matchmaking, keyValuePair.Key, keyValuePair.Value);
      }
      if (compatibleSteamIDs != null)
      {
        foreach (SteamID steamId in (IEnumerable<SteamID>) compatibleSteamIDs)
          Matchmaking.SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(this._matchmaking, steamId.ToUInt64());
      }
      if (requiredSlotsAvailable != 0)
        Matchmaking.SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(this._matchmaking, requiredSlotsAvailable);
      if (maxResults != 0)
        Matchmaking.SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListResultCountFilter(this._matchmaking, maxResults);
      Matchmaking.SteamUnityAPI_SteamMatchmaking_AddRequestLobbyListDistanceFilter(this._matchmaking, lobbyDistance);
      CommunityExpress.Instance.AddLobbyListRequestCallback(Matchmaking.SteamUnityAPI_SteamMatchmaking_RequestLobbyList(this._matchmaking), new OnMatchmakingLobbyListReceivedFromSteam(this.OnLobbyListReceivedCallback));
    }

    private void OnLobbyListReceivedCallback(ref LobbyMatchList_t callbackData)
    {
      Lobbies lobbies = new Lobbies();
      for (int lobbyIndex = 0; (long) lobbyIndex < (long) callbackData.m_nLobbiesMatching; ++lobbyIndex)
      {
        ulong lobbyByIndex = Matchmaking.SteamUnityAPI_SteamMatchmaking_GetLobbyByIndex(this._matchmaking, lobbyIndex);
        Lobby lobby1 = (Lobby) null;
        foreach (Lobby lobby2 in this._lobbyList)
        {
          if (lobby2.SteamID == lobbyByIndex)
          {
            lobby1 = lobby2;
            break;
          }
        }
        if (lobby1 == null)
          lobby1 = new Lobby(this._lobbyList, new SteamID(lobbyByIndex));
        lobbies.Add(lobby1);
      }
      this._onLobbyListReceived(lobbies);
    }

    public void CancelCurrentLobbyListRequest()
    {
      CommunityExpress.Instance.RemoveLobbyListRequestCallback(this._lobbyListRequest, new OnMatchmakingLobbyListReceivedFromSteam(this.OnLobbyListReceivedCallback));
      this._lobbyListRequest = 0UL;
    }

    public void JoinLobby(SteamID steamIDLobby, OnLobbyJoined onLobbyJoined)
    {
      if (this._lobbyJoined != null)
        this.LeaveLobby();
      foreach (Lobby lobby in this._lobbyList)
      {
        if (lobby.SteamID == steamIDLobby)
        {
          this._lobbyJoined = lobby;
          break;
        }
      }
      this._onLobbyJoined = onLobbyJoined;
      CommunityExpress.Instance.AddLobbyJoinedCallback(Matchmaking.SteamUnityAPI_SteamMatchmaking_JoinLobby(this._matchmaking, steamIDLobby.ToUInt64()), new OnMatchmakingLobbyJoinedFromSteam(this.OnLobbyJoinedCallback));
    }

    public void JoinLobby(Lobby lobby, OnLobbyJoined onLobbyJoined)
    {
      if (this._lobbyJoined != null)
        this.LeaveLobby();
      this._lobbyJoined = lobby;
      this._onLobbyJoined = onLobbyJoined;
      CommunityExpress.Instance.AddLobbyJoinedCallback(Matchmaking.SteamUnityAPI_SteamMatchmaking_JoinLobby(this._matchmaking, lobby.SteamID.ToUInt64()), new OnMatchmakingLobbyJoinedFromSteam(this.OnLobbyJoinedCallback));
    }

    private void OnLobbyJoinedCallback(ref LobbyEnter_t callbackData)
    {
      if (this._lobbyJoined == null)
      {
        this._lobbyJoined = new Lobby((Lobbies) null, new SteamID(callbackData.m_ulSteamIDLobby));
        this._lobbyList.Add(this._lobbyJoined);
      }
      this._lobbyJoined.IsLocked = (int) callbackData.m_bLocked != 0;
      this._lobbyJoined.ChatPermissions = callbackData.m_rgfChatPermissions;
      this._onLobbyJoined(this._lobbyJoined, (EChatRoomEnterResponse) callbackData.m_EChatRoomEnterResponse);
    }

    public void LeaveLobby()
    {
      if (this._lobbyJoined == null)
        return;
      Matchmaking.SteamUnityAPI_SteamMatchmaking_LeaveLobby(this._matchmaking, this._lobbyJoined.SteamID.ToUInt64());
      this._lobbyJoined = (Lobby) null;
    }

    private void PrepServerListRequest(Dictionary<string, string> filters, OnServerReceived onServerReceived, OnServerListReceived onServerListReceived, out string[] keys, out string[] values)
    {
      if ((int) this._serverListRequest != 0)
        this.CancelCurrentServerListRequest();
      this._serverList = new Servers();
      this._serverFilters = filters;
      this._onServerReceived = onServerReceived;
      this._onServerListReceived = onServerListReceived;
      if (this._onServerReceivedFromSteam == null)
      {
        this._onServerReceivedFromSteam = new OnMatchmakingServerReceivededFromSteam(this.OnServerReceived);
        this._onServerListReceivedFromSteam = new OnMatchmakingServerListReceivededFromSteam(this.OnServerListComplete);
      }
      if (this._serverFilters != null)
      {
        keys = new string[this._serverFilters.Keys.Count];
        values = new string[this._serverFilters.Values.Count];
        this._serverFilters.Keys.CopyTo(keys, 0);
        this._serverFilters.Values.CopyTo(values, 0);
      }
      else
      {
        keys = (string[]) null;
        values = (string[]) null;
      }
    }

    public Servers RequestInternetServerList(Dictionary<string, string> filters, OnServerReceived onServerReceived, OnServerListReceived onServerListReceived)
    {
      string[] keys;
      string[] values;
      this.PrepServerListRequest(filters, onServerReceived, onServerListReceived, out keys, out values);
      this._serverListRequest = this._serverFilters == null ? Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestInternetServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), (string[]) null, (string[]) null, 0U, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam)) : Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestInternetServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), keys, values, (uint) this._serverFilters.Count, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam));
      return this._serverList;
    }

    public Servers RequestLANServerList(OnServerReceived onServerReceived, OnServerListReceived onServerListReceived)
    {
      string[] keys;
      string[] values;
      this.PrepServerListRequest((Dictionary<string, string>) null, onServerReceived, onServerListReceived, out keys, out values);
      this._serverListRequest = Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestLANServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam));
      return this._serverList;
    }

    public Servers RequestSpecatorServerList(Dictionary<string, string> filters, OnServerReceived onServerReceived, OnServerListReceived onServerListReceived)
    {
      string[] keys;
      string[] values;
      this.PrepServerListRequest(filters, onServerReceived, onServerListReceived, out keys, out values);
      this._serverListRequest = this._serverFilters == null ? Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestSpectatorServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), (string[]) null, (string[]) null, 0U, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam)) : Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestSpectatorServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), keys, values, (uint) this._serverFilters.Count, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam));
      return this._serverList;
    }

    public Servers RequestHistoryServerList(Dictionary<string, string> filters, OnServerReceived onServerReceived, OnServerListReceived onServerListReceived)
    {
      string[] keys;
      string[] values;
      this.PrepServerListRequest(filters, onServerReceived, onServerListReceived, out keys, out values);
      this._serverListRequest = this._serverFilters == null ? Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestHistoryServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), (string[]) null, (string[]) null, 0U, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam)) : Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestHistoryServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), keys, values, (uint) this._serverFilters.Count, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam));
      return this._serverList;
    }

    public Servers RequestFavoriteServerList(Dictionary<string, string> filters, OnServerReceived onServerReceived, OnServerListReceived onServerListReceived)
    {
      string[] keys;
      string[] values;
      this.PrepServerListRequest(filters, onServerReceived, onServerListReceived, out keys, out values);
      this._serverListRequest = this._serverFilters == null ? Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestFavoriteServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), (string[]) null, (string[]) null, 0U, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam)) : Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestFavoriteServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), keys, values, (uint) this._serverFilters.Count, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam));
      return this._serverList;
    }

    public Servers RequestFriendServerList(Dictionary<string, string> filters, OnServerReceived onServerReceived, OnServerListReceived onServerListReceived)
    {
      string[] keys;
      string[] values;
      this.PrepServerListRequest(filters, onServerReceived, onServerListReceived, out keys, out values);
      this._serverListRequest = this._serverFilters == null ? Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestFriendServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), (string[]) null, (string[]) null, 0U, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam)) : Matchmaking.SteamUnityAPI_SteamMatchmakingServers_RequestFriendServerList(this._matchmakingServers, Matchmaking.SteamUnityAPI_SteamUtils_GetAppID(), keys, values, (uint) this._serverFilters.Count, Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerReceivedFromSteam), Marshal.GetFunctionPointerForDelegate((Delegate) this._onServerListReceivedFromSteam));
      return this._serverList;
    }

    private int StrLen(char[] buffer)
    {
      for (int index = 0; index < buffer.Length; ++index)
      {
        if ((int) buffer[index] == 0)
          return index;
      }
      return buffer.Length;
    }

    private string CharArrayToString(char[] buffer)
    {
      if (buffer == null)
        return string.Empty;
      else
        return new string(buffer, 0, this.StrLen(buffer));
    }

    private void OnServerReceived(uint request, ref gameserveritem_t callbackData)
    {
      if ((int) request != (int) this._serverListRequest)
        return;
      uint num = callbackData.m_NetAdr.m_unIP;
      IPAddress ip = new IPAddress(new byte[4]
      {
        (byte) (num >> 24),
        (byte) (num >> 16),
        (byte) (num >> 8),
        (byte) num
      });
      Server server = new Server(callbackData.m_nServerVersion, ip, callbackData.m_NetAdr.m_usConnectionPort, callbackData.m_NetAdr.m_usQueryPort, callbackData.m_nPing, this.CharArrayToString(callbackData.m_szServerName), this.CharArrayToString(callbackData.m_szMap), this.CharArrayToString(callbackData.m_szGameDescription), (int) callbackData.m_bSecure != 0, (int) callbackData.m_bPassword != 0, callbackData.m_nPlayers, callbackData.m_nMaxPlayers, callbackData.m_nBotPlayers, this.CharArrayToString(callbackData.m_szGameTags), this.CharArrayToString(callbackData.m_szGameDir), callbackData.m_nAppID);
      this._serverList.Add(server);
      if (this._onServerReceived == null)
        return;
      this._onServerReceived(this._serverList, server);
    }

    private void OnServerListComplete(uint request)
    {
      if ((int) request != (int) this._serverListRequest)
        return;
      this._serverListRequest = 0U;
      if (this._onServerListReceived == null)
        return;
      this._onServerListReceived(this._serverList);
    }

    public void CancelCurrentServerListRequest()
    {
      if ((int) this._serverListRequest == 0)
        return;
      Matchmaking.SteamUnityAPI_SteamMatchmakingServers_ReleaseRequest(this._matchmaking, this._serverListRequest);
      this._serverListRequest = 0U;
    }
  }
}
