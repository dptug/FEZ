// Type: CommunityExpressNS.GameServer
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class GameServer
  {
    private List<SteamID> _playersPendingAuth = new List<SteamID>();
    private List<SteamID> _playersConnected = new List<SteamID>();
    private List<SteamID> _botsConnected = new List<SteamID>();
    private IntPtr _gameServer;
    private bool _isInitialized;
    private bool _vacSecured;
    private bool _isDedicated;
    private string _serverName;
    private string _spectatorServerName;
    private ushort _spectatorPort;
    private string _regionName;
    private string _gameName;
    private string _gameDescription;
    private ushort _maxClients;
    private bool _isPassworded;
    private string _mapName;
    private Dictionary<string, string> _keyValues;
    private string _gameTags;
    private string _gameData;
    private string _modDir;
    private OnGameServerClientApprovedBySteam _internalOnGameServerClientApproved;
    private OnGameServerClientApproved _onGameServerClientApproved;
    private OnGameServerClientDeniedBySteam _internalOnGameServerClientDenied;
    private OnGameServerClientDenied _onGameServerClientDenied;
    private OnGameServerClientKickFromSteam _internalOnGameServerClientKick;
    private OnGameServerClientKick _onGameServerClientKick;
    private OnGameServerPolicyResponseFromSteam _internalOnGameServerPolicyResponse;

    public bool IsInitialized
    {
      get
      {
        return this._isInitialized;
      }
    }

    public IPAddress PublicIP
    {
      get
      {
        uint publicIp = GameServer.SteamUnityAPI_SteamGameServer_GetPublicIP(this._gameServer);
        return new IPAddress(new byte[4]
        {
          (byte) (publicIp >> 24),
          (byte) (publicIp >> 16),
          (byte) (publicIp >> 8),
          (byte) publicIp
        });
      }
    }

    public bool IsVacSecured
    {
      get
      {
        return this._vacSecured;
      }
    }

    public SteamID SteamID
    {
      get
      {
        return new SteamID(GameServer.SteamUnityAPI_SteamGameServer_GetSteamID(this._gameServer));
      }
    }

    public bool IsDedicated
    {
      get
      {
        return this._isDedicated;
      }
      set
      {
        this._isDedicated = value;
        this.SendBasicServerStatus();
      }
    }

    public string ServerName
    {
      get
      {
        return this._serverName;
      }
      set
      {
        this._serverName = value;
        this.SendUpdatedServerStatus();
      }
    }

    public string SpectatorServerName
    {
      get
      {
        return this._spectatorServerName;
      }
      set
      {
        this._spectatorServerName = value;
        this.SendUpdatedServerStatus();
      }
    }

    public string RegionName
    {
      get
      {
        return this._regionName;
      }
      set
      {
        this._regionName = value;
        this.SendUpdatedServerStatus();
      }
    }

    public string GameName
    {
      get
      {
        return this._gameName;
      }
      set
      {
        this._gameName = value;
        this.SendBasicServerStatus();
      }
    }

    public string GameDescription
    {
      get
      {
        return this._gameDescription;
      }
      set
      {
        this._gameDescription = value;
        this.SendBasicServerStatus();
      }
    }

    public ushort MaxClients
    {
      get
      {
        return this._maxClients;
      }
      set
      {
        this._maxClients = value;
        this.SendBasicServerStatus();
      }
    }

    public bool IsPassworded
    {
      get
      {
        return this._isPassworded;
      }
      set
      {
        this._isPassworded = value;
        this.SendBasicServerStatus();
      }
    }

    public string MapName
    {
      get
      {
        return this._mapName;
      }
      set
      {
        this._mapName = value;
        this.SendUpdatedServerStatus();
      }
    }

    public Dictionary<string, string> KeyValues
    {
      get
      {
        return this._keyValues;
      }
      set
      {
        this._keyValues = value;
        string[] strArray1 = new string[this._keyValues.Keys.Count];
        string[] strArray2 = new string[this._keyValues.Values.Count];
        this._keyValues.Keys.CopyTo(strArray1, 0);
        this._keyValues.Values.CopyTo(strArray2, 0);
        GameServer.SteamUnityAPI_SteamGameServer_SetKeyValues(this._gameServer, strArray1, strArray2, this._keyValues.Count);
      }
    }

    public string GameTags
    {
      get
      {
        return this._gameTags;
      }
      set
      {
        this._gameTags = value;
        GameServer.SteamUnityAPI_SteamGameServer_SetGameTags(this._gameServer, this._gameTags);
      }
    }

    public string GameData
    {
      get
      {
        return this._gameData;
      }
      set
      {
        this._gameData = value;
        GameServer.SteamUnityAPI_SteamGameServer_SetGameTags(this._gameServer, this._gameData);
      }
    }

    internal GameServer()
    {
      this._gameServer = GameServer.SteamUnityAPI_SteamGameServer();
    }

    ~GameServer()
    {
      this.Shutdown();
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamGameServer();

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServer_Init(uint ip, ushort masterServerPort, ushort port, ushort queryPort, EServerMode serverMode, [MarshalAs(UnmanagedType.LPStr)] string gameVersion);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_SetCallbacks(IntPtr OnGameServerClientApproved, IntPtr OnGameServerClientDeny, IntPtr OnGameServerClientKick, IntPtr OnGameServerPolicyResponse);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamGameServer_GetSteamID(IntPtr gameserver);

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamGameServer_GetPublicIP(IntPtr gameserver);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_SetBasicServerData(IntPtr gameserver, bool isDedicated, [MarshalAs(UnmanagedType.LPStr)] string gameName, [MarshalAs(UnmanagedType.LPStr)] string gameDescription, [MarshalAs(UnmanagedType.LPStr)] string modDir);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_LogOnAnonymous(IntPtr gameserver);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_UpdateServerStatus(IntPtr gameserver, int maxClients, int bots, [MarshalAs(UnmanagedType.LPStr)] string serverName, [MarshalAs(UnmanagedType.LPStr)] string spectatorServerName, ushort spectatorPort, [MarshalAs(UnmanagedType.LPStr)] string regionName, [MarshalAs(UnmanagedType.LPStr)] string mapName, bool passworded);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServer_SendUserConnectAndAuthenticate(IntPtr gameserver, uint ipClient, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] authTicket, uint authTicketSize, out ulong steamIDClient);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamGameServer_CreateUnauthenticatedUserConnection(IntPtr gameserver);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_SendUserDisconnect(IntPtr gameserver, ulong steamIDClient);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamGameServer_UpdateUserData(IntPtr gameserver, ulong steamID, [MarshalAs(UnmanagedType.LPStr)] string name, uint score);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_SetKeyValues(IntPtr gameserver, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] keys, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] values, int count);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_SetGameTags(IntPtr gameserver, [MarshalAs(UnmanagedType.LPStr)] string tags);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_SetGameData(IntPtr gameserver, [MarshalAs(UnmanagedType.LPStr)] string data);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_Shutdown();

    public bool Init(bool isDedicated, IPAddress ip, ushort port, ushort queryPort, ushort masterServerPort, ushort spectatorPort, EServerMode serverMode, string serverName, string spectatorServerName, string regionName, string gameName, string gameDescription, string gameVersion, string mapName, ushort maxClients, bool isPassworded, OnGameServerClientApproved onGameServerClientApproved, OnGameServerClientDenied onGameServerClientDenied, OnGameServerClientKick onGameServerClientKick)
    {
      return this.Init(isDedicated, ip, port, queryPort, masterServerPort, spectatorPort, serverMode, serverName, spectatorServerName, regionName, gameName, gameDescription, gameVersion, mapName, maxClients, isPassworded, string.Empty, onGameServerClientApproved, onGameServerClientDenied, onGameServerClientKick);
    }

    public bool Init(bool isDedicated, IPAddress ip, ushort port, ushort queryPort, ushort masterServerPort, ushort spectatorPort, EServerMode serverMode, string serverName, string spectatorServerName, string regionName, string gameName, string gameDescription, string gameVersion, string mapName, ushort maxClients, bool isPassworded, string modDir, OnGameServerClientApproved onGameServerClientApproved, OnGameServerClientDenied onGameServerClientDenied, OnGameServerClientKick onGameServerClientKick)
    {
      byte[] addressBytes = ip.GetAddressBytes();
      uint ip1 = (uint) ((int) addressBytes[0] << 24 | (int) addressBytes[1] << 16 | (int) addressBytes[2] << 8) | (uint) addressBytes[3];
      this._playersPendingAuth.Clear();
      this._playersConnected.Clear();
      this._botsConnected.Clear();
      this._onGameServerClientApproved = onGameServerClientApproved;
      this._onGameServerClientDenied = onGameServerClientDenied;
      this._onGameServerClientKick = onGameServerClientKick;
      if (this._internalOnGameServerClientApproved == null)
      {
        this._internalOnGameServerClientApproved = new OnGameServerClientApprovedBySteam(this.OnGameServerClientApprovedCallback);
        this._internalOnGameServerClientDenied = new OnGameServerClientDeniedBySteam(this.OnGameServerClientDeniedCallback);
        this._internalOnGameServerClientKick = new OnGameServerClientKickFromSteam(this.OnGameServerClientKickCallback);
        this._internalOnGameServerPolicyResponse = new OnGameServerPolicyResponseFromSteam(this.OnGameServerPolicyResponseCallback);
      }
      GameServer.SteamUnityAPI_SteamGameServer_SetCallbacks(Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnGameServerClientApproved), Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnGameServerClientDenied), Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnGameServerClientKick), Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnGameServerPolicyResponse));
      if (!GameServer.SteamUnityAPI_SteamGameServer_Init(ip1, masterServerPort, port, queryPort, serverMode, gameVersion))
        return false;
      this._isInitialized = true;
      this._gameServer = GameServer.SteamUnityAPI_SteamGameServer();
      this._isDedicated = isDedicated;
      this._serverName = serverName;
      this._spectatorServerName = spectatorServerName;
      this._spectatorPort = spectatorPort;
      this._mapName = mapName;
      this._regionName = regionName;
      this._gameName = gameName;
      this._gameDescription = gameDescription;
      this._maxClients = maxClients;
      this._isPassworded = isPassworded;
      this._modDir = modDir;
      this.SendBasicServerStatus();
      GameServer.SteamUnityAPI_SteamGameServer_LogOnAnonymous(this._gameServer);
      this.SendUpdatedServerStatus();
      return true;
    }

    public bool ClientConnected(IPAddress ipClient, byte[] authTicket, out SteamID steamIDClient)
    {
      byte[] addressBytes = ipClient.GetAddressBytes();
      ulong steamIDClient1;
      if (GameServer.SteamUnityAPI_SteamGameServer_SendUserConnectAndAuthenticate(this._gameServer, (uint) ((int) addressBytes[0] << 24 | (int) addressBytes[1] << 16 | (int) addressBytes[2] << 8) | (uint) addressBytes[3], authTicket, (uint) authTicket.Length, out steamIDClient1))
      {
        steamIDClient = new SteamID(steamIDClient1);
        this._playersPendingAuth.Add(steamIDClient);
        this.SendUpdatedServerStatus();
        return true;
      }
      else
      {
        this._onGameServerClientDenied((SteamID) null, EDenyReason.EDenyGeneric, "SteamGameServer::SendUserConnectAndAuthenticate failed");
        steamIDClient = (SteamID) null;
        return false;
      }
    }

    public SteamID AddBot()
    {
      SteamID steamId = new SteamID(GameServer.SteamUnityAPI_SteamGameServer_CreateUnauthenticatedUserConnection(this._gameServer));
      this._botsConnected.Add(steamId);
      return steamId;
    }

    public bool UpdateUserDetails(SteamID steamID, string displayableName, uint score)
    {
      return GameServer.SteamUnityAPI_SteamGameServer_UpdateUserData(this._gameServer, steamID.ToUInt64(), displayableName, score);
    }

    public void ClientDisconnected(SteamID steamIDClient)
    {
      bool flag = false;
      foreach (SteamID steamId in this._playersPendingAuth)
      {
        if (steamId == steamIDClient)
        {
          GameServer.SteamUnityAPI_SteamGameServer_SendUserDisconnect(this._gameServer, steamIDClient.ToUInt64());
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        foreach (SteamID steamId in this._playersConnected)
        {
          if (steamId == steamIDClient)
          {
            GameServer.SteamUnityAPI_SteamGameServer_SendUserDisconnect(this._gameServer, steamIDClient.ToUInt64());
            flag = true;
            break;
          }
        }
      }
      if (!flag)
      {
        foreach (SteamID steamId in this._botsConnected)
        {
          if (steamId == steamIDClient)
          {
            GameServer.SteamUnityAPI_SteamGameServer_SendUserDisconnect(this._gameServer, steamIDClient.ToUInt64());
            break;
          }
        }
      }
      this._playersPendingAuth.Remove(steamIDClient);
      this._playersConnected.Remove(steamIDClient);
      this._botsConnected.Remove(steamIDClient);
    }

    private void OnGameServerClientApprovedCallback(ref GSClientApprove_t callbackData)
    {
      foreach (SteamID approvedPlayer in this._playersPendingAuth)
      {
        if (approvedPlayer == callbackData.m_SteamID)
        {
          this._onGameServerClientApproved(approvedPlayer);
          this._playersConnected.Add(approvedPlayer);
          this._playersPendingAuth.Remove(approvedPlayer);
          break;
        }
      }
    }

    private void OnGameServerClientDeniedCallback(ref GSClientDeny_t callbackData)
    {
      foreach (SteamID deniedPlayer in this._playersPendingAuth)
      {
        if (deniedPlayer == callbackData.m_SteamID)
        {
          this._onGameServerClientDenied(deniedPlayer, callbackData.m_eDenyReason, new string(callbackData.m_rgchOptionalText));
          this._playersPendingAuth.Remove(deniedPlayer);
          this._playersConnected.Remove(deniedPlayer);
          break;
        }
      }
    }

    private void OnGameServerClientKickCallback(ref GSClientKick_t callbackData)
    {
      bool flag = false;
      foreach (SteamID playerToKick in this._playersPendingAuth)
      {
        if (playerToKick == callbackData.m_SteamID)
        {
          this._onGameServerClientKick(playerToKick, callbackData.m_eDenyReason);
          this._playersPendingAuth.Remove(playerToKick);
          flag = true;
          break;
        }
      }
      foreach (SteamID playerToKick in this._playersConnected)
      {
        if (playerToKick == callbackData.m_SteamID)
        {
          if (!flag)
            this._onGameServerClientKick(playerToKick, callbackData.m_eDenyReason);
          this._playersConnected.Remove(playerToKick);
          break;
        }
      }
    }

    public ICollection<Friend> GetPlayersConnected()
    {
      List<Friend> list = new List<Friend>();
      foreach (SteamID id in this._playersConnected)
        list.Add(new Friend(CommunityExpress.Instance.Friends, id));
      return (ICollection<Friend>) list;
    }

    private void OnGameServerPolicyResponseCallback(ref GSPolicyResponse_t callbackData)
    {
      this._vacSecured = (int) callbackData.m_bSecure != 0;
    }

    private void SendBasicServerStatus()
    {
      GameServer.SteamUnityAPI_SteamGameServer_SetBasicServerData(this._gameServer, this._isDedicated, this._gameName, this._gameDescription, this._modDir);
    }

    private void SendUpdatedServerStatus()
    {
      GameServer.SteamUnityAPI_SteamGameServer_UpdateServerStatus(this._gameServer, (int) this._maxClients, this._botsConnected.Count, this._serverName, this._spectatorServerName, this._spectatorPort, this._regionName, this._mapName, this._isPassworded);
    }

    public void RequestUserStats(SteamID steamID, OnUserStatsReceived onUserStatsReceived, IEnumerable<string> requestedStats)
    {
      Stats stats = new Stats();
      stats.Init(steamID, true);
      stats.RequestCurrentStats(onUserStatsReceived, requestedStats);
    }

    public void RequestUserAchievements(SteamID steamID, OnUserStatsReceived onUserStatsReceived, IEnumerable<string> requestedAchievements)
    {
      Achievements achievements = new Achievements();
      achievements.Init(steamID, true);
      achievements.RequestCurrentAchievements(onUserStatsReceived, requestedAchievements);
    }

    public void Shutdown()
    {
      GameServer.SteamUnityAPI_SteamGameServer_Shutdown();
    }
  }
}
